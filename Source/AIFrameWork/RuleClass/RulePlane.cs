
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.RuleClass
{
    //飞机(连续3+3以上，或者 3+3+1+1 3+3+2+2 3+3+3+1+1+1 3+3+3+2+2+2 )
    public class RulePlane : RuleBase
    {
        public override RuleType GetRuleType(int[] cardArray)
        {
            if (cardArray.Length < 6)
            {
                return RuleType.OutOfRule;
            }

            //将包含3张相同的牌统计出来。
            List<int> threeCollection = new List<int>();
            foreach (int i in cardArray)
            {
                var query1 = from c1 in cardArray
                             where c1 == i
                             select c1;
                if (query1.Count() == 3)
                {
                    if (!threeCollection.Contains(query1.First()))
                    {
                        threeCollection.Add(query1.First());
                    }
                }
            }

            //必须有两个或两个以上3张相同的牌，且连续
            if (threeCollection.Count < 2 || !base.CheckContinue(threeCollection,2))
            {
                return RuleType.OutOfRule;
            }

            //以下肯定都是连续，且包含两个以上的相同的3
            if (cardArray.Length == threeCollection.Count * 3)
            {
                return RuleType.PlaneNoWing;//如果全是 3+3+3
            }
            else if (cardArray.Length == 8 && threeCollection.Count ==2)
            {
                return RuleType.PlaneOneWing; // 3+3+1+1
            }
            else if (cardArray.Length == 12 && threeCollection.Count == 3)
            {
                return RuleType.PlaneOneWingMore; //3+3+3+1+1+1
            }
            else if (cardArray.Length == 10 && threeCollection.Count == 2)
            {
                //3+3+2+2 需要判断剩下的是不是相等的两对
                var query2 = from c2 in cardArray
                             where c2 != threeCollection[0] && c2 != threeCollection[1]
                             select c2;
                return CheckEqualGroup(query2) ? RuleType.PlaneTwoWing : RuleType.OutOfRule;
            }
            else if(cardArray.Length == 15 && threeCollection.Count ==3)
            {
                //3+3+3+2+2+2 需要判断剩下的是不是相等的三对
                var query2 = from c2 in cardArray
                             where c2 != threeCollection[0] && c2 != threeCollection[1] && c2!= threeCollection[2]
                             select c2;
                return CheckEqualGroup(query2) ? RuleType.PlaneTwoWingMore : RuleType.OutOfRule;
            }
            else
            {
                return RuleType.OutOfRule;
            }

        }

        private bool CheckEqualGroup(IEnumerable<int> query)
        {
            foreach (int i in query)
            {
                var q = from c in query
                        where c == i
                        select c;
                if (q.Count() != 2)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
