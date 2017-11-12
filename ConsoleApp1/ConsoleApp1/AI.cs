using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class AI
    {
        public int[,] AIMove(int[,] board, int turn, int AIplayer)
        {
            //coordinates for move to be passed to game
            int[,] move = { { 0, 0 }, { 0, 0 } };

            //1) Examine each possible AI move
            int[] orig = { 0, 0, 0 };
            int[] dest = { 0, 0, 0 };
            Dictionary<int, int[,]> movePossible = new Dictionary<int, int[,]>();
            List<int[]> origPossible = new List<int[]>();
            List<int[]> destPossible = new List<int[]>();
            //iterate through the board for origin and destination coordinates
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y] % 2 == AIplayer % 2)
                    {
                        orig[0] = x;
                        orig[1] = y;
                        orig[2] = board[x, y];
                        origPossible.Add(orig);
                    }
                    if (board[x, y] % 2 != AIplayer % 2 || board[x, y] == 0)
                    {
                        dest[0] = x;
                        dest[1] = y;
                        dest[2] = board[x, y];
                        destPossible.Add(dest);
                    }
                }
            }
            //itterate through origin/destination lists to search for valid moves
            for (var i = 0; i < origPossible.Count; i++)
            {
                for (var j = 0; j < destPossible.Count; j++)
                {
                    if (Game.ValidMove(orig[2], dest[2], origPossible[i], destPossible[j], turn) == true)
                    {
                        int weight = 0;
                        if (orig[2] == 1 || orig[2] == 3)
                        {
                            if (dest[2] == 2 || dest[2] == 4)
                            {
                                weight = 2;
                            }
                            else
                            {
                                weight = 1;
                            }
                        }
                        if (orig[2] == 3 || orig[2] == 4)
                        {
                            if (dest[2] == 1 || dest[2] == 3)
                            {
                                weight = -2;
                            }
                            else
                            {
                                weight = -1;
                            }
                        }
                        move[0, 0] = orig[0];
                        move[0, 1] = orig[1];
                        move[1, 0] = dest[0];
                        move[1, 1] = dest[1];
                        movePossible.Add(weight, move);
                    }
                }
            }
            //itterate through possible moves and select the best score by weight

            foreach (KeyValuePair<int, int[,]> pair in movePossible)
            {
                if (movePossible.ContainsKey(turn - 1) == true)
                {
                    board = (int[,])moveList[states[0]].Clone();
                    turn = states[0];
                    player1score = states[1];
                    player2score = states[2];
                }
            }
            return move;
        }
    }
}
