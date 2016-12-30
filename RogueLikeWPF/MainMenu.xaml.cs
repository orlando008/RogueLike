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

namespace Shadows
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            CharacterCreation cc = new CharacterCreation();
            cc.Owner = Application.Current.MainWindow;
            cc.ShowInTaskbar = false;
            cc.ShowDialog();

            if (cc.Canceled)
                return;
     
            MainWindow mw = new MainWindow(cc.CharacterChoice);
            mw.Owner = Application.Current.MainWindow;
            mw.ShowDialog();
        }
    }
}
