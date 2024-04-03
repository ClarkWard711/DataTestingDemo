using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NaviController : MonoBehaviour
{
    Button[] ActionButtons;
    EventSystem eventSystem;
    bool isInKeyboardMode = false;
    void Awake()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        ActionButtons = GetComponentsInChildren<Button>();
    }
    void FixedUpdate()
    {

        if (BattleSetting.Instance.State == BattleState.PlayerTurn && !BattleSetting.Instance.isWaitForPlayerToChooseUnit) 
        {
            foreach (Button button in ActionButtons)
            {
                Navigation automatic = new Navigation();
                automatic.mode = Navigation.Mode.Automatic;
                button.navigation = automatic;
            }
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")&&!isInKeyboardMode)
            {
                isInKeyboardMode = true;
                eventSystem.SetSelectedGameObject(ActionButtons[0].gameObject);
            }
        }

        if (BattleSetting.Instance.isWaitForPlayerToChooseUnit || BattleSetting.Instance.State == BattleState.EnemyTurn) 
        {
            eventSystem.SetSelectedGameObject(null);
            foreach (Button button in ActionButtons)
            {
                Navigation none = new Navigation();
                none.mode = Navigation.Mode.None;
                button.navigation = none;
            }
            isInKeyboardMode = false;
        }
    }
}    
