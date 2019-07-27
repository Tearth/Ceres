namespace CHIP8.Emulator
{
    public class Registers
    {
        private readonly byte[] _registers = new byte[16];
        private ushort _addressRegister;

        public void Init()
        {

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
