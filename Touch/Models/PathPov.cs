using System;
using Windows.Foundation;

namespace Touch.Models
{
    /// <summary>
    ///     设置路径的视角
    /// </summary>
    public class PathPov
    {
        private Point _from;
        private Point _to;

        public PathPov(Point from, Point to)
        {
            _from = from;
            _to = to;
        }

        public int GetHeading()
        {
            var tmpY = _to.X - _from.X; //纬度
            var tmpX = _to.Y - _from.Y; //经度
            if (Math.Abs(tmpY) > Math.Abs(tmpX))
                return tmpY > 0 ? 0 : 180;
            return tmpX > 0 ? 90 : 270;
        }
    }
}