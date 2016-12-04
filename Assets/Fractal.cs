using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Fractal : MonoBehaviour
{

    public Mesh[] meshs;
    public Material material;
    public uint maxComplexity;
    public float childScale = 0.5f;

    private static List<GameObject> allObjects = new List<GameObject>();
    private Material[,] materials;

    private uint complexity = 0;


    private static PairTemplate<Vector3, Quaternion>[] directionRotation = 
    {
        new PairTemplate<Vector3, Quaternion>(Vector3.up, Quaternion.identity),
        new PairTemplate<Vector3, Quaternion>(Vector3.right, Quaternion.Euler(0f, 0f, -90f)),
        new PairTemplate<Vector3, Quaternion>(Vector3.left, Quaternion.Euler(0f, 0f, 90f)),
        new PairTemplate<Vector3, Quaternion>(Vector3.forward, Quaternion.Euler(90f, 0f, 0f)),
        new PairTemplate<Vector3, Quaternion>(Vector3.back, Quaternion.Euler(-90f, 0f, 0f)),

    };


    // Use this for initialization
    void Start()
    {
        if (materials == null)
        {
            InitializeMaterials();
        }

        gameObject.AddComponent<MeshFilter>().mesh = meshs[UnityEngine.Random.Range(0, meshs.Length)];
        gameObject.AddComponent<MeshRenderer>().material =
            materials[complexity, UnityEngine.Random.Range(0, 4)];

        if (complexity < maxComplexity)
        {
            StartCoroutine(this.CreateChilds());
        }

    }

    private void Initialize(Fractal parent, PairTemplate<Vector3, Quaternion> directionRotation)
    {
        meshs = parent.meshs;
        materials = parent.materials;
        maxComplexity = parent.maxComplexity;
        complexity = parent.complexity + 1;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = directionRotation.first * (0.5f + 0.5f * childScale);
        transform.localRotation = directionRotation.second;
        
    }

    private void InitializeMaterials()
    {
        materials = new Material[maxComplexity + 1, 4];
        for (int i = 0; i <= maxComplexity; i++)
        {
            float t = i / (maxComplexity - 1f);
            t *= t;
            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.LerpUnclamped(Color.white, Color.yellow, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.LerpUnclamped(Color.white, Color.cyan, t);
            materials[i, 2] = new Material(material);
            materials[i, 2].color = Color.LerpUnclamped(Color.white, Color.black, t);
            materials[i, 3] = new Material(material);
            materials[i, 3].color = Color.LerpUnclamped(Color.white, Color.red, t);
        }
    }
   
    private IEnumerator CreateChilds()
    {
        foreach (var element in directionRotation)
        {
            var child = new GameObject("Child");
            
            child.AddComponent<BoxCollider>(); 
            child.AddComponent<Fractal>().Initialize(this, element);
            allObjects.Add(child);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
        }
    }

    public void MakeItRigidbody()
    {
        foreach (var child in allObjects)
        {
            child.AddComponent<Rigidbody>();
            child.GetComponent<Rigidbody>().mass = 0.05f;
        }
    }
}