using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text[] playerCharacterInfos;

    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<UIManager>();
                if (!instance)
                {
                    instance = new GameObject("UIManager").AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    // Use this for initialization
    void Start () {
        UpdatePlayerInfos();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdatePlayerInfos()
    {
        int cnt = 0;
        foreach (PlayerCharacter pc in PartyManager.Instance.playerPartyList)
        {
            string text =
                "Character" + (cnt + 1).ToString() + " Info" +
                "\n\nname = " + pc.PlayerData.name +
                "\nhp = " + pc.PlayerData.curHp + "/" + pc.PlayerData.maxHp;
            
            playerCharacterInfos[cnt].text = text;

            cnt++;
        }
    }
}
