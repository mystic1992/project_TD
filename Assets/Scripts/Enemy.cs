
using UnityEngine;
namespace Game {
    public class Enemy : MonoBehaviour {
        public float moveSpeed = 3f;
        private Transform m_Transform;
        private AStarFindPath.CalcObject pathObj;
        private int Id;
        private void Awake() {
            m_Transform = GetComponent<Transform>();
        }
        // Update is called once per frame
        void Update() {
            if (pathObj != null) {
                int cur_x = (int)(m_Transform.position.x + 0.5f);
                int cur_z = (int)(m_Transform.position.z + 0.5f);
                int cur_index = 0;
                for (int i = pathObj.path.Count - 1; i >=0 ; i--) {
                    if (cur_x == pathObj.path[i].x && cur_z == pathObj.path[i].y) {
                        cur_index = i;
                        break;
                    }
                }
                if (cur_index == 0) {//�����յ�
                    ActorMgr.instance.Recycle(Id, this);
                    return;
                }
                else {
                    Vector3 nextPos = new Vector3(pathObj.path[cur_index - 1].x, 0, pathObj.path[cur_index - 1].y);
                    Vector3 dir = nextPos - m_Transform.position;
                    m_Transform.position += dir.normalized * Time.deltaTime * moveSpeed;
                }
                //for (int i = 0; i < pathObj.path.Count - 1; i++) {
                //    Vector3 pos1 = new Vector3(pathObj.path[i].x, 0, pathObj.path[i].y);
                //    Vector3 pos2 = new Vector3(pathObj.path[i + 1].x, 0, pathObj.path[i + 1].y);
                //    Debug.DrawLine(pos1, pos2,Color.red);
                //}
            }
        }

        public void Init(int _id)
        {
            Id = _id;
            FindPath();
        }

        private void FindPath() {
            BattleMgr.instance.AStarFindPath.FindPathAsync(BattleMgr.instance.GetAStarPoint(m_Transform.position), BattleMgr.instance.SunTowerPoint, (AStarFindPath.CalcObject _o) => {
                pathObj = _o;
            });
        }

        public void Dead() {
            ActorMgr.instance.Recycle(Id, this);
        }


    }
}
