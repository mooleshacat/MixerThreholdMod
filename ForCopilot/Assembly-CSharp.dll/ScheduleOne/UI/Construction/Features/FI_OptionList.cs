using System;
using System.Collections.Generic;
using ScheduleOne.Construction.Features;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Construction.Features
{
	// Token: 0x02000BD7 RID: 3031
	public class FI_OptionList : FI_Base
	{
		// Token: 0x06005074 RID: 20596 RVA: 0x001546CC File Offset: 0x001528CC
		public virtual void Initialize(OptionListFeature _feature, List<FI_OptionList.Option> _options)
		{
			base.Initialize(_feature);
			this.specificFeature = _feature;
			this.options.AddRange(_options);
			this.selectionIndex = this.specificFeature.SyncAccessor_ownedOptionIndex;
			for (int i = 0; i < this.options.Count; i++)
			{
				Button component = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, this.buttonContainer).GetComponent<Button>();
				component.GetComponent<Image>().color = this.options[i].optionColor;
				component.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = this.options[i].optionLabel;
				int index = i;
				component.onClick.AddListener(delegate()
				{
					this.Select(index);
				});
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.buttonContainer);
			this.bar.anchoredPosition = new Vector2(this.bar.anchoredPosition.x, this.buttonContainer.GetChild(this.buttonContainer.childCount - 1).GetComponent<RectTransform>().anchoredPosition.y - 35f);
			this.UpdateSelection();
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x00154805 File Offset: 0x00152A05
		public override void Close()
		{
			this.Select(this.specificFeature.SyncAccessor_ownedOptionIndex);
			base.Close();
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x00154820 File Offset: 0x00152A20
		public void BuyButtonClicked()
		{
			if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance < this.options[this.selectionIndex].optionPrice)
			{
				return;
			}
			NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction(Singleton<ConstructionMenu>.Instance.SelectedConstructable.ConstructableName + ": " + this.feature.featureName, -this.options[this.selectionIndex].optionPrice, 1f, string.Empty);
			if (this.onSelectionPurchased != null)
			{
				this.onSelectionPurchased.Invoke(this.selectionIndex);
			}
			this.UpdateSelection();
		}

		// Token: 0x06005077 RID: 20599 RVA: 0x001548BE File Offset: 0x00152ABE
		public void Select(int index)
		{
			this.selectionIndex = Mathf.Clamp(index, 0, this.options.Count - 1);
			if (this.onSelectionChanged != null)
			{
				this.onSelectionChanged.Invoke(this.selectionIndex);
			}
			this.UpdateSelection();
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x001548FC File Offset: 0x00152AFC
		private void UpdateSelection()
		{
			for (int i = 0; i < this.buttonContainer.childCount; i++)
			{
				this.buttonContainer.GetChild(i).Find("SelectionIndicator").gameObject.SetActive(false);
				this.buttonContainer.GetChild(i).Find("OwnedIndicator").gameObject.SetActive(false);
			}
			this.buttonContainer.GetChild(this.selectionIndex).Find("SelectionIndicator").gameObject.SetActive(true);
			this.buttonContainer.GetChild(this.specificFeature.SyncAccessor_ownedOptionIndex).Find("OwnedIndicator").gameObject.SetActive(true);
			if (this.selectionIndex != this.specificFeature.SyncAccessor_ownedOptionIndex)
			{
				this.buyButtonText.text = "Buy (" + MoneyManager.FormatAmount(this.options[this.selectionIndex].optionPrice, false, false) + ")";
				this.buyButton.gameObject.SetActive(true);
				return;
			}
			this.buyButton.gameObject.SetActive(false);
		}

		// Token: 0x04003C6B RID: 15467
		[Header("References")]
		[SerializeField]
		protected RectTransform buttonContainer;

		// Token: 0x04003C6C RID: 15468
		[SerializeField]
		protected Button buyButton;

		// Token: 0x04003C6D RID: 15469
		[SerializeField]
		protected TextMeshProUGUI buyButtonText;

		// Token: 0x04003C6E RID: 15470
		[SerializeField]
		protected RectTransform bar;

		// Token: 0x04003C6F RID: 15471
		[Header("Prefab")]
		[SerializeField]
		protected GameObject buttonPrefab;

		// Token: 0x04003C70 RID: 15472
		public UnityEvent<int> onSelectionChanged;

		// Token: 0x04003C71 RID: 15473
		public UnityEvent<int> onSelectionPurchased;

		// Token: 0x04003C72 RID: 15474
		private List<FI_OptionList.Option> options = new List<FI_OptionList.Option>();

		// Token: 0x04003C73 RID: 15475
		public OptionListFeature specificFeature;

		// Token: 0x04003C74 RID: 15476
		private int selectionIndex;

		// Token: 0x02000BD8 RID: 3032
		public class Option
		{
			// Token: 0x0600507A RID: 20602 RVA: 0x00154A32 File Offset: 0x00152C32
			public Option(string _optionLabel, Color _optionColor, float _optionPrice)
			{
				this.optionLabel = _optionLabel;
				this.optionColor = _optionColor;
				this.optionPrice = _optionPrice;
			}

			// Token: 0x04003C75 RID: 15477
			public string optionLabel;

			// Token: 0x04003C76 RID: 15478
			public Color optionColor;

			// Token: 0x04003C77 RID: 15479
			public float optionPrice;
		}
	}
}
