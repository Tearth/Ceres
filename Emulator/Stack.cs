namespace CHIP8.Emulator
{
    public class Stack
    {
        private byte _stackPointer;
        private readonly ushort[] _stack = new ushort[16];

        public void Init()
        {

        }

        public void Push(ushort value)
        {
            _stack[_stackPointer++] = value;
        }

        public ushort Pop()
        {
            return _stack[--_stackPointer];
        }
    }
}
