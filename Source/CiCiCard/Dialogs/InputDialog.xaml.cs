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
using System.Reflection;

namespace CiCiCard.Dialogs
{
    /// <summary>
    /// InputDialog.xaml 的交互逻辑
    /// </summary>
    public partial class InputDialog : Window
    {
        public Assembly PluginAssembly { get; set; }
        public string FullName { get; set; }

        public InputDialog()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (!textBoxFullName.Text.Contains('.'))
            {
                MessageBox.Show("类名输入的不正确，缺少NameSpace部分！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Type[] types = PluginAssembly.GetTypes();
            bool isHaveClass = false;
            foreach (Type t in types)
            {
                if (t.FullName == textBoxFullName.Text)
                {
                    isHaveClass = true;
                    break;
                }
            }

            if (!isHaveClass)
            {
                MessageBox.Show("没有找到该类名" + textBoxFullName.Text, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            FullName = textBoxFullName.Text;
            this.DialogResult = true;
        }
    }
}
