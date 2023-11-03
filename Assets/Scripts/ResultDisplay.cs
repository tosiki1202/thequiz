using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultDisplay : MonoBehaviour
{
    public TextMeshProUGUI id_1;
    public TextMeshProUGUI sel_1;
    public TextMeshProUGUI ans_1;
    public TextMeshProUGUI time_1;
    void Start()
    {
        id_1.text = StoreButtonData.data[1].q_num.ToString();
        sel_1.text = StoreButtonData.data[1].q_sel.ToString();
        ans_1.text = MessageGeter.question[1].answer_index.ToString();
        time_1.text = StoreButtonData.data[1].q_time.ToString();
    }
}
