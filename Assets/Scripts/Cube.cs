using Game;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {
    public GameObject[] subCubes;
    private List<SpriteRenderer> spriteRendererLis = new List<SpriteRenderer>();
    private bool isDrag;
    void Start()
    {
        BattleMgr.instance.AddCube(this);
        foreach (var c in subCubes)
        {
            SpriteRenderer s = c.GetComponent<SpriteRenderer>();
            if (s != null)
            {
                spriteRendererLis.Add(s);
            }
            s.sortingOrder = 2;
        }
        isDrag = true;
    }
    private bool isCanSet = true;
    private bool isShake = false;
    // Update is called once per frame
    void Update()
    {
        if (isShake)
        {
            shakeTiming += Time.deltaTime;
            if (shakeTiming >= Launch.instance.shakeTime)
            {
                this.transform.position = startShakePos;
                shakeTiming = 0;
                isShake = false;
                return;
            }
            Vector3 tempPos = new Vector3(startShakePos.x + Launch.instance.curve_x.Evaluate(shakeTiming), startShakePos.y , startShakePos.z + Launch.instance.curve_y.Evaluate(shakeTiming));
            this.transform.position = tempPos;
            return;
        }
        if (isDrag)
        {
            bool isAllCanSet = true;
            foreach (var s in spriteRendererLis)
            {
                int x = Mathf.RoundToInt(s.transform.position.x);
                int z = Mathf.RoundToInt(s.transform.position.z);
                bool isRed = !BattleMgr.instance.GetCubeCanSet(this, x, z);
                if (isRed)
                {
                    s.color = Color.red;
                    isAllCanSet = false;
                }
                else
                {
                    s.color = Color.white;
                }
            }
            isCanSet = isAllCanSet;
        }

    }
    public void OnPosChange()
    {
        BattleMgr.instance.ResetMapData();
        MsgSend.SendMsg(MsgType.OnCubeSet, null);
    }

    public void SetMapData()
    {
        foreach (var c in subCubes)
        {
            int x = Mathf.RoundToInt(c.transform.position.x);
            int z = Mathf.RoundToInt(c.transform.position.z);
            BattleMgr.instance.SetMapIndex(x, z, 1);
        }
    }

    public bool SetMap()
    {
        if (isCanSet)
        {
            foreach (var c in subCubes)
            {
                int x = Mathf.RoundToInt(c.transform.position.x);
                int z = Mathf.RoundToInt(c.transform.position.z);
                BattleMgr.instance.SetMapIndex(x, z, 1);
            }
            foreach (var s in spriteRendererLis)
            {
                s.sortingOrder = 1;
            }
            MsgSend.SendMsg(MsgType.OnCubeSet, null);
            isDrag = false;
        }
        else
        {
            StartShake();
        }
        return isCanSet;
    }
    private Vector3 startShakePos = Vector3.zero;
    private float shakeTiming;
    private void StartShake()
    {
        isShake = true;
        startShakePos = this.transform.position;
        shakeTiming = 0;
    }
}
