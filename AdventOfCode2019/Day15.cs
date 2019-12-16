﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{

    public static class Day15
    {
        public static object ConsoleLock = new object();

        public static List<BigInteger> ProgramCode = new List<BigInteger>() { 3, 1033, 1008, 1033, 1, 1032, 1005, 1032, 31, 1008, 1033, 2, 1032, 1005, 1032, 58, 1008, 1033, 3, 1032, 1005, 1032, 81, 1008, 1033, 4, 1032, 1005, 1032, 104, 99, 102, 1, 1034, 1039, 1002, 1036, 1, 1041, 1001, 1035, -1, 1040, 1008, 1038, 0, 1043, 102, -1, 1043, 1032, 1, 1037, 1032, 1042, 1106, 0, 124, 1002, 1034, 1, 1039, 102, 1, 1036, 1041, 1001, 1035, 1, 1040, 1008, 1038, 0, 1043, 1, 1037, 1038, 1042, 1105, 1, 124, 1001, 1034, -1, 1039, 1008, 1036, 0, 1041, 102, 1, 1035, 1040, 102, 1, 1038, 1043, 1002, 1037, 1, 1042, 1105, 1, 124, 1001, 1034, 1, 1039, 1008, 1036, 0, 1041, 102, 1, 1035, 1040, 102, 1, 1038, 1043, 1002, 1037, 1, 1042, 1006, 1039, 217, 1006, 1040, 217, 1008, 1039, 40, 1032, 1005, 1032, 217, 1008, 1040, 40, 1032, 1005, 1032, 217, 1008, 1039, 33, 1032, 1006, 1032, 165, 1008, 1040, 35, 1032, 1006, 1032, 165, 1101, 2, 0, 1044, 1106, 0, 224, 2, 1041, 1043, 1032, 1006, 1032, 179, 1101, 1, 0, 1044, 1106, 0, 224, 1, 1041, 1043, 1032, 1006, 1032, 217, 1, 1042, 1043, 1032, 1001, 1032, -1, 1032, 1002, 1032, 39, 1032, 1, 1032, 1039, 1032, 101, -1, 1032, 1032, 101, 252, 1032, 211, 1007, 0, 37, 1044, 1105, 1, 224, 1102, 0, 1, 1044, 1105, 1, 224, 1006, 1044, 247, 101, 0, 1039, 1034, 101, 0, 1040, 1035, 102, 1, 1041, 1036, 1001, 1043, 0, 1038, 1002, 1042, 1, 1037, 4, 1044, 1105, 1, 0, 31, 10, 7, 30, 32, 67, 8, 24, 11, 62, 6, 11, 19, 78, 16, 20, 8, 80, 14, 19, 63, 8, 40, 36, 65, 34, 59, 23, 33, 29, 79, 19, 47, 28, 54, 8, 11, 41, 33, 57, 85, 25, 56, 48, 16, 90, 74, 39, 11, 79, 68, 18, 46, 33, 74, 47, 25, 60, 1, 23, 78, 69, 5, 55, 12, 28, 73, 22, 80, 30, 26, 55, 2, 6, 96, 21, 57, 34, 33, 10, 91, 72, 61, 31, 2, 24, 29, 94, 24, 12, 43, 60, 72, 79, 27, 24, 21, 95, 59, 15, 53, 34, 9, 36, 82, 83, 4, 67, 30, 62, 5, 70, 94, 1, 81, 75, 6, 18, 68, 9, 26, 38, 31, 1, 98, 57, 97, 63, 8, 60, 35, 5, 48, 36, 59, 75, 4, 88, 23, 21, 39, 10, 99, 13, 36, 53, 66, 73, 28, 33, 80, 28, 78, 23, 7, 30, 27, 77, 28, 69, 69, 1, 65, 78, 17, 17, 2, 16, 27, 91, 43, 27, 72, 93, 6, 5, 92, 12, 55, 79, 94, 98, 60, 19, 15, 36, 35, 55, 9, 62, 84, 27, 74, 56, 25, 9, 60, 72, 15, 34, 59, 15, 31, 58, 76, 24, 81, 62, 99, 35, 31, 14, 39, 25, 60, 3, 5, 46, 24, 48, 22, 1, 73, 99, 96, 27, 46, 48, 5, 65, 26, 6, 48, 11, 13, 69, 12, 33, 22, 95, 11, 72, 28, 42, 28, 88, 5, 31, 56, 50, 72, 30, 49, 84, 52, 32, 11, 45, 7, 54, 60, 12, 72, 33, 38, 62, 18, 54, 31, 8, 92, 53, 34, 4, 76, 21, 46, 81, 53, 81, 21, 10, 63, 12, 75, 22, 62, 87, 32, 23, 30, 40, 29, 24, 61, 6, 88, 70, 14, 18, 99, 13, 14, 4, 72, 5, 22, 54, 90, 75, 35, 1, 10, 49, 17, 7, 98, 8, 81, 13, 47, 59, 13, 80, 70, 9, 26, 73, 22, 77, 3, 22, 73, 99, 74, 11, 10, 60, 4, 27, 86, 46, 67, 30, 94, 29, 93, 26, 66, 25, 8, 14, 92, 24, 45, 78, 24, 23, 97, 31, 9, 25, 25, 61, 44, 35, 31, 73, 52, 80, 35, 96, 32, 43, 8, 66, 57, 87, 31, 85, 12, 50, 74, 7, 23, 61, 12, 7, 78, 1, 1, 53, 14, 54, 18, 18, 63, 41, 25, 90, 1, 85, 24, 22, 98, 62, 35, 14, 19, 50, 80, 20, 7, 73, 21, 14, 81, 19, 89, 11, 31, 84, 7, 53, 9, 54, 20, 90, 72, 31, 70, 54, 17, 31, 59, 18, 8, 69, 83, 58, 78, 12, 98, 20, 81, 26, 50, 95, 19, 25, 54, 31, 80, 67, 6, 3, 87, 6, 99, 93, 22, 75, 73, 34, 52, 58, 22, 32, 52, 34, 30, 85, 54, 58, 75, 14, 22, 97, 12, 36, 53, 67, 32, 99, 54, 15, 4, 66, 69, 7, 48, 87, 25, 17, 41, 57, 10, 63, 35, 24, 43, 5, 57, 25, 93, 22, 71, 7, 36, 63, 84, 26, 4, 7, 78, 26, 68, 77, 35, 9, 70, 17, 12, 59, 41, 78, 18, 54, 18, 80, 18, 86, 93, 19, 35, 73, 34, 53, 97, 23, 2, 95, 30, 32, 85, 21, 21, 79, 19, 18, 85, 57, 23, 85, 35, 34, 61, 30, 66, 29, 19, 76, 30, 17, 46, 1, 16, 98, 26, 25, 91, 15, 47, 54, 75, 26, 17, 36, 74, 60, 33, 28, 49, 53, 15, 13, 45, 6, 90, 26, 73, 17, 87, 4, 68, 18, 30, 22, 96, 92, 97, 14, 40, 24, 50, 96, 15, 49, 55, 79, 8, 16, 1, 50, 5, 60, 55, 14, 41, 67, 25, 26, 71, 18, 26, 89, 70, 14, 6, 51, 11, 94, 68, 69, 22, 73, 63, 6, 33, 88, 36, 51, 20, 6, 44, 26, 71, 17, 31, 11, 86, 81, 23, 31, 80, 18, 87, 26, 12, 91, 8, 41, 6, 18, 9, 33, 90, 1, 59, 56, 32, 29, 54, 50, 34, 12, 74, 97, 10, 39, 87, 41, 9, 52, 67, 21, 22, 38, 61, 57, 1, 87, 4, 35, 98, 61, 16, 95, 78, 65, 17, 31, 9, 71, 9, 52, 52, 9, 8, 73, 40, 36, 16, 48, 52, 9, 26, 39, 4, 17, 42, 1, 35, 80, 93, 4, 40, 23, 13, 66, 7, 28, 84, 73, 22, 31, 76, 31, 21, 39, 4, 83, 84, 41, 27, 66, 34, 88, 15, 50, 65, 45, 22, 65, 26, 78, 15, 50, 40, 79, 31, 38, 9, 60, 2, 51, 24, 46, 99, 42, 27, 45, 1, 71, 20, 78, 86, 95, 9, 81, 0, 0, 21, 21, 1, 10, 1, 0, 0, 0, 0, 0, 0 };
        public static List<RepairDroid> Droids = new List<RepairDroid>();
        public static void Part1()
        {
            RepairDroid repairDroid1 = new RepairDroid();
            RepairDroid repairDroid2 = new RepairDroid();
            RepairDroid repairDroid3 = new RepairDroid();
            RepairDroid repairDroid4 = new RepairDroid();
            repairDroid4.Go(4);
            //Task.WaitAny(
            ////Task.Run(() => repairDroid1.Go(1)),
            ////Task.Run(() => repairDroid2.Go(2)),
            ////Task.Run(() => repairDroid3.Go(3)),
            //Task.Run(() => repairDroid4.Go(4))
            //);
        }

        public static void PrintMap(Coordinate c, RepairDroid.Piece p)
        {
            Coordinate delta = new Coordinate(20, 40);
            char[] chars = new char[3] {'#','.','@', };
            lock(ConsoleLock)
            {
                Console.SetCursorPosition((c + delta).Column, (c + delta).Row);
                Console.Write(chars[(int)p]);
            }
        }
    }

    public class RepairDroid : IObserver<BigInteger>, IObservable<BigInteger>
    {
        public enum Direction { North = 1, South = 2, East = 3, West = 4 }
        public enum Piece { Wall, Location, OxygenSystem };
        public List<Coordinate> DirectionCoordinates = new List<Coordinate>() { new Coordinate(0,0), new Coordinate(-1,0), new Coordinate(0,1), new Coordinate(1,0), new Coordinate(0, -1) };
        public Dictionary<Coordinate, Piece> Locations = new Dictionary<Coordinate, Piece>();
        public List<Coordinate> ToVisit { get; set; } 
        public Coordinate CurrentLocation { get; set; }
        public Coordinate NextLocation { get; set; }
        public bool FoundOxygenSensor { get; set; }
        Direction _currentDirection { get; set; }

        public RepairDroid()
        {
            CurrentLocation = new Coordinate(0, 0);
            Locations.Add(CurrentLocation, Piece.Location);
            ToVisit = new List<Coordinate>(DirectionCoordinates);
            FoundOxygenSensor = false;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(BigInteger value)
        {
            Piece piece = (Piece)(int)value;
            if (!Locations.ContainsKey(NextLocation)) Locations.Add(NextLocation, piece);
            if (ToVisit.Contains(NextLocation)) ToVisit.Remove(NextLocation);
            Day15.PrintMap(NextLocation, piece);

            switch (piece)
            {
                case Piece.Wall:
                    //The repair droid hit a wall. Its position has not changed.
                    var oldDirection = _currentDirection;
                    for (int d = 1; d < 5; d++)
                    {
                        if (ToVisit.Contains(DirectionCoordinates[d] + CurrentLocation))
                        {
                            _currentDirection = (Direction)d;
                            break;
                        }
                    }
                    
                    if(_currentDirection == oldDirection)
                    {
                        if (((int)_currentDirection + 1) > 4) _currentDirection = Direction.North;
                        else _currentDirection = (Direction)(_currentDirection + 1);
                    }
                    NextLocation = CurrentLocation + DirectionCoordinates[(int)_currentDirection];
                    break;
                case Piece.Location:
                    //The repair droid has moved one step in the requested direction.
                    Coordinate previousLocation = CurrentLocation;
                    CurrentLocation = NextLocation;
                    NextLocation = CurrentLocation + DirectionCoordinates[(int)_currentDirection];
                    foreach (var c in DirectionCoordinates) if (c + CurrentLocation != previousLocation) ToVisit.Add(c);
                    break;
                case Piece.OxygenSystem:
                    //The repair droid has moved one step in the requested direction; its new position is the location of the oxygen system.
                    FoundOxygenSensor = true;
                    computer.ForceTerminate();
                    Console.WriteLine("Found it!");
                    break;

            }
            computer.OnNext((int)_currentDirection);
        }


        IntcodeComputer computer;
        public IDisposable Subscribe(IObserver<BigInteger> observer)
        {
            computer = (IntcodeComputer)observer;
            return null;
        }

        internal void Go(int dir)
        {
            _currentDirection = (Direction)dir;
            NextLocation = CurrentLocation + DirectionCoordinates[dir];
            var computer = new IntcodeComputer();
            computer.Subscribe(this);
            Subscribe(computer);
            computer.Input.Add((int)_currentDirection);
            computer.RunProgram(Day15.ProgramCode);
        }
    }
}
