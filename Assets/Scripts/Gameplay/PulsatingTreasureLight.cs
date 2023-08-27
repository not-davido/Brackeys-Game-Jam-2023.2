using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PulsatingTreasureLight : MonoBehaviour
{
    [SerializeField] Light2D Light;

    [SerializeField] float minIntensity = 1f;
    [SerializeField] float maxIntensity = 3f;
    [SerializeField] float pulseSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        // Calculate the intensity using a sine wave
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);

        // Set the light intensity
        Light.intensity = intensity;
    }
}
