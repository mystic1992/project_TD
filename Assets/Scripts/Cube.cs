using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour {
    public GameObject[] subCubes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMap()
    {
        foreach (var c in subCubes)
        {
            int x = (int)c.transform.position.x;
            int z = (int)c.transform.position.z;
            BattleMgr.instance.SetMapIndex(x, z);
        }
        MsgSend.SendMsg(MsgType.OnCubeSet, null);
    }

}
