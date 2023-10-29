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
    //ルームボタン格納
    public Room originalRoomButton;
    //ルームボタンの親オブジェクト
    public GameObject roomButtonContent;
    //ルームの情報を扱う辞書(添え字の代わりにキー(今はstring)を使う)
    Dictionary<string, RoomInfo> roomsList = new Dictionary<string, RoomInfo>();
    //ルームボタンを扱うリスト
    private List<Room> allRoomButtons = new List<Room>();


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

        //辞書の初期化
        roomsList.Clear();
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

    //ルーム一覧表示
    public void FindRoom()
    {
        CloseMenuUI();
        roomListPanel.SetActive(true);
    }

    //ルームリストに更新があった時に呼ばれる関数(継承：コールバック)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //ルームボタンUI初期化
        RoomUIinitialization();

        //辞書に登録
        UpdateRoomList(roomList);
    }

    //ルーム情報を辞書に登録
    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        //辞書にルーム登録
        for (int i=0; i<roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            
            if (info.RemovedFromList)//満室ならtrue
            {
                roomsList.Remove(info.Name);
            }
            else
            {
                roomsList[info.Name] = info;
            }
        }

        //ルームボタン表示関数
        RoomListDisplay(roomsList);
    }

    public void RoomListDisplay(Dictionary<string,RoomInfo> cachedRoomList)
    {
        foreach (var roomInfo in cachedRoomList)
        {
            //ボタン作成
            Room newButton = Instantiate(originalRoomButton);

            //生成したボタンにルーム情報設定
            newButton.RegisterRoomDetails(roomInfo.Value);
            
            //親の設定
            newButton.transform.SetParent(roomButtonContent.transform);

            allRoomButtons.Add(newButton);
        }
    }

    public void RoomUIinitialization()
    {
        //ルームUIの数分ループ
        foreach (Room rm in allRoomButtons)
        {
            Destroy(rm.gameObject);
        }

        //リストの初期化
        allRoomButtons.Clear();
    }
}
