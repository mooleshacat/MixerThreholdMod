using System;
using System.Collections.Generic;
using ScheduleOne.Map;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x020006BB RID: 1723
	public class SupplierLocation : MonoBehaviour
	{
		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002F4C RID: 12108 RVA: 0x000C6A79 File Offset: 0x000C4C79
		public bool IsOccupied
		{
			get
			{
				return this.ActiveSupplier != null;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002F4D RID: 12109 RVA: 0x000C6A87 File Offset: 0x000C4C87
		// (set) Token: 0x06002F4E RID: 12110 RVA: 0x000C6A8F File Offset: 0x000C4C8F
		public Supplier ActiveSupplier { get; private set; }

		// Token: 0x06002F4F RID: 12111 RVA: 0x000C6A98 File Offset: 0x000C4C98
		public void Awake()
		{
			SupplierLocation.AllLocations.Add(this);
			this.GenericContainer.gameObject.SetActive(false);
			WorldStorageEntity[] deliveryBays = this.DeliveryBays;
			for (int i = 0; i < deliveryBays.Length; i++)
			{
				deliveryBays[i].transform.Find("Container").gameObject.SetActive(false);
			}
			this.configs = base.GetComponentsInChildren<SupplierLocationConfiguration>();
			SupplierLocationConfiguration[] array = this.configs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Deactivate();
			}
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x000C6B1C File Offset: 0x000C4D1C
		private void OnDestroy()
		{
			SupplierLocation.AllLocations.Remove(this);
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x000C6B2C File Offset: 0x000C4D2C
		public void SetActiveSupplier(Supplier supplier)
		{
			this.ActiveSupplier = supplier;
			this.GenericContainer.gameObject.SetActive(this.ActiveSupplier != null);
			WorldStorageEntity[] deliveryBays = this.DeliveryBays;
			for (int i = 0; i < deliveryBays.Length; i++)
			{
				deliveryBays[i].transform.Find("Container").gameObject.SetActive(this.ActiveSupplier != null);
			}
			if (supplier != null)
			{
				this.PoI.SetMainText("Supplier Meeting\n(" + supplier.fullName + ")");
			}
			foreach (SupplierLocationConfiguration supplierLocationConfiguration in this.configs)
			{
				if (this.ActiveSupplier != null && supplierLocationConfiguration.SupplierID == this.ActiveSupplier.ID)
				{
					supplierLocationConfiguration.Activate();
				}
				else
				{
					supplierLocationConfiguration.Deactivate();
				}
			}
		}

		// Token: 0x0400213F RID: 8511
		public static List<SupplierLocation> AllLocations = new List<SupplierLocation>();

		// Token: 0x04002141 RID: 8513
		[Header("Settings")]
		public string LocationName;

		// Token: 0x04002142 RID: 8514
		public string LocationDescription;

		// Token: 0x04002143 RID: 8515
		[Header("References")]
		public Transform GenericContainer;

		// Token: 0x04002144 RID: 8516
		public Transform SupplierStandPoint;

		// Token: 0x04002145 RID: 8517
		public WorldStorageEntity[] DeliveryBays;

		// Token: 0x04002146 RID: 8518
		public POI PoI;

		// Token: 0x04002147 RID: 8519
		private SupplierLocationConfiguration[] configs;
	}
}
