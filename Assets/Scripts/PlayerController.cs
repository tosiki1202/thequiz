using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] public float HP;
    public string jyanru;
    public string name;
    public GeneUIManager geneUIManager;
    private void Awake()
    {
        name = photonView.Owner.NickName;
    }

    [PunRPC]
    public void StoreGenre(string jyanru)
    {
        if (photonView.IsMine)
        {
            this.jyanru = jyanru;
        }
    }
}
