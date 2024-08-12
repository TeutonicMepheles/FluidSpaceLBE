using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class TeleVX_ScannerController : MonoBehaviour
{
    const int MAXDISTORSION = 76;
	const int MINDISTORSION = 1;
	const float MAXDISTANCE = 15;
	const float MINDISTANCE = 0.1f;

    public Transform origin;
	public Material scannerMat;
	public float initialScanDistance = 0;
	public float maxScanDistance = 20;
	public float scanThickness;
	[SerializeField] private TeleVX_LineGenerator lineGenerator;
	[SerializeField] private PostProcessVolume ppv;
	[ColorUsage(true, true)]
	public List<Color> colors;

    public Camera cam;
    public float scanDistance;
    public float scanDistance1;
    public float scanDistance2;

	IEnumerator scanHandler = null;

	LensDistortion lensDistortion = null;
	DepthOfField dof = null;

	void Start(){
		ppv.profile.TryGetSettings(out lensDistortion);
		ppv.profile.TryGetSettings(out dof);
	}

	void OnEnable(){
		cam.depthTextureMode = DepthTextureMode.Depth;
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst){
		scannerMat.SetVector("_WorldSpaceScannerPos", origin.position);
		scannerMat.SetFloat("_ScanDistance1", scanDistance1);
		scannerMat.SetFloat("_ScanDistance2", scanDistance2);
		scannerMat.SetFloat("_ScanWidth", scanThickness);

		if(colors.Count >= 3){
			scannerMat.SetColor("_FirstColor", colors[0]);
			scannerMat.SetColor("_SecondColor", colors[1]);
			scannerMat.SetColor("_FillColor", colors[2]);
		}

		RaycastCornerBlit(src, dst, scannerMat);
	}

	void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat){
		float camFar = cam.farClipPlane;
		float camFov = cam.fieldOfView;
		float camAspect = cam.aspect;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = cam.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = cam.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (cam.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (cam.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (cam.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (cam.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		RenderTexture.active = dest;

		mat.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		mat.SetPass(0);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
	}

	void CheckAndBlock(){
		if(scanHandler != null){
			StopCoroutine(scanHandler);
		}
	}

    public void Scan(){
	    CheckAndBlock();
		scanHandler = ScanCoroutine();
        StartCoroutine(scanHandler);
    }

    public void ScanBack(){
		CheckAndBlock();
		scanHandler = ScanBackCoroutine();
        StartCoroutine(scanHandler);
    }

    IEnumerator ScanCoroutine(){
	    Distorsion();
        scanDistance = initialScanDistance;
		bool startShow = false;   
        while(scanDistance <= maxScanDistance ){
			if(scanDistance > maxScanDistance/2 && !startShow){
				lineGenerator.Show();
				startShow = true;
			}
            scanDistance1 = scanDistance+1;
            yield return new WaitForSecondsRealtime(.01f);
            scanDistance2 = scanDistance1;
            yield return new WaitForSecondsRealtime(.01f);
            scanDistance2 = scanDistance1+1;
            yield return new WaitForSecondsRealtime(.01f);
            scanDistance ++;

        }
        scanDistance = maxScanDistance;  
		scanHandler = null;
    }

	IEnumerator ScanBackCoroutine(){
		bool startHide = false;   
        while(scanDistance > 0 ){
			if(scanDistance < maxScanDistance/2 && !startHide){
				lineGenerator.Hide();
				startHide = true;
				Distorsion();
			}
            scanDistance1 = scanDistance-1;
            yield return new WaitForSecondsRealtime(.01f);
            scanDistance2 = scanDistance1;
            yield return new WaitForSecondsRealtime(.01f);
            scanDistance2 = scanDistance1-1;
            yield return new WaitForSecondsRealtime(.01f);
            scanDistance --;

        }
        scanDistance = initialScanDistance; 
		scanHandler = null; 
    }

	void Distorsion(){
		StartCoroutine(DistorsionCoroutine());
	}

	IEnumerator DistorsionCoroutine(){
		if(dof != null && lensDistortion != null){
			int distAmount = MAXDISTORSION;
			float distance = MINDISTANCE;

			while(distAmount >= MINDISTORSION ){
				lensDistortion.intensity.value = distAmount;
				dof.focusDistance.value = distance;
				distAmount -=5 ;
				distance += 0.1f;
				yield return new WaitForSecondsRealtime(.01f);
			}

			lensDistortion.intensity.value = MINDISTORSION;
			dof.focusDistance.value = MAXDISTANCE;
		}
    }
}
