using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BB8 RID: 3000
	public class FilterConfigPanel : MonoBehaviour
	{
		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06004FA6 RID: 20390 RVA: 0x0014FEBA File Offset: 0x0014E0BA
		// (set) Token: 0x06004FA7 RID: 20391 RVA: 0x0014FEC2 File Offset: 0x0014E0C2
		public bool IsOpen { get; private set; }

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06004FA8 RID: 20392 RVA: 0x0014FECB File Offset: 0x0014E0CB
		// (set) Token: 0x06004FA9 RID: 20393 RVA: 0x0014FED3 File Offset: 0x0014E0D3
		public ItemSlot OpenSlot { get; private set; }

		// Token: 0x06004FAA RID: 20394 RVA: 0x0014FEDC File Offset: 0x0014E0DC
		private void Awake()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 12);
			this.SearchInput.onValueChanged.AddListener(new UnityAction<string>(this.SearchChanged));
			this.Close();
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x0014FF13 File Offset: 0x0014E113
		private void Start()
		{
			this.UpdateSearch();
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x0014FF1C File Offset: 0x0014E11C
		private void Exit(ExitAction exit)
		{
			if (exit.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (exit.exitType == ExitType.Escape)
			{
				exit.Use();
				if (this.SearchContainer.gameObject.activeSelf)
				{
					this.CloseSearch();
					return;
				}
				this.Close();
			}
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x0014FF6C File Offset: 0x0014E16C
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (Input.GetMouseButtonUp(0))
			{
				if (this.mouseUp)
				{
					bool flag = this.IsMouseOverPanel();
					bool flag2 = this.IsMouseOverSearch();
					bool flag3 = this.IsMouseOverDropdown() && this.Dropdown.gameObject.activeSelf;
					if ((flag || flag2) && this.Dropdown.gameObject.activeSelf && !flag3)
					{
						this.CloseDropdown();
					}
					if (flag && !flag2 && this.SearchContainer.gameObject.activeSelf)
					{
						this.CloseSearch();
					}
					if (!flag && (!this.SearchContainer.gameObject.activeSelf || !flag2) && !flag3)
					{
						this.Close();
						return;
					}
				}
				else
				{
					this.mouseUp = true;
				}
			}
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x00150028 File Offset: 0x0014E228
		public void Open(ItemSlotUI ui)
		{
			if (ui.assignedSlot == null)
			{
				Console.LogError("ItemSlotUI has no assigned slot! Cannot open filter config panel", null);
				return;
			}
			this.IsOpen = true;
			this.OpenSlot = ui.assignedSlot;
			this.Container.gameObject.SetActive(true);
			Vector2 vector = ui.Rect.position + Vector2.one * ui.Rect.sizeDelta.x / 2f * ui.Rect.GetComponentInParent<Canvas>().scaleFactor;
			vector += Vector2.right * (this.Rect.sizeDelta.x / 2f) * base.GetComponentInParent<Canvas>().scaleFactor;
			vector -= Vector2.up * (this.Rect.sizeDelta.y / 2f) * base.GetComponentInParent<Canvas>().scaleFactor;
			vector += Vector2.up * 18f;
			this.Rect.position = vector;
			this.mouseUp = false;
			ItemSlot openSlot = this.OpenSlot;
			openSlot.onFilterChange = (Action)Delegate.Combine(openSlot.onFilterChange, new Action(this.RefreshDisplay));
			this.UpdateSearch();
			this.RefreshDisplay();
			base.StartCoroutine(this.<Open>g__Open|39_0());
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x00150198 File Offset: 0x0014E398
		public void Close()
		{
			this.IsOpen = false;
			if (this.OpenSlot != null)
			{
				ItemSlot openSlot = this.OpenSlot;
				openSlot.onFilterChange = (Action)Delegate.Remove(openSlot.onFilterChange, new Action(this.RefreshDisplay));
				this.OpenSlot = null;
			}
			for (int i = 0; i < this.itemEntries.Count; i++)
			{
				UnityEngine.Object.Destroy(this.itemEntries[i].gameObject);
			}
			this.itemEntries.Clear();
			this.CloseSearch();
			this.CloseDropdown();
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x00150238 File Offset: 0x0014E438
		private void UpdateSearch()
		{
			using (List<ItemDefinition>.Enumerator enumerator = Singleton<Registry>.Instance.GetAllItems().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemDefinition item = enumerator.Current;
					if (item.UsableInFilters)
					{
						FilterConfigPanel.SearchCategory searchCategory = this.GetSearchCategory(item.Category);
						if (searchCategory.GetItem(item.ID) == null)
						{
							RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.SearchItemPrefab, searchCategory.Container).GetComponent<RectTransform>();
							component.Find("Icon").GetComponent<Image>().sprite = item.Icon;
							component.GetComponent<Tooltip>().text = item.Name;
							component.GetComponent<Button>().onClick.AddListener(delegate()
							{
								this.ItemClicked(item.ID);
							});
							searchCategory.AddItem(item, component);
						}
					}
				}
			}
			foreach (FilterConfigPanel.SearchCategory searchCategory2 in this.searchCategories)
			{
				searchCategory2.Items.Sort((FilterConfigPanel.SearchCategory.Item a, FilterConfigPanel.SearchCategory.Item b) => a.ItemDefinition.Name.CompareTo(b.ItemDefinition.Name));
				for (int i = 0; i < searchCategory2.Items.Count; i++)
				{
					searchCategory2.Items[i].Entry.SetSiblingIndex(i + 1);
				}
			}
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x001503EC File Offset: 0x0014E5EC
		public void FilterModeSelected(int filterType)
		{
			this.FilterModeSelected((SlotFilter.EType)filterType);
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x001503F8 File Offset: 0x0014E5F8
		public void FilterModeSelected(SlotFilter.EType filterType)
		{
			SlotFilter playerFilter = this.OpenSlot.PlayerFilter;
			playerFilter.Type = filterType;
			this.OpenSlot.SetPlayerFilter(playerFilter, false);
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x00150425 File Offset: 0x0014E625
		public void QualitySelected(int quality)
		{
			this.QualitySelected((EQuality)quality);
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x00150430 File Offset: 0x0014E630
		public void QualitySelected(EQuality quality)
		{
			SlotFilter playerFilter = this.OpenSlot.PlayerFilter;
			if (playerFilter.AllowedQualities.Contains(quality))
			{
				playerFilter.AllowedQualities.Remove(quality);
			}
			else
			{
				playerFilter.AllowedQualities.Add(quality);
			}
			this.OpenSlot.SetPlayerFilter(playerFilter, false);
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x0015047F File Offset: 0x0014E67F
		public void AddClicked()
		{
			this.mouseUp = false;
			this.OpenSearch();
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x0015048E File Offset: 0x0014E68E
		public void CopyClicked()
		{
			this.mouseUp = false;
			FilterConfigPanel.copiedFilter = this.OpenSlot.PlayerFilter.Clone();
			GUIUtility.systemCopyBuffer = JsonUtility.ToJson(FilterConfigPanel.copiedFilter, false);
			this.CloseDropdown();
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x001504C2 File Offset: 0x0014E6C2
		public void PasteClicked()
		{
			this.mouseUp = false;
			this.OpenSlot.SetPlayerFilter(FilterConfigPanel.copiedFilter, false);
			this.CloseDropdown();
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x001504E4 File Offset: 0x0014E6E4
		public void ApplyToSiblingsClicked()
		{
			this.mouseUp = false;
			foreach (ItemSlot itemSlot in this.OpenSlot.SiblingSet.Slots)
			{
				if (itemSlot != this.OpenSlot)
				{
					itemSlot.SetPlayerFilter(this.OpenSlot.PlayerFilter.Clone(), false);
				}
			}
			this.CloseDropdown();
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x00150568 File Offset: 0x0014E768
		public void ClearClicked()
		{
			this.mouseUp = false;
			this.OpenSlot.SetPlayerFilter(new SlotFilter(), false);
			this.CloseDropdown();
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x00150588 File Offset: 0x0014E788
		public void ToggleDropdown()
		{
			if (this.Dropdown.gameObject.activeSelf)
			{
				this.CloseDropdown();
				return;
			}
			this.OpenDropdown();
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x001505AC File Offset: 0x0014E7AC
		public void OpenDropdown()
		{
			this.mouseUp = false;
			this.CloseSearch();
			string systemCopyBuffer = GUIUtility.systemCopyBuffer;
			if (!string.IsNullOrEmpty(systemCopyBuffer))
			{
				try
				{
					SlotFilter slotFilter = JsonUtility.FromJson<SlotFilter>(systemCopyBuffer);
					if (slotFilter != null)
					{
						FilterConfigPanel.copiedFilter = slotFilter;
					}
				}
				catch
				{
					Console.Log("Failed to parse clipboard text as SlotFilter JSON!", null);
				}
			}
			this.PasteButton.interactable = (FilterConfigPanel.copiedFilter != null);
			this.Dropdown.gameObject.SetActive(true);
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x00150628 File Offset: 0x0014E828
		public void CloseDropdown()
		{
			this.Dropdown.gameObject.SetActive(false);
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x0015063B File Offset: 0x0014E83B
		private void ItemClicked(string itemID)
		{
			this.mouseUp = false;
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				this.CloseSearch();
			}
			this.AddItem(itemID);
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x00150660 File Offset: 0x0014E860
		private void AddItem(string itemID)
		{
			SlotFilter playerFilter = this.OpenSlot.PlayerFilter;
			if (!playerFilter.ItemIDs.Contains(itemID))
			{
				playerFilter.ItemIDs.Add(itemID);
			}
			this.OpenSlot.SetPlayerFilter(playerFilter, false);
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x001506A0 File Offset: 0x0014E8A0
		private void RemoveItem(string itemID)
		{
			SlotFilter playerFilter = this.OpenSlot.PlayerFilter;
			playerFilter.ItemIDs.Remove(itemID);
			this.OpenSlot.SetPlayerFilter(playerFilter, false);
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x001506D4 File Offset: 0x0014E8D4
		private void RefreshDisplay()
		{
			this.TypeButton_None.interactable = (this.OpenSlot.PlayerFilter.Type > SlotFilter.EType.None);
			this.TypeButton_Whitelist.interactable = (this.OpenSlot.PlayerFilter.Type != SlotFilter.EType.Whitelist);
			this.TypeButton_Blacklist.interactable = (this.OpenSlot.PlayerFilter.Type != SlotFilter.EType.Blacklist);
			this.TypeLabel.text = this.OpenSlot.PlayerFilter.Type.ToString();
			if (this.OpenSlot.PlayerFilter.Type == SlotFilter.EType.Blacklist)
			{
				this.ListLabel.text = "Unallowed Items";
			}
			else
			{
				this.ListLabel.text = "Allowed Items";
			}
			this.ListBlocker.SetActive(this.OpenSlot.PlayerFilter.Type == SlotFilter.EType.None);
			for (int i = 0; i < this.QualityButtons.Length; i++)
			{
				this.QualityButtons[i].transform.Find("Image").gameObject.SetActive(this.OpenSlot.PlayerFilter.AllowedQualities.Contains((EQuality)i));
			}
			if (this.OpenSlot.PlayerFilter.ItemIDs.Count > 0)
			{
				TextMeshProUGUI listLabel = this.ListLabel;
				listLabel.text = listLabel.text + " (" + this.OpenSlot.PlayerFilter.ItemIDs.Count.ToString() + ")";
			}
			for (int j = 0; j < this.itemEntries.Count; j++)
			{
				UnityEngine.Object.Destroy(this.itemEntries[j].gameObject);
			}
			this.itemEntries.Clear();
			for (int k = 0; k < this.OpenSlot.PlayerFilter.ItemIDs.Count; k++)
			{
				ItemDefinition item = Registry.GetItem(this.OpenSlot.PlayerFilter.ItemIDs[k]);
				if (item == null)
				{
					Console.LogError("Item with ID " + this.OpenSlot.PlayerFilter.ItemIDs[k] + " not found!", null);
				}
				else
				{
					RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.ItemEntryPrefab, this.ListContainer).GetComponent<RectTransform>();
					component.transform.Find("Icon").GetComponent<Image>().sprite = item.Icon;
					component.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = item.Name;
					component.transform.Find("Remove").GetComponent<Button>().onClick.AddListener(delegate()
					{
						this.RemoveItem(item.ID);
					});
					component.transform.SetSiblingIndex(k);
					this.itemEntries.Add(component);
				}
			}
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x001509D4 File Offset: 0x0014EBD4
		private bool IsMouseOverPanel()
		{
			if (this.Rect == null)
			{
				return false;
			}
			Vector2 point = this.Rect.InverseTransformPoint(Input.mousePosition);
			return this.Rect.rect.Contains(point);
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x00150A1C File Offset: 0x0014EC1C
		private bool IsMouseOverSearch()
		{
			Vector2 point = this.SearchContainer.InverseTransformPoint(Input.mousePosition);
			return this.SearchContainer.rect.Contains(point);
		}

		// Token: 0x06004FC3 RID: 20419 RVA: 0x00150A54 File Offset: 0x0014EC54
		private bool IsMouseOverDropdown()
		{
			Vector2 point = this.Dropdown.InverseTransformPoint(Input.mousePosition);
			return this.Dropdown.rect.Contains(point);
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x00150A8C File Offset: 0x0014EC8C
		private FilterConfigPanel.SearchCategory GetSearchCategory(EItemCategory category)
		{
			for (int i = 0; i < this.searchCategories.Count; i++)
			{
				if (this.searchCategories[i].Category == category)
				{
					return this.searchCategories[i];
				}
			}
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.CategoryPrefab, this.CategoryContainer).GetComponent<RectTransform>();
			FilterConfigPanel.SearchCategory searchCategory = new FilterConfigPanel.SearchCategory
			{
				Category = category,
				Container = component
			};
			component.Find("Text").GetComponent<TextMeshProUGUI>().text = category.ToString();
			this.searchCategories.Add(searchCategory);
			this.searchCategories.Sort((FilterConfigPanel.SearchCategory a, FilterConfigPanel.SearchCategory b) => a.Category.ToString().CompareTo(b.Category.ToString()));
			for (int j = 0; j < this.searchCategories.Count; j++)
			{
				this.searchCategories[j].Container.SetSiblingIndex(j);
			}
			return searchCategory;
		}

		// Token: 0x06004FC5 RID: 20421 RVA: 0x00150B81 File Offset: 0x0014ED81
		private void OpenSearch()
		{
			this.SearchInput.text = "";
			this.SearchContainer.gameObject.SetActive(true);
			this.RefreshSearchResults();
			this.CloseDropdown();
			this.SearchInput.Select();
		}

		// Token: 0x06004FC6 RID: 20422 RVA: 0x00150BBB File Offset: 0x0014EDBB
		private void CloseSearch()
		{
			this.SearchContainer.gameObject.SetActive(false);
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x00150BCE File Offset: 0x0014EDCE
		private void SearchChanged(string search)
		{
			this.RefreshSearchResults();
		}

		// Token: 0x06004FC8 RID: 20424 RVA: 0x00150BD8 File Offset: 0x0014EDD8
		private void RefreshSearchResults()
		{
			foreach (FilterConfigPanel.SearchCategory searchCategory in this.searchCategories)
			{
				searchCategory.SetSearch(this.SearchInput.text);
			}
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x00150C52 File Offset: 0x0014EE52
		[CompilerGenerated]
		private IEnumerator <Open>g__Open|39_0()
		{
			this.ListScrollRect.verticalNormalizedPosition = 1f;
			yield return new WaitForEndOfFrame();
			this.ListScrollRect.verticalNormalizedPosition = 1f;
			yield break;
		}

		// Token: 0x04003BB6 RID: 15286
		public GameObject ItemEntryPrefab;

		// Token: 0x04003BB7 RID: 15287
		public GameObject CategoryPrefab;

		// Token: 0x04003BB8 RID: 15288
		public GameObject SearchItemPrefab;

		// Token: 0x04003BB9 RID: 15289
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x04003BBA RID: 15290
		public GameObject Container;

		// Token: 0x04003BBB RID: 15291
		public Button TypeButton_None;

		// Token: 0x04003BBC RID: 15292
		public Button TypeButton_Whitelist;

		// Token: 0x04003BBD RID: 15293
		public Button TypeButton_Blacklist;

		// Token: 0x04003BBE RID: 15294
		public TextMeshProUGUI TypeLabel;

		// Token: 0x04003BBF RID: 15295
		public TextMeshProUGUI ListLabel;

		// Token: 0x04003BC0 RID: 15296
		public RectTransform ListContainer;

		// Token: 0x04003BC1 RID: 15297
		public GameObject ListBlocker;

		// Token: 0x04003BC2 RID: 15298
		public Button[] QualityButtons;

		// Token: 0x04003BC3 RID: 15299
		public ScrollRect ListScrollRect;

		// Token: 0x04003BC4 RID: 15300
		public RectTransform Dropdown;

		// Token: 0x04003BC5 RID: 15301
		public Button CopyButton;

		// Token: 0x04003BC6 RID: 15302
		public Button PasteButton;

		// Token: 0x04003BC7 RID: 15303
		public Button ApplyToSiblingsButton;

		// Token: 0x04003BC8 RID: 15304
		public Button ClearButton;

		// Token: 0x04003BC9 RID: 15305
		[Header("Search")]
		public RectTransform SearchContainer;

		// Token: 0x04003BCA RID: 15306
		public TMP_InputField SearchInput;

		// Token: 0x04003BCB RID: 15307
		public RectTransform CategoryContainer;

		// Token: 0x04003BCC RID: 15308
		private bool mouseUp;

		// Token: 0x04003BCD RID: 15309
		private List<FilterConfigPanel.SearchCategory> searchCategories = new List<FilterConfigPanel.SearchCategory>();

		// Token: 0x04003BCE RID: 15310
		private List<RectTransform> itemEntries = new List<RectTransform>();

		// Token: 0x04003BCF RID: 15311
		private static SlotFilter copiedFilter;

		// Token: 0x02000BB9 RID: 3001
		public class SearchCategory
		{
			// Token: 0x06004FCB RID: 20427 RVA: 0x00150C64 File Offset: 0x0014EE64
			public void AddItem(ItemDefinition item, RectTransform entry)
			{
				FilterConfigPanel.SearchCategory.Item item2 = new FilterConfigPanel.SearchCategory.Item
				{
					ItemDefinition = item,
					Entry = entry
				};
				this.Items.Add(item2);
			}

			// Token: 0x06004FCC RID: 20428 RVA: 0x00150C94 File Offset: 0x0014EE94
			public void SetSearch(string search)
			{
				bool flag = this.Category.ToString().ToLower() == search.ToLower();
				int num = 0;
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (flag || this.Items[i].ItemDefinition.Name.ToLower().Contains(search.ToLower()))
					{
						this.Items[i].Entry.gameObject.SetActive(true);
						num++;
					}
					else
					{
						this.Items[i].Entry.gameObject.SetActive(false);
					}
				}
				this.Container.gameObject.SetActive(num > 0);
			}

			// Token: 0x06004FCD RID: 20429 RVA: 0x00150D58 File Offset: 0x0014EF58
			public FilterConfigPanel.SearchCategory.Item GetItem(string itemID)
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].ItemDefinition.ID == itemID)
					{
						return this.Items[i];
					}
				}
				return null;
			}

			// Token: 0x04003BD0 RID: 15312
			public EItemCategory Category;

			// Token: 0x04003BD1 RID: 15313
			public RectTransform Container;

			// Token: 0x04003BD2 RID: 15314
			public List<FilterConfigPanel.SearchCategory.Item> Items = new List<FilterConfigPanel.SearchCategory.Item>();

			// Token: 0x02000BBA RID: 3002
			public class Item
			{
				// Token: 0x04003BD3 RID: 15315
				public ItemDefinition ItemDefinition;

				// Token: 0x04003BD4 RID: 15316
				public RectTransform Entry;
			}
		}
	}
}
