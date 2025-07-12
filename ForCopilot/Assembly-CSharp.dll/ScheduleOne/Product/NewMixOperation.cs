using System;

namespace ScheduleOne.Product
{
	// Token: 0x02000929 RID: 2345
	[Serializable]
	public class NewMixOperation
	{
		// Token: 0x06003F19 RID: 16153 RVA: 0x001094A1 File Offset: 0x001076A1
		public NewMixOperation(string productID, string ingredientID)
		{
			this.ProductID = productID;
			this.IngredientID = ingredientID;
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x0000494F File Offset: 0x00002B4F
		public NewMixOperation()
		{
		}

		// Token: 0x04002D24 RID: 11556
		public string ProductID;

		// Token: 0x04002D25 RID: 11557
		public string IngredientID;
	}
}
