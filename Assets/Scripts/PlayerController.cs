using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks
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

    [PunRPC]
    public void StoreGenre(string jyanru)
    {
        if (!photonView.IsMine)
        {
            this.jyanru = jyanru;
        }
    }

    [PunRPC]
    public void StoreQuestions(string[] sentences, string[] sel_1, string[] sel_2, string[] sel_3, string[] sel_4, int[] answer_index)
    {
        if (!photonView.IsMine)
        {
            for (int i=0; i<MessageGeter.question.Length; i++)
            {
                this.my_question[i].sentence = sentences[i];
                this.my_question[i].sel_1 = sel_1[i];
                this.my_question[i].sel_2 = sel_2[i];
                this.my_question[i].sel_3 = sel_3[i];
                this.my_question[i].sel_4 = sel_4[i];
                this.my_question[i].answer_index = answer_index[i];
                this.debug_sent = sentences[0];
            }
        }
    }
}
