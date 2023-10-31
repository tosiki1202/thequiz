using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

    //継承//
    //MonoBehaviourPunCallbacks
public class PhotonManager : MonoBehaviourPunCallbacks
{
    //変数
    public static PhotonManager instance;
    //ロードパネル
    public GameObject loadingPanel;
    //ロードテキスト
    public TextMeshProUGUI loadingText;
    //ボタンの親オブジェクト
    public GameObject buttons;
    //ルーム作成パネル
    public GameObject createRoomPanel;
    //ルーム名の入力テキスト
    public TextMeshProUGUI enterRoomName;
    //ルームパネル
    public GameObject roomPanel;
    //ルームネーム
    public TextMeshProUGUI roomName;
    //エラーパネル
    public GameObject errorPanel;
    //エラーテキスト
    public TextMeshProUGUI errorText;
    //ルーム一覧
    public GameObject roomListPanel;

    //Awake
    private void Awake()
    {
        //static変数に格納
        instance = this;
    }

    //関数//
    //マスターサーバーに接続された時に呼ばれる関数(継承:コールバック)
    public override void OnConnectedToMaster()
    {
        //ロビーに接続する
        PhotonNetwork.JoinLobby();

        //テキスト更新
        loadingText.text = "ロビーに参加中...";
    }

    //ロビー接続時に呼ばれる関数(継承:コールバック)
    public override void OnJoinedLobby()
    {
        LobbyMenuDisplay();
    }

    //Start//
    private void Start()
    {
        //UIを全て閉じる関数を呼ぶ
        CloseMenuUI();

        //パネルとテキスト更新
        loadingPanel.SetActive(true);
        loadingText.text = "ネットワークに接続中...";

        if (!PhotonNetwork.IsConnected)
        {
            //ネットワーク接続
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //関数//
    //メニューUI全て閉じる関数
    public void CloseMenuUI()
    {
        loadingPanel.SetActive(false);
        buttons.SetActive(false);
        createRoomPanel.SetActive(false);
        roomPanel.SetActive(false);
        errorPanel.SetActive(false);
        roomListPanel.SetActive(false);
    }

    //ロビーUIを表示する関数
    public void LobbyMenuDisplay()
    {
        CloseMenuUI();
        buttons.SetActive(true);
    }

    //ルームを作るボタン用の関数
    public void OpenCreateRoomPanel()
    {
        CloseMenuUI();
        createRoomPanel.SetActive(true);
    }

    //ルームを作成ボタン用の関数
    public void CreateRoomButton()
    {
        if (!string.IsNullOrEmpty(enterRoomName.text))
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 8;

            //ルーム作成
            PhotonNetwork.CreateRoom(enterRoomName.text, options);

            CloseMenuUI();

            //ロードパネル表示
            loadingText.text = "ルーム作成中...";
            loadingPanel.SetActive(true);
        }
    }

    //ルームに参加時に呼ばれる関数(継承：コールバック)
    public override void OnJoinedRoom()
    {
        CloseMenuUI();
        roomPanel.SetActive(true);

        //ルームの名前を反映する
        roomName.text = PhotonNetwork.CurrentRoom.Name;
    }

    //ルーム退出関数
    public void LeavRoom()
    {
        PhotonNetwork.LeaveRoom();

        //UI
        CloseMenuUI();
        loadingText.text = "退出中...";
        loadingPanel.SetActive(true);
    }

    //ルーム退出時に呼ばれる関数(継承：コールバック)
    public override void OnLeftRoom()
    {
        LobbyMenuDisplay();
    }

    //ルーム作成できなかった時に呼ばれる関数
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //UIの表示を変える
        CloseMenuUI();
        errorText.text = "ルームの作成に失敗しました"+message;

        errorPanel.SetActive(true);
    }

    //ルーム一覧パネルを開く関数
    public void FindRoom()
    {
        CloseMenuUI();
        roomListPanel.SetActive(true);
    }
}
