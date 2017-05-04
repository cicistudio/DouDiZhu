using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIFrameWork;
using System.Collections;

namespace AIDemo
{
    public class AIOptions
    {
        private static ArrayList m_CurrentCardArray = new ArrayList();

        /// <summary>
        /// 当手中的牌。
        /// </summary>
        public static ArrayList CurrentCardArray
        {
            get { return AIOptions.m_CurrentCardArray; }
            set { AIOptions.m_CurrentCardArray = value; }
        }

        /// <summary>
        /// 当前回合的地主是谁
        /// </summary>
        public static CardPlayerType CurrentHost { get; set; }

        /// <summary>
        /// 当前AI坐在哪个位置上
        /// </summary>
        public static CardPlayerType CurrentLocation { get; set; }

        /// <summary>
        /// 判断地主是不是自己
        /// </summary>
        public static Boolean IsHostMySelf
        {
            get { return ((AIOptions.CurrentHost == AIOptions.CurrentLocation)  && (AIOptions.CurrentHost != CardPlayerType.NoPlayer)); }
        }

        private static Stack<OutPutCardInfo> m_OutPutCardStackInOneCycle = new Stack<OutPutCardInfo>();
        /// <summary>
        /// 用于记录每一回合出过的牌。
        /// </summary>
        public static Stack<OutPutCardInfo> OutPutCardStackInOneCycle
        {
            get { return AIOptions.m_OutPutCardStackInOneCycle; }
            set { AIOptions.m_OutPutCardStackInOneCycle = value; }
        }

        private static ArrayList m_HostOutPutCardArray = new ArrayList();
        /// <summary>
        /// 记录地主出过的牌。
        /// </summary>
        public static ArrayList HostOutPutCardArray
        {
            get { return AIOptions.m_HostOutPutCardArray; }
            set { AIOptions.m_HostOutPutCardArray = value; }
        }
    }
}
