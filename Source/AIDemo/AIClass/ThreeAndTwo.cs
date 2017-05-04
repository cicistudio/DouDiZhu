using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIDemo.AIClass
{
    public class ThreeAndTwo : AIBase
    {
        public override int[] GetOutPutCard(OutPutCardInfo info)
        {
            List<int> outPutKinds = base.GetKindCollection(info.CardArray, 3);
            List<int> twoKinds = base.GetKindCollection(AIOptions.CurrentCardArray, 2);
            List<int> threeKinds = base.GetKindCollection(AIOptions.CurrentCardArray, 3);
            if (twoKinds.Count == 0 || threeKinds.Count == 0)
            {
                return null;
            }
            twoKinds.Sort();
            var query = from c in threeKinds
                        where c > outPutKinds[0]
                        orderby c
                        select c;
            if (query.Count() > 0)
            {
                return new int[] { query.First(), query.First(), query.First(), twoKinds[0], twoKinds[0] };
            }
            else
            {
                return null;
            }
        }
    }
}
