using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Ring : MonoBehaviour
{
    public GameObject dotPrefab;
    public GameObject bellPrefab;
    public GameObject handPrefab;
    public Transform bellAnchor;
    public Transform handAnchor;

    public Clock clock;
    public int handPos;

    private float radius;
    public int m;
    public int r;
    private List<int> bells;
    private bool rotating;
    private float rotationStartTime;
    private LineRenderer lineRenderer;
    public float pitch;

    public void Initialize(RingData data, float rad)
    {
        rotating = false;
        clock = GetComponentInParent<Clock>();
        lineRenderer = GetComponent<LineRenderer>();
        m = data.m;
        r = data.r;
        bells = data.bells;
        radius = rad;
        pitch = data.pitch;

        int numSegments = 100;

        lineRenderer.positionCount = numSegments + 1; // +1 to close the circle
        lineRenderer.useWorldSpace = false;

        float angle = 2 * Mathf.PI / numSegments;
        Vector3[] points = new Vector3[numSegments + 1];

        for (int i = 0; i < numSegments + 1; i++)
        {
            float x = Mathf.Cos(i * angle) * radius;
            float y = Mathf.Sin(i * angle) * radius;
            points[i] = new Vector3(x, y, 0);
        }

        lineRenderer.SetPositions(points);

        for (int i = 0; i < m; i++)
        {
            if (bells.Contains(((i - r) % m + m) % m))
            {
                Bell bell = Instantiate(bellPrefab, dotPos(i), Quaternion.identity, bellAnchor).GetComponent<Bell>();
                bell.Initialize(i);
            }
            else
            {
                Instantiate(dotPrefab, dotPos(i), Quaternion.identity, bellAnchor);
            }
        }
        Instantiate(handPrefab, dotPos(handPos), Quaternion.identity, handAnchor);
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rotate();
        }
        */
        int ticksPassed = Mathf.FloorToInt(clock.time);
        handPos = ticksPassed % m;
        float tickProgress = clock.time - ticksPassed;
        handAnchor.transform.rotation = Quaternion.Euler(0, 0, -360f * ((handPos + clock.tickAC.Evaluate(tickProgress)) / m));

        if (rotating && clock.time <= rotationStartTime + clock.rotationDuration)
        {
            float progress = (clock.time - rotationStartTime)/ clock.rotationDuration;
            bellAnchor.transform.rotation = Quaternion.Euler(0, 0, -360f * ((r - 1f + clock.rotationAC.Evaluate(progress)) / m));
        } else
        {
            rotating = false;
            bellAnchor.transform.rotation = Quaternion.Euler(0, 0, -360f * (float)r / m);
        }
    }

    private Vector3 dotPos(int n)
    {
        return transform.position + Quaternion.Euler(0, 0, -360f * n / m) * Vector3.up * radius;
    }

    public bool Rotate()
    {
        if (rotating) return false;
        else
        {
            rotationStartTime = clock.time;
            rotating = true;
            r = (r + 1) % m;
            return true;
        }
    }

    
}
