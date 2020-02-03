using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    public EnemyCharacterData EnemyData { get; set; }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Hit(double dmg)
    {
        EnemyData.curHp -= dmg;
        if (EnemyData.curHp <= 0)
        {
            EnemyData.curHp = 0;

            Die();
        }
    }

    // 사망처리
    public override void Die()
    {

    }
}

