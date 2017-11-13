using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class AI
    {
        public static int[,] AIMove(int[,] board, int turn)
        {
            bool moveFound = false;
            int moveCount = 0;
            int[] origXY = { 0, 0 };
            int[] destXY = { 0, 0 };
            int[,] move = new int[2, 2];

            List<int[]> Origin = new List<int[]>();

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (turn % 2 == 0)
                    {
                        if (board[x, y] == 2 || board[x, y] == 4)
                        {
                            origXY[0] = x;
                            origXY[1] = y;
                            Origin.Add(origXY);
                        }
                    }
                    else
                    if (turn % 2 != 0)
                    {
                        if (board[x, y] == 1 || board[x, y] == 3)
                        {
                            origXY[0] = x;
                            origXY[1] = y;
                            Origin.Add(origXY);
                        }
                    }
                }
            }
            while (moveFound == false)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        foreach (int[] i in Origin)
                        {
                           
                            origXY = i;
                            destXY[0] = j;
                            destXY[1] = k;
                            int holding = board[origXY[0], origXY[1]];
                            int boardPiece = board[j, k];
                            if (Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true)
                            {
                                moveFound = true;
                                move[0, 0] = origXY[0];
                                move[0, 1] = origXY[1];
                                move[1, 0] = destXY[0];
                                move[1, 1] = destXY[1];
                            } 
                        }
                    }
                }
            }


            return move;
        }

        public static void Thinking(int wait)
        {
            System.Threading.Thread.Sleep(wait * 1000);
        }
    }
}
