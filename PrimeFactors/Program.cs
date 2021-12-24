using System;

namespace PrimeFactors
{
	class Program
	{
		static void Main(string[] args)
		{
			for (var n = 0; n < 100; n++)
			{
				Console.Write($"{n} = ");
				var p = 2;
				var m = n;
				while (m >= p * p)
				{
					if (m % p == 0)
					{
						Console.Write($"{p} * ");
						m /= p;
					}
					else
					{
						p++;
					}
				}
				Console.WriteLine($"{m}");
			}
		}
	}
}
