using UnityEngine;
using System.Collections.Generic;

public class BloodEffectSpawner : MonoBehaviour
{
    public static BloodEffectSpawner Instance;

    [Header("List of Blood Effect Prefabs")]
    public List<ParticleSystem> bloodEffects = new List<ParticleSystem>();

    public float delayBeforeFade = 0.5f;
    public float fadeDuration = 1.5f;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SpawnBlood(Vector3 position)
    {
        if (bloodEffects == null || bloodEffects.Count == 0)
        {
            Debug.LogWarning("No blood effects assigned in the list!");
            return;
        }

        // Choose a random blood effect
        int randomIndex = Random.Range(0, bloodEffects.Count);
        ParticleSystem chosenEffect = bloodEffects[randomIndex];

        // Instantiate and play it
        ParticleSystem effect = Instantiate(chosenEffect, position, Quaternion.identity);
        effect.Play();

        // Destroy it after 1 second
        Destroy(effect.gameObject, 1f);
    }
}
