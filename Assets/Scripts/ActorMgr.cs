namespace Game {
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    public class ActorMgr : Singleton<ActorMgr> {
        private Dictionary<int, Enemy> m_ActorDict = new Dictionary<int, Enemy>();
        public void AddActor(Enemy actor) {
            m_ActorDict.Add(actor.GetInstanceID(), actor);
        }
        public void RemoveActor(Enemy actor) {
            m_ActorDict.Remove(actor.GetInstanceID());
        }

        public Enemy GetEnemyInRange(Vector3 pos, float range) {
            foreach (var actor in m_ActorDict.Values) {
                if (Vector3.Distance(actor.transform.position, pos) < range) {
                    return actor;
                }
            }
            return null;
        }

    }
}