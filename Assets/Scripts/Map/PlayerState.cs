using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState instance;

    public Vector3 playerPosition;
    public Quaternion playerRotation;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SaveState(Transform playerTransform)
    {
        playerPosition = playerTransform.position;
        playerRotation = playerTransform.rotation;
    }
    
    public void RestoreState(Transform playerTransform)
    {
        playerTransform.position = playerPosition;
        playerTransform.rotation = playerRotation;
        Debug.Log("restore to: " + playerPosition);
    }
}

