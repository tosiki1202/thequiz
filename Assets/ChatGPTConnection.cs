using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTConnection : MonoBehaviour
{
    [Serializable]
    public class ChatGPTMessageModel
    {
        public string role;
        public string content;
    }

    //ChatGPT APIにRequestを送るためのJSON用クラス
    [Serializable]
    public class ChatGPTCompletionRequestModel
    {
        public string model;
        public List<ChatGPTMessageModel> messages;
    }

    //ChatGPT APIからのResponseを受け取るためのクラス
    [System.Serializable]
    public class ChatGPTResponseModel
    {
        public string id;
        public string @object;
        public int created;
        public Choice[] choices;
        public Usage usage;

        [System.Serializable]
        public class Choice
        {
            public int index;
            public ChatGPTMessageModel message;
            public string finish_reason;
        }

        [System.Serializable]
        public class Usage
        {
            public int prompt_tokens;
            public int completion_tokens;
            public int total_tokens;
        }
    }


    private readonly string _apiKey = "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";
    //会話履歴を保持するリスト
    private readonly List<ChatGPTMessageModel> _messageList = new();

    //コンストラクタ
    public ChatGPTConnection(string apiKey)
    {
        _apiKey = apiKey;
        _messageList.Add(
            new ChatGPTMessageModel()
            {
                role = "system",
                content =
            "指定された単語をジャンルとした、ニッチな知識が必要な4択クイズを3問作成して出力してください。" +
            "出力は次の形式に従うこと。" +
            "問題：(問題文を出力)(改行)" +
            "1)(選択肢1を出力)(改行)" +
            "2)(選択肢2を出力)(改行)" +
            "3)(選択肢3を出力)(改行)" +
            "4)(選択肢4を出力)(改行)" +
            "(正解選択肢の番号のみを出力)(改行)"+
            "(改行)"+
            "問題：(問題文を出力)(改行)" +
            "1)(選択肢1を出力)(改行)" +
            "2)(選択肢2を出力)(改行)" +
            "3)(選択肢3を出力)(改行)" +
            "4)(選択肢4を出力)(改行)" +
            "(正解選択肢の番号のみを出力)(改行)"+
            "(改行)"+
            "問題：(問題文を出力)(改行)" +
            "1)(選択肢1を出力)(改行)" +
            "2)(選択肢2を出力)(改行)" +
            "3)(選択肢3を出力)(改行)" +
            "4)(選択肢4を出力)(改行)" +
            "(正解選択肢の番号のみを出力)"
            });
    }

    public async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage)
    {
        //文章生成AIのAPIのエンドポイントを設定
        var apiUrl = "https://api.openai.com/v1/chat/completions";

        _messageList.Add(new ChatGPTMessageModel { role = "user", content = userMessage });

        //OpenAIのAPIリクエストに必要なヘッダー情報を設定
        var headers = new Dictionary<string, string>
            {
                {"Authorization", "Bearer " + _apiKey},
                {"Content-type", "application/json"},
                {"X-Slack-No-Retry", "1"},
            };

        //文章生成で利用するモデルやトークン上限、プロンプトをオプションに設定
        var options = new ChatGPTCompletionRequestModel()
        {
            model = "gpt-3.5-turbo",
            messages = _messageList,
            
        };
        var jsonOptions = JsonUtility.ToJson(options);

        Debug.Log("自分:" + userMessage);

        //OpenAIの文章生成(Completion)にAPIリクエストを送り、結果を変数に格納
        using var request = new UnityWebRequest(apiUrl, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        foreach (var header in headers)
        {
            request.SetRequestHeader(header.Key, header.Value);
        }

        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
            throw new Exception();
        }
        else
        {
            var responseString = request.downloadHandler.text;
            var responseObject = JsonUtility.FromJson<ChatGPTResponseModel>(responseString);
            Debug.Log("ChatGPT:" + responseObject.choices[0].message.content);
            _messageList.Add(responseObject.choices[0].message);
            return responseObject;
        }
    }

    public string GetMessageList()
    {
        return _messageList[2].content;
    }
}
