using ComplexLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complex_Test15_6
{
    class ComplexTest
    {
        static void Main()
        {
            Complex complex = 1 - Complex.imaginaryOne;
            Console.WriteLine($"{complex:f5}");
            Console.ReadLine();

        }
    }
}
