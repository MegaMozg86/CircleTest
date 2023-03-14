using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CirclesTest
{
    public class Circle
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; }
        public double Speed { get; set; }
        public double Angle { get; set; }
        public Circle(double x, double y, double radius, double speed, double angle)
        {
            X = x;
            Y = y;
            Radius = radius;
            Speed = speed;
            Angle = angle;
        }
        public void UpdatePosition(double deltaTime)
        {
            Task task = new Task(() =>
            {
                X += Speed * deltaTime * Math.Cos(Angle);
                Y += Speed * deltaTime * Math.Sin(Angle);
            });
            task.Start();
        }
        public void IsColliding(Circle other)
        {
            Task task = new Task(() =>
            {
                double distance = Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
                if (distance <= Radius + other.Radius)
                {
                    Angle = Angle + Math.PI / 2;
                    other.Angle = other.Angle + Math.PI / 2;
                }
            });
            task.Start();
        }

        public void IsCollidingWithWindow(double windowWidth, double windowHeight)
        {
            Task task = new Task(() =>
            {
                if (X - Radius < 0 || X + Radius > windowWidth)
                    Angle = Math.PI - Angle;

                if (Y - Radius < 0 || Y + Radius > windowHeight)
                    Angle = Angle * -1;
            });
            task.Start();
        }
    }
}
