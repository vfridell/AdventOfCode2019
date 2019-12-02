using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public static class Day1
    {
        public static int[] input = { 83326, 84939, 135378, 105431, 119144, 124375, 138528, 88896, 98948, 85072, 112576, 144497, 112824, 98892, 81551, 139462, 73213, 93261, 130376, 118425, 132905, 54627, 134676, 140435, 131410, 128441, 96755, 94866, 89490, 122118, 106596, 77531, 84941, 57494, 97518, 136224, 69247, 147209, 92814, 63436, 79819, 109335, 85698, 110103, 79072, 52282, 73957, 68668, 105394, 149663, 91954, 66479, 55778, 126377, 75471, 75662, 71910, 113031, 133917, 76043, 65086, 117882, 134854, 60690, 67495, 62434, 67758, 95329, 123078, 128541, 108213, 93543, 147937, 148262, 56212, 148586, 73733, 110763, 149243, 133232, 95817, 68261, 123872, 93764, 147297, 51555, 110576, 89485, 109570, 88052, 132786, 70585, 105973, 85898, 149990, 114463, 147536, 67786, 139193, 112322 };

        public static int CalcFuelRequirement(int mass) => (mass / 3) - 2;
        public static int CalcFuelRequirement2(int mass)
        {
            int fuel = (mass / 3) - 2;
            if(fuel >= 9)
            {
                fuel += CalcFuelRequirement2(fuel);
            }
            return fuel;
        }

        public static void Part1()
        {
            int totalFuel = input.Select(m => CalcFuelRequirement(m)).Sum();
            Console.WriteLine($"Total fuel required: {totalFuel}");
        }
        public static void Part2()
        {
            int totalFuel = input.Select(m => CalcFuelRequirement2(m)).Sum();
            Console.WriteLine($"Total fuel required: {totalFuel}");
        }
    }
}
