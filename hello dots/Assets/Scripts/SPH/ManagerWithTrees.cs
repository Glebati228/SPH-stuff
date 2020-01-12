using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerWithTrees : MonoBehaviour
{
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
    [SerializeField] private GameObject sphParticle;

    [Header("Properties")]
    [SerializeField] private int amount = 250;
    [SerializeField] private int rowSize = 16;

    // Data
    private KdTree<SPHParticle> particles;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        particles = new KdTree<SPHParticle>();
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = new Vector3(
                (i % rowSize) + UnityEngine.Random.Range(-0.1f, 0.1f),
                2 + ((i / (float)rowSize) / rowSize) * 1.1f,
                ((i / rowSize) % rowSize) + UnityEngine.Random.Range(-0.1f, 0.1f)
                );

            GameObject go = Instantiate(sphParticle, pos, Quaternion.identity) as GameObject;
            SPHParticle particle = go.GetComponent<SPHParticle>();
            go.transform.localScale = parameters[parameterID].particleRadius * Vector3.one;
            particle.parameterID = parameterID;
            particles.Add(particle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        particles.UpdatePositions();
    }

    private void CalculateDensityPressure()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            //particles. 
        }
    }
}
