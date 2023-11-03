using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenerateMessage : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI jyanru;
    private string str;
    private Button button;
    private MessageGeter messageGeter;
    void Start()
    {
        messageGeter = gameObject.AddComponent<MessageGeter>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => messageGeter.Generator(jyanru.text));
    }

    // Update is called once per fram
}

