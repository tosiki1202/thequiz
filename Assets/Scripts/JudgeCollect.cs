using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeCollect : MonoBehaviour
{
    public MessageGeter messageGeter;
    public MessageManager messageManager;
    public StoreButtonData storeButtonData;
    
    public bool CompareIndex()
    {
        if (messageGeter.question[messageManager.GetQuestionIndex()].answer_index == storeButtonData.data[storeButtonData.data.Count-1].id)
        {
            return true;
        }
        return false;
    }
    
}
