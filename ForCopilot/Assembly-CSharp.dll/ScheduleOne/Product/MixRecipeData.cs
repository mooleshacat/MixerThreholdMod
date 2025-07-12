using System;

namespace ScheduleOne.Product
{
	// Token: 0x02000925 RID: 2341
	[Serializable]
	public class MixRecipeData
	{
		// Token: 0x06003F09 RID: 16137 RVA: 0x0010904D File Offset: 0x0010724D
		public MixRecipeData(string product, string mixer, string output)
		{
			this.Product = product;
			this.Mixer = mixer;
			this.Output = output;
		}

		// Token: 0x04002D0F RID: 11535
		public string Product;

		// Token: 0x04002D10 RID: 11536
		public string Mixer;

		// Token: 0x04002D11 RID: 11537
		public string Output;
	}
}
