using UnityEngine;
using UnityEngine.EventSystems;

public class TouchImage : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _handle;

    private Vector2 _input;
    private RectTransform _background;

    private void Start()
    {
        _background = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_background, eventData.position, eventData.pressEventCamera, out Vector2 point);
        _input = point.normalized;
        Vector2 radius = _background.sizeDelta / 2;
        _handle.anchoredPosition = _input * radius * 1;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _input = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
    }
}
