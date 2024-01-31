
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            //タイトルに戻る
            SceneManager.LoadScene(0);
        }
    }
}