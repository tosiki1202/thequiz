using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public GameObject playersOrigin;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(playersOrigin);   
    }
}
