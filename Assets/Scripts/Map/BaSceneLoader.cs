using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class BaSceneLoader : MonoBehaviour
{
    public static BaSceneLoader instance;

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

    public void LoadMainScene()
    {
        StartCoroutine(LoadMainSceneCoroutine());
    }

    private IEnumerator LoadMainSceneCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("StudyExample1");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 确保场景完全加载后再恢复玩家的位置
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerState.instance.RestoreState(player.transform);
            Debug.Log("玩家位置恢复到: " + PlayerState.instance.playerPosition);
        }
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StudyExample1")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerState.instance.RestoreState(player.transform);
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}

