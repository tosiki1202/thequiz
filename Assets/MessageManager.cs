using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageManager : MonoBehaviour
{
    // Start is called before the first frame update
    public MessageGeter messageGeter;
    public StoreButtonData storeButtonData;
    public TextMeshProUGUI sentence_box;
    public TextMeshProUGUI sel_1_box;
    public TextMeshProUGUI sel_2_box;
    public TextMeshProUGUI sel_3_box;
    public TextMeshProUGUI sel_4_box;
    public TextMeshProUGUI answer_index_box;
    public TextMeshProUGUI selected_index_box;
    [SerializeField] private int NowQuestionIndex;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sentence_box.text = messageGeter.question[NowQuestionIndex].sentence;
        sel_1_box.text = messageGeter.question[NowQuestionIndex].sel_1;
        sel_2_box.text = messageGeter.question[NowQuestionIndex].sel_2;
        sel_3_box.text = messageGeter.question[NowQuestionIndex].sel_3;
        sel_4_box.text = messageGeter.question[NowQuestionIndex].sel_4;
        answer_index_box.text = messageGeter.question[NowQuestionIndex].answer_index.ToString("0");
        if (storeButtonData.data.Count == 0) return;
        selected_index_box.text = storeButtonData.data[storeButtonData.data.Count-1].id.ToString("0");
    }
    
    public void SetQuestionIndex(int idx)
    {
        NowQuestionIndex = idx;
    }
    public int GetQuestionIndex()
    {
        return NowQuestionIndex;
    }
}
