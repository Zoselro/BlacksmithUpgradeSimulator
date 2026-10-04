using UnityEngine;

// 대사 넘기기 아이콘(▼)을 위아래로 반복 이동
public class BobbingIcon : MonoBehaviour
{
    [SerializeField] private float amplitude = 8f; // 위아래 이동 거리(px)
    [SerializeField] private float speed = 4f;

    private RectTransform rect;
    private Vector2 basePos;

    private void Awake()
    {
        rect = (RectTransform)transform;
        basePos = rect.anchoredPosition;
    }

    private void OnEnable()
    {
        if (rect != null)
            rect.anchoredPosition = basePos;
    }

    private void Update()
    {
        rect.anchoredPosition = basePos + Vector2.up * Mathf.Sin(Time.time * speed) * amplitude;
    }
}
