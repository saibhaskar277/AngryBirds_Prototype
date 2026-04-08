using System.Collections.Generic;
using UnityEngine;


public class TrajectoryPrediction : MonoBehaviour
{
    public static TrajectoryPrediction instance;

    [Header("Dot Settings")]
    public GameObject dotPrefab;
    public int dotCount = 30;
    public float timeStep = 0.1f;

    private List<GameObject> currentDots = new List<GameObject>();
    private List<GameObject> previousDots = new List<GameObject>();

    private void Awake()
    {
        instance = this;

        // Create dots in advance (better performance)
        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.SetActive(false);
            var sr = dot.GetComponent<SpriteRenderer>();
            sr.color = Color.green;
            currentDots.Add(dot);
        }

        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.SetActive(false);
            var sr = dot.GetComponent<SpriteRenderer>();
            sr.color = Color.yellow;
            previousDots.Add(dot);
        }
    }


    public void Show(Vector2 startPos, Vector2 direction, float force)
    {
        Vector2 velocity = -direction * direction.magnitude * force;

        for (int i = 0; i < dotCount; i++)
        {
            float t = i * timeStep;
            Vector2 point = startPos + velocity * t + 0.5f * Physics2D.gravity * t * t;

            currentDots[i].transform.position = point;
            currentDots[i].SetActive(true);
        }
    }


    public void Save(Vector2 startPos, Vector2 direction, float force)
    {
        Vector2 velocity = -direction * direction.magnitude * force;

        for (int i = 0; i < dotCount; i++)
        {
            float t = i * timeStep;
            Vector2 point = startPos + velocity * t + 0.5f * Physics2D.gravity * t * t;

            previousDots[i].transform.position = point;
            previousDots[i].SetActive(true);
        }
    }

    public void HideCurrent()
    {
        foreach (var dot in currentDots)
            dot.SetActive(false);
    }

    public void ClearPrevious()
    {
        foreach (var dot in previousDots)
            dot.SetActive(false);
    }
}
