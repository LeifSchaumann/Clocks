using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public AnimationCurve rotationAC;
    public GameObject ringPrefab;
    public float rotationDuration;
    public AnimationCurve tickAC;
    public float time;
    public float speed;
    public List<RingData> rings;
    public float innerRadius;
    public float ringSpacing;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        rings = new List<RingData>
        {
            new RingData(2, 0, new List<int> { 1 }),
            new RingData(3, 0, new List<int> { 2 }),
            new RingData(4, 0, new List<int> { 2 }),
            new RingData(6, 0, new List<int> { 4 }),
            new RingData(12, 0, new List<int> { 0 }),
        };
        for (int i = 0; i < rings.Count; i++)
        {
            Ring ring = Instantiate(ringPrefab, transform).GetComponent<Ring>();
            ring.Initialize(rings[i], innerRadius + ringSpacing * i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * speed;
    }
}

public struct RingData
{
    public int m;
    public int r;
    public List<int> bells;

    public RingData(int m, int r, List<int> bells)
    {
        this.m = m;
        this.r = r;
        this.bells = bells;
    }
}