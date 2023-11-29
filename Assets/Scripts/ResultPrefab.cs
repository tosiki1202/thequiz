using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultPrefab : MonoBehaviour
{
    public Sprite maru;
    public Sprite batu;
    public TextMeshProUGUI id;
    public Image ans;
    public TextMeshProUGUI time;
    public Button detailButton;

    public void RegisterResultPrefab(int id, bool correct, float time)
    {
        if(correct == true){
            this.ans.sprite = maru;
        }else{
            this.ans.sprite = batu;
        }
        this.id.text = id.ToString();
        this.time.text = time.ToString("f2");
    }
}

