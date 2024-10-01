using UnityEngine;

public class SunAreaLighting : MonoBehaviour
{
    [SerializeField] GameObject lightPrefab;
    [SerializeField] int numberOfLights = 10;
    [SerializeField] Color lightColor;
    [SerializeField] float lightIntensity;
    [SerializeField] float lightRange;
    float radius;

    private void Start()
    {
        radius = GetRadius();
        for (int i = 0; i < numberOfLights; i++)
        {
            // Calculate spherical coordinates using Fibonacci sphere method
            float phi = Mathf.Acos(1 - 2 * (i + 0.5f) / numberOfLights);  // Polar angle
            float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * i;  // Azimuthal angle

            float x = Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = Mathf.Cos(phi);

            Vector3 lightPosition = new Vector3(x, y, z) * radius + transform.position;

            GameObject newLight = Instantiate(lightPrefab, lightPosition, Quaternion.identity);

            Light lightComponent = newLight.GetComponent<Light>();
            if (lightComponent != null)
            {
                lightComponent.color = lightColor;
                lightComponent.intensity = lightIntensity;
                lightComponent.range = lightRange;
            }

            newLight.transform.parent = transform;
        }
    }

    private float GetRadius()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        return meshRenderer.bounds.extents.magnitude / 2;
    }
}
