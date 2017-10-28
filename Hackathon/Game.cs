namespace Hackathon
{
    public static class Game
    {
        public enum Celltype
        {
            Player0 = 0,
            Player1 = 1,
            Dead = 2
        }

        public static Celltype[,] Board { get; set; }
        private static Path Path { get; set; }

        public static void Tick()
        {
            Path = new Path(Board);
            Path.Tick();
            Path.Tick();
            Path.Tick();
            Path.Tick();
            Path.Tick();
            Board = Path.Board;
        }
    }
}
