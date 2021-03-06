﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shadows
{
    /// <summary>
    /// Interaction logic for CombatDialog.xaml
    /// </summary>
    public partial class CombatDialog : UserControl
    {
        public CombatDialog()
        {
            InitializeComponent();
        }

        private void btnMoveLeft_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)this.DataContext).TheMap.ThePlayer.MoveCombatLeft();
            ((MainWindow)this.DataContext).RefreshAllProperties();
        }

        private void btnMoveRight_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)this.DataContext).TheMap.ThePlayer.MoveCombatRight();
            ((MainWindow)this.DataContext).RefreshAllProperties();
        }
    }
}
