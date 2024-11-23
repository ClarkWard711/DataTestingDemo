using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorEncounter : MonoBehaviour
{
    public int initialStepsToEncounter = 15; 
    private int currentSteps = 0; 
    private int stepsToEncounter;
    private bool playerInZone = false;

    private Vector3 lastPosition;

    void Start()
    {
        stepsToEncounter = initialStepsToEncounter; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            lastPosition = other.transform.position;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            currentSteps = 0;
        }
    }

    void Update()
    {
        if (playerInZone)
        {
            TrackSteps();
        }
    }

    void TrackSteps()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (Vector3.Distance(player.transform.position, lastPosition) >= 1.0f)
            {
                EncounterManager.instance.currentSteps++;
                lastPosition = player.transform.position;
                
                if (EncounterManager.instance.currentSteps >= EncounterManager.instance.stepsToEncounter)
                {
                    EncounterManager.instance.TriggerBattle();
                }
            }
        }
    }


    
}
