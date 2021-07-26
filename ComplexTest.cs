using ComplexLib;
using System;

namespace Complex_Test15_6
{
    class ComplexTest
    {
        static void Main()
        {
            Complex complex = 1 - Complex.imaginaryOne;
            Console.WriteLine($"{complex:f5}");
            complex.WriteBin("c");
            Console.ReadLine();

        }
    }
}
