using UnityEngine;
using DG.Tweening;

public class HealthAnim : MonoBehaviour
{
    private RectTransform _heart;

    private void Awake()
    {
        _heart = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _heart.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1f)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo).SetId("HeartAnim");
    }
}
