using Game;
using UnityEngine;

namespace Assets.Scripts {
    public class Tower : MonoBehaviour {
        public Transform attackTransform;
        public Sprite[] sprites;
        public SpriteRenderer m_SpriteRenderer;
        public int bulletId;
        private Transform m_Transform;
        private float attackRange = 15f;
        
        
        private void Awake() {
            m_Transform = gameObject.GetComponent<Transform>();
        }
        // Use this for initialization
        void Start() {

        }
        private float attackSpeed = 0.5f;
        private float attackTimer = 0f;
        // Update is called once per frame
        void Update() {
            FindTarget();
            UpdateRot();
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackSpeed && m_target != null) {
                Attack();
                attackTimer = 0;
            }
            UpdateSprite();
        }
        private Enemy m_target; 
        private void FindTarget()
        {
            if (m_target == null)
            {
                m_target = ActorMgr.instance.GetEnemyInRange(m_Transform.position, attackRange);
            }
            else
            {
                if (Vector3.Distance(m_target.transform.position, m_Transform.position) > attackRange || !m_target.isActiveAndEnabled)
                {
                    m_target = ActorMgr.instance.GetEnemyInRange(m_Transform.position, attackRange);
                }
            }
        }

        private void UpdateRot()
        {
            //if (m_target != null)
            //{
            //    Vector3 targetPos = m_target.transform.position;
            //    Vector3 dir = targetPos - m_Transform.position;
            //    float angle = getAngle(dir.x, dir.z);
            //    attackTransform.localEulerAngles = new Vector3(0,0, angle);
            //}
        }
        private bool isAttacked = false;
        private void Attack() {
            Vector3 targetPos = m_target.transform.position;
            Vector3 dir = targetPos - m_Transform.position;
            float angle = getAngle(dir.x, dir.z);
            attackTransform.localEulerAngles = new Vector3(0, 0, angle);
            BulletMgr.instance.CreateBullet(bulletId, m_Transform.position, angle);
            m_SpriteRenderer.sprite = sprites[1];
            isAttacked = true;
        }
        private float timer;
        private void UpdateSprite()
        {
            if (isAttacked)
            {
                timer += Time.deltaTime;
                if (timer >= 0.3f)
                {
                    timer = 0;
                    isAttacked = false;
                    m_SpriteRenderer.sprite = sprites[0];
                }
            }
            
        }

        public static float getAngle(float _x, float _y) {
            float angle = 90f - (Mathf.Atan2(_y, _x) * 57.29578f);
            angle = (angle + 360f) % 360f;
            return angle;
        }

    }
}