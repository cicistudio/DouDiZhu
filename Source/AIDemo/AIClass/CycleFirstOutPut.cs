using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace AIDemo.AIClass
{
    public class CycleFirstOutPut : AIBase
    {
        public override int[] GetOutPutCard(OutPutCardInfo info)
        {
            //仅用于测试使用。
            try
            {
                List<List<int>> fiveKindCollectin = base.GetContinueKindCollection(AIOptions.CurrentCardArray);
                if (fiveKindCollectin.Count > 0)
                {
                    int[] cardArray = new int[fiveKindCollectin[0].Count];
                    for (int i = 0; i < fiveKindCollectin[0].Count; i++)
                    {
                        cardArray[i] = fiveKindCollectin[0][i];
                    }
                    return cardArray;
                }

                List<int> twoKinds = base.GetKindCollection2(AIOptions.CurrentCardArray, 2);//不包含2的情况
                List<int> threeKinds = base.GetKindCollection2(AIOptions.CurrentCardArray, 3);//不包含2的情况
                List<int> singleKinds = base.GetSingleKindCollection(AIOptions.CurrentCardArray);
                List<int> fifteenKinds = GetFifteenCollection(AIOptions.CurrentCardArray);
                List<int> fourKinds = base.GetKindCollection(AIOptions.CurrentCardArray, 4);
                List<int> jokerKinds = base.GetJokerCollection(AIOptions.CurrentCardArray);
                if (threeKinds.Count > 0 && singleKinds.Count > 0)
                {
                    return new int[] { threeKinds.First(), threeKinds.First(), threeKinds.First(), singleKinds.First() };
                }
                else if (threeKinds.Count > 0 && twoKinds.Count > 0)
                {
                    return new int[] { threeKinds.First(), threeKinds.First(), threeKinds.First(), twoKinds.First(), twoKinds.First() };
                }
                else if (threeKinds.Count > 0)
                {
                    return new int[] { threeKinds.First(), threeKinds.First(), threeKinds.First() };
                }
                else if (twoKinds.Count > 0)
                {
                    return new int[] { twoKinds.First(), twoKinds.First() };
                }
                else if (singleKinds.Count > 0)
                {
                    return new int[] { singleKinds.First() };
                }
                else if (fifteenKinds.Count > 0)
                {
                    switch (fifteenKinds.Count)
                    {
                        case 1:
                            return new int[] { 2 };
                        case 2:
                            return new int[] { 2, 2 };
                        case 3:
                            return new int[] { 2, 2, 2 };
                        case 4:
                            return new int[] { 2, 2, 2, 2 };
                        default:
                            return null;
                    }
                }
                else if (fourKinds.Count > 0)
                {
                    return new int[] { fourKinds[0], fourKinds[0], fourKinds[0], fourKinds[0] };
                }
                else if (jokerKinds.Count > 0)
                {
                    if (jokerKinds.Count == 2)
                    {
                        return new int[] { 16, 17 };
                    }
                    else
                    {
                        return new int[] { jokerKinds.First() };
                    }
                }
                else
                {
#if DEBUG
                    StringBuilder log = new StringBuilder();
                    log.Append("\r\n当前系统剩余牌：");
                    foreach (int i in AIOptions.CurrentCardArray)
                    {
                        log.Append(i);
                        log.Append(",");
                    }
                    File.AppendAllText(Application.StartupPath + "\\AILog.txt", log.ToString());
#endif
                    throw new Exception("系统内部处理错误，请参考日志。");
                }
            }
            catch(Exception ex)
            {
#if DEBUG
               
                File.AppendAllText(Application.StartupPath + "\\AILog.txt", ex.Message + ex.StackTrace);
                Thread.Sleep(500);
#endif
                throw;
            }
            
        }
    }
}
