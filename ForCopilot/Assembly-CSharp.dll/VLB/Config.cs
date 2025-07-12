using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x020000E8 RID: 232
	[HelpURL("http://saladgamer.com/vlb-doc/config/")]
	public class Config : ScriptableObject
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003DB RID: 987 RVA: 0x00015DD3 File Offset: 0x00013FD3
		// (set) Token: 0x060003DC RID: 988 RVA: 0x00015DDB File Offset: 0x00013FDB
		public RenderPipeline renderPipeline
		{
			get
			{
				return this.m_RenderPipeline;
			}
			set
			{
				Debug.LogError("Modifying the RenderPipeline in standalone builds is not permitted");
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003DD RID: 989 RVA: 0x00015DE7 File Offset: 0x00013FE7
		// (set) Token: 0x060003DE RID: 990 RVA: 0x00015DEF File Offset: 0x00013FEF
		public RenderingMode renderingMode
		{
			get
			{
				return this.m_RenderingMode;
			}
			set
			{
				Debug.LogError("Modifying the RenderingMode in standalone builds is not permitted");
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00015DFC File Offset: 0x00013FFC
		public bool IsSRPBatcherSupported()
		{
			if (this.renderPipeline == RenderPipeline.BuiltIn)
			{
				return false;
			}
			RenderPipeline projectRenderPipeline = SRPHelper.projectRenderPipeline;
			return projectRenderPipeline == RenderPipeline.URP || projectRenderPipeline == RenderPipeline.HDRP;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00015E23 File Offset: 0x00014023
		public RenderingMode GetActualRenderingMode(ShaderMode shaderMode)
		{
			if (this.renderingMode == RenderingMode.SRPBatcher && !this.IsSRPBatcherSupported())
			{
				return RenderingMode.Default;
			}
			if (this.renderPipeline != RenderPipeline.BuiltIn && this.renderingMode == RenderingMode.MultiPass)
			{
				return RenderingMode.Default;
			}
			if (shaderMode == ShaderMode.HD && this.renderingMode == RenderingMode.MultiPass)
			{
				return RenderingMode.Default;
			}
			return this.renderingMode;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x00015E5E File Offset: 0x0001405E
		public bool SD_useSinglePassShader
		{
			get
			{
				return this.GetActualRenderingMode(ShaderMode.SD) > RenderingMode.MultiPass;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x00015E6A File Offset: 0x0001406A
		public bool SD_requiresDoubleSidedMesh
		{
			get
			{
				return this.SD_useSinglePassShader;
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00015E72 File Offset: 0x00014072
		public unsafe Shader GetBeamShader(ShaderMode mode)
		{
			return *this.GetBeamShaderInternal(mode);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00015E7C File Offset: 0x0001407C
		private ref Shader GetBeamShaderInternal(ShaderMode mode)
		{
			if (mode == ShaderMode.SD)
			{
				return ref this._BeamShader;
			}
			return ref this._BeamShaderHD;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00015E8E File Offset: 0x0001408E
		private int GetRenderQueueInternal(ShaderMode mode)
		{
			if (mode == ShaderMode.SD)
			{
				return this.geometryRenderQueue;
			}
			return this.geometryRenderQueueHD;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00015EA0 File Offset: 0x000140A0
		public Material NewMaterialTransient(ShaderMode mode, bool gpuInstanced)
		{
			Material material = MaterialManager.NewMaterialPersistent(this.GetBeamShader(mode), gpuInstanced);
			if (material)
			{
				material.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
				material.renderQueue = this.GetRenderQueueInternal(mode);
			}
			return material;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00015EDC File Offset: 0x000140DC
		public void SetURPScriptableRendererIndexToDepthCamera(Camera camera)
		{
			if (this.urpDepthCameraScriptableRendererIndex < 0)
			{
				return;
			}
			UniversalAdditionalCameraData universalAdditionalCameraData = CameraExtensions.GetUniversalAdditionalCameraData(camera);
			if (universalAdditionalCameraData)
			{
				universalAdditionalCameraData.SetRenderer(this.urpDepthCameraScriptableRendererIndex);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00015F0E File Offset: 0x0001410E
		public Transform fadeOutCameraTransform
		{
			get
			{
				if (this.m_CachedFadeOutCamera == null)
				{
					this.ForceUpdateFadeOutCamera();
				}
				return this.m_CachedFadeOutCamera;
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00015F2C File Offset: 0x0001412C
		public void ForceUpdateFadeOutCamera()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag(this.fadeOutCameraTag);
			if (gameObject)
			{
				this.m_CachedFadeOutCamera = gameObject.transform;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00015F59 File Offset: 0x00014159
		public int defaultRaymarchingQualityUniqueID
		{
			get
			{
				return this.m_DefaultRaymarchingQualityUniqueID;
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00015F61 File Offset: 0x00014161
		public RaymarchingQuality GetRaymarchingQualityForIndex(int index)
		{
			return this.m_RaymarchingQualities[index];
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00015F6C File Offset: 0x0001416C
		public RaymarchingQuality GetRaymarchingQualityForUniqueID(int id)
		{
			int raymarchingQualityIndexForUniqueID = this.GetRaymarchingQualityIndexForUniqueID(id);
			if (raymarchingQualityIndexForUniqueID >= 0)
			{
				return this.GetRaymarchingQualityForIndex(raymarchingQualityIndexForUniqueID);
			}
			return null;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00015F90 File Offset: 0x00014190
		public int GetRaymarchingQualityIndexForUniqueID(int id)
		{
			for (int i = 0; i < this.m_RaymarchingQualities.Length; i++)
			{
				RaymarchingQuality raymarchingQuality = this.m_RaymarchingQualities[i];
				if (raymarchingQuality != null && raymarchingQuality.uniqueID == id)
				{
					return i;
				}
			}
			Debug.LogErrorFormat("Failed to find RaymarchingQualityIndex for Unique ID {0}", new object[]
			{
				id
			});
			return -1;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00015FE1 File Offset: 0x000141E1
		public bool IsRaymarchingQualityUniqueIDValid(int id)
		{
			return this.GetRaymarchingQualityIndexForUniqueID(id) >= 0;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x00015FF0 File Offset: 0x000141F0
		public int raymarchingQualitiesCount
		{
			get
			{
				return Mathf.Max(1, (this.m_RaymarchingQualities != null) ? this.m_RaymarchingQualities.Length : 1);
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001600C File Offset: 0x0001420C
		private void CreateDefaultRaymarchingQualityPreset(bool onlyIfNeeded)
		{
			if (this.m_RaymarchingQualities == null || this.m_RaymarchingQualities.Length == 0 || !onlyIfNeeded)
			{
				this.m_RaymarchingQualities = new RaymarchingQuality[3];
				this.m_RaymarchingQualities[0] = RaymarchingQuality.New("Fast", 1, 5);
				this.m_RaymarchingQualities[1] = RaymarchingQuality.New("Balanced", 2, 10);
				this.m_RaymarchingQualities[2] = RaymarchingQuality.New("High", 3, 20);
				this.m_DefaultRaymarchingQualityUniqueID = this.m_RaymarchingQualities[1].uniqueID;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0001608A File Offset: 0x0001428A
		public bool isHDRPExposureWeightSupported
		{
			get
			{
				return this.renderPipeline == RenderPipeline.HDRP;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00016095 File Offset: 0x00014295
		public bool hasRenderPipelineMismatch
		{
			get
			{
				return SRPHelper.projectRenderPipeline == RenderPipeline.BuiltIn != (this.m_RenderPipeline == RenderPipeline.BuiltIn);
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x000160AD File Offset: 0x000142AD
		[RuntimeInitializeOnLoadMethod]
		private static void OnStartup()
		{
			Config.Instance.m_CachedFadeOutCamera = null;
			Config.Instance.RefreshGlobalShaderProperties();
			if (Config.Instance.hasRenderPipelineMismatch)
			{
				Debug.LogError("It looks like the 'Render Pipeline' is not correctly set in the config. Please make sure to select the proper value depending on your pipeline in use.", Config.Instance);
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000160E0 File Offset: 0x000142E0
		public void Reset()
		{
			this.geometryOverrideLayer = true;
			this.geometryLayerID = 1;
			this.geometryTag = "Untagged";
			this.geometryRenderQueue = 3000;
			this.geometryRenderQueueHD = 3100;
			this.sharedMeshSides = 24;
			this.sharedMeshSegments = 5;
			this.globalNoiseScale = 0.5f;
			this.globalNoiseVelocity = Consts.Beam.NoiseVelocityDefault;
			this.renderPipeline = RenderPipeline.BuiltIn;
			this.renderingMode = RenderingMode.Default;
			this.ditheringFactor = 0f;
			this.useLightColorTemperature = true;
			this.fadeOutCameraTag = "MainCamera";
			this.featureEnabledColorGradient = FeatureEnabledColorGradient.HighOnly;
			this.featureEnabledDepthBlend = true;
			this.featureEnabledNoise3D = true;
			this.featureEnabledDynamicOcclusion = true;
			this.featureEnabledMeshSkewing = true;
			this.featureEnabledShaderAccuracyHigh = true;
			this.hdBeamsCameraBlendingDistance = 0.5f;
			this.urpDepthCameraScriptableRendererIndex = -1;
			this.CreateDefaultRaymarchingQualityPreset(false);
			this.ResetInternalData();
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000161B8 File Offset: 0x000143B8
		private void RefreshGlobalShaderProperties()
		{
			Shader.SetGlobalFloat(ShaderProperties.GlobalUsesReversedZBuffer, SystemInfo.usesReversedZBuffer ? 1f : 0f);
			Shader.SetGlobalFloat(ShaderProperties.GlobalDitheringFactor, this.ditheringFactor);
			Shader.SetGlobalTexture(ShaderProperties.GlobalDitheringNoiseTex, this.ditheringNoiseTexture);
			Shader.SetGlobalFloat(ShaderProperties.HD.GlobalCameraBlendingDistance, this.hdBeamsCameraBlendingDistance);
			Shader.SetGlobalTexture(ShaderProperties.HD.GlobalJitteringNoiseTex, this.jitteringNoiseTexture);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00016224 File Offset: 0x00014424
		public void ResetInternalData()
		{
			this.noiseTexture3D = (Resources.Load("Noise3D_64x64x64") as Texture3D);
			this.dustParticlesPrefab = (Resources.Load("DustParticles", typeof(ParticleSystem)) as ParticleSystem);
			this.ditheringNoiseTexture = (Resources.Load("VLBDitheringNoise", typeof(Texture2D)) as Texture2D);
			this.jitteringNoiseTexture = (Resources.Load("VLBBlueNoise", typeof(Texture2D)) as Texture2D);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x000162A4 File Offset: 0x000144A4
		public ParticleSystem NewVolumetricDustParticles()
		{
			if (!this.dustParticlesPrefab)
			{
				if (Application.isPlaying)
				{
					Debug.LogError("Failed to instantiate VolumetricDustParticles prefab.");
				}
				return null;
			}
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate<ParticleSystem>(this.dustParticlesPrefab);
			particleSystem.useAutoRandomSeed = false;
			particleSystem.name = "Dust Particles";
			particleSystem.gameObject.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
			particleSystem.gameObject.SetActive(true);
			return particleSystem;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001630A File Offset: 0x0001450A
		private void OnEnable()
		{
			this.CreateDefaultRaymarchingQualityPreset(true);
			this.HandleBackwardCompatibility(this.pluginVersion, 20100);
			this.pluginVersion = 20100;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000045B1 File Offset: 0x000027B1
		private void HandleBackwardCompatibility(int serializedVersion, int newVersion)
		{
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0001632F File Offset: 0x0001452F
		public static Config Instance
		{
			get
			{
				return Config.GetInstance(true);
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00016337 File Offset: 0x00014537
		private static Config LoadAssetInternal(string assetName)
		{
			return Resources.Load<Config>(assetName);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00016340 File Offset: 0x00014540
		private static Config GetInstance(bool assertIfNotFound)
		{
			if (Config.ms_Instance == null)
			{
				Config x = Config.LoadAssetInternal("VLBConfigOverride" + PlatformHelper.GetCurrentPlatformSuffix());
				if (x == null)
				{
					x = Config.LoadAssetInternal("VLBConfigOverride");
				}
				Config.ms_Instance = x;
				Config.ms_Instance == null;
			}
			return Config.ms_Instance;
		}

		// Token: 0x040004A9 RID: 1193
		public const string ClassName = "Config";

		// Token: 0x040004AA RID: 1194
		public const string kAssetName = "VLBConfigOverride";

		// Token: 0x040004AB RID: 1195
		public const string kAssetNameExt = ".asset";

		// Token: 0x040004AC RID: 1196
		public bool geometryOverrideLayer = true;

		// Token: 0x040004AD RID: 1197
		public int geometryLayerID = 1;

		// Token: 0x040004AE RID: 1198
		public string geometryTag = "Untagged";

		// Token: 0x040004AF RID: 1199
		public int geometryRenderQueue = 3000;

		// Token: 0x040004B0 RID: 1200
		public int geometryRenderQueueHD = 3100;

		// Token: 0x040004B1 RID: 1201
		[FormerlySerializedAs("renderPipeline")]
		[FormerlySerializedAs("_RenderPipeline")]
		[SerializeField]
		private RenderPipeline m_RenderPipeline;

		// Token: 0x040004B2 RID: 1202
		[FormerlySerializedAs("renderingMode")]
		[FormerlySerializedAs("_RenderingMode")]
		[SerializeField]
		private RenderingMode m_RenderingMode = RenderingMode.Default;

		// Token: 0x040004B3 RID: 1203
		public float ditheringFactor;

		// Token: 0x040004B4 RID: 1204
		public bool useLightColorTemperature = true;

		// Token: 0x040004B5 RID: 1205
		public int sharedMeshSides = 24;

		// Token: 0x040004B6 RID: 1206
		public int sharedMeshSegments = 5;

		// Token: 0x040004B7 RID: 1207
		public float hdBeamsCameraBlendingDistance = 0.5f;

		// Token: 0x040004B8 RID: 1208
		public int urpDepthCameraScriptableRendererIndex = -1;

		// Token: 0x040004B9 RID: 1209
		[Range(0.01f, 2f)]
		public float globalNoiseScale = 0.5f;

		// Token: 0x040004BA RID: 1210
		public Vector3 globalNoiseVelocity = Consts.Beam.NoiseVelocityDefault;

		// Token: 0x040004BB RID: 1211
		public string fadeOutCameraTag = "MainCamera";

		// Token: 0x040004BC RID: 1212
		[HighlightNull]
		public Texture3D noiseTexture3D;

		// Token: 0x040004BD RID: 1213
		[HighlightNull]
		public ParticleSystem dustParticlesPrefab;

		// Token: 0x040004BE RID: 1214
		[HighlightNull]
		public Texture2D ditheringNoiseTexture;

		// Token: 0x040004BF RID: 1215
		[HighlightNull]
		public Texture2D jitteringNoiseTexture;

		// Token: 0x040004C0 RID: 1216
		public FeatureEnabledColorGradient featureEnabledColorGradient = FeatureEnabledColorGradient.HighOnly;

		// Token: 0x040004C1 RID: 1217
		public bool featureEnabledDepthBlend = true;

		// Token: 0x040004C2 RID: 1218
		public bool featureEnabledNoise3D = true;

		// Token: 0x040004C3 RID: 1219
		public bool featureEnabledDynamicOcclusion = true;

		// Token: 0x040004C4 RID: 1220
		public bool featureEnabledMeshSkewing = true;

		// Token: 0x040004C5 RID: 1221
		public bool featureEnabledShaderAccuracyHigh = true;

		// Token: 0x040004C6 RID: 1222
		public bool featureEnabledShadow = true;

		// Token: 0x040004C7 RID: 1223
		public bool featureEnabledCookie = true;

		// Token: 0x040004C8 RID: 1224
		[SerializeField]
		private RaymarchingQuality[] m_RaymarchingQualities;

		// Token: 0x040004C9 RID: 1225
		[SerializeField]
		private int m_DefaultRaymarchingQualityUniqueID;

		// Token: 0x040004CA RID: 1226
		[SerializeField]
		private int pluginVersion = -1;

		// Token: 0x040004CB RID: 1227
		[SerializeField]
		private Material _DummyMaterial;

		// Token: 0x040004CC RID: 1228
		[SerializeField]
		private Material _DummyMaterialHD;

		// Token: 0x040004CD RID: 1229
		[SerializeField]
		private Shader _BeamShader;

		// Token: 0x040004CE RID: 1230
		[SerializeField]
		private Shader _BeamShaderHD;

		// Token: 0x040004CF RID: 1231
		private Transform m_CachedFadeOutCamera;

		// Token: 0x040004D0 RID: 1232
		private static Config ms_Instance;
	}
}
