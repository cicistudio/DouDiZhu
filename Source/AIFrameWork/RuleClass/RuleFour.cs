using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.RuleClass
{
    /// <summary>
    /// 四张相同的牌的打法，4带0，4带2，且不能4带两张王。
    /// </summary>
    public class RuleFour : RuleBase
    {
        public override RuleType GetRuleType(int[] cardArray)
        {
            if (cardArray.Length != 4 && cardArray.Length !=6)
            {
                return RuleType.OutOfRule;
            }

            foreach (int i in cardArray)
            {
                var query = from c in cardArray
                            where c == i && c < 16
                            select c;
                if (query.Count() == 4)
                {
                    return cardArray.Length == 4 ? RuleType.FourAndZero : RuleType.FourAndTwo;
                }
            }
            return RuleType.OutOfRule;

        }
    }
}
