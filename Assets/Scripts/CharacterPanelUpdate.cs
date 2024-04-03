using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelUpdate : MonoBehaviour
{
    public static CharacterPanelUpdate Instance;
    public int currentLevel;
    public Image CharacterAvatar;
    public Text jobName,level, hp, sp, pa, sa, pd, sd, hit, nim, spd, cri, melee, remote;
    public GameObject playerSlot;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateDataPanel(int i)
    {
        CharacterAvatar.sprite = PartyPanel.Instance.PlayerParty.CharacterList[i].JobAvatarImage;
        CharacterAvatar.color = new Color(CharacterAvatar.color.r, CharacterAvatar.color.g, CharacterAvatar.color.b, 255);
        jobName.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobName;
        level.text = "等级："+PartyPanel.Instance.PlayerParty.CharacterList[i].JobLevel.ToString();
        currentLevel = PartyPanel.Instance.PlayerParty.CharacterList[i].JobLevel;
        hp.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].maxHP.ToString();
        sp.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].maxSP.ToString();
        pa.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].physicalAttack.ToString();
        sa.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].soulAttack.ToString();
        pd.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].physicalDefence.ToString();
        sd.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].soulDefence.ToString();
        hit.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].hit.ToString();
        nim.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].nimbleness.ToString();
        spd.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].speed.ToString();
        cri.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].critical.ToString();
        melee.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].melee.ToString();
        remote.text = PartyPanel.Instance.PlayerParty.CharacterList[i].JobStatsList[currentLevel].remote.ToString();
    }
}
