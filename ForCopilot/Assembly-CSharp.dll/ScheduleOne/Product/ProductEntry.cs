using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Phone.ProductManagerApp;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.Product
{
	// Token: 0x0200094E RID: 2382
	public class ProductEntry : MonoBehaviour
	{
		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06004052 RID: 16466 RVA: 0x0010FF9B File Offset: 0x0010E19B
		// (set) Token: 0x06004053 RID: 16467 RVA: 0x0010FFA3 File Offset: 0x0010E1A3
		public ProductDefinition Definition { get; private set; }

		// Token: 0x06004054 RID: 16468 RVA: 0x0010FFAC File Offset: 0x0010E1AC
		public void Initialize(ProductDefinition definition)
		{
			this.Definition = definition;
			this.Icon.sprite = definition.Icon;
			this.Button.onClick.AddListener(new UnityAction(this.Clicked));
			this.FavouriteButton.onClick.AddListener(new UnityAction(this.FavouriteClicked));
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = 0;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.onHovered.Invoke();
			});
			this.Trigger.triggers.Add(entry);
			this.UpdateListed();
			this.UpdateFavourited();
			this.UpdateDiscovered(this.Definition);
			ProductManager instance = NetworkSingleton<ProductManager>.Instance;
			instance.onProductDiscovered = (Action<ProductDefinition>)Delegate.Combine(instance.onProductDiscovered, new Action<ProductDefinition>(this.UpdateDiscovered));
			ProductManager instance2 = NetworkSingleton<ProductManager>.Instance;
			instance2.onProductListed = (Action<ProductDefinition>)Delegate.Combine(instance2.onProductListed, new Action<ProductDefinition>(this.ProductListedOrDelisted));
			ProductManager instance3 = NetworkSingleton<ProductManager>.Instance;
			instance3.onProductDelisted = (Action<ProductDefinition>)Delegate.Combine(instance3.onProductDelisted, new Action<ProductDefinition>(this.ProductListedOrDelisted));
			ProductManager instance4 = NetworkSingleton<ProductManager>.Instance;
			instance4.onProductFavourited = (Action<ProductDefinition>)Delegate.Combine(instance4.onProductFavourited, new Action<ProductDefinition>(this.ProductFavouritedOrUnFavourited));
			ProductManager instance5 = NetworkSingleton<ProductManager>.Instance;
			instance5.onProductUnfavourited = (Action<ProductDefinition>)Delegate.Combine(instance5.onProductUnfavourited, new Action<ProductDefinition>(this.ProductFavouritedOrUnFavourited));
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x00110114 File Offset: 0x0010E314
		public void Destroy()
		{
			this.destroyed = true;
			base.gameObject.SetActive(false);
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x00110134 File Offset: 0x0010E334
		private void OnDestroy()
		{
			ProductManager instance = NetworkSingleton<ProductManager>.Instance;
			instance.onProductDiscovered = (Action<ProductDefinition>)Delegate.Remove(instance.onProductDiscovered, new Action<ProductDefinition>(this.UpdateDiscovered));
			ProductManager instance2 = NetworkSingleton<ProductManager>.Instance;
			instance2.onProductListed = (Action<ProductDefinition>)Delegate.Remove(instance2.onProductListed, new Action<ProductDefinition>(this.ProductListedOrDelisted));
			ProductManager instance3 = NetworkSingleton<ProductManager>.Instance;
			instance3.onProductDelisted = (Action<ProductDefinition>)Delegate.Remove(instance3.onProductDelisted, new Action<ProductDefinition>(this.ProductListedOrDelisted));
			ProductManager instance4 = NetworkSingleton<ProductManager>.Instance;
			instance4.onProductFavourited = (Action<ProductDefinition>)Delegate.Remove(instance4.onProductFavourited, new Action<ProductDefinition>(this.ProductFavouritedOrUnFavourited));
			ProductManager instance5 = NetworkSingleton<ProductManager>.Instance;
			instance5.onProductUnfavourited = (Action<ProductDefinition>)Delegate.Remove(instance5.onProductUnfavourited, new Action<ProductDefinition>(this.ProductFavouritedOrUnFavourited));
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x001101FF File Offset: 0x0010E3FF
		private void Clicked()
		{
			PlayerSingleton<ProductManagerApp>.Instance.SelectProduct(this);
			this.UpdateListed();
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x00110214 File Offset: 0x0010E414
		private void FavouriteClicked()
		{
			if (!ProductManager.DiscoveredProducts.Contains(this.Definition))
			{
				return;
			}
			if (ProductManager.FavouritedProducts.Contains(this.Definition))
			{
				NetworkSingleton<ProductManager>.Instance.SetProductFavourited(this.Definition.ID, false);
				return;
			}
			NetworkSingleton<ProductManager>.Instance.SetProductFavourited(this.Definition.ID, true);
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x00110273 File Offset: 0x0010E473
		private void ProductListedOrDelisted(ProductDefinition def)
		{
			if (def == this.Definition)
			{
				this.UpdateListed();
			}
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x0011028C File Offset: 0x0010E48C
		public void UpdateListed()
		{
			if (this.destroyed)
			{
				return;
			}
			if (this == null)
			{
				return;
			}
			if (base.gameObject == null)
			{
				return;
			}
			if (ProductManager.ListedProducts.Contains(this.Definition))
			{
				this.Frame.color = this.SelectedColor;
				this.Tick.gameObject.SetActive(true);
				this.Cross.gameObject.SetActive(false);
				return;
			}
			this.Frame.color = this.DeselectedColor;
			this.Tick.gameObject.SetActive(false);
			this.Cross.gameObject.SetActive(true);
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x00110334 File Offset: 0x0010E534
		private void ProductFavouritedOrUnFavourited(ProductDefinition def)
		{
			if (def == this.Definition)
			{
				this.UpdateFavourited();
			}
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0011034C File Offset: 0x0010E54C
		public void UpdateFavourited()
		{
			if (this.destroyed)
			{
				return;
			}
			if (this == null)
			{
				return;
			}
			if (base.gameObject == null)
			{
				return;
			}
			if (ProductManager.FavouritedProducts.Contains(this.Definition))
			{
				this.FavouriteIcon.color = this.FavouritedColor;
				return;
			}
			this.FavouriteIcon.color = this.UnfavouritedColor;
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x001103B0 File Offset: 0x0010E5B0
		public void UpdateDiscovered(ProductDefinition def)
		{
			if (def == null)
			{
				Console.LogWarning(((def != null) ? def.ToString() : null) + " productDefinition is null", null);
			}
			if (def.ID == this.Definition.ID)
			{
				if (ProductManager.DiscoveredProducts.Contains(this.Definition))
				{
					this.Icon.color = Color.white;
				}
				else
				{
					this.Icon.color = Color.black;
				}
				this.UpdateListed();
			}
		}

		// Token: 0x04002DB9 RID: 11705
		public Color SelectedColor;

		// Token: 0x04002DBA RID: 11706
		public Color DeselectedColor;

		// Token: 0x04002DBB RID: 11707
		public Color FavouritedColor;

		// Token: 0x04002DBC RID: 11708
		public Color UnfavouritedColor;

		// Token: 0x04002DBD RID: 11709
		[Header("References")]
		public Button Button;

		// Token: 0x04002DBE RID: 11710
		public Image Frame;

		// Token: 0x04002DBF RID: 11711
		public Image Icon;

		// Token: 0x04002DC0 RID: 11712
		public RectTransform Tick;

		// Token: 0x04002DC1 RID: 11713
		public RectTransform Cross;

		// Token: 0x04002DC2 RID: 11714
		public EventTrigger Trigger;

		// Token: 0x04002DC3 RID: 11715
		public Button FavouriteButton;

		// Token: 0x04002DC4 RID: 11716
		public Image FavouriteIcon;

		// Token: 0x04002DC5 RID: 11717
		public UnityEvent onHovered;

		// Token: 0x04002DC6 RID: 11718
		private bool destroyed;
	}
}
