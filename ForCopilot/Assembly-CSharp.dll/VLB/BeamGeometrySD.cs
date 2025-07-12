using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace VLB
{
	// Token: 0x02000140 RID: 320
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-sd/")]
	public class BeamGeometrySD : BeamGeometryAbstractBase, MaterialModifier.Interface
	{
		// Token: 0x06000579 RID: 1401 RVA: 0x0001A30D File Offset: 0x0001850D
		protected override VolumetricLightBeamAbstractBase GetMaster()
		{
			return this.m_Master;
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0001A315 File Offset: 0x00018515
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x0001A322 File Offset: 0x00018522
		private bool visible
		{
			get
			{
				return base.meshRenderer.enabled;
			}
			set
			{
				base.meshRenderer.enabled = value;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0001A330 File Offset: 0x00018530
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x0001A33D File Offset: 0x0001853D
		public int sortingLayerID
		{
			get
			{
				return base.meshRenderer.sortingLayerID;
			}
			set
			{
				base.meshRenderer.sortingLayerID = value;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x0001A34B File Offset: 0x0001854B
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x0001A358 File Offset: 0x00018558
		public int sortingOrder
		{
			get
			{
				return base.meshRenderer.sortingOrder;
			}
			set
			{
				base.meshRenderer.sortingOrder = value;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0001A366 File Offset: 0x00018566
		public bool _INTERNAL_IsFadeOutCoroutineRunning
		{
			get
			{
				return this.m_CoFadeOut != null;
			}
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001A374 File Offset: 0x00018574
		private float ComputeFadeOutFactor(Transform camTransform)
		{
			if (this.m_Master.isFadeOutEnabled)
			{
				float value = Vector3.SqrMagnitude(base.meshRenderer.bounds.center - camTransform.position);
				return Mathf.InverseLerp(this.m_Master.fadeOutEnd * this.m_Master.fadeOutEnd, this.m_Master.fadeOutBegin * this.m_Master.fadeOutBegin, value);
			}
			return 1f;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001A3EC File Offset: 0x000185EC
		private IEnumerator CoUpdateFadeOut()
		{
			while (this.m_Master.isFadeOutEnabled)
			{
				this.ComputeFadeOutFactor();
				yield return null;
			}
			this.SetFadeOutFactorProp(1f);
			this.m_CoFadeOut = null;
			yield break;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001A3FC File Offset: 0x000185FC
		private void ComputeFadeOutFactor()
		{
			Transform fadeOutCameraTransform = Config.Instance.fadeOutCameraTransform;
			if (fadeOutCameraTransform)
			{
				float fadeOutFactorProp = this.ComputeFadeOutFactor(fadeOutCameraTransform);
				this.SetFadeOutFactorProp(fadeOutFactorProp);
				return;
			}
			this.SetFadeOutFactorProp(1f);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001A437 File Offset: 0x00018637
		private void SetFadeOutFactorProp(float value)
		{
			if (value > 0f)
			{
				base.meshRenderer.enabled = true;
				this.MaterialChangeStart();
				this.SetMaterialProp(ShaderProperties.SD.FadeOutFactor, value);
				this.MaterialChangeStop();
				return;
			}
			base.meshRenderer.enabled = false;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001A472 File Offset: 0x00018672
		private void StopFadeOutCoroutine()
		{
			if (this.m_CoFadeOut != null)
			{
				base.StopCoroutine(this.m_CoFadeOut);
				this.m_CoFadeOut = null;
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001A48F File Offset: 0x0001868F
		public void RestartFadeOutCoroutine()
		{
			this.StopFadeOutCoroutine();
			if (this.m_Master && this.m_Master.isFadeOutEnabled)
			{
				this.m_CoFadeOut = base.StartCoroutine(this.CoUpdateFadeOut());
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001A4C3 File Offset: 0x000186C3
		public void OnMasterEnable()
		{
			this.visible = true;
			this.RestartFadeOutCoroutine();
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001A4D2 File Offset: 0x000186D2
		public void OnMasterDisable()
		{
			this.StopFadeOutCoroutine();
			this.visible = false;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001A4E1 File Offset: 0x000186E1
		private void OnDisable()
		{
			SRPHelper.UnregisterOnBeginCameraRendering(new Action<ScriptableRenderContext, Camera>(this.OnBeginCameraRenderingSRP));
			this.m_CurrentCameraRenderingSRP = null;
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x000022C9 File Offset: 0x000004C9
		public static bool isCustomRenderPipelineSupported
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0001A4FB File Offset: 0x000186FB
		private bool shouldUseGPUInstancedMaterial
		{
			get
			{
				return this.m_Master._INTERNAL_DynamicOcclusionMode != MaterialManager.SD.DynamicOcclusion.DepthTexture && Config.Instance.GetActualRenderingMode(ShaderMode.SD) == RenderingMode.GPUInstancing;
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001A51B File Offset: 0x0001871B
		private void OnEnable()
		{
			this.RestartFadeOutCoroutine();
			SRPHelper.RegisterOnBeginCameraRendering(new Action<ScriptableRenderContext, Camera>(this.OnBeginCameraRenderingSRP));
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001A534 File Offset: 0x00018734
		public void Initialize(VolumetricLightBeamSD master)
		{
			HideFlags proceduralObjectsHideFlags = Consts.Internal.ProceduralObjectsHideFlags;
			this.m_Master = master;
			base.transform.SetParent(master.transform, false);
			base.meshRenderer = base.gameObject.GetOrAddComponent<MeshRenderer>();
			base.meshRenderer.hideFlags = proceduralObjectsHideFlags;
			base.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			base.meshRenderer.receiveShadows = false;
			base.meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
			base.meshRenderer.lightProbeUsage = LightProbeUsage.Off;
			if (!this.shouldUseGPUInstancedMaterial)
			{
				this.m_CustomMaterial = Config.Instance.NewMaterialTransient(ShaderMode.SD, false);
				this.ApplyMaterial();
			}
			if (SortingLayer.IsValid(this.m_Master.sortingLayerID))
			{
				this.sortingLayerID = this.m_Master.sortingLayerID;
			}
			else
			{
				Debug.LogError(string.Format("Beam '{0}' has an invalid sortingLayerID ({1}). Please fix it by setting a valid layer.", Utils.GetPath(this.m_Master.transform), this.m_Master.sortingLayerID));
			}
			this.sortingOrder = this.m_Master.sortingOrder;
			base.meshFilter = base.gameObject.GetOrAddComponent<MeshFilter>();
			base.meshFilter.hideFlags = proceduralObjectsHideFlags;
			base.gameObject.hideFlags = proceduralObjectsHideFlags;
			this.RestartFadeOutCoroutine();
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001A664 File Offset: 0x00018864
		public void RegenerateMesh(bool masterEnabled)
		{
			if (Config.Instance.geometryOverrideLayer)
			{
				base.gameObject.layer = Config.Instance.geometryLayerID;
			}
			else
			{
				base.gameObject.layer = this.m_Master.gameObject.layer;
			}
			base.gameObject.tag = Config.Instance.geometryTag;
			if (base.coneMesh && this.m_CurrentMeshType == MeshType.Custom)
			{
				UnityEngine.Object.DestroyImmediate(base.coneMesh);
			}
			this.m_CurrentMeshType = this.m_Master.geomMeshType;
			MeshType geomMeshType = this.m_Master.geomMeshType;
			if (geomMeshType != MeshType.Shared)
			{
				if (geomMeshType == MeshType.Custom)
				{
					base.coneMesh = MeshGenerator.GenerateConeZ_Radii(1f, 1f, 1f, this.m_Master.geomCustomSides, this.m_Master.geomCustomSegments, this.m_Master.geomCap, Config.Instance.SD_requiresDoubleSidedMesh);
					base.coneMesh.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
					base.meshFilter.mesh = base.coneMesh;
				}
				else
				{
					Debug.LogError("Unsupported MeshType");
				}
			}
			else
			{
				base.coneMesh = GlobalMeshSD.Get();
				base.meshFilter.sharedMesh = base.coneMesh;
			}
			this.UpdateMaterialAndBounds();
			this.visible = masterEnabled;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001A7AC File Offset: 0x000189AC
		private Vector3 ComputeLocalMatrix()
		{
			float num = Mathf.Max(this.m_Master.coneRadiusStart, this.m_Master.coneRadiusEnd);
			base.transform.localScale = new Vector3(num, num, this.m_Master.maxGeometryDistance);
			base.transform.localRotation = this.m_Master.beamInternalLocalRotation;
			return base.transform.localScale;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x0001A813 File Offset: 0x00018A13
		private bool isNoiseEnabled
		{
			get
			{
				return this.m_Master.isNoiseEnabled && this.m_Master.noiseIntensity > 0f && Noise3D.isSupported;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0001A83B File Offset: 0x00018A3B
		private bool isDepthBlendEnabled
		{
			get
			{
				return BatchingHelper.forceEnableDepthBlend || this.m_Master.depthBlendDistance > 0f;
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001A858 File Offset: 0x00018A58
		private MaterialManager.StaticPropertiesSD ComputeMaterialStaticProperties()
		{
			MaterialManager.ColorGradient colorGradient = MaterialManager.ColorGradient.Off;
			if (this.m_Master.colorMode == ColorMode.Gradient)
			{
				colorGradient = ((Utils.GetFloatPackingPrecision() == Utils.FloatPackingPrecision.High) ? MaterialManager.ColorGradient.MatrixHigh : MaterialManager.ColorGradient.MatrixLow);
			}
			return new MaterialManager.StaticPropertiesSD
			{
				blendingMode = (MaterialManager.BlendingMode)this.m_Master.blendingMode,
				noise3D = (this.isNoiseEnabled ? MaterialManager.Noise3D.On : MaterialManager.Noise3D.Off),
				depthBlend = (this.isDepthBlendEnabled ? MaterialManager.SD.DepthBlend.On : MaterialManager.SD.DepthBlend.Off),
				colorGradient = colorGradient,
				dynamicOcclusion = this.m_Master._INTERNAL_DynamicOcclusionMode_Runtime,
				meshSkewing = (this.m_Master.hasMeshSkewing ? MaterialManager.SD.MeshSkewing.On : MaterialManager.SD.MeshSkewing.Off),
				shaderAccuracy = ((this.m_Master.shaderAccuracy == ShaderAccuracy.Fast) ? MaterialManager.SD.ShaderAccuracy.Fast : MaterialManager.SD.ShaderAccuracy.High)
			};
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001A910 File Offset: 0x00018B10
		private bool ApplyMaterial()
		{
			MaterialManager.StaticPropertiesSD staticPropertiesSD = this.ComputeMaterialStaticProperties();
			Material material;
			if (!this.shouldUseGPUInstancedMaterial)
			{
				material = this.m_CustomMaterial;
				if (material)
				{
					staticPropertiesSD.ApplyToMaterial(material);
				}
			}
			else
			{
				material = MaterialManager.GetInstancedMaterial(this.m_Master._INTERNAL_InstancedMaterialGroupID, ref staticPropertiesSD);
			}
			base.meshRenderer.material = material;
			return material != null;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00017169 File Offset: 0x00015369
		public void SetMaterialProp(int nameID, float value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetFloat(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetFloat(nameID, value);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00017192 File Offset: 0x00015392
		public void SetMaterialProp(int nameID, Vector4 value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetVector(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetVector(nameID, value);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x000171BB File Offset: 0x000153BB
		public void SetMaterialProp(int nameID, Color value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetColor(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetColor(nameID, value);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000171E4 File Offset: 0x000153E4
		public void SetMaterialProp(int nameID, Matrix4x4 value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetMatrix(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetMatrix(nameID, value);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001A96D File Offset: 0x00018B6D
		public void SetMaterialProp(int nameID, Texture value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetTexture(nameID, value);
				return;
			}
			Debug.LogError("Setting a Texture property to a GPU instanced material is not supported");
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001726D File Offset: 0x0001546D
		private void MaterialChangeStart()
		{
			if (this.m_CustomMaterial == null)
			{
				base.meshRenderer.GetPropertyBlock(MaterialManager.materialPropertyBlock);
			}
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001728D File Offset: 0x0001548D
		private void MaterialChangeStop()
		{
			if (this.m_CustomMaterial == null)
			{
				base.meshRenderer.SetPropertyBlock(MaterialManager.materialPropertyBlock);
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001A994 File Offset: 0x00018B94
		public void SetDynamicOcclusionCallback(string shaderKeyword, MaterialModifier.Callback cb)
		{
			this.m_MaterialModifierCallback = cb;
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetKeywordEnabled(shaderKeyword, cb != null);
				if (cb != null)
				{
					cb(this);
					return;
				}
			}
			else
			{
				this.UpdateMaterialAndBounds();
			}
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001A9CC File Offset: 0x00018BCC
		public void UpdateMaterialAndBounds()
		{
			if (!this.ApplyMaterial())
			{
				return;
			}
			this.MaterialChangeStart();
			if (this.m_CustomMaterial == null && this.m_MaterialModifierCallback != null)
			{
				this.m_MaterialModifierCallback(this);
			}
			float f = this.m_Master.coneAngle * 0.017453292f / 2f;
			this.SetMaterialProp(ShaderProperties.SD.ConeSlopeCosSin, new Vector2(Mathf.Cos(f), Mathf.Sin(f)));
			Vector2 v = new Vector2(Mathf.Max(this.m_Master.coneRadiusStart, 0.0001f), Mathf.Max(this.m_Master.coneRadiusEnd, 0.0001f));
			this.SetMaterialProp(ShaderProperties.ConeRadius, v);
			float x = Mathf.Sign(this.m_Master.coneApexOffsetZ) * Mathf.Max(Mathf.Abs(this.m_Master.coneApexOffsetZ), 0.0001f);
			this.SetMaterialProp(ShaderProperties.ConeGeomProps, new Vector2(x, (float)this.m_Master.geomSides));
			if (this.m_Master.usedColorMode == ColorMode.Flat)
			{
				this.SetMaterialProp(ShaderProperties.ColorFlat, this.m_Master.color);
			}
			else
			{
				Utils.FloatPackingPrecision floatPackingPrecision = Utils.GetFloatPackingPrecision();
				this.m_ColorGradientMatrix = this.m_Master.colorGradient.SampleInMatrix((int)floatPackingPrecision);
			}
			float value;
			float value2;
			this.m_Master.GetInsideAndOutsideIntensity(out value, out value2);
			this.SetMaterialProp(ShaderProperties.SD.AlphaInside, value);
			this.SetMaterialProp(ShaderProperties.SD.AlphaOutside, value2);
			this.SetMaterialProp(ShaderProperties.SD.AttenuationLerpLinearQuad, this.m_Master.attenuationLerpLinearQuad);
			this.SetMaterialProp(ShaderProperties.DistanceFallOff, new Vector3(this.m_Master.fallOffStart, this.m_Master.fallOffEnd, this.m_Master.maxGeometryDistance));
			this.SetMaterialProp(ShaderProperties.SD.DistanceCamClipping, this.m_Master.cameraClippingDistance);
			this.SetMaterialProp(ShaderProperties.SD.FresnelPow, Mathf.Max(0.001f, this.m_Master.fresnelPow));
			this.SetMaterialProp(ShaderProperties.SD.GlareBehind, this.m_Master.glareBehind);
			this.SetMaterialProp(ShaderProperties.SD.GlareFrontal, this.m_Master.glareFrontal);
			this.SetMaterialProp(ShaderProperties.SD.DrawCap, (float)(this.m_Master.geomCap ? 1 : 0));
			this.SetMaterialProp(ShaderProperties.SD.TiltVector, this.m_Master.tiltFactor);
			this.SetMaterialProp(ShaderProperties.SD.AdditionalClippingPlaneWS, this.m_Master.additionalClippingPlane);
			if (Config.Instance.isHDRPExposureWeightSupported)
			{
				this.SetMaterialProp(ShaderProperties.HDRPExposureWeight, this.m_Master.hdrpExposureWeight);
			}
			if (this.isDepthBlendEnabled)
			{
				this.SetMaterialProp(ShaderProperties.SD.DepthBlendDistance, this.m_Master.depthBlendDistance);
			}
			if (this.isNoiseEnabled)
			{
				Noise3D.LoadIfNeeded();
				Vector3 vector = this.m_Master.noiseVelocityUseGlobal ? Config.Instance.globalNoiseVelocity : this.m_Master.noiseVelocityLocal;
				float w = this.m_Master.noiseScaleUseGlobal ? Config.Instance.globalNoiseScale : this.m_Master.noiseScaleLocal;
				this.SetMaterialProp(ShaderProperties.NoiseVelocityAndScale, new Vector4(vector.x, vector.y, vector.z, w));
				this.SetMaterialProp(ShaderProperties.NoiseParam, new Vector2(this.m_Master.noiseIntensity, (this.m_Master.noiseMode == NoiseMode.WorldSpace) ? 0f : 1f));
			}
			Vector3 vector2 = this.ComputeLocalMatrix();
			if (this.m_Master.hasMeshSkewing)
			{
				Vector3 skewingLocalForwardDirectionNormalized = this.m_Master.skewingLocalForwardDirectionNormalized;
				this.SetMaterialProp(ShaderProperties.SD.LocalForwardDirection, skewingLocalForwardDirectionNormalized);
				if (base.coneMesh != null)
				{
					Vector3 vector3 = skewingLocalForwardDirectionNormalized;
					vector3 /= vector3.z;
					vector3 *= this.m_Master.fallOffEnd;
					vector3.x /= vector2.x;
					vector3.y /= vector2.y;
					Bounds bounds = MeshGenerator.ComputeBounds(1f, 1f, 1f);
					Vector3 min = bounds.min;
					Vector3 max = bounds.max;
					if (vector3.x > 0f)
					{
						max.x += vector3.x;
					}
					else
					{
						min.x += vector3.x;
					}
					if (vector3.y > 0f)
					{
						max.y += vector3.y;
					}
					else
					{
						min.y += vector3.y;
					}
					bounds.min = min;
					bounds.max = max;
					base.coneMesh.bounds = bounds;
				}
			}
			this.UpdateMatricesPropertiesForGPUInstancingSRP();
			this.MaterialChangeStop();
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001AE88 File Offset: 0x00019088
		private void UpdateMatricesPropertiesForGPUInstancingSRP()
		{
			if (SRPHelper.IsUsingCustomRenderPipeline() && Config.Instance.GetActualRenderingMode(ShaderMode.SD) == RenderingMode.GPUInstancing)
			{
				this.SetMaterialProp(ShaderProperties.LocalToWorldMatrix, base.transform.localToWorldMatrix);
				this.SetMaterialProp(ShaderProperties.WorldToLocalMatrix, base.transform.worldToLocalMatrix);
			}
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001AED6 File Offset: 0x000190D6
		private void OnBeginCameraRenderingSRP(ScriptableRenderContext context, Camera cam)
		{
			this.m_CurrentCameraRenderingSRP = cam;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001AEE0 File Offset: 0x000190E0
		private void OnWillRenderObject()
		{
			Camera cam;
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				cam = this.m_CurrentCameraRenderingSRP;
			}
			else
			{
				cam = Camera.current;
			}
			this.OnWillCameraRenderThisBeam(cam);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001AF0C File Offset: 0x0001910C
		private void OnWillCameraRenderThisBeam(Camera cam)
		{
			if (this.m_Master && cam && cam.enabled)
			{
				this.UpdateCameraRelatedProperties(cam);
				this.m_Master._INTERNAL_OnWillCameraRenderThisBeam(cam);
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001AF40 File Offset: 0x00019140
		private void UpdateCameraRelatedProperties(Camera cam)
		{
			if (cam && this.m_Master)
			{
				this.MaterialChangeStart();
				Vector3 posOS = this.m_Master.transform.InverseTransformPoint(cam.transform.position);
				Vector3 normalized = base.transform.InverseTransformDirection(cam.transform.forward).normalized;
				float w = cam.orthographic ? -1f : this.m_Master.GetInsideBeamFactorFromObjectSpacePos(posOS);
				this.SetMaterialProp(ShaderProperties.SD.CameraParams, new Vector4(normalized.x, normalized.y, normalized.z, w));
				this.UpdateMatricesPropertiesForGPUInstancingSRP();
				if (this.m_Master.usedColorMode == ColorMode.Gradient)
				{
					this.SetMaterialProp(ShaderProperties.ColorGradientMatrix, this.m_ColorGradientMatrix);
				}
				this.MaterialChangeStop();
				if (this.m_Master.depthBlendDistance > 0f)
				{
					cam.depthTextureMode |= DepthTextureMode.Depth;
				}
			}
		}

		// Token: 0x040006A7 RID: 1703
		private VolumetricLightBeamSD m_Master;

		// Token: 0x040006A8 RID: 1704
		private MeshType m_CurrentMeshType;

		// Token: 0x040006A9 RID: 1705
		private MaterialModifier.Callback m_MaterialModifierCallback;

		// Token: 0x040006AA RID: 1706
		private Coroutine m_CoFadeOut;

		// Token: 0x040006AB RID: 1707
		private Camera m_CurrentCameraRenderingSRP;
	}
}
