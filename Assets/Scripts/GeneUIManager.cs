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

    public void UpdatePlayerInfo()
    { 
        SetGenre(photonView.Owner.NickName,
                 MessageGeter.genre,
                 PhotonNetwork.LocalPlayer.ActorNumber);
        
        SetQuestion(photonView.Owner.NickName,
                    MessageGeter.question,
                    PhotonNetwork.LocalPlayer.ActorNumber);


        photonView.RPC("SetPlayerInfo",RpcTarget.All);
        
    }

    public void SetGenre(string name, string newGenre, int actor)
    {
        //自分は普通に値を格納して、他のクライアントにRPCを出して格納してもらう
        player.GetComponent<PlayerController>().jyanru = MessageGeter.genre;
        player.GetComponent<PlayerController>().photonView.RPC("StoreGenre",
                                                                RpcTarget.Others,
                                                                newGenre);
    }

    public void SetQuestion(string name, Question[] newQuestion, int actor)
    {
        string[] sentences = new string[MessageGeter.question.Length];
        string[] sel_1 = new string[MessageGeter.question.Length];
        string[] sel_2 = new string[MessageGeter.question.Length];
        string[] sel_3 = new string[MessageGeter.question.Length];
        string[] sel_4 = new string[MessageGeter.question.Length];
        int[] answer_index = new int[MessageGeter.question.Length];
        for (int i=0; i<newQuestion.Length; i++)
        {
            sentences[i] = newQuestion[i].sentence;
            sel_1[i] = newQuestion[i].sel_1;
            sel_2[i] = newQuestion[i].sel_2;
            sel_3[i] = newQuestion[i].sel_3;
            sel_4[i] = newQuestion[i].sel_4;
            answer_index[i] = newQuestion[i].answer_index;
        }

        player.GetComponent<PlayerController>().my_question = MessageGeter.question;
        player.GetComponent<PlayerController>().debug_sent = MessageGeter.question[0].sentence;
        player.GetComponent<PlayerController>().photonView.RPC("StoreQuestions",
                                                                RpcTarget.Others,
                                                                sentences,
                                                                sel_1,
                                                                sel_2,
                                                                sel_3,
                                                                sel_4,
                                                                answer_index);
    }

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
        if (playersDictionary.Count < 2) return;
        for (int i=0; i<playersDictionary.Count; i++)
        {
            allPlayerInfo.Add(playersDictionary[i+1]);
        }

        player1_genre_box.text = allPlayerInfo[0].jyanru;
        player2_genre_box.text = allPlayerInfo[1].jyanru;
        player1_name_box.text = allPlayerInfo[0].name;
        player2_name_box.text = allPlayerInfo[1].name;   
    }
}