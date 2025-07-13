using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200007A RID: 122
	public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000E67B File Offset: 0x0000C87B
		public override bool active
		{
			get
			{
				return base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000E6A8 File Offset: 0x0000C8A8
		public override DepthTextureMode GetCameraFlags()
		{
			BuiltinDebugViewsModel.Mode mode = base.model.settings.mode;
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			switch (mode)
			{
			case BuiltinDebugViewsModel.Mode.Depth:
				depthTextureMode |= DepthTextureMode.Depth;
				break;
			case BuiltinDebugViewsModel.Mode.Normals:
				depthTextureMode |= DepthTextureMode.DepthNormals;
				break;
			case BuiltinDebugViewsModel.Mode.MotionVectors:
				depthTextureMode |= (DepthTextureMode.Depth | DepthTextureMode.MotionVectors);
				break;
			}
			return depthTextureMode;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000E6EF File Offset: 0x0000C8EF
		public override CameraEvent GetCameraEvent()
		{
			if (base.model.settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors)
			{
				return CameraEvent.BeforeImageEffectsOpaque;
			}
			return CameraEvent.BeforeImageEffects;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000E709 File Offset: 0x0000C909
		public override string GetName()
		{
			return "Builtin Debug Views";
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000E710 File Offset: 0x0000C910
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			ref BuiltinDebugViewsModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			material.shaderKeywords = null;
			if (this.context.isGBufferAvailable)
			{
				material.EnableKeyword("SOURCE_GBUFFER");
			}
			switch (settings.mode)
			{
			case BuiltinDebugViewsModel.Mode.Depth:
				this.DepthPass(cb);
				break;
			case BuiltinDebugViewsModel.Mode.Normals:
				this.DepthNormalsPass(cb);
				break;
			case BuiltinDebugViewsModel.Mode.MotionVectors:
				this.MotionVectorsPass(cb);
				break;
			}
			this.context.Interrupt();
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000E7A0 File Offset: 0x0000C9A0
		private void DepthPass(CommandBuffer cb)
		{
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.DepthSettings depth = base.model.settings.depth;
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._DepthScale, 1f / depth.scale);
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 0);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000E7FC File Offset: 0x0000C9FC
		private void DepthNormalsPass(CommandBuffer cb)
		{
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 1);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000E830 File Offset: 0x0000CA30
		private void MotionVectorsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = base.model.settings.motionVectors;
			int nameID = BuiltinDebugViewsComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(nameID, this.context.width, this.context.height, 0, FilterMode.Bilinear);
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.sourceOpacity);
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, nameID, material, 2);
			if (motionVectors.motionImageOpacity > 0f && motionVectors.motionImageAmplitude > 0f)
			{
				int tempRT = BuiltinDebugViewsComponent.Uniforms._TempRT2;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Bilinear);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionImageOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionImageAmplitude);
				cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, nameID);
				cb.Blit(nameID, tempRT, material, 3);
				cb.ReleaseTemporaryRT(nameID);
				nameID = tempRT;
			}
			if (motionVectors.motionVectorsOpacity > 0f && motionVectors.motionVectorsAmplitude > 0f)
			{
				this.PrepareArrows();
				float num = 1f / (float)motionVectors.motionVectorsResolution;
				float x = num * (float)this.context.height / (float)this.context.width;
				cb.SetGlobalVector(BuiltinDebugViewsComponent.Uniforms._Scale, new Vector2(x, num));
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionVectorsOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
				cb.DrawMesh(this.m_Arrows.mesh, Matrix4x4.identity, material, 0, 4);
			}
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, nameID);
			cb.Blit(nameID, BuiltinRenderTextureType.CameraTarget);
			cb.ReleaseTemporaryRT(nameID);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000EA24 File Offset: 0x0000CC24
		private void PrepareArrows()
		{
			int motionVectorsResolution = base.model.settings.motionVectors.motionVectorsResolution;
			int num = motionVectorsResolution * Screen.width / Screen.height;
			if (this.m_Arrows == null)
			{
				this.m_Arrows = new BuiltinDebugViewsComponent.ArrowArray();
			}
			if (this.m_Arrows.columnCount != num || this.m_Arrows.rowCount != motionVectorsResolution)
			{
				this.m_Arrows.Release();
				this.m_Arrows.BuildMesh(num, motionVectorsResolution);
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000EA9C File Offset: 0x0000CC9C
		public override void OnDisable()
		{
			if (this.m_Arrows != null)
			{
				this.m_Arrows.Release();
			}
			this.m_Arrows = null;
		}

		// Token: 0x040002B7 RID: 695
		private const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";

		// Token: 0x040002B8 RID: 696
		private BuiltinDebugViewsComponent.ArrowArray m_Arrows;

		// Token: 0x0200007B RID: 123
		private static class Uniforms
		{
			// Token: 0x040002B9 RID: 697
			internal static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");

			// Token: 0x040002BA RID: 698
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x040002BB RID: 699
			internal static readonly int _Opacity = Shader.PropertyToID("_Opacity");

			// Token: 0x040002BC RID: 700
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x040002BD RID: 701
			internal static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");

			// Token: 0x040002BE RID: 702
			internal static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");

			// Token: 0x040002BF RID: 703
			internal static readonly int _Scale = Shader.PropertyToID("_Scale");
		}

		// Token: 0x0200007C RID: 124
		private enum Pass
		{
			// Token: 0x040002C1 RID: 705
			Depth,
			// Token: 0x040002C2 RID: 706
			Normals,
			// Token: 0x040002C3 RID: 707
			MovecOpacity,
			// Token: 0x040002C4 RID: 708
			MovecImaging,
			// Token: 0x040002C5 RID: 709
			MovecArrows
		}

		// Token: 0x0200007D RID: 125
		private class ArrowArray
		{
			// Token: 0x1700003B RID: 59
			// (get) Token: 0x0600028A RID: 650 RVA: 0x0000EB36 File Offset: 0x0000CD36
			// (set) Token: 0x0600028B RID: 651 RVA: 0x0000EB3E File Offset: 0x0000CD3E
			public Mesh mesh { get; private set; }

			// Token: 0x1700003C RID: 60
			// (get) Token: 0x0600028C RID: 652 RVA: 0x0000EB47 File Offset: 0x0000CD47
			// (set) Token: 0x0600028D RID: 653 RVA: 0x0000EB4F File Offset: 0x0000CD4F
			public int columnCount { get; private set; }

			// Token: 0x1700003D RID: 61
			// (get) Token: 0x0600028E RID: 654 RVA: 0x0000EB58 File Offset: 0x0000CD58
			// (set) Token: 0x0600028F RID: 655 RVA: 0x0000EB60 File Offset: 0x0000CD60
			public int rowCount { get; private set; }

			// Token: 0x06000290 RID: 656 RVA: 0x0000EB6C File Offset: 0x0000CD6C
			public void BuildMesh(int columns, int rows)
			{
				Vector3[] array = new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 1f, 0f)
				};
				int num = 6 * columns * rows;
				List<Vector3> list = new List<Vector3>(num);
				List<Vector2> list2 = new List<Vector2>(num);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						Vector2 item = new Vector2((0.5f + (float)j) / (float)columns, (0.5f + (float)i) / (float)rows);
						for (int k = 0; k < 6; k++)
						{
							list.Add(array[k]);
							list2.Add(item);
						}
					}
				}
				int[] array2 = new int[num];
				for (int l = 0; l < num; l++)
				{
					array2[l] = l;
				}
				this.mesh = new Mesh
				{
					hideFlags = HideFlags.DontSave
				};
				this.mesh.SetVertices(list);
				this.mesh.SetUVs(0, list2);
				this.mesh.SetIndices(array2, MeshTopology.Lines, 0);
				this.mesh.UploadMeshData(true);
				this.columnCount = columns;
				this.rowCount = rows;
			}

			// Token: 0x06000291 RID: 657 RVA: 0x0000ED0F File Offset: 0x0000CF0F
			public void Release()
			{
				GraphicsUtils.Destroy(this.mesh);
				this.mesh = null;
			}
		}
	}
}
