namespace CHIP8.Emulator
{
    public class Registers
    {
        private ushort _programCounter;
        private ushort _addressRegister;
        private byte _delayTimerRegister;
        private byte _soundTimerRegister;
        private readonly byte[] _registers = new byte[16];

        public void Init()
        {
            _programCounter = 512;
        }

        public ushort GetProgramCounter()
        {
            return _programCounter;
        }

        public void SetProgramCounter(ushort value)
        {
            _programCounter = value;
        }

        public void IncProgramCounter(ushort value)
        {
            _programCounter += value;
        }

        public ushort GetAddressRegister()
        {
            return _addressRegister;
        }

        public void SetAddressRegister(ushort value)
        {
            _addressRegister = value;
        }

        public byte GetDelayTimerRegister()
        {
            return _delayTimerRegister;
        }

        public void SetDelayTimerRegister(byte value)
        {
            _delayTimerRegister = value;
        }

        public void DecDelayTimerRegister(byte value)
        {
            _delayTimerRegister -= value;
        }

        public byte GetSoundTimerRegister()
        {
            return _soundTimerRegister;
        }

        public void SetSoundTimerRegister(byte value)
        {
            _soundTimerRegister = value;
        }

        public void DecSoundTimerRegister(byte value)
        {
            _soundTimerRegister -= value;
        }

        public byte GetRegister(byte id)
        {
            return _registers[id];
        }

        public void SetRegister(byte id, byte value)
        {
            _registers[id] = value;
        }
    }
}
