using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    public EnemyCharacterData EnemyData { get; set; }

    private SpriteRenderer sprRenderer;

    private GameObject targetPlayer;

    
    // Use this for initialization
    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case CharacterState.IDLE:
                break;

            case CharacterState.MOVE:
                Move(targetPlayer.transform.position);
                break;

            case CharacterState.BATTLE:
                if (targetPlayer)
                {
                    if (atkCooldown > 0)
                    {
                        atkCooldown -= Time.deltaTime;
                    }

                    Transform enemyTr = targetPlayer.transform;
                    // 사거리에 안닿으면 이동
                    if (Vector3.Distance(enemyTr.position, transform.position) > EnemyData.atkRange)
                    {
                        Move(targetPlayer.transform.position);
                    }
                    // 사거리에 닿은 경우 공격
                    else if (atkCooldown <= 0)
                    {
                        Attack(targetPlayer.GetComponent<PlayerCharacter>());
                    }
                }
                break;
        }
    }

    public void Attack(Character target)
    {
        // TODO 공격 애니

        target.Hit(EnemyData.atk);

        // 공격 쿨다운 초기화
        atkCooldown = EnemyData.atkSpeed;
    }

    public override void Hit(double dmg)
    {
        EnemyData.curHp -= (dmg - EnemyData.def);
        if (EnemyData.curHp <= 0)
        {
            EnemyData.curHp = 0;

            Die();
        }
        else
        {
            StartCoroutine(Hitted());
        }
        UIManager.Instance.UpdateTargetEnemyInfo(this);
    }

    IEnumerator Hitted()
    {
        sprRenderer.color = Color.red;

        yield return new WaitForSeconds(0.05f);

        sprRenderer.color = Color.white;
    }
    // 사망처리
    public override void Die()
    {
        BattleManager.Instance.enemyList.Remove(gameObject);
        BattleManager.Instance.BattleEnd();
        Destroy(gameObject);
    }

    public void EnterBattle()
    {
        State = CharacterState.BATTLE;
        DetectClosestPlayerChar();
    }

    public void DetectClosestPlayerChar()
    {
        float minDist = 10000.0f;
        foreach (PlayerCharacter pc in PartyManager.Instance.playerPartyList)
        {
            float dist = Vector3.Distance(pc.transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                targetPlayer = pc.gameObject;
            }
        }
    }
}

