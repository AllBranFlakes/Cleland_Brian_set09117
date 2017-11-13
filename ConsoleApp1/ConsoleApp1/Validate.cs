using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Validate
    {
        public static bool ValidMove(int holding, int boardPiece, int[] origXY, int[] destXY, int turn)
        {
            bool isValid = false;
            if (holding == 1 && boardPiece != holding)
            {
                if (((origXY[0] + 1) == destXY[0] && ((origXY[1] - 1) == destXY[1] || (origXY[1] + 1) == destXY[1]))
                    || (origXY[0] == destXY[0] && origXY[1] == destXY[1]))
                {
                    isValid = true;
                }
            }
            else
            if (holding == 2 && boardPiece != holding)
            {
                if (((origXY[0] - 1) == destXY[0] && ((origXY[1] - 1) == destXY[1] || (origXY[1] + 1) == destXY[1]))
                    || (origXY[0] == destXY[0] && origXY[1] == destXY[1]))
                {
                    isValid = true;
                }
            }
            else
            if ((holding == 3 || holding == 4) && boardPiece != holding && boardPiece != holding - 2)
            {
                if ((((origXY[0] - 1) == destXY[0]) || ((origXY[0] + 1) == destXY[0]))
                    && (((origXY[1] - 1) == destXY[1]) || ((origXY[1] + 1) == destXY[1]))
                    || ((origXY[0] == destXY[0]) && (origXY[1] == destXY[1])))
                {
                    isValid = true;
                }
            }

            return isValid;
        }
    }
}
