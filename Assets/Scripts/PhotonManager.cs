using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PhotonManager : MonoBehaviour
{
    //変数
    public static PhotonManager instance;

    //ロードパネル
    public GameObject loadingPanel;
    //ロードテキスト
    public TextMeshProUGUI loadingText;
    //ボタンの親オブジェクト
    public GameObject buttons;
    //Awake
    private void Awake()
    {
        //static変数に格納
        instance = this;
    }

    //Start//
    private void Start()
    {
        //UIを全て閉じる関数を呼ぶ
        CloseMenuUI();

        //パネルとテキスト更新
        loadingPanel.SetActive(true);
        loadingText.text = "ネットワークに接続中...";

        //ネットワーク接続
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    
    

    //関数//
    //メニューUI全て閉じる関数作成
    public void CloseMenuUI()
    {
        loadingPanel.SetActive(false);
        buttons.SetActive(false);
    }

    //ロビーUIを表示する関数
    public void LobbyMenuDisplay()
    {
        CloseMenuUI();
        buttons.SetActive(true);
    }
}
