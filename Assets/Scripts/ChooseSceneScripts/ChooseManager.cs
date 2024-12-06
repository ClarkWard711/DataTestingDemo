using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChooseManager : MonoBehaviour
{
	public GameObject ChoosePanel;
	public GameObject PlayerPanel;
	public Button[] PlayerPanelChosen;
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
			ChoosePanel.GetComponentsInChildren<ChangeDescription>()[i].index = i;
			ChoosePanel.GetComponentsInChildren<Button>()[i].onClick.AddListener(() => Transmit(ChoosePanel.GetComponentsInChildren<Button>()[i]));
			randomIndex.Remove(index);
		}
		PlayerPanelChosen = PlayerPanel.GetComponentsInChildren<Button>();
	}

	public void Transmit(Button button)
	{
		if (!button.gameObject.GetComponent<ChangeDescription>().isChosen)
		{
			button.gameObject.GetComponent<ChangeDescription>().isChosen = true;
			int temp = 0;
			for (int i = 0; i < PlayerPanelChosen.Length; i++)
			{
				if (!PlayerPanelChosen[i].gameObject.GetComponent<Chosen>().isChosen)
				{
					PlayerPanelChosen[i].gameObject.GetComponent<Image>().sprite = button.gameObject.GetComponent<ChangeDescription>().jobData.JobAvatarImage;
					PlayerPanelChosen[i].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
					PlayerPanelChosen[i].gameObject.GetComponent<Chosen>().isChosen = true;
					PlayerPanelChosen[i].gameObject.GetComponent<Chosen>().buttonIndex = button.gameObject.GetComponent<ChangeDescription>().index;
				}
				else
				{
					temp++;
				}
			}
			if (temp == PlayerPanelChosen.Length)
			{
				PlayerPanelChosen[0].gameObject.GetComponent<Image>().sprite = button.gameObject.GetComponent<ChangeDescription>().jobData.JobAvatarImage;
				ChoosePanel.GetComponentsInChildren<ChangeDescription>()[PlayerPanelChosen[0].gameObject.GetComponent<Chosen>().buttonIndex].isChosen = false;
				PlayerPanelChosen[0].gameObject.GetComponent<Chosen>().buttonIndex = button.gameObject.GetComponent<ChangeDescription>().index;
			}
		}
	}
}
