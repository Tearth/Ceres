﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CHIP8.Emulator
{
    public class Keyboard
    {
        /*
           1	2	3	C
           4	5	6	D
           7	8	9	E
           A	0	B	F
       */
        private readonly Dictionary<byte, ConsoleKey> _keysBinding = new Dictionary<byte, ConsoleKey>
        {
            { 0x0, ConsoleKey.X },
            { 0x1, ConsoleKey.D1 },
            { 0x2, ConsoleKey.D2 },
            { 0x3, ConsoleKey.D3 },
            { 0x4, ConsoleKey.Q },
            { 0x5, ConsoleKey.W },
            { 0x6, ConsoleKey.E },
            { 0x7, ConsoleKey.A },
            { 0x8, ConsoleKey.S },
            { 0x9, ConsoleKey.D },
            { 0xA, ConsoleKey.Z },
            { 0xB, ConsoleKey.C },
            { 0xC, ConsoleKey.D4 },
            { 0xD, ConsoleKey.R },
            { 0xE, ConsoleKey.F },
            { 0xF, ConsoleKey.V }
        };

        [DllImport("user32.dll")]
        public static extern ushort GetKeyState(short nVirtKey);

        public void Init()
        {

        }

        public bool IsKeyPressed(byte keyCode)
        {
            var key = _keysBinding[keyCode];
            return (GetKeyState((short)key) & 0x80) == 0x80;
        }

        public byte ReadKey()
        {
            var keyInfo = Console.ReadKey();
            return _keysBinding.First(p => p.Value == keyInfo.Key).Key;
        }
    }
}
