using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Game {
    public class BattleUpdateMgr : MonoBehaviour {
        public GameObject cube;

        public int mapWidth = 10;
        public int mapHeight = 10;
        public GameObject ground;
        public Camera mainCamera;
        public GameObject block;
        public GameObject sunTower;
        public GameObject[] enemyPrefabs;

        private void Awake() {
            BattleMgr.instance.Init();
        }

        // Start is called before the first frame update
        void Start() {
            CreateMap();
            orthographicSize = mainCamera.orthographicSize;
        }

        // Update is called once per frame
        Vector3 beginDrag = Vector3.zero;
        Vector3 mainCameraBeginPos = Vector3.zero;
        Vector3 curMouseWorldPosition;
        private float orthographicSize;
        void Update() {
            if (Input.GetKeyUp(KeyCode.E)) {
                CreateEnemy();
            }
            if (Input.GetMouseButtonDown(0)) {
                beginDrag = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                beginDrag = new Vector3(Input.mousePosition.x * .01f, 0, Input.mousePosition.y * .01f);
                mainCameraBeginPos = mainCamera.transform.position;
            }
            if (Input.GetMouseButtonUp(0)) {
                Vector3 postion = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                int x = Mathf.CeilToInt(postion.x - 0.5f);
                int z = Mathf.CeilToInt(postion.z - 0.5f);
                if (0 <= x && x < mapWidth && 0 <= z && z < mapHeight && mapData[x,z] == 0) {
                    mapData[x, z] = 1;
                    GameObject go = Instantiate(block, new Vector3(x, 0, z), Quaternion.identity);
                    go.transform.SetParent(GameNodeMgr.SceneNode);
                    BattleMgr.instance.AStarFindPath.SetMapIndex(x,z,1);
                }

            }

            if (Input.GetMouseButton(0)) {
                curMouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                curMouseWorldPosition = new Vector3(Input.mousePosition.x * .01f, 0, Input.mousePosition.y * .01f);
            }
            float scrollWheelVal = Input.GetAxis("Mouse ScrollWheel");
            if (scrollWheelVal != 0) {
                orthographicSize -= scrollWheelVal * 6f;
                orthographicSize = Mathf.Clamp(orthographicSize, 8, 18);
                mainCamera.orthographicSize = orthographicSize;
            }
        }

        private void LateUpdate() {
            if (Input.GetMouseButton(0)) {
                Vector3 offset = curMouseWorldPosition - beginDrag;
                mainCamera.transform.position = mainCameraBeginPos - offset;
            }
        }
        private int[,] mapData;
        private void CreateMap() {
            mapData = new int[mapWidth,mapHeight];
            for (int x = 0; x < mapWidth; x++) {
                for (int z = 0; z < mapHeight; z++) {
                    mapData[x,z] = 0;
                    GameObject go = Instantiate(ground, new Vector3(x, 0, z), Quaternion.identity);
                    go.transform.SetParent(GameNodeMgr.SceneNode);
                }
            }
            BattleMgr.instance.AStarFindPath.SetMap(mapData);
            int sun_x = Random.Range(0, mapWidth);
            int sun_z = Random.Range(0, 3);
            GameObject sun = Instantiate(sunTower, new Vector3(sun_x, 0, sun_z), Quaternion.identity);
            BattleMgr.instance.SetSunTower(sun_x, sun_z);
        }


        private void CreateEnemy() {
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0,2)], new Vector3(Random.Range(0, mapWidth), 0, Random.Range(mapHeight - 3, mapHeight)), Quaternion.identity);
        }

    }

    public class TileData {
        public int x;
        public int z;
    }
}
