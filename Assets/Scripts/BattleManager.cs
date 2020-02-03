using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    public List<GameObject> enemyList;
    public double detectionRange = 3.0f;
    //private double detectionRange;

    public GameObject targetEnemy;

    private static BattleManager instance = null;
    public static BattleManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<BattleManager>();
                if (!instance)
                {
                    instance = new GameObject("BattleManager").AddComponent<BattleManager>();
                }
            }
            return instance;
        }
    }
    // EnemySpawner 보다 빠르게 초기화 필요
    void Awake () {
        enemyList = new List<GameObject>();
    }
	
    void Start ()
    {
        
    }
	// Update is called once per frame
	void Update () {
    }

    public void BattleEnd()
    {
        PartyManager.Instance.ChangePartyState(Character.CharacterState.IDLE);
        targetEnemy = null;
        DetectAround();
    }
    public void DetectAround()
    {
        StartCoroutine(DetectEnemy());
    }

    IEnumerator DetectEnemy()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Detect");
        if(PartyManager.Instance.mainChar.State != Character.CharacterState.BATTLE && targetEnemy == null)
        {
            double minDist = 10000.0f;
            List<GameObject> closeEnemies = enemyList.FindAll(go => Vector3.Distance(PartyManager.Instance.mainChar.transform.position, go.transform.position) < detectionRange);
            
            foreach (GameObject enemy in closeEnemies)
            {
                float dist = Vector3.Distance(PartyManager.Instance.mainChar.transform.position, enemy.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    targetEnemy = enemy;
                }
            }
            if(targetEnemy)
            {
                BattleWithTarget(targetEnemy);
            }
            yield return new WaitForSeconds(0.05f);
        }
        Debug.Log("Detect End");
    }

    void BattleWithTarget(GameObject enemy)
    {
        EnemyCharacter enemyChar = enemy.GetComponent<EnemyCharacter>();
        UIManager.Instance.UpdateTargetEnemyInfo(enemyChar);

        foreach(PlayerCharacter pc in PartyManager.Instance.playerPartyList)
        {
            pc.State = Character.CharacterState.BATTLE;
        }

        enemyChar.EnterBattle();
        //enemy.GetComponent<EnemyCharacter>().State = Character.CharacterState.BATTLE;
    }
}
