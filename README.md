# Ceres: CHIP-8 emulator for .NET

CHIP-8 is an interpreted programming language which was first used on computer systems at the break of 1970/1980. A typical device with CHIP-8 had 4kB RAM (where the first 512 bytes were reserved for the interpreter), 16 8-bit registers, one 16-bit register, 16-key hexadecimal keypad and 64-32 pixel display. Additional, the CHIP-8 supports two timers (delay and sound).

Games for CHIP-8 emulator can be downloaded from http://www.zophar.net/pdroms/chip8/chip-8-games-pack.html (public domain license).

# Keypad layout:

|||||
|---|---|---|---|
|1|2|3|4|
|q|w|e|r|
|a|s|d|f|
|z|x|c|v|

# Supported instructions
| Opcode        | Name |  Long description   | 
| ------------- | ---- | ------------------- |
| 00E0          | CLS  | clear display | 
| 00EE      	| RET  | return from subprogram      | 
| 1nnn |JUMP addr| jump to address nnn      |
| 2nnn |CALL addr| call subprogram at nnn address      |
| 3xkk |SE Vx, byte| skip next instruction if the value of register x is equal kk byte      |
| 4xkk |SNE Vx, byte| skip next instruction if the value of register x is not equal kk byte      |
| 5xy0 |SE Vx, Vy| skip next instruction if the value of register x is equal to the value of register y     |
| 6xkk |LD Vx, byte| set byte kk to register x     |
| 7xkk |ADD Vx, byte| add byte kk to register x     |
| 8xy0 |LD Vx, Vy| set register x to value of register y     |
| 8xy1 |OR Vx, Vy| set register x to result "the value of register x OR the value of register y"     |
| 8xy2 |AND Vx, Vy| set register x to result "the value of register x AND the value of register y"     |
| 8xy3 |XOR Vx, Vy| set register x to result "the value of register x XOR the value of register y"     |
| 8xy4 |ADD Vx, Vy| add the value of register y to register x     |
| 8xy5 |SUB Vx, Vy| subtract the value of register y to register x     |
| 8xy6 |SHR Vx, [Vy]| shift the value of register x right by one     |
| 8xy7 |SUBN Vx, Vy| set register x to result of "the value of register y minus the value of register x"     |
| 8xyE |SHL Vx, Vy| shift the value of register x left by one     |
| 9xy0 |SNE Vx, Vy| skip next instruction if the value of register x is not equal to value of register y     |
| Annn |LD I, addr| set I (16 bit register) to NNN    |
| Bnnn |JP V0, addr| jump to address NNN + the value of register 0    |
| Cxkk |RND Vx, byte| set register x to result "byte kk AND rnd[0-255]    | 
| Dxyn |DRW Vx, Vy, nibblee| draw a sprite (8 pixels width, n pixels height) at position (the value of register x, the value of register y)    |
| Ex9E |SKP Vxe| skip the next instruction if the key which symbol is stored in register x is pressed    |
| ExA1 |SKNP Vx| skip the next instruction if key which symbol is stored in register x is not pressed    |
| Fx07 |LD Vx, DT| set register x to the value of delay timer    |
| Fx0A |LD Vx, K| wait for the key press and store symbol in register x    |
| Fx15 |LD DT, Vx| set the delay timer to value of register x    |
| Fx18 |LD DT, Vx| set the soung timer to value of register x    |
| Fx1E |ADD I, Vx| add the value of register x to I (16 bit register)    |
| Fx29 |LD F, Vx| set I (16 bit register) to the address of font specified in register x    |
| Fx33 |LD B, Vx| store BCD representation of the value in register x at address specified in I (16 bit register)    |
| Fx55 |LD [I], Vx| store registers from 0 to x in memory at the address specified in I    |
| Fx65 |LD Vx, [I]| load registers from 0 to x in memory at the address specified in I    |