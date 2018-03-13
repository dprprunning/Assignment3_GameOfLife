using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment3_GameOfLife.GameLogic
{
    public class Game
    {
        public int currentState;
        public bool[,,] f = new bool[1, 10, 10];
        // Array f is the game board.
        public int[,] around = new int[8, 2] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };
        public GameHandler gh;
        private int[,] queue = new int[10000, 2];
        private int head = 0, tail = 0;

        public Game(GameHandler g)
        {
            gh = g;
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        public void OnGet()
        {
            currentState = 0;
            f[currentState, 3, 4] = true;
            f[currentState, 4, 5] = true;
            f[currentState, 5, 3] = true;
            f[currentState, 5, 4] = true;
            f[currentState, 5, 5] = true;
        }

        /// <summary>
        /// Check whether the coordinates are in the game board.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>True when valid, else False.</returns>
        private bool CheckBorder(int x, int y)
        {
            if (x < 0 || y < 0 || x >= 10 || y >= 10)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Based on the current state, decide whether each cell will survive in next state.
        /// </summary>
        public void NextState()
        {
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    // i and j are coordinates.
                    int count = 0;
                    //Count neighbors.
                    for(int k = 0; k < 8; k++)
                    {
                        // There are 8 neighbors around this cell.
                        // Check whether their coordinates are valid, and if valid, count alive cells.
                        if (CheckBorder(i + around[k, 0], j + around[k, 1]))
                        {
                            if (f[currentState, i + around[k, 0], j + around[k, 1]])
                            {
                                count++;
                            }
                        }
                    }
                    if (count == 2 || count == 3)
                    {
                        f[1 - currentState, i, j] = true;
                    }
                    else
                    {
                        f[1 - currentState, i, j] = false;
                    }
                }
            }
            int c = 0;
            while (head != tail && c < 10000)
            {
                c++;
                ChangeCell(queue[head, 0], queue[head, 1]);
            }
            Thread.Sleep(500);
            currentState = 1 - currentState;
            //return f[currentState];
        }

        /// <summary>
        /// This procedure deals with users' clicks.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public void ChangeCell(int x, int y)
        {
            if (CheckBorder(x, y))
            {
                f[currentState, x, y] = !f[currentState, x, y];
            }
            // Make sure that the coordinates are valid.
        }

        public string SendString()
        {
            string s = "";
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (f[currentState, i, j])
                    {
                        if (s != "")
                        {
                            s += ",";
                        }
                        s += (i * 10 + j).ToString();
                    }
                }
            }
            return s;
        }

        public void ReceiveString(string s)
        {
            string[] ids = s.Split(",");

            foreach(string ss in ids)
            {
                try
                {
                    int t = Int32.Parse(ss);
                    int y = t % 10;
                    int x = t / 10;
                    tail = (tail + 1) % 10000;
                    queue[tail, 0] = x;
                    queue[tail, 1] = y;
                    //Add the coordinates to queue.
                }
                catch (Exception e)
                {
                    //Ignore bad numbers.
                }
            }
        }
    }
}
