using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(64, 32);
            Console.WriteLine("Ceres: CHIP-8 Emulator");
            Console.WriteLine("Version 1.0");
            Console.WriteLine();
            Console.Write("File: ");

            var fileName = Console.ReadLine();
            Console.Clear();
            
            var emulator = new Emulator();
            emulator.Init();
            if (emulator.Load(fileName))
                emulator.Run();
            else
                Console.WriteLine("File not found");

            Console.Read();
        }
    }
}
