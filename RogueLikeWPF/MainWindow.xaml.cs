using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace RogueLikeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker _bw = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();

            RogueLike.Program.InputNeeded += Program_InputNeeded;
            RogueLike.Program.DrawPortion += Program_DrawPortion;

            _bw.DoWork += _bw_DoWork;
           
        }

        private void _bw_DoWork(object sender, DoWorkEventArgs e)
        {
            RogueLike.Program.Main(new string[] { "test" });
        }

        private void Program_DrawPortion(RogueLike.OverallMap.DrawPortionEventArgs e)
        {
            if(canvasMain.Dispatcher.CheckAccess() == true)
            {
                System.Windows.Shapes.Rectangle r = new Rectangle();
                r.Width = 10;
                r.Height = 10;
                r.StrokeThickness = .4;
                r.Stroke = new SolidColorBrush(Colors.Black);
                Canvas.SetLeft(r, e.XCoordinate * 10);
                Canvas.SetTop(r, e.YCoordinate * 10);

                canvasMain.Children.Add(r);
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => Program_DrawPortion(e)));
            }
        }

        private void Program_InputNeeded(RogueLike.Program.InputNeededEventArgs e)
        {
            //MessageBox.Show(">");
        }

        private void canvasMain_Loaded(object sender, RoutedEventArgs e)
        {
            _bw.RunWorkerAsync();
        }
    }
}
