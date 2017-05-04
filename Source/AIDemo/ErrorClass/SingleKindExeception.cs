using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIDemo.ErrorClass
{
    public class SingleKindExeception :Exception
    {
        public override string Message
        {
            get
            {
                return "检测5张单牌时出错" + base.Message;
            }
        }
    }
}
