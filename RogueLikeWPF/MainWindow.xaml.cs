using Shadows;
using Shadows.InteractableObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private Ellipse _playerDot = null;
        private BackgroundWorker _bw = new BackgroundWorker();
        private CombatUnit _currentCombatUnit;
        private Program _program;

        public Shadows.OverallMap TheMap
        {
            get
            {
                return _program._ovMap;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            _program = new Program();

            _program.MapCreated += Program_MapCreated;
            _program.PlacePlayer += Program_PlacePlayer;
            _program.StoryMessage += _program_StoryMessage;

            _bw.DoWork += _bw_DoWork;
            this.DataContext = this;
        }

        private void _program_StoryMessage(Program.StoryMessageEventArgs e)
        {
            if (canvasMain.Dispatcher.CheckAccess() == true)
            {
                StoryMessageScreen sms = new StoryMessageScreen(e.StoryMessage);
                sms.ShowDialog();
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => _program_StoryMessage(e)));
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Program_PlacePlayer(object sender, EventArgs e)
        {
            if (canvasMain.Dispatcher.CheckAccess() == true)
            {
                if (_playerDot == null)
                {
                    _playerDot = new Ellipse();
                    _playerDot.Width = 10;
                    _playerDot.Height = 10;
                    _playerDot.StrokeThickness = .1;
                    _playerDot.Stroke = new SolidColorBrush(Colors.Blue);
                    _playerDot.Fill = new SolidColorBrush(Colors.Blue);
                    canvasMain.Children.Add(_playerDot);

                    Canvas.SetLeft(_playerDot, (_program._ovMap.ThePlayer.Location.X) * 10);
                    Canvas.SetTop(_playerDot, (_program._ovMap.ThePlayer.Location.Y) * 10);


                    DrawOutline();
                }
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => Program_PlacePlayer(null,null)));
            }
        }

        private void Program_MapCreated(object sender, EventArgs e)
        {
            _program._ovMap.RoomDiscovered += _ovMap_RoomDiscovered;
            _program._ovMap.HallDiscovered += _ovMap_HallDiscovered;
            _program._ovMap.CombatEncountered += _ovMap_CombatEncountered;
            _program._ovMap.NothingEncountered += _ovMap_NothingEncountered;
            NotifyPropertyChanged("");
        }

        private void _ovMap_NothingEncountered(object sender, EventArgs e)
        {
            stackPanelFightFlee.Visibility = Visibility.Collapsed;
            this.lblCurrentActivity.Content = "Nothing unusual here...";
            this.lblCurrentActivityDetail.Content = "";
        }

        private void _ovMap_CombatEncountered(OverallMap.CombatEncounteredEventArgs e)
        {
            _currentCombatUnit = e.combatUnit;
            stackPanelFightFlee.Visibility = Visibility.Visible;
            imgCombatUnit.Visibility = Visibility.Visible;
            this.lblCurrentActivity.Content = "You encountered: " + e.combatUnit.ToString();
            this.lblCurrentActivityDetail.Content = e.combatUnit.FullEnemyStats();
            NotifyPropertyChanged("");
        }

        private void DrawOutline()
        {
            if (canvasMain.Dispatcher.CheckAccess() == true)
            {
                int maxX = 0;
                int maxY = 0;
                TheMap.GetMax(out maxX, out maxY, TheMap.ThePlayer.DungeonLevel);

                System.Windows.Shapes.Rectangle rct = new Rectangle();
                rct.Width = maxX * 10;
                rct.Height = 10;
                rct.StrokeThickness = 1.5;
                rct.Stroke = new SolidColorBrush(Colors.Gray);
                rct.Fill = new SolidColorBrush(Colors.Gray);

                canvasMain.Children.Add(rct);

                Canvas.SetLeft(rct, 0);
                Canvas.SetTop(rct, 0 * 10);



                System.Windows.Shapes.Rectangle rct2 = new Rectangle();
                rct2.Width = maxX * 10;
                rct2.Height = 10;
                rct2.StrokeThickness = 1.5;
                rct2.Stroke = new SolidColorBrush(Colors.Gray);
                rct2.Fill = new SolidColorBrush(Colors.Gray);

                canvasMain.Children.Add(rct2);

                Canvas.SetLeft(rct2, 0);
                Canvas.SetTop(rct2, maxY * 10);


                System.Windows.Shapes.Rectangle rct3 = new Rectangle();
                rct3.Width = 10;
                rct3.Height = maxY * 10;
                rct3.StrokeThickness = 1.5;
                rct3.Stroke = new SolidColorBrush(Colors.Gray);
                rct3.Fill = new SolidColorBrush(Colors.Gray);

                canvasMain.Children.Add(rct3);

                Canvas.SetLeft(rct3, 0);
                Canvas.SetTop(rct3, 0);

                System.Windows.Shapes.Rectangle rct4 = new Rectangle();
                rct4.Width = 10;
                rct4.Height = maxY * 10 + 10;
                rct4.StrokeThickness = 1.5;
                rct4.Stroke = new SolidColorBrush(Colors.Gray);
                rct4.Fill = new SolidColorBrush(Colors.Gray);

                canvasMain.Children.Add(rct4);

                Canvas.SetLeft(rct4, maxX * 10);
                Canvas.SetTop(rct4, 0);
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => DrawOutline()));
            }
        }

        private void _ovMap_HallDiscovered(OverallMap.HallDiscoveredEventArgs e)
        {
            if (canvasMain.Dispatcher.CheckAccess() == true)
            {
                System.Windows.Shapes.Rectangle rct = new Rectangle();
                rct.Width = 10;
                rct.Height = 10;
                rct.StrokeThickness = 1.5;
                rct.Stroke = new SolidColorBrush(Colors.Goldenrod);
                rct.Fill = new SolidColorBrush(Colors.Gold);

                canvasMain.Children.Add(rct);

                Canvas.SetLeft(rct, e.hallThatWasDiscovered.X * 10);
                Canvas.SetTop(rct, e.hallThatWasDiscovered.Y * 10);
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => _ovMap_HallDiscovered(e)));
            }
        }

        private void _ovMap_RoomDiscovered(OverallMap.RoomDiscoveredEventArgs e)
        {
            if (canvasMain.Dispatcher.CheckAccess() == true)
            {
                System.Windows.Shapes.Rectangle rct = new Rectangle();

                switch (e.roomTileThatWasDiscovered.ThisTileType)
                {
                    case Shadows.StructuralClasses.TileType.Floor:
                        rct.Width = 10;
                        rct.Height = 10;
                        rct.StrokeThickness = .4;
                        rct.Stroke = new SolidColorBrush(Colors.Brown);
                        rct.Fill = new SolidColorBrush(Colors.Beige);

                        canvasMain.Children.Add(rct);

                        Canvas.SetLeft(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.X + e.roomTileThatWasDiscovered.X) * 10);
                        Canvas.SetTop(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.Y + e.roomTileThatWasDiscovered.Y) * 10);

                        break;

                    case Shadows.StructuralClasses.TileType.VerticalWall:
                    case Shadows.StructuralClasses.TileType.HorizontalWall:

                        rct.Width = 10;
                        rct.Height = 10;
                        rct.StrokeThickness = 1.25;
                        rct.Stroke = new SolidColorBrush(Colors.Brown);
                        rct.Fill = new SolidColorBrush(Colors.Black);
                        canvasMain.Children.Add(rct);

                        Canvas.SetLeft(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.X + e.roomTileThatWasDiscovered.X) * 10);
                        Canvas.SetTop(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.Y + e.roomTileThatWasDiscovered.Y) * 10);

                        break;
                    case Shadows.StructuralClasses.TileType.Door:
                        rct.Width = 10;
                        rct.Height = 10;
                        rct.StrokeThickness = 2.5;
                        rct.Stroke = new SolidColorBrush(Colors.Green);
                        rct.Fill = new SolidColorBrush(Colors.Silver);

                        canvasMain.Children.Add(rct);

                        Canvas.SetLeft(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.X + e.roomTileThatWasDiscovered.X) * 10);
                        Canvas.SetTop(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.Y + e.roomTileThatWasDiscovered.Y) * 10);

                        break;

                }

            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => _ovMap_RoomDiscovered(e)));
            }


        }


        private void _bw_DoWork(object sender, DoWorkEventArgs e)
        {
            _program.StartNewGame();
            NotifyPropertyChanged("");
        }

        private void canvasMain_Loaded(object sender, RoutedEventArgs e)
        {
            _bw.RunWorkerAsync();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    _program.ProcessUserCommand("MOVELEFT");
                    break;
                case Key.Right:
                    _program.ProcessUserCommand("MOVERIGHT");
                    break;
                case Key.Up:
                    _program.ProcessUserCommand("MOVEUP");
                    break;
                case Key.Down:
                    _program.ProcessUserCommand("MOVEDOWN");
                    break;
            }

            if (_playerDot != null)
            {
                Canvas.SetLeft(_playerDot, (_program._ovMap.ThePlayer.Location.X) * 10);
                Canvas.SetTop(_playerDot, (_program._ovMap.ThePlayer.Location.Y) * 10);
                Canvas.SetZIndex(_playerDot, 99);
            }

            NotifyPropertyChanged("");
        }

        private void btnFight_Click(object sender, RoutedEventArgs e)
        {
            imgCombatUnit.Visibility = Visibility.Collapsed;
            stackPanelFightFlee.Visibility = Visibility.Collapsed;

            _program._ovMap.ResolveCombat(_currentCombatUnit);
            this.lblCurrentActivity.Content = "You defeated " + _currentCombatUnit.ToString();
            this.lblCurrentActivityDetail.Content = "Received " + _currentCombatUnit.ExperienceWorth.ToString() + " exp." + System.Environment.NewLine + " " + _currentCombatUnit.GoldWorth.ToString() + " gold.";

            _currentCombatUnit = null;
            NotifyPropertyChanged("");
        }

        private void btnFlee_Click(object sender, RoutedEventArgs e)
        {
            imgCombatUnit.Visibility = Visibility.Collapsed;
            stackPanelFightFlee.Visibility = Visibility.Collapsed;
            this.lblCurrentActivity.Content = "You escaped but gained no experience.";
            this.lblCurrentActivityDetail.Content = "";

            _program._ovMap.FleeCombat(_currentCombatUnit);
            _currentCombatUnit = null;
            NotifyPropertyChanged("");
        }
    }
}
