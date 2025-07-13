using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200011D RID: 285
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamHD))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-shadow-hd/")]
	public class VolumetricShadowHD : MonoBehaviour
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x00018887 File Offset: 0x00016A87
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x0001888F File Offset: 0x00016A8F
		public float strength
		{
			get
			{
				return this.m_Strength;
			}
			set
			{
				if (this.m_Strength != value)
				{
					this.m_Strength = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x000188A7 File Offset: 0x00016AA7
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x000188AF File Offset: 0x00016AAF
		public ShadowUpdateRate updateRate
		{
			get
			{
				return this.m_UpdateRate;
			}
			set
			{
				this.m_UpdateRate = value;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x000188B8 File Offset: 0x00016AB8
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x000188C0 File Offset: 0x00016AC0
		public int waitXFrames
		{
			get
			{
				return this.m_WaitXFrames;
			}
			set
			{
				this.m_WaitXFrames = value;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x000188C9 File Offset: 0x00016AC9
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x000188D1 File Offset: 0x00016AD1
		public LayerMask layerMask
		{
			get
			{
				return this.m_LayerMask;
			}
			set
			{
				this.m_LayerMask = value;
				this.UpdateDepthCameraProperties();
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x000188E0 File Offset: 0x00016AE0
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x000188E8 File Offset: 0x00016AE8
		public bool useOcclusionCulling
		{
			get
			{
				return this.m_UseOcclusionCulling;
			}
			set
			{
				this.m_UseOcclusionCulling = value;
				this.UpdateDepthCameraProperties();
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x000188F7 File Offset: 0x00016AF7
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x000188FF File Offset: 0x00016AFF
		public int depthMapResolution
		{
			get
			{
				return this.m_DepthMapResolution;
			}
			set
			{
				if (this.m_DepthCamera != null && Application.isPlaying)
				{
					Debug.LogErrorFormat(Consts.Shadow.GetErrorChangeRuntimeDepthMapResolution(this), Array.Empty<object>());
				}
				this.m_DepthMapResolution = value;
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001892D File Offset: 0x00016B2D
		public void ProcessOcclusionManually()
		{
			this.ProcessOcclusion(VolumetricShadowHD.ProcessOcclusionSource.User);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00018936 File Offset: 0x00016B36
		public void UpdateDepthCameraProperties()
		{
			if (this.m_DepthCamera)
			{
				this.m_DepthCamera.cullingMask = this.layerMask;
				this.m_DepthCamera.useOcclusionCulling = this.useOcclusionCulling;
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001896C File Offset: 0x00016B6C
		private void ProcessOcclusion(VolumetricShadowHD.ProcessOcclusionSource source)
		{
			if (!Config.Instance.featureEnabledShadow)
			{
				return;
			}
			if (this.m_LastFrameRendered == Time.frameCount && Application.isPlaying && source == VolumetricShadowHD.ProcessOcclusionSource.OnEnable)
			{
				return;
			}
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				this.m_NeedToUpdateOcclusionNextFrame = true;
			}
			else
			{
				this.ProcessOcclusionInternal();
			}
			this.SetDirty();
			if (this.updateRate.HasFlag(ShadowUpdateRate.OnBeamMove))
			{
				this.m_TransformPacked = base.transform.GetWorldPacked();
			}
			bool flag = this.m_LastFrameRendered < 0;
			this.m_LastFrameRendered = Time.frameCount;
			if (flag && VolumetricShadowHD._INTERNAL_ApplyRandomFrameOffset)
			{
				this.m_LastFrameRendered += UnityEngine.Random.Range(0, this.waitXFrames);
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00018A1C File Offset: 0x00016C1C
		public static void ApplyMaterialProperties(VolumetricShadowHD instance, BeamGeometryHD geom)
		{
			if (instance && instance.enabled)
			{
				geom.SetMaterialProp(ShaderProperties.HD.ShadowDepthTexture, instance.m_DepthCamera.targetTexture);
				Vector3 vector = instance.m_Master.scalable ? instance.m_Master.GetLossyScale() : Vector3.one;
				geom.SetMaterialProp(ShaderProperties.HD.ShadowProps, new Vector4(Mathf.Sign(vector.x) * Mathf.Sign(vector.z), Mathf.Sign(vector.y), instance.m_Strength, instance.m_DepthCamera.orthographic ? 0f : 1f));
				return;
			}
			geom.SetMaterialProp(ShaderProperties.HD.ShadowDepthTexture, BeamGeometryHD.InvalidTexture.NoDepth);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00018AD3 File Offset: 0x00016CD3
		private void Awake()
		{
			this.m_Master = base.GetComponent<VolumetricLightBeamHD>();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00018AE1 File Offset: 0x00016CE1
		private void OnEnable()
		{
			this.OnValidateProperties();
			this.InstantiateOrActivateDepthCamera();
			this.OnBeamEnabled();
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00018AF5 File Offset: 0x00016CF5
		private void OnDisable()
		{
			if (this.m_DepthCamera)
			{
				this.m_DepthCamera.gameObject.SetActive(false);
			}
			this.SetDirty();
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00018B1B File Offset: 0x00016D1B
		private void OnDestroy()
		{
			this.DestroyDepthCamera();
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00018B23 File Offset: 0x00016D23
		private void ProcessOcclusionInternal()
		{
			this.UpdateDepthCameraPropertiesAccordingToBeam();
			this.m_DepthCamera.Render();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00018B36 File Offset: 0x00016D36
		private void OnBeamEnabled()
		{
			if (!base.enabled)
			{
				return;
			}
			if (!this.updateRate.HasFlag(ShadowUpdateRate.Never))
			{
				this.ProcessOcclusion(VolumetricShadowHD.ProcessOcclusionSource.OnEnable);
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00018B60 File Offset: 0x00016D60
		public void OnWillCameraRenderThisBeam(Camera cam, BeamGeometryHD beamGeom)
		{
			if (!base.enabled)
			{
				return;
			}
			if (cam != null && cam.enabled && Time.frameCount != this.m_LastFrameRendered && this.updateRate != ShadowUpdateRate.Never)
			{
				bool flag = false;
				if (!flag && this.updateRate.HasFlag(ShadowUpdateRate.OnBeamMove) && !this.m_TransformPacked.IsSame(base.transform))
				{
					flag = true;
				}
				if (!flag && this.updateRate.HasFlag(ShadowUpdateRate.EveryXFrames) && Time.frameCount >= this.m_LastFrameRendered + this.waitXFrames)
				{
					flag = true;
				}
				if (flag)
				{
					this.ProcessOcclusion(VolumetricShadowHD.ProcessOcclusionSource.RenderLoop);
				}
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00018C10 File Offset: 0x00016E10
		private void Update()
		{
			if (this.m_NeedToUpdateOcclusionNextFrame && this.m_Master && this.m_DepthCamera && Time.frameCount > 1)
			{
				this.ProcessOcclusionInternal();
				this.m_NeedToUpdateOcclusionNextFrame = false;
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00018C4C File Offset: 0x00016E4C
		private void UpdateDepthCameraPropertiesAccordingToBeam()
		{
			Utils.SetupDepthCamera(this.m_DepthCamera, this.m_Master.GetConeApexOffsetZ(true), this.m_Master.maxGeometryDistance, this.m_Master.coneRadiusStart, this.m_Master.coneRadiusEnd, this.m_Master.beamLocalForward, this.m_Master.GetLossyScale(), this.m_Master.scalable, this.m_Master.beamInternalLocalRotation, false);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00018CC0 File Offset: 0x00016EC0
		private void InstantiateOrActivateDepthCamera()
		{
			if (this.m_DepthCamera != null)
			{
				this.m_DepthCamera.gameObject.SetActive(true);
				return;
			}
			base.gameObject.ForeachComponentsInDirectChildrenOnly(delegate(Camera cam)
			{
				UnityEngine.Object.DestroyImmediate(cam.gameObject);
			}, true);
			this.m_DepthCamera = Utils.NewWithComponent<Camera>("Depth Camera");
			if (this.m_DepthCamera && this.m_Master)
			{
				this.m_DepthCamera.enabled = false;
				this.UpdateDepthCameraProperties();
				this.m_DepthCamera.clearFlags = CameraClearFlags.Depth;
				this.m_DepthCamera.depthTextureMode = DepthTextureMode.Depth;
				this.m_DepthCamera.renderingPath = RenderingPath.Forward;
				this.m_DepthCamera.gameObject.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
				this.m_DepthCamera.transform.SetParent(base.transform, false);
				Config.Instance.SetURPScriptableRendererIndexToDepthCamera(this.m_DepthCamera);
				RenderTexture targetTexture = new RenderTexture(this.depthMapResolution, this.depthMapResolution, 16, RenderTextureFormat.Depth);
				this.m_DepthCamera.targetTexture = targetTexture;
				this.UpdateDepthCameraPropertiesAccordingToBeam();
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00018DE4 File Offset: 0x00016FE4
		private void DestroyDepthCamera()
		{
			if (this.m_DepthCamera)
			{
				if (this.m_DepthCamera.targetTexture)
				{
					this.m_DepthCamera.targetTexture.Release();
					UnityEngine.Object.DestroyImmediate(this.m_DepthCamera.targetTexture);
					this.m_DepthCamera.targetTexture = null;
				}
				UnityEngine.Object.DestroyImmediate(this.m_DepthCamera.gameObject);
				this.m_DepthCamera = null;
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00018E53 File Offset: 0x00017053
		private void OnValidateProperties()
		{
			this.m_WaitXFrames = Mathf.Clamp(this.m_WaitXFrames, 1, 60);
			this.m_DepthMapResolution = Mathf.Clamp(Mathf.NextPowerOfTwo(this.m_DepthMapResolution), 8, 2048);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00018E85 File Offset: 0x00017085
		private void SetDirty()
		{
			if (this.m_Master)
			{
				this.m_Master.SetPropertyDirty(DirtyProps.ShadowProps);
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00018EA4 File Offset: 0x000170A4
		public int _INTERNAL_LastFrameRendered
		{
			get
			{
				return this.m_LastFrameRendered;
			}
		}

		// Token: 0x04000637 RID: 1591
		public const string ClassName = "VolumetricShadowHD";

		// Token: 0x04000638 RID: 1592
		[SerializeField]
		private float m_Strength = 1f;

		// Token: 0x04000639 RID: 1593
		[SerializeField]
		private ShadowUpdateRate m_UpdateRate = ShadowUpdateRate.EveryXFrames;

		// Token: 0x0400063A RID: 1594
		[SerializeField]
		private int m_WaitXFrames = 3;

		// Token: 0x0400063B RID: 1595
		[SerializeField]
		private LayerMask m_LayerMask = Consts.Shadow.LayerMaskDefault;

		// Token: 0x0400063C RID: 1596
		[SerializeField]
		private bool m_UseOcclusionCulling = true;

		// Token: 0x0400063D RID: 1597
		[SerializeField]
		private int m_DepthMapResolution = 128;

		// Token: 0x0400063E RID: 1598
		private VolumetricLightBeamHD m_Master;

		// Token: 0x0400063F RID: 1599
		private TransformUtils.Packed m_TransformPacked;

		// Token: 0x04000640 RID: 1600
		private int m_LastFrameRendered = int.MinValue;

		// Token: 0x04000641 RID: 1601
		private Camera m_DepthCamera;

		// Token: 0x04000642 RID: 1602
		private bool m_NeedToUpdateOcclusionNextFrame;

		// Token: 0x04000643 RID: 1603
		public static bool _INTERNAL_ApplyRandomFrameOffset = true;

		// Token: 0x0200011E RID: 286
		private enum ProcessOcclusionSource
		{
			// Token: 0x04000645 RID: 1605
			RenderLoop,
			// Token: 0x04000646 RID: 1606
			OnEnable,
			// Token: 0x04000647 RID: 1607
			EditorUpdate,
			// Token: 0x04000648 RID: 1608
			User
		}
	}
}
