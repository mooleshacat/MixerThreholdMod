using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000099 RID: 153
	public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600030A RID: 778 RVA: 0x00012BF4 File Offset: 0x00010DF4
		public override bool active
		{
			get
			{
				UserLutModel.Settings settings = base.model.settings;
				return base.model.enabled && settings.lut != null && settings.contribution > 0f && settings.lut.height == (int)Mathf.Sqrt((float)settings.lut.width) && !this.context.interrupted;
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00012C64 File Offset: 0x00010E64
		public override void Prepare(Material uberMaterial)
		{
			UserLutModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("USER_LUT");
			uberMaterial.SetTexture(UserLutComponent.Uniforms._UserLut, settings.lut);
			uberMaterial.SetVector(UserLutComponent.Uniforms._UserLut_Params, new Vector4(1f / (float)settings.lut.width, 1f / (float)settings.lut.height, (float)settings.lut.height - 1f, settings.contribution));
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00012CE8 File Offset: 0x00010EE8
		public void OnGUI()
		{
			UserLutModel.Settings settings = base.model.settings;
			GUI.DrawTexture(new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)settings.lut.width, (float)settings.lut.height), settings.lut);
		}

		// Token: 0x0200009A RID: 154
		private static class Uniforms
		{
			// Token: 0x04000385 RID: 901
			internal static readonly int _UserLut = Shader.PropertyToID("_UserLut");

			// Token: 0x04000386 RID: 902
			internal static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");
		}
	}
}
