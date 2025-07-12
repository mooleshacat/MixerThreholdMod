using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000B3 RID: 179
	[Serializable]
	public class ColorGradingModel : PostProcessingModel
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00013542 File Offset: 0x00011742
		// (set) Token: 0x06000339 RID: 825 RVA: 0x0001354A File Offset: 0x0001174A
		public ColorGradingModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
				this.OnValidate();
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00013559 File Offset: 0x00011759
		// (set) Token: 0x0600033B RID: 827 RVA: 0x00013561 File Offset: 0x00011761
		public bool isDirty { get; internal set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0001356A File Offset: 0x0001176A
		// (set) Token: 0x0600033D RID: 829 RVA: 0x00013572 File Offset: 0x00011772
		public RenderTexture bakedLut { get; internal set; }

		// Token: 0x0600033E RID: 830 RVA: 0x0001357B File Offset: 0x0001177B
		public override void Reset()
		{
			this.m_Settings = ColorGradingModel.Settings.defaultSettings;
			this.OnValidate();
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0001358E File Offset: 0x0001178E
		public override void OnValidate()
		{
			this.isDirty = true;
		}

		// Token: 0x040003D7 RID: 983
		[SerializeField]
		private ColorGradingModel.Settings m_Settings = ColorGradingModel.Settings.defaultSettings;

		// Token: 0x020000B4 RID: 180
		public enum Tonemapper
		{
			// Token: 0x040003DB RID: 987
			None,
			// Token: 0x040003DC RID: 988
			ACES,
			// Token: 0x040003DD RID: 989
			Neutral
		}

		// Token: 0x020000B5 RID: 181
		[Serializable]
		public struct TonemappingSettings
		{
			// Token: 0x17000063 RID: 99
			// (get) Token: 0x06000341 RID: 833 RVA: 0x000135AC File Offset: 0x000117AC
			public static ColorGradingModel.TonemappingSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.TonemappingSettings
					{
						tonemapper = ColorGradingModel.Tonemapper.Neutral,
						neutralBlackIn = 0.02f,
						neutralWhiteIn = 10f,
						neutralBlackOut = 0f,
						neutralWhiteOut = 10f,
						neutralWhiteLevel = 5.3f,
						neutralWhiteClip = 10f
					};
				}
			}

			// Token: 0x040003DE RID: 990
			[Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
			public ColorGradingModel.Tonemapper tonemapper;

			// Token: 0x040003DF RID: 991
			[Range(-0.1f, 0.1f)]
			public float neutralBlackIn;

			// Token: 0x040003E0 RID: 992
			[Range(1f, 20f)]
			public float neutralWhiteIn;

			// Token: 0x040003E1 RID: 993
			[Range(-0.09f, 0.1f)]
			public float neutralBlackOut;

			// Token: 0x040003E2 RID: 994
			[Range(1f, 19f)]
			public float neutralWhiteOut;

			// Token: 0x040003E3 RID: 995
			[Range(0.1f, 20f)]
			public float neutralWhiteLevel;

			// Token: 0x040003E4 RID: 996
			[Range(1f, 10f)]
			public float neutralWhiteClip;
		}

		// Token: 0x020000B6 RID: 182
		[Serializable]
		public struct BasicSettings
		{
			// Token: 0x17000064 RID: 100
			// (get) Token: 0x06000342 RID: 834 RVA: 0x00013614 File Offset: 0x00011814
			public static ColorGradingModel.BasicSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.BasicSettings
					{
						postExposure = 0f,
						temperature = 0f,
						tint = 0f,
						hueShift = 0f,
						saturation = 1f,
						contrast = 1f
					};
				}
			}

			// Token: 0x040003E5 RID: 997
			[Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
			public float postExposure;

			// Token: 0x040003E6 RID: 998
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to a custom color temperature.")]
			public float temperature;

			// Token: 0x040003E7 RID: 999
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
			public float tint;

			// Token: 0x040003E8 RID: 1000
			[Range(-180f, 180f)]
			[Tooltip("Shift the hue of all colors.")]
			public float hueShift;

			// Token: 0x040003E9 RID: 1001
			[Range(0f, 2f)]
			[Tooltip("Pushes the intensity of all colors.")]
			public float saturation;

			// Token: 0x040003EA RID: 1002
			[Range(0f, 2f)]
			[Tooltip("Expands or shrinks the overall range of tonal values.")]
			public float contrast;
		}

		// Token: 0x020000B7 RID: 183
		[Serializable]
		public struct ChannelMixerSettings
		{
			// Token: 0x17000065 RID: 101
			// (get) Token: 0x06000343 RID: 835 RVA: 0x00013674 File Offset: 0x00011874
			public static ColorGradingModel.ChannelMixerSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ChannelMixerSettings
					{
						red = new Vector3(1f, 0f, 0f),
						green = new Vector3(0f, 1f, 0f),
						blue = new Vector3(0f, 0f, 1f),
						currentEditingChannel = 0
					};
				}
			}

			// Token: 0x040003EB RID: 1003
			public Vector3 red;

			// Token: 0x040003EC RID: 1004
			public Vector3 green;

			// Token: 0x040003ED RID: 1005
			public Vector3 blue;

			// Token: 0x040003EE RID: 1006
			[HideInInspector]
			public int currentEditingChannel;
		}

		// Token: 0x020000B8 RID: 184
		[Serializable]
		public struct LogWheelsSettings
		{
			// Token: 0x17000066 RID: 102
			// (get) Token: 0x06000344 RID: 836 RVA: 0x000136E4 File Offset: 0x000118E4
			public static ColorGradingModel.LogWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LogWheelsSettings
					{
						slope = Color.clear,
						power = Color.clear,
						offset = Color.clear
					};
				}
			}

			// Token: 0x040003EF RID: 1007
			[Trackball("GetSlopeValue")]
			public Color slope;

			// Token: 0x040003F0 RID: 1008
			[Trackball("GetPowerValue")]
			public Color power;

			// Token: 0x040003F1 RID: 1009
			[Trackball("GetOffsetValue")]
			public Color offset;
		}

		// Token: 0x020000B9 RID: 185
		[Serializable]
		public struct LinearWheelsSettings
		{
			// Token: 0x17000067 RID: 103
			// (get) Token: 0x06000345 RID: 837 RVA: 0x00013720 File Offset: 0x00011920
			public static ColorGradingModel.LinearWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LinearWheelsSettings
					{
						lift = Color.clear,
						gamma = Color.clear,
						gain = Color.clear
					};
				}
			}

			// Token: 0x040003F2 RID: 1010
			[Trackball("GetLiftValue")]
			public Color lift;

			// Token: 0x040003F3 RID: 1011
			[Trackball("GetGammaValue")]
			public Color gamma;

			// Token: 0x040003F4 RID: 1012
			[Trackball("GetGainValue")]
			public Color gain;
		}

		// Token: 0x020000BA RID: 186
		public enum ColorWheelMode
		{
			// Token: 0x040003F6 RID: 1014
			Linear,
			// Token: 0x040003F7 RID: 1015
			Log
		}

		// Token: 0x020000BB RID: 187
		[Serializable]
		public struct ColorWheelsSettings
		{
			// Token: 0x17000068 RID: 104
			// (get) Token: 0x06000346 RID: 838 RVA: 0x0001375C File Offset: 0x0001195C
			public static ColorGradingModel.ColorWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ColorWheelsSettings
					{
						mode = ColorGradingModel.ColorWheelMode.Log,
						log = ColorGradingModel.LogWheelsSettings.defaultSettings,
						linear = ColorGradingModel.LinearWheelsSettings.defaultSettings
					};
				}
			}

			// Token: 0x040003F8 RID: 1016
			public ColorGradingModel.ColorWheelMode mode;

			// Token: 0x040003F9 RID: 1017
			[TrackballGroup]
			public ColorGradingModel.LogWheelsSettings log;

			// Token: 0x040003FA RID: 1018
			[TrackballGroup]
			public ColorGradingModel.LinearWheelsSettings linear;
		}

		// Token: 0x020000BC RID: 188
		[Serializable]
		public struct CurvesSettings
		{
			// Token: 0x17000069 RID: 105
			// (get) Token: 0x06000347 RID: 839 RVA: 0x00013794 File Offset: 0x00011994
			public static ColorGradingModel.CurvesSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.CurvesSettings
					{
						master = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						red = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						green = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						blue = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						e_CurrentEditingCurve = 0,
						e_CurveY = true,
						e_CurveR = false,
						e_CurveG = false,
						e_CurveB = false
					};
				}
			}

			// Token: 0x040003FB RID: 1019
			public ColorGradingCurve master;

			// Token: 0x040003FC RID: 1020
			public ColorGradingCurve red;

			// Token: 0x040003FD RID: 1021
			public ColorGradingCurve green;

			// Token: 0x040003FE RID: 1022
			public ColorGradingCurve blue;

			// Token: 0x040003FF RID: 1023
			public ColorGradingCurve hueVShue;

			// Token: 0x04000400 RID: 1024
			public ColorGradingCurve hueVSsat;

			// Token: 0x04000401 RID: 1025
			public ColorGradingCurve satVSsat;

			// Token: 0x04000402 RID: 1026
			public ColorGradingCurve lumVSsat;

			// Token: 0x04000403 RID: 1027
			[HideInInspector]
			public int e_CurrentEditingCurve;

			// Token: 0x04000404 RID: 1028
			[HideInInspector]
			public bool e_CurveY;

			// Token: 0x04000405 RID: 1029
			[HideInInspector]
			public bool e_CurveR;

			// Token: 0x04000406 RID: 1030
			[HideInInspector]
			public bool e_CurveG;

			// Token: 0x04000407 RID: 1031
			[HideInInspector]
			public bool e_CurveB;
		}

		// Token: 0x020000BD RID: 189
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700006A RID: 106
			// (get) Token: 0x06000348 RID: 840 RVA: 0x00013A1C File Offset: 0x00011C1C
			public static ColorGradingModel.Settings defaultSettings
			{
				get
				{
					return new ColorGradingModel.Settings
					{
						tonemapping = ColorGradingModel.TonemappingSettings.defaultSettings,
						basic = ColorGradingModel.BasicSettings.defaultSettings,
						channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings,
						colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings,
						curves = ColorGradingModel.CurvesSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000408 RID: 1032
			public ColorGradingModel.TonemappingSettings tonemapping;

			// Token: 0x04000409 RID: 1033
			public ColorGradingModel.BasicSettings basic;

			// Token: 0x0400040A RID: 1034
			public ColorGradingModel.ChannelMixerSettings channelMixer;

			// Token: 0x0400040B RID: 1035
			public ColorGradingModel.ColorWheelsSettings colorWheels;

			// Token: 0x0400040C RID: 1036
			public ColorGradingModel.CurvesSettings curves;
		}
	}
}
