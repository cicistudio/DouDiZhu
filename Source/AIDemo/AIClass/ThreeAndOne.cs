using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIDemo.AIClass
{
    public class ThreeAndOne : AIBase
    {
        public override int[] GetOutPutCard(OutPutCardInfo info)
        {
            if (AIOptions.CurrentCardArray.Count < 4)
            {
                return null;
            }
            List<int> outPutKinds = base.GetKindCollection(info.CardArray, 3);
            List<int> threeKinds = base.GetKindCollection(AIOptions.CurrentCardArray, 3);
            List<int> singleKinds = base.GetSingleKindCollection(AIOptions.CurrentCardArray);
            if (threeKinds.Count == 0)
            {
                return null;
            }
            
            var query = from c in threeKinds
                        where c > outPutKinds[0]
                        orderby c
                        select c;
            if (query.Count() > 0)
            {
                if (singleKinds.Count > 0)
                {
                    return new int[] { query.First(), query.First(), query.First(), singleKinds[0] };
                }
                else
                {
                    //拆牌来打
                    var q = from int cc in AIOptions.CurrentCardArray
                            where cc != query.First()
                            orderby cc
                            select cc;
                    return new[] { query.First(), query.First(), query.First(), q.First() };
                }
            }
            return null;
        }
    }
}
