using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour {

    public int partyCap = 3; // 파티 인원

    public Vector3 mousePos;
    public Camera cam;
    public GameObject unitPrefap;

    public List<PlayerCharacter> playerPartyList;
    public PlayerCharacter mainChar;

    private static PartyManager instance = null;
    public static PartyManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<PartyManager>();
                if (!instance)
                {
                    instance = new GameObject("PartyManager").AddComponent<PartyManager>();
                }
            }
            return instance;
        }
    }
    
    // UIManager Start() 보다 먼저 초기화 필요
    void Awake () {
        playerPartyList = new List<PlayerCharacter>();

        GenerateParty();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
            mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -cam.transform.position.z - 1));

            RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward * 20.0f);
            Debug.DrawRay(mousePos, transform.forward * 20, Color.red, 1.0f);
            if (hit)
            {
                //playerPartyList.Find(c => c.PlayerData.name == "char1").Hit(2);
                
                MoveParty(mousePos);
            }
        }
    }

    void GenerateParty()
    {
        for(int i = 0; i < partyCap; i++)
        {
            GameObject go = Instantiate<GameObject>(unitPrefap, transform.position - (Vector3.right * 2.0f * i), Quaternion.identity, transform);
            go.name = "char" + (i + 1).ToString();

            PlayerCharacter pcScript = go.GetComponent<PlayerCharacter>();
            if (i == 0)
            {
                pcScript.isMainCharacter = true;
                mainChar = pcScript; // 편의를 위해 메인캐릭터 저장
            }

            PlayerCharacterData newPlayerData = new PlayerCharacterData
            {
                maxHp = 10,
                curHp = 10,
                atk = 1,
                atkSpeed = 1,
                atkRange = 2.0f,
                def = 0,
                name = "char" + (i + 1).ToString()
            };

            pcScript.PlayerData = newPlayerData;

            playerPartyList.Add(pcScript);
        }
    }

    void MoveParty(Vector3 targetPos)
    {
        BattleManager.Instance.targetEnemy = null;
        //Transform mainCharTr = playerPartyList.Find(c => c.isMainCharacter).transform;
        foreach (PlayerCharacter pc in playerPartyList)
        {
            if(pc.isMainCharacter)
            {
                pc.SetMoveTargetPos(targetPos);
            }
            else
            {
                pc.SetFollowTarget(mainChar.transform);
            }
        }
    }

    public void ChangePartyState(Character.CharacterState state)
    {
        foreach (PlayerCharacter pc in playerPartyList)
        {
            pc.State = state;
        }
    }

    void BattleWithTarget()
    {

    }
}
