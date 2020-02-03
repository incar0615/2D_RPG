using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    public double maxHp;
    public double curHp;
    public double atk;
    public double def;
    public double atkSpeed;
    public double atkRange;

    public string name;
}

public class EnemyCharacterData : CharacterData
{

}

public class PlayerCharacterData : CharacterData
{
}