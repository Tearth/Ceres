using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CHIP8
{
    public class Display
    {
        bool[] display = new bool[64 * 32];

        public Display()
        {

        }

        public void Init()
        {
            for (int i = 0; i < display.Length; i++)
            {
                display[i] = false;
            }
        }

        public void Clear()
        {
            Console.BackgroundColor = ConsoleColor.Black;

            for (int i = 0; i < display.Length; i++)
            {
                display[i] = false;
                var x = GetX((ushort)i);
                var y = GetY((ushort)i);

                Console.SetCursorPosition(x, y);
                Console.Write(" ");
            }
        }

        public bool GetPixel(ushort position)
        {
            return display[position];
        }

        public void SetPixel(ushort position)
        {
            var x = GetX(position);
            var y = GetY(position);

            var flag = display[position] ^ true;
            if (flag)
                Console.BackgroundColor = ConsoleColor.White;
            else
                Console.BackgroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(x, y);
            Console.Write(" ");

            display[position] = flag;
        }

        ushort GetX(ushort position)
        {
            return (ushort)(position % 64);
        }

        ushort GetY(ushort position)
        {
            return (ushort)(position / 64);
        }
    }
}
