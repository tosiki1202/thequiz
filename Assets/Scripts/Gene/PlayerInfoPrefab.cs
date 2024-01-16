using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class PlayerInfoPrefab : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI genre;

    public void RegisterPlayerInfoPrefab(string name, string genre)
    {
        this.name.text = name;
        this.genre.text = genre;
    }
}

