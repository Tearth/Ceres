using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP8
{
    public class Registers
    {
        byte[] registers = new byte[16];
        ushort addressRegister;

        public Registers()
        {

        }

        public void Init()
        {
            for (int i = 0; i < registers.Length; i++)
            {
                registers[i] = 0;
            }

            addressRegister = 0;
        }

        public byte GetRegister(byte id)
        {
            return registers[id];
        }

        public void SetRegister(byte id, byte value)
        {
            registers[id] = value;
        }

        public ushort GetaddressRegsiter()
        {
            return addressRegister;
        }

        public void SetaddressRegister(ushort value)
        {
            addressRegister = value;
        }
    }
}
