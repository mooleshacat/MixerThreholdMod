using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCustomization
{
	// Token: 0x02000B80 RID: 2944
	public class CharacterCustomizationCategory : MonoBehaviour
	{
		// Token: 0x06004E18 RID: 19992 RVA: 0x0014A544 File Offset: 0x00148744
		private void Awake()
		{
			this.ui = base.GetComponentInParent<CharacterCustomizationUI>();
			this.options = base.GetComponentsInChildren<CharacterCustomizationOption>(true);
			this.TitleText.text = this.CategoryName;
			this.BackButton.onClick.AddListener(new UnityAction(this.Back));
			for (int i = 0; i < this.options.Length; i++)
			{
				CharacterCustomizationOption option = this.options[i];
				this.options[i].onSelect.AddListener(delegate()
				{
					this.OptionSelected(option);
				});
				this.options[i].onDeselect.AddListener(delegate()
				{
					this.OptionDeselected(option);
				});
				this.options[i].onPurchase.AddListener(delegate()
				{
					this.OptionPurchased(option);
				});
			}
			for (int j = 0; j < this.options.Length; j++)
			{
				for (int k = j + 1; k < this.options.Length; k++)
				{
					if (this.options[k].Price < this.options[j].Price)
					{
						Transform transform = this.options[j].transform;
						this.options[j].transform.SetSiblingIndex(k);
						this.options[k].transform.SetSiblingIndex(j);
					}
				}
			}
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x0014A69C File Offset: 0x0014889C
		public void Open()
		{
			bool flag = false;
			for (int i = 0; i < this.options.Length; i++)
			{
				if (this.ui.IsOptionCurrentlyApplied(this.options[i]))
				{
					flag = true;
					this.options[i].SetPurchased(true);
				}
				else
				{
					this.options[i].SetPurchased(false);
					this.options[i].SetSelected(false);
				}
			}
			if (!flag && this.options.Length != 0)
			{
				this.options[0].SetPurchased(true);
			}
			this.ScrollRect.verticalScrollbar.value = 1f;
			if (this.onOpen != null)
			{
				this.onOpen.Invoke();
			}
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x0014A744 File Offset: 0x00148944
		public void Back()
		{
			this.ui.SetActiveCategory(null);
			for (int i = 0; i < this.options.Length; i++)
			{
				this.options[i].ParentCategoryClosed();
			}
			if (this.onClose != null)
			{
				this.onClose.Invoke();
			}
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x0014A790 File Offset: 0x00148990
		private void OptionSelected(CharacterCustomizationOption option)
		{
			this.ui.OptionSelected(option);
			for (int i = 0; i < this.options.Length; i++)
			{
				this.options[i].SiblingOptionSelected(option);
			}
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x0014A7CA File Offset: 0x001489CA
		private void OptionDeselected(CharacterCustomizationOption option)
		{
			this.ui.OptionDeselected(option);
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x0014A7D8 File Offset: 0x001489D8
		private void OptionPurchased(CharacterCustomizationOption option)
		{
			this.ui.OptionPurchased(option);
			for (int i = 0; i < this.options.Length; i++)
			{
				this.options[i].SiblingOptionPurchased(option);
			}
		}

		// Token: 0x04003A74 RID: 14964
		public string CategoryName;

		// Token: 0x04003A75 RID: 14965
		[Header("References")]
		public TextMeshProUGUI TitleText;

		// Token: 0x04003A76 RID: 14966
		public Button BackButton;

		// Token: 0x04003A77 RID: 14967
		public ScrollRect ScrollRect;

		// Token: 0x04003A78 RID: 14968
		private CharacterCustomizationUI ui;

		// Token: 0x04003A79 RID: 14969
		private CharacterCustomizationOption[] options;

		// Token: 0x04003A7A RID: 14970
		public UnityEvent onOpen;

		// Token: 0x04003A7B RID: 14971
		public UnityEvent onClose;
	}
}
