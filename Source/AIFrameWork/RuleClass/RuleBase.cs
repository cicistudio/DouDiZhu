using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIFrameWork.RuleClass
{
    public abstract class RuleBase
    {
        public abstract RuleType GetRuleType(int[] cardArray);

        /// <summary>
        /// 用于检测去掉重复的牌后是否连续
        /// </summary>
        /// <param name="cardArray">牌的数组形式</param>
        /// <param name="continCount">必须至少连续多少张</param>
        /// <returns></returns>
        public virtual bool CheckContinue(int [] cardArray,int continCount)
        {
            var query = (from c in cardArray
                         where c < 15  //不能为大小王和2
                         orderby c   
                         select c
                         ).Distinct();
            if (query.Count() < continCount)
            {
                return false;
            }

            /********判断是否连续********/
            return CheckContinue(query);
        }

        private bool CheckContinue(IEnumerable<int> query)
        {
            int min = query.First();
            int n = 0;
            foreach (int i in query)
            {
                if ((i - min) != n)
                {
                    return false;
                }
                n++;
            }
            return true;
        }

        public virtual bool CheckContinue(List<int> cardCollection, int continCount)
        {
            var query = from c in cardCollection
                        where c < 15  //不能为大小王和2
                        orderby c
                        select c;
                        
            if (query.Count() < continCount)
            {
                return false;
            }

            /********判断是否连续********/
            return CheckContinue(query);
        }
    }
}
