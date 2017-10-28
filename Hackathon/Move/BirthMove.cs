namespace Hackathon.Move
{
    public class BirthMove : BaseMove
    {
        public BirthMove(Point empty, Point kill1, Point kill2)
        {
            EmptyPoint = empty;
            Kill1Point = kill1;
            Kill2Point = kill2;
        }

        public Point EmptyPoint { get; set; }
        public Point Kill1Point { get; set; }
        public Point Kill2Point { get; set; }

        public override string ToString()
        {
            return "Birth " + EmptyPoint + " " + Kill1Point + " " + Kill2Point;
        }
    }
}
