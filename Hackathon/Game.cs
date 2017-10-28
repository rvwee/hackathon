namespace Hackathon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;
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
        public static int[] LivingCells { get; set; }

        public static int StartBirthCount = 550;
        public static int PathCount = 1;
        public static int PathDepth = 15;

        public static BaseMove GetBestMove()
        {
            BaseMove bestMove = null;
            int bestMoveScore = int.MinValue;

            var moves = GenerateRandomMoves(Board);

            foreach (var move in moves)
            {
                move.Simulate(Board, LivingCells);
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

        private static IEnumerable<BaseMove> GenerateRandomMoves(Celltype[,] board)
        {
            List<Point>[] points = new List<Point>[3];
            points[0] = new List<Point>();
            points[1] = new List<Point>();
            points[2] = new List<Point>();

            for (int x = 0; x < Settings.FieldWidth; x++)
            {
                for (int y = 0; y < Settings.FieldHeight; y++)
                {
                    points[(int) board[x, y]].Add(new Point(x, y));
                }
            }

            if (LivingCells[Settings.EnemyBotId] <= 5 && LivingCells[Settings.MyBotId] > 10)
            {
                foreach (var point in points[Settings.EnemyBotId])
                    yield return new KillMove(point);

                foreach (var point in Neighbours(points[Settings.EnemyBotId]))
                    yield return GenerateNeightbourBirthMove(points[Settings.MyBotId], point);
            }
            else
            {
                foreach (var point in points[Settings.MyBotId])
                    yield return new KillMove(point);

                bool canBirth = points[Settings.MyBotId].Count > 1 && points[(int)Celltype.Dead].Count > 0;
                if (canBirth)
                {
                    for (int i = 0; i < StartBirthCount; i++)
                    {
                        yield return GenerateBirthMove(Settings.MyBotId, points);
                    }
                }
            }
        }

        private static IEnumerable<Point> Neighbours(List<Point> enemyPoints)
        {
            int minX = Math.Max(enemyPoints.Min(p => p.X) - 1, 0), maxX = Math.Min(enemyPoints.Max(p => p.X) + 1, Settings.FieldWidth - 1);
            int minY = Math.Max(enemyPoints.Min(p => p.Y) - 1, 0), maxY = Math.Min(enemyPoints.Max(p => p.Y) + 1, Settings.FieldHeight - 1);

            for (int dy = minY; dy <= maxY; dy++)
            {
                for (int dx = minX; dx <= maxX; dx++)
                {
                    if (Board[dx, dy] != Celltype.Dead)
                        continue;

                    yield return new Point(dx, dy);
                }
            }
        }

        private static BirthMove GenerateBirthMove(int playerId, List<Point>[] points)
        {
            var ownFields = new List<Point>(points[playerId]);

            var emptyField = points[(int)Celltype.Dead][Random.Next(points[(int)Celltype.Dead].Count)];

            var random1 = Random.Next(ownFields.Count);
            var point1 = ownFields[random1];
            ownFields.RemoveAt(random1);

            var random2 = Random.Next(ownFields.Count);
            var point2 = ownFields[random2];
                
            return new BirthMove(emptyField, point1, point2);
        }

        private static BirthMove GenerateNeightbourBirthMove(List<Point> myPoints, Point emptyPoint)
        {
            var ownFields = new List<Point>(myPoints);

            var random1 = Random.Next(ownFields.Count);
            var point1 = ownFields[random1];
            ownFields.RemoveAt(random1);

            var random2 = Random.Next(ownFields.Count);
            var point2 = ownFields[random2];

            return new BirthMove(emptyPoint, point1, point2);
        }
    }
}
