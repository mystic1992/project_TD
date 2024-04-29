
using System;
using UnityEngine;

namespace Game {

    public class BattleMgr : Singleton<BattleMgr> {


        

        private AStarFindPath aStarFindPath;
        private AStarPoint sunTowerPoint;
        private int[,] mapData;
        public AStarFindPath AStarFindPath {
            get { return aStarFindPath; }
        }

        public AStarPoint SunTowerPoint {
            get { return sunTowerPoint; }
        }

        public int MapWidth {
            get { return mapWidth; }
        }

        public int MapHeight {
            get { return mapHeight; }
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

        public void SetMapIndex(int _x, int _z)
        {
            if (0 <= _x && _x < mapWidth && 0 <= _z && _z < mapHeight && mapData[_x, _z] == 0)
            {
                mapData[_x, _z] = 1;
                aStarFindPath.SetMapIndex(_x, _z, 1);
            }
        }
        private int mapWidth;
        private int mapHeight;
        public void CreateMap(int _mapWidth, int _mapHeight, GameObject ground, GameObject bg, GameObject sunTower, GameObject _monsterLair)
        {
            mapWidth = _mapWidth;
            mapHeight = _mapHeight;
            mapData = new int[mapWidth, mapHeight];
            for (int x = 0; x < mapWidth; x++)
            {
                for (int z = 0; z < mapHeight; z++)
                {
                    mapData[x, z] = 0;
                    GameObject go = GameObject.Instantiate(ground, new Vector3(x, 0, z), Quaternion.identity);
                    go.transform.SetParent(GameNodeMgr.SceneNode);
                }
            }
            int bgWidth = (int)(mapWidth / 3f) + 1;
            int bgHeight = (int)(mapHeight / 3f) + 1;
            for (int x = 0; x < bgWidth; x++)
            {
                for (int z = 0; z < bgHeight; z++)
                {
                    GameObject go = GameObject.Instantiate(bg, new Vector3(x * 3, 0, z * 3), Quaternion.identity);
                    go.transform.SetParent(GameNodeMgr.SceneNode);
                }
            }
            BattleMgr.instance.AStarFindPath.SetMap(mapData);
            int sun_x = UnityEngine.Random.Range(0, mapWidth);
            int sun_z = UnityEngine.Random.Range(0, 3);
            GameObject sun = GameObject.Instantiate(sunTower, new Vector3(sun_x, 0, sun_z), Quaternion.identity);
            BattleMgr.instance.SetSunTower(sun_x, sun_z);
            ///创建三个怪物巢穴
            for (int x = 0; x < 3; x++) {
                GameObject go = GameObject.Instantiate(_monsterLair, new Vector3(UnityEngine.Random.Range(0, mapWidth), 0, mapHeight - 1), Quaternion.identity);
                go.transform.SetParent(GameNodeMgr.SceneNode);
            }
        }
    }
}
