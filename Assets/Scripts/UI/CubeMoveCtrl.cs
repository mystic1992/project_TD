using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeMoveCtrl : MonoBehaviour, IDragHandler{

    private Cube curCube;
    public void SetCube(Cube _cube) {
        curCube = _cube;
    }
    private int oldCube_X;
    private int oldCube_Z;
    public void OnDrag(PointerEventData _data) {
        Vector3 pos = GameNodeMgr.MainCamera.ScreenToWorldPoint(_data.position);
        int x = Mathf.RoundToInt(pos.x);
        int z = Mathf.RoundToInt(pos.z);
        curCube.transform.position = new Vector3(x, 0, z);
        if (oldCube_X != x || oldCube_Z != z)
        {
            curCube.OnPosChange();
            oldCube_X = x;
            oldCube_Z = z;
        }
    }
}
