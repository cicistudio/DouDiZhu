﻿using System;
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
    public class LianDuiInfo : CombineBaseInfo
    {
        public override CommClass.PokerCombineType CombineType
        {
            get { return CommClass.PokerCombineType.LianDui; }
        }
        public override int Socre
        {
            get { return 40; }
        }

        /// <summary>
        /// 多少连对
        /// </summary>
        public int Count
        { get; set; }
    }
}
