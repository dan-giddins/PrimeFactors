using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeFactors
{
	internal class Program
	{
		static private System.Diagnostics.Stopwatch Watch;
		private const long Range = 1000;

		static private void Main()
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
			PrintPrimeDict(primeDictCurrent);
			Console.WriteLine($"For a range of {Range}:");
			var timeCurrent = Watch.ElapsedMilliseconds;
			Console.WriteLine($"The current method took {timeCurrent} ms");

			// new method
			Watch = System.Diagnostics.Stopwatch.StartNew();
			// number, prime, power
			var primeDictNew = new Dictionary<long, Dictionary<long, long>>();
			for (var n = 2; n < Range; n++)
			{
				primeDictNew.Add(n, new Dictionary<long, long>());
			}
			// loop through potential bases (all numbers > 1)
			for (var potBase = 2; potBase < Range; potBase++)
			{
				if (primeDictNew[potBase].Any())
				{
					// if the numeric value of potBase already has factors
					// potBase cannot be prime
					// all primes have only one prime factor of themself
					// as potBase is not prime, it cannot be a base
					continue;
				}
				// loop through the possible powers for said potBase
				for (var pow = 1; IntPower(potBase, pow) < Range; pow++)
				{
					// sequence extender thingy (based on potBase)
					// this is some kind of dark magic that I do not understand
					for (var extender = 0; extender < potBase - 1; extender++)
					{
						// sequence for this potBase and power
						// how and why is `+ extender * Power(potBase, pow)` needed here?!
						for (
							var n = IntPower(potBase, pow) + extender * IntPower(potBase, pow);
							n < Range;
							n += IntPower(potBase, pow + 1))
						{
							// currentPrimePowersProduct < n
							primeDictNew[n].Add(potBase, pow);
						}
					}
				}
			}
			Watch.Stop();
			//PrintPrimeDict(primeDictNew);
			var timeNew = Watch.ElapsedMilliseconds;
			Console.WriteLine($"The new method took {timeNew} ms");

			if (timeNew - timeCurrent > 0)
			{
				Console.WriteLine($"The current method is {timeNew - timeCurrent} ms faster");
			}
			else if (timeNew - timeCurrent < 0)
			{
				Console.WriteLine($"The NEW method is {timeCurrent - timeNew} ms faster!!!");
				Console.WriteLine("You should probably celebrate!");
			}
		}

		static private void PrintPrimeDict(Dictionary<long, Dictionary<long, long>> primeDict)
		{
			for (var n = 2; n < Range; n++)
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
