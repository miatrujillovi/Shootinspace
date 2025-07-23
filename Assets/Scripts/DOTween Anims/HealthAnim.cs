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
        _heart.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 1f)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo).SetId("HeartAnim");
    }
}
