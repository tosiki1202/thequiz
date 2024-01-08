using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public string jyanru;
    public string name;
    
    private void Awake()
    {
        GameObject parentObject = GameObject.Find("PlayersOrigin");
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
}
