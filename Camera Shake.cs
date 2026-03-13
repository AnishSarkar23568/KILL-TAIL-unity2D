using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private Vector3 originalPos;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    public void TriggerShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(0.1f, 0.2f) * magnitude;
            float y = Random.Range(1f, 2f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
