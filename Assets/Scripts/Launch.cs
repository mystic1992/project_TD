
using UnityEngine;
namespace Game {
    public class Launch : MonoBehaviour {


        private static Launch _g_instance;


        public static Launch instance
        {
            get
            {
                return _g_instance;
            }
        }

        public int mapWidth = 10;
        public int mapHeight = 10;
        public GameObject ground;
        public GameObject bg;
        public Camera mainCamera;
        public GameObject block;
        public GameObject sunTower;
        public GameObject monsterLair;
        public AnimationCurve curve_x;
        public AnimationCurve curve_y;
        public float shakeTime;

        private void Awake() {
            _g_instance = this;
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
