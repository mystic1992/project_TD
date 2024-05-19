using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour {
    public GameObject[] subCubes;
    private List<SpriteRenderer> spriteRendererLis = new List<SpriteRenderer>();
    private bool isDrag;
    void Start()
    {
        foreach (var c in subCubes)
        {
            SpriteRenderer s = c.GetComponent<SpriteRenderer>();
            if (s != null)
            {
                spriteRendererLis.Add(s);
            }
        }
        isDrag = true;
    }
    private bool isCanSet = true;
    // Update is called once per frame
    void Update()
    {
        if (isDrag)
        {
            bool isAllCanSet = true;
            foreach (var s in spriteRendererLis)
            {
                int x = Mathf.RoundToInt(s.transform.position.x);
                int z = Mathf.RoundToInt(s.transform.position.z);
                int index = BattleMgr.instance.GetMapDataIndex(x, z);
                if (index > 0)
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
            MsgSend.SendMsg(MsgType.OnCubeSet, null);
            isDrag = false;
        }
        return isCanSet;
    }

}
