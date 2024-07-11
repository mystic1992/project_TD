using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleWinBg : MonoBehaviour, IDragHandler,IPointerDownHandler, IPointerUpHandler, IBeginDragHandler
{
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameNodeMgr.MainCamera;
        orthographicSize = mainCamera.orthographicSize;
    }

    Vector3 beginDrag = Vector3.zero;
    Vector3 mainCameraBeginPos = Vector3.zero;
    Vector3 curMouseWorldPosition;
    private float orthographicSize;
    private Vector3 oldPosition1;
    private Vector3 oldPosition2;
    private bool isTwoFinger = false;
    void Update()
    {
        float scrollWheelVal = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelVal != 0) {
            orthographicSize -= scrollWheelVal * 6f;
            orthographicSize = Mathf.Clamp(orthographicSize, 8, 18);
            mainCamera.orthographicSize = orthographicSize;
        }

        if (Input.touchCount > 1) {
            isTwoFinger = true;
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved) {
                var tempPosition1 = Input.GetTouch(0).position;
                var tempPosition2 = Input.GetTouch(1).position;

                float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
                float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);

                //�����ϴκ����˫ָ����֮��ľ�����
                //Ȼ��ȥ����������ľ���
                orthographicSize -= (currentTouchDistance - lastTouchDistance) * Time.deltaTime;


                //�Ѿ�������ס��min��max֮��
                orthographicSize = Mathf.Clamp(orthographicSize, 8, 18);

                //������һ�δ������λ�ã����ڶԱ�
                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;

                mainCamera.orthographicSize = orthographicSize;
            }
        }
        else {
            isTwoFinger = false;
        }
    }
    private Vector3 BeginDragPos;
    public void OnBeginDrag(PointerEventData _data)
    {
        if (isTwoFinger) {
            return;
        }
        BeginDragPos = mainCamera.ScreenToWorldPoint(_data.position);
    }
    public void OnDrag(PointerEventData _data)
    {
        if (isTwoFinger) {
            return;
        }
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
