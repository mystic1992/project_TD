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
    public GameObject cubCtrl;
    public Button cubeCancelBtn;
    public Button cubeRotBtn;
    public Button cubeConfirmBtn;
    private void Awake() {
        _g_instance = this;
        pauseBtn.onClick.AddListener(OnPauseBtnClick);
        playBtn.onClick.AddListener(OnPlayBtnClick);
        speedBtn.onClick.AddListener(OnSpeedBtnClick);
        cubeCancelBtn.onClick.AddListener(OnCubeCancelBtnClick);
        cubeRotBtn.onClick.AddListener(OnCubeRotBtnClick);
        cubeConfirmBtn.onClick.AddListener(OnCubeConfirmBtnClick);

        cubCtrl.SetActive(false);
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
            cubCtrl.transform.position = pos1;
        }
    }


    void OnPauseBtnClick() {
        Time.timeScale = 0;
    }

    void OnPlayBtnClick() {
        Time.timeScale = 1;
    }

    void OnSpeedBtnClick() {
        Time.timeScale = 2;
    }

    void OnCubeCancelBtnClick() {

    }

    void OnCubeRotBtnClick() {
        curCube.transform.localEulerAngles += new Vector3(0, 90, 0);
        curCube.SetMap();
    }

    void OnCubeConfirmBtnClick() {

    }
    private Cube curCube;
    public void SetCubeCtrl(Cube _cube) {
        curCube = _cube;
        cubCtrl.SetActive(true);
    }
    
}
