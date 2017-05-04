using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIDemo.AIClass
{
    public class ThreeAndZero : AIBase
    {
        public override int[] GetOutPutCard(OutPutCardInfo info)
        {
            List<int> threeKinds = base.GetKindCollection(AIOptions.CurrentCardArray, 3);
            var query = from c in threeKinds
                        where c > info.CardArray.Max()
                        orderby c
                        select c;
            if (query.Count() > 0)
            {
                return new int[] { query.First(), query.First(), query.First() };
            }
            else
            {
                return null;
            }
        }
    }
}
