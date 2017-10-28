namespace Hackathon
{
    using System;
    using Move;

    public class Path
    {
        public Game.Celltype[,] Board { get; set; }
        private Game.Celltype[,] NewBoard { get; set; }
        private int[] LivingCells { get; set; }

        public Path(Game.Celltype[,] board, int[] livingCells, BaseMove initialMove)
        {
            LivingCells = new[] {livingCells[0], livingCells[1]};
            Board = board.Clone() as Game.Celltype[,];

            if (initialMove is KillMove)
            {
                var killMove = (KillMove) initialMove;
                var killedId = (int)Board[killMove.Point.X, killMove.Point.Y];
                Board[killMove.Point.X, killMove.Point.Y] = Game.Celltype.Dead;

                LivingCells[killedId] = LivingCells[killedId] - 1;
            }
            else if (initialMove is BirthMove)
            {
                var birthMove = (BirthMove) initialMove;
                Board[birthMove.Kill1Point.X, birthMove.Kill1Point.Y] = Game.Celltype.Dead;
                Board[birthMove.Kill2Point.X, birthMove.Kill2Point.Y] = Game.Celltype.Dead;
                Board[birthMove.EmptyPoint.X, birthMove.EmptyPoint.Y] = (Game.Celltype)Settings.MyBotId;
                LivingCells[Settings.MyBotId] = LivingCells[Settings.MyBotId] - 1;
            }
        }

        public bool Tick()
        {
            if (LivingCells[Settings.MyBotId] == 0 || LivingCells[Settings.EnemyBotId] == 0)
                return false;

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
            return true;
        }

        public int GetScore()
        {
            if (LivingCells[Settings.MyBotId] == 0)
                return -75;


            if (LivingCells[Settings.EnemyBotId] == 0)
                return 75;

            return LivingCells[Settings.MyBotId] - LivingCells[Settings.EnemyBotId];
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
            var playerId = (int)NewBoard[x, y];
            LivingCells[playerId] = LivingCells[playerId] - 1;

            NewBoard[x, y] = Game.Celltype.Dead;
        }

        private void BirthField(int x, int y, int playerId)
        {
            LivingCells[playerId] = LivingCells[playerId] + 1;
            NewBoard[x, y] = (Game.Celltype)playerId;
        }
    }
}
