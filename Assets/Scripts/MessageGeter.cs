using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class MessageGeter : MonoBehaviour
{
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
    public Question[] question = new Question[3];
    public MessageManager messageManager;

    private async UniTask GenerateMessage(string str)
    {
        var chatGPTConnection = new ChatGPTConnection();
        await chatGPTConnection.RequestAsync(str);
        string context = chatGPTConnection.GetMessageList();
        Regex rex = new Regex("\n+");
        context = rex.Replace(context, "\n");
        string[] lines = context.Split("\n");

        //１問当たり6行
        if (lines.Length > 6*messageManager.GetMAXQUESTIONINDEX())
        {
            Debug.Log("格納エラー");
        }
        else
        {
            for (int i=0; i<messageManager.GetMAXQUESTIONINDEX(); i++)
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
