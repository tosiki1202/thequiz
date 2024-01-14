using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public string jyanru;
    public string name;
    public Question[] my_question = new Question[MessageGeter.question.Length];
    public string debug_sent;
    public bool ready = false;
    
    private void Awake()
    {
        this.transform.SetParent(GameObject.Find("PlayersOrigin").transform);
        name = photonView.Owner.NickName;
    }

    public void UpdatePlayerInfo()
    {
        if (!photonView.IsMine) return;

        name = photonView.Owner.NickName;
        jyanru = MessageGeter.genre;
        debug_sent = MessageGeter.question[0].sentence;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // データの送信
            stream.SendNext(jyanru);
            stream.SendNext(name);
            for (int i=0; i<my_question.Length; i++)
            {
                //stream.SendNext(my_question[i]);
            }
            stream.SendNext(my_question[0].sentence);
            stream.SendNext(ready);
        }
        else
        {
            // データの受信
            jyanru = (string)stream.ReceiveNext();
            name = (string)stream.ReceiveNext();
            for (int i=0; i<my_question.Length; i++)
            {
                //my_question[i] = (Question)stream.ReceiveNext();
            }
            debug_sent = (string)stream.ReceiveNext();
            ready = (bool)stream.ReceiveNext();
        }
    }
}
