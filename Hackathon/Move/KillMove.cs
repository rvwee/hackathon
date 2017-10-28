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

        public override void Print()
        {
            Console.WriteLine("Kill " + Point);
        }

        public override int GetScore()
        {
            throw new System.NotImplementedException();
        }
    }
}
