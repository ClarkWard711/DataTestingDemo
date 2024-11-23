using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Edgar.Unity.Examples
{
    public abstract class SimpleGameManagerBase<STGameManager> : MonoBehaviour
        where STGameManager : class
    {
        public static STGameManager Instance;

        public GameObject Canvas;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this as STGameManager;
            }
            else if (!ReferenceEquals(Instance, this))
            {
                Debug.Log("SimpleGameManagerBase--Awake--3");
                Destroy(gameObject);
                return;
            }

            if (Canvas != null)
            {
                Canvas.SetActive(true);
            }

            //SingletonAwake();
        }

        protected virtual void SingletonAwake()
        {
            LoadNextLevel();
        }

        public abstract void LoadNextLevel();

        protected void SetLevelInfo(string text)
        {
            var canvas = GetCanvas();
            var levelInfo = canvas.transform.Find("Levelinfo")?.gameObject.GetComponent<Text>();

            if (levelInfo != null)
            {
                levelInfo.text = text;
            }
        }

        protected void ShowLoadingScreen(string loadingText, string secondaryText)
        {
            var canvas = GetCanvas();
            var loadingImage = canvas.transform.Find("LoadingImage")?.gameObject;
            var LoadingTextComponent = loadingImage?.transform.Find("LoadingText")?.gameObject.GetComponent<Text>();
            var secondaryTextComponent = loadingImage?.transform.Find("secondaryText")?.gameObject.GetComponent<Text>();
            if (loadingImage != null)
            {
                loadingImage.SetActive(true);
            }

            if (LoadingTextComponent != null)
            {
                LoadingTextComponent.text = loadingText;
            }

            if (secondaryTextComponent != null)
            {
                secondaryTextComponent.text = secondaryText;
            }

        }

        protected GameObject GetCanvas()
        {
            var canvas = Canvas ?? GameObject.Find("Canvas");
            if (canvas == null)
            {
                throw new InvalidOperationException("Canvas not found");
            }
            return canvas;
        }

        protected void HideLoadingScreen()
        {
            var canvas = GetCanvas();
            var loadingImage = canvas.transform.Find("LoadingImage")?.gameObject;

            if (loadingImage != null)
            {
                loadingImage.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (ReferenceEquals(this, Instance))
            {
                Instance = null;
            }
        }
    }
}
