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
			//for (var n = 2; n < 100; n += 4)
			//{
			//	primeDictFast[n].Add(2, 1);
			//}
			//for (var n = 4; n < 100; n += 8)
			//{
			//	primeDictFast[n].Add(2, 2);
			//}
			//for (var n = 8; n < 100; n += 16)
			//{
			//	primeDictFast[n].Add(2, 3);

			//for (var n = 16; n < 100; n += 32)
			//{
			//	primeDictFast[n].Add(2, 3);
			//}
			for (int p = 1; p < Math.Log(100, 2); p++)
			{
				for (var n = (int)Math.Pow(2, p); n < 100; n += (int)Math.Pow(2, p + 1))
				{
					primeDictFast[n].Add(2, p);
				}
			}
			for (var n = 2; n < 100; n++)
			{
				Console.WriteLine($"{n} = {string.Join(" * ", primeDictFast[n].Select(x => $"{x.Key}^{x.Value}"))}");
			}
		}
	}
}
