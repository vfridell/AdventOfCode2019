using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public static class Day2
    {
        //public static List<int> input = new List<int>() { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 };
        //public static List<int> input = new List<int>() { 1, 1, 1, 4, 99, 5, 6, 0, 99 };
        public static List<int> input = new List<int>() { 1, 0, 0, 3, 1, 1, 2, 3, 1, 3, 4, 3, 1, 5, 0, 3, 2, 9, 1, 19, 1, 19, 5, 23, 1, 9, 23, 27, 2, 27, 6, 31, 1, 5, 31, 35, 2, 9, 35, 39, 2, 6, 39, 43, 2, 43, 13, 47, 2, 13, 47, 51, 1, 10, 51, 55, 1, 9, 55, 59, 1, 6, 59, 63, 2, 63, 9, 67, 1, 67, 6, 71, 1, 71, 13, 75, 1, 6, 75, 79, 1, 9, 79, 83, 2, 9, 83, 87, 1, 87, 6, 91, 1, 91, 13, 95, 2, 6, 95, 99, 1, 10, 99, 103, 2, 103, 9, 107, 1, 6, 107, 111, 1, 10, 111, 115, 2, 6, 115, 119, 1, 5, 119, 123, 1, 123, 13, 127, 1, 127, 5, 131, 1, 6, 131, 135, 2, 135, 13, 139, 1, 139, 2, 143, 1, 143, 10, 0, 99, 2, 0, 14, 0 };
        public static Dictionary<int, Action<int, int, int, List<int>>> opCodes = new Dictionary<int, Action<int, int, int, List<int>>>()
        {
            { 1, (a,b,c,l) => { l[c] = l[a] + l[b]; } },
            { 2, (a,b,c,l) => { l[c] = l[a] * l[b]; } },
        };

        public static void Part1()
        {
            int input1 = 12;
            int input2 = 2;
            input[1] = input1;
            input[2] = input2;

            int opCodeIndex = 0;
            int opCode = input[opCodeIndex];
            while(opCode != 99)
            {
                opCodes[opCode](input[opCodeIndex + 1], input[opCodeIndex + 2], input[opCodeIndex + 3], input);
                opCodeIndex += 4;
                opCode = input[opCodeIndex];
            }

            Console.WriteLine($"Value at position 0: {input[0]}");

        }

        public static void Part2()
        {

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    List<int> output = RunProgram(i, j);
                    IntcodeComputer comp = new IntcodeComputer();
                    //List<int> output = comp.RunProgram(i, j, input);
                    if(output[0] == 19690720)
                    {
                        Console.WriteLine($"Noun {i}, Verb {j}: 100 * {i} + {j} = {100 * i + j}");
                        return;
                    }
                    Console.WriteLine($"Value at position 0: {output[0]}");
                }
            }
        }

        public static List<int> RunProgram(int input1, int input2)
        {
            List<int> currentInput = new List<int>(input);

            currentInput[1] = input1;
            currentInput[2] = input2;

            int opCodeIndex = 0;
            int opCode = currentInput[opCodeIndex];
            while (opCode != 99)
            {
                opCodes[opCode](currentInput[opCodeIndex + 1], currentInput[opCodeIndex + 2], currentInput[opCodeIndex + 3], currentInput);
                opCodeIndex += 4;
                opCode = currentInput[opCodeIndex];
            }
            return currentInput;
        }
    }
}
