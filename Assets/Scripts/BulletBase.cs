
using UnityEngine;

namespace Game {
    public class BulletBase : MonoBehaviour {
        private Transform m_Transform;
        private Vector3 moveDir;
        private float moveSpeed = 20f;
        private Vector3 curPos;
        protected int RaycastLayerMask;//子弹射线检测mask
        private void Awake() {
            m_Transform = GetComponent<Transform>();
            RaycastLayerMask = LayerMgr.BulletTrigger;
        }
        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            float deltaTime = Time.deltaTime;
            UpdatePostion(deltaTime);
            curPos = m_Transform.position;
            UpdateTrigger(deltaTime);
            UpdateLiveTime(deltaTime);
        }
        public static float Sin(float _angle) =>
            Mathf.Sin((_angle * 3.141593f) / 180f);

        public static float Cos(float _angle) =>
            Mathf.Cos((_angle * 3.141593f) / 180f);
        public void Init(float _moveAngle) {
            curPos = m_Transform.position;
            moveDir = new Vector3(Sin(_moveAngle), Cos(_moveAngle), 0);
        }

        protected virtual void UpdateTrigger(float _deltaTime) {
            Vector3 moveDirV3 = new Vector3(moveDir.x, 0f, moveDir.y);
            //这个后面要改成射线,用CapsuleCastAll太耗了
            int count = GetRaycastHit(moveDirV3, _deltaTime);
            if (count > 0) {
                Enemy actor = null;
                for (int i = 0; i < count; i++) {
                    RaycastHit hit = rayCastHitCache[i];
                    if (hit.collider.gameObject.layer == LayerMgr.Enemy) {
                        //判断是否穿透,是否弹射
                        actor = hit.collider.GetComponent<Enemy>();
                        if (actor != null) {
                            break;
                        }
                    }
                }
                if (actor != null) {
                    actor.Dead();
                    //击中处理，子弹回收
                    Recycle();
                }
            }
            return;
        }
        protected RaycastHit[] rayCastHitCache = new RaycastHit[5];
        protected virtual int GetRaycastHit(Vector3 _moveDir, float _deltaTime) {
            int count = 0;
            float frameDistance = moveSpeed * _deltaTime;
            if (frameDistance < 0.2f) {//防止子弹速度过慢导致打不中人
                frameDistance = 0.2f;
            }
            //起始坐标运算
            Vector3 beginPos = curPos - frameDistance * _moveDir;
            count = Physics.RaycastNonAlloc(beginPos, _moveDir.normalized, rayCastHitCache, frameDistance, RaycastLayerMask);
            return count;
        }
        private Vector3 tagPosition;
        protected virtual void UpdatePostion(float _deltaTime) {
            float realSpeed = moveSpeed;
            tagPosition.x = curPos.x + moveDir.x * realSpeed * _deltaTime;
            tagPosition.z = curPos.z + moveDir.y * realSpeed * _deltaTime;
            m_Transform.position = tagPosition;
        }
        private float liveTime = 0;
        private void UpdateLiveTime(float _deltaTime) {
            liveTime += _deltaTime;
            if (liveTime >= 3) {
                Recycle();
            }
        }

        public void Recycle() {
            //回收
            BulletMgr.instance.Recycle(1,this);
        }
    }
}