﻿namespace Hackathon.Move
{
    using System.Linq;

    public abstract class BaseMove
    {
        private int Score { get; set; }
        
        public void Simulate(Game.Celltype[,] board, int[] livingCells)
        {
            var paths = BuildPaths(board, livingCells);
            for (int x = 0; x < paths.Length; x++)
            {
                for (int y = 0; y < Game.PathDepth; y++)
                {
                    if (!paths[x].Tick())
                    {
                        Score = paths.Sum(p => p.GetScore());
                        return;
                    }

                }
            }

            Score = paths.Sum(p => p.GetScore());
        }

        public int GetScore()
        {
            return Score;
        }

        protected Path[] BuildPaths(Game.Celltype[,] board, int[] livingCells)
        {
            var paths = new Path[Game.PathCount];
            for(int x = 0; x < Game.PathCount; x++)
                paths[x] = new Path(board, livingCells, this);

            return paths;
        }
    }
}
