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
    private GameObject player; // PhotonNetworkでInstantiateしたプレハブを入れる

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
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetGenre_Player1",RpcTarget.All,MessageGeter.genre);    
        }
        else
        {
            photonView.RPC("SetGenre_Player2",RpcTarget.All,MessageGeter.genre);    
        }
    }

    [PunRPC]
    public void SetGenre_Player1(string newGenre)
    {
        player1_genre_box.text = newGenre;
        if (photonView.IsMine)
        {
            player.GetComponent<PlayerController>().StoreGenre(newGenre);
        }
    }
    [PunRPC]
    public void SetGenre_Player2(string newGenre)
    {
        player2_genre_box.text = newGenre;   
        if (photonView.IsMine)
        {
            player.GetComponent<PlayerController>().StoreGenre(newGenre);
        }
    }
}
