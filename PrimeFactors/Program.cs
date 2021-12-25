using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeFactors
{
	class Program
	{
		private static System.Diagnostics.Stopwatch Watch;
		private const long Range = 100000;

		static void Main()
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
					if (m % p == 0)
					{
						primeFactors.Add(p);
						m /= p;
					}
					else
					{
						p++;
					}
				}
				primeFactors.Add(m);
				primeDictCurrent.Add(n, primeFactors.GroupBy(x => x)
					.Select(x => new KeyValuePair<long, long>(x.Key, x.Count()))
					.ToDictionary(x => x.Key, x => x.Value));
			}
			Watch.Stop();
			//PrintPrimeDict(primeDictCurrent);
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
			// loop through potential primes
			for (var potPrime = 2; potPrime < Range; potPrime++)
			{
				var isPrime = true;
				// loop through the possible powers for said potPrime
				for (var pow = 1; isPrime && Power(potPrime, pow) < Range; pow++)
				{
					// sequence modifer thing (based on potPrime)
					for (var loop = 0; isPrime && loop < potPrime - 1; loop++)
					{
						// sequence
						for (
							var n = Power(potPrime, pow) + loop * Power(potPrime, pow);
							isPrime && n < Range;
							n += Power(potPrime, pow + 1))
						{
							// is prime?
							if (primeDictNew[n].Aggregate(1, (long acc, KeyValuePair<long, long> val) =>
								acc * Power(val.Key, val.Value)) < n)
							{
								primeDictNew[n].Add(potPrime, pow);
							}
							else
							{
								// current number is not prime
								isPrime = false;
							}
						}
					}
				}
			}
			Watch.Stop();
			//PrintPrimeDict(primeDictFast);
			var timeNew = Watch.ElapsedMilliseconds;
			Console.WriteLine($"The new method took {timeNew} ms");
			Console.WriteLine($"The current method is {timeNew - timeCurrent} ms faster!");
		}

		private static void PrintPrimeDict(Dictionary<long, Dictionary<long, long>> primeDict)
		{
			for (var n = 2; n < Range; n++)
			{
				Console.WriteLine($"{n} = {string.Join(" * ", primeDict[n].Select(x => $"{x.Key}^{x.Value}"))}");
			}
		}
		private static long Power(long x, long n)
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
