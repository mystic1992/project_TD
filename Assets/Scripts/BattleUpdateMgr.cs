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
            
        }

        // Update is called once per frame

        void Update() {

        }

        





    }
}
