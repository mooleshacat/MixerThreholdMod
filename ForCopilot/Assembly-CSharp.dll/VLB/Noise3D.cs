using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200013C RID: 316
	public static class Noise3D
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x00019FD6 File Offset: 0x000181D6
		public static bool isSupported
		{
			get
			{
				if (!Noise3D.ms_IsSupportedChecked)
				{
					Noise3D.ms_IsSupported = (SystemInfo.graphicsShaderLevel >= 35);
					if (!Noise3D.ms_IsSupported)
					{
						Debug.LogWarning(Noise3D.isNotSupportedString);
					}
					Noise3D.ms_IsSupportedChecked = true;
				}
				return Noise3D.ms_IsSupported;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0001A00C File Offset: 0x0001820C
		public static bool isProperlyLoaded
		{
			get
			{
				return Noise3D.ms_NoiseTexture != null;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0001A019 File Offset: 0x00018219
		public static string isNotSupportedString
		{
			get
			{
				return string.Format("3D Noise requires higher shader capabilities (Shader Model 3.5 / OpenGL ES 3.0), which are not available on the current platform: graphicsShaderLevel (current/required) = {0} / {1}", SystemInfo.graphicsShaderLevel, 35);
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001A036 File Offset: 0x00018236
		[RuntimeInitializeOnLoadMethod]
		private static void OnStartUp()
		{
			Noise3D.LoadIfNeeded();
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001A040 File Offset: 0x00018240
		public static void LoadIfNeeded()
		{
			if (!Noise3D.isSupported)
			{
				return;
			}
			if (Noise3D.ms_NoiseTexture == null)
			{
				Noise3D.ms_NoiseTexture = Config.Instance.noiseTexture3D;
				Shader.SetGlobalTexture(ShaderProperties.GlobalNoiseTex3D, Noise3D.ms_NoiseTexture);
				Shader.SetGlobalFloat(ShaderProperties.GlobalNoiseCustomTime, -1f);
			}
		}

		// Token: 0x040006A1 RID: 1697
		private static bool ms_IsSupportedChecked;

		// Token: 0x040006A2 RID: 1698
		private static bool ms_IsSupported;

		// Token: 0x040006A3 RID: 1699
		private static Texture3D ms_NoiseTexture;

		// Token: 0x040006A4 RID: 1700
		private const int kMinShaderLevel = 35;
	}
}
