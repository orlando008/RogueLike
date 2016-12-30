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
    /// Interaction logic for CharacterCreation.xaml
    /// </summary>
    public partial class CharacterCreation : Window
    {
        public CharacterCreation()
        {
            InitializeComponent();
        }

        public bool Canceled = false;
        public CommonEnumerations.BaseClassTypes CharacterChoice = CommonEnumerations.BaseClassTypes.Warrior;

        private void btnWarrior_Click(object sender, RoutedEventArgs e)
        {
            CharacterChoice = CommonEnumerations.BaseClassTypes.Warrior;
            this.Close();
        }

        private void btnRogue_Click(object sender, RoutedEventArgs e)
        {
            CharacterChoice = CommonEnumerations.BaseClassTypes.Rogue;
            this.Close();
        }

        private void btnMage_Click(object sender, RoutedEventArgs e)
        {
            CharacterChoice = CommonEnumerations.BaseClassTypes.Mage;
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Canceled = true;
            this.Close();
        }
    }
}
