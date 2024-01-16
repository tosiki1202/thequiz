using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class Fading : MonoBehaviour {
    [SerializeField] private float SPEED = 0.01f;
    [SerializeField] private Image _FadeImage;

    private Color FadeColor;

    void Awake()
    {
        FadeColor = _FadeImage.color;
        FadeColor.a = 1;
        _FadeImage.color = FadeColor;
    }

    async void Update()
    {
        if (_FadeImage.color.a < 0)
        {
            Destroy(gameObject);
            return;
        }
        await AlphaSubtract();
    }

    async UniTask AlphaSubtract()
    {
        FadeColor.a -= SPEED;
        _FadeImage.color = FadeColor;
        await UniTask.Delay(5);
    }
}
