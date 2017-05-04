using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.CardCompare
{
    public class CompFiveAndMore : CompareBase
    {
        public override CardCompareResult GetCardCompareResult(int[] cardArray1, int[] cardArray2)
        {
            var q1 = from c1 in cardArray1
                     orderby c1
                     select c1;
            var q2 = from c2 in cardArray2
                     orderby c2
                     select c2;
            if (q2.Last() > q1.Last())
            {
                return CardCompareResult.ParamOneIsSmaller;
            }
            else if (q2.Last() < q1.Last())
            {
                return CardCompareResult.ParamOneIsBigger;
            }
            else
            {
                return CardCompareResult.ParamOneAndTwoEqual;
            }
        }
    }
}
