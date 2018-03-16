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
        private const int GAMEBOARD = 40;
        public bool[,,] f = new bool[2, GAMEBOARD, GAMEBOARD];
        // Array f is the game board.
        public int[,] around = new int[8, 2] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };
        public GameHandler gh;
        private int[,] queue = new int[10000, 2];
        private int head = 0, tail = 0;
        private bool emptyGameboard = false;

        public Game(GameHandler g)
        {
            gh = g;
            currentState = 0;
            f[currentState, 3, 4] = true;
            f[currentState, 4, 5] = true;
            f[currentState, 5, 3] = true;
            f[currentState, 5, 4] = true;
            f[currentState, 5, 5] = true;
            Thread th = new Thread(GameMain);
            th.Start();
        }

        private void GameMain()
        {
            while (true)
            {
                Task.Run(async () =>
                {
                    if(emptyGameboard == false)
                    {
                        await SendStringAsync();
                    }
                    NextState();
                });
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Check whether the coordinates are in the game board.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>True when valid, else False.</returns>
        private bool CheckBorder(int x, int y)
        {
            if (x < 0 || y < 0 || x >= GAMEBOARD || y >= GAMEBOARD)
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
            for(int i = 0; i < GAMEBOARD; i++)
            {
                for(int j = 0; j < GAMEBOARD; j++)
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
                    if (f[currentState, i, j])
                    {
                        if (count == 2 || count == 3)
                        {
                            f[1 - currentState, i, j] = true;
                        }
                        else
                        {
                            f[1 - currentState, i, j] = false;
                        }
                    }
                    else
                    {
                        if (count == 3)
                        {
                            f[1 - currentState, i, j] = true;
                        }
                        else
                        {
                            f[1 - currentState, i, j] = false;
                        }
                    }
                }
            }
            currentState = 1 - currentState;
            int c = 0;
            while (head != tail && c < 10000)
            {
                c++;
                ChangeCell(queue[head, 0], queue[head, 1]);
                head = (head + 1) % 10000;
            }
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

        public async Task SendStringAsync()
        {
            string s = "";
            for (int i = 0; i < GAMEBOARD; i++)
            {
                for (int j = 0; j < GAMEBOARD; j++)
                {
                    if (f[currentState, i, j])
                    {
                        if (s != "")
                        {
                            s += ",";
                        }
                        s += (i * GAMEBOARD + j).ToString();
                    }
                }
            }
            if(s == "")
            {
                emptyGameboard = true;
            }
            await gh.SendMessageToAllAsync(s);
        }

        public void ReceiveString(string s)
        {
            emptyGameboard = false;
            string[] ids = s.Split(",").Distinct().ToArray();

            foreach (string ss in ids)
            {
                if (Int32.TryParse(ss, out int t)) {
                    int y = t % GAMEBOARD;
                    int x = t / GAMEBOARD;
                    queue[tail, 0] = x;
                    queue[tail, 1] = y;
                    tail = (tail + 1) % 10000;
                    //Add the coordinates to queue.
                    emptyGameboard = false;
                }
            }
        }
    }
}
