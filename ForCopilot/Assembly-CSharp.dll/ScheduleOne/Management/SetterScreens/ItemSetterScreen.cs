using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management.Presets.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.Management.SetterScreens
{
	// Token: 0x020005CE RID: 1486
	public class ItemSetterScreen : Singleton<ItemSetterScreen>
	{
		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x0600248B RID: 9355 RVA: 0x00095726 File Offset: 0x00093926
		// (set) Token: 0x0600248C RID: 9356 RVA: 0x0009572E File Offset: 0x0009392E
		public ItemList Option { get; private set; }

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x0600248D RID: 9357 RVA: 0x00095737 File Offset: 0x00093937
		public bool IsOpen
		{
			get
			{
				return this.Option != null;
			}
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x00095744 File Offset: 0x00093944
		protected override void Awake()
		{
			base.Awake();
			this.allEntry = this.CreateEntry(null, "All", new Action(this.AllClicked), "", false);
			this.noneEntry = this.CreateEntry(null, "None", new Action(this.NoneClicked), "", false);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
			this.Close();
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000957B8 File Offset: 0x000939B8
		public virtual void Open(ItemList option)
		{
			this.Option = option;
			this.TitleLabel.text = this.Option.Name;
			base.gameObject.SetActive(true);
			this.allEntry.gameObject.SetActive(option.CanBeAll);
			this.noneEntry.gameObject.SetActive(option.CanBeNone);
			this.CreateEntries();
			this.RefreshTicks();
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x00095826 File Offset: 0x00093A26
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.Close();
			}
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x00095850 File Offset: 0x00093A50
		public virtual void Close()
		{
			this.Option = null;
			this.DestroyEntries();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x0009586C File Offset: 0x00093A6C
		private RectTransform CreateEntry(Sprite icon, string label, Action onClick, string prefabID = "", bool createPair = false)
		{
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.ListEntryPrefab, this.EntryContainer).GetComponent<RectTransform>();
			if (icon == null)
			{
				component.Find("Icon").gameObject.SetActive(false);
				component.Find("Title").GetComponent<RectTransform>().offsetMin = new Vector2(0.5f, 0f);
			}
			else
			{
				component.Find("Icon").GetComponent<Image>().sprite = icon;
			}
			component.Find("Title").GetComponent<TextMeshProUGUI>().text = label;
			component.GetComponent<Button>().onClick.AddListener(delegate()
			{
				onClick();
			});
			if (createPair)
			{
				this.pairs.Add(new ItemSetterScreen.Pair
				{
					prefabID = prefabID,
					entry = component
				});
			}
			return component;
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x0009594E File Offset: 0x00093B4E
		private void AllClicked()
		{
			this.Option.All = true;
			this.Option.None = false;
			this.RefreshTicks();
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x0009596E File Offset: 0x00093B6E
		private void NoneClicked()
		{
			this.Option.All = false;
			this.Option.None = true;
			this.Option.Selection.Clear();
			this.RefreshTicks();
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000959A0 File Offset: 0x00093BA0
		private void EntryClicked(string prefabID)
		{
			if (this.Option.All)
			{
				this.Option.Selection.Clear();
				this.Option.Selection.AddRange(this.Option.OptionList);
				this.Option.Selection.Remove(prefabID);
			}
			else if (this.Option.Selection.Contains(prefabID))
			{
				this.Option.Selection.Remove(prefabID);
			}
			else
			{
				this.Option.Selection.Add(prefabID);
			}
			this.Option.All = false;
			this.Option.None = false;
			this.RefreshTicks();
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x00095A50 File Offset: 0x00093C50
		private void RefreshTicks()
		{
			this.SetEntryTicked(this.allEntry, false);
			this.SetEntryTicked(this.noneEntry, false);
			for (int k = 0; k < this.pairs.Count; k++)
			{
				this.SetEntryTicked(this.pairs[k].entry, false);
			}
			if (this.Option.All)
			{
				this.SetEntryTicked(this.allEntry, true);
				for (int j = 0; j < this.pairs.Count; j++)
				{
					this.SetEntryTicked(this.pairs[j].entry, true);
				}
				return;
			}
			if (this.Option.None || this.Option.Selection.Count == 0)
			{
				this.SetEntryTicked(this.noneEntry, true);
				return;
			}
			int i;
			Predicate<ItemSetterScreen.Pair> <>9__0;
			int i2;
			for (i = 0; i < this.Option.Selection.Count; i = i2 + 1)
			{
				List<ItemSetterScreen.Pair> list = this.pairs;
				Predicate<ItemSetterScreen.Pair> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((ItemSetterScreen.Pair x) => x.prefabID == this.Option.Selection[i]));
				}
				this.SetEntryTicked(list.Find(match).entry, true);
				i2 = i;
			}
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x00095B91 File Offset: 0x00093D91
		private void SetEntryTicked(RectTransform entry, bool ticked)
		{
			entry.Find("Tick").gameObject.SetActive(ticked);
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x00095BAC File Offset: 0x00093DAC
		private void CreateEntries()
		{
			for (int i = 0; i < this.Option.OptionList.Count; i++)
			{
				Console.Log(this.Option.OptionList[i], null);
			}
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x00095BEC File Offset: 0x00093DEC
		private void DestroyEntries()
		{
			foreach (ItemSetterScreen.Pair pair in this.pairs)
			{
				UnityEngine.Object.Destroy(pair.entry.gameObject);
			}
			this.pairs.Clear();
		}

		// Token: 0x04001B12 RID: 6930
		[Header("Prefabs")]
		public GameObject ListEntryPrefab;

		// Token: 0x04001B13 RID: 6931
		[Header("References")]
		public RectTransform EntryContainer;

		// Token: 0x04001B14 RID: 6932
		public TextMeshProUGUI TitleLabel;

		// Token: 0x04001B15 RID: 6933
		private RectTransform allEntry;

		// Token: 0x04001B16 RID: 6934
		private RectTransform noneEntry;

		// Token: 0x04001B17 RID: 6935
		private List<ItemSetterScreen.Pair> pairs = new List<ItemSetterScreen.Pair>();

		// Token: 0x020005CF RID: 1487
		private class Pair
		{
			// Token: 0x04001B18 RID: 6936
			public string prefabID;

			// Token: 0x04001B19 RID: 6937
			public RectTransform entry;
		}
	}
}
