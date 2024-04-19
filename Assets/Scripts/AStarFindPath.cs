
namespace Game {
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    public class AStarFindPath {

        public class CalcObject {
            public AStarPoint from;
            public AStarPoint to;
            public List<AStarPoint> path = new List<AStarPoint>();
            public Action<CalcObject> callBack;

        }


        private int[,] map;
        private List<AStarPoint> openList = new List<AStarPoint>();
        private List<AStarPoint> closeList = new List<AStarPoint>();
        //private ObjectCache _m_CalcObjectPool = new ObjectCache();
        public AStarFindPath() {
        }

        public void SetMap(int[,] _map) {
            map = _map;
        }

        public void SetMapIndex(int _x, int _y, int _value) {
            map[_x, _y] = _value;
        }



        //调用者使用完成后,归还计算对象
        public void PushCalcObject(CalcObject _o) {
            //_m_CalcObjectPool.Push<CalcObject>(_o);
            _o = null;
        }



        //从openlsit里获取F值最小的点
        private AStarPoint GetMinFInOpenList() {
            AStarPoint p = null;
            for (int i = 0; i < openList.Count; i++) {
                if (p == null || p.F > openList[i].F) {
                    p = openList[i];
                }
            }
            return p;
        }
        //判断是否在closeList
        private bool IsInCloseList(int _x, int _y) {
            for (int i = 0; i < closeList.Count; i++) {
                if (closeList[i].x == _x && closeList[i].y == _y) {
                    return true;
                }
            }
            return false;
        }
        //从closeList中获得对应坐标点
        private AStarPoint GetPointInCloseList(int _x, int _y) {
            for (int i = 0; i < closeList.Count; i++) {
                if (closeList[i].x == _x && closeList[i].y == _y) {
                    return closeList[i];
                }
            }
            return null;
        }
        //判断是否在openList
        private bool IsInOpenList(int _x, int _y) {
            for (int i = 0; i < openList.Count; i++) {
                if (openList[i].x == _x && openList[i].y == _y) {
                    return true;
                }
            }
            return false;
        }
        //从openList中获得对应坐标点
        private AStarPoint GetPointInOpenList(int _x, int _y) {
            for (int i = 0; i < openList.Count; i++) {
                if (openList[i].x == _x && openList[i].y == _y) {
                    return openList[i];
                }
            }
            return null;
        }
        //获取G值
        private int GetG(AStarPoint _point) {
            if (_point.father == null) {//起点
                return 0;
            }
            if (_point.x == _point.father.x || _point.y == _point.father.y) {
                return _point.father.G + 10; //相邻的上下左右方向
            }
            else {
                return _point.father.G + 14;//相邻斜角方向
            }
        }
        //获取H值
        private int GetH(AStarPoint _point, AStarPoint _target) {
            return Math.Abs(_point.x - _target.x) * 10 + Math.Abs(_point.y - _target.y) * 10;
        }

        //上下左右寻路
        private void CheckPointIn4Direction(AStarPoint _point, AStarPoint _startPoint, ref AStarPoint _endPoint) {
            for (int x = _point.x - 1; x <= _point.x + 1; x++) {
                for (int y = _point.y - 1; y <= _point.y + 1; y++) {
                    if ((x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1)) && !(x == _point.x && y == _point.y) && !(x != _point.x && y != _point.y)) {
                        if (map[x, y] == 0 && !IsInCloseList(x, y)) {
                            if (IsInOpenList(x, y)) {
                                AStarPoint p = GetPointInOpenList(x, y);
                                int G = 0;
                                if (_point.x == p.x || _point.y == p.y) {
                                    G = _point.G + 10;
                                }
                                else {
                                    G = _point.G + 14;
                                }
                                if (G < p.G) {
                                    p.father = _point;
                                    p.G = G;
                                }
                            }
                            else {
                                AStarPoint p = new AStarPoint();
                                p.x = x;
                                p.y = y;
                                p.father = _point;
                                p.G = GetG(p);
                                p.H = GetH(p, _endPoint);
                                openList.Add(p);
                            }
                        }
                    }
                }
            }
        }

        //八发方向寻路
        private void CheckPointIn8Direction(AStarPoint _point, AStarPoint _startPoint, ref AStarPoint _endPoint) {
            for (int x = _point.x - 1; x <= _point.x + 1; x++) {
                for (int y = _point.y - 1; y <= _point.y + 1; y++) {
                    if ((x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1)) && !(x == _point.x && y == _point.y)) {
                        if (map[x, y] == 0 && !IsInCloseList(x, y)) {
                            if (IsInOpenList(x, y)) {
                                AStarPoint p = GetPointInOpenList(x, y);
                                int G = 0;
                                if (_point.x == p.x || _point.y == p.y) {
                                    G = _point.G + 10;
                                }
                                else {
                                    G = _point.G + 14;
                                }
                                if (G < p.G) {
                                    p.father = _point;
                                    p.G = G;
                                }
                            }
                            else {
                                AStarPoint p = new AStarPoint();
                                p.x = x;
                                p.y = y;
                                p.father = _point;
                                p.G = GetG(p);
                                p.H = GetH(p, _endPoint);
                                openList.Add(p);
                            }
                        }
                    }
                }
            }
        }

        public void FindPathAsync(AStarPoint _startPoint, AStarPoint _endPoint, Action<CalcObject> _callBack) {
            //CalcObject calcObejct = _m_CalcObjectPool.Get<CalcObject>();
            CalcObject calcObejct = new CalcObject();
            calcObejct.from = _startPoint;
            calcObejct.to = _endPoint;
            calcObejct.callBack = _callBack;
            FindePath(calcObejct);
            //System.Threading.ThreadPool.QueueUserWorkItem(FindePath, calcObejct);

        }

        void FindePath(System.Object _o) {
            CalcObject _calcObejct = (CalcObject)_o;
            if (_calcObejct == null) return;
            AStarPoint _startPoint = _calcObejct.from;
            AStarPoint _endPoint = _calcObejct.to;

            openList.Clear();
            closeList.Clear();
            List<AStarPoint> path = _calcObejct.path;
            openList.Add(_startPoint);
            while (!(IsInOpenList(_endPoint.x, _endPoint.y))) {
                AStarPoint pTemp = GetMinFInOpenList();
                if (pTemp == null) return;
                openList.Remove(pTemp);
                closeList.Add(pTemp);
                CheckPointIn4Direction(pTemp, _startPoint, ref _endPoint);
                if (openList.Count == 0) {
                    if (_calcObejct.callBack != null) {
                        _calcObejct.callBack(_calcObejct);
                    }
                    return;
                }
            }

            AStarPoint p = GetPointInOpenList(_endPoint.x, _endPoint.y);
            while (p.father != null) {
                path.Add(p);
                p = p.father;
            }
            path.Add(p);
            if (_calcObejct.callBack != null) {
                _calcObejct.callBack(_calcObejct);
            }
        }


    }

    public class AStarPoint {
        public int x;                //坐标x
        public int y;                //坐标y
        public int G;                //起点到当前点的开销
        public int H;                //当前点到终点的开销            
        public AStarPoint father;    //父亲节点

        public int F { get { return G + H; } }

        public AStarPoint() {

        }
    }
}