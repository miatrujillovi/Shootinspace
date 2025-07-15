using UnityEngine;
using DG.Tweening;

public class FuelAnim : MonoBehaviour
{
    private Transform modelTransform;
    private float endPosition;

    private void Awake()
    {
        modelTransform = transform;
    }

    private void Start()
    {
        endPosition = transform.position.y + 0.8f;

        modelTransform.DOLocalMoveY(endPosition, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        DOTween.To(() => 0f, angle => {
            modelTransform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }, 360f, 5f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
