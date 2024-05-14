using Game;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterLair : MonoBehaviour
{
    private float CreateEnemyInterval;
    private LineRenderer lineRenderer;
    private bool isCreateEnemy = false;
    private void Awake() {
        lineRenderer = this.GetComponentInChildren<LineRenderer>();
        MsgSend.RegisterMsg(MsgType.OnCubeSet, OnCubeSet);
        MsgSend.RegisterMsg(MsgType.BeginCreateMonster, OnBeginCreateMonster);
        MsgSend.RegisterMsg(MsgType.PauseCreateMonster, OnPauseCreateMonster);
    }

    private void OnDestroy() {
        MsgSend.UnregisterMsg(MsgType.OnCubeSet, OnCubeSet);
        MsgSend.UnregisterMsg(MsgType.BeginCreateMonster, OnBeginCreateMonster);
        MsgSend.UnregisterMsg(MsgType.PauseCreateMonster, OnPauseCreateMonster);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        CreateEnemyInterval = Random.Range(0.1f, 1f);
        FindPath();
    }
    private float timer = 0;
    // Update is called once per frame
    void Update()
    {
        if (isCreateEnemy) {
            timer += Time.deltaTime;
            if (timer > CreateEnemyInterval) {
                timer = 0;
                CreateEnemy();
            }
        }
    }

    private void CreateEnemy() {
        ActorMgr.instance.CreateActor(Random.Range(1, 7), this.transform.position, this);
    }
    private AStarFindPath.CalcObject path;
    public AStarFindPath.CalcObject Path
    {
        get
        {
            return path;
        }
    }
    private void FindPath() {
        BattleMgr.instance.AStarFindPath.FindPathAsync(BattleMgr.instance.GetAStarPoint(transform.position), BattleMgr.instance.SunTowerPoint, (AStarFindPath.CalcObject _o) => {
            path = _o;
            DrawPath();
        });
    }

    private void DrawPath() {
        if (path != null) {
            lineRenderer.positionCount = path.path.Count;
            for (int i = 0; i < path.path.Count; i++) {
                lineRenderer.SetPosition(i, new Vector3(path.path[i].x, 0, path.path[i].y));
            }
        }
    }

    private void OnCubeSet(object _obj) {
        FindPath();
    }

    private void OnBeginCreateMonster(object _obj) {
        CreateEnemyInterval = 1f / BattleMgr.instance.WaveNum;
        isCreateEnemy = true;
    }

    private void OnPauseCreateMonster(object _obj) {
        isCreateEnemy = false;
        timer = 0;
    }

    private void ChangeCreateEnemyInterval(float _value) {
        CreateEnemyInterval = _value;
    }


}
