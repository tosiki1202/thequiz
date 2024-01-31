using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultPrefabMulti : MonoBehaviour
{
    public Sprite maru;
    public Sprite batu;
    public TextMeshProUGUI id;
    public Image ans1;
    public TextMeshProUGUI time1;
    public Image ans2;
    public TextMeshProUGUI time2;
    public Button detailButton;

    public void RegisterResultPrefabMulti(int id, bool correct1,bool correct2, float time1,float time2)
    {
        this.id.text = id.ToString();
        if(correct1 == true){
            this.ans1.sprite = maru;
        }else{
            this.ans1.sprite = batu;
        }
        if(correct2 == true){
            this.ans2.sprite = maru;
        }else{
            this.ans2.sprite = batu;
        }
        this.time1.text = time1.ToString("f2");
        this.time2.text = time2.ToString("f2");
    }
}

