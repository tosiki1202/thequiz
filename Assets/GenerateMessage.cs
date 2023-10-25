using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenerateMessage : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI jyanru;
    public MessageGeter messageGeter;
    private string str;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => messageGeter.Generator(jyanru.text));
    }

    // Update is called once per fram
}

