using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeathHandler : MonoBehaviour
{
    [Header("Core")]
    public Animator animator;
    public Rigidbody[] ragdollBodies;
    public Collider[] ragdollColliders;
    public Renderer[] renderers;

    [Header("Timing")]
    public float fadeDelay = 3f;
    public float fadeSpeed = 1f;

    [Header("Debug")]
    public bool testDie = false;

    private bool isDead = false;
    private Material[] materials;
    private Color[] originalColors;

    [Header("Shaders")]
    public Shader fadeShader;


    void Start()
    {
        SetRagdoll(false);
    }

    void Update()
    {
        if (testDie)
        {
            testDie = false;
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.enabled = false;
        SetRagdoll(true);

        var mats = new List<Material>();
        foreach (var rend in renderers)
        {
            foreach (var mat in rend.materials)
            {
                if (mat.HasProperty("_Surface"))
                {
                    if (fadeShader != null)
                    {
                        mat.shader = fadeShader;
                    }

                    mat.SetOverrideTag("RenderType", "Transparent");
                    mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.EnableKeyword("_ALPHABLEND_ON");
                    mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    mat.DisableKeyword("_SURFACE_TYPE_OPAQUE");
                }
                mats.Add(mat);
            }
        }

        materials = mats.ToArray();
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            var mat = materials[i];
            string prop = mat.HasProperty("_BaseColor") ? "_BaseColor" : "_Color";
            Color c = mat.HasProperty(prop) ? mat.GetColor(prop) : mat.color;
            c.a = 1f;
            mat.SetColor(prop, c);
            originalColors[i] = c;
        }

        StartCoroutine(FadeAfterDelay());
    }


    void SetRagdoll(bool state)
    {
        foreach (var rb in ragdollBodies) rb.isKinematic = !state;
        foreach (var col in ragdollColliders) col.enabled = state;
    }

    IEnumerator FadeAfterDelay()
    {
        yield return new WaitForSeconds(fadeDelay);
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            alpha = Mathf.Clamp01(alpha);
            for (int i = 0; i < materials.Length; i++)
            {
                var mat = materials[i];
                string prop = mat.HasProperty("_BaseColor") ? "_BaseColor" : "_Color";
                Color c = originalColors[i];
                c.a = alpha;
                mat.SetColor(prop, c);
            }
            yield return null;
        }
        LevelManager.Instance.OnEnemyDefeated();
        Destroy(gameObject);
    }
}
