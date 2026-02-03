using System;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public int Dimension = 10;
    public Octave[]  Octaves;
    public float UVScale;
    
    protected MeshFilter meshFilter;

    protected Mesh mesh;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = new Mesh();
        mesh.name = gameObject.name;

        mesh.vertices = GenerateVerts();
        mesh.triangles = GenerateTries();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.uv = GenerateUVS();
        
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(Dimension +1) * (Dimension +1 )];
        float helf = Dimension / 2f;
        
        for (int x = 0; x <= Dimension; x++)
        {
            for (int z = 0; z <= Dimension; z++)
            {
                verts[index(x, z)] = new Vector3(x, 0, z);
            }
        }
        return verts;
    }

    private int index(int x, int z)
    {
        return x * (Dimension +1 ) + z;
    }

    private int[] GenerateTries()
    {
        var tries = new int[mesh.vertices.Length * 6];

        for (int x = 0; x < Dimension; x++)
        {
            for (int z = 0; z < Dimension; z++)
            {
                tries[index(x,z) * 6 + 0] = index(x,z);
                tries[index(x,z) * 6 + 1] = index(x +1 ,z +1);
                tries[index(x,z) * 6 + 2] = index(x +1 ,z);
                tries[index(x,z) * 6 + 3] = index(x,z);
                tries[index(x,z) * 6 + 4] = index(x,z + 1);
                tries[index(x,z) * 6 + 5] = index(x + 1 ,z + 1);
            }
        }
        return tries;
    }
    
    private Vector2[] GenerateUVS()
    {
        var uvs = new  Vector2[mesh.vertices.Length];

        for (int x = 0; x <= Dimension; x++)
        {
            for (int z = 0; z <= Dimension; z++)
            {
                var vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[index(x,z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }
        return uvs;
    }
    
    public float GetHeight(Vector3 position)
    {
        //scale factor and position in local space
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localPos = Vector3.Scale((position - transform.position), scale);

        //get edge points
        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        //clamp if the position is outside the plane
        p1.x = Mathf.Clamp(p1.x, 0, Dimension);
        p1.z = Mathf.Clamp(p1.z, 0, Dimension);
        p2.x = Mathf.Clamp(p2.x, 0, Dimension);
        p2.z = Mathf.Clamp(p2.z, 0, Dimension);
        p3.x = Mathf.Clamp(p3.x, 0, Dimension);
        p3.z = Mathf.Clamp(p3.z, 0, Dimension);
        p4.x = Mathf.Clamp(p4.x, 0, Dimension);
        p4.z = Mathf.Clamp(p4.z, 0, Dimension);

        //get the max distance to one of the edges and take that to compute max - dist
        var max = Mathf.Max(
            Vector3.Distance(p1, localPos),
            Vector3.Distance(p2, localPos),
            Vector3.Distance(p3, localPos),
            Vector3.Distance(p4, localPos) + Mathf.Epsilon
        );

        var dist =
            (max - Vector3.Distance(p1, localPos)) +
            (max - Vector3.Distance(p2, localPos)) +
            (max - Vector3.Distance(p3, localPos)) +
            (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);

        //weighted sum
        var height =
            mesh.vertices[index((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localPos)) +
            mesh.vertices[index((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localPos)) +
            mesh.vertices[index((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localPos)) +
            mesh.vertices[index((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localPos));

        //scale
        return height * transform.lossyScale.y / dist;
    }
    
    // Update is called once per frame
    void Update()
    {
        var verts = mesh.vertices;
        for (int x = 0; x <= Dimension; x++)
        {
            for (int z = 0; z <= Dimension; z++)
            {
                var y = 0f;
                
                for (int o = 0; o < Octaves.Length; o++)
                {
                    if (Octaves[o].alternate)
                    {
                        var perl = Mathf.PerlinNoise((x * Octaves[o].scale.x) / Dimension,
                            (z * Octaves[o].scale.y) / Dimension) * Mathf.PI * 2f;
                        
                        y += Mathf.Cos(Octaves[o].speed.magnitude * Time.time) * Octaves[o].height;
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x * Octaves[o].scale.x + Time.time * Octaves[o].speed.x) / Dimension, (z * Octaves[0].scale.y + Time.time * Octaves[o].speed.y) / Dimension) - 0.5f;
                        y += perl * Octaves[o].height;
                    }
                }
                
                verts[index(x,z)] = new Vector3(x, y, z);
            }
        }

        mesh.vertices = verts;
    }

    [Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}
