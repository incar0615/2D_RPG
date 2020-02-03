using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public PlayerCharacterData PlayerData { get; set; }
    public bool isMainCharacter;
    
    private Vector3 moveTargetPos;
    private Transform followTargetTr;
    private SpriteRenderer sprRenderer;
    // Use this for initialization
    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(State)
        {
            case CharacterState.IDLE:
                break;

            case CharacterState.MOVE:
                Move(moveTargetPos);
                break;

            case CharacterState.FOLLOW:
                Follow();
                break;

            case CharacterState.BATTLE:
                
                if(BattleManager.Instance.targetEnemy)
                {
                    if (atkCooldown > 0)
                    {
                        atkCooldown -= Time.deltaTime;
                    }

                    Transform enemyTr = BattleManager.Instance.targetEnemy.transform;
                    // 사거리에 안닿으면 이동
                    if (Vector3.Distance(enemyTr.position, transform.position) > PlayerData.atkRange)
                    {
                        Move(BattleManager.Instance.targetEnemy.transform.position);
                    }
                    // 사거리에 닿은 경우 공격
                    else if(atkCooldown <= 0)
                    {
                        Attack(BattleManager.Instance.targetEnemy.GetComponent<EnemyCharacter>());
                    }
                }
                break;
        }
    }

    public void Attack(Character target)
    {
        // TODO 공격 애니
        target.Hit(PlayerData.atk);

        // 공격 쿨다운 초기화
        atkCooldown = PlayerData.atkSpeed; 
    }

    public override void Hit(double dmg)
    {
        // TODO 피격 애니 

        PlayerData.curHp -= dmg;
        if (PlayerData.curHp <= 0)
        {
            // 플레이어의 경우 죽음 처리 안하고 일단 체력 회복하도록
            // PlayerData.curHp = 0;
            PlayerData.curHp = 10;

            Die();
        }
        else
        {
            StartCoroutine(Hitted());
        }
        UIManager.Instance.UpdatePlayerInfos();
    }

    IEnumerator Hitted()
    {
        sprRenderer.color = Color.red;

        yield return new WaitForSeconds(0.05f);

        sprRenderer.color = Color.white;
    }

    // TODO 사망처리
    public override void Die()
    {

    }

    public void SetMoveTargetPos(Vector3 targetPos)
    {
        State = CharacterState.MOVE;
        moveTargetPos = targetPos;
    }

    public void SetFollowTarget(Transform targetTr)
    {
        if (State == CharacterState.FOLLOW) return; // 이미 Follow 상태면 재 설정 필요 없음 

        State = CharacterState.FOLLOW;
        followTargetTr = targetTr;
    }

    public override void Move(Vector3 targetPos)
    {
        Vector3 deflection = targetPos - transform.position;
        if (deflection.magnitude < 0.1)
        {
            State = CharacterState.IDLE;
            BattleManager.Instance.DetectAround();
        }
        else
        {
            transform.Translate(deflection.normalized * 0.2f);
        }
    }
    public void Follow()
    {
        Vector3 deflection = followTargetTr.position - transform.position;
        deflection.z = 0;

        if (deflection.magnitude > 3.0)
        {
           transform.Translate(deflection.normalized * 0.2f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Unit" && !isMainCharacter && State != CharacterState.BATTLE)
        {
            Vector3 deflection = transform.position - col.transform.position;
            deflection.z = 0;

            transform.Translate(deflection.normalized * 0.2f);
        }
        
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Unit" && !isMainCharacter && State != CharacterState.BATTLE)
        {
            Vector3 deflection = transform.position - col.transform.position;
            deflection.z = 0;

            transform.Translate(deflection.normalized * 0.2f);
        }
    }
}

