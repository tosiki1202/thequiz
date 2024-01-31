using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class TestSerialize : MonoBehaviour
{
    public string inputString1 = "Hello, こんにちは!";
    public string inputString2 = "あ";
    public string inputString3 = "い";
    public string inputString4 = "ファえw";
    public string inputString5 = "草なんよr";
    public int inputInt = 3;
    public byte[] serializedData = new byte[512];
    public int offset = 0;
    public int off = 0;
    public string q,s1,s2,s3,s4;
    public int ans;
    


    void Start()
    {
        MyProtocol.Serialize(inputString1,serializedData,ref offset);
        MyProtocol.Serialize(inputString2,serializedData,ref offset);
        MyProtocol.Serialize(inputString3,serializedData,ref offset);
        MyProtocol.Serialize(inputString4,serializedData,ref offset);
        MyProtocol.Serialize(inputString5,serializedData,ref offset);
        Protocol.Serialize(inputInt,serializedData,ref offset);

        
        MyProtocol.Deserialize(out q, serializedData, ref off);
        MyProtocol.Deserialize(out s1, serializedData, ref off);
        MyProtocol.Deserialize(out s2, serializedData, ref off);
        MyProtocol.Deserialize(out s3, serializedData, ref off);
        MyProtocol.Deserialize(out s4, serializedData, ref off);
        Protocol.Deserialize(out ans, serializedData, ref off);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
