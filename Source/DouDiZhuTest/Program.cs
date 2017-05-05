using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CiCiAI;
using System.Security.Cryptography;
using CiCiAI.Core.Rules;
using CiCiAI.Core.Combine;
using CiCiAI.Core;

namespace DouDiZhuTest
{
    class Program
    {
        static List<string> LeftPokerList = new List<string>();
        static List<string> MiddlePokerList = new List<string>();
        static List<string> RightPokerList = new List<string>();
        static List<string> DiPaiPokerList = new List<string>();

        static int m_count = 0;
        static void Main(string[] args)
        {
            MainClass main = new MainClass();
            EvaluationInfo bestEval = main.TestEvaluationSocre(GeneratePokers());
            //main.NewGame(TestPokers());

            //List<CombineBaseInfo> test = new List<CombineBaseInfo>();
            //test.Add(new DuiZiInfo() { CombinePokerString ="a" });
            //test.Add(new DuiZiInfo() { CombinePokerString = "b" });
            //test.Add(new DuiZiInfo() { CombinePokerString = "c" });
            //test.Add(new DuiZiInfo() { CombinePokerString = "d" });
            //test.Add(new DuiZiInfo() { CombinePokerString = "e" });
            //test.Add(new DuiZiInfo() { CombinePokerString = "f" });
            //Permutation( test , 0,5);
            Console.ReadKey();
        }
        //排列组合
        public static void Permutation(List<CombineBaseInfo> s, int from, int to)
        {
            if (to <= 1)
                return;
            if (from == to)
            {
                foreach(CombineBaseInfo info in s)
                {
                    Console.Write(info.CombinePokerString);
                }
               
                Console.WriteLine(++m_count);
            }
            else
            {
                for (int i = from; i <= to; i++)
                {
                    swap(s, i, from);
                    Permutation(s, from + 1, to);
                    swap(s, from, i);
                }
            }
        }

        public static void swap(List<CombineBaseInfo> s, int i, int j)
        {
            CombineBaseInfo temp = s[i];
            s[i] = s[j];
            s[j] = temp;
        }
 

        static string TestPokers()
        {
           // return "3,4,4,4,5,5,5,6,6,6,7,7,7,8,8,8,9,9,10,10";
            return "3,4,4,4,5,6,6,6,6,7,8,10,J,Q,K,A,2,JKBig";
            //return "4,5,6,7,8,9,10,J,J,Q,Q,JKSmall,JKBig,8,K,A,2";
           // return "3,4,4,5,5,6,6,7,7,8,8,9,9,10,10";
        }


        static string GeneratePokers()
        {
            List<string> pokerList = new List<string>();
            List<string> ranPokerList = new List<string>();
            for (int j = 1; j <= 4; j++)
            {
                for (int i = 2; i <= 10; i++)
                {
                    pokerList.Add(i.ToString());
                }
                pokerList.Add("J");
                pokerList.Add("Q");
                pokerList.Add("K");
                pokerList.Add("A");
            }
            pokerList.Add("JKSmall");
            pokerList.Add("JKBig");

            Random ran = new Random(GetRandomSeed());
            for (int m = 53; m >= 0;m-- )
            {
                int r = ran.Next(0, m + 1);
                ranPokerList.Add(pokerList[r]);
                pokerList.RemoveAt(r);
            }

            for (int n = 0; n < 54;n++ )
            {
                if(n<17)
                {
                    LeftPokerList.Add(ranPokerList[n]);
                }
                else if(n<34)
                {
                    MiddlePokerList.Add(ranPokerList[n]);
                }
                else if(n<51)
                {
                    RightPokerList.Add(ranPokerList[n]);
                }
                else
                {
                    DiPaiPokerList.Add(ranPokerList[n]);
                }
            }

            StringBuilder sb = new StringBuilder();
            for(int i =0;i<17;i++)
            {
                sb.Append(MiddlePokerList[i]);
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        /// <summary>
        /// 生成随机数种子
        /// </summary>
        /// <returns>种子</returns>
        private static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

            rngCsp.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
