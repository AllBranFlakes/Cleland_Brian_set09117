using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class AI
    {
        public static int[] AIMove(int[,] board, int turn)
        {
            int[] origXY = { 0, 0 };
            int[] destXY = { 0, 0 };
            int score = 0;
            List<int[]> moveList = new List<int[]>();


            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (turn % 2 == 0)
                    {
                        if (board[x, y] == 2 || board[x, y] == 4)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    origXY[0] = x;
                                    origXY[1] = y;
                                    destXY[0] = j;
                                    destXY[1] = k;
                                    int holding = board[x, y];
                                    int boardPiece = board[j, k];
                                    int weight = holding + holding + boardPiece;
                                    if (weight == 6 || weight == 8)
                                    {
                                        weight = 1;
                                    }
                                    if (Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true && (score < weight) && (boardPiece != holding || boardPiece % 2 != holding % 2))
                                    {
                                        score = weight;
                                        int[] move = { 0, 0, 0, 0 };
                                        move[0] = origXY[0];
                                        move[1] = origXY[1];
                                        move[2] = destXY[0];
                                        move[3] = destXY[1];
                                        moveList.Add(move);
                                    }
                                }
                            }
                        }
                    }
                    else
                    if (turn % 2 != 0)
                    {
                        if (board[x, y] == 1 || board[x, y] == 3)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    origXY[0] = x;
                                    origXY[1] = y;
                                    destXY[0] = j;
                                    destXY[1] = k;
                                    int holding = board[x, y];
                                    int boardPiece = board[j, k];
                                    int weight = holding + holding + boardPiece;
                                    if (Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true && (score < weight) && (boardPiece != holding || (boardPiece % 2 != holding % 2 && boardPiece != 0)))
                                    {
                                        score = weight;
                                        int[] move = { 0, 0, 0, 0 };
                                        move[0] = origXY[0];
                                        move[1] = origXY[1];
                                        move[2] = destXY[0];
                                        move[3] = destXY[1];
                                        moveList.Add(move);

                                    }
                                }
                            }
                        }
                    }
                }
            }

            Random r = new Random();

            int rInt = r.Next(moveList.Count);
            int[] temp = { 0, 0, 0, 0 };
            if (moveList.Count != 0)
            {
                if (rInt != 0)
                {
                    temp = (int[])moveList[rInt].Clone();
                }
                else if (rInt == 0)
                {
                    temp = (int[])moveList[0].Clone();
                }
            }
            else
            {
                temp[0] = 0;
                temp[1] = 0;
                temp[2] = 0;
                temp[3] = 0;
            }

            return temp;
        }

        public static void Thinking(int wait)
        {
            System.Threading.Thread.Sleep(wait * 100);
        }
    }
}