using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000144 RID: 324
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-dynocclusion-sd-depthbuffer/")]
	public class DynamicOcclusionDepthBuffer : DynamicOcclusionAbstractBase
	{
		// Token: 0x060005BD RID: 1469 RVA: 0x0001B3CD File Offset: 0x000195CD
		protected override string GetShaderKeyword()
		{
			return "VLB_OCCLUSION_DEPTH_TEXTURE";
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001B3D4 File Offset: 0x000195D4
		protected override MaterialManager.SD.DynamicOcclusion GetDynamicOcclusionMode()
		{
			return MaterialManager.SD.DynamicOcclusion.DepthTexture;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001B3D7 File Offset: 0x000195D7
		private void ProcessOcclusionInternal()
		{
			this.UpdateDepthCameraPropertiesAccordingToBeam();
			this.m_DepthCamera.Render();
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001B3EA File Offset: 0x000195EA
		protected override bool OnProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource source)
		{
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				this.m_NeedToUpdateOcclusionNextFrame = true;
			}
			else
			{
				this.ProcessOcclusionInternal();
			}
			return true;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001B403 File Offset: 0x00019603
		private void Update()
		{
			if (this.m_NeedToUpdateOcclusionNextFrame && this.m_Master && this.m_DepthCamera && Time.frameCount > 1)
			{
				this.ProcessOcclusionInternal();
				this.m_NeedToUpdateOcclusionNextFrame = false;
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001B43C File Offset: 0x0001963C
		private void UpdateDepthCameraPropertiesAccordingToBeam()
		{
			Utils.SetupDepthCamera(this.m_DepthCamera, this.m_Master.coneApexOffsetZ, this.m_Master.maxGeometryDistance, this.m_Master.coneRadiusStart, this.m_Master.coneRadiusEnd, this.m_Master.beamLocalForward, this.m_Master.GetLossyScale(), this.m_Master.IsScalable(), this.m_Master.beamInternalLocalRotation, true);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001B4B0 File Offset: 0x000196B0
		public bool HasLayerMaskIssues()
		{
			if (Config.Instance.geometryOverrideLayer)
			{
				int num = 1 << Config.Instance.geometryLayerID;
				return (this.layerMask.value & num) == num;
			}
			return false;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001B4EB File Offset: 0x000196EB
		protected override void OnValidateProperties()
		{
			base.OnValidateProperties();
			this.depthMapResolution = Mathf.Clamp(Mathf.NextPowerOfTwo(this.depthMapResolution), 8, 2048);
			this.fadeDistanceToSurface = Mathf.Max(this.fadeDistanceToSurface, 0f);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001B528 File Offset: 0x00019728
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
				this.m_DepthCamera.cullingMask = this.layerMask;
				this.m_DepthCamera.clearFlags = CameraClearFlags.Depth;
				this.m_DepthCamera.depthTextureMode = DepthTextureMode.Depth;
				this.m_DepthCamera.renderingPath = RenderingPath.VertexLit;
				this.m_DepthCamera.useOcclusionCulling = this.useOcclusionCulling;
				this.m_DepthCamera.gameObject.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
				this.m_DepthCamera.transform.SetParent(base.transform, false);
				Config.Instance.SetURPScriptableRendererIndexToDepthCamera(this.m_DepthCamera);
				RenderTexture targetTexture = new RenderTexture(this.depthMapResolution, this.depthMapResolution, 16, RenderTextureFormat.Depth);
				this.m_DepthCamera.targetTexture = targetTexture;
				this.UpdateDepthCameraPropertiesAccordingToBeam();
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001B66A File Offset: 0x0001986A
		protected override void OnEnablePostValidate()
		{
			this.InstantiateOrActivateDepthCamera();
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001B672 File Offset: 0x00019872
		protected override void OnDisable()
		{
			base.OnDisable();
			if (this.m_DepthCamera)
			{
				this.m_DepthCamera.gameObject.SetActive(false);
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001B698 File Offset: 0x00019898
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001B6A0 File Offset: 0x000198A0
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.DestroyDepthCamera();
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001B6B0 File Offset: 0x000198B0
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

		// Token: 0x060005CB RID: 1483 RVA: 0x0001B720 File Offset: 0x00019920
		protected override void OnModifyMaterialCallback(MaterialModifier.Interface owner)
		{
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionDepthTexture, this.m_DepthCamera.targetTexture);
			Vector3 lossyScale = this.m_Master.GetLossyScale();
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionDepthProps, new Vector4(Mathf.Sign(lossyScale.x) * Mathf.Sign(lossyScale.z), Mathf.Sign(lossyScale.y), this.fadeDistanceToSurface, this.m_DepthCamera.orthographic ? 0f : 1f));
		}

		// Token: 0x040006BD RID: 1725
		public new const string ClassName = "DynamicOcclusionDepthBuffer";

		// Token: 0x040006BE RID: 1726
		public LayerMask layerMask = Consts.DynOcclusion.LayerMaskDefault;

		// Token: 0x040006BF RID: 1727
		public bool useOcclusionCulling = true;

		// Token: 0x040006C0 RID: 1728
		public int depthMapResolution = 128;

		// Token: 0x040006C1 RID: 1729
		public float fadeDistanceToSurface;

		// Token: 0x040006C2 RID: 1730
		private Camera m_DepthCamera;

		// Token: 0x040006C3 RID: 1731
		private bool m_NeedToUpdateOcclusionNextFrame;
	}
}
