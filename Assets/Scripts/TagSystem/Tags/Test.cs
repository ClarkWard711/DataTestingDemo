using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject[] playerUnits;
    public PartyMember PlayerPartyMember;
    public GameObject Position;
    public GameObject CurrentActUnit;
    Ray TargetChosenRay;
    RaycastHit2D TargetHit;
    private void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            if (PlayerPartyMember.CharacterList[i] != null)
            {
                Instantiate(PlayerPartyMember.CharacterList[i].JobPrefab);
            }
        }

        playerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");
        playerUnits[0].GetComponent<GivingData>().AddTagToCharacter(Melee.CreateInstance<Melee>());
        playerUnits[1].GetComponent<GivingData>().AddTagToCharacter(Remote.CreateInstance<Remote>());
    }

    void FixedUpdate()
    {

        TargetChosenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        TargetHit = Physics2D.Raycast(TargetChosenRay.origin, Vector2.down);
        if (TargetHit.collider != null)
        {
            if (TargetHit.collider.gameObject.tag == "PlayerUnit")
            {
                CurrentActUnit = TargetHit.collider.gameObject;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentActUnit!=null)
            {
                CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(Defense.CreateInstance<Defense>());
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(EndTurn());
        }
    }

    IEnumerator DelayedCallback(float delay)
    {
        Debug.Log("delaying");
        yield return new WaitForSeconds(delay);
        Debug.Log("delay Finished");
        // 在这里执行需要延迟的后续操作
    }

    IEnumerator EndTurn()
    {
        foreach (GameObject character in playerUnits)
        {
            foreach (var tag in character.GetComponent<GivingData>().tagList)
            {
                tag.OnTurnEndCallback();

                
                yield return StartCoroutine(DelayedCallback(2f));
            }
        }
    }
}
