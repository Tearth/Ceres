namespace CHIP8.Emulator
{
    public class Stack
    {
        readonly ushort[] _stack = new ushort[16];
        byte _stackPointer;

        public void Init()
        {
            for(int i=0; i<_stack.Length; i++)
            {
                _stack[i] = 0;
            }

            _stackPointer = 0;
        }

        public void Push(ushort value)
        {
            _stack[_stackPointer] = value;
            _stackPointer++;
        }

        public ushort Pop()
        {
            _stackPointer--;
            return _stack[_stackPointer];
        }
    }
}
