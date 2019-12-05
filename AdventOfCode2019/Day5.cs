﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public class IntcodeComputer
    {
        public Dictionary<int, Func<int, List<ParamMode>, List<int>, int>> opCodes;
        public int Output { get; set; }
        public int Input { get; set; }
        public enum ParamMode { Position = 0, Immediate = 1};

        public void SetOutput(int o)
        {
            Console.WriteLine($"Output: {o}");
            Output = 0;
        }

        public IntcodeComputer()
        {
            opCodes = new Dictionary<int, Func<int, List<ParamMode>, List<int>, int>>()
            {
                { 1, (p, m, l) => //Add
                    {
                        Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        Console.WriteLine($"Add {(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} + {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])} => index {l[p+3]} ");
                        l[l[p+3]] = (m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) + (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] );
                        return p+4;
                    }
                },
                { 2, (p, m, l) => //Mult
                    {
                        Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        Console.WriteLine($"Mult {(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} * {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])} => index {l[p+3]} ");
                        l[l[p+3]] = (m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) * (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] );
                        return p+4;
                    }
                },
                { 3, (p, m, l) => //Input
                    {
                        Console.WriteLine($"Input {Input} => index {l[p+1]}");
                        l[l[p+1]] = Input;
                        return p+2;
                    }
                },
                { 4, (p, m, l) => //Output
                    {
                        SetOutput(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) ;
                        return p+2;
                    }
                },
                { 5, (p, m, l) => //Jump if True
                    {
                        Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]}");
                        Console.WriteLine($"JIFT {(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} is non-zero? jump to index {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])}  ");
                        return (m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) != 0 ? (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] ) : p+3;
                    }
                },
                { 6, (p, m, l) => // Jump if False
                    {
                        Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]}");
                        Console.WriteLine($"JIFF {(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} is zero? jump to index {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])}  ");
                        return (m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) == 0 ? (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] ) : p+3;
                    }
                },
                { 7, (p, m, l) => // Less Than
                    {
                        Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        Console.WriteLine($"{(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} < {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])} => index {l[p+3]} = 1 else index {l[p+3]} = 0");
                        if((m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) < (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] )) l[l[p+3]] = 1;
                        else l[l[p+3]] = 0;
                        return p+4;
                    }
                },
                { 8, (p, m, l) => // Greater Than
                    {
                        Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        Console.WriteLine($"{(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} == {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])} => index {l[p+3]} = 1 else index {l[p+3]} = 0");
                        if((m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) == (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] )) l[l[p+3]] = 1;
                        else  l[l[p+3]] = 0;
                        return p+4;
                    }
                },
            };
        }
             
        public List<ParamMode> GetModes(int opCode)
        {
            if (opCode < 100) return new List<ParamMode>() { 0, 0, 0, 0, 0, 0, 0, 0, };
            var s = opCode.ToString().PadLeft(10, '0');
            var s1 = s.Substring(0, 8);
            return s.Substring(0, 8).Select(c => c == '0' ? ParamMode.Position : ParamMode.Immediate).Reverse().ToList();
        }

        public List<int> RunProgram(List<int> input)
        {
            List<int> currentInput = new List<int>(input);

            int p = 0;
            int opCode = currentInput[p];
            List<ParamMode> m = GetModes(opCode);
            opCode = int.Parse(opCode.ToString().PadLeft(10, '0').Substring(8, 2));
            while (opCode != 99)
            {
                int newP = opCodes[opCode](p, m, currentInput);
                p = newP;
                opCode = currentInput[p];
                m = GetModes(opCode);
                opCode = int.Parse(opCode.ToString().PadLeft(10, '0').Substring(8, 2));
            }
            return currentInput;
        }
    }

    public static class Day5
    {
        public static List<int> input = new List<int>() { 3, 225, 1, 225, 6, 6, 1100, 1, 238, 225, 104, 0, 1101, 37, 34, 224, 101, -71, 224, 224, 4, 224, 1002, 223, 8, 223, 101, 6, 224, 224, 1, 224, 223, 223, 1002, 113, 50, 224, 1001, 224, -2550, 224, 4, 224, 1002, 223, 8, 223, 101, 2, 224, 224, 1, 223, 224, 223, 1101, 13, 50, 225, 102, 7, 187, 224, 1001, 224, -224, 224, 4, 224, 1002, 223, 8, 223, 1001, 224, 5, 224, 1, 224, 223, 223, 1101, 79, 72, 225, 1101, 42, 42, 225, 1102, 46, 76, 224, 101, -3496, 224, 224, 4, 224, 102, 8, 223, 223, 101, 5, 224, 224, 1, 223, 224, 223, 1102, 51, 90, 225, 1101, 11, 91, 225, 1001, 118, 49, 224, 1001, 224, -140, 224, 4, 224, 102, 8, 223, 223, 101, 5, 224, 224, 1, 224, 223, 223, 2, 191, 87, 224, 1001, 224, -1218, 224, 4, 224, 1002, 223, 8, 223, 101, 4, 224, 224, 1, 224, 223, 223, 1, 217, 83, 224, 1001, 224, -124, 224, 4, 224, 1002, 223, 8, 223, 101, 5, 224, 224, 1, 223, 224, 223, 1101, 32, 77, 225, 1101, 29, 80, 225, 101, 93, 58, 224, 1001, 224, -143, 224, 4, 224, 102, 8, 223, 223, 1001, 224, 4, 224, 1, 223, 224, 223, 1101, 45, 69, 225, 4, 223, 99, 0, 0, 0, 677, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1105, 0, 99999, 1105, 227, 247, 1105, 1, 99999, 1005, 227, 99999, 1005, 0, 256, 1105, 1, 99999, 1106, 227, 99999, 1106, 0, 265, 1105, 1, 99999, 1006, 0, 99999, 1006, 227, 274, 1105, 1, 99999, 1105, 1, 280, 1105, 1, 99999, 1, 225, 225, 225, 1101, 294, 0, 0, 105, 1, 0, 1105, 1, 99999, 1106, 0, 300, 1105, 1, 99999, 1, 225, 225, 225, 1101, 314, 0, 0, 106, 0, 0, 1105, 1, 99999, 7, 226, 226, 224, 102, 2, 223, 223, 1005, 224, 329, 101, 1, 223, 223, 108, 677, 226, 224, 102, 2, 223, 223, 1005, 224, 344, 1001, 223, 1, 223, 1108, 226, 677, 224, 102, 2, 223, 223, 1005, 224, 359, 1001, 223, 1, 223, 8, 677, 226, 224, 102, 2, 223, 223, 1006, 224, 374, 1001, 223, 1, 223, 107, 226, 226, 224, 102, 2, 223, 223, 1006, 224, 389, 101, 1, 223, 223, 1108, 677, 226, 224, 1002, 223, 2, 223, 1005, 224, 404, 1001, 223, 1, 223, 108, 677, 677, 224, 102, 2, 223, 223, 1005, 224, 419, 101, 1, 223, 223, 7, 226, 677, 224, 1002, 223, 2, 223, 1006, 224, 434, 1001, 223, 1, 223, 107, 226, 677, 224, 102, 2, 223, 223, 1005, 224, 449, 101, 1, 223, 223, 1108, 677, 677, 224, 1002, 223, 2, 223, 1006, 224, 464, 101, 1, 223, 223, 7, 677, 226, 224, 102, 2, 223, 223, 1006, 224, 479, 101, 1, 223, 223, 1007, 677, 677, 224, 1002, 223, 2, 223, 1005, 224, 494, 101, 1, 223, 223, 1008, 226, 226, 224, 102, 2, 223, 223, 1006, 224, 509, 1001, 223, 1, 223, 107, 677, 677, 224, 102, 2, 223, 223, 1006, 224, 524, 1001, 223, 1, 223, 8, 226, 226, 224, 1002, 223, 2, 223, 1005, 224, 539, 1001, 223, 1, 223, 1007, 677, 226, 224, 102, 2, 223, 223, 1006, 224, 554, 1001, 223, 1, 223, 1007, 226, 226, 224, 1002, 223, 2, 223, 1005, 224, 569, 1001, 223, 1, 223, 8, 226, 677, 224, 1002, 223, 2, 223, 1006, 224, 584, 101, 1, 223, 223, 108, 226, 226, 224, 1002, 223, 2, 223, 1006, 224, 599, 101, 1, 223, 223, 1107, 677, 226, 224, 1002, 223, 2, 223, 1005, 224, 614, 1001, 223, 1, 223, 1107, 226, 677, 224, 102, 2, 223, 223, 1006, 224, 629, 1001, 223, 1, 223, 1008, 226, 677, 224, 102, 2, 223, 223, 1005, 224, 644, 101, 1, 223, 223, 1107, 226, 226, 224, 102, 2, 223, 223, 1006, 224, 659, 1001, 223, 1, 223, 1008, 677, 677, 224, 102, 2, 223, 223, 1006, 224, 674, 1001, 223, 1, 223, 4, 223, 99, 226 };
        //public static List<int> input = new List<int>() { 3, 3, 1107, -1, 8, 3, 4, 3, 99 };

        public static void Part1()
        {
            IntcodeComputer comp = new IntcodeComputer();

            comp.Input = 5;
            List<int> output = comp.RunProgram(input);

        }
    }
}