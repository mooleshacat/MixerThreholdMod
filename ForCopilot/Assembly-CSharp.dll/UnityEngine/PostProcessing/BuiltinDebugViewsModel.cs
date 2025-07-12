using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000AC RID: 172
	[Serializable]
	public class BuiltinDebugViewsModel : PostProcessingModel
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600032A RID: 810 RVA: 0x000133BA File Offset: 0x000115BA
		// (set) Token: 0x0600032B RID: 811 RVA: 0x000133C2 File Offset: 0x000115C2
		public BuiltinDebugViewsModel.Settings settings
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600032C RID: 812 RVA: 0x000133CB File Offset: 0x000115CB
		public bool willInterrupt
		{
			get
			{
				return !this.IsModeActive(BuiltinDebugViewsModel.Mode.None) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut);
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000133FE File Offset: 0x000115FE
		public override void Reset()
		{
			this.settings = BuiltinDebugViewsModel.Settings.defaultSettings;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0001340B File Offset: 0x0001160B
		public bool IsModeActive(BuiltinDebugViewsModel.Mode mode)
		{
			return this.m_Settings.mode == mode;
		}

		// Token: 0x040003BE RID: 958
		[SerializeField]
		private BuiltinDebugViewsModel.Settings m_Settings = BuiltinDebugViewsModel.Settings.defaultSettings;

		// Token: 0x020000AD RID: 173
		[Serializable]
		public struct DepthSettings
		{
			// Token: 0x1700005B RID: 91
			// (get) Token: 0x06000330 RID: 816 RVA: 0x00013430 File Offset: 0x00011630
			public static BuiltinDebugViewsModel.DepthSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.DepthSettings
					{
						scale = 1f
					};
				}
			}

			// Token: 0x040003BF RID: 959
			[Range(0f, 1f)]
			[Tooltip("Scales the camera far plane before displaying the depth map.")]
			public float scale;
		}

		// Token: 0x020000AE RID: 174
		[Serializable]
		public struct MotionVectorsSettings
		{
			// Token: 0x1700005C RID: 92
			// (get) Token: 0x06000331 RID: 817 RVA: 0x00013454 File Offset: 0x00011654
			public static BuiltinDebugViewsModel.MotionVectorsSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.MotionVectorsSettings
					{
						sourceOpacity = 1f,
						motionImageOpacity = 0f,
						motionImageAmplitude = 16f,
						motionVectorsOpacity = 1f,
						motionVectorsResolution = 24,
						motionVectorsAmplitude = 64f
					};
				}
			}

			// Token: 0x040003C0 RID: 960
			[Range(0f, 1f)]
			[Tooltip("Opacity of the source render.")]
			public float sourceOpacity;

			// Token: 0x040003C1 RID: 961
			[Range(0f, 1f)]
			[Tooltip("Opacity of the per-pixel motion vector colors.")]
			public float motionImageOpacity;

			// Token: 0x040003C2 RID: 962
			[Min(0f)]
			[Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
			public float motionImageAmplitude;

			// Token: 0x040003C3 RID: 963
			[Range(0f, 1f)]
			[Tooltip("Opacity for the motion vector arrows.")]
			public float motionVectorsOpacity;

			// Token: 0x040003C4 RID: 964
			[Range(8f, 64f)]
			[Tooltip("The arrow density on screen.")]
			public int motionVectorsResolution;

			// Token: 0x040003C5 RID: 965
			[Min(0f)]
			[Tooltip("Tweaks the arrows length.")]
			public float motionVectorsAmplitude;
		}

		// Token: 0x020000AF RID: 175
		public enum Mode
		{
			// Token: 0x040003C7 RID: 967
			None,
			// Token: 0x040003C8 RID: 968
			Depth,
			// Token: 0x040003C9 RID: 969
			Normals,
			// Token: 0x040003CA RID: 970
			MotionVectors,
			// Token: 0x040003CB RID: 971
			AmbientOcclusion,
			// Token: 0x040003CC RID: 972
			EyeAdaptation,
			// Token: 0x040003CD RID: 973
			FocusPlane,
			// Token: 0x040003CE RID: 974
			PreGradingLog,
			// Token: 0x040003CF RID: 975
			LogLut,
			// Token: 0x040003D0 RID: 976
			UserLut
		}

		// Token: 0x020000B0 RID: 176
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700005D RID: 93
			// (get) Token: 0x06000332 RID: 818 RVA: 0x000134B0 File Offset: 0x000116B0
			public static BuiltinDebugViewsModel.Settings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.Settings
					{
						mode = BuiltinDebugViewsModel.Mode.None,
						depth = BuiltinDebugViewsModel.DepthSettings.defaultSettings,
						motionVectors = BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings
					};
				}
			}

			// Token: 0x040003D1 RID: 977
			public BuiltinDebugViewsModel.Mode mode;

			// Token: 0x040003D2 RID: 978
			public BuiltinDebugViewsModel.DepthSettings depth;

			// Token: 0x040003D3 RID: 979
			public BuiltinDebugViewsModel.MotionVectorsSettings motionVectors;
		}
	}
}
