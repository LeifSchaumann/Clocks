using System;
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
    public event Action<int> Tick;
    public int ticks;
    public List<float> pitches;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        ticks = 0;
        pitches = new List<float> { 0.5f, 0.74915f, 0.62996f, 0.8409f, 1f };
        rings = new List<RingData>
        {
            new RingData(2, 0, new List<int> { 1 }, pitches[0]),
            new RingData(3, 0, new List<int> { 2 }, pitches[1]),
            new RingData(4, 0, new List<int> { 2 }, pitches[2]),
            new RingData(6, 0, new List<int> { 4 }, pitches[3]),
            new RingData(12, 0, new List<int> { 0 }, pitches[4]),
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
        if (time >= ticks + 1)
        {
            ticks++;
            if (Tick != null)
            {
                Tick(ticks);
            }
        }
    }
}

public struct RingData
{
    public int m;
    public int r;
    public List<int> bells;
    public float pitch;

    public RingData(int m, int r, List<int> bells, float pitch)
    {
        this.m = m;
        this.r = r;
        this.bells = bells;
        this.pitch = pitch;
    }
}