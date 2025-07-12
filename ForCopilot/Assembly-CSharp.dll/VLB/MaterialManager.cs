using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace VLB
{
	// Token: 0x02000121 RID: 289
	public static class MaterialManager
	{
		// Token: 0x06000511 RID: 1297 RVA: 0x00018F21 File Offset: 0x00017121
		public static Material NewMaterialPersistent(Shader shader, bool gpuInstanced)
		{
			if (!shader)
			{
				Debug.LogError("Invalid VLB Shader. Please try to reset the VLB Config asset or reinstall the plugin.");
				return null;
			}
			Material material = new Material(shader);
			BatchingHelper.SetMaterialProperties(material, gpuInstanced);
			return material;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00018F44 File Offset: 0x00017144
		public static Material GetInstancedMaterial(uint groupID, ref MaterialManager.StaticPropertiesSD staticProps)
		{
			MaterialManager.IStaticProperties staticProperties = staticProps;
			return MaterialManager.GetInstancedMaterial(MaterialManager.ms_MaterialsGroupSD, groupID, ref staticProperties);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00018F6C File Offset: 0x0001716C
		public static Material GetInstancedMaterial(uint groupID, ref MaterialManager.StaticPropertiesHD staticProps)
		{
			MaterialManager.IStaticProperties staticProperties = staticProps;
			return MaterialManager.GetInstancedMaterial(MaterialManager.ms_MaterialsGroupHD, groupID, ref staticProperties);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00018F94 File Offset: 0x00017194
		private static Material GetInstancedMaterial(Hashtable groups, uint groupID, ref MaterialManager.IStaticProperties staticProps)
		{
			MaterialManager.MaterialsGroup materialsGroup = (MaterialManager.MaterialsGroup)groups[groupID];
			if (materialsGroup == null)
			{
				materialsGroup = new MaterialManager.MaterialsGroup(staticProps.GetPropertiesCount());
				groups[groupID] = materialsGroup;
			}
			int materialID = staticProps.GetMaterialID();
			Material material = materialsGroup.materials[materialID];
			if (material == null)
			{
				material = Config.Instance.NewMaterialTransient(staticProps.GetShaderMode(), true);
				if (material)
				{
					materialsGroup.materials[materialID] = material;
					staticProps.ApplyToMaterial(material);
				}
			}
			return material;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00019017 File Offset: 0x00017217
		private static void SetBlendingMode(this Material mat, int nameID, BlendMode value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00019017 File Offset: 0x00017217
		private static void SetStencilRef(this Material mat, int nameID, int value)
		{
			mat.SetInt(nameID, value);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00019017 File Offset: 0x00017217
		private static void SetStencilComp(this Material mat, int nameID, CompareFunction value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00019017 File Offset: 0x00017217
		private static void SetStencilOp(this Material mat, int nameID, StencilOp value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00019017 File Offset: 0x00017217
		private static void SetCull(this Material mat, int nameID, CullMode value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00019017 File Offset: 0x00017217
		private static void SetZWrite(this Material mat, int nameID, MaterialManager.ZWrite value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00019017 File Offset: 0x00017217
		private static void SetZTest(this Material mat, int nameID, CompareFunction value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00019024 File Offset: 0x00017224
		// Note: this type is marked as 'beforefieldinit'.
		static MaterialManager()
		{
			BlendMode[] array = new BlendMode[3];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.F186F2262AE48F2AA4F90C9A6B35913B0F6B0B895423B6267252259BFD357D3B).FieldHandle);
			MaterialManager.BlendingMode_SrcFactor = array;
			BlendMode[] array2 = new BlendMode[3];
			RuntimeHelpers.InitializeArray(array2, fieldof(<PrivateImplementationDetails>.0A0EC6D4742068B4D88C6145B8224EF1DC240C8A305CDFC50C3AAF9121E6875D).FieldHandle);
			MaterialManager.BlendingMode_DstFactor = array2;
			bool[] array3 = new bool[3];
			array3[0] = true;
			array3[1] = true;
			MaterialManager.BlendingMode_AlphaAsBlack = array3;
			MaterialManager.ms_MaterialsGroupSD = new Hashtable(1);
			MaterialManager.ms_MaterialsGroupHD = new Hashtable(1);
		}

		// Token: 0x0400064B RID: 1611
		public static MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400064C RID: 1612
		private static readonly BlendMode[] BlendingMode_SrcFactor;

		// Token: 0x0400064D RID: 1613
		private static readonly BlendMode[] BlendingMode_DstFactor;

		// Token: 0x0400064E RID: 1614
		private static readonly bool[] BlendingMode_AlphaAsBlack;

		// Token: 0x0400064F RID: 1615
		private static Hashtable ms_MaterialsGroupSD;

		// Token: 0x04000650 RID: 1616
		private static Hashtable ms_MaterialsGroupHD;

		// Token: 0x02000122 RID: 290
		public enum BlendingMode
		{
			// Token: 0x04000652 RID: 1618
			Additive,
			// Token: 0x04000653 RID: 1619
			SoftAdditive,
			// Token: 0x04000654 RID: 1620
			TraditionalTransparency,
			// Token: 0x04000655 RID: 1621
			Count
		}

		// Token: 0x02000123 RID: 291
		public enum ColorGradient
		{
			// Token: 0x04000657 RID: 1623
			Off,
			// Token: 0x04000658 RID: 1624
			MatrixLow,
			// Token: 0x04000659 RID: 1625
			MatrixHigh,
			// Token: 0x0400065A RID: 1626
			Count
		}

		// Token: 0x02000124 RID: 292
		public enum Noise3D
		{
			// Token: 0x0400065C RID: 1628
			Off,
			// Token: 0x0400065D RID: 1629
			On,
			// Token: 0x0400065E RID: 1630
			Count
		}

		// Token: 0x02000125 RID: 293
		public static class SD
		{
			// Token: 0x02000126 RID: 294
			public enum DepthBlend
			{
				// Token: 0x04000660 RID: 1632
				Off,
				// Token: 0x04000661 RID: 1633
				On,
				// Token: 0x04000662 RID: 1634
				Count
			}

			// Token: 0x02000127 RID: 295
			public enum DynamicOcclusion
			{
				// Token: 0x04000664 RID: 1636
				Off,
				// Token: 0x04000665 RID: 1637
				ClippingPlane,
				// Token: 0x04000666 RID: 1638
				DepthTexture,
				// Token: 0x04000667 RID: 1639
				Count
			}

			// Token: 0x02000128 RID: 296
			public enum MeshSkewing
			{
				// Token: 0x04000669 RID: 1641
				Off,
				// Token: 0x0400066A RID: 1642
				On,
				// Token: 0x0400066B RID: 1643
				Count
			}

			// Token: 0x02000129 RID: 297
			public enum ShaderAccuracy
			{
				// Token: 0x0400066D RID: 1645
				Fast,
				// Token: 0x0400066E RID: 1646
				High,
				// Token: 0x0400066F RID: 1647
				Count
			}
		}

		// Token: 0x0200012A RID: 298
		public static class HD
		{
			// Token: 0x0200012B RID: 299
			public enum Attenuation
			{
				// Token: 0x04000671 RID: 1649
				Linear,
				// Token: 0x04000672 RID: 1650
				Quadratic,
				// Token: 0x04000673 RID: 1651
				Count
			}

			// Token: 0x0200012C RID: 300
			public enum Shadow
			{
				// Token: 0x04000675 RID: 1653
				Off,
				// Token: 0x04000676 RID: 1654
				On,
				// Token: 0x04000677 RID: 1655
				Count
			}

			// Token: 0x0200012D RID: 301
			public enum Cookie
			{
				// Token: 0x04000679 RID: 1657
				Off,
				// Token: 0x0400067A RID: 1658
				SingleChannel,
				// Token: 0x0400067B RID: 1659
				RGBA,
				// Token: 0x0400067C RID: 1660
				Count
			}
		}

		// Token: 0x0200012E RID: 302
		private interface IStaticProperties
		{
			// Token: 0x0600051D RID: 1309
			int GetPropertiesCount();

			// Token: 0x0600051E RID: 1310
			int GetMaterialID();

			// Token: 0x0600051F RID: 1311
			void ApplyToMaterial(Material mat);

			// Token: 0x06000520 RID: 1312
			ShaderMode GetShaderMode();
		}

		// Token: 0x0200012F RID: 303
		public struct StaticPropertiesSD : MaterialManager.IStaticProperties
		{
			// Token: 0x06000521 RID: 1313 RVA: 0x00014B5A File Offset: 0x00012D5A
			public ShaderMode GetShaderMode()
			{
				return ShaderMode.SD;
			}

			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x06000522 RID: 1314 RVA: 0x00019090 File Offset: 0x00017290
			public static int staticPropertiesCount
			{
				get
				{
					return 432;
				}
			}

			// Token: 0x06000523 RID: 1315 RVA: 0x00019097 File Offset: 0x00017297
			public int GetPropertiesCount()
			{
				return MaterialManager.StaticPropertiesSD.staticPropertiesCount;
			}

			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x06000524 RID: 1316 RVA: 0x0001909E File Offset: 0x0001729E
			private int blendingModeID
			{
				get
				{
					return (int)this.blendingMode;
				}
			}

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x06000525 RID: 1317 RVA: 0x000190A6 File Offset: 0x000172A6
			private int noise3DID
			{
				get
				{
					if (!Config.Instance.featureEnabledNoise3D)
					{
						return 0;
					}
					return (int)this.noise3D;
				}
			}

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x06000526 RID: 1318 RVA: 0x000190BC File Offset: 0x000172BC
			private int depthBlendID
			{
				get
				{
					if (!Config.Instance.featureEnabledDepthBlend)
					{
						return 0;
					}
					return (int)this.depthBlend;
				}
			}

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x06000527 RID: 1319 RVA: 0x000190D2 File Offset: 0x000172D2
			private int colorGradientID
			{
				get
				{
					if (Config.Instance.featureEnabledColorGradient == FeatureEnabledColorGradient.Off)
					{
						return 0;
					}
					return (int)this.colorGradient;
				}
			}

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x06000528 RID: 1320 RVA: 0x000190E8 File Offset: 0x000172E8
			private int dynamicOcclusionID
			{
				get
				{
					if (!Config.Instance.featureEnabledDynamicOcclusion)
					{
						return 0;
					}
					return (int)this.dynamicOcclusion;
				}
			}

			// Token: 0x170000EE RID: 238
			// (get) Token: 0x06000529 RID: 1321 RVA: 0x000190FE File Offset: 0x000172FE
			private int meshSkewingID
			{
				get
				{
					if (!Config.Instance.featureEnabledMeshSkewing)
					{
						return 0;
					}
					return (int)this.meshSkewing;
				}
			}

			// Token: 0x170000EF RID: 239
			// (get) Token: 0x0600052A RID: 1322 RVA: 0x00019114 File Offset: 0x00017314
			private int shaderAccuracyID
			{
				get
				{
					if (!Config.Instance.featureEnabledShaderAccuracyHigh)
					{
						return 0;
					}
					return (int)this.shaderAccuracy;
				}
			}

			// Token: 0x0600052B RID: 1323 RVA: 0x0001912A File Offset: 0x0001732A
			public int GetMaterialID()
			{
				return (((((this.blendingModeID * 2 + this.noise3DID) * 2 + this.depthBlendID) * 3 + this.colorGradientID) * 3 + this.dynamicOcclusionID) * 2 + this.meshSkewingID) * 2 + this.shaderAccuracyID;
			}

			// Token: 0x0600052C RID: 1324 RVA: 0x00019168 File Offset: 0x00017368
			public void ApplyToMaterial(Material mat)
			{
				mat.SetKeywordEnabled("VLB_ALPHA_AS_BLACK", MaterialManager.BlendingMode_AlphaAsBlack[(int)this.blendingMode]);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_LOW", this.colorGradient == MaterialManager.ColorGradient.MatrixLow);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_HIGH", this.colorGradient == MaterialManager.ColorGradient.MatrixHigh);
				mat.SetKeywordEnabled("VLB_DEPTH_BLEND", this.depthBlend == MaterialManager.SD.DepthBlend.On);
				mat.SetKeywordEnabled("VLB_NOISE_3D", this.noise3D == MaterialManager.Noise3D.On);
				mat.SetKeywordEnabled("VLB_OCCLUSION_CLIPPING_PLANE", this.dynamicOcclusion == MaterialManager.SD.DynamicOcclusion.ClippingPlane);
				mat.SetKeywordEnabled("VLB_OCCLUSION_DEPTH_TEXTURE", this.dynamicOcclusion == MaterialManager.SD.DynamicOcclusion.DepthTexture);
				mat.SetKeywordEnabled("VLB_MESH_SKEWING", this.meshSkewing == MaterialManager.SD.MeshSkewing.On);
				mat.SetKeywordEnabled("VLB_SHADER_ACCURACY_HIGH", this.shaderAccuracy == MaterialManager.SD.ShaderAccuracy.High);
				mat.SetBlendingMode(ShaderProperties.BlendSrcFactor, MaterialManager.BlendingMode_SrcFactor[(int)this.blendingMode]);
				mat.SetBlendingMode(ShaderProperties.BlendDstFactor, MaterialManager.BlendingMode_DstFactor[(int)this.blendingMode]);
				mat.SetZTest(ShaderProperties.ZTest, CompareFunction.LessEqual);
			}

			// Token: 0x0400067D RID: 1661
			public MaterialManager.BlendingMode blendingMode;

			// Token: 0x0400067E RID: 1662
			public MaterialManager.Noise3D noise3D;

			// Token: 0x0400067F RID: 1663
			public MaterialManager.SD.DepthBlend depthBlend;

			// Token: 0x04000680 RID: 1664
			public MaterialManager.ColorGradient colorGradient;

			// Token: 0x04000681 RID: 1665
			public MaterialManager.SD.DynamicOcclusion dynamicOcclusion;

			// Token: 0x04000682 RID: 1666
			public MaterialManager.SD.MeshSkewing meshSkewing;

			// Token: 0x04000683 RID: 1667
			public MaterialManager.SD.ShaderAccuracy shaderAccuracy;
		}

		// Token: 0x02000130 RID: 304
		public struct StaticPropertiesHD : MaterialManager.IStaticProperties
		{
			// Token: 0x0600052D RID: 1325 RVA: 0x000022C9 File Offset: 0x000004C9
			public ShaderMode GetShaderMode()
			{
				return ShaderMode.HD;
			}

			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x0600052E RID: 1326 RVA: 0x00019266 File Offset: 0x00017466
			public static int staticPropertiesCount
			{
				get
				{
					return 216 * Config.Instance.raymarchingQualitiesCount;
				}
			}

			// Token: 0x0600052F RID: 1327 RVA: 0x00019278 File Offset: 0x00017478
			public int GetPropertiesCount()
			{
				return MaterialManager.StaticPropertiesHD.staticPropertiesCount;
			}

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x06000530 RID: 1328 RVA: 0x0001927F File Offset: 0x0001747F
			private int blendingModeID
			{
				get
				{
					return (int)this.blendingMode;
				}
			}

			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x06000531 RID: 1329 RVA: 0x00019287 File Offset: 0x00017487
			private int attenuationID
			{
				get
				{
					return (int)this.attenuation;
				}
			}

			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x06000532 RID: 1330 RVA: 0x0001928F File Offset: 0x0001748F
			private int noise3DID
			{
				get
				{
					if (!Config.Instance.featureEnabledNoise3D)
					{
						return 0;
					}
					return (int)this.noise3D;
				}
			}

			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x06000533 RID: 1331 RVA: 0x000192A5 File Offset: 0x000174A5
			private int colorGradientID
			{
				get
				{
					if (Config.Instance.featureEnabledColorGradient == FeatureEnabledColorGradient.Off)
					{
						return 0;
					}
					return (int)this.colorGradient;
				}
			}

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x06000534 RID: 1332 RVA: 0x000192BB File Offset: 0x000174BB
			private int dynamicOcclusionID
			{
				get
				{
					if (!Config.Instance.featureEnabledShadow)
					{
						return 0;
					}
					return (int)this.shadow;
				}
			}

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x06000535 RID: 1333 RVA: 0x000192D1 File Offset: 0x000174D1
			private int cookieID
			{
				get
				{
					if (!Config.Instance.featureEnabledCookie)
					{
						return 0;
					}
					return (int)this.cookie;
				}
			}

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x06000536 RID: 1334 RVA: 0x000192E7 File Offset: 0x000174E7
			private int raymarchingQualityID
			{
				get
				{
					return this.raymarchingQualityIndex;
				}
			}

			// Token: 0x06000537 RID: 1335 RVA: 0x000192F0 File Offset: 0x000174F0
			public int GetMaterialID()
			{
				return (((((this.blendingModeID * 2 + this.attenuationID) * 2 + this.noise3DID) * 3 + this.colorGradientID) * 2 + this.dynamicOcclusionID) * 3 + this.cookieID) * Config.Instance.raymarchingQualitiesCount + this.raymarchingQualityID;
			}

			// Token: 0x06000538 RID: 1336 RVA: 0x00019344 File Offset: 0x00017544
			public void ApplyToMaterial(Material mat)
			{
				mat.SetKeywordEnabled("VLB_ALPHA_AS_BLACK", MaterialManager.BlendingMode_AlphaAsBlack[(int)this.blendingMode]);
				mat.SetKeywordEnabled("VLB_ATTENUATION_LINEAR", this.attenuation == MaterialManager.HD.Attenuation.Linear);
				mat.SetKeywordEnabled("VLB_ATTENUATION_QUAD", this.attenuation == MaterialManager.HD.Attenuation.Quadratic);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_LOW", this.colorGradient == MaterialManager.ColorGradient.MatrixLow);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_HIGH", this.colorGradient == MaterialManager.ColorGradient.MatrixHigh);
				mat.SetKeywordEnabled("VLB_NOISE_3D", this.noise3D == MaterialManager.Noise3D.On);
				mat.SetKeywordEnabled("VLB_SHADOW", this.shadow == MaterialManager.HD.Shadow.On);
				mat.SetKeywordEnabled("VLB_COOKIE_1CHANNEL", this.cookie == MaterialManager.HD.Cookie.SingleChannel);
				mat.SetKeywordEnabled("VLB_COOKIE_RGBA", this.cookie == MaterialManager.HD.Cookie.RGBA);
				for (int i = 0; i < Config.Instance.raymarchingQualitiesCount; i++)
				{
					mat.SetKeywordEnabled(ShaderKeywords.HD.GetRaymarchingQuality(i), this.raymarchingQualityIndex == i);
				}
				mat.SetBlendingMode(ShaderProperties.BlendSrcFactor, MaterialManager.BlendingMode_SrcFactor[(int)this.blendingMode]);
				mat.SetBlendingMode(ShaderProperties.BlendDstFactor, MaterialManager.BlendingMode_DstFactor[(int)this.blendingMode]);
				mat.SetZTest(ShaderProperties.ZTest, CompareFunction.Always);
			}

			// Token: 0x04000684 RID: 1668
			public MaterialManager.BlendingMode blendingMode;

			// Token: 0x04000685 RID: 1669
			public MaterialManager.HD.Attenuation attenuation;

			// Token: 0x04000686 RID: 1670
			public MaterialManager.Noise3D noise3D;

			// Token: 0x04000687 RID: 1671
			public MaterialManager.ColorGradient colorGradient;

			// Token: 0x04000688 RID: 1672
			public MaterialManager.HD.Shadow shadow;

			// Token: 0x04000689 RID: 1673
			public MaterialManager.HD.Cookie cookie;

			// Token: 0x0400068A RID: 1674
			public int raymarchingQualityIndex;
		}

		// Token: 0x02000131 RID: 305
		private class MaterialsGroup
		{
			// Token: 0x06000539 RID: 1337 RVA: 0x0001946C File Offset: 0x0001766C
			public MaterialsGroup(int count)
			{
				this.materials = new Material[count];
			}

			// Token: 0x0400068B RID: 1675
			public Material[] materials;
		}

		// Token: 0x02000132 RID: 306
		private enum ZWrite
		{
			// Token: 0x0400068D RID: 1677
			Off,
			// Token: 0x0400068E RID: 1678
			On
		}
	}
}
