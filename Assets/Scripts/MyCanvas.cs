//引数でボタンの名前とboolを受け取りCanvasの中にあるボタンのinteractablを操作
//Canvasにアタッチ
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
    //SetActive(string name, bool act)
    //引数で指定した文字列とcanvasの子や孫にあるボタンを比較し
    //boolによってinteractable(ボタンを押下できるか)を操作
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

