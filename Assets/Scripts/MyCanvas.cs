//Canvasにセットすることにより子のボタンのinteractableを操作する
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MyCanvas : MonoBehaviour
{
    private static Canvas _canvas;
    private Button btn;
    void Start()
    {
        _canvas = GetComponent<Canvas>();
    }

    public static void SetActive(string name, bool act) {
        if (_canvas == null)
        {
            Debug.LogError("Canvas is not assigned.");
            return;
        }
        foreach(Transform child in _canvas.transform) {
            if(child.name == name) {
                Button btn = child.GetComponent<Button>();
                btn.interactable = act;
                return;
            }
        }
    }
}
