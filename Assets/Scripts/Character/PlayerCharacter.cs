using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public PlayerCharacterData PlayerData { get; set; }
    public bool isMainCharacter;
    
    private Vector3 moveTargetPos;
    private Transform followTargetTr;
    
    // Use this for initialization
    void Start()
    {
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
                // BattleWithTarget();
                break;
        }
    }

    public override void Hit(double dmg)
    {
        PlayerData.curHp -= dmg;
        if (PlayerData.curHp <= 0)
        {
            PlayerData.curHp = 0;

            Die();
        }
        UIManager.Instance.UpdatePlayerInfos();
    }

    // 사망처리
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

