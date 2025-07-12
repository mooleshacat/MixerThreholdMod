using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Funly.SkyStudio
{
	// Token: 0x020001F6 RID: 502
	[RequireComponent(typeof(Camera))]
	[RequireComponent(typeof(UniversalAdditionalCameraData))]
	public class URPWeatherDepth : MonoBehaviour
	{
		// Token: 0x06000B20 RID: 2848 RVA: 0x00030E2B File Offset: 0x0002F02B
		private void Start()
		{
			this.m_Camera = base.GetComponent<Camera>();
			this.m_CameraData = base.GetComponent<UniversalAdditionalCameraData>();
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00030E48 File Offset: 0x0002F048
		private void Update()
		{
			this.m_CameraData.SetRenderer(1);
			Shader.SetGlobalTexture("_OverheadDepthTex", this.renderTexture);
			Shader.SetGlobalVector("_OverheadDepthPosition", this.m_Camera.transform.position);
			Shader.SetGlobalFloat("_OverheadDepthNearClip", this.m_Camera.nearClipPlane);
			Shader.SetGlobalFloat("_OverheadDepthFarClip", this.m_Camera.farClipPlane);
		}

		// Token: 0x04000BDA RID: 3034
		public RenderTexture renderTexture;

		// Token: 0x04000BDB RID: 3035
		private Camera m_Camera;

		// Token: 0x04000BDC RID: 3036
		private UniversalAdditionalCameraData m_CameraData;
	}
}
