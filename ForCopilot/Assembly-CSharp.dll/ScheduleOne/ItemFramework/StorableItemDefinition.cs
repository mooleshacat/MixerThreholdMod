using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Levelling;
using ScheduleOne.StationFramework;
using ScheduleOne.Storage;
using ScheduleOne.UI.Shop;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000998 RID: 2456
	[CreateAssetMenu(fileName = "StorableItemDefinition", menuName = "ScriptableObjects/StorableItemDefinition", order = 1)]
	[Serializable]
	public class StorableItemDefinition : ItemDefinition
	{
		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06004254 RID: 16980 RVA: 0x00116DEC File Offset: 0x00114FEC
		public bool IsPurchasable
		{
			get
			{
				return !this.RequiresLevelToPurchase || NetworkSingleton<LevelManager>.Instance.GetFullRank() >= this.RequiredRank;
			}
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x00116E0D File Offset: 0x0011500D
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new StorableItemInstance(this, quantity);
		}

		// Token: 0x04002F23 RID: 12067
		[Header("Purchasing")]
		public float BasePurchasePrice = 10f;

		// Token: 0x04002F24 RID: 12068
		public List<ShopListing.CategoryInstance> ShopCategories = new List<ShopListing.CategoryInstance>();

		// Token: 0x04002F25 RID: 12069
		public bool RequiresLevelToPurchase;

		// Token: 0x04002F26 RID: 12070
		public FullRank RequiredRank;

		// Token: 0x04002F27 RID: 12071
		[Header("Reselling")]
		[Range(0f, 1f)]
		public float ResellMultiplier = 0.5f;

		// Token: 0x04002F28 RID: 12072
		[Header("Storable Item")]
		public StoredItem StoredItem;

		// Token: 0x04002F29 RID: 12073
		[Tooltip("Optional station item if this item can be used at a station.")]
		public StationItem StationItem;
	}
}
