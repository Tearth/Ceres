using System;

namespace CHIP8
{
    internal class Program
    {
        private static void Main()
        {
            Console.SetWindowSize(64, 32);
            Console.WriteLine("Ceres: CHIP-8 Emulator");
            Console.WriteLine();
            Console.Write("File: ");

            var fileName = Console.ReadLine();
            Console.Clear();
            
            var emulator = new Emulator.Emulator();
            emulator.Init();
            if (emulator.Load(fileName))
                emulator.Run();
            else
                Console.WriteLine("File not found");

            Console.Read();
        }
    }
}
