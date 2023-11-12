using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using TMPro;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private int MAXQUESTIONINDEX;
    [SerializeField] private int NowQuestionIndex;
    [SerializeField] private int DELAYSHOWMS;
    private int correct = 0;
    public TextMeshProUGUI correctAnsNum;
    public TextMeshProUGUI qNumText;
    public TextMeshProUGUI sentence_box;
    public TextMeshProUGUI sel_1_box;
    public TextMeshProUGUI sel_2_box;
    public TextMeshProUGUI sel_3_box;
    public TextMeshProUGUI sel_4_box;

    public async UniTask Q_Displaycontrol()
    {
        ClearQuizSet();
        await Show(sentence_box, MessageGeter.question[NowQuestionIndex].sentence);
        await UniTask.Delay(1000);
        sel_1_box.text = MessageGeter.question[NowQuestionIndex].sel_1;
        sel_2_box.text = MessageGeter.question[NowQuestionIndex].sel_2;
        sel_3_box.text = MessageGeter.question[NowQuestionIndex].sel_3;
        sel_4_box.text = MessageGeter.question[NowQuestionIndex].sel_4;
    }

    public async UniTask NextQuestion(){
        if(StoreButtonData.data[NowQuestionIndex].q_correct == true){
            correct += 1;
        }
        NowQuestionIndex++;
        if (NowQuestionIndex+1 > MessageGeter.question.Length)
        {
            Debug.Log("問題終了");
            await UniTask.Delay(1500);
            SceneManager.LoadScene("ResultScene");
            return;
        }
        await Q_Displaycontrol();
    }

    //表示上限数を1ずつ上げることで、いい感じに文字表示する関数
    public async UniTask Show(TextMeshProUGUI _box, string _text)
    {
        for (int i=0; i<_text.Length; i++)
        {
            _box.maxVisibleCharacters = i;
            _box.text = _text;
            await UniTask.Delay(DELAYSHOWMS);
        }
        _box.maxVisibleCharacters = _text.Length;
    }

    // 文章が入るボックスを綺麗にする関数
    public void ClearQuizSet()
    {
        sentence_box.text = null;
        sel_1_box.text = null;
        sel_2_box.text = null;
        sel_3_box.text = null;
        sel_4_box.text = null;
    }

    public void SetQuestionIndex(int idx)
    {
        NowQuestionIndex = idx;
    }
    public int GetQuestionIndex()
    {
        return NowQuestionIndex;
    }
    public int GetMAXQUESTIONINDEX()
    {
        return MAXQUESTIONINDEX;
    }
}
