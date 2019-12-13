using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public struct Coordinate
    {
        public Coordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public int Row;
        public int Column;

        public override string ToString() => $"({Row},{Column})";

        public static Coordinate operator +(Coordinate c1, Coordinate c2) => new Coordinate(c1.Row + c2.Row, c1.Column + c2.Column);
        public static Coordinate operator -(Coordinate c1, Coordinate c2) => new Coordinate(c1.Row - c2.Row, c1.Column - c2.Column);
        public static int Distance(Coordinate c1, Coordinate c2) => Math.Abs(c1.Row - c2.Row) + Math.Abs(c1.Column - c2.Column);
        public static bool AreNeighbors(Coordinate c1, Coordinate c2) => Math.Abs(c1.Row - c2.Row) <= 1  && Math.Abs(c1.Column - c2.Column) <= 1;

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinate)) return false;
            Coordinate coordinate =  (Coordinate)obj;
            return Row == coordinate.Row &&
                   Column == coordinate.Column;
        }

        public override int GetHashCode()
        {
            var hashCode = 240067226;
            hashCode = hashCode * -1521134295 + Row.GetHashCode();
            hashCode = hashCode * -1521134295 + Column.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Coordinate c1, Coordinate c2) => c1.Row == c2.Row && c1.Column == c2.Column;
        public static bool operator !=(Coordinate c1, Coordinate c2) => c1.Row != c2.Row || c1.Column != c2.Column;

    }
}
