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
    public static string genre;
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
        genre = str;
        GeneUIManager.instance.SetGeneratingText("生成中・・・");
        GeneUIManager.instance.GeneratingUIDisplay();
        // APIは使用料かかるのでダミーデータをquestion[i]に入れるようにします
        //少し生成を待つコード(雰囲気的に)
        
        await UniTask.Delay(1200);  
        string[] lines = new string[6*MAXQUESTIONINDEX];
        for (int i=0; i<MAXQUESTIONINDEX; i++)
        {
            int lines_index = i * 6;
            lines[lines_index] = "問題"+ (i+1) + "の文章";
            lines[lines_index+1] = "Q"+ (i+1) + "_select1";
            lines[lines_index+2] = "Q"+ (i+1) + "_select2";
            lines[lines_index+3] = "Q"+ (i+1) + "_select3";
            lines[lines_index+4] = "Q"+ (i+1) + "_select4";
            lines[lines_index+5] = "Q"+ (i+1) + "_answerIndex";
        }
        
        /*
        var chatGPTConnection = new ChatGPTConnection();
        await chatGPTConnection.RequestAsync(str);
        string context = chatGPTConnection.GetMessageList();
        Regex rex = new Regex("\n+");
        context = rex.Replace(context, "\n");
        string[] lines = context.Split("\n");
        */

        //１問当たり6行、格納できていないならエラー処理
        if (lines.Length != 6*MAXQUESTIONINDEX)
        {
            GeneUIManager.instance.SetGeneratingText("生成中に問題が発生しました。\n再度お試し下さい。");
            await UniTask.Delay(2000);
            GeneUIManager.instance.CloseGeneUI();
            return;
        }
        
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
