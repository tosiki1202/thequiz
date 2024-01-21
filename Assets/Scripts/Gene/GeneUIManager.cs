using System.Collections;
using System;
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
    public TextMeshProUGUI generatingText;
    public TextMeshProUGUI onlineStartText;
    public PlayerInfoPrefab originalPlayerInfoPrefab;
    public GameObject playerInfoContent;
    public GameObject generatePanel;
    public GameObject readyPanel;
    public GameObject geneInputPanel;
    public GameObject StartButton;
    public GameObject playerPrefab; // PhotonNetworkで生成するオブジェクトを指定(Resoursesフォルダに入っていること)
    public static GameObject player; // PhotonNetworkでInstantiateしたプレハブを入れる
    public static List<PlayerController> allPlayerInfo = new List<PlayerController>();
    public static Dictionary<int,PlayerController> playersDictionary = new Dictionary<int,PlayerController>();
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
        }
        DontDestroyOnLoad(playersOrigin);
        CloseMenuUI();
        geneInputPanel.SetActive(true);
        if (!PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(false);
            onlineStartText.text = "ホストを待機中...";
            onlineStartText.gameObject.SetActive(true);
        }
        else
        {
            StartButton.SetActive(true);
            onlineStartText.text = "";
            onlineStartText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!PhotonNetwork.IsConnected) return;
        if (!PhotonNetwork.IsMasterClient) return;

        if (player.GetComponent<PlayerController>().ready)
        {
            for (int i=0; i<allPlayerInfo.Count; i++)
            {
                if (!allPlayerInfo[i].ready) return;
            }
            StartButton.GetComponent<Button>().interactable = true;
        }
    }

    public void CloseMenuUI()
    {
        readyPanel.SetActive(false);
        generatePanel.SetActive(false);
        geneInputPanel.SetActive(false);
    }
    public void GeneratingUIDisplay()
    {
        CloseMenuUI();
        generatePanel.SetActive(true);

    }
    public void SetGeneratingText(string _text)
    {
        generatingText.text = _text;
    }
    public void SetReadyStat()
    {
        if (player.GetComponent<PlayerController>().my_question.Length == 0)
        {
            Debug.Log("question is null!");
            return;
        }
        player.GetComponent<PlayerController>().ready = true;
    }
    public void StartGame()
    {
        photonView.RPC("SetOnlineText",RpcTarget.All);
        PhotonNetwork.LoadLevel("QuizScene");
    }

    [PunRPC]
    public void SetOnlineText()
    {
        onlineStartText.text = "スタート!";
    }

    public void UpdatePlayerInfo()
    { 
        player.GetComponent<PlayerController>().UpdatePlayerInfo();
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
    public void SetPlayerInfo()
    {
        Debug.Log("プレイヤーインフォセット");
        Debug.Log("all: "+allPlayerInfo.Count);
        List<int> temp = new List<int>();
        
        playersDictionary.Clear();
        List<Transform> children = GetChildren(playersOrigin.transform);
        foreach (var players in children)
        {
            playersDictionary.Add(players.GetComponent<PhotonView>().Owner.ActorNumber, players.GetComponent<PlayerController>());
            temp.Add(players.GetComponent<PhotonView>().Owner.ActorNumber);
        }
        temp.Sort();

        allPlayerInfo.Clear();
        for (int i=0; i<playersDictionary.Count; i++)
        {
            allPlayerInfo.Add(playersDictionary[temp[i]]);
        }

        if (SceneManager.GetActiveScene().name != "GeneScene_photon")
        {
            return;
        }
        foreach (Transform child in playerInfoContent.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i=0; i<allPlayerInfo.Count; i++)
        {
            PlayerInfoPrefab newPrefab = Instantiate(originalPlayerInfoPrefab);
            newPrefab.RegisterPlayerInfoPrefab(allPlayerInfo[i].name, allPlayerInfo[i].jyanru, allPlayerInfo[i].ready);
            newPrefab.transform.SetParent(playerInfoContent.transform);
        }
    }
}