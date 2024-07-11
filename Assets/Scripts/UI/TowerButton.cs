using Assets.Scripts;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public Text coinTxt;
    private Camera mainCamera;
    //public GameObject selectGo;
    private int id;
    // Start is called before the first frame update
    void Start() {
        mainCamera = GameNodeMgr.MainCamera;
        //selectGo.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (BattleMgr.instance.CoinNum >= needCoin) {
            coinTxt.color = Color.white;
        }
        else {
            coinTxt.color = Color.red;
        }
    }

    public void OnBeginDrag(PointerEventData _data) {
        if (BattleMgr.instance.CoinNum < needCoin) {
            return;
        }
        CreateMoveTower();
        Vector3 pos = mainCamera.ScreenToWorldPoint(_data.position);
        int x = Mathf.RoundToInt(pos.x);
        int z = Mathf.RoundToInt(pos.z);
        dragTower.transform.position = new Vector3(x, 0, z);
        dragTowerMono.SetOrderInLayer(3);
    }

    public void OnEndDrag(PointerEventData _data) {
        if (BattleMgr.instance.CoinNum < needCoin) {
            return;
        }
        Vector3 pos = mainCamera.ScreenToWorldPoint(_data.position);
        int x = Mathf.RoundToInt(pos.x);
        int z = Mathf.RoundToInt(pos.z);
        if (BattleMgr.instance.GetMapDataIndex(x,z) == 1) {//可以创建
            BattleMgr.instance.AddCoin(-needCoin);
            BattleMgr.instance.SetMapDataByTower(x,z);
            dragTowerMono.SetOrderInLayer(2);
        }
        else {
            BattleMgr.instance.RemoveTower(dragTowerMono);
            Destroy(dragTower);
        }

    }
    public void OnDrag(PointerEventData _data) {
        if (BattleMgr.instance.CoinNum < needCoin) {
            return;
        }
        Vector3 pos = mainCamera.ScreenToWorldPoint(_data.position);
        int x = Mathf.RoundToInt(pos.x);
        int z = Mathf.RoundToInt(pos.z);
        dragTower.transform.position = new Vector3(x, 0, z);
        if (BattleMgr.instance.GetMapDataIndex(x, z) == 1)
        {//可以创建
            dragTowerMono.SetColorRed(false);
        }
        else
        {
            dragTowerMono.SetColorRed(true);
        }
    }
    private GameObject curTower;
    private int needCoin;
    public void CreateTower(int _id, int _needCoin) {
        id = _id;
        needCoin = _needCoin;
        coinTxt.text = _needCoin.ToString();
        string path = string.Format("UI/tower_{0}", id);
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject go = UnityEngine.Object.Instantiate<GameObject>(prefab);
        if (go != null) {
            go.transform.SetParent(this.transform);
            go.transform.SetSiblingIndex(0);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            curTower = go;
        }
    }
    private GameObject dragTower;
    private Tower dragTowerMono;
    public void CreateMoveTower() {
        
        string path = string.Format("tower/Tower_{0}", id);
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject go = UnityEngine.Object.Instantiate<GameObject>(prefab);
        if (go != null) {
            dragTower = go;
            dragTowerMono = dragTower.GetComponent<Tower>();
        }
        
    }

}
