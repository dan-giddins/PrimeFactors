using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeFactors
{
	class Program
	{
		static void Main()
		{
			// number, prime, power
			var primeDict = new Dictionary<int, IEnumerable<KeyValuePair<int, int>>>();
			for (var n = 2; n < 100; n++)
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
				primeDict.Add(n, primeFactors.GroupBy(x => x)
					.Select(x => new KeyValuePair<int, int>(x.Key, x.Count())));
				Console.WriteLine($"{n} = {string.Join(" * ", primeDict[n].Select(x => $"{x.Key}^{x.Value}"))}");
			}
		}
	}
}
