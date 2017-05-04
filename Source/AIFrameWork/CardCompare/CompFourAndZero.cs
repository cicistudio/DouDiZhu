using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.CardCompare
{
    public class CompFourAndZero : CompareBase
    {
        public override CardCompareResult GetCardCompareResult(int[] cardArray1, int[] cardArray2)
        {
            if (this.Rule1 == RuleType.FourAndZero && this.Rule2 != RuleType.FourAndZero)
            {
                return CardCompareResult.ParamOneIsBigger;
            }
            else if (this.Rule1 != RuleType.FourAndZero && this.Rule2 == RuleType.FourAndZero)
            {
                return CardCompareResult.ParamOneIsSmaller;
            }
            else
            {
                if (cardArray1[0] > cardArray2[0])
                {
                    return CardCompareResult.ParamOneIsBigger;
                }
                else if (cardArray1[0] < cardArray2[0])
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
}
