using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000BE RID: 190
	[Serializable]
	public class DepthOfFieldModel : PostProcessingModel
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00013A6E File Offset: 0x00011C6E
		// (set) Token: 0x0600034A RID: 842 RVA: 0x00013A76 File Offset: 0x00011C76
		public DepthOfFieldModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00013A7F File Offset: 0x00011C7F
		public override void Reset()
		{
			this.m_Settings = DepthOfFieldModel.Settings.defaultSettings;
		}

		// Token: 0x0400040D RID: 1037
		[SerializeField]
		private DepthOfFieldModel.Settings m_Settings = DepthOfFieldModel.Settings.defaultSettings;

		// Token: 0x020000BF RID: 191
		public enum KernelSize
		{
			// Token: 0x0400040F RID: 1039
			Small,
			// Token: 0x04000410 RID: 1040
			Medium,
			// Token: 0x04000411 RID: 1041
			Large,
			// Token: 0x04000412 RID: 1042
			VeryLarge
		}

		// Token: 0x020000C0 RID: 192
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700006C RID: 108
			// (get) Token: 0x0600034D RID: 845 RVA: 0x00013AA0 File Offset: 0x00011CA0
			public static DepthOfFieldModel.Settings defaultSettings
			{
				get
				{
					return new DepthOfFieldModel.Settings
					{
						focusDistance = 10f,
						aperture = 5.6f,
						focalLength = 50f,
						useCameraFov = false,
						kernelSize = DepthOfFieldModel.KernelSize.Medium
					};
				}
			}

			// Token: 0x04000413 RID: 1043
			[Min(0.1f)]
			[Tooltip("Distance to the point of focus.")]
			public float focusDistance;

			// Token: 0x04000414 RID: 1044
			[Range(0.05f, 32f)]
			[Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
			public float aperture;

			// Token: 0x04000415 RID: 1045
			[Range(1f, 300f)]
			[Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
			public float focalLength;

			// Token: 0x04000416 RID: 1046
			[Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera. Using this setting isn't recommended.")]
			public bool useCameraFov;

			// Token: 0x04000417 RID: 1047
			[Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")]
			public DepthOfFieldModel.KernelSize kernelSize;
		}
	}
}
