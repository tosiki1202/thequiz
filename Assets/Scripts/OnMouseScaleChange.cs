using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnMouseScaleChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float onScale = 1.1f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * onScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}