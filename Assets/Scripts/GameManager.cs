
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

//リアルタイムAPIのイベントコールバック。サーバーからのイベントと、OpRaiseEventを介してクライアントから送信されたイベントをカバーします。
//カスタムイベントを受信することができるようになる
public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    //イベントについて記載があるドキュメント
    //https://doc.photonengine.com/ja-jp/pun/current/gameplay/rpcsandraiseevent#_thy2n6w3gsafi04


    public List<PlayerInfo> playerList = new List<PlayerInfo>();//プレイヤー情報を扱うクラスのリスト


    public enum EventCodes : byte//自作イベント：byteは扱うデータ(0 ～ 255)
    {
        NewPlayer,//新しいプレイヤー情報をマスターに送る
        ListPlayers,//全員にプレイヤー情報を共有
        UpdateStat,//キルデス数の更新
    }

    /// <summary>
    /// ゲームの状態
    /// </summary>
    public enum GameState
    {
        Playing,
        Ending
    }

    public GameState state;//状態を格納



    private void Start()
    {
        //ネットワーク接続されていない場合
        if (!PhotonNetwork.IsConnected)
        {
            //タイトルに戻る
            SceneManager.LoadScene(0);
        }
        else//繋がっている場合
        {
            //マスターにユーザー情報を発信する
            NewPlayerGet(PhotonNetwork.NickName,MessageGeter.genre);

            //状態をゲーム中に設定する
            state = GameState.Playing;
        }
    }


    //イベント発生時に呼び出される
    public void OnEvent(EventData photonEvent)
    {

        if (photonEvent.Code < 200)//200以上はphotonが独自のイベントを使っているため200以下のみに処理をする
        {
            EventCodes eventCode = (EventCodes)photonEvent.Code;//今回のイベントコードを格納（型変換）
            object[] data = (object[])photonEvent.CustomData;//インデクサーとCustomDataKeyを介して、イベントのカスタムデータにアクセスします

            switch (eventCode)
            {
                case EventCodes.NewPlayer://発生したイベントがNewPlayerなら
                    NewPlayerSet(data);//マスターに新規ユーザー情報処理させる
                    break;

                case EventCodes.ListPlayers:
                    ListPlayersSet(data);//ユーザー情報を共有
                    break;

                case EventCodes.UpdateStat:

                    break;

              
            }
        }
    }

    public override void OnEnable()//コンポーネントがオンになると呼ばれる
    {
        //実装されているコールバック・インターフェースのコールバック用オブジェクトを登録します。
        //PhotonNetwork.AddCallbackTarget(this);//追加する
    }


    public override void OnDisable()//コンポーネントがオフになると呼ばれる
    {
        PhotonNetwork.RemoveCallbackTarget(this);//削除する
    }




    /// <summary>
    ///  新規ユーザーがネットワーク経由でマスターに自分の情報を送る
    /// </summary>
    public void NewPlayerGet(string name,string jyanru)//イベントを発生させる関数
    {
        //objectは色々な型を入れることができる：
        object[] info = new object[2];//データ格納配列を作成
        info[0] = name;//名前
        info[1] = jyanru;//ユーザー管理番号


        // RaiseEventでカスタムイベントを発生：データを送る
        PhotonNetwork.RaiseEvent((byte)EventCodes.NewPlayer,//発生させるイベント
            info,//送るもの（プレイヤーデータ）
            new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },//ルームマスターだけに送信される設定
            new SendOptions { Reliability = true }//信頼性の設定：信頼できるのでプレイヤーに送信される
        );

    }


    /// <summary>
    ///送られてきた新プレイヤーの情報をリストに格納する
    /// </summary>
    public void NewPlayerSet(object[] data)//マスターが行う処理　イベント発生時に行う処理
    {
        PlayerInfo player = new PlayerInfo((string)data[0], (string)data[1]);//ネットワークからプレイヤー情報を取得

        playerList.Add(player);//リストに追加

        ListPlayersGet();//マスターが取得したプレイヤー情報を他のプレイヤーに共有
    }



    /// <summary>
    /// 取得したプレイヤー情報をルーム内の全プレイヤーに送信する
    /// </summary>
    public void ListPlayersGet()//マスターが行う処理　イベントを発生させる関数
    {
        object[] info = new object[playerList.Count + 1];//ゲームの状況＆プレイヤー情報格納配列作成

        info[0] = state;//最初にはゲームの状況を入れる

        for (int i = 0; i < playerList.Count; i++) //参加ユーザーの数分ループ
        {
            object[] temp = new object[2];//一時的格納する配列

            temp[0] = playerList[i].name;
            temp[1] = playerList[i].actor;


            info[i + 1] = temp;//プレイヤー情報を格納している配列に格納する。0にはゲームの状態が入っているため＋１する。
        }


        //RaiseEventでカスタムイベントを発生：データを送る
        PhotonNetwork.RaiseEvent((byte)EventCodes.ListPlayers,////発生させるイベント
            info,//送るもの（プレイヤーデータ）
            new RaiseEventOptions { Receivers = ReceiverGroup.All },//全員に送信するイベント設定
            new SendOptions { Reliability = true }//信頼性の設定：信頼できるのでプレイヤーに送信される
        );
    }



    /// <summary>
    /// ListPlayersSendで新しくプレイヤー情報が送られてきたので、リストに格納する
    /// </summary>
    public void ListPlayersSet(object[] data)//イベントが発生したら呼ばれる関数　全プレイヤーで呼ばれる
    {
        playerList.Clear();//既に持っているプレイヤーのリストを初期化

        state = (GameState)data[0];//ゲーム状態を変数に格納


        for (int i = 1; i < data.Length; i++)//1にする 0はゲーム状態なので1から始める
        {
            object[] info = (object[])data[i];//

            PlayerInfo player = new PlayerInfo(
                (string)info[0],//名前
                (string)info[1]//管理番号
                );


            playerList.Add(player);//リストに追加



        }


        

    }
}




[System.Serializable]
public class PlayerInfo//プレイヤー情報を管理するクラス
{
    public string name;//名前
    public string actor;//番号

    //情報を格納
    public PlayerInfo(string _name, string _actor)
    {
        name = _name;
        actor = _actor;
    }
}