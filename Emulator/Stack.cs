namespace CHIP8.Emulator
{
    public class Stack
    {
        private readonly ushort[] _stack = new ushort[16];
        private byte _stackPointer;

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
