using System;
using ScheduleOne.Money;
using ScheduleOne.Vehicles;
using TMPro;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008B7 RID: 2231
	public class VehicleSaleSign : MonoBehaviour
	{
		// Token: 0x06003C5A RID: 15450 RVA: 0x000FE670 File Offset: 0x000FC870
		private void Awake()
		{
			LandVehicle componentInParent = base.GetComponentInParent<LandVehicle>();
			if (componentInParent != null)
			{
				this.NameLabel.text = componentInParent.VehicleName;
				this.PriceLabel.text = MoneyManager.FormatAmount(componentInParent.VehiclePrice, false, false);
			}
		}

		// Token: 0x04002B21 RID: 11041
		public TextMeshPro NameLabel;

		// Token: 0x04002B22 RID: 11042
		public TextMeshPro PriceLabel;
	}
}
