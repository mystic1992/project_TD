using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLair : MonoBehaviour
{
    private float CreateEnemyInterval;
    // Start is called before the first frame update
    void Start()
    {
        CreateEnemyInterval = Random.Range(0.1f, 1f);
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
}
