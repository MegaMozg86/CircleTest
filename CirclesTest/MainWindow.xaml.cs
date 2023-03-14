using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace CirclesTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Canvas _canvas = new Canvas();
        private readonly Random _random = new Random();
        private readonly List<Circle> _circles = new List<Circle>();
        public MainWindow()
        {
            InitializeComponent();

            Content = _canvas;
            _timer.Interval = TimeSpan.FromMilliseconds(16);
            _timer.Tick += OnTimerTick;
            _timer.Start();

            CreateCircle();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _canvas.Children.Clear();
            double deltaTime = _timer.Interval.TotalSeconds;
            for(int i = 0; i < _circles.Count; ++i)
            {
                for(int j = i + 1; j < _circles.Count; ++j)
                {
                    _circles[i].IsColliding(_circles[j]);
                }
            }
            foreach (var circle in _circles)
            {
                circle.IsCollidingWithWindow(this.Width, this.Height);
                circle.UpdatePosition(deltaTime);
                DrawCircle(circle.X, circle.Y, circle.Radius);
            }
        }

        private void DrawCircle(double x, double y, double radius)
        {
            var circle = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Stroke = Brushes.Blue,
                Fill = Brushes.Blue
            };
            Canvas.SetLeft(circle, x - radius);
            Canvas.SetTop(circle, y - radius);
            _canvas.Children.Add(circle);
        }

        private void CreateCircle()
        {
            double x = _random.Next(10, (int)this.Width);
            double y = _random.Next(10, (int)this.Height);
            double radius = _random.Next(10, 30);
            double speed = _random.Next(50, 150);
            double angle = _random.NextDouble() * Math.PI * 2;
            _circles.Add(new Circle(x, y, radius, speed, angle));
        }

        private void RemoveCircle()
        {
            if(_circles.Count > 0)
                _circles.Remove(_circles[0]);
        }

        private void ChangeSpeed(bool slowly = false)
        {
            double delta = 10.0;
            if (slowly)
                delta = -10.0;
            foreach (var circle in _circles)
            {
                circle.Speed += delta;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.A:
                    CreateCircle();
                    break;

                case Key.R:
                    RemoveCircle();
                    break;

                case Key.F:
                    ChangeSpeed();
                    break;

                case Key.S:
                    ChangeSpeed(true);
                    break;
            }
        }
    }
}
