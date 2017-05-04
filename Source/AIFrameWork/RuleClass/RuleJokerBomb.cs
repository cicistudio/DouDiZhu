using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.RuleClass
{
    /// <summary>
    /// 检查是否是双王炸弹
    /// </summary>
    public class RuleJokerBomb : RuleBase
    {
        public override RuleType GetRuleType(int[] cardArray)
        {
            if (cardArray.Length != 2)
            {
                return RuleType.OutOfRule;
            }

            return (cardArray.Contains(16) && cardArray.Contains(17)) ? RuleType.JokersBomb : RuleType.OutOfRule;
        }
    }
}
