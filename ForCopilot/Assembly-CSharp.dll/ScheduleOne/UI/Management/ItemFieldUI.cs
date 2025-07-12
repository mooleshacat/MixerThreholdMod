using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B2E RID: 2862
	public class ItemFieldUI : MonoBehaviour
	{
		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06004C4C RID: 19532 RVA: 0x00141054 File Offset: 0x0013F254
		// (set) Token: 0x06004C4D RID: 19533 RVA: 0x0014105C File Offset: 0x0013F25C
		public List<ItemField> Fields { get; protected set; } = new List<ItemField>();

		// Token: 0x06004C4E RID: 19534 RVA: 0x00141068 File Offset: 0x0013F268
		public void Bind(List<ItemField> field)
		{
			this.Fields = new List<ItemField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onItemChanged.AddListener(new UnityAction<ItemDefinition>(this.Refresh));
			this.Refresh(this.Fields[0].SelectedItem);
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x001410D4 File Offset: 0x0013F2D4
		private void Refresh(ItemDefinition newVal)
		{
			this.IconImg.gameObject.SetActive(false);
			this.NoneSelected.gameObject.SetActive(false);
			this.MultipleSelected.gameObject.SetActive(false);
			if (!this.AreFieldsUniform())
			{
				this.MultipleSelected.SetActive(true);
				this.SelectionLabel.text = "Mixed";
				return;
			}
			if (newVal != null)
			{
				this.IconImg.sprite = newVal.Icon;
				this.SelectionLabel.text = newVal.Name;
				this.IconImg.gameObject.SetActive(true);
				return;
			}
			this.NoneSelected.SetActive(true);
			this.SelectionLabel.text = (this.ShowNoneAsAny ? "Any" : "None");
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x001411A4 File Offset: 0x0013F3A4
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (this.Fields[i].SelectedItem != this.Fields[i + 1].SelectedItem)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x001411F8 File Offset: 0x0013F3F8
		public void Clicked()
		{
			List<ItemSelector.Option> list = new List<ItemSelector.Option>();
			ItemSelector.Option selectedOption = null;
			bool flag = this.AreFieldsUniform();
			if (this.Fields[0].CanSelectNone)
			{
				list.Add(new ItemSelector.Option(this.ShowNoneAsAny ? "Any" : "None", null));
				if (flag && this.Fields[0].SelectedItem == null)
				{
					selectedOption = list[0];
				}
			}
			foreach (ItemDefinition itemDefinition in this.Fields[0].Options)
			{
				ItemSelector.Option option = new ItemSelector.Option(itemDefinition.Name, itemDefinition);
				list.Add(option);
				if (flag && this.Fields[0].SelectedItem == option.Item)
				{
					selectedOption = option;
				}
			}
			Singleton<ManagementInterface>.Instance.ItemSelectorScreen.Initialize(this.FieldLabel.text, list, selectedOption, new Action<ItemSelector.Option>(this.OptionSelected));
			Singleton<ManagementInterface>.Instance.ItemSelectorScreen.Open();
		}

		// Token: 0x06004C52 RID: 19538 RVA: 0x0014132C File Offset: 0x0013F52C
		private void OptionSelected(ItemSelector.Option option)
		{
			foreach (ItemField itemField in this.Fields)
			{
				itemField.SetItem(option.Item, true);
			}
		}

		// Token: 0x040038DF RID: 14559
		public bool ShowNoneAsAny;

		// Token: 0x040038E0 RID: 14560
		[Header("References")]
		public TextMeshProUGUI FieldLabel;

		// Token: 0x040038E1 RID: 14561
		public Image IconImg;

		// Token: 0x040038E2 RID: 14562
		public TextMeshProUGUI SelectionLabel;

		// Token: 0x040038E3 RID: 14563
		public GameObject NoneSelected;

		// Token: 0x040038E4 RID: 14564
		public GameObject MultipleSelected;
	}
}
