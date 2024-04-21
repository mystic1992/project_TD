using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleWin : MonoBehaviour
{
    public Text coinNumTxt;
    public Button pauseBtn;
    public Button playBtn;
    public Button speedBtn;
    public CubeButton[] cubeBtns;

    private void Awake() {
        pauseBtn.onClick.AddListener(OnPauseBtnClick);
        playBtn.onClick.AddListener(OnPlayBtnClick);
        speedBtn.onClick.AddListener(OnSpeedBtnClick);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            foreach (CubeButton c in cubeBtns) {
                c.CreateCube(Random.Range(1, 8));
            }
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
}
