﻿using RogueLike;
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
        private Ellipse _playerDot = null;
        private BackgroundWorker _bw = new BackgroundWorker();
        private List<RogueLike.OverallMap.DrawPortionEventArgs> ListOfThings = new List<RogueLike.OverallMap.DrawPortionEventArgs>();

        public MainWindow()
        {
            InitializeComponent();

            RogueLike.Program.InputNeeded += Program_InputNeeded;
            RogueLike.Program.MapCreated += Program_MapCreated;
            RogueLike.Program.PlacePlayer += Program_PlacePlayer;

            _bw.DoWork += _bw_DoWork;
           
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

                    Canvas.SetLeft(_playerDot, (RogueLike.Program._ovMap.ThePlayer.Location.X) * 10);
                    Canvas.SetTop(_playerDot, (RogueLike.Program._ovMap.ThePlayer.Location.Y) * 10);
                }
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => Program_PlacePlayer(null,null)));
            }
        }

        private void Program_MapCreated(object sender, EventArgs e)
        {
            RogueLike.Program._ovMap.RoomDiscovered += _ovMap_RoomDiscovered;
            RogueLike.Program._ovMap.HallDiscovered += _ovMap_HallDiscovered;
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
                    case RogueLike.StructuralClasses.TileType.Floor:
                        rct.Width = 10;
                        rct.Height = 10;
                        rct.StrokeThickness = .4;
                        rct.Stroke = new SolidColorBrush(Colors.Brown);
                        rct.Fill = new SolidColorBrush(Colors.Beige);

                        canvasMain.Children.Add(rct);

                        Canvas.SetLeft(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.X + e.roomTileThatWasDiscovered.X) * 10);
                        Canvas.SetTop(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.Y + e.roomTileThatWasDiscovered.Y) * 10);

                        break;

                    case RogueLike.StructuralClasses.TileType.VerticalWall:
                    case RogueLike.StructuralClasses.TileType.HorizontalWall:

                        rct.Width = 10;
                        rct.Height = 10;
                        rct.StrokeThickness = 1.25;
                        rct.Stroke = new SolidColorBrush(Colors.Brown);
                        rct.Fill = new SolidColorBrush(Colors.Black);
                        canvasMain.Children.Add(rct);

                        Canvas.SetLeft(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.X + e.roomTileThatWasDiscovered.X) * 10);
                        Canvas.SetTop(rct, (e.roomTileThatWasDiscovered.ParentRoom.Origin.Y + e.roomTileThatWasDiscovered.Y) * 10);

                        break;
                    case RogueLike.StructuralClasses.TileType.Door:
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
            RogueLike.Program.Main(new string[] { "test" });
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
                    break;
                case Key.Right:
                    RogueLike.Program.ProcessUserCommand("MOVERIGHT");
                    break;
                case Key.Up:
                    RogueLike.Program.ProcessUserCommand("MOVEUP");
                    break;
                case Key.Down:
                    RogueLike.Program.ProcessUserCommand("MOVEDOWN");
                    break;
            }

            if (_playerDot != null)
            {
                Canvas.SetLeft(_playerDot, (RogueLike.Program._ovMap.ThePlayer.Location.X) * 10);
                Canvas.SetTop(_playerDot, (RogueLike.Program._ovMap.ThePlayer.Location.Y) * 10);
                Canvas.SetZIndex(_playerDot, 99);
            }

        }
    }
}
