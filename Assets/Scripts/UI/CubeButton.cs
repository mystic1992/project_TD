using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeButton : MonoBehaviour,IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Camera mainCamera;
    public GameObject selectGo;
    private int id;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameNodeMgr.MainCamera;
        selectGo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData _data)
    {
        //selectGo.SetActive(true);
    }

    public void OnBeginDrag(PointerEventData _data)
    {
        if (curCube == null) {
            return;
        }
        CreateMoveCube();
        Vector3 pos = mainCamera.ScreenToWorldPoint(_data.position);
        int x = Mathf.RoundToInt(pos.x);
        int z = Mathf.RoundToInt(pos.z);
        dragCube.transform.position = new Vector3(x, 0, z);
    }

    public void OnEndDrag(PointerEventData _data)
    {


    }
    private int oldCube_X;
    private int oldCube_Z;
    public void OnDrag(PointerEventData _data)
    {
        if (curCube == null) {
            return;
        }
        Vector3 pos = mainCamera.ScreenToWorldPoint(_data.position);
        int x = Mathf.RoundToInt(pos.x);
        int z = Mathf.RoundToInt(pos.z);
        dragCube.transform.position = new Vector3(x, 0, z);
        if (oldCube_X != x || oldCube_Z != z)
        {
            dragCube.GetComponent<Cube>().OnPosChange();
            oldCube_X = x;
            oldCube_Z = z;
        }
    }
    private GameObject curCube;
    
    public void CreateCube(int _id)
    {
        if (curCube != null)
        {
            GameObject.Destroy(curCube);
        }
        id = _id;
        string path = string.Format("UI/cube_{0}", id);
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject go = UnityEngine.Object.Instantiate<GameObject>(prefab);
        if (go != null)
        {
            go.transform.SetParent(this.transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            curCube = go;
        }
    }
    private GameObject dragCube;

    public void CreateMoveCube()
    {
        string path = string.Format("cube/cube_{0}", id);
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject go = UnityEngine.Object.Instantiate<GameObject>(prefab);
        if (go != null)
        {
            dragCube = go;
        }
        Cube cube = dragCube.GetComponent<Cube>();
        BattleWin.instance.SetCubeCtrl(this,cube);
        curCube.SetActive(false);
    }

    public void ShowCube() {
        curCube.SetActive(true);
        if (dragCube != null)
        {
            BattleMgr.instance.RemoveCube(dragCube.GetComponent<Cube>());
            GameObject.Destroy(dragCube);
        }
    }

    public void DestroyCube() {
        if (curCube != null) {
            GameObject.Destroy(curCube);
        }
    }
}
