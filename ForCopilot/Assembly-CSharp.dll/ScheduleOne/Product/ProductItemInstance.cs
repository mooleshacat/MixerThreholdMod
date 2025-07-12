using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Serializing.Helping;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Properties;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x0200092F RID: 2351
	[Serializable]
	public class ProductItemInstance : QualityItemInstance
	{
		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06003F3C RID: 16188 RVA: 0x00109A08 File Offset: 0x00107C08
		[CodegenExclude]
		public PackagingDefinition AppliedPackaging
		{
			get
			{
				if (this.packaging == null && this.PackagingID != string.Empty)
				{
					this.packaging = (Registry.GetItem(this.PackagingID) as PackagingDefinition);
					if (this.packaging == null)
					{
						Console.LogError("Failed to load packaging with ID (" + this.PackagingID + ")", null);
					}
				}
				return this.packaging;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06003F3D RID: 16189 RVA: 0x00109A7A File Offset: 0x00107C7A
		[CodegenExclude]
		public int Amount
		{
			get
			{
				if (!(this.AppliedPackaging != null))
				{
					return 1;
				}
				return this.AppliedPackaging.Quantity;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06003F3E RID: 16190 RVA: 0x00109A97 File Offset: 0x00107C97
		public override string Name
		{
			get
			{
				return base.Name + ((this.packaging == null) ? " (Unpackaged)" : string.Empty);
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06003F3F RID: 16191 RVA: 0x00109ABE File Offset: 0x00107CBE
		[CodegenExclude]
		public override Equippable Equippable
		{
			get
			{
				return this.GetEquippable();
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06003F40 RID: 16192 RVA: 0x00109AC6 File Offset: 0x00107CC6
		[CodegenExclude]
		public override StoredItem StoredItem
		{
			get
			{
				return this.GetStoredItem();
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06003F41 RID: 16193 RVA: 0x00109ACE File Offset: 0x00107CCE
		[CodegenExclude]
		public override Sprite Icon
		{
			get
			{
				return this.GetIcon();
			}
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x00109AD6 File Offset: 0x00107CD6
		public ProductItemInstance()
		{
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x00109AEC File Offset: 0x00107CEC
		public ProductItemInstance(ItemDefinition definition, int quantity, EQuality quality, PackagingDefinition _packaging = null) : base(definition, quantity, quality)
		{
			this.packaging = _packaging;
			if (this.packaging != null)
			{
				this.PackagingID = this.packaging.ID;
				return;
			}
			this.PackagingID = string.Empty;
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x00109B40 File Offset: 0x00107D40
		public override bool CanStackWith(ItemInstance other, bool checkQuantities = true)
		{
			if (!(other is ProductItemInstance))
			{
				return false;
			}
			if ((other as ProductItemInstance).AppliedPackaging != null)
			{
				if (this.AppliedPackaging == null)
				{
					return false;
				}
				if ((other as ProductItemInstance).AppliedPackaging.ID != this.AppliedPackaging.ID)
				{
					return false;
				}
			}
			else if (this.AppliedPackaging != null)
			{
				return false;
			}
			return base.CanStackWith(other, checkQuantities);
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x00109BB8 File Offset: 0x00107DB8
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new ProductItemInstance(base.Definition, quantity, this.Quality, this.AppliedPackaging);
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x00109BEC File Offset: 0x00107DEC
		public virtual void SetPackaging(PackagingDefinition def)
		{
			this.packaging = def;
			if (this.packaging != null)
			{
				this.PackagingID = this.packaging.ID;
			}
			else
			{
				this.PackagingID = string.Empty;
			}
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x00109C3F File Offset: 0x00107E3F
		private Equippable GetEquippable()
		{
			if (this.AppliedPackaging != null)
			{
				return this.AppliedPackaging.Equippable_Filled;
			}
			return base.Equippable;
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x00109C61 File Offset: 0x00107E61
		private StoredItem GetStoredItem()
		{
			if (this.AppliedPackaging != null)
			{
				return this.AppliedPackaging.StoredItem_Filled;
			}
			return base.StoredItem;
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x00109C83 File Offset: 0x00107E83
		public virtual void SetupPackagingVisuals(FilledPackagingVisuals visuals)
		{
			visuals.ResetVisuals();
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x00109C8B File Offset: 0x00107E8B
		private Sprite GetIcon()
		{
			if (this.AppliedPackaging != null)
			{
				return Singleton<ProductIconManager>.Instance.GetIcon(this.ID, this.AppliedPackaging.ID, false);
			}
			return base.Icon;
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x00109CBE File Offset: 0x00107EBE
		public override ItemData GetItemData()
		{
			return new ProductItemData(this.ID, this.Quantity, this.Quality.ToString(), this.PackagingID);
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x00109CE8 File Offset: 0x00107EE8
		public virtual float GetAddictiveness()
		{
			return (base.Definition as ProductDefinition).GetAddictiveness();
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x00109CFC File Offset: 0x00107EFC
		public float GetSimilarity(ProductDefinition other, EQuality quality)
		{
			ProductDefinition productDefinition = base.Definition as ProductDefinition;
			float num = 0f;
			if (other.DrugType == productDefinition.DrugType)
			{
				num = 0.4f;
			}
			int num2 = 0;
			for (int i = 0; i < other.Properties.Count; i++)
			{
				if (productDefinition.HasProperty(other.Properties[i]))
				{
					num2++;
				}
			}
			for (int j = 0; j < productDefinition.Properties.Count; j++)
			{
				if (!other.HasProperty(productDefinition.Properties[j]))
				{
					num2--;
				}
			}
			float num3 = 0.3f;
			int num4 = Mathf.Max(productDefinition.Properties.Count, other.Properties.Count);
			if (num4 > 0)
			{
				num3 *= Mathf.Clamp01((float)num2 / (float)num4);
			}
			float num5 = Mathf.Clamp((float)this.Quality / (float)quality, 0f, 1f) * 0.3f;
			return Mathf.Clamp01(num + num3 + num5);
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x00109DFC File Offset: 0x00107FFC
		public virtual void ApplyEffectsToNPC(NPC npc)
		{
			List<Property> list = new List<Property>();
			list.AddRange((base.Definition as ProductDefinition).Properties);
			list = (from x in list
			orderby x.Tier
			select x).ToList<Property>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].ApplyToNPC(npc);
			}
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x00109E70 File Offset: 0x00108070
		public virtual void ClearEffectsFromNPC(NPC npc)
		{
			List<Property> list = new List<Property>();
			list.AddRange((base.Definition as ProductDefinition).Properties);
			list = (from x in list
			orderby x.Tier
			select x).ToList<Property>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].ClearFromNPC(npc);
			}
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x00109EE4 File Offset: 0x001080E4
		public virtual void ApplyEffectsToPlayer(Player player)
		{
			List<Property> list = new List<Property>();
			list.AddRange((base.Definition as ProductDefinition).Properties);
			list = (from x in list
			orderby x.Tier
			select x).ToList<Property>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].ApplyToPlayer(player);
			}
		}

		// Token: 0x06003F51 RID: 16209 RVA: 0x00109F58 File Offset: 0x00108158
		public virtual void ClearEffectsFromPlayer(Player Player)
		{
			List<Property> list = new List<Property>();
			list.AddRange((base.Definition as ProductDefinition).Properties);
			list = (from x in list
			orderby x.Tier
			select x).ToList<Property>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].ClearFromPlayer(Player);
			}
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x00109FCC File Offset: 0x001081CC
		public override float GetMonetaryValue()
		{
			if (this.definition == null)
			{
				Console.LogWarning("ProductItemInstance.GetMonetaryValue() - Definition is null", null);
				return 0f;
			}
			return (this.definition as ProductDefinition).MarketValue * (float)this.Quantity * (float)this.Amount;
		}

		// Token: 0x04002D3D RID: 11581
		public string PackagingID = string.Empty;

		// Token: 0x04002D3E RID: 11582
		[CodegenExclude]
		private PackagingDefinition packaging;
	}
}
