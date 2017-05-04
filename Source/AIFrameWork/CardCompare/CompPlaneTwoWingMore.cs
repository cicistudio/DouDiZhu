using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.CardCompare
{
    public class CompPlaneTwoWingMore : CompareBase
    {
        public override CardCompareResult GetCardCompareResult(int[] cardArray1, int[] cardArray2)
        {
            return GetPlaneResult(cardArray1, cardArray2);
        }
    }
}
