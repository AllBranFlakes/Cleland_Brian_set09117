using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class AI
    {
        public int score = 0;
        public int bestscore = 0;
        public int bestmove = 0;
        public int nextmove = 0;

        public int MakeMove(int depth)
        {
            int maxDepth = depth;
            negaMax(depth, maxDepth);
            return bestmove;
        }



        public int EvalGame(int[,] board, int turn) //calculates the score from all the pieces on the board
        {
            int score = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != 0)
                    {
                        score += EvalPiece(board[i, j], turn);
                    }
                }
            }

            return score;
        }

        public int EvalPiece(int piece, int turn)
        {
            int pieceScore = 0;
            if (piece%2 != turn%2)
            {
                pieceScore = 5;
            }
            return pieceScore;
        }

        private int negaMax(int depth, int maxDepth)
        {
           // if (depth <= 0)
           // {
           //     return EvalGame();
           // }

            int max = -200000000;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        for (int l = 0; l < 8; l++)
                        {
                            if (GenerateMove(i, j, k, l)) //generates all possible moves
                            {
                                //code to move the piece on the board
                                score = -negaMax(depth - 1, maxDepth);

                                if (score > max)
                                {
                                    max = score;

                                    if (depth == maxDepth)
                                    {
                                        bestmove = nextmove;
                                    }
                                }

                                //code to undo the move
                            }
                        }
                    }
                }
            }

            return max;
        }

        public bool GenerateMove(int i, int j, int k, int l)
        {
            // generate a move
            if (Game.ValidMove(1,1,[1,1],[1,1],1) == true) //if a legal move
            {
                return true;
            }

            return false;
        }

    }
}
