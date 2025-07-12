using UnityEngine;

public enum AttackKind { Projectile, Melee, Beam, AoE }

public interface IParryable
{
    // Llamado al colisionar con el ParryField
    void OnParried(Vector3 parryDir, GameObject source, bool perfect);

    // Duraci�n (seg) tras aparecer en la que a�n se puede hacer parry perfecto
    float ParryWindow { get; }

    AttackKind Kind { get; }

    void PlayParryFeedback(bool perfect);
}
