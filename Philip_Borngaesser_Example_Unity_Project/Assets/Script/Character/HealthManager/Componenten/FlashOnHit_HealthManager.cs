using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class FlashOnHit_HealthManager : MonoBehaviour
{
    HealthManager _healthManager;
    public float timeBetweenFlashes = 0.1f;
    public Material flashMaterial;
    Material rootMaterial;

    public List<SpriteRenderer> spriteRenderers;
    public List<Color> spriteRendererColors;


    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        rootMaterial = spriteRenderers[0].material;
        _healthManager.OnCalculateDamage.AddListener(OnDamageTaken);
        foreach(SpriteRenderer renderer in spriteRenderers)
        {
            spriteRendererColors.Add(renderer.color);
        }
    }


    void OnDamageTaken(bool isDead, int damage,Transform hitpos)
    {
        StartCoroutine(OnFlash());
    }

    IEnumerator OnFlash()
    {
        SetMaterial(flashMaterial);
        SetFlashColor();
        yield return new WaitForSeconds(timeBetweenFlashes);
        SetMaterial(rootMaterial);
        SetRootColor();
        yield return new WaitForSeconds(timeBetweenFlashes);
        SetMaterial(flashMaterial);
        SetFlashColor();
        yield return new WaitForSeconds(timeBetweenFlashes);
        SetMaterial(rootMaterial);
        SetRootColor();
        yield return new WaitForSeconds(timeBetweenFlashes);
        SetMaterial(flashMaterial);
        SetFlashColor();
        yield return new WaitForSeconds(timeBetweenFlashes);
        SetMaterial(rootMaterial);
        SetRootColor();
    }

    void SetMaterial(Material mat)
    {
        foreach(SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material = mat;
        }
    }

    void SetRootColor()
    {
        for (int index = 0; index < spriteRendererColors.Count; index++)
        {
            spriteRenderers[index].color = spriteRendererColors[index];
        }
    }

    void SetFlashColor()
    {
        for (int index = 0; index < spriteRendererColors.Count; index++)
        {
            spriteRenderers[index].color = Color.white;
        }
    }

}
