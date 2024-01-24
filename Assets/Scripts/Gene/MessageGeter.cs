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
using Photon.Pun;

public class MessageGeter : MonoBehaviourPunCallbacks
{
    [SerializeField] private int MAXQUESTIONINDEX = 3;
    public static MessageGeter instance;
    private string Request_sentence;
    public static string genre;
    
    //Question[]の定義はPhoton.Punの中に移動させました
    public static Question[] question = new Question[3];

    private void Awake()
    {
        instance = this;
        for (int i=0; i<MAXQUESTIONINDEX; i++)
        {
            question[i].sentence = "";
            question[i].sel_1 = "";
            question[i].sel_2 = "";
            question[i].sel_3 = "";
            question[i].sel_4 = "";
            question[i].answer_index = 0;
        }
    }
    private async UniTask GenerateMessage(string str)
    {
        genre = str;
        GeneUIManager.instance.SetGeneratingText("Generating...");
        GeneUIManager.instance.GeneratingUIDisplay();
        // APIは使用料かかるのでダミーデータをquestion[i]に入れるようにします
        // 少し生成を待つコード(雰囲気的に)
        
        // await UniTask.Delay(1200);  
        
        // string[] lines = new string[6*MAXQUESTIONINDEX];
        // for (int i=0; i<MAXQUESTIONINDEX; i++)
        // {
        //     int lines_index = i * 6;
        //     lines[lines_index] = "問題"+ (i+1) + "の文章";
        //     lines[lines_index+1] = "Q"+ (i+1) + "_select1";
        //     lines[lines_index+2] = "Q"+ (i+1) + "_select2";
        //     lines[lines_index+3] = "Q"+ (i+1) + "_select3";
        //     lines[lines_index+4] = "Q"+ (i+1) + "_select4";
        //     lines[lines_index+5] = "Q"+ (i+1) + "_answerIndex";
        // }
        
        
        var chatGPTConnection = new ChatGPTConnection();
        await chatGPTConnection.RequestAsync(str);
        string context = chatGPTConnection.GetMessageList();
        Regex rex = new Regex("\n+");
        context = rex.Replace(context, "\n");
        string[] lines = context.Split("\n");
        

        //１問当たり6行、格納できていないならエラー処理
        if (lines.Length != 6*MAXQUESTIONINDEX)
        {
            GeneUIManager.instance.SetGeneratingText("An error occrred. Please try it again.");
            await UniTask.Delay(2000);
            GeneUIManager.instance.CloseMenuUI();
            GeneUIManager.instance.geneInputPanel.SetActive(true);
            return;
        }

        for (int i=0; i<MAXQUESTIONINDEX; i++)
        {
            int lines_index = i * 6;
            if(!lines[lines_index+5].Any(char.IsDigit))
            {
                GeneUIManager.instance.SetGeneratingText("An error occrred. Please try it again.");
                await UniTask.Delay(2000);
                GeneUIManager.instance.CloseMenuUI();
                GeneUIManager.instance.geneInputPanel.SetActive(true);
                return;
            }
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
        
        GeneUIManager.instance.SetGeneratingText("Success!");
        await UniTask.Delay(500);
        GeneUIManager.instance.CloseMenuUI();
        GeneUIManager.instance.readyPanel.SetActive(true);
        GeneUIManager.instance.UpdatePlayerInfo();
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
