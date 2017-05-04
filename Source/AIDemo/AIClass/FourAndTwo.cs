using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIDemo.AIClass
{
    public class FourAndTwo:AIBase
    {
        public override int[] GetOutPutCard(OutPutCardInfo info)
        {
            if (AIOptions.CurrentCardArray.Count < 6)
            {
                return null;
            }
            List<int> fourKinds = base.GetKindCollection(AIOptions.CurrentCardArray, 4);
            var query = from c in fourKinds
                        where c > info.CardArray[0]
                        orderby c
                        select c;
            if (query.Count() > 0)
            {
                List<int> singleCollection = base.GetSingleKindCollection(AIOptions.CurrentCardArray);
                if (singleCollection.Count >= 2)
                {
                    return new int[] { query.First(), query.First(), query.First(), query.First(), singleCollection[0], singleCollection[1] };
                }
                else
                {
                    var q = from int cc in AIOptions.CurrentCardArray
                            where cc != fourKinds[0]
                            orderby cc
                            select cc;
                    return new int[] { query.First(), query.First(), query.First(), query.First(), q.First(), q.ToList()[1] };
                }
            }
            return null;
        }
    }
}
