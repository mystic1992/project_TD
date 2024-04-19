/********************************************************************
  filename: BulletMgr
  author: Mario Chen
  purpose:  子弹管理类
  Tips:子弹的创建,缓存,预加载
*********************************************************************/
namespace Game {
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    public class BulletMgr : Singleton<BulletMgr> {
        private Transform rootNode;
        //子弹实例字典
        private Dictionary<int, Queue<GameObject>> bulletDict = new Dictionary<int, Queue<GameObject>>(20);
        //子弹预制体字典
        private Dictionary<int, GameObject> bulletCloneDict = new Dictionary<int, GameObject>(20);
        //使用中的子弹列表
        private List<BulletBase> useBulletList = new List<BulletBase>(100);

        public List<BulletBase> UseBulletList {
            get {
                return useBulletList;
            }
        }

        /// <summary>
        /// 创建子弹管理类,需要跟节点
        /// </summary>
        /// <param name="_rootNode"></param>
        public BulletMgr() {
            if (rootNode == null) {
                GameObject battleBullet = GameObject.Find("Bullet");
                rootNode = battleBullet.transform;
            }
        }
        ~BulletMgr() {
        }


        /// <summary>
        /// 清空管理类
        /// </summary>
        public void Clear() {
            //todo 清空全部go
            
            
            //卸载义减少GC
            foreach(var kvp in bulletDict)
            {
                var queue = kvp.Value;
                if (queue != null) {
                    while (queue.Count > 0) {
                        GameObject.Destroy(queue.Dequeue());
                    }
                }
            }
            bulletDict.Clear();

            for (int i = useBulletList.Count - 1; i >= 0; i--) {
                GameObject.Destroy(useBulletList[i].gameObject);
            }
            useBulletList.Clear();
            //子弹基于clone加载的暂时不卸载
            //bulletCloneDict.Clear();
        }




        private void LoadBullet(int _id, Action<GameObject> _onLoaded) {
            GameObject go = null;
            int prefabId = _id;
            if (bulletCloneDict.TryGetValue(prefabId, out go)) {
                if (_onLoaded != null) {
                    _onLoaded(go);
                }
                return;
            }
            string loadingbulletName = string.Format("bullet_{0}", _id);
            if (true) {
                string path = string.Format("bullet/{0}", loadingbulletName);
                go = Resources.Load<GameObject>(path);
                if (go != null) {
                    bulletCloneDict.Add(prefabId, go);
                    if (_onLoaded != null) {
                        _onLoaded(go);
                    }
                }
                else {
                    if (_onLoaded != null) {
                        _onLoaded(null);
                    }
                }
                return;
            }
            else {
                //string assetPath = string.Format("bullet/bullet_{0}", _mainId);
                //string assetName = loadingbulletName;
                //Dictionary<long,GameObject> tempbulletCloneDict = bulletCloneDict;
                //long tempprefabId = prefabId;
                //Action<GameObject> temp_onLoaded = _onLoaded;
                //GameResModule.instance.loadAsset(assetPath,
                //    (bool _isSuc, ALAssetBundleObj _assetObj) => {
                //        if (!_isSuc) {
                //            Logs.LogError("bullet AssetBundle Load err by :" + assetPath);
                //            return;
                //        }
                //        go = _assetObj.load(assetName) as GameObject;
                //        if (go == null) {
                //            Logs.LogError("bullet AssetBundleObj Load err by :" + assetName);
                //            return;
                //        }
                //        if (!tempbulletCloneDict.ContainsKey(tempprefabId)) {
                //            tempbulletCloneDict.Add(tempprefabId, go);
                //        }
                //        if (temp_onLoaded != null) {
                //            temp_onLoaded(go);
                //        }
                //    }, null);
            }
        }

        private GameObject GetBulletFromCache(int _id) {
            Queue<GameObject> queue;
            if (bulletDict.TryGetValue(_id, out queue)) {
                if (queue != null && queue.Count > 0) {
                    return queue.Dequeue();
                }
            }
            GameObject go = UnityEngine.Object.Instantiate<GameObject>(bulletCloneDict[_id]);
            go.transform.SetParent(rootNode);
            return go;
        }



        private void GetBullet(int _id, Action<GameObject> _onGetBullet) {
            Queue<GameObject> queue;
            if (bulletDict.TryGetValue(_id, out queue)) {
                if (queue != null && queue.Count > 0) {
                    if (_onGetBullet != null) {
                        _onGetBullet(queue.Dequeue());
                    }
                    return;
                }
            }
            if (bulletCloneDict.ContainsKey(_id)) {
                GameObject go = UnityEngine.Object.Instantiate<GameObject>(bulletCloneDict[_id]);
                go.transform.SetParent(rootNode);
                if (_onGetBullet != null) {
                    _onGetBullet(go);
                }
                return;
            }
            Transform temprootNode = rootNode;
            Action<GameObject> temp_onGetBullet = _onGetBullet;
            LoadBullet(_id, (GameObject _go) => {
                if (_go != null) {
                    GameObject go2 = UnityEngine.Object.Instantiate<GameObject>(_go);
                    go2.transform.SetParent(temprootNode);
                    if (temp_onGetBullet != null) {
                        temp_onGetBullet(go2);
                    }
                }
            });
        }


        /// <summary>
        /// 回收单个子弹
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_go"></param>
        public void Recycle(int _id, BulletBase _bullet) {
            GameObject go = _bullet.gameObject;
            if (go == null) {
                return;
            }
            int prefabId = _id;
            int index = useBulletList.IndexOf(_bullet);
            if (index != -1) {
                useBulletList.RemoveAt(index);
            }
            go.SetActive(false);
            Queue<GameObject> queue;
            if (bulletDict.TryGetValue(prefabId, out queue)) {
                queue.Enqueue(go);
            }
            else {
                queue = new Queue<GameObject>(20);
                queue.Enqueue(go);
                this.bulletDict.Add(prefabId, queue);
            }
        }

        /// <summary>
        /// 回收全部激活的子弹
        /// </summary>
        public void RecycleAll() {
            for (int i = useBulletList.Count - 1; i >= 0; i--) {
                BulletBase bullet = useBulletList[i];
                bullet.Recycle();
            }
        }


        /// <summary>
        /// 创建子弹
        /// </summary>
        /// <param name="_bulletID"></param>
        /// <param name="_pos"></param>
        /// <param name="_rota"></param>
        /// <param name="_onCreate"></param>
        public void CreateBullet(int _bulletID, Vector3 _pos, float _moveAngle) {
            int bulletPrefabId = _bulletID;
            if (bulletDict.ContainsKey(bulletPrefabId) || bulletCloneDict.ContainsKey(bulletPrefabId)) {
                //因为匿名函数得创建有内存开销，所以这边是优化处理，在缓存里有该资源得时候就不需要匿名函数
                GameObject go = GetBulletFromCache(bulletPrefabId);
                if (go != null) {
                    Transform transform = go.transform;
                    transform.localRotation = Quaternion.Euler(0, _moveAngle, 0);
                    transform.position = _pos;
                    BulletBase component = transform.GetComponent<BulletBase>();
                    component.enabled = true;
                    component.Init(_moveAngle);
                    useBulletList.Add(component);
                }
            }
            else {
                GetBullet(bulletPrefabId, (GameObject _go) => {
                    Transform transform = _go.transform;
                    transform.position = _pos;
                    BulletBase component = transform.GetComponent<BulletBase>();
                    component.enabled = true;
                    component.Init(_moveAngle);
                });
            }
        }


    }
}
