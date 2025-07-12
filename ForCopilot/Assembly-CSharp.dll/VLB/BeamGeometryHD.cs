using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace VLB
{
	// Token: 0x02000115 RID: 277
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-hd/")]
	public class BeamGeometryHD : BeamGeometryAbstractBase
	{
		// Token: 0x0600043D RID: 1085 RVA: 0x00016D03 File Offset: 0x00014F03
		protected override VolumetricLightBeamAbstractBase GetMaster()
		{
			return this.m_Master;
		}

		// Token: 0x170000A3 RID: 163
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x00016D0B File Offset: 0x00014F0B
		public bool visible
		{
			set
			{
				if (base.meshRenderer)
				{
					base.meshRenderer.enabled = value;
				}
			}
		}

		// Token: 0x170000A4 RID: 164
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x00016D26 File Offset: 0x00014F26
		public int sortingLayerID
		{
			set
			{
				if (base.meshRenderer)
				{
					base.meshRenderer.sortingLayerID = value;
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x00016D41 File Offset: 0x00014F41
		public int sortingOrder
		{
			set
			{
				if (base.meshRenderer)
				{
					base.meshRenderer.sortingOrder = value;
				}
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00016D5C File Offset: 0x00014F5C
		private void OnDisable()
		{
			SRPHelper.UnregisterOnBeginCameraRendering(new Action<ScriptableRenderContext, Camera>(this.OnBeginCameraRenderingSRP));
			this.m_CurrentCameraRenderingSRP = null;
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x000022C9 File Offset: 0x000004C9
		public static bool isCustomRenderPipelineSupported
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x00016D76 File Offset: 0x00014F76
		private bool shouldUseGPUInstancedMaterial
		{
			get
			{
				return Config.Instance.GetActualRenderingMode(ShaderMode.HD) == RenderingMode.GPUInstancing && this.m_Cookie == null && this.m_Shadow == null;
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00016DA4 File Offset: 0x00014FA4
		private void OnEnable()
		{
			SRPHelper.RegisterOnBeginCameraRendering(new Action<ScriptableRenderContext, Camera>(this.OnBeginCameraRenderingSRP));
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00016DB8 File Offset: 0x00014FB8
		public void Initialize(VolumetricLightBeamHD master)
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
			this.m_Cookie = this.m_Master.GetAdditionalComponentCookie();
			this.m_Shadow = this.m_Master.GetAdditionalComponentShadow();
			if (!this.shouldUseGPUInstancedMaterial)
			{
				this.m_CustomMaterial = Config.Instance.NewMaterialTransient(ShaderMode.HD, false);
				this.ApplyMaterial();
			}
			if (this.m_Master.DoesSupportSorting2D())
			{
				if (SortingLayer.IsValid(this.m_Master.GetSortingLayerID()))
				{
					this.sortingLayerID = this.m_Master.GetSortingLayerID();
				}
				else
				{
					Debug.LogError(string.Format("Beam '{0}' has an invalid sortingLayerID ({1}). Please fix it by setting a valid layer.", Utils.GetPath(this.m_Master.transform), this.m_Master.GetSortingLayerID()));
				}
				this.sortingOrder = this.m_Master.GetSortingOrder();
			}
			base.meshFilter = base.gameObject.GetOrAddComponent<MeshFilter>();
			base.meshFilter.hideFlags = proceduralObjectsHideFlags;
			base.gameObject.hideFlags = proceduralObjectsHideFlags;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00016F10 File Offset: 0x00015110
		public void RegenerateMesh()
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
			base.coneMesh = GlobalMeshHD.Get();
			base.meshFilter.sharedMesh = base.coneMesh;
			this.UpdateMaterialAndBounds();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00016F94 File Offset: 0x00015194
		private Vector3 ComputeLocalMatrix()
		{
			float num = Mathf.Max(this.m_Master.coneRadiusStart, this.m_Master.coneRadiusEnd);
			Vector3 vector = new Vector3(num, num, this.m_Master.maxGeometryDistance);
			if (!this.m_Master.scalable)
			{
				vector = vector.Divide(this.m_Master.GetLossyScale());
			}
			base.transform.localScale = vector;
			base.transform.localRotation = this.m_Master.beamInternalLocalRotation;
			return vector;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00017013 File Offset: 0x00015213
		private bool isNoiseEnabled
		{
			get
			{
				return this.m_Master.isNoiseEnabled && this.m_Master.noiseIntensity > 0f && Noise3D.isSupported;
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0001703C File Offset: 0x0001523C
		private MaterialManager.StaticPropertiesHD ComputeMaterialStaticProperties()
		{
			MaterialManager.ColorGradient colorGradient = MaterialManager.ColorGradient.Off;
			if (this.m_Master.colorMode == ColorMode.Gradient)
			{
				colorGradient = ((Utils.GetFloatPackingPrecision() == Utils.FloatPackingPrecision.High) ? MaterialManager.ColorGradient.MatrixHigh : MaterialManager.ColorGradient.MatrixLow);
			}
			return new MaterialManager.StaticPropertiesHD
			{
				blendingMode = (MaterialManager.BlendingMode)this.m_Master.blendingMode,
				attenuation = ((this.m_Master.attenuationEquation == AttenuationEquationHD.Linear) ? MaterialManager.HD.Attenuation.Linear : MaterialManager.HD.Attenuation.Quadratic),
				noise3D = (this.isNoiseEnabled ? MaterialManager.Noise3D.On : MaterialManager.Noise3D.Off),
				colorGradient = colorGradient,
				shadow = ((this.m_Shadow != null) ? MaterialManager.HD.Shadow.On : MaterialManager.HD.Shadow.Off),
				cookie = ((this.m_Cookie != null) ? ((this.m_Cookie.channel == CookieChannel.RGBA) ? MaterialManager.HD.Cookie.RGBA : MaterialManager.HD.Cookie.SingleChannel) : MaterialManager.HD.Cookie.Off),
				raymarchingQualityIndex = this.m_Master.raymarchingQualityIndex
			};
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0001710C File Offset: 0x0001530C
		private bool ApplyMaterial()
		{
			MaterialManager.StaticPropertiesHD staticPropertiesHD = this.ComputeMaterialStaticProperties();
			Material material;
			if (!this.shouldUseGPUInstancedMaterial)
			{
				material = this.m_CustomMaterial;
				if (material)
				{
					staticPropertiesHD.ApplyToMaterial(material);
				}
			}
			else
			{
				material = MaterialManager.GetInstancedMaterial(this.m_Master._INTERNAL_InstancedMaterialGroupID, ref staticPropertiesHD);
			}
			base.meshRenderer.material = material;
			return material != null;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00017169 File Offset: 0x00015369
		public void SetMaterialProp(int nameID, float value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetFloat(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetFloat(nameID, value);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00017192 File Offset: 0x00015392
		public void SetMaterialProp(int nameID, Vector4 value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetVector(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetVector(nameID, value);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000171BB File Offset: 0x000153BB
		public void SetMaterialProp(int nameID, Color value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetColor(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetColor(nameID, value);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000171E4 File Offset: 0x000153E4
		public void SetMaterialProp(int nameID, Matrix4x4 value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetMatrix(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetMatrix(nameID, value);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0001720D File Offset: 0x0001540D
		public void SetMaterialProp(int nameID, Texture value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetTexture(nameID, value);
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001722C File Offset: 0x0001542C
		public void SetMaterialProp(int nameID, BeamGeometryHD.InvalidTexture invalidTexture)
		{
			if (this.m_CustomMaterial)
			{
				Texture value = null;
				if (invalidTexture == BeamGeometryHD.InvalidTexture.NoDepth)
				{
					value = (SystemInfo.usesReversedZBuffer ? Texture2D.blackTexture : Texture2D.whiteTexture);
				}
				this.m_CustomMaterial.SetTexture(nameID, value);
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001726D File Offset: 0x0001546D
		private void MaterialChangeStart()
		{
			if (this.m_CustomMaterial == null)
			{
				base.meshRenderer.GetPropertyBlock(MaterialManager.materialPropertyBlock);
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0001728D File Offset: 0x0001548D
		private void MaterialChangeStop()
		{
			if (this.m_CustomMaterial == null)
			{
				base.meshRenderer.SetPropertyBlock(MaterialManager.materialPropertyBlock);
			}
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000172AD File Offset: 0x000154AD
		public void SetPropertyDirty(DirtyProps prop)
		{
			this.m_DirtyProps |= prop;
			if (prop.HasAtLeastOneFlag(DirtyProps.OnlyMaterialChangeOnly))
			{
				this.UpdateMaterialAndBounds();
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x000172DA File Offset: 0x000154DA
		private void UpdateMaterialAndBounds()
		{
			if (!this.ApplyMaterial())
			{
				return;
			}
			this.MaterialChangeStart();
			this.m_DirtyProps = DirtyProps.All;
			if (this.isNoiseEnabled)
			{
				Noise3D.LoadIfNeeded();
			}
			this.ComputeLocalMatrix();
			this.UpdateMatricesPropertiesForGPUInstancingSRP();
			this.MaterialChangeStop();
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00017318 File Offset: 0x00015518
		private void UpdateMatricesPropertiesForGPUInstancingSRP()
		{
			if (SRPHelper.IsUsingCustomRenderPipeline() && Config.Instance.GetActualRenderingMode(ShaderMode.HD) == RenderingMode.GPUInstancing)
			{
				this.SetMaterialProp(ShaderProperties.LocalToWorldMatrix, base.transform.localToWorldMatrix);
				this.SetMaterialProp(ShaderProperties.WorldToLocalMatrix, base.transform.worldToLocalMatrix);
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00017366 File Offset: 0x00015566
		private void OnBeginCameraRenderingSRP(ScriptableRenderContext context, Camera cam)
		{
			this.m_CurrentCameraRenderingSRP = cam;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00017370 File Offset: 0x00015570
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

		// Token: 0x06000458 RID: 1112 RVA: 0x0001739C File Offset: 0x0001559C
		private void OnWillCameraRenderThisBeam(Camera cam)
		{
			if (this.m_Master && cam && cam.enabled)
			{
				this.UpdateMaterialPropertiesForCamera(cam);
				if (this.m_Shadow)
				{
					this.m_Shadow.OnWillCameraRenderThisBeam(cam, this);
				}
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000173DC File Offset: 0x000155DC
		private void UpdateDirtyMaterialProperties()
		{
			if (this.m_DirtyProps != DirtyProps.None)
			{
				if (this.m_DirtyProps.HasFlag(DirtyProps.Intensity))
				{
					this.SetMaterialProp(ShaderProperties.HD.Intensity, this.m_Master.intensity);
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.HDRPExposureWeight) && Config.Instance.isHDRPExposureWeightSupported)
				{
					this.SetMaterialProp(ShaderProperties.HDRPExposureWeight, this.m_Master.hdrpExposureWeight);
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.SideSoftness))
				{
					this.SetMaterialProp(ShaderProperties.HD.SideSoftness, this.m_Master.sideSoftness);
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.Color))
				{
					if (this.m_Master.colorMode == ColorMode.Flat)
					{
						this.SetMaterialProp(ShaderProperties.ColorFlat, this.m_Master.colorFlat);
					}
					else
					{
						Utils.FloatPackingPrecision floatPackingPrecision = Utils.GetFloatPackingPrecision();
						this.m_ColorGradientMatrix = this.m_Master.colorGradient.SampleInMatrix((int)floatPackingPrecision);
					}
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.Cone))
				{
					Vector2 v = new Vector2(Mathf.Max(this.m_Master.coneRadiusStart, 0.0001f), Mathf.Max(this.m_Master.coneRadiusEnd, 0.0001f));
					this.SetMaterialProp(ShaderProperties.ConeRadius, v);
					float coneApexOffsetZ = this.m_Master.GetConeApexOffsetZ(false);
					float x = Mathf.Sign(coneApexOffsetZ) * Mathf.Max(Mathf.Abs(coneApexOffsetZ), 0.0001f);
					this.SetMaterialProp(ShaderProperties.ConeGeomProps, new Vector2(x, (float)Config.Instance.sharedMeshSides));
					this.SetMaterialProp(ShaderProperties.DistanceFallOff, new Vector3(this.m_Master.fallOffStart, this.m_Master.fallOffEnd, this.m_Master.maxGeometryDistance));
					this.ComputeLocalMatrix();
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.Jittering))
				{
					this.SetMaterialProp(ShaderProperties.HD.Jittering, new Vector4(this.m_Master.jitteringFactor, (float)this.m_Master.jitteringFrameRate, this.m_Master.jitteringLerpRange.minValue, this.m_Master.jitteringLerpRange.maxValue));
				}
				if (this.isNoiseEnabled)
				{
					if (this.m_DirtyProps.HasFlag(DirtyProps.NoiseMode) || this.m_DirtyProps.HasFlag(DirtyProps.NoiseIntensity))
					{
						this.SetMaterialProp(ShaderProperties.NoiseParam, new Vector2(this.m_Master.noiseIntensity, (this.m_Master.noiseMode == NoiseMode.WorldSpace) ? 0f : 1f));
					}
					if (this.m_DirtyProps.HasFlag(DirtyProps.NoiseVelocityAndScale))
					{
						Vector3 vector = this.m_Master.noiseVelocityUseGlobal ? Config.Instance.globalNoiseVelocity : this.m_Master.noiseVelocityLocal;
						float w = this.m_Master.noiseScaleUseGlobal ? Config.Instance.globalNoiseScale : this.m_Master.noiseScaleLocal;
						this.SetMaterialProp(ShaderProperties.NoiseVelocityAndScale, new Vector4(vector.x, vector.y, vector.z, w));
					}
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.CookieProps))
				{
					VolumetricCookieHD.ApplyMaterialProperties(this.m_Cookie, this);
				}
				if (this.m_DirtyProps.HasFlag(DirtyProps.ShadowProps))
				{
					VolumetricShadowHD.ApplyMaterialProperties(this.m_Shadow, this);
				}
				this.m_DirtyProps = DirtyProps.None;
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00017798 File Offset: 0x00015998
		private void UpdateMaterialPropertiesForCamera(Camera cam)
		{
			if (cam && this.m_Master)
			{
				this.MaterialChangeStart();
				this.SetMaterialProp(ShaderProperties.HD.TransformScale, this.m_Master.scalable ? this.m_Master.GetLossyScale() : Vector3.one);
				Vector3 normalized = base.transform.InverseTransformDirection(cam.transform.forward).normalized;
				this.SetMaterialProp(ShaderProperties.HD.CameraForwardOS, normalized);
				this.SetMaterialProp(ShaderProperties.HD.CameraForwardWS, cam.transform.forward);
				this.UpdateDirtyMaterialProperties();
				if (this.m_Master.colorMode == ColorMode.Gradient)
				{
					this.SetMaterialProp(ShaderProperties.ColorGradientMatrix, this.m_ColorGradientMatrix);
				}
				this.UpdateMatricesPropertiesForGPUInstancingSRP();
				this.MaterialChangeStop();
				cam.depthTextureMode |= DepthTextureMode.Depth;
			}
		}

		// Token: 0x040005FE RID: 1534
		private VolumetricLightBeamHD m_Master;

		// Token: 0x040005FF RID: 1535
		private VolumetricCookieHD m_Cookie;

		// Token: 0x04000600 RID: 1536
		private VolumetricShadowHD m_Shadow;

		// Token: 0x04000601 RID: 1537
		private Camera m_CurrentCameraRenderingSRP;

		// Token: 0x04000602 RID: 1538
		private DirtyProps m_DirtyProps;

		// Token: 0x02000116 RID: 278
		public enum InvalidTexture
		{
			// Token: 0x04000604 RID: 1540
			Null,
			// Token: 0x04000605 RID: 1541
			NoDepth
		}
	}
}
