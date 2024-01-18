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
    public PlayerInfoPrefab originalPlayerInfoPrefab;
    public GameObject playerInfoContent;
    public GameObject generatePanel;
    public GameObject readyPanel;
    public GameObject geneInputPanel;
    public Button StartButton;
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
            StartButton.interactable = true;
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
        player.GetComponent<PlayerController>().ready = false;
        PhotonNetwork.LoadLevel("QuizScene");
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
        playersDictionary.Clear();
        List<Transform> children = GetChildren(playersOrigin.transform);
        Debug.Log("c"+children.Count);
        foreach (var players in children)
        {
            playersDictionary.Add(players.GetComponent<PhotonView>().Owner.ActorNumber, players.GetComponent<PlayerController>());
        }

        allPlayerInfo.Clear();
        if (playersDictionary.Count == 1)
        {
            allPlayerInfo.Add(playersDictionary[1]);
        }
        if (playersDictionary.Count < 2) return;
        for (int i=0; i<playersDictionary.Count; i++)
        {
            allPlayerInfo.Add(playersDictionary[i+1]);
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