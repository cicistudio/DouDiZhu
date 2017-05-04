using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIDemo.AIClass
{
    public class Bomb : AIBase
    {
        public override int[] GetOutPutCard(OutPutCardInfo info)
        {
            if (20 - AIOptions.HostOutPutCardArray.Count > 2)
            {
                return null;
            }
            //如果地主剩余两张牌，就给地主致命一击。
            List<int> fourKinds = base.GetKindCollection(AIOptions.CurrentCardArray, 4);
            var query = from c in fourKinds
                        where c > info.CardArray[0]
                        orderby c
                        select c;
            if (query.Count() > 0)
            {
                return new int[] { query.First(), query.First(), query.First(), query.First() };
            }
            else
            {
                //再判断双王情况
                if (AIOptions.CurrentCardArray.Contains(16) && AIOptions.CurrentCardArray.Contains(17))
                {
                    return new int[] { 16, 17 };
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
