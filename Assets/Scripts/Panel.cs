using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class Panel : MonoBehaviour
{
    public static Panel Instance;
    public Text level, hp, sp, pa, sa, pd, sd, hit, nim, spd, cri, melee, remote;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDataPanel()
    {
        level.text = DataInputs.Instance.playerCurrentLevel.ToString();
        hp.text = DataInputs.Instance.maxHPOfPlayer.ToString();
        sp.text = DataInputs.Instance.maxSPOfPlayer.ToString();
        pa.text = DataInputs.Instance.physicalAttackOfPlayer.ToString();
        sa.text = DataInputs.Instance.soulAttackOfPlayer.ToString();
        pd.text = DataInputs.Instance.physicalDefenceOfPlayer.ToString();
        sd.text = DataInputs.Instance.soulDefenceOfPlayer.ToString();
        hit.text = DataInputs.Instance.hitOfPlayer.ToString();
        nim.text = DataInputs.Instance.nimblenessOfPlayer.ToString();
        spd.text = DataInputs.Instance.speedOfPlayer.ToString();
        cri.text = DataInputs.Instance.criticalOfPlayer.ToString();
        melee.text = DataInputs.Instance.meleeOfPlayer.ToString();
        remote.text = DataInputs.Instance.remoteOfPlayer.ToString();
    }
}
