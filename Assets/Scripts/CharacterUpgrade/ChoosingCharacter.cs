using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosingCharacter : MonoBehaviour
{
    public static ChoosingCharacter Instance;

    public Image CharacterChosen;

    public List<Image> BackgroundList;

    bool JPressed = false, KPressed = false;

    public int ID;

    public Text LevelText;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;
    }

    void Update()
    {
        if (!CharacterUpgrade.Instance.isShowed)
        {
            foreach (Image image in BackgroundList)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 100 / 255f);
            }
            CharacterChosen.color = new Color(CharacterChosen.color.r, CharacterChosen.color.g, CharacterChosen.color.b, 0);
            ID = -1;
            LevelText.text = "等级：";
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            JPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            KPressed = true;
        }
    }

    void FixedUpdate()
    {
        if (ID != -1) 
        {
            if (JPressed)
            {
                JPressed = false;

                if (CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobLevel <= 9)
                {
                    CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobLevel++;
                    LevelText.text = "等级：" + CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobLevel;
                    CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].currentHP = CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobStatsList[CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobLevel].maxHP;
                    CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].currentSP = CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobStatsList[CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobLevel].maxSP;
                    PartyPanel.Instance.UpdatePartyPanel();
                }
            }
            if (KPressed)
            {
                KPressed = false;

                if (CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobLevel >= 2)
                {
                    CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobLevel--;
                    LevelText.text = "等级：" + CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobLevel;
                    CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].currentHP = CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobStatsList[CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobLevel].maxHP;
                    CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].currentSP = CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobStatsList[CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobLevel].maxSP;
                    PartyPanel.Instance.UpdatePartyPanel();
                }
            }
        }
    }

    public void ChangeChosenCharacter(int i)
    {
        foreach (Image image in BackgroundList)
        {

            image.color = new Color(image.color.r, image.color.g, image.color.b, 100 / 255f);
        }

        BackgroundList[i].color = new Color(BackgroundList[i].color.r, BackgroundList[i].color.g, BackgroundList[i].color.b, 255);
        ID = i;
        CharacterChosen.sprite = CharacterUpgrade.Instance.PlayerParty.CharacterList[i].JobAvatarImage;
        CharacterChosen.color = new Color(CharacterChosen.color.r, CharacterChosen.color.g, CharacterChosen.color.b, 1);
        LevelText.text = "等级：" + CharacterUpgrade.Instance.PlayerParty.CharacterList[ID].JobLevel;
    }
}
