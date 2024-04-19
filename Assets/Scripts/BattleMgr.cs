
using UnityEngine;

namespace Game {

    public class BattleMgr : Singleton<BattleMgr> {


        

        private AStarFindPath aStarFindPath;
        private AStarPoint sunTowerPoint;
        public AStarFindPath AStarFindPath {
            get { return aStarFindPath; }
        }

        public AStarPoint SunTowerPoint {
            get { return sunTowerPoint; }
        }

        public void Init() {
            aStarFindPath = new AStarFindPath();
        }

        public void SetSunTower(int _x, int _z) {
            sunTowerPoint = new AStarPoint();
            sunTowerPoint.x = _x;
            sunTowerPoint.y = _z;
        }

       public AStarPoint GetAStarPoint(Vector3 _pos) {
            AStarPoint p = new AStarPoint();
            p.x = (int)_pos.x;
            p.y = (int)_pos.z;
            return p;
        }
    }
}
