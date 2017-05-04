using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIDemo.AIClass
{
    public class FiveAndMore:AIBase
    {
        public override int[] GetOutPutCard(OutPutCardInfo info)
        {
            if (info.CardArray.Max() == 14)
            {
                return null;//如果到A，那么就肯定打不动了。
            }

            //如果是队友打的牌，那么就不出牌，这个已经在基类中实现了。
            List<List<int>> fiveKindCollectin = base.GetContinueKindCollection(AIOptions.CurrentCardArray);
            foreach (List<int> collection in fiveKindCollectin)
            {
               
                var query = from c in collection
                            where c > info.CardArray.Min()
                            orderby c
                            select c;
                if (query.Count() >= info.CardArray.Length)
                {
                    int[] outPutArray = new int[info.CardArray.Length];
                    for (int i =0; i < outPutArray.Length; i++)
                    {
                        outPutArray[i] = query.ToList()[i];
                    }
                    return outPutArray;
                }
            }
            return null;
        }
    }
}
