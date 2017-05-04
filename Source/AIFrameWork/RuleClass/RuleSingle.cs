using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.RuleClass
{
    /// <summary>
    /// 检查是否符合单张牌的出牌规则
    /// </summary>
    public class RuleSingle : RuleBase
    {
        public override RuleType GetRuleType(int[] cardArray)
        {
            return cardArray.Length == 1 ? RuleType.Single : RuleType.OutOfRule;
        }
    }
}
