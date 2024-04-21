using Game;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleWinBg : MonoBehaviour, IDragHandler,IPointerDownHandler, IPointerUpHandler, IBeginDragHandler
{
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameNodeMgr.MainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Vector3 BeginDragPos;
    public void OnBeginDrag(PointerEventData _data)
    {
        BeginDragPos = mainCamera.ScreenToWorldPoint(_data.position);
    }
    public void OnDrag(PointerEventData _data)
    {
        Vector3 pos = mainCamera.ScreenToWorldPoint(_data.position);
        Vector3 pos1 = pos - BeginDragPos;
        mainCamera.transform.position -= pos1;
    }

    public void OnPointerDown(PointerEventData _data)
    {

    }

    public void OnPointerUp(PointerEventData _data)
    {

    }
}
