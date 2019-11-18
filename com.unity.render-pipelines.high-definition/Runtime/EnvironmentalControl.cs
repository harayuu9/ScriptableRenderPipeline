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
    
    private float areaIntensity;
    private float[] spotIntensity;

    private void Start()
    {
        areaIntensity = area.intensity;
        spotIntensity = spots.Select(spot => spot.intensity).ToArray();
    }

    // Update is called once per frame
    private void Update()
    {
        directional.transform.Rotate(1.0f, 0, 0);

        var weight = Vector3.Dot(Vector3.down, directional.transform.forward);

        if (weight > 0)
        {
            directional.color = Color.Lerp(first, second, directionalCurve.Evaluate(1.0f - weight));
        }

        area.intensity = areaIntensity * Mathf.Max(0, weight);
        for (var i = 0; i < 4; i++)
        {
            spots[i].intensity = spotIntensity[i] * Mathf.Max(-weight + 0.2f, 0);
        }
    }
}
