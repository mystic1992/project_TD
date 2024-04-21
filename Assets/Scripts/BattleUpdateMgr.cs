using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Game {
    public class BattleUpdateMgr : MonoBehaviour {
        public int mapWidth = 10;
        public int mapHeight = 10;
        public GameObject ground;
        public GameObject bg;
        public Camera mainCamera;
        public GameObject block;
        public GameObject sunTower;

        private void Awake() {
            BattleMgr.instance.Init();
        }

        // Start is called before the first frame update
        void Start() {
            BattleMgr.instance.CreateMap(mapWidth, mapHeight, ground, bg, sunTower);
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
            if (Input.GetMouseButtonUp(0)) {
                //Vector3 postion = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                //int x = Mathf.CeilToInt(postion.x - 0.5f);
                //int z = Mathf.CeilToInt(postion.z - 0.5f);
                //if (0 <= x && x < mapWidth && 0 <= z && z < mapHeight && mapData[x,z] == 0) {
                //    mapData[x, z] = 1;
                //    GameObject go = Instantiate(block, new Vector3(x, 0, z), Quaternion.identity);
                //    go.transform.SetParent(GameNodeMgr.SceneNode);
                //    BattleMgr.instance.AStarFindPath.SetMapIndex(x,z,1);
                //}

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

        



        private void CreateEnemy() {
            ActorMgr.instance.CreateActor(Random.Range(1, 6),new Vector3(Random.Range(0, mapWidth), 0, Random.Range(mapHeight - 3, mapHeight)));
        }

    }
}
