using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ItemIconCreator
{
	// Token: 0x0200022C RID: 556
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[DisallowMultipleComponent]
	public class IconCreator : MonoBehaviour
	{
		// Token: 0x06000BD5 RID: 3029 RVA: 0x000368FC File Offset: 0x00034AFC
		private void Awake()
		{
			this.mainCam = base.gameObject.GetComponent<Camera>();
			this.originalClearFlags = this.mainCam.clearFlags;
			if (IconCreatorCanvas.instance != null)
			{
				IconCreatorCanvas.instance.SetInfo(0, 0, "", false, this.nextIconKey);
			}
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00036950 File Offset: 0x00034B50
		protected void Initialize()
		{
			this.mainCam.clearFlags = this.originalClearFlags;
			this.isCreatingIcons = true;
			foreach (Camera camera in UnityEngine.Object.FindObjectsOfType<Camera>())
			{
				if (!(camera == this.mainCam))
				{
					camera.gameObject.SetActive(false);
				}
			}
			if (this.useTransparency)
			{
				this.CreateBlackAndWhiteCameras();
			}
			this.CacheAndInitialiseFields();
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000369BC File Offset: 0x00034BBC
		protected void DeleteCameras()
		{
			if (this.whiteCam != null)
			{
				UnityEngine.Object.Destroy(this.whiteCam.gameObject);
			}
			if (this.blackCam != null)
			{
				UnityEngine.Object.Destroy(this.blackCam.gameObject);
			}
			this.isCreatingIcons = false;
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00036A0C File Offset: 0x00034C0C
		public virtual void BuildIcons()
		{
			Debug.LogError("Not implemented");
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00036A18 File Offset: 0x00034C18
		protected IEnumerator CaptureFrame(string objectName, int i)
		{
			if (this.whiteCam != null)
			{
				this.whiteCam.enabled = true;
			}
			if (this.blackCam != null)
			{
				this.blackCam.enabled = true;
			}
			yield return new WaitForEndOfFrame();
			if (this.useTransparency)
			{
				this.RenderCamToTexture(this.blackCam, this.texBlack);
				this.RenderCamToTexture(this.whiteCam, this.texWhite);
				this.CalculateOutputTexture();
			}
			else
			{
				this.RenderCamToTexture(this.mainCam, this.finalTexture);
			}
			this.SavePng(objectName, i);
			this.mainCam.enabled = true;
			yield break;
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00036A38 File Offset: 0x00034C38
		protected virtual void Update()
		{
			if (this.mode == IconCreator.Mode.Automatic)
			{
				return;
			}
			if (!this.CanMove)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0))
			{
				this.mousePostion = Input.mousePosition;
			}
			if (Input.GetMouseButton(0))
			{
				Vector3 vector = this.mousePostion - Input.mousePosition;
				this.currentObject.Rotate(new Vector3(-vector.y, vector.x, vector.z) * Time.deltaTime * 40f, Space.World);
				this.mousePostion = Input.mousePosition;
				if (this.dynamicFov)
				{
					this.UpdateFOV(this.currentObject.gameObject);
				}
				if (this.lookAtObjectCenter)
				{
					this.LookAtTargetCenter(this.currentObject.gameObject);
				}
			}
			this.UpdateFOV(Input.mouseScrollDelta.y * -2.5f);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00036B11 File Offset: 0x00034D11
		private void RenderCamToTexture(Camera cam, Texture2D tex)
		{
			cam.enabled = true;
			cam.Render();
			this.WriteScreenImageToTexture(tex);
			cam.enabled = false;
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00036B30 File Offset: 0x00034D30
		private void CreateBlackAndWhiteCameras()
		{
			this.mainCam.clearFlags = CameraClearFlags.Color;
			GameObject gameObject = new GameObject();
			gameObject.name = "White Background Camera";
			this.whiteCam = gameObject.AddComponent<Camera>();
			this.whiteCam.CopyFrom(this.mainCam);
			this.whiteCam.backgroundColor = Color.white;
			gameObject.transform.SetParent(base.gameObject.transform, true);
			gameObject = new GameObject();
			gameObject.name = "Black Background Camera";
			this.blackCam = gameObject.AddComponent<Camera>();
			this.blackCam.CopyFrom(this.mainCam);
			this.blackCam.backgroundColor = Color.black;
			gameObject.transform.SetParent(base.gameObject.transform, true);
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00036BF4 File Offset: 0x00034DF4
		protected void CreateNewFolderForIcons()
		{
			this.finalPath = this.GetFinalFolder();
			if (Directory.Exists(this.finalPath))
			{
				int num = 1;
				while (Directory.Exists(this.finalPath + " " + num.ToString()))
				{
					num++;
				}
				this.finalPath = this.finalPath + " " + num.ToString();
			}
			Directory.CreateDirectory(this.finalPath);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00036C69 File Offset: 0x00034E69
		public string GetFinalFolder()
		{
			if (!string.IsNullOrWhiteSpace(this.GetBaseLocation()))
			{
				return Path.Combine(this.GetBaseLocation(), this.folderName);
			}
			return this.folderName;
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00036C90 File Offset: 0x00034E90
		private void WriteScreenImageToTexture(Texture2D tex)
		{
			tex.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.width), 0, 0);
			tex.Apply();
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00036CBC File Offset: 0x00034EBC
		private void CalculateOutputTexture()
		{
			for (int i = 0; i < this.finalTexture.height; i++)
			{
				for (int j = 0; j < this.finalTexture.width; j++)
				{
					float num = this.texWhite.GetPixel(j, i).r - this.texBlack.GetPixel(j, i).r;
					num = 1f - num;
					Color color;
					if (num == 0f)
					{
						color = Color.clear;
					}
					else
					{
						color = this.texBlack.GetPixel(j, i) / num;
					}
					color.a = num;
					this.finalTexture.SetPixel(j, i, color);
				}
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00036D64 File Offset: 0x00034F64
		private void SavePng(string name, int i)
		{
			string fileName = this.GetFileName(name, i);
			string text = this.finalPath + "/" + fileName;
			Debug.Log("Writing to: " + text);
			byte[] bytes = ImageConversion.EncodeToPNG(this.finalTexture);
			File.WriteAllBytes(text, bytes);
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00036DB0 File Offset: 0x00034FB0
		public string GetFileName(string name, int i)
		{
			string str;
			if (this.useDafaultName)
			{
				str = name;
			}
			else
			{
				str = this.iconFileName;
			}
			str += " Icon";
			if (this.includeResolutionInFileName)
			{
				str = str + " " + this.mainCam.scaledPixelHeight.ToString() + "x";
			}
			return str + ".png";
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00036E14 File Offset: 0x00035014
		private void CacheAndInitialiseFields()
		{
			this.texBlack = new Texture2D(this.mainCam.pixelWidth, this.mainCam.pixelHeight, TextureFormat.RGB24, false);
			this.texWhite = new Texture2D(this.mainCam.pixelWidth, this.mainCam.pixelHeight, TextureFormat.RGB24, false);
			this.finalTexture = new Texture2D(this.mainCam.pixelWidth, this.mainCam.pixelHeight, TextureFormat.ARGB32, false);
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00036E8C File Offset: 0x0003508C
		protected void UpdateFOV(GameObject targetItem)
		{
			float targetFov = this.GetTargetFov(targetItem);
			if (this.useTransparency && this.isCreatingIcons)
			{
				this.whiteCam.fieldOfView = targetFov;
				this.blackCam.fieldOfView = targetFov;
			}
			this.mainCam.fieldOfView = targetFov;
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00036ED8 File Offset: 0x000350D8
		protected void UpdateFOV(float value)
		{
			if (value == 0f)
			{
				return;
			}
			value = this.mainCam.fieldOfView * value / 100f;
			this.dynamicFov = false;
			if (this.useTransparency)
			{
				this.whiteCam.fieldOfView += value;
				this.blackCam.fieldOfView += value;
			}
			this.mainCam.fieldOfView += value;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00036F4C File Offset: 0x0003514C
		protected void LookAtTargetCenter(GameObject targetItem)
		{
			Vector3 meshCenter = this.GetMeshCenter(targetItem);
			this.mainCam.transform.LookAt(meshCenter);
			if (this.whiteCam != null)
			{
				this.whiteCam.transform.LookAt(meshCenter);
			}
			if (this.blackCam != null)
			{
				this.blackCam.transform.LookAt(meshCenter);
			}
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00036FB0 File Offset: 0x000351B0
		private float GetTargetFov(GameObject a)
		{
			Vector3 vector = Vector3.one * 30000f;
			Vector3 vector2 = Vector3.zero;
			List<Renderer> renderers = this.GetRenderers(a);
			for (int i = 0; i < renderers.Count; i++)
			{
				if (Vector3.Distance(Vector3.zero, renderers[i].bounds.min) < Vector3.Distance(Vector3.zero, vector))
				{
					vector = renderers[i].bounds.min;
				}
				if (Vector3.Distance(Vector3.zero, renderers[i].bounds.max) > Vector3.Distance(Vector3.zero, vector2))
				{
					vector2 = renderers[i].bounds.max;
				}
			}
			Vector3 a2 = (vector + vector2) / 2f;
			float num = (vector2 - vector).magnitude / 2f;
			float num2 = Vector3.Distance(a2, base.transform.position);
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			return Mathf.Asin(num / num3) * 57.29578f * 2f + this.fovOffset;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x000370E4 File Offset: 0x000352E4
		private List<Renderer> GetRenderers(GameObject obj)
		{
			List<Renderer> list = new List<Renderer>();
			if (obj.GetComponents<Renderer>() != null)
			{
				list.AddRange(obj.GetComponents<Renderer>());
			}
			if (obj.GetComponentsInChildren<Renderer>() != null)
			{
				list.AddRange(obj.GetComponentsInChildren<Renderer>());
			}
			return list;
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00037120 File Offset: 0x00035320
		private Vector3 GetMeshCenter(GameObject a)
		{
			Vector3 a2 = Vector3.zero;
			List<Renderer> renderers = this.GetRenderers(a);
			if (renderers == null)
			{
				Debug.LogError("No mesh was founded in object " + a.name);
				return a.transform.position;
			}
			for (int i = 0; i < renderers.Count; i++)
			{
				a2 += renderers[i].bounds.center;
			}
			return a2 / (float)renderers.Count;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x000045B1 File Offset: 0x000027B1
		protected void RevealInFinder()
		{
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0003719C File Offset: 0x0003539C
		public virtual bool CheckConditions()
		{
			if (this.pathLocation == IconCreator.SaveLocation.custom && !Directory.Exists(this.folderName))
			{
				Debug.LogError("Folder " + this.folderName + " does not exists");
				return false;
			}
			if (!this.useDafaultName && string.IsNullOrWhiteSpace(this.iconFileName))
			{
				Debug.LogError("Invalid icon file name");
				return false;
			}
			return true;
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x000371FD File Offset: 0x000353FD
		private string GetBaseLocation()
		{
			if (this.pathLocation == IconCreator.SaveLocation.dataPath)
			{
				return Application.dataPath;
			}
			if (this.pathLocation == IconCreator.SaveLocation.persistentDataPath)
			{
				return Application.persistentDataPath;
			}
			if (this.pathLocation == IconCreator.SaveLocation.projectFolder)
			{
				return Path.GetDirectoryName(Application.dataPath);
			}
			return "";
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00037235 File Offset: 0x00035435
		private void OnValidate()
		{
			if (this.mainCam == null)
			{
				this.mainCam = base.GetComponent<Camera>();
			}
		}

		// Token: 0x04000D13 RID: 3347
		protected bool isCreatingIcons;

		// Token: 0x04000D14 RID: 3348
		public bool useDafaultName;

		// Token: 0x04000D15 RID: 3349
		public bool includeResolutionInFileName;

		// Token: 0x04000D16 RID: 3350
		public string iconFileName;

		// Token: 0x04000D17 RID: 3351
		public IconCreator.SaveLocation pathLocation;

		// Token: 0x04000D18 RID: 3352
		public IconCreator.Mode mode;

		// Token: 0x04000D19 RID: 3353
		public string folderName = "Screenshots";

		// Token: 0x04000D1A RID: 3354
		public bool useTransparency = true;

		// Token: 0x04000D1B RID: 3355
		public bool lookAtObjectCenter;

		// Token: 0x04000D1C RID: 3356
		public bool dynamicFov;

		// Token: 0x04000D1D RID: 3357
		public float fovOffset;

		// Token: 0x04000D1E RID: 3358
		protected string finalPath;

		// Token: 0x04000D1F RID: 3359
		private Vector3 mousePostion;

		// Token: 0x04000D20 RID: 3360
		public KeyCode nextIconKey = KeyCode.Space;

		// Token: 0x04000D21 RID: 3361
		protected bool CanMove;

		// Token: 0x04000D22 RID: 3362
		public bool preview = true;

		// Token: 0x04000D23 RID: 3363
		protected Camera whiteCam;

		// Token: 0x04000D24 RID: 3364
		protected Camera blackCam;

		// Token: 0x04000D25 RID: 3365
		public Camera mainCam;

		// Token: 0x04000D26 RID: 3366
		protected Texture2D texBlack;

		// Token: 0x04000D27 RID: 3367
		protected Texture2D texWhite;

		// Token: 0x04000D28 RID: 3368
		protected Texture2D finalTexture;

		// Token: 0x04000D29 RID: 3369
		private CameraClearFlags originalClearFlags;

		// Token: 0x04000D2A RID: 3370
		protected Transform currentObject;

		// Token: 0x0200022D RID: 557
		public enum SaveLocation
		{
			// Token: 0x04000D2C RID: 3372
			persistentDataPath,
			// Token: 0x04000D2D RID: 3373
			dataPath,
			// Token: 0x04000D2E RID: 3374
			projectFolder,
			// Token: 0x04000D2F RID: 3375
			custom
		}

		// Token: 0x0200022E RID: 558
		public enum Mode
		{
			// Token: 0x04000D31 RID: 3377
			Automatic,
			// Token: 0x04000D32 RID: 3378
			Manual
		}
	}
}
