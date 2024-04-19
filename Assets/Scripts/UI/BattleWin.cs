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

    private void Awake() {
        pauseBtn.onClick.AddListener(OnPauseBtnClick);
        playBtn.onClick.AddListener(OnPlayBtnClick);
        speedBtn.onClick.AddListener(OnSpeedBtnClick);
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
