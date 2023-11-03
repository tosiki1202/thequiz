using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultSentence : MonoBehaviour
{
    public TextMeshProUGUI id_1;
    public TextMeshProUGUI id_2;
    public TextMeshProUGUI id_3;
    public TextMeshProUGUI sel_1;
    public TextMeshProUGUI sel_2;
    public TextMeshProUGUI sel_3;
    public TextMeshProUGUI ans_1;
    public TextMeshProUGUI ans_2;
    public TextMeshProUGUI ans_3;
    public TextMeshProUGUI sentence_1;
    public TextMeshProUGUI sentence_2;
    public TextMeshProUGUI sentence_3;
    public Button button;
    public GameObject QData;
    public GameObject Question;
    void Start()
    {
        button.onClick.AddListener(Transit);
        id_1.text = StoreButtonData.data[0].q_num.ToString();
        id_2.text = StoreButtonData.data[1].q_num.ToString();
        id_3.text = StoreButtonData.data[2].q_num.ToString();
        sel_1.text = StoreButtonData.data[0].q_sel.ToString();
        sel_2.text = StoreButtonData.data[1].q_sel.ToString();
        sel_3.text = StoreButtonData.data[2].q_sel.ToString();
        ans_1.text = MessageGeter.question[0].answer_index.ToString();
        ans_2.text = MessageGeter.question[1].answer_index.ToString();
        ans_3.text = MessageGeter.question[2].answer_index.ToString();
        sentence_1.text = MessageGeter.question[0].sentence;
        sentence_2.text = MessageGeter.question[1].sentence;
        sentence_3.text = MessageGeter.question[2].sentence;
    }
    public void Transit()
    {
        QData.SetActive(true);
        Question.SetActive(false);
    }
}
