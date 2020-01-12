using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPHParticle : MonoBehaviour
{

#pragma warning disable 0649
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 forcePhysic;
    public Vector3 forceHeading;
    public float density;
    public float pressure;
    public int parameterID;
#pragma warning restore 0649

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
        forcePhysic = Vector3.zero;
        forceHeading = Vector3.zero;
        density = 0.0f;
        pressure = 0.0f;
    }
}
