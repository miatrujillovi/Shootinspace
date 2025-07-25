using UnityEngine;
using DG.Tweening;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Parameters")]
    [SerializeField] private float swayAmount;
    [SerializeField] private float swaySpeed;

    private Vector3 startPosition;
    private Tween swayAnim;

    private void Start()
    {
        startPosition = transform.localPosition;

        swayAnim = transform.DOLocalMoveX(swayAmount, 1f / swaySpeed)
            .SetRelative(true)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        swayAnim?.Kill();
        transform.localPosition = startPosition;
    }

}
