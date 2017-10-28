namespace Hackathon
{
    using System;
    using Move;

    public class Path
    {
        public Game.Celltype[,] Board { get; set; }
        private Game.Celltype[,] NewBoard { get; set; }

        public Path(Game.Celltype[,] board, BaseMove initialMove)
        {
            Board = board.Clone() as Game.Celltype[,];

            if (initialMove is KillMove)
            {
                var killMove = (KillMove) initialMove;
                Board[killMove.Point.X, killMove.Point.Y] = Game.Celltype.Dead;
            }
            else if (initialMove is BirthMove)
            {
                var birthMove = (BirthMove) initialMove;
                Board[birthMove.Kill1Point.X, birthMove.Kill1Point.Y] = Game.Celltype.Dead;
                Board[birthMove.Kill2Point.X, birthMove.Kill2Point.Y] = Game.Celltype.Dead;
                Board[birthMove.EmptyPoint.X, birthMove.EmptyPoint.Y] = (Game.Celltype)Settings.MyBotId;
            }
        }

        public void Tick()
        {
            NewBoard = Board.Clone() as Game.Celltype[,];

            for (var x = 0; x < Settings.FieldWidth; x++)
            {
                for (var y = 0; y < Settings.FieldHeight; y++)
                {
                    int[] neighborCount = CountNeighbors(x, y);
                    var neighborSum = neighborCount[0] + neighborCount[1];

                    if (IsAlive(x, y) && (neighborSum < 2 || neighborSum > 3))
                    { // dies
                        KillField(x, y);
                    }
                    else if (!IsAlive(x, y) && neighborSum == 3)
                    {  // born
                        var playerId = neighborCount[0] > neighborCount[1] ? 0 : 1;
                        BirthField(x, y, playerId);
                    }
                }
            }

            Board = NewBoard;
        }

        public int GetScore()
        {
            var score = 0;

            for (var x = 0; x < Settings.FieldWidth; x++)
            {
                for (var y = 0; y < Settings.FieldHeight; y++)
                {
                    var field = Board[x, y];
                    if (field == Game.Celltype.Dead)
                        continue;

                    score += (int) field == Settings.MyBotId ? 1 : -1;
                }
            }

            return score;
        }

        private int[] CountNeighbors(int x, int y)
        {
            int[] neighbors = new int[2];

            int minX = Math.Max(0, x - 1), maxX = Math.Min(x + 1, Settings.FieldWidth - 1);
            int minY = Math.Max(0, y - 1), maxY = Math.Min(y + 1, Settings.FieldHeight - 1);

            for (int dy = minY; dy <= maxY; dy++)
            {
                for (int dx = minX; dx <= maxX; dx++)
                {
                    if ((dx == x && dy == y) || Board[dx, dy] == Game.Celltype.Dead)
                        continue;

                    neighbors[(int) Board[dx, dy]]++;
                }
            }

            return neighbors;
        }

        private bool IsAlive(int x, int y)
        {
            return Board[x, y] != Game.Celltype.Dead;
        }

        private void KillField(int x, int y)
        {
            NewBoard[x, y] = Game.Celltype.Dead;
        }

        private void BirthField(int x, int y, int playerId)
        {
            NewBoard[x, y] = (Game.Celltype)playerId;
        }
    }
}
