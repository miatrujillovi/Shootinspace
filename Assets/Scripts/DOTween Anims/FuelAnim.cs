using UnityEngine;
using DG.Tweening;

public class FuelAnim : MonoBehaviour
{
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private float maxHeight;

    private Transform modelTransform;
    private float endPosition;
    private Tween anim;

    private void Awake()
    {
        modelTransform = transform;
    }

    private void Start()
    {
        endPosition = transform.position.y + maxHeight;

        modelTransform.DOLocalMoveY(endPosition, ySpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        DOTween.To(() => 0f, angle => {
            modelTransform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }, 360f, xSpeed).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        anim?.Kill();
    }
}
