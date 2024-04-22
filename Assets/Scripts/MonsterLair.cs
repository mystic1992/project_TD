using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterLair : MonoBehaviour
{
    private float CreateEnemyInterval;
    private LineRenderer lineRenderer;
    private void Awake() {
        lineRenderer = this.GetComponentInChildren<LineRenderer>();
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
        timer += Time.deltaTime;
        if (timer > CreateEnemyInterval) {
            timer = 0;
            CreateEnemy();
        }
    }

    private void CreateEnemy() {
        ActorMgr.instance.CreateActor(Random.Range(1, 7), this.transform.position);
    }

    private void FindPath() {
        BattleMgr.instance.AStarFindPath.FindPathAsync(BattleMgr.instance.GetAStarPoint(transform.position), BattleMgr.instance.SunTowerPoint, (AStarFindPath.CalcObject _o) => {
            lineRenderer.positionCount = _o.path.Count;
            for (int i = 0; i < _o.path.Count; i++) {
                lineRenderer.SetPosition(i, new Vector3(_o.path[i].x, 0, _o.path[i].y));
            }
        });
    }
}
