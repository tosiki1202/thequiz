using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerControler : MonoBehaviourPunCallbacks
{
    public string jyanru;

    public void Update()
    {
        photonView.RPC("PrintJyanru",RpcTarget.All);
        if (!photonView.IsMine)
        {
            return;
        }

        jyanru = MessageGeter.genre;
    }

    [PunRPC]
    public void PrintJyanru()
    {
        Debug.Log(jyanru);
        Debug.Log(MessageGeter.genre);
    }
}
