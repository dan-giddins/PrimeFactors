using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace PrimeFactors
{
	internal class Program
	{
		static private System.Diagnostics.Stopwatch Watch;
		private const long Range = 100;

		static private void Main()
		{
			//Console.WriteLine($"For a range of {Range}:");
			var (timeCurrent, primeDictCurrent) = CurrentPrimes();
			var (timeNew, primeDictNew) = NewPrimes();
			WritePrimesTableCSV(primeDictNew);
			PrintTimeDiff(timeCurrent, timeNew);
		}

		static private void WritePrimesTableCSV(Dictionary<long, Dictionary<long, long>> primeDict)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Primes");
				var headingCol = 1;
				worksheet.Cells[1, headingCol].Value = "Number";
				worksheet.Cells[1, headingCol].Style.Font.Bold = true;
				foreach (var factor in primeDict[1].Reverse())
				{
					headingCol++;
					worksheet.Cells[1, headingCol].Value = factor.Key;
					worksheet.Cells[1, headingCol].Style.Font.Bold = true;
				}
				var row = 1;
				foreach (var n in primeDict)
				{
					row++;
					var bodyCol = 1;
					worksheet.Cells[row, bodyCol].Value = n.Key;
					foreach (var factor in n.Value.Reverse())
					{
						bodyCol++;
						worksheet.Cells[row, bodyCol].Value = factor.Value;
					}
				}
				package.SaveAs(new FileInfo("primes.xlsx"));
			}
		}

		static private void PrintTimeDiff(long timeCurrent, long timeNew)
		{
			if (timeNew - timeCurrent > 0)
			{
				Console.WriteLine($"The new method is {timeNew - timeCurrent} ms slower");
			}
			else if (timeNew - timeCurrent < 0)
			{
				Console.WriteLine($"The new method is {timeCurrent - timeNew} ms faster!!!");
				Console.WriteLine("You should probably celebrate!");
			}
		}

		static private (long, Dictionary<long, Dictionary<long, long>>) NewPrimes()
		{
			// new method
			Watch = System.Diagnostics.Stopwatch.StartNew();
			// number, prime, power
			var primeDictNew = new Dictionary<long, Dictionary<long, long>>();
			for (var n = 1; n < Range; n++)
			{
				primeDictNew.Add(n, new Dictionary<long, long>());
			}
			// loop through potential bases (all numbers > 1)
			for (var @base = 2; @base < Range; @base++)
			{
				// check if base is actully not prime
				var currentPrimePowersProduct = primeDictNew[@base].Aggregate(
					1, (long acc, KeyValuePair<long, long> val) =>
						acc * IntPower(val.Key, val.Value));
				if (currentPrimePowersProduct == @base)
				{
					// the current prime product is already finished for this number
					// therefor current base cannot be part of this prime product
					// therefor current base also cannot be prime itself
					// (by the time we get to testing a non-prime number
					// all the factors of that number should have already been discovered
					// so this will exit on the first time seeing this non-prime)
					continue;
				}
				// loop through the possible powers for said @base
				for (var pow = 0; IntPower(@base, pow) < Range; pow++)
				{
					// sequence extender thingy (based on @base)
					// this is some kind of dark magic that I do not understand
					for (var extender = 0; extender < @base - 1; extender++)
					{
						// sequence for this @base and power
						// how and why is `+ extender * Power(@base, pow)` needed here?!
						for (var n = IntPower(@base, pow) + extender * IntPower(@base, pow);
							n < Range;
							n += IntPower(@base, pow + 1))
						{
							primeDictNew[n].Add(@base, pow);
						}
					}
				}
			}
			Watch.Stop();
			//PrintPrimeDict(primeDictNew);
			var timeNew = Watch.ElapsedMilliseconds;
			Console.WriteLine($"The new method took {timeNew} ms");
			return (timeNew, primeDictNew);
		}

		static private (long, Dictionary<long, Dictionary<long, long>>) CurrentPrimes()
		{
			// current method
			Watch = System.Diagnostics.Stopwatch.StartNew();
			// number, prime, power
			var primeDictCurrent = new Dictionary<long, Dictionary<long, long>>();
			for (var n = 2; n < Range; n++)
			{
				var primeFactors = new List<long>();
				var p = 2;
				var m = n;
				while (m >= p * p)
				{
					// if p is not prime there will be some p' where:
					// p' is prime,
					// p' < p,
					// and p % p' == 0 (i.e p' is a prime factor of p)
					// as p' < p, this p' will have already been factorised out of m
					// therefor m cannot contain p as a factor
					if (m % p == 0)
					{
						// p is a factor
						primeFactors.Add(p);
						// set m to be the remaing part of the number that needs factorising
						m /= p;
					}
					else
					{
						// try dividing m by a number one greater
						p++;
					}
				}
				primeFactors.Add(m);
				// group, count and project the list of primes for this into a dictory of <key = base, power>
				primeDictCurrent.Add(n, primeFactors.GroupBy(x => x)
					.Select(x => new KeyValuePair<long, long>(x.Key, x.Count()))
					.ToDictionary(x => x.Key, x => x.Value));
			}
			Watch.Stop();
			//PrintPrimeDict(primeDictCurrent);

			var timeCurrent = Watch.ElapsedMilliseconds;
			Console.WriteLine($"The current method took {timeCurrent} ms");
			return (timeCurrent, primeDictCurrent);
		}

		static private void PrintPrimeDict(Dictionary<long, Dictionary<long, long>> primeDict)
		{
			for (var n = 1; n < Range; n++)
			{
				Console.WriteLine($"{n} = {string.Join(" * ", primeDict[n].Select(x => $"{x.Key}^{x.Value}"))}");
			}
		}

		static private long IntPower(long x, long n)
		{
			var result = 1L;
			while (n > 0)
			{
				if ((n & 1) == 0)
				{
					x *= x;
					n >>= 1;
				}
				else
				{
					result *= x;
					--n;
				}
			}
			return result;
		}
	}
}
