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
        public GameObject monsterLair;

        private void Awake() {
            BattleMgr.instance.Init();
        }

        // Start is called before the first frame update
        void Start() {
            BattleMgr.instance.CreateMap(mapWidth, mapHeight, ground, bg, sunTower, monsterLair);
            orthographicSize = mainCamera.orthographicSize;
        }

        // Update is called once per frame
        Vector3 beginDrag = Vector3.zero;
        Vector3 mainCameraBeginPos = Vector3.zero;
        Vector3 curMouseWorldPosition;
        private float orthographicSize;
        void Update() {

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

        





    }
}
