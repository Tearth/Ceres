using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace CHIP8
{
    public class Emulator
    {
        ushort programCounter;
        byte delayTimer;
        byte soundTimer;

        float timersDeltaTime = 17;
        int mainLoopDeltaTime = 1;

        Random random = new Random();
        Display display = new Display();
        Memory memory = new Memory();
        Registers registers = new Registers();
        Keyboard keyboard = new Keyboard();
        Stack stack = new Stack();

        Stopwatch mainWatch = new Stopwatch();
        Stopwatch timersWatch = new Stopwatch();

        public Emulator()
        {

        }

        public void Init()
        {
            display.Init();
            memory.Init();
            registers.Init();
            keyboard.Init();
            stack.Init();

            delayTimer = 0;
            soundTimer = 0;
            programCounter = 512;
        }

        public bool Load(String appFile)
        {
            if (!File.Exists(appFile))
                return false;

            var app = File.ReadAllBytes(appFile);
            for (int i = 0; i < app.Length; i++)
            {
                memory.Write((ushort)(512 + i), app[i]);
            }

            return true;
        }

        public void Run()
        {
            mainWatch.Start();
            timersWatch.Start();

            while (true)
            {
                var miliseconds = mainWatch.ElapsedMilliseconds;
                if (miliseconds > mainLoopDeltaTime)
                {
                    mainWatch.Restart();

                    var instruction = memory.Read(programCounter) << 8 | memory.Read((ushort)(programCounter + 1));

                    UpdateTimers();
                    Parse(instruction);

                    programCounter += 2;
                }
            }
        }

        void UpdateTimers()
        {
            var miliseconds = timersWatch.ElapsedMilliseconds;
            if (miliseconds > timersDeltaTime)
            {
                timersWatch.Restart();

                if (delayTimer > 0)
                    delayTimer--;
                else
                    delayTimer = 0;

                if (soundTimer > 0)
                {
                    new Thread(() => { Console.Beep(); }).Start();
                    soundTimer--;
                }
                else
                    soundTimer = 0;
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
                            default: { break; }
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
                            default: { break; }
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
                            default: { break; }
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
                                    BCDRepresentation(instruction);
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
                            default: { break; }
                        }
                        break;
                    }
                default: { break; }
            }
        }

        //00E0 - CLS
        void ClearDisplay()
        {
            display.Clear();
        }

        //00EE - RET
        void ReturnFromSubroutine()
        {
            var addr = stack.Pop();
            programCounter = (ushort)(addr);
        }

        //1nnn - JP addr
        void Jump(int instruction)
        {
            var addr = instruction & 0x0FFF;
            programCounter = (ushort)(addr - 2);
        }

        //2nnn - CALL addr
        void Call(int instruction)
        {
            var addr = instruction & 0x0FFF;

            stack.Push(programCounter);
            programCounter = (ushort)(addr - 2);
        }

        //3xkk - SE Vx, byte
        void SkipIfRegisterEqualByte(int instruction)
        {
            var register = (byte)((instruction & 0x0F00) >> 8);
            var byteVar = instruction & 0x00FF;

            if (registers.GetRegister(register) == byteVar)
                programCounter += 2;
        }

        //4xkk - SNE Vx, byte
        void SkipIfRegisterNotEqualByte(int instruction)
        {
            var register = (byte)((instruction & 0x0F00) >> 8);
            var byteVar = instruction & 0x00FF;

            if (registers.GetRegister(register) != byteVar)
                programCounter += 2;
        }

        //5xy0 - SE Vx, Vy
        void SkipIfRegisterEqualRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            if (registers.GetRegister(reg1) == registers.GetRegister(reg2))
                programCounter += 2;
        }

        //6xkk - LD Vx, byte
        void SetByteToRegister(int instruction)
        {
            var register = (byte)((instruction & 0x0F00) >> 8);
            var byteVar = instruction & 0x00FF;

            registers.SetRegister(register, (byte)byteVar);
        }

        //7xkk - ADD Vx, byte
        void AddByteToRegister(int instruction)
        {
            var register = (byte)((instruction & 0x0F00) >> 8);
            var registerValue = registers.GetRegister(register);
            var byteVar = instruction & 0x00FF;

            registers.SetRegister(register, (byte)(byteVar + registerValue));
        }

        //8xy0 - LD Vx, Vy
        void SetRegisterToRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            registers.SetRegister(reg1, registers.GetRegister(reg2));
        }

        //8xy1 - OR Vx, Vy
        void RegisterOrRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (byte)(registers.GetRegister(reg1) | registers.GetRegister(reg2));
            registers.SetRegister(reg1, result);
        }

        //8xy2 - AND Vx, Vy
        void RegisterAndRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (byte)(registers.GetRegister(reg1) & registers.GetRegister(reg2));
            registers.SetRegister(reg1, result);
        }

        //8xy3 - XOR Vx, Vy
        void RegisterXorRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (byte)(registers.GetRegister(reg1) ^ registers.GetRegister(reg2));
            registers.SetRegister(reg1, result);
        }

        //8xy4 - ADD Vx, Vy
        void AddRegisterToRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (ushort)(registers.GetRegister(reg1) + registers.GetRegister(reg2));
            if (result > 255)
                registers.SetRegister(15, 1);
            else
                registers.SetRegister(15, 0);

            registers.SetRegister(reg1, (byte)result);
        }

        //8xy5 - SUB Vx, Vy
        void RegisterSubRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (registers.GetRegister(reg1) - registers.GetRegister(reg2));
            if (result < 0)
            {
                //result *= -1;
                registers.SetRegister(15, 0);
            }
            else
            {
                registers.SetRegister(15, 1);
            }

            registers.SetRegister(reg1, (byte)result);
        }

        //8xy6 - SHR Vx {, Vy}
        void RegisterShrRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            if ((byte)(registers.GetRegister(reg1) & 1) == 1)
                registers.SetRegister(15, 1);
            else
                registers.SetRegister(15, 0);

            var result = (byte)(registers.GetRegister(reg1) >> 1);
            registers.SetRegister(reg1, result);
        }

        //8xy7 - SUBN Vx, Vy
        void RegisterSubnRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            var result = (registers.GetRegister(reg2) - registers.GetRegister(reg1));
            if (result < 0)
            {
                //result *= -1;
                registers.SetRegister(15, 0);
            }
            else
            {
                registers.SetRegister(15, 1);
            }

            registers.SetRegister(reg1, (byte)result);
        }

        //8xyE - SHL Vx {, Vy}
        void RegisterShlRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            if ((byte)(registers.GetRegister(reg1) & 0x80) == 1)
                registers.SetRegister(15, 1);
            else
                registers.SetRegister(15, 0);

            var result = (byte)(registers.GetRegister(reg1) << 1);
            registers.SetRegister(reg1, result);
        }

        //9xy0 - SNE Vx, Vy
        void SkipIfRegisterNotEqualRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);

            if (registers.GetRegister(reg1) != registers.GetRegister(reg2))
                programCounter += 2;
        }

        //Annn - LD I, addr
        void SetIValue(int instruction)
        {
            var value = (ushort)(instruction & 0x0FFF);

            registers.SetaddressRegister(value);
        }

        //Bnnn - JP V0, addr
        void JumpAddRegister(int instruction)
        {
            var addr = (ushort)(instruction & 0x0FFF);
            programCounter = (ushort)((addr + registers.GetRegister(0)) - 2);
        }

        //Cxkk - RND Vx, byte
        void Rand(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var value = (byte)(instruction & 0x00FF);

            var result = (byte)(random.Next(0, 255) & value);
            registers.SetRegister(reg1, result);
        }

        //Dxyn - DRW Vx, Vy, nibble
        void Draw(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var reg2 = (byte)((instruction & 0x00F0) >> 4);
            var height = instruction & 0x000F;

            var xPos = registers.GetRegister(reg1);
            var yPos = registers.GetRegister(reg2);

            var address = registers.GetaddressRegsiter();

            registers.SetRegister(15, 0);
            for (int y = 0; y < height; y++)
            {
                var pixel = memory.Read((ushort)(address + y));
                var xLine = xPos;
                for (int x = 0; x < 8; x++)
                {
                    if (xLine > 63)
                        xLine = 0;
                    if (xLine < 0)
                        xLine = 63;
                    if (yPos > 31)
                        yPos = 0;
                    if (yPos < 0)
                        yPos = 31;

                    var bit = (pixel & (0x80 >> x)) != 0;
                    var position = (ushort)(xLine + (yPos * 64));

                    if (bit)
                    {
                        if (display.GetPixel(position))
                            registers.SetRegister(15, 1);
                        display.SetPixel(position);
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
            var keyCode = registers.GetRegister(reg1);
            if (keyboard.IsKeyPressed(keyCode))
                programCounter += 2;
        }

        //ExA1 - SKNP Vx
        void SkipIfKeyNotPressed(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var keyCode = registers.GetRegister(reg1);
            if (!keyboard.IsKeyPressed(keyCode))
                programCounter += 2;
        }

        //Fx07 - LD Vx, DT
        void SetRegisterToDelayTime(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            registers.SetRegister(reg1, delayTimer);
        }

        //Fx0A - LD Vx, K
        void WaitForKey(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var key = keyboard.ReadKey();

            registers.SetRegister(reg1, key);
        }

        //Fx15 - LD DT, Vx
        void SetDelayTimeToRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            delayTimer = registers.GetRegister(reg1);
        }

        //Fx18 - LD ST, Vx
        void SetSoundTimerToRegister(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            soundTimer = registers.GetRegister(reg1);
        }

        //Fx1E - ADD I, Vx
        void AddToI(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var result = (ushort)(registers.GetaddressRegsiter() + registers.GetRegister(reg1));
            registers.SetaddressRegister(result);
        }

        //Fx29 - LD F, Vx
        void SetIToFont(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var position = registers.GetRegister(reg1);
            var result = (ushort)(position * 5);
            registers.SetaddressRegister(result);
        }

        //Fx33 - LD B, Vx
        void BCDRepresentation(int instruction)
        {
            var reg1 = (byte)((instruction & 0x0F00) >> 8);
            var value = registers.GetRegister(reg1);

            var ones = value % 10;
            value /= 10;

            var tens = value % 10;
            value /= 10;

            var hundreds = value;

            var address = registers.GetaddressRegsiter();
            memory.Write((ushort)(address), (byte)hundreds);
            memory.Write((ushort)(address + 1), (byte)tens);
            memory.Write((ushort)(address + 2), (byte)ones);
        }

        //Fx55 - LD [I], Vx
        void StoreRegistersInMemory(int instruction)
        {
            var address = registers.GetaddressRegsiter();
            var reg1 = (byte)((instruction & 0x0F00) >> 8);

            for (byte i = 0; i <= reg1; i++)
            {
                memory.Write((ushort)(address + i), registers.GetRegister(i));
            }
        }

        //Fx65 - LD Vx, [I]
        void ReadRegistersFromMemory(int instruction)
        {
            var address = registers.GetaddressRegsiter();
            var x = (byte)((instruction & 0x0F00) >> 8);

            for (byte i = 0; i <= x; i++)
            {
                var value = memory.Read((ushort)(address + i));
                registers.SetRegister(i, value);
            }
        }
    }
}