using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleWin : MonoBehaviour
{

    private static BattleWin _g_instance;

    public static BattleWin instance {
        get {
            return _g_instance;
        }
    }


    public Text coinNumTxt;
    public Button pauseBtn;
    public Button playBtn;
    public Button speedBtn;
    public CubeButton[] cubeBtns;
    public Button cubeCancelBtn;
    public Button cubeRotBtn;
    public Button cubeConfirmBtn;
    public CubeMoveCtrl cubMoveCtrl;
    public TowerButton[] towerBtns;
    public Button startBtn;

    private CubeButton curSelectCube;
    private List<int> towerNeedCoin = new List<int>() { 10, 30, 55,15, 40, 20, 25 };
    private void Awake() {
        _g_instance = this;
        pauseBtn.onClick.AddListener(OnPauseBtnClick);
        playBtn.onClick.AddListener(OnPlayBtnClick);
        speedBtn.onClick.AddListener(OnSpeedBtnClick);
        cubeCancelBtn.onClick.AddListener(OnCubeCancelBtnClick);
        cubeRotBtn.onClick.AddListener(OnCubeRotBtnClick);
        cubeConfirmBtn.onClick.AddListener(OnCubeConfirmBtnClick);
        startBtn.onClick.AddListener(OnStartBtnClick);    

        cubMoveCtrl.gameObject.SetActive(false);
        coinNumTxt.text = "0";
        //≤‚ ‘¥˙¬Î
        foreach (TowerButton t in towerBtns) {
            int id = Random.Range(1, 7);
            t.CreateTower(id, towerNeedCoin[id]);
        }
        foreach (CubeButton c in cubeBtns) {
            c.CreateCube(Random.Range(1, 8));
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            foreach (CubeButton c in cubeBtns) {
                c.CreateCube(Random.Range(1, 8));
            }
        }

        if (curCube != null) {
            Vector3 pos = GameNodeMgr.MainCamera.WorldToScreenPoint(curCube.transform.position);
            Vector3 pos1 = GameNodeMgr.UiCamera.ScreenToWorldPoint(pos);
            cubMoveCtrl.transform.position = pos1;
        }
    }


    void OnPauseBtnClick() {
        Time.timeScale = 0;
    }

    void OnPlayBtnClick() {
        Time.timeScale = 1;
        foreach (CubeButton c in cubeBtns) {
            c.CreateCube(Random.Range(1, 8));
        }
    }

    void OnSpeedBtnClick() {
        Time.timeScale = 2;
    }

    void OnCubeCancelBtnClick() {
        if (curSelectCube != null) {
            curSelectCube.ShowCube();
        }
        HideCubeCtrl();
    }

    void OnCubeRotBtnClick() {
        curCube.transform.localEulerAngles += new Vector3(0, 90, 0);
        
    }

    void OnCubeConfirmBtnClick() {
        if (curSelectCube != null) {
            curSelectCube.DestroyCube();
        }
        curCube.SetMap();
        HideCubeCtrl();
    }

    void OnStartBtnClick() {
        BattleMgr.instance.SetWave();
        MsgSend.SendMsg(MsgType.BeginCreateMonster, null);
    }

    private Cube curCube;
    public void SetCubeCtrl(CubeButton _cubeBtn, Cube _cube) {
        if (curSelectCube != null && curSelectCube != _cubeBtn && cubMoveCtrl.gameObject.activeInHierarchy)
        {
            OnCubeConfirmBtnClick();
        }
        curSelectCube = _cubeBtn;
        curCube = _cube;
        cubMoveCtrl.gameObject.SetActive(true);
        cubMoveCtrl.SetCube(_cube);
    }



    public void HideCubeCtrl() {
        cubMoveCtrl.gameObject.SetActive(false);
        cubMoveCtrl.SetCube(null);
    }

    public void SetCoinNum(int _num)
    {
        coinNumTxt.text = _num.ToString();
    }
    
}
