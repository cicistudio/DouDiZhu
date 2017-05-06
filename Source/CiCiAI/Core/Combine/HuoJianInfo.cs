using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiCiAI.Core.Combine
{
    public class HuoJianInfo: CombineBaseInfo
    {
        public override CommClass.PokerCombineType CombineType
        {
            get { return CommClass.PokerCombineType.ZhaDan; }
        }

        public override int Socre
        {
            get { return 80; }
        }
    }
}
