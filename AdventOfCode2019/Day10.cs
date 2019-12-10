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
            "......#.#.","#..#.#....","..#######.",".#.#.###..",".#..#.....","..#....#.#","#..#....#.",".##.#..###","##...#..#.",".#....####"
        };

        public static void Part1()
        {

            Dictionary<Coordinate, char> map = new Dictionary<Coordinate, char>();
            Coordinate coord = new Coordinate(0,0);
            foreach(string s in Input)
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
            //Dictionary<Coordinate, List<Func<Coordinate, bool>>> rays = new Dictionary<Coordinate, List<Func<Coordinate, bool>>>();
            Dictionary<Coordinate, Dictionary<Coordinate,double>> coordinateSlopes = new Dictionary<Coordinate, Dictionary<Coordinate, double>>();
            foreach (Coordinate a1 in Asteroids)
            {
                coordinateSlopes.Add(a1, new Dictionary<Coordinate, double>());
                //List<Func<Coordinate, bool>> myRays = new List<Func<Coordinate, bool>>();
                //rays.Add(a1, myRays);
                foreach (Coordinate a2 in Asteroids)
                {
                    if (a1 == a2) continue;
                    double m = (double)(a2.Row - a1.Row) / (double)(a2.Column - a1.Column);

                    coordinateSlopes[a1].Add(a2, m);
                    //myRays.Add((c) =>
                    //{
                    //    if (double.IsInfinity(m))
                    //        return c.Column == a1.Column;
                    //    else
                    //        return c.Row - a1.Row == m * (c.Column = a1.Column);
                    //});
                }
            }

            int maxVisibleAsteroids = coordinateSlopes.Max(kvp => kvp.Value.Count);
            int count = coordinateSlopes[new Coordinate(8, 5)].Select(kvp => kvp.Value).Distinct().Count();

            Coordinate answer = new Coordinate(8, 5);
            List<Coordinate> visibleFromAnswer = new List<Coordinate>();
            foreach (var group in coordinateSlopes[answer].GroupBy(kvp => kvp.Value))
            {
                visibleFromAnswer.Add(group.OrderBy(kvp => Coordinate.Distance(answer, kvp.Key)).First().Key);
            }

            coord = new Coordinate(0, 0);
            foreach (string s in Input)
            {
                int i = 0;
                foreach (char c in s)
                {
                    if (visibleFromAnswer.Contains(coord)) Console.Write("%");
                    else if (answer == coord) Console.Write("!");
                    else Console.Write(map[coord]);
                    coord = coord + new Coordinate(0, 1);
                }
                Console.WriteLine();
                coord = new Coordinate(coord.Row + 1, 0);
            }

            Coordinate bestCoord = coordinateSlopes.Where(kvp => kvp.Value.Count == maxVisibleAsteroids).Single().Key;
            Console.WriteLine($"Best Coordinate {bestCoord} has {maxVisibleAsteroids} asteroids visible");

        }



    }
}
