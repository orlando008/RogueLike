using Shadows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        private CommonEnumerations.BaseClassTypes _chosenBaseClass;
        private const int TILE_WIDTH = 10;

        public Shadows.OverallMap TheMap
        {
            get
            {
                return _program._ovMap;
            }
        }

        public MainWindow(CommonEnumerations.BaseClassTypes bct)
        {
            InitializeComponent();

            _chosenBaseClass = bct;
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
                Paragraph p = new Paragraph();
                p.Inlines.Add(new Run(e.StoryMessage));
                p.Foreground = new SolidColorBrush(e.messageColor);
                p.Margin = new Thickness(0);
                combatTextBox.Document.Blocks.Add(p);
                combatTextBox.ScrollToEnd();
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => _program_StoryMessage(e)));
            }

            System.Threading.Thread.Sleep(1000);
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

        public void RefreshAllProperties()
        {
            NotifyPropertyChanged("");
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

                    Canvas.SetLeft(_playerDot, (_program._ovMap.ThePlayer.Location.X) * TILE_WIDTH);
                    Canvas.SetTop(_playerDot, (_program._ovMap.ThePlayer.Location.Y) * TILE_WIDTH);


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
            _program._ovMap.StoryMessage += _program_StoryMessage;
            NotifyPropertyChanged("");

        }

        private void _ovMap_NothingEncountered(object sender, EventArgs e)
        {
        }

        private void _ovMap_CombatEncountered(OverallMap.CombatEncounteredEventArgs e)
        {
            _currentCombatUnit = e.combatUnit;

            Paragraph p = new Paragraph();
            string enemyName = e.combatUnit.GetEnemyFormName();
            string aOrAn = "a";
            if(enemyName.StartsWith("A") || enemyName.StartsWith("E") || enemyName.StartsWith("I") || enemyName.StartsWith("O") || enemyName.StartsWith("U") || enemyName.StartsWith("H"))
            {
                aOrAn = "an";
            }

            string msg = "You entered a battle with " + aOrAn + " " + enemyName + ".";
            Program.StoryMessageEventArgs sme = new Program.StoryMessageEventArgs(msg, Colors.LightPink);

            _program_StoryMessage(sme);

            UIElement uietodelete = null;
            foreach (UIElement uie in canvasMain.Children)
            {
                if(uie.GetType() == typeof(Rectangle))
                {
                    if (((Rectangle)uie).Tag != null)
                    {
                        if (((Rectangle)uie).Tag.GetType() == typeof(CombatUnit))
                        {
                            if (((CombatUnit)(((Rectangle)uie).Tag)).Equals(e.combatUnit))
                            {
                                uietodelete = uie;
                                break;
                            }
                        }

                    }
                        
                }
            }

            if(uietodelete != null)
            {
                canvasMain.Children.Remove(uietodelete);
            }

            canvasMain.Visibility = Visibility.Collapsed;
            cbatDialogControl.Visibility = Visibility.Visible;

            TheMap.CurrentCombatLogic.InitiateBattle();
            NotifyPropertyChanged("");
        }

        private void DrawOutline()
        {
            if (canvasMain.Dispatcher.CheckAccess() == true)
            {
                int maxX = 0;
                int maxY = 0;
                TheMap.GetMax(out maxX, out maxY, TheMap.ThePlayer.DungeonLevel-1);

                System.Windows.Shapes.Rectangle rct = new Rectangle();
                rct.Width = maxX * TILE_WIDTH;
                rct.Height = 10;
                rct.StrokeThickness = 1.5;
                rct.Stroke = new SolidColorBrush(Colors.Gray);
                rct.Fill = new SolidColorBrush(Colors.Gray);

                canvasMain.Children.Add(rct);

                Canvas.SetLeft(rct, 0);
                Canvas.SetTop(rct, 0 * TILE_WIDTH);



                System.Windows.Shapes.Rectangle rct2 = new Rectangle();
                rct2.Width = maxX * TILE_WIDTH;
                rct2.Height = 10;
                rct2.StrokeThickness = 1.5;
                rct2.Stroke = new SolidColorBrush(Colors.Gray);
                rct2.Fill = new SolidColorBrush(Colors.Gray);

                canvasMain.Children.Add(rct2);

                Canvas.SetLeft(rct2, 0);
                Canvas.SetTop(rct2, maxY * TILE_WIDTH);


                System.Windows.Shapes.Rectangle rct3 = new Rectangle();
                rct3.Width = 10;
                rct3.Height = maxY * TILE_WIDTH;
                rct3.StrokeThickness = 1.5;
                rct3.Stroke = new SolidColorBrush(Colors.Gray);
                rct3.Fill = new SolidColorBrush(Colors.Gray);

                canvasMain.Children.Add(rct3);

                Canvas.SetLeft(rct3, 0);
                Canvas.SetTop(rct3, 0);

                System.Windows.Shapes.Rectangle rct4 = new Rectangle();
                rct4.Width = 10;
                rct4.Height = maxY * TILE_WIDTH + TILE_WIDTH;
                rct4.StrokeThickness = 1.5;
                rct4.Stroke = new SolidColorBrush(Colors.Gray);
                rct4.Fill = new SolidColorBrush(Colors.Gray);

                canvasMain.Children.Add(rct4);

                Canvas.SetLeft(rct4, maxX * TILE_WIDTH);
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
                rct.Tag = "HALL";
                canvasMain.Children.Add(rct);

                Canvas.SetLeft(rct, e.hallThatWasDiscovered.X * TILE_WIDTH);
                Canvas.SetTop(rct, e.hallThatWasDiscovered.Y * TILE_WIDTH);

                if(TheMap.EnemyLocations.ContainsKey(TheMap.ThePlayer.DungeonLevel-1) && TheMap.EnemyLocations[TheMap.ThePlayer.DungeonLevel-1].FirstOrDefault(x => x.DungeonCoordinate.Equals(e.hallThatWasDiscovered)) != null)
                {
                    CombatUnit cu = TheMap.EnemyLocations[TheMap.ThePlayer.DungeonLevel - 1].FirstOrDefault(x => x.DungeonCoordinate.Equals(e.hallThatWasDiscovered));
                    System.Windows.Shapes.Rectangle rct2 = new Rectangle();
                    rct2.Width = 10;
                    rct2.Height = 10;
                    rct2.StrokeThickness = 1.5;
                    rct2.Stroke = new SolidColorBrush(Colors.Red);
                    rct2.Fill = new SolidColorBrush(Colors.Red);
                    rct2.Tag = cu;
                    canvasMain.Children.Add(rct2);

                    Canvas.SetLeft(rct2, e.hallThatWasDiscovered.X * TILE_WIDTH);
                    Canvas.SetTop(rct2, e.hallThatWasDiscovered.Y * TILE_WIDTH);
                }
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
                        rct.Tag = "ROOM";

                        canvasMain.Children.Add(rct);

                        Canvas.SetLeft(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.X + e.roomTileThatWasDiscovered.X) * TILE_WIDTH);
                        Canvas.SetTop(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.Y + e.roomTileThatWasDiscovered.Y) * TILE_WIDTH);

                        if (TheMap.EnemyLocations.ContainsKey(TheMap.ThePlayer.DungeonLevel - 1) && TheMap.EnemyLocations[TheMap.ThePlayer.DungeonLevel - 1].FirstOrDefault(x => x.DungeonCoordinate.Equals(e.roomTileThatWasDiscovered.DungeonCoordinate)) != null)
                        {
                            CombatUnit cu = TheMap.EnemyLocations[TheMap.ThePlayer.DungeonLevel - 1].FirstOrDefault(x => x.DungeonCoordinate.Equals(e.roomTileThatWasDiscovered.DungeonCoordinate));
                            System.Windows.Shapes.Rectangle rct2 = new Rectangle();
                            rct2.Width = 10;
                            rct2.Height = 10;
                            rct2.StrokeThickness = 1.5;
                            rct2.Stroke = new SolidColorBrush(Colors.Red);
                            rct2.Fill = new SolidColorBrush(Colors.Red);
                            rct2.Tag = cu;
                            canvasMain.Children.Add(rct2);

                            Canvas.SetLeft(rct2, e.roomTileThatWasDiscovered.DungeonCoordinate.X * TILE_WIDTH);
                            Canvas.SetTop(rct2, e.roomTileThatWasDiscovered.DungeonCoordinate.Y * TILE_WIDTH);
                        }
                        break;

                    case Shadows.StructuralClasses.TileType.VerticalWall:
                    case Shadows.StructuralClasses.TileType.HorizontalWall:

                        rct.Width = 10;
                        rct.Height = 10;
                        rct.StrokeThickness = 1.25;
                        rct.Stroke = new SolidColorBrush(Colors.Brown);
                        rct.Fill = new SolidColorBrush(Colors.Black);
                        rct.Tag = "ROOM";
                        canvasMain.Children.Add(rct);

                        Canvas.SetLeft(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.X + e.roomTileThatWasDiscovered.X) * TILE_WIDTH);
                        Canvas.SetTop(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.Y + e.roomTileThatWasDiscovered.Y) * TILE_WIDTH);

                        break;
                    case Shadows.StructuralClasses.TileType.Door:
                        rct.Width = 10;
                        rct.Height = 10;
                        rct.StrokeThickness = 2.5;
                        rct.Stroke = new SolidColorBrush(Colors.Green);
                        rct.Fill = new SolidColorBrush(Colors.Silver);
                        rct.Tag = "ROOM";
                        canvasMain.Children.Add(rct);

                        Canvas.SetLeft(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.X + e.roomTileThatWasDiscovered.X) * TILE_WIDTH);
                        Canvas.SetTop(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.Y + e.roomTileThatWasDiscovered.Y) * TILE_WIDTH);

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
            _program.StartNewGame(_chosenBaseClass);
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
                Canvas.SetLeft(_playerDot, (_program._ovMap.ThePlayer.Location.X) * TILE_WIDTH);
                Canvas.SetTop(_playerDot, (_program._ovMap.ThePlayer.Location.Y) * TILE_WIDTH);
                Canvas.SetZIndex(_playerDot, 99);
            }

            NotifyPropertyChanged("");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
            }
            catch (Exception)
            {
            }
        }
    }
}
