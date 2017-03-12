using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP8
{
    public class Memory
    {
        byte[] memory = new byte[4096];
        byte[] fontSet = new byte[80] { 0xF0, 0x90, 0x90, 0x90, 0xF0,
                                        0x20, 0x60, 0x20, 0x20, 0x70, 
                                        0xF0, 0x10, 0xF0, 0x80, 0xF0, 
                                        0xF0, 0x10, 0xF0, 0x10, 0xF0, 
                                        0x90, 0x90, 0xF0, 0x10, 0x10, 
                                        0xF0, 0x80, 0xF0, 0x10, 0xF0, 
                                        0xF0, 0x80, 0xF0, 0x90, 0xF0, 
                                        0xF0, 0x10, 0x20, 0x40, 0x40, 
                                        0xF0, 0x90, 0xF0, 0x90, 0xF0, 
                                        0xF0, 0x90, 0xF0, 0x10, 0xF0, 
                                        0xF0, 0x90, 0xF0, 0x90, 0x90,
                                        0xE0, 0x90, 0xE0, 0x90, 0xE0,
                                        0xF0, 0x80, 0x80, 0x80, 0xF0,
                                        0xE0, 0x90, 0x90, 0x90, 0xE0,
                                        0xF0, 0x80, 0xF0, 0x80, 0xF0,
                                        0xF0, 0x80, 0xF0, 0x80, 0x80 };

        public void Init()
        {
            for (int i = 0; i < memory.Length; i++)
            {
                memory[i] = 0;
            }

            for (int i = 0; i < fontSet.Length; i++)
            {
                memory[i] = fontSet[i];
            }
        }

        public byte Read(ushort address)
        {
            return memory[address];
        }

        public void Write(ushort address, byte value)
        {
            memory[address] = value;
        }
    }
}
