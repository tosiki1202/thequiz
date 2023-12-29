using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public string jyanru;
    public string name;
    public GeneUIManager geneUIManager;
    private void Awake()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        name = PhotonNetwork.NickName;
        geneUIManager = GameObject.FindGameObjectWithTag("GeneUIManager").GetComponent<GeneUIManager>();
    }

    public void StoreGenre(string jyanru)
    {
        this.jyanru = jyanru;
    }
}
