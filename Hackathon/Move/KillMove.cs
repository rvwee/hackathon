namespace Hackathon.Move
{
    using System;
    public class KillMove : BaseMove
    {
        public KillMove(Point point)
        {
            Point = point;
        }

        public Point Point { get; set; }

        public override string ToString()
        {
            return ("Kill " + Point);
        }

        //public override bool Equals(object obj)
        //{
        //    var other = obj as KillMove;
        //    return other != null && other.Point.Equals(Point);
        //}
    }
}
