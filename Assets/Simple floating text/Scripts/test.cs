using UnityEngine;
using FlyRabbit.Text;
using TMPro;

public class test : MonoBehaviour
{
    public Transform myTransform;
    private Vector3 offset = new Vector3(0, 0.5f, 0);
    private Transform followTarget = null;
    private Vector3 textPositon = new Vector3(0, 0.5f, 0);
    private bool isFollowTarget = false;
    public bool IsFollowTarget
    {
        get { return isFollowTarget; }
        set
        {
            if (value == isFollowTarget)
            {
                return;
            }
            isFollowTarget = value;
            if (isFollowTarget)
            {
                followTarget = myTransform.transform;
                textPositon = offset;
            }
            else
            {
                followTarget = null;
            }
        }
    }
    private string content;
    // 开始
    void Start()
    {
    }

    // 更新
    void Update()
    {
        if (!isFollowTarget)
        {
            textPositon = offset + myTransform.position;
        }
        //按0可以调整文字是否跟随目标
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            IsFollowTarget = !IsFollowTarget;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Random.Range(1, 100) < 10)
            {
                content = "<b>9999999999";
                SpecialEffectText.Instance.CreatFloatingText(StyleDictionary.styleDictionary[StyleList.UI_Style1Crit], content, textPositon, followTarget);
            }
            else
            {
                content = Random.Range(1, 9999).ToString();
                SpecialEffectText.Instance.CreatFloatingText(StyleDictionary.styleDictionary[StyleList.UI_Style1], content, textPositon, followTarget);
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Random.Range(1, 100) < 10)
            {
                content = "<b>9999999999";
                SpecialEffectText.Instance.CreatFloatingText(StyleDictionary.styleDictionary[StyleList.UI_Style2Crit], content, textPositon, followTarget);
            }
            else
            {
                content = Random.Range(1, 9999).ToString();
                SpecialEffectText.Instance.CreatFloatingText(StyleDictionary.styleDictionary[StyleList.UI_Style2], content, textPositon, followTarget);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (Random.Range(1, 100) < 10)
            {
                content = "<b>9999999999";
                SpecialEffectText.Instance.CreatFloatingText(StyleDictionary.styleDictionary[StyleList.UI_Style3Crit], content, textPositon, followTarget);
            }
            else
            {
                content = Random.Range(1, 9999).ToString();
                SpecialEffectText.Instance.CreatFloatingText(StyleDictionary.styleDictionary[StyleList.UI_Style3], content, textPositon, followTarget);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            content = Random.Range(1, 9999).ToString();
            SpecialEffectText.Instance.CreatFloatingText(StyleDictionary.styleDictionary[StyleList.World_Style1], content, textPositon, followTarget,true);
        }
    }
}
