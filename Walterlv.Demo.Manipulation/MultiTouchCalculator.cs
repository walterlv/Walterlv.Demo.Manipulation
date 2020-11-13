using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Walterlv.Demo
{
    public class MultiTouchCalculator
    {
        private readonly Dictionary<int, Point> _points;

        public IReadOnlyDictionary<int, Point> Points => _points;

        public void Start()
        {
        }

        public ManipulationDelta Report(int id, Point position)
        {
            // 在这里添加算法。
            return new ManipulationDelta(new Vector(0, 0), 0, new Vector(1, 1), new Vector(0, 0));
        }

        public void Complete()
        {
        }
    }
}
