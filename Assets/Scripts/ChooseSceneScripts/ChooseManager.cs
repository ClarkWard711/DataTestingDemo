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
	public Button[] ChoosePanelChosen;
	public Button ContinueButton;
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
		ChoosePanelChosen = ChoosePanel.GetComponentsInChildren<Button>();
		for (int i = 0; i < total; i++)
		{
			int index = randomIndex[Random.Range(0, randomIndex.Count)];
			ChoosePanel.GetComponentsInChildren<Image>()[i + 1].sprite = PlayerSaveController.Instance.AllJobs.CharacterList[index].JobAvatarImage;
			ChoosePanel.GetComponentsInChildren<Image>()[i + 1].color = new Color(1, 1, 1, 1);
			ChoosePanel.GetComponentsInChildren<ChangeDescription>()[i].jobData = PlayerSaveController.Instance.AllJobs.CharacterList[index];
			ChoosePanel.GetComponentsInChildren<ChangeDescription>()[i].index = i;
			Debug.Log(i);
			int j = i;
			ChoosePanelChosen[i].onClick.AddListener(() => Transmit(j));
			randomIndex.Remove(index);
		}
		PlayerPanelChosen = PlayerPanel.GetComponentsInChildren<Button>();
	}

	public void Transmit(int index)
	{
		Debug.Log(index);
		if (!ChoosePanelChosen[index].gameObject.GetComponent<ChangeDescription>().isChosen)
		{
			ChoosePanelChosen[index].gameObject.GetComponent<ChangeDescription>().isChosen = true;
			int temp = 0;
			for (int i = 0; i < PlayerPanelChosen.Length; i++)
			{
				if (!PlayerPanelChosen[i].gameObject.GetComponent<Chosen>().isChosen)
				{
					PlayerPanelChosen[i].gameObject.GetComponent<Image>().preserveAspect = true;
					PlayerPanelChosen[i].gameObject.GetComponent<Image>().sprite = ChoosePanelChosen[index].gameObject.GetComponent<ChangeDescription>().jobData.JobAvatarImage;
					PlayerPanelChosen[i].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
					PlayerPanelChosen[i].gameObject.GetComponent<Chosen>().isChosen = true;
					PlayerPanelChosen[i].gameObject.GetComponent<Chosen>().buttonIndex = ChoosePanelChosen[index].gameObject.GetComponent<ChangeDescription>().index;
					if (i == PlayerPanelChosen.Length)
					{
						ContinueButton.interactable = true;
					}
					return;
				}
				else
				{
					temp++;
				}
			}
			if (temp == PlayerPanelChosen.Length)
			{
				PlayerPanelChosen[0].gameObject.GetComponent<Image>().sprite = ChoosePanelChosen[index].gameObject.GetComponent<ChangeDescription>().jobData.JobAvatarImage;
				ChoosePanel.GetComponentsInChildren<ChangeDescription>()[PlayerPanelChosen[0].gameObject.GetComponent<Chosen>().buttonIndex].isChosen = false;
				PlayerPanelChosen[0].gameObject.GetComponent<Chosen>().buttonIndex = ChoosePanelChosen[index].gameObject.GetComponent<ChangeDescription>().index;
			}
		}
	}
}
