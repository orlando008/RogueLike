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
    /// Interaction logic for StoryMessageScreen.xaml
    /// </summary>
    public partial class StoryMessageScreen : Window
    {
        public StoryMessageScreen(string message)
        {
            InitializeComponent();
            lblStoryMessage.Text = message;
        }

        private void imgOK_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
