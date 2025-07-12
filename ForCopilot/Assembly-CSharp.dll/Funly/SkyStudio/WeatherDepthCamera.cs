using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001EE RID: 494
	[RequireComponent(typeof(Camera))]
	public class WeatherDepthCamera : MonoBehaviour
	{
		// Token: 0x06000AFD RID: 2813 RVA: 0x000307BB File Offset: 0x0002E9BB
		private void Start()
		{
			this.m_DepthCamera = base.GetComponent<Camera>();
			this.m_DepthCamera.enabled = false;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x000307D5 File Offset: 0x0002E9D5
		private void Update()
		{
			if (this.m_DepthCamera.enabled)
			{
				this.m_DepthCamera.enabled = false;
			}
			if (Time.frameCount % this.renderFrameInterval != 0)
			{
				return;
			}
			this.RenderOverheadCamera();
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00030808 File Offset: 0x0002EA08
		private void RenderOverheadCamera()
		{
			this.PrepareRenderTexture();
			if (this.depthShader == null)
			{
				Debug.LogError("Can't render depth since depth shader is missing.");
				return;
			}
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = this.overheadDepthTexture;
			GL.Clear(true, true, Color.black);
			this.m_DepthCamera.RenderWithShader(this.depthShader, "RenderType");
			RenderTexture.active = active;
			Shader.SetGlobalTexture("_OverheadDepthTex", this.overheadDepthTexture);
			Shader.SetGlobalVector("_OverheadDepthPosition", this.m_DepthCamera.transform.position);
			Shader.SetGlobalFloat("_OverheadDepthNearClip", this.m_DepthCamera.nearClipPlane);
			Shader.SetGlobalFloat("_OverheadDepthFarClip", this.m_DepthCamera.farClipPlane);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x000308C4 File Offset: 0x0002EAC4
		private void PrepareRenderTexture()
		{
			if (this.overheadDepthTexture == null)
			{
				int num = Mathf.ClosestPowerOfTwo(Mathf.FloorToInt((float)this.textureResolution));
				RenderTextureFormat format = RenderTextureFormat.ARGB32;
				this.overheadDepthTexture = new RenderTexture(num, num, 24, format, RenderTextureReadWrite.Linear);
				this.overheadDepthTexture.useMipMap = false;
				this.overheadDepthTexture.autoGenerateMips = false;
				this.overheadDepthTexture.filterMode = FilterMode.Point;
				this.overheadDepthTexture.antiAliasing = 2;
			}
			if (!this.overheadDepthTexture.IsCreated())
			{
				this.overheadDepthTexture.Create();
			}
			if (this.m_DepthCamera.targetTexture != this.overheadDepthTexture)
			{
				this.m_DepthCamera.targetTexture = this.overheadDepthTexture;
			}
		}

		// Token: 0x04000BB9 RID: 3001
		private Camera m_DepthCamera;

		// Token: 0x04000BBA RID: 3002
		[Tooltip("Shader used to render out depth + normal texture. This should be the sky studio depth shader.")]
		public Shader depthShader;

		// Token: 0x04000BBB RID: 3003
		[HideInInspector]
		public RenderTexture overheadDepthTexture;

		// Token: 0x04000BBC RID: 3004
		[Tooltip("You can help increase performance by only rendering periodically some number of frames.")]
		[Range(1f, 60f)]
		public int renderFrameInterval = 5;

		// Token: 0x04000BBD RID: 3005
		[Tooltip("The resolution of the texture. Higher resolution uses more rendering time but makes more precise weather along edges.")]
		[Range(128f, 8192f)]
		public int textureResolution = 1024;
	}
}
