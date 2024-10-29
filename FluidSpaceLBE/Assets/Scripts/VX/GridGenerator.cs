using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GridGenerator : MonoBehaviour
{
    public Material linesMaterial;
    public Material pointsMaterial;
    public Transform target;
    public int dimension;
    public float spacing = 1.0f;  // 新增的spacing参数，用于控制网格密度
    public GameObject pointPrefab1;
    public GameObject pointPrefab2;
    public float prefab1Weight = 0.5f; // Prefab1出现的权重
    public float prefab2Weight = 0.5f; // Prefab2出现的权重

    Vector3 center;
    GameObject points;

    const string ALPHA = "_Alpha";
    const float MAXTRANSPARENCY_LINE = 0.013f;
    const float MAXTRANSPARENCY_DOT = 0.35f;

    void Start(){
        SpawnPoints();
        ResetMaterials();
        center = transform.position;
        center = Vector3.zero;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void OnEndCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        Draw3DMatrix();
    }

    void Draw3DMatrix(){    
        Vector3 offset = center + new Vector3(dimension/2 * spacing, dimension/2 * spacing, dimension/2 * spacing);

        for(int i=0; i<=dimension; i++){
           for(int j=0; j<=dimension; j++){
                Vector3 A1 = new Vector3(0, i * spacing, j * spacing) - offset;
                Vector3 B1 = new Vector3(center.x + dimension * spacing, i * spacing, j * spacing) - offset;
                DrawLine(A1, B1);

                Vector3 A2 = new Vector3(i * spacing, j * spacing, 0) - offset;
                Vector3 B2 = new Vector3(i * spacing, j * spacing, center.z + dimension * spacing) - offset;
                DrawLine(A2, B2);
           }
        }

        for(int i=0; i<=dimension; i++){
           for(int j=0; j<=dimension; j++){
                Vector3 A1 = new Vector3(i * spacing, j * spacing, 0) - offset;
                Vector3 B1 = new Vector3(i * spacing, j * spacing, center.z + dimension * spacing) - offset;
                DrawLine(A1, B1);

                Vector3 A2 = new Vector3(i * spacing, 0, j * spacing) - offset;
                Vector3 B2 = new Vector3(i * spacing, center.y + dimension * spacing, j * spacing) - offset;
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
        points = new GameObject("Points");
        Vector3 offset = center + new Vector3(dimension/2 * spacing, dimension/2 * spacing, dimension/2 * spacing);

        for(int i=0; i<=dimension; i++){
            for(int j=0; j<=dimension; j++){
                for(int k=0; k<=dimension; k++){
                    Vector3 pos = new Vector3(i * spacing, j * spacing, k * spacing) - offset;
                    GameObject prefabToSpawn = ChoosePrefab();
                    GameObject p = Instantiate(prefabToSpawn, pos, Quaternion.identity);
                    p.transform.SetParent(points.transform);
                }
            }
        }
    }

    GameObject ChoosePrefab() {
        float totalWeight = prefab1Weight + prefab2Weight;
        float randomValue = UnityEngine.Random.Range(0, totalWeight);

        if (randomValue < prefab1Weight) {
            return pointPrefab1;
        } else {
            return pointPrefab2;
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
        float lineAlpha = 0;
        float pointAlpha = 0;
        linesMaterial.SetFloat(ALPHA, lineAlpha);
        pointsMaterial.SetFloat(ALPHA, pointAlpha);
        while (lineAlpha < MAXTRANSPARENCY_LINE || pointAlpha < MAXTRANSPARENCY_DOT){
            if (lineAlpha < MAXTRANSPARENCY_LINE) {
                lineAlpha += 0.01f;
                linesMaterial.SetFloat(ALPHA, lineAlpha);
            }
            if (pointAlpha < MAXTRANSPARENCY_DOT) {
                pointAlpha += 0.01f;
                pointsMaterial.SetFloat(ALPHA, pointAlpha);
            }
            yield return null;
        }
    }

    IEnumerator HideCoroutine(){
        float lineAlpha = MAXTRANSPARENCY_LINE;
        float pointAlpha = MAXTRANSPARENCY_DOT;
        linesMaterial.SetFloat(ALPHA, lineAlpha);
        pointsMaterial.SetFloat(ALPHA, pointAlpha);
        while (lineAlpha > 0f || pointAlpha > 0f){
            if (lineAlpha > 0f) {
                lineAlpha -= 0.01f;
                linesMaterial.SetFloat(ALPHA, lineAlpha);
            }
            if (pointAlpha > 0f) {
                pointAlpha -= 0.01f;
                pointsMaterial.SetFloat(ALPHA, pointAlpha);
            }
            yield return null;
        }
    }

}