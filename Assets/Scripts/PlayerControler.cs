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
        jyanru = MessageGeter.genre;
        if (!photonView.IsMine)
        {
            return;
        }

        
    }

    [PunRPC]
    public void PrintJyanru()
    {
        Debug.Log(jyanru);
        Debug.Log(MessageGeter.genre);
    }
}
