using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public static class Day12
    {
        public static List<Moon> InputActual = new List<Moon>
        {
            new Moon(new int[] {15, -2, -6}),
            new Moon(new int[] {-5, -4, -11}),
            new Moon(new int[] {0, -6, 0}),
            new Moon(new int[] {5, 9, 6}),
        };

        public static List<Moon> InputTest1 = new List<Moon>
        {
            new Moon(new int[] {-8, -10, 0}),
            new Moon(new int[] {5, 5, 10}),
            new Moon(new int[] {2, -7, 3}),
            new Moon(new int[] {9, -8, -3}),
        };

        public static List<Moon> MoonList = InputActual;


        public static void Part1()
        {
            foreach (Moon moon in MoonList) Console.WriteLine(moon);

            for(int i=0; i<1000; i++) RunStep();

            int totalEnergy = MoonList.Select(m => m.KineticEnergy * m.PotentialEnergy).Sum();

            Console.WriteLine($"Total energy: {totalEnergy}");
            
        }

        public static void Part2()
        {
        }

        private static void RunStep2()
        {

        }

        private static void RunStep()
        {
            for (int i = 0; i < MoonList.Count - 1; i++)
            {
                for (int j = i + 1; j < MoonList.Count; j++)
                {
                    //Console.WriteLine($"{i},{j}");
                    Moon.ApplyGravity(MoonList[i], MoonList[j]);
                }
            }
            foreach (Moon moon in MoonList) moon.ApplyVelocity();
            //foreach (Moon moon in MoonList) Console.WriteLine(moon);
        }
    }


    public class MoonGroup
    {
        public List<Moon> Moons;

        public MoonGroup(List<Moon> initialState)
        {
            Moons = new List<Moon>(initialState);
            //int det = 
        }
    }

    public class Moon
    {
        private int[] vals = new int[6];

        public Moon(int [] initialValues)
        {
            X = initialValues[0];
            Y = initialValues[1];
            Z = initialValues[2];
            Xvel = 0;
            Yvel = 0;
            Zvel = 0;
        }

        public int X { get { return vals[0]; } set { vals[0] = value; } }
        public int Y { get { return vals[1]; } set { vals[1] = value; } }
        public int Z { get { return vals[2]; } set { vals[2] = value; } }

        public int Xvel { get { return vals[3]; } set { vals[3] = value; } }
        public int Yvel { get { return vals[4]; } set { vals[4] = value; } }
        public int Zvel { get { return vals[5]; } set { vals[5] = value; } }

        public int PotentialEnergy => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
        public int KineticEnergy => Math.Abs(Xvel) + Math.Abs(Yvel) + Math.Abs(Zvel);

        public void ApplyVelocity()
        {
            for (int i = 0; i < 3; i++) vals[i] += vals[i + 3];
        }

        public override string ToString() => $"pos=<{X}, {Y}, {Z}>, vel=<{Xvel}, {Yvel}, {Zvel}>";

        public static void ApplyGravity(Moon moon1, Moon moon2)
        {
            for(int i = 0; i< 3; i++)
            {
                if (moon1.vals[i] == moon2.vals[i]) continue;
                int moon1Delta = moon1.vals[i] < moon2.vals[i] ? 1 : -1;
                moon1.vals[i + 3] += moon1Delta;
                moon2.vals[i + 3] += (moon1Delta * -1);
            }
        }




    }
}
