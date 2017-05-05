using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*************************************************************
 * Copyright by cicistudio 2000-2017
 * http://chengchen.cnblogs.com
 * https://github.com/cicistudio/
 * The Current code cannot be used in the Commercial software.
 * Mail to me if you have any queries. cicistudio@qq.com
 ************************************************************/
namespace CiCiAI.Core.Combine
{
    public abstract class  CombineBaseInfo
    {
        public string CombinePokerString { get; set; }
        public abstract CommClass.PokerCombineType CombineType { get; }

        public abstract int Socre { get; }
    }
}
