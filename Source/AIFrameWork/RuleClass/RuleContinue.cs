using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.RuleClass
{
    /// <summary>
    /// 5张或5张以上单牌的连续打法,不能将王和2算入近来，基类已经封装
    /// </summary>
    public class RuleContinue:RuleBase
    {
        public override RuleType GetRuleType(int[] cardArray)
        {
            foreach (int i in cardArray)
            {
                var query = from c in cardArray
                            where c == i
                            select i;
                if (query.Count() !=1)
                {
                    return RuleType.OutOfRule;//必须为单牌
                }
            }

            return base.CheckContinue(cardArray, 5) ? RuleType.FiveAndMore : RuleType.OutOfRule;
        }
    }
}
