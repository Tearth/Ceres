using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CHIP8.Emulator
{
    public class Emulator
    {
        ushort _programCounter;
        byte _delayTimer;
        byte _soundTimer;

        readonly float _timersDeltaTime = 17;
        readonly int _mainLoopDeltaTime = 1;

        readonly Random _random = new Random();
        readonly Display _display = new Display();
        readonly Memory _memory = new Memory();
        readonly Registers _registers = new Registers();
        readonly Keyboard _keyboard = new Keyboard();
        readonly Stack _stack = new Stack();

        readonly Stopwatch _mainWatch = new Stopwatch();
        readonly Stopwatch _timersWatch = new Stopwatch();

        public void Init()
        {
            _display.Init();
            _memory.Init();
            _registers.Init();
            _keyboard.Init();
            _stack.Init();

            _delayTimer = 0;
            _soundTimer = 0;
            _programCounter = 512;
        }

        public bool Load(String appFile)
        {
            if (!File.Exists(appFile))
                return false;

            var app = File.ReadAllBytes(appFile);
            for (int i = 0; i < app.Length; i++)
            {
                _memory.Write((ushort)(512 + i), app[i]);
            }

            return true;
        }

        public void Run()
        {
            _mainWatch.Start();
            _timersWatch.Start();

            while (true)
            {
                var milliseconds = _mainWatch.ElapsedMilliseconds;
                if (milliseconds > _mainLoopDeltaTime)
                {
                    _mainWatch.Restart();

                    var instruction = _memory.Read(_programCounter) << 8 | _memory.Read((ushort)(_programCounter + 1));

                    UpdateTimers();
                    Parse(instruction);

                    _programCounter += 2;
                }
            }
        }

        void UpdateTimers()
        {
            var milliseconds = _timersWatch.ElapsedMilliseconds;
            if (milliseconds > _timersDeltaTime)
            {
                _timersWatch.Restart();

                if (_delayTimer > 0)
                    _delayTimer--;
                else
                    _delayTimer = 0;

                if (_soundTimer > 0)
                {
                    new Thread(Console.Beep).Start();
                    _soundTimer--;
                }
                else
                    _soundTimer = 0;
            }
        }

        void Parse(int instruction)
        {
            switch (instruction & 0xF000)
            {
                case 0x0000:
                    {
                        switch (instruction)
                        {
                            case 0x00E0:
                                {
                                    ClearDisplay();
                                    break;
                                }
                            case 0x00EE:
                                {
                                    ReturnFromSubroutine();
                                    break;
                                }
                        }
                        break;
                    }
                case 0x1000:
                    {
                        Jump(instruction);
                        break;
                    }
                case 0x2000:
                    {
                        Call(instruction);
                        break;
                    }
                case 0x3000:
                    {
                        SkipIfRegisterEqualByte(instruction);
                        break;
                    }
                case 0x4000:
                    {
                        SkipIfRegisterNotEqualByte(instruction);
                        break;
                    }
                case 0x5000:
                    {
                        SkipIfRegisterEqualRegister(instruction);
                        break;
                    }
                case 0x6000:
                    {
                        SetByteToRegister(instruction);
                        break;
                    }
                case 0x7000:
                    {
                        AddByteToRegister(instruction);
                        break;
                    }
                case 0x8000:
                    {
                        switch (instruction & 0xF00F)
                        {
                            case 0x8000:
                                {
                                    SetRegisterToRegister(instruction);
                                    break;
                                }
                            case 0x8001:
                                {
                                    RegisterOrRegister(instruction);
                                    break;
                                }
                            case 0x8002:
                                {
                                    RegisterAndRegister(instruction);
                                    break;
                                }
                            case 0x8003:
                                {
                                    RegisterXorRegister(instruction);
                                    break;
                                }
                            case 0x8004:
                                {
                                    AddRegisterToRegister(instruction);
                                    break;
                                }
                            case 0x8005:
                                {
                                    RegisterSubRegister(instruction);
                                    break;
                                }
                            case 0x8006:
                                {
                                    RegisterShrRegister(instruction);
                                    break;
                                }
                            case 0x8007:
                                {
                                    RegisterSubnRegister(instruction);
                                    break;
                                }
                            case 0x800E:
                                {
                                    RegisterShlRegister(instruction);
                                    break;
                                }
                        }
                        break;
                    }
                case 0x9000:
                    {
                        SkipIfRegisterNotEqualRegister(instruction);
                        break;
                    }
                case 0xA000:
                    {
                        SetIValue(instruction);
                        break;
                    }
                case 0xB000:
                    {
                        JumpAddRegister(instruction);
                        break;
                    }
                case 0xC000:
                    {
                        Rand(instruction);
                        break;
                    }
                case 0xD000:
                    {
                        Draw(instruction);
                        break;
                    }
                case 0xE000:
                    {
                        switch (instruction & 0xF0FF)
                        {
                            case 0xE09E:
                                {
                                    SkipIfKeyPressed(instruction);
                                    break;
                                }
                            case 0xE0A1:
                                {
                                    SkipIfKeyNotPressed(instruction);
                                    break;
                                }
                        }
                        break;
                    }
                case 0xF000:
                    {
                        switch (instruction & 0xF0FF)
                        {
                            case 0xF007:
                                {
                                    SetRegisterToDelayTime(instruction);
                                    break;
                                }
                            case 0xF00A:
                                {
                                    WaitForKey(instruction);
                                    break;
                                }
                            case 0xF015:
                                {
                                    SetDelayTimeToRegister(instruction);
                                    break;
                                }
                            case 0xF018:
                                {
                                    SetSoundTimerToRegister(instruction);
                                    break;
                                }
                            case 0xF01E:
                                {
                                    AddToI(instruction);
                                    break;
                                }
                            case 0xF029:
                                {
                                    SetIToFont(instruction);
                                    break;
                                }
                            case 0xF033:
                                {
                                    BcdRepresentation(instruction);
                                    break;
                                }
                            case 0xF055:
                                {
                                    StoreRegistersInMemory(instruction);
                                    break;
                                }
                            case 0xF065:
                                {
                                    ReadRegistersFromMemory(instruction);
                                    break;
                                }
                        }
                        break;
                    }
            }
        }

        //00E0 - CLS
        void ClearDisplay()
        {
            _display.Clear();
        }

        //00EE - RET
        void ReturnFromSubroutine()
        {
            var addr = _stack.Pop();
            _programCounter = addr;
        }

        //1nnn - JP addr
        void Jump(int instruction)
        {
            var addr = instruction & 0x0FFF;
            _programCounter = (ushort)(addr - 2);
        }

        //2nnn - CALL addr
        void Call(int instruction)
        {
            var addr = instruction & 0x0FFF;

            _stack.Push(_programCounter);
            _programCounter = (ushort)(addr - 2);
        }

        //3xkk - SE Vx, byte
        void SkipIfRegisterEqualByte(int instruction)
        {
            var register = (byte)((instruction & 0x0F00) >> 8);
            var byteVar = instruction & 0x00FF;

            if (_registers.GetRegister(register) == byteVar)
                _programCounter += 2;
        }

        //4xkk - SNE Vx, byte
        void SkipIfRegisterNotEqualByte(int instruction)
        {
            var register = (byte)((instruction & 0x0F00) >> 8);
            var byteVar = instruction & 0x00FF;

            if (_registers.GetRegister(register) != byteVar)
                _programCounter += 2;
        }

        //5xy0 - SE Vx, Vy
        void SkipIfRegisterEqualRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            if (_registers.GetRegister(reg1) == _registers.GetRegister(reg2))
                _programCounter += 2;
        }

        //6xkk - LD Vx, byte
        void SetByteToRegister(int instruction)
        {
            var register = (byte)((instruction & 0x0F00) >> 8);
            var byteVar = instruction & 0x00FF;

            _registers.SetRegister(register, (byte)byteVar);
        }

        //7xkk - ADD Vx, byte
        void AddByteToRegister(int instruction)
        {
            var register = (byte)((instruction & 0x0F00) >> 8);
            var registerValue = _registers.GetRegister(register);
            var byteVar = instruction & 0x00FF;

            _registers.SetRegister(register, (byte)(byteVar + registerValue));
        }

        //8xy0 - LD Vx, Vy
        void SetRegisterToRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            _registers.SetRegister(reg1, _registers.GetRegister(reg2));
        }

        //8xy1 - OR Vx, Vy
        void RegisterOrRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (byte)(_registers.GetRegister(reg1) | _registers.GetRegister(reg2));
            _registers.SetRegister(reg1, result);
        }

        //8xy2 - AND Vx, Vy
        void RegisterAndRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (byte)(_registers.GetRegister(reg1) & _registers.GetRegister(reg2));
            _registers.SetRegister(reg1, result);
        }

        //8xy3 - XOR Vx, Vy
        void RegisterXorRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (byte)(_registers.GetRegister(reg1) ^ _registers.GetRegister(reg2));
            _registers.SetRegister(reg1, result);
        }

        //8xy4 - ADD Vx, Vy
        void AddRegisterToRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (ushort)(_registers.GetRegister(reg1) + _registers.GetRegister(reg2));
            _registers.SetRegister(15, result > 255 ? (byte) 1 : (byte) 0);

            _registers.SetRegister(reg1, (byte)result);
        }

        //8xy5 - SUB Vx, Vy
        void RegisterSubRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (_registers.GetRegister(reg1) - _registers.GetRegister(reg2));
            _registers.SetRegister(15, result < 0 ? (byte) 0 : (byte) 1);

            _registers.SetRegister(reg1, (byte)result);
        }

        //8xy6 - SHR Vx {, Vy}
        void RegisterShrRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);

            _registers.SetRegister(15, (byte) (_registers.GetRegister(reg1) & 1) == 1 ? (byte) 1 : (byte) 0);

            var result = (byte)(_registers.GetRegister(reg1) >> 1);
            _registers.SetRegister(reg1, result);
        }

        //8xy7 - SUBN Vx, Vy
        void RegisterSubnRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (_registers.GetRegister(reg2) - _registers.GetRegister(reg1));
            _registers.SetRegister(15, result < 0 ? (byte) 0 : (byte) 1);

            _registers.SetRegister(reg1, (byte)result);
        }

        //8xyE - SHL Vx {, Vy}
        void RegisterShlRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);

            _registers.SetRegister(15, (byte) (_registers.GetRegister(reg1) & 0x80) == 1 ? (byte) 1 : (byte) 0);

            var result = (byte)(_registers.GetRegister(reg1) << 1);
            _registers.SetRegister(reg1, result);
        }

        //9xy0 - SNE Vx, Vy
        void SkipIfRegisterNotEqualRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            if (_registers.GetRegister(reg1) != _registers.GetRegister(reg2))
                _programCounter += 2;
        }

        //Annn - LD I, addr
        void SetIValue(int instruction)
        {
            var value = (ushort)(instruction & 0x0FFF);

            _registers.SetAddressRegister(value);
        }

        //Bnnn - JP V0, addr
        void JumpAddRegister(int instruction)
        {
            var addr = (ushort)(instruction & 0x0FFF);
            _programCounter = (ushort)((addr + _registers.GetRegister(0)) - 2);
        }

        //Cxkk - RND Vx, byte
        void Rand(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var value = (byte)(instruction & 0x00FF);

            var result = (byte)(_random.Next(0, 255) & value);
            _registers.SetRegister(reg1, result);
        }

        //Dxyn - DRW Vx, Vy, nibble
        void Draw(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);
            var height = instruction & 0x000F;

            var xPos = _registers.GetRegister(reg1);
            var yPos = _registers.GetRegister(reg2);

            var address = _registers.GetAddressRegister();

            _registers.SetRegister(15, 0);
            for (int y = 0; y < height; y++)
            {
                var pixel = _memory.Read((ushort)(address + y));
                var xLine = xPos;
                for (int x = 0; x < 8; x++)
                {
                    if (xLine > 63)
                        xLine = 0;
                    if (yPos > 31)
                        yPos = 0;

                    var bit = (pixel & (0x80 >> x)) != 0;
                    var position = (ushort)(xLine + (yPos * 64));

                    if (bit)
                    {
                        if (_display.GetPixel(position))
                            _registers.SetRegister(15, 1);
                        _display.SetPixel(position);
                    }

                    xLine++;
                }

                yPos++;
            }
        }

        //Ex9E - SKP Vx
        void SkipIfKeyPressed(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var keyCode = _registers.GetRegister(reg1);
            if (_keyboard.IsKeyPressed(keyCode))
                _programCounter += 2;
        }

        //ExA1 - SKNP Vx
        void SkipIfKeyNotPressed(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var keyCode = _registers.GetRegister(reg1);
            if (!_keyboard.IsKeyPressed(keyCode))
                _programCounter += 2;
        }

        //Fx07 - LD Vx, DT
        void SetRegisterToDelayTime(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            _registers.SetRegister(reg1, _delayTimer);
        }

        //Fx0A - LD Vx, K
        void WaitForKey(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var key = _keyboard.ReadKey();

            _registers.SetRegister(reg1, key);
        }

        //Fx15 - LD DT, Vx
        void SetDelayTimeToRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            _delayTimer = _registers.GetRegister(reg1);
        }

        //Fx18 - LD ST, Vx
        void SetSoundTimerToRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            _soundTimer = _registers.GetRegister(reg1);
        }

        //Fx1E - ADD I, Vx
        void AddToI(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var result = (ushort)(_registers.GetAddressRegister() + _registers.GetRegister(reg1));
            _registers.SetAddressRegister(result);
        }

        //Fx29 - LD F, Vx
        void SetIToFont(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var position = _registers.GetRegister(reg1);
            var result = (ushort)(position * 5);
            _registers.SetAddressRegister(result);
        }

        //Fx33 - LD B, Vx
        void BcdRepresentation(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var value = _registers.GetRegister(reg1);

            var ones = value % 10;
            value /= 10;

            var tens = value % 10;
            value /= 10;

            var hundreds = value;

            var address = _registers.GetAddressRegister();
            _memory.Write(address, hundreds);
            _memory.Write((ushort)(address + 1), (byte)tens);
            _memory.Write((ushort)(address + 2), (byte)ones);
        }

        //Fx55 - LD [I], Vx
        void StoreRegistersInMemory(int instruction)
        {
            var address = _registers.GetAddressRegister();
            var reg1 = (byte)((instruction & 0x0F00) >> 8);

            for (byte i = 0; i <= reg1; i++)
            {
                _memory.Write((ushort)(address + i), _registers.GetRegister(i));
            }
        }

        //Fx65 - LD Vx, [I]
        void ReadRegistersFromMemory(int instruction)
        {
            var address = _registers.GetAddressRegister();
            var x = (byte)((instruction & 0x0F00) >> 8);

            for (byte i = 0; i <= x; i++)
            {
                var value = _memory.Read((ushort)(address + i));
                _registers.SetRegister(i, value);
            }
        }
    }
}