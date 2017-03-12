using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP8
{
    public class Stack
    {
        ushort[] stack = new ushort[16];
        byte stackPointer;

        public void Init()
        {
            for(int i=0; i<stack.Length; i++)
            {
                stack[i] = 0;
            }

            stackPointer = 0;
        }

        public void Push(ushort value)
        {
            stack[stackPointer] = value;
            stackPointer++;
        }

        public ushort Pop()
        {
            stackPointer--;
            return stack[stackPointer];
        }
    }
}
