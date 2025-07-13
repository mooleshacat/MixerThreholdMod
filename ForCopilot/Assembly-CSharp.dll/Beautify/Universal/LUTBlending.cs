using System;
using UnityEngine;

namespace Beautify.Universal
{
	// Token: 0x020001FD RID: 509
	[ExecuteInEditMode]
	public class LUTBlending : MonoBehaviour
	{
		// Token: 0x06000B3C RID: 2876 RVA: 0x0003123F File Offset: 0x0002F43F
		private void OnEnable()
		{
			this.UpdateBeautifyLUT();
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00031247 File Offset: 0x0002F447
		private void OnValidate()
		{
			this.oldPhase = -1f;
			this.UpdateBeautifyLUT();
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0003125A File Offset: 0x0002F45A
		private void OnDestroy()
		{
			if (this.rt != null)
			{
				this.rt.Release();
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0003123F File Offset: 0x0002F43F
		private void LateUpdate()
		{
			this.UpdateBeautifyLUT();
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00031278 File Offset: 0x0002F478
		private void UpdateBeautifyLUT()
		{
			if (this.oldPhase == this.phase || this.LUT1 == null || this.LUT2 == null || this.lerpShader == null)
			{
				return;
			}
			this.oldPhase = this.phase;
			if (this.rt == null)
			{
				this.rt = new RenderTexture(this.LUT1.width, this.LUT1.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
				this.rt.filterMode = FilterMode.Point;
			}
			if (this.lerpMat == null)
			{
				this.lerpMat = new Material(this.lerpShader);
			}
			this.lerpMat.SetTexture(LUTBlending.ShaderParams.LUT2, this.LUT2);
			this.lerpMat.SetFloat(LUTBlending.ShaderParams.Phase, this.phase);
			Graphics.Blit(this.LUT1, this.rt, this.lerpMat);
			BeautifySettings.settings.lut.Override(true);
			float num = Mathf.Lerp(this.LUT1Intensity, this.LUT2Intensity, this.phase);
			BeautifySettings.settings.lutIntensity.Override(num);
			BeautifySettings.settings.lutTexture.Override(this.rt);
		}

		// Token: 0x04000BDE RID: 3038
		public Texture2D LUT1;

		// Token: 0x04000BDF RID: 3039
		public Texture2D LUT2;

		// Token: 0x04000BE0 RID: 3040
		[Range(0f, 1f)]
		public float LUT1Intensity = 1f;

		// Token: 0x04000BE1 RID: 3041
		[Range(0f, 1f)]
		public float LUT2Intensity = 1f;

		// Token: 0x04000BE2 RID: 3042
		[Range(0f, 1f)]
		public float phase;

		// Token: 0x04000BE3 RID: 3043
		public Shader lerpShader;

		// Token: 0x04000BE4 RID: 3044
		private float oldPhase = -1f;

		// Token: 0x04000BE5 RID: 3045
		private RenderTexture rt;

		// Token: 0x04000BE6 RID: 3046
		private Material lerpMat;

		// Token: 0x020001FE RID: 510
		private static class ShaderParams
		{
			// Token: 0x04000BE7 RID: 3047
			public static int LUT2 = Shader.PropertyToID("_LUT2");

			// Token: 0x04000BE8 RID: 3048
			public static int Phase = Shader.PropertyToID("_Phase");
		}
	}
}
