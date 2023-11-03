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
    [SerializeField] private int DELAYSHOWFRAME;

    public Timer timer;
    public SetButton setButton1;
    public SetButton setButton2;
    public SetButton setButton3;
    public SetButton setButton4;
    public StoreButtonData storeButtonData;
    public TextMeshProUGUI sentence_box;
    public TextMeshProUGUI sel_1_box;
    public TextMeshProUGUI sel_2_box;
    public TextMeshProUGUI sel_3_box;
    public TextMeshProUGUI sel_4_box;

    async void Start()
    {
        await Q_Displaycontrol();
    }  

    public async UniTask Q_Displaycontrol()
    {
        ClearQuizSet();
        await Show(sentence_box, MessageGeter.question[NowQuestionIndex].sentence);
        await Show(sel_1_box, MessageGeter.question[NowQuestionIndex].sel_1);
        await Show(sel_2_box, MessageGeter.question[NowQuestionIndex].sel_2);
        await Show(sel_3_box, MessageGeter.question[NowQuestionIndex].sel_3);
        await Show(sel_4_box, MessageGeter.question[NowQuestionIndex].sel_4);
        InitButtons();
        timer.GoTimer();
    }

    public async void NextQuestion(){
        NowQuestionIndex++;
        if (NowQuestionIndex+1 > MessageGeter.question.Length)
        {
            Debug.Log("問題終了");
            await UniTask.DelayFrame(300);
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
            //DELAYSHOWFRAMEだけ待つ
            await UniTask.DelayFrame(DELAYSHOWFRAME);
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
    public void InitButtons()
    {
        setButton1.InitButton();
        setButton2.InitButton();
        setButton3.InitButton();
        setButton4.InitButton();
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
