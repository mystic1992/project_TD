using Game;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts {
    public class Tower : MonoBehaviour {
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
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackSpeed) {
                Attack();
                attackTimer = 0;
            }
        }

        private void Attack() {
            Enemy enemy = ActorMgr.instance.GetEnemyInRange(m_Transform.position, attackRange);
            if (enemy != null) { // If there is an enemy in range
                Vector3 targetPos = enemy.transform.position;
                Vector3 dir = targetPos - m_Transform.position;
                BulletMgr.instance.CreateBullet(1, m_Transform.position, getAngle(dir.x, dir.z));
            }
            
        }

        public static float getAngle(float _x, float _y) {
            float angle = 90f - (Mathf.Atan2(_y, _x) * 57.29578f);
            angle = (angle + 360f) % 360f;
            return angle;
        }

    }
}