using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class DaynNight : MonoBehaviour
{
    // Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField] private float timeDuration = 2f;

    // Variables
    [SerializeField, Range(0, 12)] private float TimeOfDay; // Time now ranges from 0 to 12
    private Coroutine timeTransitionCoroutine;

    public void SetTime(float targetTime)
    {
        // Stop any ongoing transition
        if (timeTransitionCoroutine != null)
        {
            StopCoroutine(timeTransitionCoroutine);
        }

        // Start a new transition to the target time
        timeTransitionCoroutine = StartCoroutine(SmoothSetTime(targetTime));
    }

    private IEnumerator SmoothSetTime(float targetTime)
    {
        float elapsed = 0f;
        float startTime = TimeOfDay;

        while (elapsed < timeDuration)
        {
            elapsed += Time.deltaTime;
            TimeOfDay = Mathf.Lerp(startTime, targetTime, elapsed / timeDuration);
            UpdateLighting(TimeOfDay / 12f); // Update lighting during the transition
            yield return null;
        }

        // Ensure the final value is set
        TimeOfDay = targetTime;
        UpdateLighting(TimeOfDay / 12f);
    }

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            // Increment time (replace with a reference to the game time if needed)
            // TimeOfDay += Time.deltaTime / 2f; // Adjust speed to fit 0-12 range
            TimeOfDay %= 12; // Modulus to ensure always between 0-12
            UpdateLighting(TimeOfDay / 12f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 12f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        // Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        // If the directional light is set, rotate and set its color
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            // Adjust rotation for 0-12 range
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    // Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        // Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        // Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}