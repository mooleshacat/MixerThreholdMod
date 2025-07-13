using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000084 RID: 132
	public sealed class DitheringComponent : PostProcessingComponentRenderTexture<DitheringModel>
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0001028E File Offset: 0x0000E48E
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000102AD File Offset: 0x0000E4AD
		public override void OnDisable()
		{
			this.noiseTextures = null;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x000102B8 File Offset: 0x0000E4B8
		private void LoadNoiseTextures()
		{
			this.noiseTextures = new Texture2D[64];
			for (int i = 0; i < 64; i++)
			{
				this.noiseTextures[i] = Resources.Load<Texture2D>("Bluenoise64/LDR_LLL1_" + i.ToString());
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00010300 File Offset: 0x0000E500
		public override void Prepare(Material uberMaterial)
		{
			int num = this.textureIndex + 1;
			this.textureIndex = num;
			if (num >= 64)
			{
				this.textureIndex = 0;
			}
			float value = Random.value;
			float value2 = Random.value;
			if (this.noiseTextures == null)
			{
				this.LoadNoiseTextures();
			}
			Texture2D texture2D = this.noiseTextures[this.textureIndex];
			uberMaterial.EnableKeyword("DITHERING");
			uberMaterial.SetTexture(DitheringComponent.Uniforms._DitheringTex, texture2D);
			uberMaterial.SetVector(DitheringComponent.Uniforms._DitheringCoords, new Vector4((float)this.context.width / (float)texture2D.width, (float)this.context.height / (float)texture2D.height, value, value2));
		}

		// Token: 0x040002F3 RID: 755
		private Texture2D[] noiseTextures;

		// Token: 0x040002F4 RID: 756
		private int textureIndex;

		// Token: 0x040002F5 RID: 757
		private const int k_TextureCount = 64;

		// Token: 0x02000085 RID: 133
		private static class Uniforms
		{
			// Token: 0x040002F6 RID: 758
			internal static readonly int _DitheringTex = Shader.PropertyToID("_DitheringTex");

			// Token: 0x040002F7 RID: 759
			internal static readonly int _DitheringCoords = Shader.PropertyToID("_DitheringCoords");
		}
	}
}
