using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.RuleClass
{
    /// <summary>
    /// 检查有三张一样的牌的出牌规则，3带0， 3带1，3带2
    /// </summary>
    public class RuleThree: RuleBase
    {
        public override RuleType GetRuleType(int[] cardArray)
        {
            if (cardArray.Length < 3 || cardArray.Length > 5)
            {
                return RuleType.OutOfRule;
            }

            if (cardArray.Length == 4 || cardArray.Length == 3)
            {
                //3带0，3带1
                foreach(int i in cardArray)
                {
                    var query = from c in cardArray
                                where c == i
                                select c;
                    if (query.Count() == 3)
                    {
                        return cardArray.Length == 3 ? RuleType.ThreeAndZero : RuleType.ThreeAndOne; //如果在这4张牌中有3个是相等的，那么一定符合规则
                    }

                }
                return RuleType.OutOfRule;

            }
            else
            {
                //5张牌的情况
                foreach (int i in cardArray)
                {
                     var query = from c in cardArray
                                where c == i
                                select c;
                     if (query.Count() == 3)
                     {
                         //如果有3张牌相等，那么剩下的2张牌必须相等。
                         var q = from cc in cardArray
                                 where cc != query.First()
                                 select cc;
                         return q.First() == q.Last() ? RuleType.ThreeAndTwo : RuleType.OutOfRule;
                     }
                }
                return RuleType.OutOfRule;
            }


        }
    }
}
