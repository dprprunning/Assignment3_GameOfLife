using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment3_GameOfLife.Pages
{
    public class GameModel : PageModel
    {
        public int currentState;
        public bool[,,] f = new bool[1, 10, 10];
        // Array f is the game board.
        public int[,] around = new int[8, 2] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

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
                    // i and j are loops of coordinates.
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
            currentState = 1 - currentState;
            Thread.Sleep(1000);
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
    }
}