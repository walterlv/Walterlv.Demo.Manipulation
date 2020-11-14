using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Walterlv.Demo
{
    public class MultiTouchCalculator
    {
        private readonly Dictionary<int, Point> _points = new Dictionary<int, Point>();

        public IReadOnlyDictionary<int, Point> Points => _points;

        public void Start()
        {
            _points.Clear();
            Log($"=========================================");
        }

        private Point _lastCenter;
        private double _lastLength = 1.0;

        /// <summary>
        /// TODO: 在这里添加算法。
        /// </summary>
        /// <param name="id">触摸点 Id，不同手指对应不同 Id，相同手指对应相同 Id。</param>
        /// <param name="position">触摸点坐标。</param>
        /// <returns>相比于上一次的变换量。</returns>
        public ManipulationDelta Report(int id, Point position)
        {
            // 准备状态。
            Vector translation = default;
            double scale = 1.0;
            var existed = _points.ContainsKey(id);
            _points[id] = position;
            var isMultiTouch = _points.Count > 1;

            // 计算几何中心。
            var center = Center(_points.Values.ToList());
            var length = Length(_points.Values.ToList());
            Log($"【{_points.Count}点】 几何中心：{center}  几何长度：{length}");

            // 计算变换量。
            if (existed)
            {
                // 当没有新点时。
                translation = center - _lastCenter;
                Log($" - 平移：{translation}");
                if (isMultiTouch)
                {
                    scale = length / _lastLength;
                    Log($" - 缩放：{scale}");
                }
            }
            else
            {
                // 当此点为新点时。
                Log($"加入点 {position}");
            }

            // 更新几何中心/长度。
            _lastCenter = center;
            if (isMultiTouch)
            {
                _lastLength = length;
            }

            // 返回变换量。
            //scale = 1;
            return new ManipulationDelta(
                translation,                // 平移
                0,                          // 旋转（正时针为正数）
                new Vector(scale, scale),   // 缩放比
                new Vector(0, 0));          // 扩展量（未使用）
        }

        public void Complete()
        {
            _points.Clear();
        }

        private static Point Center(IReadOnlyList<Point> points) => points is null
            ? throw new ArgumentNullException(nameof(points))
            : (points.Count switch
            {
                0 => throw new ArgumentException("至少需要有一个点才能计算几何中心。", nameof(points)),
                1 => points[0],
                2 => new Point((points[0].X + points[1].X) / 2, (points[0].Y + points[1].Y) / 2),
                _ => GeometryCenter(points),
            });

        private static Point GeometryCenter(IReadOnlyList<Point> points)
        {
            var n = points.Count;

            // 多边形的面积为：
            var area = 0.0;
            for (var i = 0; i < n - 1; i++)
            {
                var xi = points[i].X;
                var yi = points[i].Y;
                var xi1 = points[i + 1].X;
                var yi1 = points[i + 1].Y;
                area += xi * yi1 - xi1 * yi;
            }
            area /= 2;

            // 多边形的中心：
            var centerX = 0.0;
            for (var i = 0; i < n - 1; i++)
            {
                var xi = points[i].X;
                var yi = points[i].Y;
                var xi1 = points[i + 1].X;
                var yi1 = points[i + 1].Y;
                centerX += (xi + xi1) * (xi * yi1 - xi1 * yi);
            }
            centerX /= 6 * area;

            var centerY = 0.0;
            for (var i = 0; i < n - 1; i++)
            {
                var xi = points[i].X;
                var yi = points[i].Y;
                var xi1 = points[i + 1].X;
                var yi1 = points[i + 1].Y;
                centerY += (yi + yi1) * (xi * yi1 - xi1 * yi);
            }
            centerY /= 6 * area;

            return new Point(centerX, centerY);
        }

        private static double Length(IReadOnlyList<Point> points) => points is null
            ? throw new ArgumentNullException(nameof(points))
            : (points.Count switch
            {
                0 => throw new ArgumentException("至少需要有一个点才能计算几何长度。", nameof(points)),
                1 => 0,
                2 => (points[0] - points[1]).Length,
                _ => GeometryLength(points),
            });

        private static double GeometryLength(IReadOnlyList<Point> points)
        {
            var n = points.Count;

            // 多边形的面积为：
            var area = 0.0;
            for (var i = 0; i < n - 1; i++)
            {
                var xi = points[i].X;
                var yi = points[i].Y;
                var xi1 = points[i + 1].X;
                var yi1 = points[i + 1].Y;
                area += xi * yi1 - xi1 * yi;
            }
            area /= 2;

            return Math.Sqrt(Math.Abs(area));
        }

        private static void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
