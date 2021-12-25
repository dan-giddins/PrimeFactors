using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeFactors
{
	class Program
	{
		private const int Range = 100000;
		private static System.Diagnostics.Stopwatch Watch;

		static void Main()
		{
			// current method
			Watch = System.Diagnostics.Stopwatch.StartNew();
			// number, prime, power
			var primeDictCurrent = new Dictionary<int, Dictionary<int, int>>();
			for (var n = 2; n < Range; n++)
			{
				var primeFactors = new List<int>();
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
					.Select(x => new KeyValuePair<int, int>(x.Key, x.Count()))
					.ToDictionary(x => x.Key, x => x.Value));
			}
			Watch.Stop();
			//PrintPrimeDict(primeDictCurrent);
			Console.WriteLine($"For a range of {Range}:");
			var timeCurrent = Watch.ElapsedMilliseconds;
			Console.WriteLine($"The current method took {timeCurrent} ms");

			// fast? method
			Watch = System.Diagnostics.Stopwatch.StartNew();
			// number, prime, power
			var primeDictFast = new Dictionary<int, Dictionary<int, int>>();
			for (var n = 2; n < Range; n++)
			{
				primeDictFast.Add(n, new Dictionary<int, int>());
			}
			// loop through potential primes
			for (var potPrime = 2; potPrime < Range; potPrime++)
			{
				var isPrime = true;
				// loop through the possible powers for said potPrime
				for (var pow = 1; isPrime && pow < Math.Log(Range, potPrime); pow++)
				{
					// sequence modifer thing(based on potPrime)
					for (var loop = 0; isPrime && loop < potPrime - 1; loop++)
					{
						// sequence
						for (
							var n = (int)Math.Pow(potPrime, pow) + loop * (int)Math.Pow(potPrime, pow);
							isPrime && n < Range;
							n += (int)Math.Pow(potPrime, pow + 1))
						{
							// is prime?
							if (primeDictFast[n].Aggregate(1, (acc, val) =>
								acc * (int)Math.Pow(val.Key, val.Value)) < n)
							{
								primeDictFast[n].Add(potPrime, pow);
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
			var timeFast = Watch.ElapsedMilliseconds;
			Console.WriteLine($"The fast method took {timeFast} ms");
			Console.WriteLine($"The fast method is {timeCurrent - timeFast} ms faster!");
		}

		private static void PrintPrimeDict(Dictionary<int, Dictionary<int, int>> primeDict)
		{
			for (var n = 2; n < Range; n++)
			{
				Console.WriteLine($"{n} = {string.Join(" * ", primeDict[n].Select(x => $"{x.Key}^{x.Value}"))}");
			}
		}
	}
}
