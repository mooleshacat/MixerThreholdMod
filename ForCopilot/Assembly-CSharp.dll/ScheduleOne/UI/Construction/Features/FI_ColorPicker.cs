using System;
using ScheduleOne.Construction.Features;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Construction.Features
{
	// Token: 0x02000BD4 RID: 3028
	public class FI_ColorPicker : FI_Base
	{
		// Token: 0x06005069 RID: 20585 RVA: 0x001542F8 File Offset: 0x001524F8
		public override void Initialize(Feature _feature)
		{
			base.Initialize(_feature);
			this.specificFeature = (this.feature as ColorFeature);
			this.selectionIndex = this.specificFeature.SyncAccessor_ownedColorIndex;
			for (int i = 0; i < this.specificFeature.colors.Count; i++)
			{
				Button component = UnityEngine.Object.Instantiate<GameObject>(this.colorButtonPrefab, this.colorButtonContainer).GetComponent<Button>();
				component.GetComponent<Image>().color = this.specificFeature.colors[i].color;
				int index = i;
				component.onClick.AddListener(delegate()
				{
					this.Select(index);
				});
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.colorButtonContainer);
			this.bar.anchoredPosition = new Vector2(this.bar.anchoredPosition.x, this.colorButtonContainer.GetChild(this.colorButtonContainer.childCount - 1).GetComponent<RectTransform>().anchoredPosition.y - 35f);
			this.UpdateSelection();
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x00154408 File Offset: 0x00152608
		public override void Close()
		{
			this.Select(this.specificFeature.SyncAccessor_ownedColorIndex);
			base.Close();
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x00154424 File Offset: 0x00152624
		public void BuyButtonClicked()
		{
			if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance < this.specificFeature.colors[this.selectionIndex].price)
			{
				return;
			}
			NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction(Singleton<ConstructionMenu>.Instance.SelectedConstructable.ConstructableName + ": " + this.feature.featureName, -this.specificFeature.colors[this.selectionIndex].price, 1f, string.Empty);
			if (this.onSelectionPurchased != null)
			{
				this.onSelectionPurchased.Invoke(this.specificFeature.colors[this.selectionIndex]);
			}
			this.UpdateSelection();
		}

		// Token: 0x0600506C RID: 20588 RVA: 0x001544DC File Offset: 0x001526DC
		public void Select(int index)
		{
			this.selectionIndex = Mathf.Clamp(index, 0, this.specificFeature.colors.Count - 1);
			if (this.onSelectionChanged != null)
			{
				this.onSelectionChanged.Invoke(this.specificFeature.colors[this.selectionIndex]);
			}
			this.UpdateSelection();
		}

		// Token: 0x0600506D RID: 20589 RVA: 0x00154538 File Offset: 0x00152738
		private void UpdateSelection()
		{
			this.colorLabel.text = this.specificFeature.colors[this.selectionIndex].colorName;
			for (int i = 0; i < this.colorButtonContainer.childCount; i++)
			{
				this.colorButtonContainer.GetChild(i).Find("SelectionIndicator").gameObject.SetActive(false);
				this.colorButtonContainer.GetChild(i).Find("OwnedIndicator").gameObject.SetActive(false);
			}
			this.colorButtonContainer.GetChild(this.selectionIndex).Find("SelectionIndicator").gameObject.SetActive(true);
			this.colorButtonContainer.GetChild(this.specificFeature.SyncAccessor_ownedColorIndex).Find("OwnedIndicator").gameObject.SetActive(true);
			if (this.selectionIndex != this.specificFeature.SyncAccessor_ownedColorIndex)
			{
				this.buyButtonText.text = "Buy (" + MoneyManager.FormatAmount(this.specificFeature.colors[this.selectionIndex].price, false, false) + ")";
				this.buyButton.gameObject.SetActive(true);
				return;
			}
			this.buyButton.gameObject.SetActive(false);
		}

		// Token: 0x04003C5D RID: 15453
		[Header("References")]
		[SerializeField]
		protected RectTransform colorButtonContainer;

		// Token: 0x04003C5E RID: 15454
		[SerializeField]
		protected Button buyButton;

		// Token: 0x04003C5F RID: 15455
		[SerializeField]
		protected TextMeshProUGUI buyButtonText;

		// Token: 0x04003C60 RID: 15456
		[SerializeField]
		protected TextMeshProUGUI colorLabel;

		// Token: 0x04003C61 RID: 15457
		[SerializeField]
		protected RectTransform bar;

		// Token: 0x04003C62 RID: 15458
		[Header("Prefab")]
		[SerializeField]
		protected GameObject colorButtonPrefab;

		// Token: 0x04003C63 RID: 15459
		public UnityEvent<ColorFeature.NamedColor> onSelectionChanged;

		// Token: 0x04003C64 RID: 15460
		public UnityEvent<ColorFeature.NamedColor> onSelectionPurchased;

		// Token: 0x04003C65 RID: 15461
		private ColorFeature specificFeature;

		// Token: 0x04003C66 RID: 15462
		private int selectionIndex;
	}
}
