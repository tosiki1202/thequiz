using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Comparison : MonoBehaviour
{
    public TextMeshProUGUI correctNum;
    [SerializeField] private int MAXQUESTIONINDEX = 3;
    private int correctAns = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < MAXQUESTIONINDEX; i++){
            //int answer = MessageGeter.question[i].answer_index;
            //int sel = StoreButtonData.data[i].q_sel;
            if(MessageGeter.question[i].answer_index == StoreButtonData.data[i].q_sel){
                correctAns += 1;
            }
        }
        correctNum.text = correctAns.ToString();

    }
}
