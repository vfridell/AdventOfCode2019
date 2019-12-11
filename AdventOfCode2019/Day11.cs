using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public static class Day11
    {

        public class PaintingRobot : IObserver<BigInteger>, IObservable<BigInteger>
        {
            public class ColorTimesPainted { public int color { get; set; } public int timesPainted { get; set; } };

            public Coordinate Direction = new Coordinate(-1, 0);
            public Coordinate Position = new Coordinate(0, 0);
            public Dictionary<Coordinate, ColorTimesPainted> HullColorNumberTimesPainted = new Dictionary<Coordinate, ColorTimesPainted>();
            public List<BigInteger> Instructions = new List<BigInteger>();
            public bool ColorInstruction = true;

            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(BigInteger value)
            {
                Instructions.Add(value);
                if(ColorInstruction)
                {
                    if(!HullColorNumberTimesPainted.ContainsKey(Position))
                    {
                        HullColorNumberTimesPainted.Add(Position, new ColorTimesPainted() { color = (int)value, timesPainted = 1 });
                    }
                    else
                    {
                        HullColorNumberTimesPainted[Position].color = (int)value;
                        HullColorNumberTimesPainted[Position].timesPainted++;
                    }

                }
                else
                {
                    // move instruction
                    if (value == 0) Direction = TurnLeft(Direction);
                    else Direction = TurnRight(Direction);
                    Position += Direction;

                    ColorTimesPainted color = new ColorTimesPainted() { color = 0, timesPainted = 0 };
                    if (!HullColorNumberTimesPainted.TryGetValue(Position, out color))
                        computer.OnNext(0);
                    else
                        computer.OnNext(color.color);
                }
                ColorInstruction = !ColorInstruction;
            }

            Dictionary<Coordinate, Coordinate> LeftDirections = new Dictionary<Coordinate, Coordinate>()
            {
                { new Coordinate(-1, 0), new Coordinate(0,-1) },
                { new Coordinate(0, 1), new Coordinate(-1, 0)},
                { new Coordinate(1, 0), new Coordinate(0, 1)},
                { new Coordinate(0, -1),  new Coordinate(1, 0) },
            };

            Dictionary<Coordinate, Coordinate> RightDirections = new Dictionary<Coordinate, Coordinate>()
            {
                { new Coordinate(0,-1),  new Coordinate(-1, 0) },
                { new Coordinate(-1, 0), new Coordinate(0, 1)},
                { new Coordinate(0, 1), new Coordinate(1, 0)},
                { new Coordinate(1, 0), new Coordinate(0, -1)},
            };

            private Coordinate TurnLeft(Coordinate direction) => LeftDirections[direction];
            private Coordinate TurnRight(Coordinate direction) => RightDirections[direction];

            IObserver<BigInteger> computer;
            public IDisposable Subscribe(IObserver<BigInteger> observer)
            {
                computer = observer;
                return null;
            }
        }

        private static readonly List<BigInteger> program = new List<BigInteger>()
        {
            3,8,1005,8,315,1106,0,11,0,0,0,104,1,104,0,3,8,1002,8,-1,10,101,1,10,10,4,10,1008,8,0,10,4,10,101,0,8,29,2,1006,16,10,3,8,102,-1,8,10,1001,10,1,10,4,10,1008,8,0,10,4,10,102,1,8,55,3,8,102,-1,8,10,1001,10,1,10,4,10,108,1,8,10,4,10,101,0,8,76,1,101,17,10,1006,0,3,2,1005,2,10,3,8,1002,8,-1,10,1001,10,1,10,4,10,1008,8,1,10,4,10,101,0,8,110,1,107,8,10,3,8,1002,8,-1,10,1001,10,1,10,4,10,108,0,8,10,4,10,101,0,8,135,1,108,19,10,2,7,14,10,2,104,10,10,3,8,1002,8,-1,10,101,1,10,10,4,10,1008,8,1,10,4,10,101,0,8,170,1,1003,12,10,1006,0,98,1006,0,6,1006,0,59,3,8,1002,8,-1,10,1001,10,1,10,4,10,1008,8,0,10,4,10,102,1,8,205,1,4,18,10,1006,0,53,1006,0,47,1006,0,86,3,8,1002,8,-1,10,101,1,10,10,4,10,108,0,8,10,4,10,1001,8,0,239,2,9,12,10,3,8,1002,8,-1,10,1001,10,1,10,4,10,1008,8,1,10,4,10,101,0,8,266,1006,0,8,1,109,12,10,3,8,1002,8,-1,10,1001,10,1,10,4,10,108,1,8,10,4,10,1001,8,0,294,101,1,9,9,1007,9,1035,10,1005,10,15,99,109,637,104,0,104,1,21102,936995730328,1,1,21102,1,332,0,1105,1,436,21102,1,937109070740,1,21101,0,343,0,1106,0,436,3,10,104,0,104,1,3,10,104,0,104,0,3,10,104,0,104,1,3,10,104,0,104,1,3,10,104,0,104,0,3,10,104,0,104,1,21102,1,179410308187,1,21101,0,390,0,1105,1,436,21101,0,29195603035,1,21102,1,401,0,1106,0,436,3,10,104,0,104,0,3,10,104,0,104,0,21102,825016079204,1,1,21102,1,424,0,1105,1,436,21102,1,825544672020,1,21102,435,1,0,1106,0,436,99,109,2,21202,-1,1,1,21102,1,40,2,21102,467,1,3,21101,0,457,0,1105,1,500,109,-2,2106,0,0,0,1,0,0,1,109,2,3,10,204,-1,1001,462,463,478,4,0,1001,462,1,462,108,4,462,10,1006,10,494,1102,0,1,462,109,-2,2106,0,0,0,109,4,1202,-1,1,499,1207,-3,0,10,1006,10,517,21102,1,0,-3,22101,0,-3,1,22101,0,-2,2,21101,1,0,3,21101,0,536,0,1106,0,541,109,-4,2106,0,0,109,5,1207,-3,1,10,1006,10,564,2207,-4,-2,10,1006,10,564,21202,-4,1,-4,1105,1,632,21202,-4,1,1,21201,-3,-1,2,21202,-2,2,3,21101,583,0,0,1106,0,541,22102,1,1,-4,21101,0,1,-1,2207,-4,-2,10,1006,10,602,21101,0,0,-1,22202,-2,-1,-2,2107,0,-3,10,1006,10,624,21202,-1,1,1,21101,624,0,0,106,0,499,21202,-2,-1,-2,22201,-4,-2,-4,109,-5,2106,0,0
        };

        public static void Part1()
        {
            IntcodeComputer computer = new IntcodeComputer();
            computer.Debug = true;
            computer.Input.Add(0);
            PaintingRobot robot = new PaintingRobot();
            computer.Subscribe(robot);
            robot.Subscribe(computer);
            computer.RunProgram(program);

            Console.WriteLine($"Painted {robot.HullColorNumberTimesPainted.Count()} panel at least once");
        }

        public static void Part2()
        {
            IntcodeComputer computer = new IntcodeComputer();
            computer.Input.Add(1);
            PaintingRobot robot = new PaintingRobot();
            computer.Subscribe(robot);
            robot.Subscribe(computer);
            computer.RunProgram(program);

            int minRow = robot.HullColorNumberTimesPainted.Where(kvp => kvp.Value.color == 1).Min(kvp => kvp.Key.Row);
            int maxRow = robot.HullColorNumberTimesPainted.Where(kvp => kvp.Value.color == 1).Max(kvp => kvp.Key.Row);
            int minCol = robot.HullColorNumberTimesPainted.Where(kvp => kvp.Value.color == 1).Min(kvp => kvp.Key.Column);
            int maxCol = robot.HullColorNumberTimesPainted.Where(kvp => kvp.Value.color == 1).Max(kvp => kvp.Key.Column);
            for (int row = minRow; row <= maxRow; row++)
            {
                for (int col = minCol; col <= maxCol; col++)
                {
                    var ctp = new PaintingRobot.ColorTimesPainted() { color = 0, timesPainted = 0 };
                    if (!robot.HullColorNumberTimesPainted.TryGetValue(new Coordinate(row, col), out ctp))
                        Console.Write(".");
                    else if(ctp.color == 0)
                        Console.Write(".");
                    else 
                        Console.Write("#");
                }
                Console.WriteLine();
            }
        }
    }
}
