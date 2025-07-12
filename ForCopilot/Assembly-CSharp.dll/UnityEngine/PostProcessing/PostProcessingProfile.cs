using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000DF RID: 223
	public class PostProcessingProfile : ScriptableObject
	{
		// Token: 0x0400047F RID: 1151
		public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();

		// Token: 0x04000480 RID: 1152
		public FogModel fog = new FogModel();

		// Token: 0x04000481 RID: 1153
		public AntialiasingModel antialiasing = new AntialiasingModel();

		// Token: 0x04000482 RID: 1154
		public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();

		// Token: 0x04000483 RID: 1155
		public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();

		// Token: 0x04000484 RID: 1156
		public DepthOfFieldModel depthOfField = new DepthOfFieldModel();

		// Token: 0x04000485 RID: 1157
		public MotionBlurModel motionBlur = new MotionBlurModel();

		// Token: 0x04000486 RID: 1158
		public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();

		// Token: 0x04000487 RID: 1159
		public BloomModel bloom = new BloomModel();

		// Token: 0x04000488 RID: 1160
		public ColorGradingModel colorGrading = new ColorGradingModel();

		// Token: 0x04000489 RID: 1161
		public UserLutModel userLut = new UserLutModel();

		// Token: 0x0400048A RID: 1162
		public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();

		// Token: 0x0400048B RID: 1163
		public GrainModel grain = new GrainModel();

		// Token: 0x0400048C RID: 1164
		public VignetteModel vignette = new VignetteModel();

		// Token: 0x0400048D RID: 1165
		public DitheringModel dithering = new DitheringModel();
	}
}
