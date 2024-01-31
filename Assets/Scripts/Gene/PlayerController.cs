using System.Collections;
using System;
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
    public bool ready = false; //問題を生成したか
    public bool is_answered = false; //問題に回答済みか
    public bool is_stored = false; //問題の解答を保存したか
    public Data[] my_data = new Data[MessageGeter.question.Length*2];
    public int point;//何問ポイントか
    public int correct;
    
    private void Awake()
    {
        this.transform.SetParent(GameObject.Find("PlayersOrigin").transform);
        name = photonView.Owner.NickName;
    }

    private void Update()
    {
        GeneUIManager.instance.SetPlayerInfo();
    }

    public void UpdatePlayerInfo()
    {
        if (!photonView.IsMine) return;

        jyanru = MessageGeter.genre;
        name = photonView.Owner.NickName;
        my_question = MessageGeter.question;
        my_data = StoreButtonData.data;
        ready = true;
        debug_sent = MessageGeter.question[0].sentence;
    }

    //毎秒10回呼び出し
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // データの送信
            stream.SendNext(jyanru);
            stream.SendNext(name);
            for (int i=0; i<my_question.Length; i++)
            {
                stream.SendNext(my_question[i].sentence);
                stream.SendNext(my_question[i].sel_1);
                stream.SendNext(my_question[i].sel_2);
                stream.SendNext(my_question[i].sel_3);
                stream.SendNext(my_question[i].sel_4);
                stream.SendNext(my_question[i].answer_index);
            }
            stream.SendNext(my_question[0].sentence);
            stream.SendNext(ready);
            stream.SendNext(is_answered);
            for (int i=0; i<my_data.Length; i++)
            {
                stream.SendNext(my_data[i].q_correct);
                stream.SendNext(my_data[i].q_num);
                stream.SendNext(my_data[i].q_sel);
                stream.SendNext(my_data[i].q_time);
            }
            stream.SendNext(point);
            stream.SendNext(is_stored);
            stream.SendNext(correct);
        }
        else
        {
            // データの受信
            jyanru = (string)stream.ReceiveNext();
            name = (string)stream.ReceiveNext();
            for (int i=0; i<my_question.Length; i++)
            {
                my_question[i].sentence = (string)stream.ReceiveNext();
                my_question[i].sel_1 = (string)stream.ReceiveNext();
                my_question[i].sel_2 = (string)stream.ReceiveNext();
                my_question[i].sel_3 = (string)stream.ReceiveNext();
                my_question[i].sel_4 = (string)stream.ReceiveNext();
                my_question[i].answer_index = (int)stream.ReceiveNext();
            }
            debug_sent = (string)stream.ReceiveNext();
            ready = (bool)stream.ReceiveNext();
            is_answered = (bool)stream.ReceiveNext();
            for (int i=0; i<my_data.Length; i++)
            {
                my_data[i].q_correct = (bool)stream.ReceiveNext();
                my_data[i].q_num = (int)stream.ReceiveNext();
                my_data[i].q_sel = (int)stream.ReceiveNext();
                my_data[i].q_time = (float)stream.ReceiveNext();
            }
            point = (int)stream.ReceiveNext();
            is_stored= (bool)stream.ReceiveNext();
            correct = (int)stream.ReceiveNext();
        }
        //GeneUIManager.instance.SetPlayerInfo();
    }
}
