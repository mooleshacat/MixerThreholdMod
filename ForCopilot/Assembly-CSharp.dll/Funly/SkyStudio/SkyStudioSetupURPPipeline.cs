using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Funly.SkyStudio
{
	// Token: 0x020001F5 RID: 501
	[ExecuteInEditMode]
	public class SkyStudioSetupURPPipeline : MonoBehaviour
	{
		// Token: 0x06000B1E RID: 2846 RVA: 0x00030E01 File Offset: 0x0002F001
		private void Update()
		{
			if (this.pipelineAsset != GraphicsSettings.renderPipelineAsset)
			{
				GraphicsSettings.renderPipelineAsset = this.pipelineAsset;
				QualitySettings.renderPipeline = this.pipelineAsset;
			}
		}

		// Token: 0x04000BD9 RID: 3033
		[HelpBox("For URP projects, Sky Studio will assign this rendering pipeline into GraphicsSettings. We have to install this pipeline so that we can embed our own custom render features, which are required for certain Sky Studio features like rain splashes to work properly. If you need to add rendering features, or customize the rendering pipeline asset please update this reference, and ensure that the 'SkyStudio-WeatherDepthForwardRenderer' is assigned to render feature index 1. Feel free to add any custom render features after index 1.", HelpBoxMessageType.Info)]
		[Tooltip("The rendering pipeline that will be assigned into the graphics settings when this scene becomes active.")]
		public RenderPipelineAsset pipelineAsset;
	}
}
