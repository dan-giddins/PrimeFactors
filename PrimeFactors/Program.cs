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
			// loop through potential primes (all numbers > 1)
			for (var potPrime = 2; potPrime < Range; potPrime++)
			{
				// assume number is prime
				var isPrime = true;
				// loop through the possible powers for said potPrime
				for (var pow = 1; isPrime && Power(potPrime, pow) < Range; pow++)
				{
					// sequence extender thingy (based on potPrime)
					// this is some kind of dark magic that I do not understand
					// it does work though
					for (var extender = 0; isPrime && extender < potPrime - 1; extender++)
					{
						// sequence for this potPrime and power
						// why does loop * Power(potPrime, pow) even work?!
						for (
							var n = Power(potPrime, pow) + extender * Power(potPrime, pow);
							isPrime && n < Range;
							n += Power(potPrime, pow + 1))
						{
							// is potPrime actully prime?
							var currentPrimePowersProduct = primeDictNew[n].Aggregate(
								1, (long acc, KeyValuePair<long, long> val) =>
									acc * Power(val.Key, val.Value));
							if (currentPrimePowersProduct < n)
							{
								primeDictNew[n].Add(potPrime, pow);
							}
							else
							{
								// if currentPrimePowersProduct >= n
								// (although I belive this is only hit in the == n state)
								// the current prime product is already finished for this number
								// therefor current number cannot be a prime product
								// therefor cannot be prime itself
								// set isPrime to false to exit out all loops and move to the next potential prime
								// (by the time we get to testing a non-prime number
								// all the factors of that number should have already been discovered
								// so this will exit on the first time seeing this number)
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

			if (timeNew - timeCurrent > 0)
			{
				Console.WriteLine($"The current method is {timeNew - timeCurrent} ms faster");
				Console.WriteLine("Not really surprising tbh");
			}
			else if (timeNew - timeCurrent < 0)
			{
				Console.WriteLine($"The NEW method is {timeCurrent - timeNew} ms faster!!!");
				Console.WriteLine("You should probably celebrate!");
			}
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
