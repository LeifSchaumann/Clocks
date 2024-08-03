using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bell : MonoBehaviour
{
    private Clock clock;
    private Ring ring;
    private int pos;
    private ParticleSystem ps;
    private AudioSource audioSource;

    public void Initialize(int position)
    {
        ring = GetComponentInParent<Ring>();
        clock = ring.clock;
        ps = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        pos = position;
        clock.Tick += Tick;
        audioSource.pitch = ring.pitch;
    }

    public void Tick(int t)
    {
        if ((pos + ring.r - t) % ring.m == 0)
        {
            ps.Emit(1);
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

}
