using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;
using ScheduleOne.Properties;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C3B RID: 3131
	[Serializable]
	public class MixOperation
	{
		// Token: 0x0600573B RID: 22331 RVA: 0x00170D8E File Offset: 0x0016EF8E
		public MixOperation(string productID, EQuality productQuality, string ingredientID, int quantity)
		{
			this.ProductID = productID;
			this.ProductQuality = productQuality;
			this.IngredientID = ingredientID;
			this.Quantity = quantity;
		}

		// Token: 0x0600573C RID: 22332 RVA: 0x0000494F File Offset: 0x00002B4F
		public MixOperation()
		{
		}

		// Token: 0x0600573D RID: 22333 RVA: 0x00170DB4 File Offset: 0x0016EFB4
		public EDrugType GetOutput(out List<Property> properties)
		{
			ProductDefinition item = Registry.GetItem<ProductDefinition>(this.ProductID);
			PropertyItemDefinition item2 = Registry.GetItem<PropertyItemDefinition>(this.IngredientID);
			properties = PropertyMixCalculator.MixProperties(item.Properties, item2.Properties[0], item.DrugType);
			return item.DrugType;
		}

		// Token: 0x0600573E RID: 22334 RVA: 0x00170E00 File Offset: 0x0016F000
		public bool IsOutputKnown(out ProductDefinition knownProduct)
		{
			List<Property> properties;
			EDrugType output = this.GetOutput(out properties);
			knownProduct = NetworkSingleton<ProductManager>.Instance.GetKnownProduct(output, properties);
			return knownProduct != null;
		}

		// Token: 0x0400404E RID: 16462
		public string ProductID;

		// Token: 0x0400404F RID: 16463
		public EQuality ProductQuality;

		// Token: 0x04004050 RID: 16464
		public string IngredientID;

		// Token: 0x04004051 RID: 16465
		public int Quantity;
	}
}
