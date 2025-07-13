using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000CA RID: 202
	[Serializable]
	public class MotionBlurModel : PostProcessingModel
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00013CB2 File Offset: 0x00011EB2
		// (set) Token: 0x06000363 RID: 867 RVA: 0x00013CBA File Offset: 0x00011EBA
		public MotionBlurModel.Settings settings
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

		// Token: 0x06000364 RID: 868 RVA: 0x00013CC3 File Offset: 0x00011EC3
		public override void Reset()
		{
			this.m_Settings = MotionBlurModel.Settings.defaultSettings;
		}

		// Token: 0x0400042F RID: 1071
		[SerializeField]
		private MotionBlurModel.Settings m_Settings = MotionBlurModel.Settings.defaultSettings;

		// Token: 0x020000CB RID: 203
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000076 RID: 118
			// (get) Token: 0x06000366 RID: 870 RVA: 0x00013CE4 File Offset: 0x00011EE4
			public static MotionBlurModel.Settings defaultSettings
			{
				get
				{
					return new MotionBlurModel.Settings
					{
						shutterAngle = 270f,
						sampleCount = 10,
						frameBlending = 0f
					};
				}
			}

			// Token: 0x04000430 RID: 1072
			[Range(0f, 360f)]
			[Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
			public float shutterAngle;

			// Token: 0x04000431 RID: 1073
			[Range(4f, 32f)]
			[Tooltip("The amount of sample points, which affects quality and performances.")]
			public int sampleCount;

			// Token: 0x04000432 RID: 1074
			[Range(0f, 1f)]
			[Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
			public float frameBlending;
		}
	}
}
