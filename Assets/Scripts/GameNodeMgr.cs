/********************************************************************
  filename: GameNodeMgr
  author: Mario Chen

  purpose:  游戏节点管理类型
*********************************************************************/

namespace Game {
    using UnityEngine;
    public class GameNodeMgr {
        private static Transform m_bulletNode;

        public static Transform BulletNode {
            get {
                if (m_bulletNode == null) {
                    GameObject battleBullet = GameObject.Find("Bullet");
                    if (null == battleBullet) {
                    }
                    else {
                        m_bulletNode = battleBullet.transform;
                    }
                }
                return m_bulletNode;
            }
        }

        private static Transform m_actorNode;

        public static Transform ActorNode {
            get {
                if (m_actorNode == null) {
                    GameObject battleActor = GameObject.Find("Actor");
                    if (null == battleActor) {
                        //Logs.LogError("Roy___" + "场景内无法搜索到路径下：Actor 的物件。");
                    }
                    else {
                        m_actorNode = battleActor.transform;
                    }
                }
                return m_actorNode;
            }
        }


        private static Transform m_damageNode;

        public static Transform DamageNode {
            get {
                if (m_damageNode == null) {
                    GameObject go = GameObject.Find("Damage");
                    m_damageNode = go == null ? null : go.transform;
                }
                return m_damageNode;
            }
        }


        private static Transform m_hpSliderNode;

        public static Transform HpSliderNode {
            get {
                if (m_hpSliderNode == null) {
                    GameObject hpGo = GameObject.Find("Canvas/Canvas_bk/GameWorldUI/Hp");
                    if (null == hpGo) {

                    }
                    else {
                        m_hpSliderNode = hpGo.transform;
                    }
                }
                return m_hpSliderNode;
            }
        }



        private static Transform m_DamageEffectNode;

        public static Transform damageEffectNode {
            get {
                if (m_DamageEffectNode == null) {
                    var go = GameObject.Find("Canvas/Canvas_bk/GameWorldUI/DamageEffect");
                    if (go == null) {
                        go = new GameObject("DamageEffect");
                        var root = GameObject.Find("Canvas/Canvas_bk/GameWorldUI");
                        var rectTransform = go.AddComponent<RectTransform>();
                        rectTransform.anchorMin = new Vector2(0, 0);
                        rectTransform.anchorMax = new Vector2(0, 0);
                        rectTransform.SetParent(root.transform, false);
                        go.layer = LayerMgr.Ui;
                        Canvas canvas = go.AddComponent<Canvas>(); //增加一个canvas，跟其他UI分离开，这个节点底下的东西全是静态的
                        canvas.overrideSorting = true;
                    }
                    m_DamageEffectNode = go.transform;
                }
                return m_DamageEffectNode;
            }
        }


        private static Camera m_mainCamera;

        public static Camera MainCamera {
            get {
                if (m_mainCamera == null) {
                    GameObject mainCameraGo = GameObject.Find("CameraFollowRoot/Main Camera");
                    if (null == mainCameraGo) {
                        //Logs.LogError("Roy___" + "场景内无法搜索到路径下：CameraFollowRoot/Main Camera 的物件。");
                    }
                    else {
                        m_mainCamera = mainCameraGo.GetComponent<Camera>();
                    }
                }
                return m_mainCamera;
            }
        }

        private static Camera m_uiCamera;

        public static Camera UiCamera {
            get {
                if (m_uiCamera == null) {
                    GameObject uiCameraGo = GameObject.Find("UICamer");
                    if (null == uiCameraGo) {
                        //Logs.LogError("Roy___" + "场景内无法搜索到路径下：UICamer 的物件。");
                    }
                    else {
                        m_uiCamera = uiCameraGo.GetComponent<Camera>();
                    }
                }
                return m_uiCamera;
            }
        }




        private static Transform m_sceneNode;

        public static Transform SceneNode {
            get {
                if (m_sceneNode == null) {
                    GameObject battleScene = GameObject.Find("Scene");
                    if (null == battleScene) {
                        //Logs.LogError("Roy___" + "场景内无法搜索到路径下：Scene 的物件。");
                    }
                    else {
                        m_sceneNode = battleScene.transform;
                    }
                }
                return m_sceneNode;
            }
        }
        


        private static Transform m_AudioNode;

        public static Transform AudioNode {
            get {
                if (m_AudioNode == null) {
                    GameObject audio = GameObject.Find("Audio");
                    if (null == audio) {
                        //Logs.LogError("Roy___" + "场景内无法搜索到路径下：audio 的物件。");
                    }
                    else {
                        m_AudioNode = audio.transform;
                    }
                }
                return m_AudioNode;
            }
        }

        private static Transform m_SfxNode;

        public static Transform SfxNode {
            get {
                if (m_SfxNode == null) {
                    GameObject sfx = GameObject.Find("Sfx");
                    if (null == sfx) {
                        //Logs.LogError("Roy___" + "场景内无法搜索到路径下：audio 的物件。");
                    }
                    else {
                        m_SfxNode = sfx.transform;
                    }
                }
                return m_SfxNode;
            }
        }

    }
}