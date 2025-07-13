using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200011B RID: 283
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[SelectionBase]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-hd/")]
	public class VolumetricLightBeamHD : VolumetricLightBeamAbstractBase
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00017C1F File Offset: 0x00015E1F
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x00017C27 File Offset: 0x00015E27
		public bool colorFromLight
		{
			get
			{
				return this.m_ColorFromLight;
			}
			set
			{
				if (this.m_ColorFromLight != value)
				{
					this.m_ColorFromLight = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00017C3F File Offset: 0x00015E3F
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x00017C55 File Offset: 0x00015E55
		public ColorMode colorMode
		{
			get
			{
				if (Config.Instance.featureEnabledColorGradient == FeatureEnabledColorGradient.Off)
				{
					return ColorMode.Flat;
				}
				return this.m_ColorMode;
			}
			set
			{
				if (this.m_ColorMode != value)
				{
					this.m_ColorMode = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.ColorMode);
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00017C74 File Offset: 0x00015E74
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x00017C7C File Offset: 0x00015E7C
		public Color colorFlat
		{
			get
			{
				return this.m_ColorFlat;
			}
			set
			{
				if (this.m_ColorFlat != value)
				{
					this.m_ColorFlat = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Color);
				}
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x00017CA1 File Offset: 0x00015EA1
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x00017CA9 File Offset: 0x00015EA9
		public Gradient colorGradient
		{
			get
			{
				return this.m_ColorGradient;
			}
			set
			{
				if (this.m_ColorGradient != value)
				{
					this.m_ColorGradient = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Color);
				}
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00017CC9 File Offset: 0x00015EC9
		private bool useColorFromAttachedLightSpot
		{
			get
			{
				return this.colorFromLight && base.lightSpotAttached != null;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00017CE1 File Offset: 0x00015EE1
		private bool useColorTemperatureFromAttachedLightSpot
		{
			get
			{
				return this.useColorFromAttachedLightSpot && base.lightSpotAttached.useColorTemperature && Config.Instance.useLightColorTemperature;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00017D04 File Offset: 0x00015F04
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x00017D0C File Offset: 0x00015F0C
		public float intensity
		{
			get
			{
				return this.m_Intensity;
			}
			set
			{
				if (this.m_Intensity != value)
				{
					this.m_Intensity = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Intensity);
				}
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x00017D2B File Offset: 0x00015F2B
		// (set) Token: 0x0600048D RID: 1165 RVA: 0x00017D33 File Offset: 0x00015F33
		public float intensityMultiplier
		{
			get
			{
				return this.m_IntensityMultiplier;
			}
			set
			{
				if (this.m_IntensityMultiplier != value)
				{
					this.m_IntensityMultiplier = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x00017D4B File Offset: 0x00015F4B
		public bool useIntensityFromAttachedLightSpot
		{
			get
			{
				return this.intensityMultiplier >= 0f && base.lightSpotAttached != null;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00017D68 File Offset: 0x00015F68
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x00017D70 File Offset: 0x00015F70
		public float hdrpExposureWeight
		{
			get
			{
				return this.m_HDRPExposureWeight;
			}
			set
			{
				if (this.m_HDRPExposureWeight != value)
				{
					this.m_HDRPExposureWeight = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.HDRPExposureWeight);
				}
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x00017D8F File Offset: 0x00015F8F
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x00017D97 File Offset: 0x00015F97
		public BlendingMode blendingMode
		{
			get
			{
				return this.m_BlendingMode;
			}
			set
			{
				if (this.m_BlendingMode != value)
				{
					this.m_BlendingMode = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.BlendingMode);
				}
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x00017DB7 File Offset: 0x00015FB7
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x00017DBF File Offset: 0x00015FBF
		public float spotAngle
		{
			get
			{
				return this.m_SpotAngle;
			}
			set
			{
				if (this.m_SpotAngle != value)
				{
					this.m_SpotAngle = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00017DDF File Offset: 0x00015FDF
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x00017DE7 File Offset: 0x00015FE7
		public float spotAngleMultiplier
		{
			get
			{
				return this.m_SpotAngleMultiplier;
			}
			set
			{
				if (this.m_SpotAngleMultiplier != value)
				{
					this.m_SpotAngleMultiplier = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x00017DFF File Offset: 0x00015FFF
		public bool useSpotAngleFromAttachedLightSpot
		{
			get
			{
				return this.spotAngleMultiplier >= 0f && base.lightSpotAttached != null;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00017E1C File Offset: 0x0001601C
		public float coneAngle
		{
			get
			{
				return Mathf.Atan2(this.coneRadiusEnd - this.coneRadiusStart, this.maxGeometryDistance) * 57.29578f * 2f;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x00017E42 File Offset: 0x00016042
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x00017E4A File Offset: 0x0001604A
		public float coneRadiusStart
		{
			get
			{
				return this.m_ConeRadiusStart;
			}
			set
			{
				if (this.m_ConeRadiusStart != value)
				{
					this.m_ConeRadiusStart = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x00017E6A File Offset: 0x0001606A
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x00017E7D File Offset: 0x0001607D
		public float coneRadiusEnd
		{
			get
			{
				return Utils.ComputeConeRadiusEnd(this.maxGeometryDistance, this.spotAngle);
			}
			set
			{
				this.spotAngle = Utils.ComputeSpotAngle(this.maxGeometryDistance, value);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x00017E94 File Offset: 0x00016094
		public float coneVolume
		{
			get
			{
				float coneRadiusStart = this.coneRadiusStart;
				float coneRadiusEnd = this.coneRadiusEnd;
				return 1.0471976f * (coneRadiusStart * coneRadiusStart + coneRadiusStart * coneRadiusEnd + coneRadiusEnd * coneRadiusEnd) * this.fallOffEnd;
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00017EC8 File Offset: 0x000160C8
		public float GetConeApexOffsetZ(bool counterApplyScaleForUnscalableBeam)
		{
			float num = this.coneRadiusStart / this.coneRadiusEnd;
			if (num == 1f)
			{
				return float.MaxValue;
			}
			float num2 = this.maxGeometryDistance * num / (1f - num);
			if (counterApplyScaleForUnscalableBeam && !this.scalable)
			{
				num2 /= this.GetLossyScale().z;
			}
			return num2;
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00017F1C File Offset: 0x0001611C
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x00017F24 File Offset: 0x00016124
		public bool scalable
		{
			get
			{
				return this.m_Scalable;
			}
			set
			{
				if (this.m_Scalable != value)
				{
					this.m_Scalable = value;
					this.SetPropertyDirty(DirtyProps.Attenuation);
				}
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00017F41 File Offset: 0x00016141
		public override bool IsScalable()
		{
			return this.scalable;
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00017F49 File Offset: 0x00016149
		// (set) Token: 0x060004A3 RID: 1187 RVA: 0x00017F51 File Offset: 0x00016151
		public AttenuationEquationHD attenuationEquation
		{
			get
			{
				return this.m_AttenuationEquation;
			}
			set
			{
				if (this.m_AttenuationEquation != value)
				{
					this.m_AttenuationEquation = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Attenuation);
				}
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x00017F74 File Offset: 0x00016174
		// (set) Token: 0x060004A5 RID: 1189 RVA: 0x00017F7C File Offset: 0x0001617C
		public float fallOffStart
		{
			get
			{
				return this.m_FallOffStart;
			}
			set
			{
				if (this.m_FallOffStart != value)
				{
					this.m_FallOffStart = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x00017F9C File Offset: 0x0001619C
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x00017FA4 File Offset: 0x000161A4
		public float fallOffEnd
		{
			get
			{
				return this.m_FallOffEnd;
			}
			set
			{
				if (this.m_FallOffEnd != value)
				{
					this.m_FallOffEnd = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00017FC4 File Offset: 0x000161C4
		public float maxGeometryDistance
		{
			get
			{
				return this.fallOffEnd;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00017FCC File Offset: 0x000161CC
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x00017FD4 File Offset: 0x000161D4
		public float fallOffEndMultiplier
		{
			get
			{
				return this.m_FallOffEndMultiplier;
			}
			set
			{
				if (this.m_FallOffEndMultiplier != value)
				{
					this.m_FallOffEndMultiplier = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x00017FEC File Offset: 0x000161EC
		public bool useFallOffEndFromAttachedLightSpot
		{
			get
			{
				return this.fallOffEndMultiplier >= 0f && base.lightSpotAttached != null;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00018009 File Offset: 0x00016209
		// (set) Token: 0x060004AD RID: 1197 RVA: 0x00018011 File Offset: 0x00016211
		public float sideSoftness
		{
			get
			{
				return this.m_SideSoftness;
			}
			set
			{
				if (this.m_SideSoftness != value)
				{
					this.m_SideSoftness = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.SideSoftness);
				}
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00018034 File Offset: 0x00016234
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x0001803C File Offset: 0x0001623C
		public float jitteringFactor
		{
			get
			{
				return this.m_JitteringFactor;
			}
			set
			{
				if (this.m_JitteringFactor != value)
				{
					this.m_JitteringFactor = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Jittering);
				}
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0001805F File Offset: 0x0001625F
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x00018067 File Offset: 0x00016267
		public int jitteringFrameRate
		{
			get
			{
				return this.m_JitteringFrameRate;
			}
			set
			{
				if (this.m_JitteringFrameRate != value)
				{
					this.m_JitteringFrameRate = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Jittering);
				}
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0001808A File Offset: 0x0001628A
		// (set) Token: 0x060004B3 RID: 1203 RVA: 0x00018092 File Offset: 0x00016292
		public MinMaxRangeFloat jitteringLerpRange
		{
			get
			{
				return this.m_JitteringLerpRange;
			}
			set
			{
				if (this.m_JitteringLerpRange != value)
				{
					this.m_JitteringLerpRange = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Jittering);
				}
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x000180BA File Offset: 0x000162BA
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x000180C2 File Offset: 0x000162C2
		public NoiseMode noiseMode
		{
			get
			{
				return this.m_NoiseMode;
			}
			set
			{
				if (this.m_NoiseMode != value)
				{
					this.m_NoiseMode = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseMode);
				}
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x000180E5 File Offset: 0x000162E5
		public bool isNoiseEnabled
		{
			get
			{
				return this.noiseMode > NoiseMode.Disabled;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x000180F0 File Offset: 0x000162F0
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x000180F8 File Offset: 0x000162F8
		public float noiseIntensity
		{
			get
			{
				return this.m_NoiseIntensity;
			}
			set
			{
				if (this.m_NoiseIntensity != value)
				{
					this.m_NoiseIntensity = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseIntensity);
				}
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0001811B File Offset: 0x0001631B
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x00018123 File Offset: 0x00016323
		public bool noiseScaleUseGlobal
		{
			get
			{
				return this.m_NoiseScaleUseGlobal;
			}
			set
			{
				if (this.m_NoiseScaleUseGlobal != value)
				{
					this.m_NoiseScaleUseGlobal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00018146 File Offset: 0x00016346
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x0001814E File Offset: 0x0001634E
		public float noiseScaleLocal
		{
			get
			{
				return this.m_NoiseScaleLocal;
			}
			set
			{
				if (this.m_NoiseScaleLocal != value)
				{
					this.m_NoiseScaleLocal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00018171 File Offset: 0x00016371
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x00018179 File Offset: 0x00016379
		public bool noiseVelocityUseGlobal
		{
			get
			{
				return this.m_NoiseVelocityUseGlobal;
			}
			set
			{
				if (this.m_NoiseVelocityUseGlobal != value)
				{
					this.m_NoiseVelocityUseGlobal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0001819C File Offset: 0x0001639C
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x000181A4 File Offset: 0x000163A4
		public Vector3 noiseVelocityLocal
		{
			get
			{
				return this.m_NoiseVelocityLocal;
			}
			set
			{
				if (this.m_NoiseVelocityLocal != value)
				{
					this.m_NoiseVelocityLocal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x000181CC File Offset: 0x000163CC
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x000181D4 File Offset: 0x000163D4
		public int raymarchingQualityID
		{
			get
			{
				return this.m_RaymarchingQualityID;
			}
			set
			{
				if (this.m_RaymarchingQualityID != value)
				{
					this.m_RaymarchingQualityID = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.RaymarchingQuality);
				}
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x000181F7 File Offset: 0x000163F7
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x00018209 File Offset: 0x00016409
		public int raymarchingQualityIndex
		{
			get
			{
				return Config.Instance.GetRaymarchingQualityIndexForUniqueID(this.raymarchingQualityID);
			}
			set
			{
				this.raymarchingQualityID = Config.Instance.GetRaymarchingQualityForIndex(this.raymarchingQualityIndex).uniqueID;
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00018226 File Offset: 0x00016426
		public override BeamGeometryAbstractBase GetBeamGeometry()
		{
			return this.m_BeamGeom;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001822E File Offset: 0x0001642E
		protected override void SetBeamGeometryNull()
		{
			this.m_BeamGeom = null;
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00018237 File Offset: 0x00016437
		public int blendingModeAsInt
		{
			get
			{
				return Mathf.Clamp((int)this.blendingMode, 0, Enum.GetValues(typeof(BlendingMode)).Length);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00018259 File Offset: 0x00016459
		public Quaternion beamInternalLocalRotation
		{
			get
			{
				if (this.GetDimensions() != Dimensions.Dim3D)
				{
					return Quaternion.LookRotation(Vector3.right, Vector3.up);
				}
				return Quaternion.identity;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00018278 File Offset: 0x00016478
		public Vector3 beamLocalForward
		{
			get
			{
				if (this.GetDimensions() != Dimensions.Dim3D)
				{
					return Vector3.right;
				}
				return Vector3.forward;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x0001828D File Offset: 0x0001648D
		public Vector3 beamGlobalForward
		{
			get
			{
				return base.transform.TransformDirection(this.beamLocalForward);
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000182A0 File Offset: 0x000164A0
		public override Vector3 GetLossyScale()
		{
			if (this.GetDimensions() != Dimensions.Dim3D)
			{
				return new Vector3(base.transform.lossyScale.z, base.transform.lossyScale.y, base.transform.lossyScale.x);
			}
			return base.transform.lossyScale;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000182F6 File Offset: 0x000164F6
		public VolumetricCookieHD GetAdditionalComponentCookie()
		{
			return base.GetComponent<VolumetricCookieHD>();
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x000182FE File Offset: 0x000164FE
		public VolumetricShadowHD GetAdditionalComponentShadow()
		{
			return base.GetComponent<VolumetricShadowHD>();
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00018306 File Offset: 0x00016506
		public void SetPropertyDirty(DirtyProps flags)
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.SetPropertyDirty(flags);
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00014B5A File Offset: 0x00012D5A
		public virtual Dimensions GetDimensions()
		{
			return Dimensions.Dim3D;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00014B5A File Offset: 0x00012D5A
		public virtual bool DoesSupportSorting2D()
		{
			return false;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00014B5A File Offset: 0x00012D5A
		public virtual int GetSortingLayerID()
		{
			return 0;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00014B5A File Offset: 0x00012D5A
		public virtual int GetSortingOrder()
		{
			return 0;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00018321 File Offset: 0x00016521
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00018329 File Offset: 0x00016529
		public uint _INTERNAL_InstancedMaterialGroupID { get; protected set; }

		// Token: 0x060004D5 RID: 1237 RVA: 0x00018332 File Offset: 0x00016532
		public float GetInsideBeamFactor(Vector3 posWS)
		{
			return this.GetInsideBeamFactorFromObjectSpacePos(base.transform.InverseTransformPoint(posWS));
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00018348 File Offset: 0x00016548
		public float GetInsideBeamFactorFromObjectSpacePos(Vector3 posOS)
		{
			if (this.GetDimensions() == Dimensions.Dim2D)
			{
				posOS = new Vector3(posOS.z, posOS.y, posOS.x);
			}
			if (posOS.z < 0f)
			{
				return -1f;
			}
			Vector2 normalized = new Vector2(posOS.xy().magnitude, posOS.z + this.GetConeApexOffsetZ(true)).normalized;
			return Mathf.Clamp((Mathf.Abs(Mathf.Sin(this.coneAngle * 0.017453292f / 2f)) - Mathf.Abs(normalized.x)) / 0.1f, -1f, 1f);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x000183F4 File Offset: 0x000165F4
		public virtual void GenerateGeometry()
		{
			if (this.pluginVersion == -1)
			{
				this.raymarchingQualityID = Config.Instance.defaultRaymarchingQualityUniqueID;
			}
			if (!Config.Instance.IsRaymarchingQualityUniqueIDValid(this.raymarchingQualityID))
			{
				Debug.LogErrorFormat(base.gameObject, "HD Beam '{0}': fallback to default quality '{1}'", new object[]
				{
					base.name,
					Config.Instance.GetRaymarchingQualityForUniqueID(Config.Instance.defaultRaymarchingQualityUniqueID).name
				});
				this.raymarchingQualityID = Config.Instance.defaultRaymarchingQualityUniqueID;
				Utils.MarkCurrentSceneDirty();
			}
			this.HandleBackwardCompatibility(this.pluginVersion, 20100);
			this.pluginVersion = 20100;
			this.ValidateProperties();
			if (this.m_BeamGeom == null)
			{
				this.m_BeamGeom = Utils.NewWithComponent<BeamGeometryHD>("Beam Geometry");
				this.m_BeamGeom.Initialize(this);
			}
			this.m_BeamGeom.RegenerateMesh();
			this.m_BeamGeom.visible = base.enabled;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x000184E4 File Offset: 0x000166E4
		public virtual void UpdateAfterManualPropertyChange()
		{
			this.ValidateProperties();
			this.SetPropertyDirty(DirtyProps.All);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000184F7 File Offset: 0x000166F7
		private void Start()
		{
			base.InitLightSpotAttachedCached();
			this.GenerateGeometry();
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00018505 File Offset: 0x00016705
		private void OnEnable()
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.visible = true;
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00018520 File Offset: 0x00016720
		private void OnDisable()
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.visible = false;
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001853B File Offset: 0x0001673B
		private void OnDidApplyAnimationProperties()
		{
			this.AssignPropertiesFromAttachedSpotLight();
			this.UpdateAfterManualPropertyChange();
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001854C File Offset: 0x0001674C
		public void AssignPropertiesFromAttachedSpotLight()
		{
			Light lightSpotAttached = base.lightSpotAttached;
			if (lightSpotAttached)
			{
				if (this.useIntensityFromAttachedLightSpot)
				{
					this.intensity = SpotLightHelper.GetIntensity(lightSpotAttached) * this.intensityMultiplier;
				}
				if (this.useFallOffEndFromAttachedLightSpot)
				{
					this.fallOffEnd = SpotLightHelper.GetFallOffEnd(lightSpotAttached) * this.fallOffEndMultiplier;
				}
				if (this.useSpotAngleFromAttachedLightSpot)
				{
					this.spotAngle = Mathf.Clamp(SpotLightHelper.GetSpotAngle(lightSpotAttached) * this.spotAngleMultiplier, 0.1f, 179.9f);
				}
				if (this.m_ColorFromLight)
				{
					this.colorMode = ColorMode.Flat;
					if (this.useColorTemperatureFromAttachedLightSpot)
					{
						Color b = Mathf.CorrelatedColorTemperatureToRGB(lightSpotAttached.colorTemperature);
						this.colorFlat = (lightSpotAttached.color.linear * b).gamma;
						return;
					}
					this.colorFlat = lightSpotAttached.color;
				}
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00018620 File Offset: 0x00016820
		private void ClampProperties()
		{
			this.m_Intensity = Mathf.Max(this.m_Intensity, 0f);
			this.m_FallOffEnd = Mathf.Max(0.01f, this.m_FallOffEnd);
			this.m_FallOffStart = Mathf.Clamp(this.m_FallOffStart, 0f, this.m_FallOffEnd - 0.01f);
			this.m_SpotAngle = Mathf.Clamp(this.m_SpotAngle, 0.1f, 179.9f);
			this.m_ConeRadiusStart = Mathf.Max(this.m_ConeRadiusStart, 0f);
			this.m_SideSoftness = Mathf.Clamp(this.m_SideSoftness, 0.0001f, 10f);
			this.m_JitteringFactor = Mathf.Max(this.m_JitteringFactor, 0f);
			this.m_JitteringFrameRate = Mathf.Clamp(this.m_JitteringFrameRate, 0, 120);
			this.m_NoiseIntensity = Mathf.Clamp(this.m_NoiseIntensity, 0f, 1f);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001870C File Offset: 0x0001690C
		private void ValidateProperties()
		{
			this.AssignPropertiesFromAttachedSpotLight();
			this.ClampProperties();
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001871A File Offset: 0x0001691A
		private void HandleBackwardCompatibility(int serializedVersion, int newVersion)
		{
			if (serializedVersion == -1)
			{
				return;
			}
			if (serializedVersion == newVersion)
			{
				return;
			}
			Utils.MarkCurrentSceneDirty();
		}

		// Token: 0x04000617 RID: 1559
		public new const string ClassName = "VolumetricLightBeamHD";

		// Token: 0x04000618 RID: 1560
		[SerializeField]
		private bool m_ColorFromLight = true;

		// Token: 0x04000619 RID: 1561
		[SerializeField]
		private ColorMode m_ColorMode;

		// Token: 0x0400061A RID: 1562
		[SerializeField]
		private Color m_ColorFlat = Consts.Beam.FlatColor;

		// Token: 0x0400061B RID: 1563
		[SerializeField]
		private Gradient m_ColorGradient;

		// Token: 0x0400061C RID: 1564
		[SerializeField]
		private BlendingMode m_BlendingMode;

		// Token: 0x0400061D RID: 1565
		[SerializeField]
		private float m_Intensity = 1f;

		// Token: 0x0400061E RID: 1566
		[SerializeField]
		private float m_IntensityMultiplier = 1f;

		// Token: 0x0400061F RID: 1567
		[SerializeField]
		private float m_HDRPExposureWeight;

		// Token: 0x04000620 RID: 1568
		[SerializeField]
		private float m_SpotAngle = 35f;

		// Token: 0x04000621 RID: 1569
		[SerializeField]
		private float m_SpotAngleMultiplier = 1f;

		// Token: 0x04000622 RID: 1570
		[SerializeField]
		private float m_ConeRadiusStart = 0.1f;

		// Token: 0x04000623 RID: 1571
		[SerializeField]
		private bool m_Scalable = true;

		// Token: 0x04000624 RID: 1572
		[SerializeField]
		private float m_FallOffStart;

		// Token: 0x04000625 RID: 1573
		[SerializeField]
		private float m_FallOffEnd = 3f;

		// Token: 0x04000626 RID: 1574
		[SerializeField]
		private float m_FallOffEndMultiplier = 1f;

		// Token: 0x04000627 RID: 1575
		[SerializeField]
		private AttenuationEquationHD m_AttenuationEquation = AttenuationEquationHD.Quadratic;

		// Token: 0x04000628 RID: 1576
		[SerializeField]
		private float m_SideSoftness = 1f;

		// Token: 0x04000629 RID: 1577
		[SerializeField]
		private int m_RaymarchingQualityID = -1;

		// Token: 0x0400062A RID: 1578
		[SerializeField]
		private float m_JitteringFactor;

		// Token: 0x0400062B RID: 1579
		[SerializeField]
		private int m_JitteringFrameRate = 60;

		// Token: 0x0400062C RID: 1580
		[MinMaxRange(0f, 1f)]
		[SerializeField]
		private MinMaxRangeFloat m_JitteringLerpRange = Consts.Beam.HD.JitteringLerpRange;

		// Token: 0x0400062D RID: 1581
		[SerializeField]
		private NoiseMode m_NoiseMode;

		// Token: 0x0400062E RID: 1582
		[SerializeField]
		private float m_NoiseIntensity = 0.5f;

		// Token: 0x0400062F RID: 1583
		[SerializeField]
		private bool m_NoiseScaleUseGlobal = true;

		// Token: 0x04000630 RID: 1584
		[SerializeField]
		private float m_NoiseScaleLocal = 0.5f;

		// Token: 0x04000631 RID: 1585
		[SerializeField]
		private bool m_NoiseVelocityUseGlobal = true;

		// Token: 0x04000632 RID: 1586
		[SerializeField]
		private Vector3 m_NoiseVelocityLocal = Consts.Beam.NoiseVelocityDefault;

		// Token: 0x04000634 RID: 1588
		protected BeamGeometryHD m_BeamGeom;
	}
}
