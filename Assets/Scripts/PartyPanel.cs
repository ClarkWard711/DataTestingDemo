using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyPanel : MonoBehaviour
{
    public static PartyPanel Instance;

    public PartyMember PlayerParty;

    public GameObject PartySlot;
    public GameObject Grid;

    public List<GameObject> PartySlotsList = new List<GameObject>();

    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;
        UpdatePartyPanel();
    }

    public void UpdatePartyPanel()
    {
        Instance.PartySlotsList.Clear();

        /*if (Grid.transform.childCount > 0) 
        {
            for (int i = 0; i < Grid.transform.childCount; i++)
            {
                Debug.Log(Grid.transform.childCount);
                Destroy(Grid.transform.GetChild(0).gameObject);
            }
        }*/
        while (Grid.transform.childCount != 0)
        {
             DestroyImmediate(Grid.transform.GetChild(0).gameObject);
        }
        
        for (int i = 0; i < 6; i++)
        {
            //Debug.Log(i);
            if (PlayerParty.CharacterList[i] != null)
            {
                Instance.PartySlotsList.Add(Instantiate(Instance.PartySlot));
                for (int j = 0; j < PartySlotsList.Count; j++)
                {
                    //Debug.Log("dierci" + j);
                    if (PartySlotsList[j] != null)
                    {
                        Instance.PartySlotsList[j].transform.SetParent(Instance.Grid.transform);
                        CharacterPanelUpdate.Instance.UpdateDataPanel(i);
                    }
                }
            }
        }
    }
}
