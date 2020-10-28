using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TelegramKP_Komissia.AppData;

namespace TelegramKP_Komissia
{
    /// <summary>
    /// Логика взаимодействия для Statistic.xaml
    /// </summary>
    public partial class Statistic : Window
    {
        public Statistic()
        {
            InitializeComponent();

            TxtHelp.Text = TxtHelp.Text + " " + Global.countHelp;
            TxtLinks.Text = TxtLinks.Text + " " + Global.countLinks;
            TxtMenu.Text = TxtMenu.Text + " " + Global.countMenu;
            TxtStart.Text = TxtStart.Text + " " + Global.countStart;
            TxtSupport.Text = TxtSupport.Text + " " + Global.countSupport;
            txtKeyboard.Text = txtKeyboard.Text + " " + Global.countKeyboard;

        }
    }
}
