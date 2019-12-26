using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Manager : MonoBehaviour
{

    private struct SPHParticle
    {
#pragma warning disable 0649
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 forcePhysic;
        public Vector3 forceHeading;
        public float density;
        public float pressure;
        public int parameterID;
        public GameObject go;
#pragma warning restore 0649

        public void Init(Vector3 _position, int _parameterID, GameObject _go)
        {
            position = _position;
            parameterID = _parameterID;
            go = _go;

            velocity = Vector3.zero;
            forcePhysic = Vector3.zero;
            forceHeading = Vector3.zero;
            density = 0.0f;
            pressure = 0.0f;
        }
    }



    [System.Serializable]
    private struct SPHParameters
    {
#pragma warning disable 0649 
        public float particleRadius;
        public float smoothingRadius;
        public float smoothingRadiusSq;
        public float restDensity;
        public float gravityMult;
        public float particleMass;
        public float particleViscosity;
        public float particleDrag;
#pragma warning restore 0649
    }


    private struct SPHCollider
    {
        public Vector3 position;
        public Vector3 right;
        public Vector3 up;
        public Vector2 scale;

        public void Init(Transform _transform)
        {
            position = _transform.position;
            right = _transform.right;
            up = _transform.up;
            scale = new Vector2(_transform.lossyScale.x / 2f, _transform.lossyScale.y / 2f);
        }
    }


    // Consts
    private static Vector3 GRAVITY = new Vector3(0.0f, -9.81f, 0.0f);
    private const float GAS_CONST = 2000.0f;
    private const float DT = 0.0008f;
    private const float BOUND_DAMPING = -0.5f;

    // Properties
    [Header("Import")]
    [SerializeField] private GameObject character0Prefab = null;

    [Header("Parameters")]
    [SerializeField] private int parameterID = 0;
    [SerializeField] private SPHParameters[] parameters = null;

    [Header("Properties")]
    [SerializeField] private int amount = 250;
    [SerializeField] private int rowSize = 16;

    // Data
    private SPHParticle[] particles;




    private void Start()
    {
        InitSPH();
    }

    private void Update()
    {
        ComputeDensityPressure();
        ComputeForces();
        Integrate();
        ComputeColliders();
        ApplyPosition();
    }

    private void InitSPH()
    {
        particles = new SPHParticle[amount];
        for (int i = 0; i < amount; i++)
        {
            float x = (i % rowSize) + UnityEngine.Random.Range(-0.1f, 0.1f);
            float y = 2 + ((i / (float)rowSize) / rowSize) * 1.1f;
            float z = ((i / rowSize) % rowSize) + UnityEngine.Random.Range(-0.1f, 0.1f);

            GameObject go = Instantiate(character0Prefab) as GameObject;
            go.transform.localScale = Vector3.one * parameters[parameterID].particleRadius;
            go.transform.position = new Vector3(x, y, z);

            particles[i].Init(new Vector3(x, y, z), parameterID, go);
        }
    }

    private void ComputeDensityPressure()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].density = 0.0f;

            for (int j = 0; j < particles.Length; j++)
            {
                Vector3 pos = particles[j].position - particles[i].position;
                float sqrDistance = pos.sqrMagnitude;

                if (sqrDistance < parameters[particles[i].parameterID].smoothingRadius)
                {
                    particles[i].density += parameters[particles[i].parameterID].particleMass *
                                            (315.0f / (64.0f * Mathf.PI * Mathf.Pow(parameters[particles[i].parameterID].smoothingRadius, 9.0f))) *
                                            Mathf.Pow(parameters[particles[i].parameterID].smoothingRadiusSq - sqrDistance, 3.0f);
                }
            }
            particles[i].pressure = GAS_CONST * (particles[i].density - parameters[particles[i].parameterID].restDensity);
        }
    }


    private void ComputeForces()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            Vector3 additivePressureForce = Vector3.one;
            Vector3 additiveViscocityForce = Vector3.one;
            for (int j = 0; j < particles.Length; j++)
            {
                Vector3 pos = particles[j].position - particles[i].position;
                Vector3 direction = pos.normalized;
                float sqrDistance = pos.sqrMagnitude;
                float distance = pos.magnitude;

                if (distance < parameters[particles[i].parameterID].smoothingRadius)
                {
                    additivePressureForce += -direction * parameters[particles[i].parameterID].particleMass *
                                             (particles[i].pressure + particles[j].pressure) / (2.0f * particles[j].density) *
                                             (-45.0f / (Mathf.PI * Mathf.Pow(parameters[particles[i].parameterID].smoothingRadius, 6.0f))) *
                                             Mathf.Pow(parameters[particles[i].parameterID].smoothingRadius - distance, 2.0f);

                    additiveViscocityForce += parameters[particles[i].parameterID].particleViscosity *
                                              parameters[particles[i].parameterID].particleMass *
                                              (particles[j].velocity - particles[i].velocity) / particles[j].density *
                                              (45.0f / (Mathf.PI * Mathf.Pow(parameters[particles[i].parameterID].smoothingRadius, 6.0f))) *
                                              (parameters[particles[i].parameterID].smoothingRadius - distance);
                }
            }
            Vector3 gravityForce = GRAVITY * particles[i].density * parameters[particles[i].parameterID].gravityMult;

            particles[i].forcePhysic = gravityForce + additivePressureForce + additiveViscocityForce;
        }
    }


    private void Integrate()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].velocity += DT * particles[i].forcePhysic / particles[i].density;
            particles[i].position += DT * particles[i].velocity;
        }
    }

    private static bool Intersect(SPHCollider collider, Vector3 position, float radius, out Vector3 penetrationNormal, out Vector3 penetrationPosition, out float penetrationLength)
    {
        Vector3 colliderProjection = collider.position - position;

        penetrationNormal = Vector3.Cross(collider.right, collider.up);
        penetrationLength = Mathf.Abs(Vector3.Dot(colliderProjection, penetrationNormal)) - (radius / 2.0f);
        penetrationPosition = collider.position - colliderProjection;

        return penetrationLength < 0.0f
            && Mathf.Abs(Vector3.Dot(colliderProjection, collider.right)) < collider.scale.x
            && Mathf.Abs(Vector3.Dot(colliderProjection, collider.up)) < collider.scale.y;
    }



    private static Vector3 DampVelocity(SPHCollider collider, Vector3 velocity, Vector3 penetrationNormal, float drag)
    {
        Vector3 newVelocity = Vector3.Dot(velocity, penetrationNormal) * penetrationNormal * BOUND_DAMPING
                            + Vector3.Dot(velocity, collider.right) * collider.right * drag
                            + Vector3.Dot(velocity, collider.up) * collider.up * drag;
        newVelocity = Vector3.Dot(newVelocity, Vector3.forward) * Vector3.forward
                    + Vector3.Dot(newVelocity, Vector3.right) * Vector3.right
                    + Vector3.Dot(newVelocity, Vector3.up) * Vector3.up;
        return newVelocity;
    }

    private void ComputeColliders()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("SPHCollider");
        SPHCollider[] colliders = new SPHCollider[go.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
           
            colliders[i].Init(go[i].transform);
        }

        for (int i = 0; i < particles.Length; i++)
        {
            for (int j = 0; j < colliders.Length; j++)
            {
                // Check collision
                if (Intersect(colliders[j], particles[i].position, parameters[particles[i].parameterID].particleRadius, out Vector3 penetrationNormal, out Vector3 penetrationPosition, out float penetrationLength))
                {
                    particles[i].velocity = DampVelocity(colliders[j], particles[i].velocity, penetrationNormal, 1.0f - parameters[particles[i].parameterID].particleDrag);
                    particles[i].position = penetrationPosition - penetrationNormal * Mathf.Abs(penetrationLength);
                }
            }
        }
    }

    private void ApplyPosition()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].go.transform.position = particles[i].position;
        }
    }
}