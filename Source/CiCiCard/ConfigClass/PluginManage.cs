using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.IO;
using System.Reflection;
using AIFrameWork;
using AIPlugin;
using CiCiStudio.CardFramework;
using System.Threading;

namespace CiCiCard.ConfigClass
{
    public class PluginManage
    {
        private static Dictionary<CardPlayerType, Loader> m_AIPluginLoadersDict = new Dictionary<CardPlayerType, Loader>();

        /// <summary>
        /// 通过CardPlayerType可以获得对应的AI插件的Appdomain的Loader
        /// </summary>
        public static Dictionary<CardPlayerType, Loader> AIPluginLoadersDict
        {
            get { return PluginManage.m_AIPluginLoadersDict; }
            set { PluginManage.m_AIPluginLoadersDict = value; }
        }

        public static ConfigInfo ConfigInfo { get; set; }

        public static ConfigInfo GetConfigInfofromXML()
        {
            ConfigInfo info = new ConfigInfo();
            XmlDocument xmlDoc = new XmlDocument();
            string currentPath = Environment.CurrentDirectory + "\\";
            if (!File.Exists(currentPath + "PluginConfig.xml"))
            {
                throw new Exception("没有找到配置文件!PluginConfig.xml");
            }
            xmlDoc.Load(currentPath + "PluginConfig.xml");
            info.LeftAIShortPath = xmlDoc.SelectSingleNode("PluginConfig/LeftPlayerAI").InnerText;
            info.LeftAIFullName = xmlDoc.SelectSingleNode("PluginConfig/LeftPlayerAI").Attributes["FullName"].Value;
            info.LeftAIPath = currentPath + "AIPlugin\\" + info.LeftAIShortPath;
            info.RightAIShortPath =  xmlDoc.SelectSingleNode("PluginConfig/RightPlayerAI").InnerText;
            info.RightAIFullName = xmlDoc.SelectSingleNode("PluginConfig/RightPlayerAI").Attributes["FullName"].Value;
            info.RightAIPath = currentPath + "AIPlugin\\" + info.RightAIShortPath;
            info.MiddleAIShortPath = xmlDoc.SelectSingleNode("PluginConfig/MiddlePlayerAI").InnerText;
            info.MiddleAIFullName = xmlDoc.SelectSingleNode("PluginConfig/MiddlePlayerAI").Attributes["FullName"].Value;
            info.MiddleAIPath = currentPath + "AIPlugin\\" +  info.MiddleAIShortPath;
            info.IsMiddleAI = Convert.ToBoolean(xmlDoc.SelectSingleNode("PluginConfig/IsMiddleAI").InnerText);
            info.IsShowAllCard = Convert.ToBoolean(xmlDoc.SelectSingleNode("PluginConfig/IsShowAllCard").InnerText);
            return info;
        }

        public static void SetPlugin(string pluginName, CardPlayerType player,string aiFullName)
        {
            if (m_AIPluginLoadersDict.ContainsKey(player))
            {
                m_AIPluginLoadersDict[player].Unload();
                m_AIPluginLoadersDict.Remove(player);
            }
            try
            {
                Loader load = new Loader(player.ToString());
                load.AddAssembly("AI", Environment.CurrentDirectory + "\\AIPlugin\\" + pluginName);
                load.FullNameDict.Add("AI", aiFullName);
                m_AIPluginLoadersDict.Add(player, load);
            }
            catch(Exception ex)
            {
                MessageBox.Show("无法加载新的AI插件。" + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.MainWindow.Close();
            }
        }

        public static Loader GetPlugin(CardPlayerType player)
        {
            return m_AIPluginLoadersDict[player];
        }

        public static object Invoke(CardPlayerType player, string methodName)
        {
           return Invoke(player, methodName, null);
        }

        public static object Invoke(CardPlayerType player, string methodName, params Object[] args)
        {
           return m_AIPluginLoadersDict[player].Invoke("AI", methodName, args);
        }

        /// <summary>
        /// 该方法用于程序首回合Game之前，从配置文件中加载AI插件
        /// </summary>
        /// <returns></returns>
        public static bool LoadPluginFromXML()
        {
            ConfigInfo info = GetConfigInfofromXML();
            ConfigInfo = info;
            if (!File.Exists(info.LeftAIPath))
            {
                throw new Exception("没有找到左侧玩家的AI插件!");
            }
            if (!File.Exists(info.RightAIPath))
            {
                throw new Exception("没有找到右侧玩家的AI插件!" );
            }

            if (info.IsMiddleAI)
            {
                if (!File.Exists(info.MiddleAIPath))
                {
                    throw new Exception("中间玩家已经被设置为AI模式，但是没有找到中间玩家的AI插件!");
                }
            }
            try
            {
                //使用Appdomain将每个插件隔离开来。
                //Loader leftLoad = new Loader("LeftPlayer");
                //leftLoad.AddAssembly("AI", info.LeftAIPath);
                //leftLoad.FullNameDict.Add("AI", info.LeftAIFullName);
                //m_AIPluginLoadersDict.Add(CardPlayerType.LeftPlayer, leftLoad);
              
                //Loader rightLoad = new Loader("RightPlayer");
                //rightLoad.AddAssembly("AI", info.RightAIPath);
                //rightLoad.FullNameDict.Add("AI", info.RightAIFullName);
                //m_AIPluginLoadersDict.Add(CardPlayerType.RightPlayer, rightLoad);

                //Loader middleLoad = null;
                //if (info.IsMiddleAI)
                //{
                //    middleLoad = new Loader("MiddlePlayer");
                //    middleLoad.AddAssembly("AI", info.MiddleAIPath);
                //    middleLoad.FullNameDict.Add("AI", info.MiddleAIFullName);
                //    m_AIPluginLoadersDict.Add(CardPlayerType.MiddlePlayer, middleLoad);
                //}

                SetPlugin(info.LeftAIShortPath, CardPlayerType.LeftPlayer, info.LeftAIFullName);
                SetPlugin(info.RightAIShortPath, CardPlayerType.RightPlayer, info.RightAIFullName);
                if (info.IsMiddleAI)
                {
                    SetPlugin(info.MiddleAIShortPath, CardPlayerType.MiddlePlayer, info.MiddleAIFullName);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("无法加载AI插件，" + ex.Message);
            }
            return true;
        }
    }
}
