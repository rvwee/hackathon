namespace Hackathon
{
    using System;
    using System.Collections.Generic;
    using Move;

    public static class Game
    {
        public static readonly Random Random = new Random();

        public enum Celltype
        {
            Player0 = 0,
            Player1 = 1,
            Dead = 2
        }

        public static Celltype[,] Board { get; set; }

        public static int StartMoveCount = 25;
        public static int PathCount = 50;
        public static int PathDepth = 20;

        public static BaseMove GetBestMove()
        {
            BaseMove bestMove = null;
            int bestMoveScore = int.MinValue;
            
            for (var x = 0; x < StartMoveCount; x++)
            {
                var move = GenerateRandomMove(Settings.MyBotId, Board);
                move.Simulate(Board);
                var score = move.GetScore();
                if (score > bestMoveScore)
                {
                    bestMove = move;
                    bestMoveScore = score;
                }
            }

            Console.Error.WriteLine("Performing move '" + bestMove + "' with score " + bestMoveScore);
            return bestMove;
        }

        private static BaseMove GenerateRandomMove(int playerId, Celltype[,] board)
        {
            List<Point>[] points = new List<Point>[3];
            points[0] = new List<Point>();
            points[1] = new List<Point>();
            points[2] = new List<Point>();

            for (int x = 0; x < Settings.FieldWidth; x++)
                for (int y = 0; y < Settings.FieldHeight; y++)
                    points[(int)board[x, y]].Add(new Point(x, y));

            bool canBirth = points[playerId].Count > 1 && points[(int)Celltype.Dead].Count > 0;

            if (canBirth && Random.NextDouble() > 0.5)
            {
                var emptyField = points[(int)Celltype.Dead][Random.Next(points[(int)Celltype.Dead].Count)];

                var random1 = Random.Next(points[playerId].Count);
                var point1 = points[playerId][random1];
                points[playerId].RemoveAt(random1);

                var random2 = Random.Next(points[playerId].Count);
                var point2 = points[playerId][random2];
                
                return new BirthMove(emptyField, point1, point2);
            }

            return new KillMove(points[playerId][Random.Next(points[playerId].Count)]);
        }
    }
}
