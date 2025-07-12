using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000AD5 RID: 2773
	public class CounterOfferProductSelector : MonoBehaviour
	{
		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06004A65 RID: 19045 RVA: 0x0013893F File Offset: 0x00136B3F
		// (set) Token: 0x06004A66 RID: 19046 RVA: 0x00138947 File Offset: 0x00136B47
		public bool IsOpen { get; private set; }

		// Token: 0x06004A67 RID: 19047 RVA: 0x00138950 File Offset: 0x00136B50
		public void Awake()
		{
			this.SearchBar.onValueChanged.AddListener(new UnityAction<string>(this.SetSearchTerm));
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x0013896E File Offset: 0x00136B6E
		public void Open()
		{
			this.IsOpen = true;
			this.Container.gameObject.SetActive(true);
			this.EnsureAllEntriesExist();
			this.SetSearchTerm(string.Empty);
			this.SearchBar.ActivateInputField();
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x001389A4 File Offset: 0x00136BA4
		public void Close()
		{
			this.IsOpen = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x001389BE File Offset: 0x00136BBE
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Submit) && this.lastPreviewedResult != null)
			{
				this.ProductSelected(this.lastPreviewedResult);
			}
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x001389EC File Offset: 0x00136BEC
		public void SetSearchTerm(string search)
		{
			this.searchTerm = search.ToLower();
			this.SearchBar.SetTextWithoutNotify(this.searchTerm);
			this.RebuildResultsList();
			if (search != string.Empty && this.results.Count > 0)
			{
				this.ProductHovered(this.results[0]);
			}
		}

		// Token: 0x06004A6C RID: 19052 RVA: 0x00138A4C File Offset: 0x00136C4C
		private void RebuildResultsList()
		{
			this.results = this.GetMatchingProducts(this.searchTerm);
			this.results.Sort(delegate(ProductDefinition a, ProductDefinition b)
			{
				int num = a.DrugType.CompareTo(b.DrugType);
				if (num != 0)
				{
					return num;
				}
				return a.Name.CompareTo(b.Name);
			});
			Console.Log(string.Format("Found {0} results for {1}", this.results.Count, this.searchTerm), null);
			this.pageCount = Mathf.CeilToInt((float)this.results.Count / 25f);
			this.SetPage(this.pageIndex);
		}

		// Token: 0x06004A6D RID: 19053 RVA: 0x00138AE4 File Offset: 0x00136CE4
		private List<ProductDefinition> GetMatchingProducts(string searchTerm)
		{
			List<ProductDefinition> list = new List<ProductDefinition>();
			List<EDrugType> list2 = new List<EDrugType>();
			foreach (object obj in Enum.GetValues(typeof(EDrugType)))
			{
				EDrugType item = (EDrugType)obj;
				if (searchTerm.ToLower().Contains(item.ToString().ToLower()))
				{
					list2.Add(item);
				}
			}
			if (searchTerm.ToLower().Contains("weed"))
			{
				list2.Add(EDrugType.Marijuana);
			}
			if (searchTerm.ToLower().Contains("coke"))
			{
				list2.Add(EDrugType.Cocaine);
			}
			if (searchTerm.ToLower().Contains("meth"))
			{
				list2.Add(EDrugType.Methamphetamine);
			}
			foreach (ProductDefinition productDefinition in ProductManager.DiscoveredProducts)
			{
				if (list2.Contains(productDefinition.DrugType))
				{
					list.Add(productDefinition);
				}
				else if (productDefinition.Name.ToLower().Contains(searchTerm))
				{
					list.Add(productDefinition);
				}
			}
			return list;
		}

		// Token: 0x06004A6E RID: 19054 RVA: 0x00138C34 File Offset: 0x00136E34
		private void EnsureAllEntriesExist()
		{
			foreach (ProductDefinition productDefinition in ProductManager.DiscoveredProducts)
			{
				if (!this.productEntriesDict.ContainsKey(productDefinition))
				{
					this.CreateProductEntry(productDefinition);
				}
			}
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x00138C94 File Offset: 0x00136E94
		private void CreateProductEntry(ProductDefinition product)
		{
			if (this.productEntriesDict.ContainsKey(product))
			{
				return;
			}
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.ProductEntryPrefab, this.ProductContainer).GetComponent<RectTransform>();
			component.Find("Icon").GetComponent<Image>().sprite = product.Icon;
			component.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.ProductSelected(product);
			});
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = 0;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.ProductHovered(product);
			});
			component.gameObject.AddComponent<EventTrigger>().triggers.Add(entry);
			this.productEntries.Add(component);
			this.productEntriesDict.Add(product, component);
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x00138D74 File Offset: 0x00136F74
		public void ChangePage(int change)
		{
			this.SetPage(this.pageIndex + change);
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x00138D84 File Offset: 0x00136F84
		private void SetPage(int page)
		{
			this.pageIndex = Mathf.Clamp(page, 0, Mathf.Max(this.pageCount - 1, 0));
			int num = this.pageIndex * 25;
			int num2 = Mathf.Min(num + 25, this.results.Count);
			Console.Log(string.Format("Page {0} / {1} ({2} - {3})", new object[]
			{
				this.pageIndex + 1,
				this.pageCount,
				num,
				num2
			}), null);
			List<ProductDefinition> range = this.results.GetRange(num, num2 - num);
			List<ProductDefinition> list = this.productEntriesDict.Keys.ToList<ProductDefinition>();
			for (int i = 0; i < list.Count; i++)
			{
				RectTransform rectTransform = this.productEntriesDict[list[i]];
				if (range.Contains(list[i]))
				{
					rectTransform.gameObject.SetActive(true);
				}
				else
				{
					rectTransform.gameObject.SetActive(false);
				}
			}
			for (int j = 0; j < range.Count; j++)
			{
				this.productEntriesDict[range[j]].SetSiblingIndex(j);
			}
			this.PageLabel.text = string.Format("{0} / {1}", this.pageIndex + 1, this.pageCount);
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x00138EE4 File Offset: 0x001370E4
		private void ProductHovered(ProductDefinition def)
		{
			if (this.onProductPreviewed != null)
			{
				this.onProductPreviewed(def);
			}
			this.lastPreviewedResult = def;
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x00138F01 File Offset: 0x00137101
		private void ProductSelected(ProductDefinition def)
		{
			if (this.onProductSelected != null)
			{
				this.onProductSelected(def);
			}
			this.Close();
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x00138F20 File Offset: 0x00137120
		public bool IsMouseOverSelector()
		{
			bool flag = RectTransformUtility.RectangleContainsScreenPoint(this.Container, GameInput.MousePosition, PlayerSingleton<PlayerCamera>.Instance.OverlayCamera);
			Console.Log(string.Format("Mouse over selector: {0}", flag), null);
			return flag;
		}

		// Token: 0x040036C0 RID: 14016
		public const int ENTRIES_PER_PAGE = 25;

		// Token: 0x040036C1 RID: 14017
		public RectTransform Container;

		// Token: 0x040036C2 RID: 14018
		public InputField SearchBar;

		// Token: 0x040036C3 RID: 14019
		public RectTransform ProductContainer;

		// Token: 0x040036C4 RID: 14020
		public Text PageLabel;

		// Token: 0x040036C5 RID: 14021
		public GameObject ProductEntryPrefab;

		// Token: 0x040036C7 RID: 14023
		public Action<ProductDefinition> onProductPreviewed;

		// Token: 0x040036C8 RID: 14024
		public Action<ProductDefinition> onProductSelected;

		// Token: 0x040036C9 RID: 14025
		private List<RectTransform> productEntries = new List<RectTransform>();

		// Token: 0x040036CA RID: 14026
		private Dictionary<ProductDefinition, RectTransform> productEntriesDict = new Dictionary<ProductDefinition, RectTransform>();

		// Token: 0x040036CB RID: 14027
		private string searchTerm = string.Empty;

		// Token: 0x040036CC RID: 14028
		private int pageIndex;

		// Token: 0x040036CD RID: 14029
		private int pageCount;

		// Token: 0x040036CE RID: 14030
		private List<ProductDefinition> results = new List<ProductDefinition>();

		// Token: 0x040036CF RID: 14031
		private ProductDefinition lastPreviewedResult;
	}
}
