using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using System.Xml;
using System.Reflection;
using Microsoft.Win32;
using CiCiCard.ConfigClass;
using CiCiStudio.CardFramework;
using CiCiStudio.CardFramework.CommonClass;
using AIFrameWork;


namespace CiCiCard.Dialogs
{
    /// <summary>
    /// PlayerSettingDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerSettingDialog : Window
    {
        public PlayerSettingDialog()
        {
            InitializeComponent();
        }

        private string m_LeftFullName;
        private string m_MiddleFullName;
        private string m_RightFullName;
        private ConfigInfo m_GameConfig; 

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (GameOptions.GameStatus != GameStatus.Nothing && GameOptions.GameStatus != GameStatus.GameEnd)
            {
                MessageBox.Show("游戏正在进行中，无法设置插件！", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
                return;
            }

            foreach (string file in Directory.GetFiles(Environment.CurrentDirectory + "\\AIPlugin"))
            {
                if (System.IO.Path.GetFileName(file) == "AIFrameWork.dll") continue;
                if (System.IO.Path.GetExtension(file).ToLower() != ".dll") continue;//必须是dll结尾
                comboBoxLeft.Items.Add(System.IO.Path.GetFileName(file));
                comboBoxMiddle.Items.Add(System.IO.Path.GetFileName(file));
                comboBoxRight.Items.Add(System.IO.Path.GetFileName(file));
            }
            ConfigInfo config = PluginManage.GetConfigInfofromXML();
            comboBoxLeft.SelectedItem = config.LeftAIShortPath;
            comboBoxMiddle.SelectedItem = config.MiddleAIShortPath;
            comboBoxRight.SelectedItem = config.RightAIShortPath;
            m_LeftFullName = config.LeftAIFullName;
            m_MiddleFullName = config.MiddleAIFullName;
            m_RightFullName = config.RightAIFullName;
            radioCPU.IsChecked = config.IsMiddleAI;
            radioMan.IsChecked = !config.IsMiddleAI;
            checkBoxShowAllCard.IsChecked = config.IsShowAllCard;
            m_GameConfig = config;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void radioMan_Checked(object sender, RoutedEventArgs e)
        {
            this.comboBoxMiddle.IsEnabled = false;
            checkBoxShowAllCard.IsEnabled = false;
            checkBoxShowAllCard.Visibility = Visibility.Hidden;
        }

        private void radioCPU_Checked(object sender, RoutedEventArgs e)
        {
            this.comboBoxMiddle.IsEnabled = true;
            checkBoxShowAllCard.IsEnabled = true;
            checkBoxShowAllCard.Visibility = Visibility.Visible;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Environment.CurrentDirectory + "\\PluginConfig.xml");
                xmlDoc.SelectSingleNode("PluginConfig/LeftPlayerAI").Attributes["FullName"].InnerText = m_LeftFullName;
                xmlDoc.SelectSingleNode("PluginConfig/LeftPlayerAI").InnerText = comboBoxLeft.Text;
                xmlDoc.SelectSingleNode("PluginConfig/MiddlePlayerAI").Attributes["FullName"].InnerText = m_MiddleFullName;
                xmlDoc.SelectSingleNode("PluginConfig/MiddlePlayerAI").InnerText = comboBoxMiddle.Text;
                xmlDoc.SelectSingleNode("PluginConfig/RightPlayerAI").Attributes["FullName"].InnerText = m_RightFullName;
                xmlDoc.SelectSingleNode("PluginConfig/RightPlayerAI").InnerText = comboBoxRight.Text;
                xmlDoc.SelectSingleNode("PluginConfig/IsMiddleAI").InnerText = radioCPU.IsChecked.Value.ToString();
                xmlDoc.SelectSingleNode("PluginConfig/IsShowAllCard").InnerText = checkBoxShowAllCard.IsChecked.Value.ToString();
                xmlDoc.Save(Environment.CurrentDirectory + "\\PluginConfig.xml");
                Thread.Sleep(100);
                PluginManage.ConfigInfo = PluginManage.GetConfigInfofromXML();//重新读取配置
                //如果插件改变了，就卸载掉原来的插件，从而使用新的插件。
                if (m_GameConfig.LeftAIShortPath != comboBoxLeft.Text)
                {
                    PluginManage.SetPlugin(comboBoxLeft.Text, CardPlayerType.LeftPlayer,m_LeftFullName);
                }
                if (m_GameConfig.MiddleAIShortPath != comboBoxMiddle.Text || m_GameConfig.IsMiddleAI != radioCPU.IsChecked.Value)
                {
                    if (radioCPU.IsChecked.Value)
                    {
                        PluginManage.SetPlugin(comboBoxMiddle.Text, CardPlayerType.MiddlePlayer,m_MiddleFullName);
                    }
                }

                if (m_GameConfig.RightAIShortPath != comboBoxRight.Text)
                {
                    PluginManage.SetPlugin(comboBoxRight.Text, CardPlayerType.RightPlayer,m_RightFullName);
                }                
                this.DialogResult = true;
            }
            catch
            {
                throw;
            }
        }

        private void SelectNewPlugin(ComboBox combobox, CardPlayerType player)
        {
            if (combobox.SelectedValue == null)
            {
                return;
            }
            Assembly asm = Assembly.LoadFile(Environment.CurrentDirectory + "\\AIPlugin\\" + combobox.SelectedValue);
            //遍历assembly对象，找到集成IBase的类。
            Type[] types = asm.GetTypes();
            bool isHaveIBase = false;
            foreach (Type t in types)
            {
                if (t.GetInterface("IBase") != null)
                {
                    isHaveIBase = true;
                    switch (player)
                    {
                        case CardPlayerType.LeftPlayer:
                            m_LeftFullName = t.FullName;
                            break;
                        case CardPlayerType.MiddlePlayer:
                            m_MiddleFullName = t.FullName;
                            break;
                        case CardPlayerType.RightPlayer:
                            m_RightFullName = t.FullName;
                            break;
                    }
                    break;
                }
            }
            if (!isHaveIBase)
            {
                MessageBox.Show("您选择的插件中，没有找到继承IBase接口的类！请手动输入这个类的完全名称，形式：namespace.classname", "", MessageBoxButton.OK, MessageBoxImage.Information);
                InputDialog dialog = new InputDialog();
                dialog.PluginAssembly = asm;
                if (dialog.ShowDialog().Value)
                {
                    switch (player)
                    {
                        case CardPlayerType.LeftPlayer:
                            m_LeftFullName = dialog.FullName;
                            break;
                        case CardPlayerType.MiddlePlayer:
                            m_MiddleFullName = dialog.FullName;
                            break;
                        case CardPlayerType.RightPlayer:
                            m_RightFullName = dialog.FullName;
                            break;
                    }
                }
                else
                {
                    combobox.SelectedItem = comboBoxLeft.Text;
                }
            }
        }

        private void comboBoxLeft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectNewPlugin(comboBoxLeft, CardPlayerType.LeftPlayer);
        }

        private void comboBoxMiddle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectNewPlugin(comboBoxMiddle, CardPlayerType.MiddlePlayer);
        }

        private void comboBoxRight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectNewPlugin(comboBoxRight, CardPlayerType.RightPlayer);
        }
    }
}
