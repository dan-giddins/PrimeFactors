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
			var primeDictProc = new Dictionary<int, Dictionary<int, int>>();
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
				primeDictProc.Add(n, primeFactors.GroupBy(x => x)
					.Select(x => new KeyValuePair<int, int>(x.Key, x.Count()))
					.ToDictionary(x => x.Key, x => x.Value));
				//Console.WriteLine($"{n} = {string.Join(" * ", primeDictProc[n].Select(x => $"{x.Key}^{x.Value}"))}");
			}
			//var pow2Count = primeDict.SelectMany(x => x.Value.Where(x => x.Key == 2).Select(x => x.Value));
			//foreach (var item in pow2Count)
			//{
			//	Console.Write($"{item} ");
			//}
			//Console.WriteLine();
			//Console.WriteLine();
			//// every other power is 1
			//var pow2CountRemove1 = pow2Count.Where(x => x != 1);
			//foreach (var item in pow2CountRemove1)
			//{
			//	Console.Write($"{item} ");
			//}
			//Console.WriteLine();
			//Console.WriteLine();
			//// every other power from the remaining list is 2
			//var pow2CountRemove2 = pow2CountRemove1.Where(x => x != 2);
			//foreach (var item in pow2CountRemove2)
			//{
			//	Console.Write($"{item} ");
			//}
			//Console.WriteLine();
			// number, prime, power
			var primeDictFast = new Dictionary<int, Dictionary<int, int>>();
			for (var n = 2; n < 100; n++)
			{
				primeDictFast.Add(n, new Dictionary<int, int>());
			}
			//for (int prime = 2; prime < 100; prime++)
			//{
			//	for (int pow = 1; pow < Math.Log(100, prime); pow++)
			//	{
			//		for (var n = (int)Math.Pow(prime, pow); n < 100; n += (int)Math.Pow(prime, pow + 1))
			//		{
			//			primeDictFast[n].Add(prime, pow);
			//		}
			//	}
			//}
			// 2s
			for (var pow = 1; pow < Math.Log(100, 2); pow++)
			{
				// fact - 1 loops
				for (var n = (int)Math.Pow(2, pow); n < 100; n += (int)Math.Pow(2, pow + 1))
				{
					primeDictFast[n].Add(2, pow);
				}
			}
			// 3s
			for (var pow = 1; pow < Math.Log(100, 3); pow++)
			{
				// fact - 1 loops
				for (var n = (int)Math.Pow(3, pow); n < 100; n += (int)Math.Pow(3, pow + 1))
				{
					primeDictFast[n].Add(3, pow);
				}
				for (var n = (int)Math.Pow(3, pow) + (int)Math.Pow(3, pow); n < 100; n += (int)Math.Pow(3, pow + 1))
				{
					primeDictFast[n].Add(3, pow);
				}
			}
			// 5s
			for (var pow = 1; pow < Math.Log(100, 5); pow++)
			{
				// fact - 1 loops
				for (var l = 0; l < 5 - 1; l++)
				{
					for (var n = (int)Math.Pow(5, pow) + l * (int)Math.Pow(5, pow); n < 100; n += (int)Math.Pow(5, pow + 1))
					{
						primeDictFast[n].Add(5, pow);
					}
				}
			}
			for (var n = 2; n < 100; n++)
			{
				Console.WriteLine($"{n} = {string.Join(" * ", primeDictFast[n].Select(x => $"{x.Key}^{x.Value}"))}");
			}
		}
	}
}
