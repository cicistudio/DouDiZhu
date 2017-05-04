using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.RuleClass
{
    /// <summary>
    /// 检查是否符合成对出牌规则，一对，或者大于3对连出，才符合此规则。
    /// </summary>
    public class RuleGroup : RuleBase
    {
        public override RuleType GetRuleType(int[] cardArray)
        {
            //如果是奇数就说明不符合此规则
            if (cardArray.Length % 2 != 0)
            {
                return RuleType.OutOfRule;
            }

            if (cardArray.Length == 2)
            {
                //如果是一对就说明符合此规则
                return cardArray[0] == cardArray[1] ? RuleType.TwoGroup : RuleType.OutOfRule;
            }
            else
            {
                foreach (int i in cardArray)
                {
                    var query = from c in cardArray
                                where c == i
                                select c;
                    if (query.Count() != 2)//如果不成对规则不成立
                    {
                        return RuleType.OutOfRule;
                    }
                }
                //能走到这一步说明肯定都成对，那么只要判断是否连续就行了。
                return base.CheckContinue(cardArray, 3) ? RuleType.ThreeGroupMore : RuleType.OutOfRule;
            }
        }
    }
}
