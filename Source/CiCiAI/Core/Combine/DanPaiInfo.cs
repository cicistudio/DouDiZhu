using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiCiAI.Core.Combine
{
    /// <summary>
    /// 单牌
    /// </summary>
    public class DanPaiInfo:CombineBaseInfo
    {
        public override CommClass.PokerCombineType CombineType
        {
            get { return CommClass.PokerCombineType.NONE; }
        }

        public override int Socre
        {
            get { return -5; }
        }
    }
}
