using System.Collections;
using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    [Header("Component Links")]
    [SerializeField] private Light flashlightLight;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    [Header("Beam Brightness")]
    [Tooltip("The normal brightness of the flashlight beam")]
    [SerializeField] private float baseIntensity = 30f;

    [Header("Atmospheric Glitch Bursts")]
    [Tooltip("Minimum seconds between random glitch episodes")]
    [SerializeField] private float minIntervalBetweenGlitches = 8f;
    [Tooltip("Maximum seconds between random glitch episodes")]
    [SerializeField] private float maxIntervalBetweenGlitches = 20f;
    [Tooltip("Number of rapid flickers during a glitch episode")]
    [SerializeField] private int minFlickerCount = 3;
    [SerializeField] private int maxFlickerCount = 7;

    [Header("Subtle High-Frequency Hum")]
    [Tooltip("Adds a constant, barely-noticeable electrical vibration to the beam")]
    [SerializeField] private bool enableMicroJitter = true;
    [SerializeField] private float microJitterStrength = 1.5f;

    private bool isPlayerEnabled = true;
    private bool isGlitching = false;
    private Coroutine glitchRoutine;

    void Awake()
    {
        if (flashlightLight == null)
        {
            flashlightLight = GetComponent<Light>();
        }

        if (flashlightLight != null)
        {
            flashlightLight.intensity = baseIntensity;
        }
    }

    void Start()
    {
        glitchRoutine = StartCoroutine(GlitchLoop());
    }

    void Update()
    {
        // 1. Player Toggle Input
        if (Input.GetKeyDown(toggleKey))
        {
            isPlayerEnabled = !isPlayerEnabled;

            if (flashlightLight != null)
            {
                flashlightLight.enabled = isPlayerEnabled;
                if (isPlayerEnabled)
                {
                    flashlightLight.intensity = baseIntensity;
                }
            }
        }

        // 2. Continuous Micro Jitter (Only active when flashlight is on and not in a hard glitch)
        if (isPlayerEnabled && enableMicroJitter && !isGlitching && flashlightLight != null)
        {
            float noise = (Mathf.PerlinNoise(Time.time * 25f, 0f) - 0.5f) * 2f;
            flashlightLight.intensity = Mathf.Max(0.5f, baseIntensity + (noise * microJitterStrength));
        }
    }

    private IEnumerator GlitchLoop()
    {
        while (true)
        {
            // Wait for a random interval before triggering a glitch
            float waitTime = Random.Range(minIntervalBetweenGlitches, maxIntervalBetweenGlitches);
            yield return new WaitForSeconds(waitTime);

            // Only glitch if the player actually has the light switched on
            if (isPlayerEnabled && flashlightLight != null)
            {
                yield return StartCoroutine(PerformGlitchBurst());
            }
        }
    }

    private IEnumerator PerformGlitchBurst()
    {
        isGlitching = true;
        int flickers = Random.Range(minFlickerCount, maxFlickerCount + 1);

        for (int i = 0; i < flickers; i++)
        {
            // Sudden drop or complete cutout
            float dropIntensity = Random.value > 0.4f ? Random.Range(1f, baseIntensity * 0.25f) : 0f;
            flashlightLight.intensity = dropIntensity;
            yield return new WaitForSeconds(Random.Range(0.03f, 0.08f));

            // Sharp rebound snap
            flashlightLight.intensity = baseIntensity * Random.Range(0.85f, 1.15f);
            yield return new WaitForSeconds(Random.Range(0.04f, 0.12f));
        }

        // Settle back to stable base intensity
        flashlightLight.intensity = baseIntensity;
        isGlitching = false;
    }
}