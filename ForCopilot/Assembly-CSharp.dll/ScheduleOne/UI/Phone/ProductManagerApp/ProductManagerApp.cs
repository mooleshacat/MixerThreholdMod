using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.ProductManagerApp
{
	// Token: 0x02000AED RID: 2797
	public class ProductManagerApp : App<ProductManagerApp>
	{
		// Token: 0x06004B17 RID: 19223 RVA: 0x0013B6D3 File Offset: 0x001398D3
		protected override void Awake()
		{
			base.Awake();
			this.DetailPanel.SetActiveProduct(null);
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x0013B6E8 File Offset: 0x001398E8
		protected override void Start()
		{
			base.Start();
			ProductManager instance = NetworkSingleton<ProductManager>.Instance;
			instance.onProductDiscovered = (Action<ProductDefinition>)Delegate.Combine(instance.onProductDiscovered, new Action<ProductDefinition>(this.CreateEntry));
			ProductManager instance2 = NetworkSingleton<ProductManager>.Instance;
			instance2.onProductFavourited = (Action<ProductDefinition>)Delegate.Combine(instance2.onProductFavourited, new Action<ProductDefinition>(this.ProductFavourited));
			ProductManager instance3 = NetworkSingleton<ProductManager>.Instance;
			instance3.onProductUnfavourited = (Action<ProductDefinition>)Delegate.Combine(instance3.onProductUnfavourited, new Action<ProductDefinition>(this.ProductUnfavourited));
			foreach (ProductDefinition definition in ProductManager.FavouritedProducts)
			{
				this.CreateFavouriteEntry(definition);
			}
			foreach (ProductDefinition definition2 in ProductManager.DiscoveredProducts)
			{
				this.CreateEntry(definition2);
			}
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x0013B7F4 File Offset: 0x001399F4
		private void LateUpdate()
		{
			if (!base.isOpen)
			{
				return;
			}
			if (this.selectedEntry != null)
			{
				this.SelectionIndicator.position = this.selectedEntry.transform.position;
			}
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x0013B828 File Offset: 0x00139A28
		public virtual void CreateEntry(ProductDefinition definition)
		{
			ProductManagerApp.ProductTypeContainer productTypeContainer = this.ProductTypeContainers.Find((ProductManagerApp.ProductTypeContainer x) => x.DrugType == definition.DrugTypes[0].DrugType);
			ProductEntry component = UnityEngine.Object.Instantiate<GameObject>(this.EntryPrefab, productTypeContainer.Container).GetComponent<ProductEntry>();
			component.Initialize(definition);
			this.entries.Add(component);
			productTypeContainer.RefreshNoneDisplay();
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x0013B89A File Offset: 0x00139A9A
		private void ProductFavourited(ProductDefinition product)
		{
			this.CreateFavouriteEntry(product);
		}

		// Token: 0x06004B1C RID: 19228 RVA: 0x0013B8A3 File Offset: 0x00139AA3
		private void ProductUnfavourited(ProductDefinition product)
		{
			this.RemoveFavouriteEntry(product);
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x0013B8AC File Offset: 0x00139AAC
		private void CreateFavouriteEntry(ProductDefinition definition)
		{
			if (this.favouriteEntries.Find((ProductEntry x) => x.Definition == definition) != null)
			{
				return;
			}
			ProductEntry component = UnityEngine.Object.Instantiate<GameObject>(this.EntryPrefab, this.FavouritesContainer.Container).GetComponent<ProductEntry>();
			component.Initialize(definition);
			this.favouriteEntries.Add(component);
			this.FavouritesContainer.RefreshNoneDisplay();
			this.DelayedRebuildLayout();
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x0013B92C File Offset: 0x00139B2C
		private void RemoveFavouriteEntry(ProductDefinition definition)
		{
			ProductEntry productEntry = this.favouriteEntries.Find((ProductEntry x) => x.Definition == definition);
			if (this.selectedEntry == productEntry)
			{
				this.selectedEntry = null;
				this.SelectionIndicator.gameObject.SetActive(false);
				this.DetailPanel.SetActiveProduct(null);
			}
			if (productEntry != null)
			{
				this.favouriteEntries.Remove(productEntry);
				productEntry.Destroy();
			}
			this.FavouritesContainer.RefreshNoneDisplay();
			this.DelayedRebuildLayout();
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x0013B9BD File Offset: 0x00139BBD
		private void DelayedRebuildLayout()
		{
			base.StartCoroutine(this.<DelayedRebuildLayout>g__Delay|17_0());
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x0013B9CC File Offset: 0x00139BCC
		public void SelectProduct(ProductEntry entry)
		{
			this.selectedEntry = entry;
			this.DetailPanel.SetActiveProduct(entry.Definition);
			this.SelectionIndicator.position = entry.transform.position;
			this.SelectionIndicator.gameObject.SetActive(true);
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x0013BA18 File Offset: 0x00139C18
		public override void SetOpen(bool open)
		{
			ProductManagerApp.<>c__DisplayClass19_0 CS$<>8__locals1 = new ProductManagerApp.<>c__DisplayClass19_0();
			CS$<>8__locals1.<>4__this = this;
			base.SetOpen(open);
			if (open)
			{
				for (int i = 0; i < this.entries.Count; i++)
				{
					this.entries[i].UpdateDiscovered(this.entries[i].Definition);
					this.entries[i].UpdateListed();
				}
				for (int j = 0; j < this.favouriteEntries.Count; j++)
				{
					this.favouriteEntries[j].UpdateDiscovered(this.favouriteEntries[j].Definition);
					this.favouriteEntries[j].UpdateListed();
				}
				LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
				base.gameObject.SetActive(false);
				base.gameObject.SetActive(true);
				CS$<>8__locals1.layoutGroups = base.GetComponentsInChildren<VerticalLayoutGroup>();
				for (int k = 0; k < CS$<>8__locals1.layoutGroups.Length; k++)
				{
					CS$<>8__locals1.layoutGroups[k].enabled = false;
					CS$<>8__locals1.layoutGroups[k].enabled = true;
				}
				if (this.selectedEntry != null)
				{
					this.DetailPanel.SetActiveProduct(this.selectedEntry.Definition);
				}
				base.StartCoroutine(CS$<>8__locals1.<SetOpen>g__Delay|0());
			}
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x0013BB7E File Offset: 0x00139D7E
		[CompilerGenerated]
		private IEnumerator <DelayedRebuildLayout>g__Delay|17_0()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
			yield return new WaitForEndOfFrame();
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
			ContentSizeFitter[] componentsInChildren = base.GetComponentsInChildren<ContentSizeFitter>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
				componentsInChildren[i].enabled = true;
			}
			yield break;
		}

		// Token: 0x0400377A RID: 14202
		[Header("References")]
		public ProductManagerApp.ProductTypeContainer FavouritesContainer;

		// Token: 0x0400377B RID: 14203
		public List<ProductManagerApp.ProductTypeContainer> ProductTypeContainers;

		// Token: 0x0400377C RID: 14204
		public ProductAppDetailPanel DetailPanel;

		// Token: 0x0400377D RID: 14205
		public RectTransform SelectionIndicator;

		// Token: 0x0400377E RID: 14206
		public GameObject EntryPrefab;

		// Token: 0x0400377F RID: 14207
		private List<ProductEntry> favouriteEntries = new List<ProductEntry>();

		// Token: 0x04003780 RID: 14208
		private List<ProductEntry> entries = new List<ProductEntry>();

		// Token: 0x04003781 RID: 14209
		private ProductEntry selectedEntry;

		// Token: 0x02000AEE RID: 2798
		[Serializable]
		public class ProductTypeContainer
		{
			// Token: 0x06004B24 RID: 19236 RVA: 0x0013BB8D File Offset: 0x00139D8D
			public void RefreshNoneDisplay()
			{
				this.NoneDisplay.gameObject.SetActive(this.Container.childCount == 0);
			}

			// Token: 0x04003782 RID: 14210
			public EDrugType DrugType;

			// Token: 0x04003783 RID: 14211
			public RectTransform Container;

			// Token: 0x04003784 RID: 14212
			public RectTransform NoneDisplay;
		}
	}
}
