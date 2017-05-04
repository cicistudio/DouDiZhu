using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.CardCompare
{
    public class CompFourAndTwo : CompareBase
    {
        public override CardCompareResult GetCardCompareResult(int[] cardArray1, int[] cardArray2)
        {
            int card1 = 0;
            int card2 = 0;
            foreach(int i in cardArray1)
            {
                var q1 = from c1 in cardArray1
                         where c1 == i
                         select c1;
                if (q1.Count() == 4)
                {
                    card1 = i;
                    break;
                }
            }

            foreach (int i in cardArray2)
            {
                var q2 = from c2 in cardArray2
                         where c2 == i
                         select c2;
                if (q2.Count() == 4)
                {
                    card2 = i;
                    break;
                }
            }

            if (card1 > card2)
            {
                return CardCompareResult.ParamOneIsBigger;
            }
            else if (card1 < card2)
            {
                return CardCompareResult.ParamOneIsSmaller;
            }
            else
            {
                return CardCompareResult.ParamOneAndTwoEqual;
            }

        }
    }
}
