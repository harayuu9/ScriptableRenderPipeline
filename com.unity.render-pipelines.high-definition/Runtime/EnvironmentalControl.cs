using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class EnvironmentalControl : MonoBehaviour
{
    [SerializeField] private Light directional;

    [SerializeField] private Color first;
    [SerializeField] private Color second;
    [SerializeField] private AnimationCurve directionalCurve;

    [SerializeField] private HDAdditionalLightData area;
    [SerializeField] private HDAdditionalLightData[] spots = new HDAdditionalLightData[4];

    [SerializeField] private float dayTime = 7.0f;
    [SerializeField] private float nightTime = 5.0f;

    private float areaIntensity;
    private float[] spotIntensity;

    private void Start()
    {
        areaIntensity = area.intensity;
        spotIntensity = spots.Select(spot => spot.intensity).ToArray();

        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        var weight = Vector3.Dot(Vector3.down, directional.transform.forward);

        if (weight > 0)
        {
            directional.transform.Rotate(180.0f / (60.0f * dayTime) * Time.deltaTime, 0, 0);
            directional.color = Color.Lerp(first, second, directionalCurve.Evaluate(1.0f - weight));
        }
        else
        {
            directional.transform.Rotate(180.0f / (60.0f * nightTime) * Time.deltaTime, 0, 0);
        }

        area.intensity = areaIntensity * Mathf.Max(0, weight);
        for (var i = 0; i < 4; i++)
        {
            spots[i].intensity = spotIntensity[i] * Mathf.Max(-weight + 0.2f, 0);
        }
    }
}
