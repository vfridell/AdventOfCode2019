using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace AdventOfCode2019
{
    public static class Day7
    {
        public static void Part1()
        {
            List<int> program = new List<int>() { 3, 8, 1001, 8, 10, 8, 105, 1, 0, 0, 21, 30, 51, 72, 81, 94, 175, 256, 337, 418, 99999, 3, 9, 101, 5, 9, 9, 4, 9, 99, 3, 9, 1001, 9, 3, 9, 1002, 9, 2, 9, 1001, 9, 2, 9, 1002, 9, 5, 9, 4, 9, 99, 3, 9, 1002, 9, 4, 9, 101, 4, 9, 9, 102, 5, 9, 9, 101, 3, 9, 9, 4, 9, 99, 3, 9, 1002, 9, 4, 9, 4, 9, 99, 3, 9, 102, 3, 9, 9, 1001, 9, 4, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 99, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 99, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 99, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 99 };
            //List<int> program = new List<int>() { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 };
            Dictionary<string, IntcodeComputer> computers = new Dictionary<string, IntcodeComputer>();
            computers.Add("A", new IntcodeComputer());
            computers.Add("B", new IntcodeComputer());
            computers.Add("C", new IntcodeComputer());
            computers.Add("D", new IntcodeComputer());
            computers.Add("E", new IntcodeComputer());

            computers["A"].Subscribe(computers["B"]);
            computers["B"].Subscribe(computers["C"]);
            computers["C"].Subscribe(computers["D"]);
            computers["D"].Subscribe(computers["E"]);

            List<int> phases = new List<int> { 4, 3, 2, 1, 0 };
            LinkedList<int> phasesLL = new LinkedList<int>(phases);

            List<List<int>> phaseOrders = new List<List<int>>();
            GetPhaseSyncOrder(phasesLL, new List<int>(), phaseOrders);
            BigInteger maxThrusterPower = int.MinValue;
            foreach(List<int> phaseOrder in phaseOrders)
            {
                // set the phase inputs
                int i = 0;
                foreach (var computer in computers.Values)
                {
                    computer.Input.Clear();
                    computer.Input.Add(phaseOrder[i++]);
                }
                
                computers["A"].Input.Add(0);
                computers["A"].RunProgram(program);
                computers["B"].RunProgram(program);
                computers["C"].RunProgram(program);
                computers["D"].RunProgram(program);
                computers["E"].RunProgram(program);
                BigInteger thrustPower = computers["E"].Output;
                Console.WriteLine($"ThrustPower: {thrustPower}");
                maxThrusterPower = BigInteger.Max(thrustPower, maxThrusterPower);

            }
            Console.WriteLine($"MaxThrustPower: {maxThrusterPower}");
        }

        public static void Part2()
        {
            List<int> program = new List<int>() { 3, 8, 1001, 8, 10, 8, 105, 1, 0, 0, 21, 30, 51, 72, 81, 94, 175, 256, 337, 418, 99999, 3, 9, 101, 5, 9, 9, 4, 9, 99, 3, 9, 1001, 9, 3, 9, 1002, 9, 2, 9, 1001, 9, 2, 9, 1002, 9, 5, 9, 4, 9, 99, 3, 9, 1002, 9, 4, 9, 101, 4, 9, 9, 102, 5, 9, 9, 101, 3, 9, 9, 4, 9, 99, 3, 9, 1002, 9, 4, 9, 4, 9, 99, 3, 9, 102, 3, 9, 9, 1001, 9, 4, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 99, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 99, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 99, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 99 };
            //List<int> program = new List<int>() {3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54, -5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4, 53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10 };
            Dictionary<string, IntcodeComputer> computers = new Dictionary<string, IntcodeComputer>();
            computers.Add("A", new IntcodeComputer());
            computers.Add("B", new IntcodeComputer());
            computers.Add("C", new IntcodeComputer());
            computers.Add("D", new IntcodeComputer());
            computers.Add("E", new IntcodeComputer());

            computers["A"].Subscribe(computers["B"]);
            computers["B"].Subscribe(computers["C"]);
            computers["C"].Subscribe(computers["D"]);
            computers["D"].Subscribe(computers["E"]);
            computers["E"].Subscribe(computers["A"]);

            List<int> phases = new List<int> { 5, 6, 7, 8, 9 };
            LinkedList<int> phasesLL = new LinkedList<int>(phases);

            List<List<int>> phaseOrders = new List<List<int>>();
            GetPhaseSyncOrder(phasesLL, new List<int>(), phaseOrders);
            BigInteger maxThrusterPower = int.MinValue;
            foreach (List<int> phaseOrder in phaseOrders)
            {
                // set the phase inputs
                int i = 0;
                foreach (var computer in computers.Values)
                {
                    computer.Input.Clear();
                    computer.Input.Add(phaseOrder[i++]);
                }

                computers["A"].Input.Add(0);
                Task.Run(() => computers["A"].RunProgram(program));
                Task.Run(() => computers["B"].RunProgram(program));
                Task.Run(() => computers["C"].RunProgram(program));
                Task.Run(() => computers["D"].RunProgram(program));
                computers["E"].RunProgram(program);
                BigInteger thrustPower = computers["E"].Output;
                Console.WriteLine($"ThrustPower: {thrustPower}");
                maxThrusterPower = BigInteger.Max(thrustPower, maxThrusterPower);

            }
            Console.WriteLine($"MaxThrustPower: {maxThrusterPower}");
        }

        public static void GetPhaseSyncOrder(LinkedList<int> remaining, List<int> constructing, List<List<int>> result)
        {
            if (remaining.Count == 1)
            {
                constructing.Add(remaining.First.Value);
                result.Add(constructing);
                return;
            }

            for(int i=0; i<remaining.Count; i++)
            {
                LinkedList<int> ll2 = new LinkedList<int>(remaining);
                int moves = 0;
                var enumerator = ll2.GetEnumerator();
                enumerator.MoveNext();
                while (moves < i)
                {
                    enumerator.MoveNext();
                    moves++;
                }
                ll2.Remove(enumerator.Current);
                List<int> subConstructing = new List<int>(constructing);
                subConstructing.Add(enumerator.Current);
                GetPhaseSyncOrder(ll2, subConstructing, result);
            }
        }
    }
}
