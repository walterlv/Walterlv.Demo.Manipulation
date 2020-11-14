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
        public ManipulationDelta Move(int id, Point position)
        {
            // 准备状态。
            Vector translation = default;
            double scale = 1.0;
            var existed = _points.ContainsKey(id);
            _points[id] = position;
            var isMultiTouch = _points.Count > 1;

            // 计算算数中心和算数长度平均值。
            var center = Center(_points.Values.ToList());
            var length = Length(_points.Values.ToList());
            Log($"点集：{string.Join("; ", _points)}");
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
            return new ManipulationDelta(
                translation,                // 平移
                0,                          // 旋转（正时针为正数）
                new Vector(scale, scale),   // 缩放比
                new Vector(0, 0));          // 扩展量（未使用）
        }

        public void Up(int id)
        {
            var isMultiTouch = _points.Count > 1;
            var removed = _points.Remove(id);
            if (_points.Count == 0)
            {
                _lastCenter = default;
                _lastLength = 1.0;
            }
            else if (removed)
            {
                var center = Center(_points.Values.ToList());
                _lastCenter = center;
                if (isMultiTouch)
                {
                    var length = Length(_points.Values.ToList());
                    _lastLength = length;
                }
            }
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
            var center = new Point(points.Average(x => x.X), points.Average(x => x.Y));
            return center;
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
            var center = new Point(points.Average(x => x.X), points.Average(x => x.Y));
            return points.Average(x => (x - center).Length);
        }

        private static void Log(string message)
        {
            //Debug.WriteLine(message);
        }
    }
}
