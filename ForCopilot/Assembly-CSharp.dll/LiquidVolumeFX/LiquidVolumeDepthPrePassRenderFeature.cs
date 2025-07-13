using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LiquidVolumeFX
{
	// Token: 0x0200018D RID: 397
	public class LiquidVolumeDepthPrePassRenderFeature : ScriptableRendererFeature
	{
		// Token: 0x06000832 RID: 2098 RVA: 0x00025EF4 File Offset: 0x000240F4
		public static void AddLiquidToBackRenderers(LiquidVolume lv)
		{
			if (lv == null || lv.topology != TOPOLOGY.Irregular || LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers.Contains(lv))
			{
				return;
			}
			LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers.Add(lv);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00025F22 File Offset: 0x00024122
		public static void RemoveLiquidFromBackRenderers(LiquidVolume lv)
		{
			if (lv == null || !LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers.Contains(lv))
			{
				return;
			}
			LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers.Remove(lv);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00025F47 File Offset: 0x00024147
		public static void AddLiquidToFrontRenderers(LiquidVolume lv)
		{
			if (lv == null || lv.topology != TOPOLOGY.Irregular || LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers.Contains(lv))
			{
				return;
			}
			LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers.Add(lv);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00025F75 File Offset: 0x00024175
		public static void RemoveLiquidFromFrontRenderers(LiquidVolume lv)
		{
			if (lv == null || !LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers.Contains(lv))
			{
				return;
			}
			LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers.Remove(lv);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00025F9C File Offset: 0x0002419C
		private void OnDestroy()
		{
			Shader.SetGlobalFloat(LiquidVolumeDepthPrePassRenderFeature.ShaderParams.ForcedInvisible, 0f);
			CoreUtils.Destroy(this.mat);
			if (this.backPass != null)
			{
				this.backPass.CleanUp();
			}
			if (this.frontPass != null)
			{
				this.frontPass.CleanUp();
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00025FEC File Offset: 0x000241EC
		public override void Create()
		{
			base.name = "Liquid Volume Depth PrePass";
			this.shader = Shader.Find("LiquidVolume/DepthPrePass");
			if (this.shader == null)
			{
				return;
			}
			this.mat = CoreUtils.CreateEngineMaterial(this.shader);
			this.backPass = new LiquidVolumeDepthPrePassRenderFeature.DepthPass(this.mat, LiquidVolumeDepthPrePassRenderFeature.Pass.BackBuffer, this.renderPassEvent);
			this.frontPass = new LiquidVolumeDepthPrePassRenderFeature.DepthPass(this.mat, LiquidVolumeDepthPrePassRenderFeature.Pass.FrontBuffer, this.renderPassEvent);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00026064 File Offset: 0x00024264
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			LiquidVolumeDepthPrePassRenderFeature.installed = true;
			if (this.backPass != null && LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers.Count > 0)
			{
				this.backPass.Setup(this, renderer);
				renderer.EnqueuePass(this.backPass);
			}
			if (this.frontPass != null && LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers.Count > 0)
			{
				this.frontPass.Setup(this, renderer);
				this.frontPass.renderer = renderer;
				renderer.EnqueuePass(this.frontPass);
			}
		}

		// Token: 0x04000913 RID: 2323
		public static readonly List<LiquidVolume> lvBackRenderers = new List<LiquidVolume>();

		// Token: 0x04000914 RID: 2324
		public static readonly List<LiquidVolume> lvFrontRenderers = new List<LiquidVolume>();

		// Token: 0x04000915 RID: 2325
		[SerializeField]
		[HideInInspector]
		private Shader shader;

		// Token: 0x04000916 RID: 2326
		public static bool installed;

		// Token: 0x04000917 RID: 2327
		private Material mat;

		// Token: 0x04000918 RID: 2328
		private LiquidVolumeDepthPrePassRenderFeature.DepthPass backPass;

		// Token: 0x04000919 RID: 2329
		private LiquidVolumeDepthPrePassRenderFeature.DepthPass frontPass;

		// Token: 0x0400091A RID: 2330
		[Tooltip("Renders each irregular liquid volume completely before rendering the next one.")]
		public bool interleavedRendering;

		// Token: 0x0400091B RID: 2331
		public RenderPassEvent renderPassEvent = 450;

		// Token: 0x0200018E RID: 398
		private static class ShaderParams
		{
			// Token: 0x0400091C RID: 2332
			public const string RTBackBufferName = "_VLBackBufferTexture";

			// Token: 0x0400091D RID: 2333
			public static int RTBackBuffer = Shader.PropertyToID("_VLBackBufferTexture");

			// Token: 0x0400091E RID: 2334
			public const string RTFrontBufferName = "_VLFrontBufferTexture";

			// Token: 0x0400091F RID: 2335
			public static int RTFrontBuffer = Shader.PropertyToID("_VLFrontBufferTexture");

			// Token: 0x04000920 RID: 2336
			public static int FlaskThickness = Shader.PropertyToID("_FlaskThickness");

			// Token: 0x04000921 RID: 2337
			public static int ForcedInvisible = Shader.PropertyToID("_LVForcedInvisible");

			// Token: 0x04000922 RID: 2338
			public const string SKW_FP_RENDER_TEXTURE = "LIQUID_VOLUME_FP_RENDER_TEXTURES";
		}

		// Token: 0x0200018F RID: 399
		private enum Pass
		{
			// Token: 0x04000924 RID: 2340
			BackBuffer,
			// Token: 0x04000925 RID: 2341
			FrontBuffer
		}

		// Token: 0x02000190 RID: 400
		private class DepthPass : ScriptableRenderPass
		{
			// Token: 0x0600083C RID: 2108 RVA: 0x00026148 File Offset: 0x00024348
			public DepthPass(Material mat, LiquidVolumeDepthPrePassRenderFeature.Pass pass, RenderPassEvent renderPassEvent)
			{
				base.renderPassEvent = renderPassEvent;
				this.mat = mat;
				this.passData.depthPass = this;
				if (pass == LiquidVolumeDepthPrePassRenderFeature.Pass.BackBuffer)
				{
					this.targetNameId = LiquidVolumeDepthPrePassRenderFeature.ShaderParams.RTBackBuffer;
					RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(this.targetNameId, 0, CubemapFace.Unknown, -1);
					this.targetRT = RTHandles.Alloc(renderTargetIdentifier, "_VLBackBufferTexture");
					this.passId = 0;
					this.lvRenderers = LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers;
					return;
				}
				if (pass != LiquidVolumeDepthPrePassRenderFeature.Pass.FrontBuffer)
				{
					return;
				}
				this.targetNameId = LiquidVolumeDepthPrePassRenderFeature.ShaderParams.RTFrontBuffer;
				RenderTargetIdentifier renderTargetIdentifier2 = new RenderTargetIdentifier(this.targetNameId, 0, CubemapFace.Unknown, -1);
				this.targetRT = RTHandles.Alloc(renderTargetIdentifier2, "_VLFrontBufferTexture");
				this.passId = 1;
				this.lvRenderers = LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers;
			}

			// Token: 0x0600083D RID: 2109 RVA: 0x00026205 File Offset: 0x00024405
			public void Setup(LiquidVolumeDepthPrePassRenderFeature feature, ScriptableRenderer renderer)
			{
				this.renderer = renderer;
				this.interleavedRendering = feature.interleavedRendering;
			}

			// Token: 0x0600083E RID: 2110 RVA: 0x0002621C File Offset: 0x0002441C
			private int SortByDistanceToCamera(LiquidVolume lv1, LiquidVolume lv2)
			{
				bool flag = lv1 == null;
				bool flag2 = lv2 == null;
				if (flag && flag2)
				{
					return 0;
				}
				if (flag2)
				{
					return 1;
				}
				if (flag)
				{
					return -1;
				}
				float num = Vector3.Distance(lv1.transform.position, LiquidVolumeDepthPrePassRenderFeature.DepthPass.currentCameraPosition);
				float num2 = Vector3.Distance(lv2.transform.position, LiquidVolumeDepthPrePassRenderFeature.DepthPass.currentCameraPosition);
				if (num < num2)
				{
					return 1;
				}
				if (num > num2)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x0600083F RID: 2111 RVA: 0x00026284 File Offset: 0x00024484
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				cameraTextureDescriptor.colorFormat = (LiquidVolume.useFPRenderTextures ? RenderTextureFormat.RHalf : RenderTextureFormat.ARGB32);
				cameraTextureDescriptor.sRGB = false;
				cameraTextureDescriptor.depthBufferBits = 16;
				cameraTextureDescriptor.msaaSamples = 1;
				cmd.GetTemporaryRT(this.targetNameId, cameraTextureDescriptor);
				if (!this.interleavedRendering)
				{
					base.ConfigureTarget(this.targetRT);
				}
				base.ConfigureInput(1);
			}

			// Token: 0x06000840 RID: 2112 RVA: 0x000262E8 File Offset: 0x000244E8
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				if (this.lvRenderers == null)
				{
					return;
				}
				CommandBuffer commandBuffer = CommandBufferPool.Get("LiquidVolumeDepthPrePass");
				commandBuffer.Clear();
				this.passData.cam = renderingData.cameraData.camera;
				this.passData.cmd = commandBuffer;
				this.passData.mat = this.mat;
				this.passData.cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				this.passData.source = this.renderer.cameraColorTargetHandle;
				this.passData.depth = this.renderer.cameraDepthTargetHandle;
				LiquidVolumeDepthPrePassRenderFeature.DepthPass.ExecutePass(this.passData);
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}

			// Token: 0x06000841 RID: 2113 RVA: 0x000263A0 File Offset: 0x000245A0
			private static void ExecutePass(LiquidVolumeDepthPrePassRenderFeature.DepthPass.PassData passData)
			{
				CommandBuffer cmd = passData.cmd;
				cmd.SetGlobalFloat(LiquidVolumeDepthPrePassRenderFeature.ShaderParams.ForcedInvisible, 0f);
				Camera cam = passData.cam;
				LiquidVolumeDepthPrePassRenderFeature.DepthPass depthPass = passData.depthPass;
				RenderTextureDescriptor cameraTargetDescriptor = passData.cameraTargetDescriptor;
				cameraTargetDescriptor.colorFormat = (LiquidVolume.useFPRenderTextures ? RenderTextureFormat.RHalf : RenderTextureFormat.ARGB32);
				cameraTargetDescriptor.sRGB = false;
				cameraTargetDescriptor.depthBufferBits = 16;
				cameraTargetDescriptor.msaaSamples = 1;
				cmd.GetTemporaryRT(depthPass.targetNameId, cameraTargetDescriptor);
				int count = depthPass.lvRenderers.Count;
				if (depthPass.interleavedRendering)
				{
					RenderTargetIdentifier rt = new RenderTargetIdentifier(depthPass.targetNameId, 0, CubemapFace.Unknown, -1);
					LiquidVolumeDepthPrePassRenderFeature.DepthPass.currentCameraPosition = cam.transform.position;
					depthPass.lvRenderers.Sort(new Comparison<LiquidVolume>(depthPass.SortByDistanceToCamera));
					for (int i = 0; i < count; i++)
					{
						LiquidVolume liquidVolume = depthPass.lvRenderers[i];
						if (liquidVolume != null && liquidVolume.isActiveAndEnabled)
						{
							if (liquidVolume.topology == TOPOLOGY.Irregular)
							{
								cmd.SetRenderTarget(rt, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
								if (LiquidVolume.useFPRenderTextures)
								{
									cmd.ClearRenderTarget(true, true, new Color(cam.farClipPlane, 0f, 0f, 0f), 1f);
									cmd.EnableShaderKeyword("LIQUID_VOLUME_FP_RENDER_TEXTURES");
								}
								else
								{
									cmd.ClearRenderTarget(true, true, new Color(0.9882353f, 0.4470558f, 0.75f, 0f), 1f);
									cmd.DisableShaderKeyword("LIQUID_VOLUME_FP_RENDER_TEXTURES");
								}
								cmd.SetGlobalFloat(LiquidVolumeDepthPrePassRenderFeature.ShaderParams.FlaskThickness, 1f - liquidVolume.flaskThickness);
								cmd.DrawRenderer(liquidVolume.mr, passData.mat, (liquidVolume.subMeshIndex >= 0) ? liquidVolume.subMeshIndex : 0, depthPass.passId);
							}
							RenderTargetIdentifier color = new RenderTargetIdentifier(passData.source, 0, CubemapFace.Unknown, -1);
							RenderTargetIdentifier depth = new RenderTargetIdentifier(passData.depth, 0, CubemapFace.Unknown, -1);
							cmd.SetRenderTarget(color, depth);
							cmd.DrawRenderer(liquidVolume.mr, liquidVolume.liqMat, (liquidVolume.subMeshIndex >= 0) ? liquidVolume.subMeshIndex : 0, 1);
						}
					}
					cmd.SetGlobalFloat(LiquidVolumeDepthPrePassRenderFeature.ShaderParams.ForcedInvisible, 1f);
					return;
				}
				RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(depthPass.targetNameId, 0, CubemapFace.Unknown, -1);
				cmd.SetRenderTarget(renderTargetIdentifier);
				cmd.SetGlobalTexture(depthPass.targetNameId, renderTargetIdentifier);
				if (LiquidVolume.useFPRenderTextures)
				{
					cmd.ClearRenderTarget(true, true, new Color(cam.farClipPlane, 0f, 0f, 0f), 1f);
					cmd.EnableShaderKeyword("LIQUID_VOLUME_FP_RENDER_TEXTURES");
				}
				else
				{
					cmd.ClearRenderTarget(true, true, new Color(0.9882353f, 0.4470558f, 0.75f, 0f), 1f);
					cmd.DisableShaderKeyword("LIQUID_VOLUME_FP_RENDER_TEXTURES");
				}
				for (int j = 0; j < count; j++)
				{
					LiquidVolume liquidVolume2 = depthPass.lvRenderers[j];
					if (liquidVolume2 != null && liquidVolume2.isActiveAndEnabled)
					{
						cmd.SetGlobalFloat(LiquidVolumeDepthPrePassRenderFeature.ShaderParams.FlaskThickness, 1f - liquidVolume2.flaskThickness);
						cmd.DrawRenderer(liquidVolume2.mr, passData.mat, (liquidVolume2.subMeshIndex >= 0) ? liquidVolume2.subMeshIndex : 0, depthPass.passId);
					}
				}
			}

			// Token: 0x06000842 RID: 2114 RVA: 0x000266E1 File Offset: 0x000248E1
			public void CleanUp()
			{
				RTHandles.Release(this.targetRT);
			}

			// Token: 0x04000926 RID: 2342
			private const string profilerTag = "LiquidVolumeDepthPrePass";

			// Token: 0x04000927 RID: 2343
			private Material mat;

			// Token: 0x04000928 RID: 2344
			private int targetNameId;

			// Token: 0x04000929 RID: 2345
			private RTHandle targetRT;

			// Token: 0x0400092A RID: 2346
			private int passId;

			// Token: 0x0400092B RID: 2347
			private List<LiquidVolume> lvRenderers;

			// Token: 0x0400092C RID: 2348
			public ScriptableRenderer renderer;

			// Token: 0x0400092D RID: 2349
			public bool interleavedRendering;

			// Token: 0x0400092E RID: 2350
			private static Vector3 currentCameraPosition;

			// Token: 0x0400092F RID: 2351
			private readonly LiquidVolumeDepthPrePassRenderFeature.DepthPass.PassData passData = new LiquidVolumeDepthPrePassRenderFeature.DepthPass.PassData();

			// Token: 0x02000191 RID: 401
			private class PassData
			{
				// Token: 0x04000930 RID: 2352
				public Camera cam;

				// Token: 0x04000931 RID: 2353
				public CommandBuffer cmd;

				// Token: 0x04000932 RID: 2354
				public LiquidVolumeDepthPrePassRenderFeature.DepthPass depthPass;

				// Token: 0x04000933 RID: 2355
				public Material mat;

				// Token: 0x04000934 RID: 2356
				public RTHandle source;

				// Token: 0x04000935 RID: 2357
				public RTHandle depth;

				// Token: 0x04000936 RID: 2358
				public RenderTextureDescriptor cameraTargetDescriptor;
			}
		}
	}
}
