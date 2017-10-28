namespace Hackathon.Move
{
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
    }
}
