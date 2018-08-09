using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Touch.ViewModels;

// TODO 没有检查经纬度是否为空
// ReSharper disable PossibleInvalidOperationException

namespace Touch.Models
{
    /// <summary>
    ///     实现图片聚类
    /// 把一定范围内的图片，选取一张代表图片，上传给Google以获取游览路径
    /// </summary>
    public class PhotoClustering
    {
        private readonly List<List<int>> _clusteringResult = new List<List<int>>();
        private readonly List<ImageModel> _photoPath;
        private readonly bool[] _vis = new bool[1000];

        public PhotoClustering(List<ImageModel> photoPath)
        {
            _photoPath = photoPath;
            _photoPath.Sort();
            for (var i = 0; i <= _photoPath.Count; ++i)
                _vis[i] = false;
            /*for (int i = 0; i < _photoPath.Count; ++i)
                Debug.WriteLine(_photoPath.ElementAt(i).DateTaken);*/
        }

        private void Dfs(int nowId, ICollection<int> group)
        {
            _vis[nowId] = true;
            var nowX = _photoPath.ElementAt(nowId).Latitude.Value;
            var nowY = _photoPath.ElementAt(nowId).Longitude.Value;
            for (var i = 0; i < _photoPath.Count; ++i)
                if (!_vis[i])
                    if (Math.Abs(nowX - _photoPath.ElementAt(i).Latitude.Value) < 0.001 &&
                        Math.Abs(nowY - _photoPath.ElementAt(i).Longitude.Value) < 0.001)
                    {
                        group.Add(i);
                        Dfs(i, group);
                    }
        }

        public List<Point> GetPhotoClustering()
        {
            var path = new List<Point>();
            for (var i = 0; i < _photoPath.Count; ++i) //去掉没有经纬度的图片
                if (!(_photoPath.ElementAt(i).Latitude.HasValue && _photoPath.ElementAt(i).Longitude.HasValue))
                    _photoPath.RemoveAt(i);
            //Debug.WriteLine(_photoPath.ElementAt(i).Latitude.ToString() + _photoPath.ElementAt(i).Longitude.ToString());
            for (var i = 0; i < _photoPath.Count; ++i)
                if (!_vis[i])
                {
                    var group = new List<int>
                    {
                        i
                    };
                    Dfs(i, group);
                    _clusteringResult.Add(group);
                }
            //Debug.Write("end:" + _clusteringResult.Count);
            for (var i = 0; i < _clusteringResult.Count; ++i)
            {
                var tmp = _clusteringResult.ElementAt(i);
                /*Debug.Write("group:");
                for(int j=0;j<tmp.Count;++j)
                {
                    Debug.WriteLine(_photoPath.ElementAt(tmp[j]).DateTaken);
                    //Debug.Write(_photoPath.ElementAt(tmp[j]).Longitude);
                }*/
                path.Add(new Point(_photoPath.ElementAt(tmp[0]).Latitude.Value,
                    _photoPath.ElementAt(tmp[0]).Longitude.Value));
            }
            return path;
        }

        public List<ImageModel> UpdateImageList()
        {
            return _photoPath;
        }

        public List<List<int>> GetClusteringResult()
        {
            return _clusteringResult;
        }
    }
}