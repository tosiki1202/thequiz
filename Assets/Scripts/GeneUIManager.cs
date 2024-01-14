using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GeneUIManager : MonoBehaviourPunCallbacks
{
    public static GeneUIManager instance;
    [SerializeField] TextMeshProUGUI GeneratingText;
    [SerializeField] GameObject GeneratePanel;
    public GameObject playerPrefab;
    public TextMeshProUGUI player1_genre_box;
    public TextMeshProUGUI player2_genre_box;
    public TextMeshProUGUI player1_name_box;
    public TextMeshProUGUI player2_name_box;
    public GameObject player1_ready_box;
    public GameObject player2_ready_box;
    public Button StartButton;
    private GameObject player; // PhotonNetworkでInstantiateしたプレハブを入れる

    public List<PlayerController> allPlayerInfo = new List<PlayerController>();
    public Dictionary<int,PlayerController> playersDictionary = new Dictionary<int,PlayerController>();
    public GameObject playersOrigin;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            player = PhotonNetwork.Instantiate(playerPrefab.name,new Vector3(0,0,0),Quaternion.identity);
            photonView.RPC("SetPlayerInfo",RpcTarget.All);
        }
    }

    private void Update()
    {
        if (player.GetComponent<PlayerController>().ready)
        {
            StartButton.interactable = true;
        }
    }

    public void CloseGeneUI()
    {
        GeneratePanel.SetActive(false);
    }
    public void GeneratingUIDisplay()
    {
        CloseGeneUI();
        GeneratePanel.SetActive(true);

    }
    public void SetGeneratingText(string _text)
    {
        GeneratingText.text = _text;
    }
    public void SetReadyStat()
    {
        if (player.GetComponent<PlayerController>().my_question.Length == 0)
        {
            Debug.Log("question is null!");
            return;
        }
        player.GetComponent<PlayerController>().ready = true;
        //SetReady(player.GetComponent<PlayerController>().ready);
        photonView.RPC("SetPlayerInfo",RpcTarget.All);
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("QuizScene");
    }

    public void UpdatePlayerInfo()
    { 
        player.GetComponent<PlayerController>().jyanru = MessageGeter.genre;
        player.GetComponent<PlayerController>().my_question = MessageGeter.question;
        player.GetComponent<PlayerController>().debug_sent = MessageGeter.question[0].sentence;
        photonView.RPC("SetPlayerInfo",RpcTarget.All);
        
    }

    //SetPlayerInfoで使用
    private List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        for (int i=0; i < parent.childCount; i++)
        {
            children.Add(parent.GetChild(i));
        }

        return children;
    }

    //プレイヤーリスト情報を更新して、UIを書き換える
    [PunRPC]
    public void SetPlayerInfo()
    {
        playersDictionary.Clear();
        List<Transform> children = GetChildren(playersOrigin.transform);
        foreach (var players in children)
        {
            playersDictionary.Add(players.GetComponent<PhotonView>().Owner.ActorNumber, players.GetComponent<PlayerController>());
        }

        allPlayerInfo.Clear();
        if (playersDictionary.Count < 1) return;
        for (int i=0; i<playersDictionary.Count; i++)
        {
            allPlayerInfo.Add(playersDictionary[i+1]);
        }

        player1_genre_box.text = allPlayerInfo[0].jyanru;
        player1_name_box.text = allPlayerInfo[0].name;
        player1_ready_box.SetActive(allPlayerInfo[0].ready);

        player2_genre_box.text = allPlayerInfo[1].jyanru;
        player2_name_box.text = allPlayerInfo[1].name;   
        player2_ready_box.SetActive(allPlayerInfo[1].ready);
    }
}