using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace VLB
{
	// Token: 0x02000157 RID: 343
	public static class SRPHelper
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x0001D4E9 File Offset: 0x0001B6E9
		public static string renderPipelineScriptingDefineSymbolAsString
		{
			get
			{
				return "VLB_URP";
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0001D4F0 File Offset: 0x0001B6F0
		public static RenderPipeline projectRenderPipeline
		{
			get
			{
				if (!SRPHelper.m_IsRenderPipelineCached)
				{
					SRPHelper.m_RenderPipelineCached = SRPHelper.ComputeRenderPipeline();
					SRPHelper.m_IsRenderPipelineCached = true;
				}
				return SRPHelper.m_RenderPipelineCached;
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001D510 File Offset: 0x0001B710
		private static RenderPipeline ComputeRenderPipeline()
		{
			RenderPipelineAsset renderPipelineAsset = GraphicsSettings.renderPipelineAsset;
			if (renderPipelineAsset)
			{
				string text = renderPipelineAsset.GetType().ToString();
				if (text.Contains("Universal"))
				{
					return RenderPipeline.URP;
				}
				if (text.Contains("Lightweight"))
				{
					return RenderPipeline.URP;
				}
				if (text.Contains("HD"))
				{
					return RenderPipeline.HDRP;
				}
			}
			return RenderPipeline.BuiltIn;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001D565 File Offset: 0x0001B765
		public static bool IsUsingCustomRenderPipeline()
		{
			return RenderPipelineManager.currentPipeline != null || GraphicsSettings.renderPipelineAsset != null;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001D57B File Offset: 0x0001B77B
		public static void RegisterOnBeginCameraRendering(Action<ScriptableRenderContext, Camera> cb)
		{
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				RenderPipelineManager.beginCameraRendering -= cb;
				RenderPipelineManager.beginCameraRendering += cb;
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001D590 File Offset: 0x0001B790
		public static void UnregisterOnBeginCameraRendering(Action<ScriptableRenderContext, Camera> cb)
		{
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				RenderPipelineManager.beginCameraRendering -= cb;
			}
		}

		// Token: 0x04000763 RID: 1891
		private static bool m_IsRenderPipelineCached;

		// Token: 0x04000764 RID: 1892
		private static RenderPipeline m_RenderPipelineCached;
	}
}
