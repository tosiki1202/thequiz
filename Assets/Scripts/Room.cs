using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class Room : MonoBehaviour
{
    //ルーム名テキスト
    public TextMeshProUGUI buttonText;
    //ルーム情報
    private RoomInfo info;

    //このボタンの変数にルーム情報格納
    public void RegisterRoomDetails(RoomInfo info)
    {
        this.info = info;

        buttonText.text = this.info.Name;
    }

    public void OpenRoom()
    {
        //ルーム参加関数をよぶ
        PhotonManager.instance.JoinRoom(info);
    }
}
