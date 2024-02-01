using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShell : MonoBehaviour {
    public Mesh shellMesh;
    public Material shellMaterial;

    public bool updateStatics = true;

    // These variables and what they do are explained on the shader code side of things
    // You can see below (line 70) which shader uniforms match up with these variables
    [Range(1, 256)]
    public int shellCount = 16;

    [Range(0.0f, 1.0f)]
    public float shellLength = 0.15f;

    [Range(0.01f, 3.0f)]
    public float distanceAttenuation = 1.0f;

    [Range(1.0f, 1000.0f)]
    public float density = 100.0f;

    [Range(0.0f, 1.0f)]
    public float noiseMin = 0.0f;

    [Range(0.0f, 1.0f)]
    public float noiseMax = 1.0f;

    [Range(0.0f, 10.0f)]
    public float thickness = 1.0f;

    [Range(0.0f, 10.0f)]
    public float curvature = 1.0f;

    [Range(0.0f, 1.0f)]
    public float displacementStrength = 0.1f;

    public Color shellColor;
    

    [Range(0.0f, 5.0f)]
    public float occlusionAttenuation = 1.0f;
    
    [Range(0.0f, 1.0f)]
    public float occlusionBias = 0.0f;

    // private Material shellMaterial;
    private GameObject[] shells;

    private Vector3 displacementDirection = new Vector3(0, 0, 0);

    void OnEnable() {
        shells = new GameObject[shellCount];

        for (int i = 1; i < shellCount; ++i) {
            shells[i] = new GameObject("Shell " + i.ToString());
            shells[i].layer = gameObject.layer;
            shells[i].transform.localPosition = new Vector3(0, 0, -(float) i / shellCount * shellLength);
            shells[i].AddComponent<MeshFilter>();
            shells[i].AddComponent<MeshRenderer>();
            
            shells[i].GetComponent<MeshFilter>().mesh = shellMesh;
            shells[i].GetComponent<MeshRenderer>().material = shellMaterial;
            shells[i].transform.SetParent(this.transform, false);

            // Material material = shells[i].GetComponent<MeshRenderer>().material;
            
            var propertyBlock = new MaterialPropertyBlock();

            propertyBlock.SetInt("_ShellCount", shellCount);
            propertyBlock.SetInt("_ShellIndex", i);
            propertyBlock.SetFloat("_ShellLength", shellLength);
            propertyBlock.SetFloat("_Density", density);
            propertyBlock.SetFloat("_Thickness", thickness);
            propertyBlock.SetFloat("_Attenuation", occlusionAttenuation);
            propertyBlock.SetFloat("_ShellDistanceAttenuation", distanceAttenuation);
            propertyBlock.SetFloat("_Curvature", curvature);
            propertyBlock.SetFloat("_DisplacementStrength", displacementStrength);
            propertyBlock.SetFloat("_OcclusionBias", occlusionBias);
            propertyBlock.SetFloat("_NoiseMin", noiseMin);
            propertyBlock.SetFloat("_NoiseMax", noiseMax);
            propertyBlock.SetVector("_ShellColor", shellColor);
            propertyBlock.SetVector("_TilePosition", transform.position);
            
            shells[i].GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
        }
    }

    void OnDisable() {
        for (int i = 0; i < shells.Length; ++i) {
            Destroy(shells[i]);
        }

        shells = null;
    }
}
