using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B3C RID: 2876
	public class ItemSelector : ClipboardScreen
	{
		// Token: 0x06004CAA RID: 19626 RVA: 0x00142BE8 File Offset: 0x00140DE8
		public void Initialize(string selectionTitle, List<ItemSelector.Option> _options, ItemSelector.Option _selectedOption = null, Action<ItemSelector.Option> _optionCallback = null)
		{
			this.TitleLabel.text = selectionTitle;
			this.options = new List<ItemSelector.Option>();
			this.options.AddRange(_options);
			this.selectedOption = _selectedOption;
			this.optionCallback = _optionCallback;
			this.DeleteOptions();
			this.CreateOptions(this.options);
			this.HoveredItemLabel.enabled = false;
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x00142C45 File Offset: 0x00140E45
		public override void Open()
		{
			base.Open();
			Singleton<ManagementInterface>.Instance.MainScreen.Close();
		}

		// Token: 0x06004CAC RID: 19628 RVA: 0x00142C5C File Offset: 0x00140E5C
		public override void Close()
		{
			base.Close();
			this.HoveredItemLabel.enabled = false;
			Singleton<ManagementInterface>.Instance.MainScreen.Open();
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x00142C7F File Offset: 0x00140E7F
		private void ButtonClicked(ItemSelector.Option option)
		{
			if (this.optionCallback != null)
			{
				this.optionCallback(option);
			}
			this.Close();
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x00142C9C File Offset: 0x00140E9C
		private void ButtonHovered(ItemSelector.Option option)
		{
			this.HoveredItemLabel.text = option.Title;
			this.HoveredItemLabel.enabled = true;
			this.HoveredItemLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -140f - Mathf.Ceil((float)this.optionButtons.Count / 5f) * this.optionButtons[0].sizeDelta.y);
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x00142D14 File Offset: 0x00140F14
		private void ButtonHoverEnd(ItemSelector.Option option)
		{
			this.HoveredItemLabel.enabled = false;
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x00142D24 File Offset: 0x00140F24
		private void CreateOptions(List<ItemSelector.Option> options)
		{
			for (int i = 0; i < options.Count; i++)
			{
				Button component = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab, this.OptionContainer).GetComponent<Button>();
				if (options[i].Item != null)
				{
					component.transform.Find("None").gameObject.SetActive(false);
					component.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = options[i].Item.Icon;
					component.transform.Find("Icon").gameObject.SetActive(true);
				}
				else
				{
					component.transform.Find("None").gameObject.SetActive(true);
					component.transform.Find("Icon").gameObject.SetActive(false);
				}
				if (options[i] == this.selectedOption)
				{
					component.transform.Find("Outline").gameObject.GetComponent<Image>().color = new Color32(90, 90, 90, byte.MaxValue);
				}
				ItemSelector.Option opt = options[i];
				component.onClick.AddListener(delegate()
				{
					this.ButtonClicked(opt);
				});
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = 0;
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					this.ButtonHovered(opt);
				});
				component.GetComponent<EventTrigger>().triggers.Add(entry);
				entry = new EventTrigger.Entry();
				entry.eventID = 1;
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					this.ButtonHoverEnd(opt);
				});
				component.GetComponent<EventTrigger>().triggers.Add(entry);
				this.optionButtons.Add(component.GetComponent<RectTransform>());
			}
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x00142F00 File Offset: 0x00141100
		private void DeleteOptions()
		{
			for (int i = 0; i < this.optionButtons.Count; i++)
			{
				UnityEngine.Object.Destroy(this.optionButtons[i].gameObject);
			}
			this.optionButtons.Clear();
		}

		// Token: 0x04003925 RID: 14629
		[Header("References")]
		public RectTransform OptionContainer;

		// Token: 0x04003926 RID: 14630
		public TextMeshProUGUI TitleLabel;

		// Token: 0x04003927 RID: 14631
		public TextMeshProUGUI HoveredItemLabel;

		// Token: 0x04003928 RID: 14632
		public GameObject OptionPrefab;

		// Token: 0x04003929 RID: 14633
		[Header("Settings")]
		public Sprite EmptyOptionSprite;

		// Token: 0x0400392A RID: 14634
		private Coroutine lerpRoutine;

		// Token: 0x0400392B RID: 14635
		private List<ItemSelector.Option> options = new List<ItemSelector.Option>();

		// Token: 0x0400392C RID: 14636
		private ItemSelector.Option selectedOption;

		// Token: 0x0400392D RID: 14637
		private List<RectTransform> optionButtons = new List<RectTransform>();

		// Token: 0x0400392E RID: 14638
		private Action<ItemSelector.Option> optionCallback;

		// Token: 0x02000B3D RID: 2877
		[Serializable]
		public class Option
		{
			// Token: 0x06004CB3 RID: 19635 RVA: 0x00142F62 File Offset: 0x00141162
			public Option(string title, ItemDefinition item)
			{
				this.Title = title;
				this.Item = item;
			}

			// Token: 0x0400392F RID: 14639
			public string Title;

			// Token: 0x04003930 RID: 14640
			public ItemDefinition Item;
		}
	}
}
