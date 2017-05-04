using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.CardCompare
{
    public class CompPlaneOneWingMore : CompareBase
    {
        public override CardCompareResult GetCardCompareResult(int[] cardArray1, int[] cardArray2)
        {
            return GetPlaneResult(cardArray1, cardArray2);//有漏洞需要修改，因为可能存在3+3+3+1+1+1 的所有1都相同的情况。
        }
    }
}
