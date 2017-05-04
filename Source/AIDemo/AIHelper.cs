using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using AIFrameWork;
using AIDemo.AIClass;
using System.Threading;

namespace AIDemo
{
    public class AIHelper
    {
        /// <summary>
        /// 清除本次的游戏数据
        /// </summary>
        public static void ClearGame()
        {
            AIOptions.CurrentCardArray.Clear();
            AIOptions.CurrentHost = CardPlayerType.NoPlayer;
            AIOptions.CurrentLocation = CardPlayerType.NoPlayer;
            AIOptions.OutPutCardStackInOneCycle.Clear();
            AIOptions.HostOutPutCardArray.Clear();
        }

        public static int[] GetOutPutCard(bool isCycleFirst)
        {
            if (isCycleFirst)
            {
                return GetCycleFirstCard();
            }
            else
            {
                try
                {
                    OutPutCardInfo info = AIOptions.OutPutCardStackInOneCycle.Pop();//获取上一个玩家以及出牌信息
                    RuleType ruleType = RuleHelper.GetRuleType(info.CardArray);//获取上一个玩家打出牌的类型
                    Type[] types = Assembly.GetExecutingAssembly().GetTypes();
                    AIBase ai = null;
                    //利用简单工厂外加反射来简化代码量
                    foreach (Type t in types)
                    {
                        if (t.Name == ruleType.ToString() && !t.Name.Contains("DisplayClass"))//这里很巧妙，枚举的类型Tostring后刚好和类名的名字一样。
                        {
                            ai = (AIBase)Assembly.GetExecutingAssembly().CreateInstance(t.FullName);
                            break;
                        }
                    }
                    if (ai == null)
                    {
                        MessageBox.Show("上一个玩家作弊～，出牌不符合规则！");
                        return null;
                    }
                    int[] cardArray = ai.GetCardByReflect(info);
                    if (cardArray == null)
                    {
                        //如果没有找到合适的牌，那么就开始判断自己是否有炸弹，给地主致命一击。
                        ai = new Bomb();
                        return ai.GetOutPutCard(info);
                    }
                    else
                    {
                        return cardArray;
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    File.AppendAllText(Application.StartupPath + "\\AILog.txt", ex.Message + ex.StackTrace);
                    Thread.Sleep(500);
#endif
                    MessageBox.Show("GetCardByReflect出错" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        private static int[] GetCycleFirstCard()
        {
            //获取如果是第一个出牌的人应该出什么牌的规则，此测试AI，是有什么就出什么，直到最后才出单牌。不能返回null，必须出牌。
            try
            {
                return new CycleFirstOutPut().GetOutPutCard(null);
            }
            catch(Exception ex)
            {
#if DEBUG
                File.AppendAllText(Application.StartupPath + "\\AILog.txt", ex.Message + ex.StackTrace);
                Thread.Sleep(500);
#endif
                MessageBox.Show("GetCycleFirstCard出错" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
