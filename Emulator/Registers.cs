namespace CHIP8.Emulator
{
    public class Registers
    {
        readonly byte[] _registers = new byte[16];
        ushort _addressRegister;

        public void Init()
        {
            for (int i = 0; i < _registers.Length; i++)
            {
                _registers[i] = 0;
            }

            _addressRegister = 0;
        }

        public byte GetRegister(byte id)
        {
            return _registers[id];
        }

        public void SetRegister(byte id, byte value)
        {
            _registers[id] = value;
        }

        public ushort GetAddressRegister()
        {
            return _addressRegister;
        }

        public void SetAddressRegister(ushort value)
        {
            _addressRegister = value;
        }
    }
}
