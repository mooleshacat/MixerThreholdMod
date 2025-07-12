using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Product.Packaging;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x0200092B RID: 2347
	public class ProductIconManager : Singleton<ProductIconManager>
	{
		// Token: 0x06003F33 RID: 16179 RVA: 0x0010975C File Offset: 0x0010795C
		public Sprite GetIcon(string productID, string packagingID, bool ignoreError = false)
		{
			ProductIconManager.ProductIcon productIcon = this.icons.Find((ProductIconManager.ProductIcon x) => x.ProductID == productID && x.PackagingID == packagingID);
			if (productIcon == null)
			{
				if (!ignoreError)
				{
					Console.LogError(string.Concat(new string[]
					{
						"Failed to find icon for packaging (",
						packagingID,
						") containing product (",
						productID,
						")"
					}), null);
				}
				return null;
			}
			return productIcon.Icon;
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x001097E0 File Offset: 0x001079E0
		public Sprite GenerateIcons(string productID)
		{
			if (Registry.GetItem(productID) == null)
			{
				Console.LogError("Failed to find product with ID: " + productID, null);
				return null;
			}
			if (this.icons.Any((ProductIconManager.ProductIcon x) => x.ProductID == productID) && Registry.GetItem(productID) != null)
			{
				return Registry.GetItem(productID).Icon;
			}
			for (int i = 0; i < this.Packaging.Length; i++)
			{
				Texture2D texture2D = this.GenerateProductTexture(productID, this.Packaging[i].ID);
				if (texture2D == null)
				{
					Console.LogError(string.Concat(new string[]
					{
						"Failed to generate icon for packaging (",
						this.Packaging[i].ID,
						") containing product (",
						productID,
						")"
					}), null);
				}
				else
				{
					ProductIconManager.ProductIcon productIcon = new ProductIconManager.ProductIcon();
					productIcon.ProductID = productID;
					productIcon.PackagingID = this.Packaging[i].ID;
					texture2D.Apply();
					productIcon.Icon = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
					this.icons.Add(productIcon);
				}
			}
			Texture2D texture2D2 = this.GenerateProductTexture(productID, "none");
			texture2D2.Apply();
			return Sprite.Create(texture2D2, new Rect(0f, 0f, (float)texture2D2.width, (float)texture2D2.height), new Vector2(0.5f, 0.5f));
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x001099A0 File Offset: 0x00107BA0
		private Texture2D GenerateProductTexture(string productID, string packagingID)
		{
			return this.IconGenerator.GeneratePackagingIcon(packagingID, productID);
		}

		// Token: 0x04002D32 RID: 11570
		[SerializeField]
		private List<ProductIconManager.ProductIcon> icons = new List<ProductIconManager.ProductIcon>();

		// Token: 0x04002D33 RID: 11571
		[Header("Product and packaging")]
		public IconGenerator IconGenerator;

		// Token: 0x04002D34 RID: 11572
		public string IconContainerPath = "ProductIcons";

		// Token: 0x04002D35 RID: 11573
		public ProductDefinition[] Products;

		// Token: 0x04002D36 RID: 11574
		public PackagingDefinition[] Packaging;

		// Token: 0x0200092C RID: 2348
		[Serializable]
		public class ProductIcon
		{
			// Token: 0x04002D37 RID: 11575
			public string ProductID;

			// Token: 0x04002D38 RID: 11576
			public string PackagingID;

			// Token: 0x04002D39 RID: 11577
			public Sprite Icon;
		}
	}
}
