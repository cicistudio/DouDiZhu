using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.CardCompare
{
    public abstract class CompareBase
    {
        public RuleType Rule1 { get; set; }
        public RuleType Rule2 { get; set; }

        public abstract CardCompareResult GetCardCompareResult(int[] cardArray1, int[] cardArray2);

        protected CardCompareResult GetThreeResult(int[] cardArray1, int[] cardArray2)
        {
            int card1 = 0;
            int card2 = 0;
            foreach (int i in cardArray1)
            {
                var q1 = from c1 in cardArray1
                         where c1 == i
                         select c1;
                if (q1.Count() == 3)
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
                if (q2.Count() == 3)
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

        protected CardCompareResult GetPlaneResult(int[] cardArray1, int[] cardArray2)
        {
            int max1 = 0;
            int max2 = 0;
            foreach (int i in cardArray1)
            {
                var q = from c in cardArray1
                        where c == i
                        select c;
                if (q.Count() == 3)
                {
                    if (max1 < q.First())
                    {
                        max1 = q.First();
                    }
                }
            }

            foreach (int i in cardArray2)
            {
                var q = from c in cardArray2
                        where c == i
                        select c;
                if (q.Count() == 3)
                {
                    if (max2 < q.First())
                    {
                        max2 = q.First();
                    }
                }
            }

            if (max1 > max2)
            {
                return CardCompareResult.ParamOneIsBigger;
            }
            else if (max1 < max2)
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
