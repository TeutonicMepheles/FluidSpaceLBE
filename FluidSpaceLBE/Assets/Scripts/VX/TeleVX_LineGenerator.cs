using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleVX_LineGenerator : MonoBehaviour
{
    public Material linesMaterial;
    public Material pointsMaterial;
    public Transform target;
    public int dimension;
    public GameObject pointPrefab;
    
    Vector3 center;
    GameObject points;

    const string ALPHA = "_Alpha";
    const float MAXTRANSPARENCY = 0.5f;
    
    void Start(){
        SpawnPoints();
        ResetMaterials();
        center = transform.position;
        center = Vector3.zero;
    }
    
    void OnPostRender(){
        Draw3DMatrix();
    }

    void Draw3DMatrix(){    

        Vector3 offset = center + new Vector3(dimension/2, dimension/2, dimension/2);

        for(int i=0; i<=dimension; i++){
           for(int j=0; j<=dimension; j++){
                Vector3 A1 = new Vector3(0, i, j) - offset;
                Vector3 B1 = new Vector3(center.x + dimension, i, j) - offset;
                DrawLine(A1, B1);

                Vector3 A2 = new Vector3(i, j, 0) - offset;
                Vector3 B2 = new Vector3(i, j, center.z + dimension) - offset;
                DrawLine(A2, B2);
           }
        }

        for(int i=0; i<=dimension; i++){
           for(int j=0; j<=dimension; j++){
                Vector3 A1 = new Vector3(i, j, 0) - offset;
                Vector3 B1 = new Vector3(i, j, center.z + dimension) - offset;
                DrawLine(A1, B1);

                Vector3 A2 = new Vector3(i, 0, j) - offset;
                Vector3 B2 = new Vector3(i, center.y + dimension, j) - offset;
                DrawLine(A2, B2);
           }
        }
     
    }

    void DrawLine(Vector3 from, Vector3 to){
        linesMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(from);
        GL.Vertex(to);
        GL.End();
    }

    void SpawnPoints(){
        points = new GameObject();
        Vector3 offset = center + new Vector3(dimension/2, dimension/2, dimension/2);

        for(int i=0; i<=dimension; i++){
            for(int j=0; j<=dimension; j++){
                for(int k=0; k<=dimension; k++){
                    Vector3 pos = new Vector3(i, j, k) - offset;
                    GameObject p = Instantiate(pointPrefab, pos, Quaternion.identity);
                    p.transform.SetParent(points.transform);
                }
            }
        }
    }

    void ResetMaterials(){
        linesMaterial.SetFloat(ALPHA, 0);
        pointsMaterial.SetFloat(ALPHA, 0);
    }

    public void Show(){
        StartCoroutine(ShowCoroutine());
    }
    
    public void Hide(){
        StartCoroutine(HideCoroutine());
    }

    IEnumerator ShowCoroutine(){
        float alpha = 0;
        linesMaterial.SetFloat(ALPHA, alpha);
        pointsMaterial.SetFloat(ALPHA, alpha);
        while (alpha < MAXTRANSPARENCY){
            alpha += 0.01f;
            linesMaterial.SetFloat(ALPHA, alpha);
            pointsMaterial.SetFloat(ALPHA, alpha);
            yield return null;
        }
    }

    IEnumerator HideCoroutine(){
        float alpha = MAXTRANSPARENCY;
        linesMaterial.SetFloat(ALPHA, alpha);
        pointsMaterial.SetFloat(ALPHA, alpha);
        while (alpha > 0f){
            alpha -= 0.01f;
            linesMaterial.SetFloat(ALPHA, alpha);
            pointsMaterial.SetFloat(ALPHA, alpha);
            yield return null;
        }
    }
}
