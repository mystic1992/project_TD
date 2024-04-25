using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
        selectGo.SetActive(true);
    }

    public void OnBeginDrag(PointerEventData _data)
    {
        CreateMoveCube();
        Vector3 pos = mainCamera.ScreenToWorldPoint(_data.position);
        int x = (int)(pos.x - 0.5f);
        int z = (int)(pos.z - 0.5f);
        dragCube.transform.position = new Vector3(x, 0, z);
    }

    public void OnEndDrag(PointerEventData _data)
    {
        Cube cube = dragCube.GetComponent<Cube>();
        cube.SetMap();

    }
    public void OnDrag(PointerEventData _data)
    {
        Vector3 pos = mainCamera.ScreenToWorldPoint(_data.position);
        int x = (int)(pos.x - 0.5f);
        int z = (int)(pos.z - 0.5f);
        dragCube.transform.position = new Vector3(x, 0, z);
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
        BattleWin.instance.SetCubeCtrl(cube);
    }
}
