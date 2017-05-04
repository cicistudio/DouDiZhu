using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIDemo.ErrorClass
{
    public class FiveContinueExeception: Exception
    {
        public override string Message
        {
            get
            {
                return "获取5张连牌时出错。" + base.Message;
            }
        }
    }
}
