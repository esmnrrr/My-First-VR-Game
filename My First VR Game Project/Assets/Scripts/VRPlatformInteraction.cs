using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlatformInteraction : MonoBehaviour
{
    public GameObject platformPrefab;
    public int count = 60;
    public float radius = 6f;
    public float heightStep = 1.4f;
    public float angleStep = 18f;
    public Vector2 scaleRange = new Vector2(1.6f, 2.6f);

    void Start()
    {
        float angle = 0f;
        float y = 0.8f;
        for (int i = 0; i < count; i++)
        {
            var pos = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                y,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius
            );

            var go = Instantiate(platformPrefab, pos, Quaternion.identity, transform);
            float size = Random.Range(scaleRange.x, scaleRange.y);
            go.transform.localScale = new Vector3(size, 0.2f, size * 0.8f);
            go.name = $"Platform_{i:D2}";

            // hafif açıyla oyuncuya bakacak şekilde döndür
            go.transform.rotation = Quaternion.Euler(0f, angle + 90f, 0f);

            angle += angleStep + Random.Range(-4f, 4f);
            y += heightStep + Random.Range(-0.2f, 0.2f);
        }
    }
}
