using System;

namespace CHIP8.Emulator
{
    public class Display
    {
        readonly bool[] _display = new bool[64 * 32];

        public void Init()
        {
            for (int i = 0; i < _display.Length; i++)
            {
                _display[i] = false;
            }
        }

        public void Clear()
        {
            Console.BackgroundColor = ConsoleColor.Black;

            for (int i = 0; i < _display.Length; i++)
            {
                _display[i] = false;
                var x = GetX((ushort)i);
                var y = GetY((ushort)i);

                Console.SetCursorPosition(x, y);
                Console.Write(" ");
            }
        }

        public bool GetPixel(ushort position)
        {
            return _display[position];
        }

        public void SetPixel(ushort position)
        {
            var x = GetX(position);
            var y = GetY(position);

            var flag = _display[position] ^ true;
            Console.BackgroundColor = flag ? ConsoleColor.White : ConsoleColor.Black;

            Console.SetCursorPosition(x, y);
            Console.Write(" ");

            _display[position] = flag;
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
