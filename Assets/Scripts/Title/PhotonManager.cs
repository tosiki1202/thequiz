using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cysharp.Threading.Tasks;
using TMPro;

    //継承//
    //MonoBehaviourPunCallbacks
public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private int MAXPLAYERS = 2;
    //変数
    public static PhotonManager instance;
    //ロードパネル
    public GameObject loadingPanel;
    //ロードテキスト
    public TextMeshProUGUI loadingText;
    //ボタンの親オブジェクト
    public GameObject SelectPanel;
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
    public GameObject errorExitButton;
    //ルーム一覧
    public GameObject roomListPanel;
    //ルームボタン格納
    public Room originalRoomButton;
    //ルームボタンの親
    public GameObject roomButtonContent;
    //ルームの情報を扱う辞書
    Dictionary<string,RoomInfo> roomsList = new Dictionary<string, RoomInfo>();
    //ルームボタンを扱うリスト
    private List<Room> allRoomButtons = new List<Room>();
    //名前テキスト
    public TextMeshProUGUI playerNameText;
    //名前テキスト格納リスト
    public List<TextMeshProUGUI> allPlayerNames = new List<TextMeshProUGUI>();
    //名前の親
    public GameObject playerNameContent;
    //ボタン格納
    public GameObject startButton;
    //遷移シーン名
    public string levelToPlay;
    public TextMeshProUGUI connectionStatus;

    public GameObject nameInputPanel;
    public TextMeshProUGUI placeholderText;
    public TMP_InputField nameInput;
    private bool setName;
    public TextMeshProUGUI waitingText;
    public GameObject howToPlayPanel;
    public GameObject soloStartButton;
    private bool is_solo = false;

    //Awake
    private void Awake()
    {
        //static変数に格納
        instance = this;
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            connectionStatus.text = " Connection Status: Connected";
        }
        else
        {
            connectionStatus.text = " Connection Status: Unconnected";
            ErrorPanelDisplay();
        }
    }

    //関数//
    //マスターサーバーに接続された時に呼ばれる関数(継承:コールバック)
    public override void OnConnectedToMaster()
    {
        //ロビーに接続する
        PhotonNetwork.JoinLobby();

        //テキスト更新
        loadingText.text = "Joining...";

        //Masterと同じシーンに遷移する設定
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //ロビー接続時に呼ばれる関数(継承:コールバック)
    public override void OnJoinedLobby()
    {
        if (!setName) ConfirmationName();
        else LobbyMenuDisplay();

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
        loadingText.text = "Establishing a Connection...";

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
        SelectPanel.SetActive(false);
        createRoomPanel.SetActive(false);
        roomPanel.SetActive(false);
        errorPanel.SetActive(false);
        roomListPanel.SetActive(false);
        nameInputPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    //ロビーUIを表示する関数
    public void LobbyMenuDisplay()
    {
        CloseMenuUI();
        SelectPanel.SetActive(true);
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
            options.MaxPlayers = MAXPLAYERS;

            //ルーム作成
            PhotonNetwork.CreateRoom(enterRoomName.text, options);

            CloseMenuUI();

            //ロードパネル表示
            loadingText.text = "Creating...";
            loadingPanel.SetActive(true);
        }
    }

    //ルームに参加時に呼ばれる関数(継承：コールバック)
    public override void OnJoinedRoom()
    {
        if (is_solo) return;
        CloseMenuUI();
        roomPanel.SetActive(true);

        //ルームの名前を反映する
        roomName.text = PhotonNetwork.CurrentRoom.Name;

        //ルームにいるプレイヤー情報取得
        GetAllPlayer();

        //マスターか判定してボタン表示
        CheckRoomMaster();

        StartCoroutine(SwitchText());
    }

    IEnumerator SwitchText()
    {
        while (allPlayerNames.Count != PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            startButton.GetComponent<Button>().interactable = false;
            for (int i=0; i<4; i++)
            {
                switch(i)
                {
                    case 0:
                        waitingText.text = "他のプレイヤーの参加を待機しています "+allPlayerNames.Count+"/"+PhotonNetwork.CurrentRoom.MaxPlayers;
                    break;
                    case 1:
                        waitingText.text = "他のプレイヤーの参加を待機しています. "+allPlayerNames.Count+"/"+PhotonNetwork.CurrentRoom.MaxPlayers;
                    break;
                    case 2:
                    waitingText.text = "他のプレイヤーの参加を待機しています.. "+allPlayerNames.Count+"/"+PhotonNetwork.CurrentRoom.MaxPlayers;
                    break;
                    case 3:
                    waitingText.text = "他のプレイヤーの参加を待機しています... "+allPlayerNames.Count+"/"+PhotonNetwork.CurrentRoom.MaxPlayers;
                    break;
                }
                
                yield return new WaitForSeconds(0.5f);
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.GetComponent<Button>().interactable = true;
            waitingText.text = "準備完了!";
        }
        else
        {
            waitingText.text = "ホストがゲーム開始するのを待っています...";
        }
        
    }

    //ルーム退出関数
    public void LeavRoom()
    {
        PhotonNetwork.LeaveRoom();

        //UI
        CloseMenuUI();
        loadingText.text = "Exiting...";
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
        errorText.text = "Failed to Create"+message;

        errorPanel.SetActive(true);
    }

    public void ErrorPanelDisplay()
    {
        CloseMenuUI();
        errorText.text = "ネットワークとの接続が切断されました。ページを再読み込みしてください。";
        errorExitButton.SetActive(false);
        errorPanel.SetActive(true);
    }

    //ルーム一覧パネルを開く関数
    public void FindRoom()
    {
        CloseMenuUI();
        roomListPanel.SetActive(true);
    }

    //ルームリストに更新があった時によぶ
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

            if (info.RemovedFromList)
            {
                //入れない部屋を辞書から削除
                roomsList.Remove(info.Name);
            }
            else
            {
                roomsList[info.Name] = info;
            }
        }

        //ルームボタン表示
        RoomListDisplay(roomsList);
    }

    public void RoomListDisplay(Dictionary<string, RoomInfo> cachedRoomList)
    {
        foreach(var roomInfo in cachedRoomList)
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

        allRoomButtons.Clear();
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);

        CloseMenuUI();

        loadingText.text = "Joining...";
        loadingPanel.SetActive(true);
    }

    //ルームにいるプレイヤー情報取得
    public void GetAllPlayer()
    {
        //名前テキストUI初期化
        InitializePlayerList();

        //プレイヤー表示関数
        PlayerDisplay();
    }

    public void InitializePlayerList()
    {
        foreach(var rm in allPlayerNames)
        {
            Destroy(rm.gameObject);
        }

        allPlayerNames.Clear();
    }

    public void PlayerDisplay()
    {
        //ルームに参加している人数分UI作成
        foreach (var players in PhotonNetwork.PlayerList)
        {
            //UI生成関数
            PlayerTextGeneration(players);
        }
    }

    public void PlayerTextGeneration(Player players)
    {
        //UI生成
        TextMeshProUGUI newPlayerText = Instantiate(playerNameText);

        //テキストに名前を反映
        newPlayerText.text = players.NickName;

        //親の設定
        newPlayerText.transform.SetParent(playerNameContent.transform);

        //リストに登録
        allPlayerNames.Add(newPlayerText);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerTextGeneration(newPlayer); 
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GetAllPlayer();
        StartCoroutine(SwitchText());
    }

    public void CheckRoomMaster()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }

    //遷移
    public async void PlayGame()
    {
        startButton.SetActive(false);
        photonView.RPC("SetStartText",RpcTarget.All);
        await UniTask.Delay(1200);
        PhotonNetwork.LoadLevel(levelToPlay);
    }

    public async void SoloPlayGame()
    {
        is_solo = true;
        soloStartButton.SetActive(false);
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 1;
        PhotonNetwork.CreateRoom("", options);
        CloseMenuUI();
        loadingText.text = "Starting...";
        loadingPanel.SetActive(true);
        await UniTask.Delay(1200);
        PhotonNetwork.LoadLevel(levelToPlay);
    }

    [PunRPC]
    public void SetStartText()
    {
        waitingText.text = "ホストがゲームをスタートしました。まもなく開始します!";
    }

    public void ConfirmationName()
    {
        if (!setName)
        {
            CloseMenuUI();
            nameInputPanel.SetActive(true);

            //名前を入力したことがあれば、スキップ
            if (PlayerPrefs.HasKey("playerName"))
            {
                placeholderText.text = PlayerPrefs.GetString("playerName");
                nameInput.text = PlayerPrefs.GetString("playerName");
            }
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("playerName");
            }
        }
    }

    public void SetName()
    {
        if (!string.IsNullOrEmpty(nameInput.text))
        {
            if (nameInput.text.Length > 7)
            {
                errorText.text = "名前は７文字以内で入力してください。";
                errorPanel.SetActive(true);
                return;
            }
            PhotonNetwork.NickName = nameInput.text;

            PlayerPrefs.SetString("playerName", nameInput.text);

            LobbyMenuDisplay();

            setName = true;
        }
    }

    public void HowToPlayDisPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }

    public void CloseErrorPanel()
    {
        errorPanel.SetActive(false);
    }
}
