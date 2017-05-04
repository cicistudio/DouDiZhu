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
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CiCiCard.ConfigClass;

namespace CiCiCard.Dialogs
{
    /// <summary>
    /// ScoreDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreDialog : Window
    {
        public string LeftScore { get; set; }
        public string MiddleScore { get; set; }
        public string RightScore { get; set; }

        public ScoreDialog()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScoreInfo score = null;
            using (Stream stream = File.Open(Environment.CurrentDirectory + "\\Score.bin", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();              
                score = (ScoreInfo)formatter.Deserialize(stream);
                stream.Close();
            }
            labelLeft.Content += score.LeftScore.ToString();
            labelMiddle.Content += score.MiddleScore.ToString();
            labelRight.Content += score.RightScore.ToString();

            if (LeftScore!= null && LeftScore != string.Empty)
            {
                labelLeft.Content += LeftScore;
            }
            if (MiddleScore != null && MiddleScore != string.Empty)
            {
                labelMiddle.Content += MiddleScore;
            }
            if (RightScore != null && RightScore != string.Empty)
            {
                labelRight.Content += RightScore;
            }
        }

        private void buttonNewGame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
