using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using TMPro;

public class MessageGeter : MonoBehaviour
{
    [SerializeField] private int MAXQUESTIONINDEX = 3;
    private string Request_sentence;
    public struct Question
    {
        public string sentence;
        public string sel_1;
        public string sel_2;
        public string sel_3;
        public string sel_4;
        public int answer_index;
    }
    public static Question[] question = new Question[3];
    private async UniTask GenerateMessage(string str)
    {
        GeneUIManager.instance.SetGeneratingText("生成中・・・");
        GeneUIManager.instance.GeneratingUIDisplay();
        // APIは使用料かかるのでダミーデータをquestion[i]に入れるようにします
        //少し生成を待つコード(雰囲気的に)
        await UniTask.Delay(1200);
        string[] lines = new string[] {"問題1の文章","Q1_select1","Q1_select2","Q1_select3","Q1_select4","Q1_answerIndex",
                                        "問題2の文章","Q2_select1","Q2_select2","Q2_select3","Q2_select4","Q1_answerIndex",
                                        "問題3の文章","Q3_select1","Q3_select2","Q3_select3","Q3_select4","Q1_answerIndex"};

        //var chatGPTConnection = new ChatGPTConnection();
        //await chatGPTConnection.RequestAsync(str);
        //string context = chatGPTConnection.GetMessageList();
        //Regex rex = new Regex("\n+");
        //context = rex.Replace(context, "\n");
        //string[] lines = context.Split("\n");

        //１問当たり6行
        if (lines.Length > 6*MAXQUESTIONINDEX)
        {
            Debug.Log("格納エラー");
        }
        else
        {
            for (int i=0; i<MAXQUESTIONINDEX; i++)
            {
                int lines_index = i * 6;
                question[i].sentence = lines[lines_index];
                question[i].sel_1 = lines[lines_index+1];
                question[i].sel_2 = lines[lines_index+2];
                question[i].sel_3 = lines[lines_index+3];
                question[i].sel_4 = lines[lines_index+4];
                question[i].answer_index = int.Parse(Regex.Replace (lines[lines_index+5], @"[^0-9]", ""));
            }
        }
        GeneUIManager.instance.SetGeneratingText("生成完了！");
        await UniTask.Delay(500);
        GeneUIManager.instance.CloseGeneUI();
        SceneManager.LoadScene("QuizScene");
    }

    public async void Generator(string Request_sentence)
    {
        if (Request_sentence == null)
        {
            Debug.Log("Empty jyanru");
            return;
        }
        this.Request_sentence = Request_sentence;
        await GenerateMessage(Request_sentence);
    }
}
