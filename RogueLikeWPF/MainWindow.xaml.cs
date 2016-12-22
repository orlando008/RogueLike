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
        private bool _midDrawing = true;
        private BackgroundWorker _bw = new BackgroundWorker();
        private List<RogueLike.OverallMap.DrawPortionEventArgs> ListOfThings = new List<RogueLike.OverallMap.DrawPortionEventArgs>();

        public MainWindow()
        {
            InitializeComponent();

            RogueLike.Program.InputNeeded += Program_InputNeeded;
            RogueLike.Program.DrawPortion += Program_DrawPortion;
            RogueLike.Program.DrawBegin += Program_DrawBegin;
            RogueLike.Program.DrawEnd += Program_DrawEnd; ;

            _bw.DoWork += _bw_DoWork;
           
        }

        private void Program_DrawEnd(object sender, EventArgs e)
        {
            if (canvasMain.Dispatcher.CheckAccess() == true)
            {
                _midDrawing = false;
                
                canvasMain.Children.Clear();
                foreach (RogueLike.OverallMap.DrawPortionEventArgs item in ListOfThings)
                {
                    System.Windows.Shapes.Rectangle r = new Rectangle();
                    r.Width = 10;
                    r.Height = 10;
                    r.StrokeThickness = .4;
                    r.Stroke = new SolidColorBrush(Colors.Black);

                    switch (item.StringData)
                    {
                        case ":":
                            r.Fill = new SolidColorBrush(Colors.Blue);
                            break;
                        case "|":
                        case "-":
                        case "*":
                            r.Fill = new SolidColorBrush(Colors.DarkGray);
                            break;
                        case "#":
                            r.Fill = new SolidColorBrush(Colors.Brown);
                            break;
                        case " ":
                            r.Fill = new SolidColorBrush(Colors.DarkBlue);
                            break;

                    }

                    Canvas.SetLeft(r, item.XCoordinate * 10);
                    Canvas.SetTop(r, item.YCoordinate * 10);

                    canvasMain.Children.Add(r);

                    if (item.StringData == ":")
                    {
                        System.Windows.Shapes.Ellipse c = new Ellipse();
                        c.Width = 10;
                        c.Height = 10;
                        c.StrokeThickness = .4;

                        c.Stroke = new SolidColorBrush(Colors.Yellow);
                        Canvas.SetLeft(c, item.XCoordinate * 10);
                        Canvas.SetTop(c, item.YCoordinate * 10);

                        canvasMain.Children.Add(c);
                    }
                }
                
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => Program_DrawEnd(sender,e)));
            }

        }

        private void Program_DrawBegin(object sender, EventArgs e)
        {
            _midDrawing = true;
            ListOfThings.Clear();
        }

        private void _bw_DoWork(object sender, DoWorkEventArgs e)
        {
            RogueLike.Program.Main(new string[] { "test" });
        }

        private void Program_DrawPortion(RogueLike.OverallMap.DrawPortionEventArgs e)
        {
            RogueLike.OverallMap.DrawPortionEventArgs deep = new RogueLike.OverallMap.DrawPortionEventArgs();
            deep.StringData = e.StringData;
            deep.XCoordinate = e.XCoordinate;
            deep.YCoordinate = e.YCoordinate;

            ListOfThings.Add(deep);
        }

        private void Program_InputNeeded(RogueLike.Program.InputNeededEventArgs e)
        {
            //MessageBox.Show(">");
        }

        private void canvasMain_Loaded(object sender, RoutedEventArgs e)
        {
            _bw.RunWorkerAsync();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Left:
                    RogueLike.Program.ProcessUserCommand("MOVELEFT");
                    RogueLike.Program.ProcessUserCommand("DRAW");
                    break;
                case Key.Right:
                    RogueLike.Program.ProcessUserCommand("MOVERIGHT");
                    RogueLike.Program.ProcessUserCommand("DRAW");
                    break;
                case Key.Up:
                    RogueLike.Program.ProcessUserCommand("MOVEUP");
                    RogueLike.Program.ProcessUserCommand("DRAW");
                    break;
                case Key.Down:
                    RogueLike.Program.ProcessUserCommand("MOVEDOWN");
                    RogueLike.Program.ProcessUserCommand("DRAW");
                    break;
            }
        }
    }
}
