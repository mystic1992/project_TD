namespace Game {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using UnityEngine;
    public class ActorMgr : Singleton<ActorMgr> {
        //实例字典
        private Dictionary<int, Queue<GameObject>> m_ActorCacheDict = new Dictionary<int, Queue<GameObject>>(20);
        //预制体字典
        private Dictionary<int, GameObject> m_ActorCloneDict = new Dictionary<int, GameObject>(20);
        private Dictionary<int, Enemy> m_ActorDict = new Dictionary<int, Enemy>();
        private Transform rootNode;

        public ActorMgr()
        {
            if (rootNode == null)
            {
                GameObject go = GameObject.Find("Actor");
                rootNode = go.transform;
            }
        }
        public void CreateActor(int _id, Vector3 _pos, MonsterLair _monsterLair)
        {
            if (m_ActorCacheDict.ContainsKey(_id) || m_ActorCloneDict.ContainsKey(_id))
            {
                //因为匿名函数得创建有内存开销，所以这边是优化处理，在缓存里有该资源得时候就不需要匿名函数
                GameObject go = GetActorFromCache(_id);
                if (go != null)
                {
                    go.SetActive(true);
                    Transform transform = go.transform;
                    transform.localEulerAngles = Vector3.zero;
                    transform.position = _pos;
                    Enemy component = transform.GetComponent<Enemy>();
                    component.enabled = true;
                    component.Init(_id, _monsterLair);
                    m_ActorDict.Add(component.GetInstanceID(), component);
                }
            }
            else
            {
                GetActor(_id, (GameObject _go) => {
                    _go.SetActive(true);
                    Transform transform = _go.transform;
                    transform.position = _pos;
                    Enemy component = transform.GetComponent<Enemy>();
                    component.enabled = true;
                    component.Init(_id, _monsterLair);
                    m_ActorDict.Add(component.GetInstanceID(), component);
                });
            }
        }

        private void LoadActor(int _id, Action<GameObject> _onLoaded)
        {
            GameObject go = null;
            int prefabId = _id;
            if (m_ActorCloneDict.TryGetValue(prefabId, out go))
            {
                if (_onLoaded != null)
                {
                    _onLoaded(go);
                }
                return;
            }
            string loadingName = string.Format("actor_{0}", _id);
            string path = string.Format("actor/{0}", loadingName);
            go = Resources.Load<GameObject>(path);
            if (go != null)
            {
                m_ActorCloneDict.Add(prefabId, go);
                if (_onLoaded != null)
                {
                    _onLoaded(go);
                }
            }
            else
            {
                if (_onLoaded != null)
                {
                    _onLoaded(null);
                }
            }
            return;
            
        }

        private GameObject GetActorFromCache(int _id)
        {
            Queue<GameObject> queue;
            if (m_ActorCacheDict.TryGetValue(_id, out queue))
            {
                if (queue != null && queue.Count > 0)
                {
                    return queue.Dequeue();
                }
            }
            GameObject go = UnityEngine.Object.Instantiate<GameObject>(m_ActorCloneDict[_id]);
            go.transform.SetParent(rootNode);
            return go;
        }



        private void GetActor(int _id, Action<GameObject> _callback)
        {
            Queue<GameObject> queue;
            if (m_ActorCacheDict.TryGetValue(_id, out queue))
            {
                if (queue != null && queue.Count > 0)
                {
                    if (_callback != null)
                    {
                        _callback(queue.Dequeue());
                    }
                    return;
                }
            }
            if (m_ActorCloneDict.ContainsKey(_id))
            {
                GameObject go = UnityEngine.Object.Instantiate<GameObject>(m_ActorCloneDict[_id]);
                go.transform.SetParent(rootNode);
                if (_callback != null)
                {
                    _callback(go);
                }
                return;
            }
            Transform temprootNode = rootNode;
            Action<GameObject> temp_callback = _callback;
            LoadActor(_id, (GameObject _go) => {
                if (_go != null)
                {
                    GameObject go2 = UnityEngine.Object.Instantiate<GameObject>(_go);
                    go2.transform.SetParent(temprootNode);
                    if (temp_callback != null)
                    {
                        temp_callback(go2);
                    }
                }
            });
        }

        public void Recycle(int _id, Enemy _actor)
        {
            GameObject go = _actor.gameObject;
            if (go == null)
            {
                return;
            }
            _actor.enabled = true;
            int InstanceID = _actor.GetInstanceID();
            int id = _id;
            if (m_ActorDict.ContainsKey(InstanceID))
            {
                m_ActorDict.Remove(InstanceID);
            }
            go.SetActive(false);
            Queue<GameObject> queue;
            if (m_ActorCacheDict.TryGetValue(id, out queue))
            {
                queue.Enqueue(go);
            }
            else
            {
                queue = new Queue<GameObject>(20);
                queue.Enqueue(go);
                m_ActorCacheDict.Add(id, queue);
            }
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