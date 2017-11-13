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

            while (moveFound == false)
            {
               
                    int boardPiece = board[boardRow, boardColumn];

                    //pick
                    if (holding == 0)
                    {
                        if ((turn % 2 == 0) && (boardPiece % 2 == 0))
                        {
                            // set origin Black
                            hop = true;// required to stop game turn updating on keypress
                            origXY[0] = boardRow;
                            origXY[1] = boardColumn;
                            holding = boardPiece;
                            board[boardRow, boardColumn] = 0;
                            Draw.DrawPieces(board);

                        }
                        else
                        if ((turn % 2 != 0) && (boardPiece % 2 != 0))
                        {
                            //set origin Red
                            hop = true;
                            origXY[0] = boardRow;
                            origXY[1] = boardColumn;
                            holding = boardPiece;
                            board[boardRow, boardColumn] = 0;
                            Draw.DrawPieces(board);

                        }
                    }
                    //place
                    else
                    if (holding != 0)
                    {
                        //set the destination
                        destXY[0] = boardRow;
                        destXY[1] = boardColumn;
                        // placing the piece
                        if ((Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true) && board[boardRow, boardColumn] == 0)
                        {
                            // king move

                            if ((holding == 1 && boardRow == 7) || (holding == 2 && boardRow == 0))
                            {
                                board[boardRow, boardColumn] = holding + 2;
                                holding = 0;
                                Draw.DrawPieces(board);
                                if (origXY[0] != destXY[0])
                                {
                                    hop = false;
                                }
                            }
                            // standard move
                            else
                            {
                                board[boardRow, boardColumn] = holding;
                                holding = 0;
                                Draw.DrawPieces(board);
                                if (origXY[0] != destXY[0])
                                {
                                    hop = false;
                                }
                            }
                        }
                        // taking a piece
                        else
                        if (Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true
                                && boardPiece != 0)
                        {
                            int spaceX = (destXY[0] + (destXY[0] - origXY[0]));
                            int spaceY = (destXY[1] + (destXY[1] - origXY[1]));

                            /* stops spaceX & spaceY from running of the ends of the array */
                            if (spaceX < 0 || spaceX > 7)
                            {
                                spaceX = destXY[0];
                            }
                            if (spaceY < 0 || spaceY > 7)
                            {
                                spaceY = destXY[1];
                            }

                            if (board[spaceX, spaceY] == 0)
                            {

                                if ((holding == 1 && spaceX == 7) || (holding == 2 && spaceX == 0))
                                {

                                    if (holding == 1)
                                    {
                                        player1score++;
                                    }
                                    else
                                    if (holding == 2)
                                    {
                                        player2score++;
                                    }

                                    holding = holding + 2;
                                    board[spaceX, spaceY] = holding;
                                    board[boardRow, boardColumn] = 0;
                                    hop = false;
                                    board[origXY[0], origXY[1]] = 0;
                                    origXY[0] = spaceX;
                                    origXY[1] = spaceY;

                                    Draw.DrawPieces(board);
                                }
                                else
                                {
                                    int adderA = 0;
                                    int adderB = 0;
                                    board[origXY[0], origXY[1]] = 0;
                                    board[spaceX, spaceY] = holding;
                                    board[boardRow, boardColumn] = 0;
                                    switch (holding)
                                    {
                                        case 1:
                                            adderA = 1;
                                            adderB = 1;
                                            player1score++;
                                            break;
                                        case 2:
                                            adderA = -1;
                                            adderB = -1;
                                            player2score++;
                                            break;
                                        case 3:
                                            player1score++;
                                            adderA = 1;
                                            adderB = -1;
                                            break;
                                        case 4:
                                            player2score++;
                                            adderA = -1;
                                            adderB = 1;
                                            break;
                                    }

                                    if ((spaceY == 0 || spaceY == 1) && spaceX >= 2 && spaceX <= 5)
                                    {
                                        if (((board[spaceX + adderA, spaceY + 1] != 0 && board[spaceX + adderA, spaceY + 1] % 2 != holding % 2) && board[spaceX + (adderA + adderA), spaceY + 2] == 0)
                                         || ((board[spaceX + adderB, spaceY + 1] != 0 && board[spaceX + adderB, spaceY + 1] % 2 != holding % 2) && board[spaceX + (adderB + adderB), spaceY + 2] == 0))
                                        {
                                            origXY[0] = spaceX;
                                            origXY[1] = spaceY;

                                            hop = true;
                                        }
                                        else
                                        {
                                            hop = false;
                                            holding = 0;
                                        }
                                    }
                                    else
                                    if ((spaceY == 6 || spaceY == 7) && spaceX >= 2 && spaceX <= 5)
                                    {
                                        if (board[spaceX + adderA, spaceY - 1] != 0 && board[spaceX + adderA, spaceY - 1] % 2 != holding % 2 && board[spaceX + (adderA + adderA), spaceY - 2] == 0
                                         || board[spaceX + adderB, spaceY - 1] != 0 && board[spaceX + adderB, spaceY - 1] % 2 != holding % 2 && board[spaceX + (adderB + adderB), spaceY - 2] == 0)
                                        {
                                            origXY[0] = spaceX;
                                            origXY[1] = spaceY;

                                            hop = true;
                                        }
                                        else
                                        {
                                            hop = false;
                                            holding = 0;
                                        }
                                    }
                                    else
                                    if ((spaceY == 2 || spaceY == 3 || spaceY == 4 || spaceY == 5) && spaceX >= 2 && spaceX <= 5)
                                    {
                                        if ((board[spaceX + adderA, spaceY - 1] != 0 && board[spaceX + adderA, spaceY - 1] % 2 != holding % 2 && board[spaceX + (adderA + adderA), spaceY - 2] == 0)
                                         || (board[spaceX + adderB, spaceY - 1] != 0 && board[spaceX + adderB, spaceY - 1] % 2 != holding % 2 && board[spaceX + (adderB + adderB), spaceY - 2] == 0)
                                         || (board[spaceX + adderA, spaceY + 1] != 0 && board[spaceX + adderA, spaceY + 1] % 2 != holding % 2 && board[spaceX + (adderA + adderA), spaceY + 2] == 0)
                                         || (board[spaceX + adderB, spaceY + 1] != 0 && board[spaceX + adderB, spaceY + 1] % 2 != holding % 2 && board[spaceX + (adderB + adderB), spaceY + 2] == 0))
                                        {
                                            origXY[0] = spaceX;
                                            origXY[1] = spaceY;

                                            hop = true;
                                        }
                                        else
                                        {
                                            hop = false;
                                            holding = 0;
                                        }
                                    }

                                    // hop checks for board extremes
                                    else
                                    if (spaceX == 0 || spaceX == 1)
                                    {
                                        if (spaceY == 0 || spaceY == 1)
                                        {
                                            if (board[spaceX + 1, spaceY + 1] != 0 && board[spaceX + 1, spaceY + 1] % 2 != holding % 2 && board[spaceX + 2, spaceY + 2] == 0)
                                            {
                                                origXY[0] = spaceX;
                                                origXY[1] = spaceY;

                                                hop = true;
                                            }
                                            else
                                            {
                                                hop = false;
                                                holding = 0;
                                            }
                                        }
                                        else
                                        if (spaceY == 6 || spaceY == 7)
                                        {
                                            if (board[spaceX + 1, spaceY - 1] != 0 && board[spaceX + 1, spaceY - 1] % 2 != holding % 2 && board[spaceX + 2, spaceY - 2] == 0)
                                            {
                                                origXY[0] = spaceX;
                                                origXY[1] = spaceY;

                                                hop = true;
                                            }
                                            else
                                            {
                                                hop = false;
                                                holding = 0;
                                            }
                                        }
                                        else
                                        {
                                            hop = false;
                                            holding = 0;
                                        }
                                    }
                                    else
                                    if (spaceX == 6 || spaceX == 7)
                                    {
                                        if (spaceY == 0 || spaceY == 1)
                                        {
                                            if (board[spaceX - 1, spaceY + 1] != 0 && board[spaceX - 1, spaceY + 1] % 2 != holding % 2 && board[spaceX - 2, spaceY + 2] == 0)
                                            {
                                                origXY[0] = spaceX;
                                                origXY[1] = spaceY;

                                                hop = true;
                                            }
                                            else
                                            {
                                                hop = false;
                                                holding = 0;
                                            }
                                        }
                                        else
                                        if (spaceY == 6 || spaceY == 7)
                                        {
                                            if (board[spaceX - 1, spaceY - 1] != 0 && board[spaceX - 1, spaceY - 1] % 2 != holding % 2 && board[spaceX - 2, spaceY - 2] == 0)
                                            {
                                                origXY[0] = spaceX;
                                                origXY[1] = spaceY;

                                                hop = true;
                                            }
                                            else
                                            {
                                                hop = false;
                                                holding = 0;
                                            }
                                        }
                                        else
                                        {
                                            hop = false;
                                            holding = 0;
                                        }
                                    }
                                    else
                                    {
                                        hop = false;
                                        holding = 0;
                                    }
                                    Draw.DrawPieces(board);
                                }
                            }
                        }

                        if (hop == false)
                        {
                            holding = 0;
                            turn++;
                            if (moveList.ContainsKey(turn) != true)
                            {
                                moveList.Add(turn, (int[,])board.Clone());
                            }
                            moveList[turn] = (int[,])board.Clone();
                        }
                    }

                    Draw.WriteScores(player1score, player2score, turn);
                
            }

            return board;
        }

        public static void Thinking(int wait)
        {
            System.Threading.Thread.Sleep(wait * 1000);
        }
    }
}
