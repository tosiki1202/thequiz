using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionTransition : MonoBehaviour
{
    private Button button;
    public MessageManager messageManager;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => Transit());
    }

    public void Transit()
    {
        int index = messageManager.GetQuestionIndex();

        if (messageManager.GetMAXQUESTIONINDEX() > index+1)
        {
            messageManager.SetQuestionIndex(index+1);
        }
        else
        {
            Debug.Log("添え字が超えそうなため移動しません！");
        }
        
    }
    
}
