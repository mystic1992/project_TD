using Game;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeMoveCtrl : MonoBehaviour, IDragHandler{

    private Cube curCube;
    public void SetCube(Cube _cube) {
        curCube = _cube;
    }

    public void OnDrag(PointerEventData _data) {
        Vector3 pos = GameNodeMgr.MainCamera.ScreenToWorldPoint(_data.position);
        int x = (int)(pos.x + 0.5f);
        int z = (int)(pos.z + 0.5f);
        curCube.transform.position = new Vector3(x, 0, z);
    }
}
