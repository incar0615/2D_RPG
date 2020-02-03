using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefap;
   

	// Use this for initialization
	void Start () {
        
        StartCoroutine(SpawnEnemyCoroutine());

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnEnemy()
    {
        Debug.Log("spawnEnemy");
        
        GameObject go = Instantiate<GameObject>(enemyPrefap, transform.position + Vector3.right * Random.Range(-4.0f, 4.0f) + Vector3.up * Random.Range(-4.0f, 4.0f), Quaternion.identity);
        go.name = "enemy";

        EnemyCharacter ecScript = go.GetComponent<EnemyCharacter>();

        EnemyCharacterData newEnemyData = new EnemyCharacterData
        {
            maxHp = 10,
            curHp = 10,
            atk = 1,
            atkSpeed = 1,
            atkRange = 2,
            def = 0,
            name = "enemy1"
        };

        ecScript.EnemyData = newEnemyData;

        BattleManager.Instance.enemyList.Add(go);
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(5);
            SpawnEnemy();
        }
    }
}
