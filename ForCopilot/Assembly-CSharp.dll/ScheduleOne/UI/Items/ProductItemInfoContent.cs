using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;
using TMPro;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BC8 RID: 3016
	public class ProductItemInfoContent : QualityItemInfoContent
	{
		// Token: 0x0600501E RID: 20510 RVA: 0x00152E54 File Offset: 0x00151054
		public override void Initialize(ItemInstance instance)
		{
			base.Initialize(instance);
			ProductItemInstance productItemInstance = instance as ProductItemInstance;
			if (productItemInstance == null)
			{
				Console.LogError("ProductItemInfoContent can only be used with ProductItemInstance!", null);
				return;
			}
			this.Initialize(productItemInstance.Definition);
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x00152E8C File Offset: 0x0015108C
		public override void Initialize(ItemDefinition definition)
		{
			base.Initialize(definition);
			ProductDefinition productDefinition = definition as ProductDefinition;
			PropertyUtility.DrugTypeData drugTypeData = PropertyUtility.GetDrugTypeData(productDefinition.DrugTypes[0].DrugType);
			TextMeshProUGUI qualityLabel = this.QualityLabel;
			qualityLabel.text = qualityLabel.text + " " + drugTypeData.Name;
			for (int i = 0; i < this.PropertyLabels.Count; i++)
			{
				if (productDefinition.Properties.Count > i)
				{
					this.PropertyLabels[i].text = "• " + productDefinition.Properties[i].Name;
					this.PropertyLabels[i].color = productDefinition.Properties[i].LabelColor;
					this.PropertyLabels[i].enabled = true;
				}
				else
				{
					this.PropertyLabels[i].enabled = false;
				}
			}
		}

		// Token: 0x04003C1E RID: 15390
		public List<TextMeshProUGUI> PropertyLabels = new List<TextMeshProUGUI>();
	}
}
