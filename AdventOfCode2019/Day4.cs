using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public static class Day4
    {
        public static void Part1and2()
        {
            int inputStart = 145852;
            int inputEnd = 616942;
            Regex adjacentDigits = new Regex(@"([0-9])\1");
            Regex digitGroups = new Regex(@"([0-9])\1+");

            List<Func<int, bool>> checks = new List<Func<int, bool>>()
            {
                (i) => adjacentDigits.IsMatch(i.ToString()),
                (i) => {
                    var chars = i.ToString().ToCharArray();
                    for (int j=1; j<chars.Length; j++)
                    {
                        if (chars[j] < chars[j-1]) return false;
                    }
                    return true;
                },
                // part 2 below
                (i) => digitGroups.Matches(i.ToString()).Cast<Match>().Select(m => m.Length).Any(l => l == 2),
            };

            int metCriteriaCount = 0;
            for(int candidate = inputStart; candidate <= inputEnd; candidate++ )
            {
                if(checks.All(f => f(candidate)))
                {
                    metCriteriaCount++;
                }
            }
            Console.WriteLine($"{metCriteriaCount} Total candidate passwords");
        }
    }
}
