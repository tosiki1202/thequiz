using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultPrefab : MonoBehaviour
{
    public TextMeshProUGUI id;
    public TextMeshProUGUI ans;
    public TextMeshProUGUI time;
    public Button detailButton;

    public void RegisterResultPrefab(int id, bool correct, float time)
    {
        this.id.text = id.ToString();
        this.ans.text = correct.ToString();
        this.time.text = time.ToString("f2");
    }
}
