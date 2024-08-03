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
    public List<RingData> ringDatas;
    public float innerRadius;
    public float ringSpacing;
    public event Action<int> Tick;
    public int ticks;
    public List<float> pitches;
    private List<Ring> rings;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        ticks = 0;
        for (int i = 0; i <= 12; i++)
        {
            pitches.Add(0.5f * Mathf.Pow(2, (float)i / 12));
        }
        /*
        ringDatas = new List<RingData>
        {
            new RingData(2, 0, new List<int> { 1 }, pitches[0]),
            new RingData(3, 0, new List<int> { 2 }, pitches[4]),
            new RingData(4, 0, new List<int> { 2 }, pitches[7]),
            new RingData(6, 0, new List<int> { 4 }, pitches[9]),
            new RingData(12, 0, new List<int> { 0 }, pitches[12]),
        };
        */
        /*
        ringDatas = new List<RingData>
        {
            new RingData(2, 0, new List<int> { 1 }, pitches[0]),
            new RingData(3, 0, new List<int> { 2 }, pitches[4]),
            new RingData(5, 0, new List<int> { 0 }, pitches[7]),
            new RingData(6, 0, new List<int> { 4 }, pitches[2]),
            new RingData(10, 0, new List<int> { 2 }, pitches[5]),
            new RingData(15, 0, new List<int> { 3 }, pitches[9]),
            new RingData(30, 0, new List<int> { 6, 24 }, pitches[12])
        };*/
        ringDatas = new List<RingData>
        {
            new RingData(2, 0, new List<int> { 0 }, pitches[0]),
            new RingData(3, 0, new List<int> { 0 }, pitches[4]),
            new RingData(4, 0, new List<int> { 0 }, pitches[7]),
            new RingData(6, 0, new List<int> { 0 }, pitches[9]),
            new RingData(12, 0, new List<int> { 0 }, pitches[12]),
        };
        rings = new List<Ring>();
        for (int i = 0; i < ringDatas.Count; i++)
        {
            Ring ring = Instantiate(ringPrefab, transform).GetComponent<Ring>();
            ring.Initialize(ringDatas[i], innerRadius + ringSpacing * i);
            rings.Add(ring);
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.Set(mouseWorldPos.x, mouseWorldPos.y, 0f);
            float distance = (transform.position - mouseWorldPos).magnitude;
            int layer = Mathf.FloorToInt((distance - innerRadius)/ringSpacing + 0.5f);
            if (layer >= 0 && layer < rings.Count)
            {
                rings[layer].Rotate();
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