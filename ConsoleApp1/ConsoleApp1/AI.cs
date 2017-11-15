using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class AI
    {
        public static int[] AIMove(int[,] board, int turn)
        {
            int[] origXY = { 0, 0 };
            int[] destXY = { 0, 0 };
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

                                    if (Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true && (boardPiece != holding || (boardPiece % 2 != holding % 2 && boardPiece != 0)))
                                    {
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

                                    if (Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true && (boardPiece != holding || (boardPiece % 2 != holding % 2 && boardPiece != 0)))
                                    {
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
/*
 // C++ program to find the next optimal move for
// a player
#include<bits/stdc++.h>
using namespace std;

struct Move
{
int row, col;
};

char player = 'x', opponent = 'o';

// This function returns true if there are moves
// remaining on the board. It returns false if
// there are no moves left to play.
bool isMovesLeft(char board[3][3])
{
for (int i = 0; i<3; i++)
for (int j = 0; j<3; j++)
    if (board[i][j]=='_')
        return true;
return false;
}

int evaluate(char b[3][3])
{
// Checking for Rows for X or O victory.
for (int row = 0; row<3; row++)
{
if (b[row][0]==b[row][1] &&
    b[row][1]==b[row][2])
{
    if (b[row][0]==player)
        return +10;
    else if (b[row][0]==opponent)
        return -10;
}
}

// Checking for Columns for X or O victory.
for (int col = 0; col<3; col++)
{
if (b[0][col]==b[1][col] &&
    b[1][col]==b[2][col])
{
    if (b[0][col]==player)
        return +10;

    else if (b[0][col]==opponent)
        return -10;
}
}

// Checking for Diagonals for X or O victory.
if (b[0][0]==b[1][1] && b[1][1]==b[2][2])
{
if (b[0][0]==player)
    return +10;
else if (b[0][0]==opponent)
    return -10;
}

if (b[0][2]==b[1][1] && b[1][1]==b[2][0])
{
if (b[0][2]==player)
    return +10;
else if (b[0][2]==opponent)
    return -10;
}

// Else if none of them have won then return 0
return 0;
}

// This is the minimax function. It considers all
// the possible ways the game can go and returns
// the value of the board
int minimax(char board[3][3], int depth, bool isMax)
{
int score = evaluate(board);

// If Maximizer has won the game return his/her
// evaluated score
if (score == 10)
return score;

// If Minimizer has won the game return his/her
// evaluated score
if (score == -10)
return score;

// If there are no more moves and no winner then
// it is a tie
if (isMovesLeft(board)==false)
return 0;

// If this maximizer's move
if (isMax)
{
int best = -1000;

// Traverse all cells
for (int i = 0; i<3; i++)
{
    for (int j = 0; j<3; j++)
    {
        // Check if cell is empty
        if (board[i][j]=='_')
        {
            // Make the move
            board[i][j] = player;

            // Call minimax recursively and choose
            // the maximum value
            best = max( best,
                minimax(board, depth+1, !isMax) );

            // Undo the move
            board[i][j] = '_';
        }
    }
}
return best;
}

// If this minimizer's move
else
{
int best = 1000;

// Traverse all cells
for (int i = 0; i<3; i++)
{
    for (int j = 0; j<3; j++)
    {
        // Check if cell is empty
        if (board[i][j]=='_')
        {
            // Make the move
            board[i][j] = opponent;

            // Call minimax recursively and choose
            // the minimum value
            best = min(best,
                minimax(board, depth+1, !isMax));

            // Undo the move
            board[i][j] = '_';
        }
    }
}
return best;
}
}

// This will return the best possible move for the player
Move findBestMove(char board[3][3])
{
int bestVal = -1000;
Move bestMove;
bestMove.row = -1;
bestMove.col = -1;

// Traverse all cells, evalutae minimax function for
// all empty cells. And return the cell with optimal
// value.
for (int i = 0; i<3; i++)
{
for (int j = 0; j<3; j++)
{
    // Check if celll is empty
    if (board[i][j]=='_')
    {
        // Make the move
        board[i][j] = player;

        // compute evaluation function for this
        // move.
        int moveVal = minimax(board, 0, false);

        // Undo the move
        board[i][j] = '_';

        // If the value of the current move is
        // more than the best value, then update
        // best/
        if (moveVal > bestVal)
        {
            bestMove.row = i;
            bestMove.col = j;
            bestVal = moveVal;
        }
    }
}
}

printf("The value of the best Move is : %dnn",
    bestVal);

return bestMove;
}

// Driver code
int main()
{
char board[3][3] =
{
{ 'x', 'o', 'x' },
{ 'o', 'o', 'x' },
{ '_', '_', '_' }
};

Move bestMove = findBestMove(board);

printf("The Optimal Move is :n");
printf("ROW: %d COL: %dnn", bestMove.row,
                        bestMove.col );
return 0;
}

* /
*/