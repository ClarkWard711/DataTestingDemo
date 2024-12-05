using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChooseManager : MonoBehaviour
{
	public GameObject ChoosePanel;
	public GameObject PlayerPanel;
	public void Start()
	{
		for (int i = 0; i < ChoosePanel.GetComponentsInChildren<Image>().Length; i++)
		{
			ChoosePanel.GetComponentsInChildren<Image>()[i].preserveAspect = true;
		}
		List<int> randomIndex = new List<int>();
		for (int i = 0; i < PlayerSaveController.Instance.AllJobs.CharacterList.Count; i++)
		{
			bool isUnlocked = PlayerSaveController.Instance.playerSaveData.jobUnlockState.TryGetValue(i, out isUnlocked);
			if (isUnlocked)
			{
				randomIndex.Add(i);
			}
		}
		int total = randomIndex.Count;
		for (int i = 0; i < total; i++)
		{
			int index = randomIndex[Random.Range(0, randomIndex.Count)];
			ChoosePanel.GetComponentsInChildren<Image>()[i + 1].sprite = PlayerSaveController.Instance.AllJobs.CharacterList[index].JobAvatarImage;
			ChoosePanel.GetComponentsInChildren<Image>()[i + 1].color = new Color(1, 1, 1, 1);
			ChoosePanel.GetComponentsInChildren<ChangeDescription>()[i].jobData = PlayerSaveController.Instance.AllJobs.CharacterList[index];
			randomIndex.Remove(index);
		}
	}
}
