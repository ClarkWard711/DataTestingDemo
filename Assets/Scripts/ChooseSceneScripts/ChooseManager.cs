using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ChooseManager : MonoBehaviour
{
	public GameObject ChoosePanel;
	public GameObject PlayerPanel;
	public Button[] PlayerPanelChosen;
	public Button[] ChoosePanelChosen;
	public Button ContinueButton;
	public bool chooseJobFin;
	public GameObject SkillPanel;
	public GameObject SkillPrefab;
	public GameObject SkillToBeChosen;
	public Text SkillDescription;
	public Image JobImage;
	public GameObject JobPanel;
	int num = 0;
	public Text basic, basic1, whatToDo;
	public GameObject playerSkill;
	public Button[] Skillbuttons;
	public bool isChooseSkillFin;
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
		Skillbuttons = playerSkill.GetComponentsInChildren<Button>();
		ContinueButton.onClick.AddListener(() => Continue());
		for (int i = 0; i < 4; i++)
		{
			int j = i;
			Skillbuttons[i].onClick.AddListener(() => SkillBackChoose(j));
		}
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
					PlayerPanelChosen[i].gameObject.GetComponent<Chosen>().selectedJob = ChoosePanelChosen[index].gameObject.GetComponent<ChangeDescription>().jobData;
					if (i == PlayerPanelChosen.Length - 1)
					{
						ContinueButton.interactable = true;
						chooseJobFin = true;
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
				PlayerPanelChosen[0].gameObject.GetComponent<Chosen>().selectedJob = ChoosePanelChosen[index].gameObject.GetComponent<ChangeDescription>().jobData;
			}
		}
	}

	public void Continue()
	{
		if (isChooseSkillFin)
		{
			isChooseSkillFin = false;
			//写入技能id
			num++;
			for (int i = 0; i < 4; i++)
			{
				Skillbuttons[i].gameObject.GetComponent<Chosen>().isChosen = false;
				Skillbuttons[i].gameObject.GetComponentInChildren<Text>().text = "-";
				Skillbuttons[i].gameObject.GetComponent<Chosen>().id = -1;
				ContinueButton.interactable = false;
				isChooseSkillFin = false;
			}
			if (num == 4)
			{
				//开始游戏
			}
		}
		if (chooseJobFin)
		{
			whatToDo.text = "选择四个技能构成你的技能面板";
			ContinueButton.interactable = false;
			SkillPanel.SetActive(true);
			JobPanel.SetActive(false);
			JobImage.sprite = PlayerPanelChosen[num].gameObject.GetComponent<Chosen>().selectedJob.JobAvatarImage;
			basic.text = PlayerPanelChosen[num].gameObject.GetComponent<Chosen>().selectedJob.JobPrefab.GetComponent<JobSkillHolder>().JobSkill.skillList[0].SkillName + PlayerPanelChosen[num].gameObject.GetComponent<Chosen>().selectedJob.JobPrefab.GetComponent<JobSkillHolder>().JobSkill.skillList[0].Description;
			basic1.text = PlayerPanelChosen[num].gameObject.GetComponent<Chosen>().selectedJob.JobPrefab.GetComponent<JobSkillHolder>().JobSkill.skillList[1].SkillName + PlayerPanelChosen[num].gameObject.GetComponent<Chosen>().selectedJob.JobPrefab.GetComponent<JobSkillHolder>().JobSkill.skillList[1].Description;
			foreach (Transform child in SkillToBeChosen.transform)
			{
				Destroy(child.gameObject);
			}
			for (int i = 2; i < PlayerPanelChosen[num].gameObject.GetComponent<Chosen>().selectedJob.JobPrefab.GetComponent<JobSkillHolder>().JobSkill.skillList.Count; i++)
			{
				var Skill = Instantiate(SkillPrefab, SkillToBeChosen.transform);
				Skill.GetComponent<ChangeDescription>().text = SkillDescription;
				Skill.GetComponent<ChangeDescription>().isSkill = true;
				Skill.GetComponentInChildren<Text>().text = PlayerPanelChosen[num].gameObject.GetComponent<Chosen>().selectedJob.JobPrefab.GetComponent<JobSkillHolder>().JobSkill.skillList[i].SkillName;
				Skill.GetComponent<ChangeDescription>().description = PlayerPanelChosen[num].gameObject.GetComponent<Chosen>().selectedJob.JobPrefab.GetComponent<JobSkillHolder>().JobSkill.skillList[i].Description;
				int id = i;
				Skill.GetComponent<Button>().onClick.AddListener(() => SkillChoose(id));
			}
		}
	}

	public void SkillChoose(int id)
	{
		for (int i = 0; i < 4; i++)
		{
			if (!Skillbuttons[i].gameObject.GetComponent<Chosen>().isChosen)
			{
				Skillbuttons[i].gameObject.GetComponent<Chosen>().isChosen = true;
				Skillbuttons[i].gameObject.GetComponentInChildren<Text>().text = PlayerPanelChosen[num].gameObject.GetComponent<Chosen>().selectedJob.JobPrefab.GetComponent<JobSkillHolder>().JobSkill.skillList[id].SkillName;
				Skillbuttons[i].gameObject.GetComponent<ChangeDescription>().description = PlayerPanelChosen[num].gameObject.GetComponent<Chosen>().selectedJob.JobPrefab.GetComponent<JobSkillHolder>().JobSkill.skillList[id].Description;
				Skillbuttons[i].gameObject.GetComponent<Chosen>().id = id;
				if (i == 3)
				{
					ContinueButton.interactable = true;
					isChooseSkillFin = true;
				}
				SkillToBeChosen.GetComponentsInChildren<Button>()[id - 2].interactable = false;
				return;
			}
		}
	}

	public void SkillBackChoose(int index)
	{
		Skillbuttons[index].gameObject.GetComponent<Chosen>().isChosen = false;
		Skillbuttons[index].gameObject.GetComponentInChildren<Text>().text = "-";
		SkillToBeChosen.GetComponentsInChildren<Button>()[Skillbuttons[index].gameObject.GetComponent<Chosen>().id - 2].interactable = true;
		Skillbuttons[index].gameObject.GetComponent<Chosen>().id = -1;
		ContinueButton.interactable = false;
		isChooseSkillFin = false;
	}
}
