using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultDisplay : MonoBehaviour
{
    public TextMeshProUGUI genre;
    public TextMeshProUGUI id_1;
    public TextMeshProUGUI id_2;
    public TextMeshProUGUI id_3;
    public TextMeshProUGUI ans_1;
    public TextMeshProUGUI ans_2;
    public TextMeshProUGUI ans_3;
    public TextMeshProUGUI time_1;
    public TextMeshProUGUI time_2;
    public TextMeshProUGUI time_3;
    void Start()
    {
        genre.text = MessageGeter.genre;
        id_1.text = StoreButtonData.data[0].q_num.ToString();
        id_2.text = StoreButtonData.data[1].q_num.ToString();
        id_3.text = StoreButtonData.data[2].q_num.ToString();
        ans_1.text = StoreButtonData.data[0].q_correct.ToString();
        ans_2.text = StoreButtonData.data[1].q_correct.ToString();
        ans_3.text = StoreButtonData.data[2].q_correct.ToString();
        time_1.text = StoreButtonData.data[0].q_time.ToString();
        time_2.text = StoreButtonData.data[1].q_time.ToString();
        time_3.text = StoreButtonData.data[2].q_time.ToString();
    }
}
