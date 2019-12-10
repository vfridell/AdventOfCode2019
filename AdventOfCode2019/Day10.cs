using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public static class Day10
    {
        public static List<string> Input = new List<string>()
        {
           // "#..#.#.#.######..#.#...##","##.#..#.#..##.#..######.#",".#.##.#..##..#.#.####.#..",".#..##.#.#..#.#...#...#.#","#...###.##.##..##...#..#.","##..#.#.#.###...#.##..#.#","###.###.#.##.##....#####.",".#####.#.#...#..#####..#.",".#.##...#.#...#####.##...","######.#..##.#..#.#.#....","###.##.#######....##.#..#",".####.##..#.##.#.#.##...#","##...##.######..##..#.###","...###...#..#...#.###..#.",".#####...##..#..#####.###",".#####..#.#######.###.##.","#...###.####.##.##.#.##.#",".#.#.#.#.#.##.#..#.#..###","##.#.####.###....###..##.","#..##.#....#..#..#.#..#.#","##..#..#...#..##..####..#","....#.....##..#.##.#...##",".##..#.#..##..##.#..##..#",".##..#####....#####.#.#.#","#..#..#..##...#..#.#.#.##"
           
            // ".#....#####...#..","##...##.#####..##","##...#...#.#####.","..#.....X...###..","..#.#.....#....##"
".#..##.###...#######","##.############..##.",".#.######.########.#",".###.#######.####.#.","#####.##.#.##.###.##","..#####..#.#########","####################","#.####....###.#.#.##","##.#################","#####.##.###..####..","..######..##.#######","####.##.####...##..#",".#####..#.######.###","##...#.##########...","#.##########.#######",".####.#.###.###.#.##","....##.##.###..#####",".#.#.###########.###","#.#.#.#####.####.###","###.##.####.##.#..##"

        };
        public enum Position { Left, Right};

        public static void Part1()
        {

            Dictionary<Coordinate, char> map = new Dictionary<Coordinate, char>();
            Coordinate coord = new Coordinate(0, 0);
            foreach (string s in Input)
            {
                int i = 0;
                foreach (char c in s)
                {
                    map.Add(coord, s[i++]);
                    coord = coord + new Coordinate(0, 1);
                }
                coord = new Coordinate(coord.Row + 1, 0);
            }

            List<Coordinate> Asteroids = map.Where(kvp => kvp.Value != '.').Select(kvp => kvp.Key).ToList();
            Dictionary<Coordinate, Dictionary<Coordinate, (double, Position)>> coordinateSlopes = new Dictionary<Coordinate, Dictionary<Coordinate, (double, Position)>>();
            foreach (Coordinate a1 in Asteroids)
            {
                coordinateSlopes.Add(a1, new Dictionary<Coordinate, (double, Position)>());
                foreach (Coordinate a2 in Asteroids)
                {
                    if (a1 == a2) continue;
                    double m = (double)(a2.Row - a1.Row) / (double)(a2.Column - a1.Column);
                    Position pos = (a2.Column < a1.Column) ? Position.Left : Position.Right;
                    coordinateSlopes[a1].Add(a2, (m, pos));
                }
            }

            IEnumerable<(Coordinate, int)> visibleAsteroids = coordinateSlopes.Select(kvp => (kvp.Key, kvp.Value.GroupBy(kvp2 => kvp2.Value).Count()));

            (Coordinate answer, int maxVisibleAsteroids) = visibleAsteroids.OrderByDescending(t => t.Item2).First();
            Console.WriteLine($"Best Coordinate {answer} has {maxVisibleAsteroids} asteroids visible");
        }

        public static void Part2()
        {

            Dictionary<Coordinate, char> map = new Dictionary<Coordinate, char>();
            Coordinate coord = new Coordinate(0, 0);
            foreach (string s in Input)
            {
                int i = 0;
                foreach (char c in s)
                {
                    map.Add(coord, s[i++]);
                    coord = coord + new Coordinate(0, 1);
                }
                coord = new Coordinate(coord.Row + 1, 0);
            }

            List<Coordinate> Asteroids = map.Where(kvp => kvp.Value != '.').Select(kvp => kvp.Key).ToList();
            Dictionary<Coordinate, Dictionary<Coordinate, (double, Position)>> coordinateSlopes = new Dictionary<Coordinate, Dictionary<Coordinate, (double, Position)>>();
            foreach (Coordinate a1 in Asteroids)
            {
                coordinateSlopes.Add(a1, new Dictionary<Coordinate, (double, Position)>());
                foreach (Coordinate a2 in Asteroids)
                {
                    if (a1 == a2) continue;
                    double m = (double)(a2.Row - a1.Row) / (double)(a2.Column - a1.Column);
                    Position pos = (a2.Column < a1.Column) ? Position.Left : Position.Right;
                    coordinateSlopes[a1].Add(a2, (m, pos));
                }
            }

            IEnumerable<(Coordinate, int)> visibleAsteroids = coordinateSlopes.Select(kvp => (kvp.Key, kvp.Value.GroupBy(kvp2 => kvp2.Value).Count()));
            (Coordinate answer, int maxVisibleAsteroids) = visibleAsteroids.OrderByDescending(t => t.Item2).First();
            Console.WriteLine($"Best Coordinate {answer} has {maxVisibleAsteroids} asteroids visible");

            var righties = coordinateSlopes[answer]
                            .Where(kvp => kvp.Value.Item2 == Position.Right)
                            .OrderBy(kvp => kvp.Value.Item1)
                            .ThenBy(kvp => Coordinate.Distance(answer, kvp.Key));
            var lefties = coordinateSlopes[answer]
                            .Where(kvp => kvp.Value.Item2 == Position.Left)
                            .OrderByDescending(kvp => kvp.Value.Item1)
                            .ThenBy(kvp => Coordinate.Distance(answer, kvp.Key));

            var all = righties.Concat(lefties).GroupBy(kvp => kvp.Value);
            List<Coordinate> blowedUp = new List<Coordinate>();
            while (true)
            {
                foreach (var group in all)
                {
                    if (group.Select(kvp => kvp.Key).Any(c => !blowedUp.Contains(c)))
                    {
                        var asteeroidToBlowUp = group.Select(kvp => kvp.Key).FirstOrDefault(c => !blowedUp.Contains(c));
                        blowedUp.Add(asteeroidToBlowUp);
                        Console.WriteLine($"blowed up {asteeroidToBlowUp}");
                        if(blowedUp.Count == 200)
                        {
                            Console.WriteLine($"200th blowed up is {asteeroidToBlowUp}");
                        }
                    }
                }
            }
        }

        private static void DisplayVisibleFromCoordinate(Dictionary<Coordinate, char> map, Dictionary<Coordinate, Dictionary<Coordinate, (double, Position)>> coordinateSlopes, Coordinate inputCoordinate)
        {
            Coordinate coord;
            List<Coordinate> visibleFromAnswer = new List<Coordinate>();
            foreach (var group in coordinateSlopes[inputCoordinate].GroupBy(kvp => kvp.Value))
            {
                visibleFromAnswer.Add(group.OrderBy(kvp => Coordinate.Distance(inputCoordinate, kvp.Key)).First().Key);
            }

            coord = new Coordinate(0, 0);
            foreach (string s in Input)
            {
                int i = 0;
                foreach (char c in s)
                {
                    if (visibleFromAnswer.Contains(coord)) Console.Write("%");
                    else if (inputCoordinate == coord) Console.Write("!");
                    else Console.Write(map[coord]);
                    coord = coord + new Coordinate(0, 1);
                }
                Console.WriteLine();
                coord = new Coordinate(coord.Row + 1, 0);
            }

            return;
        }
    }
}
