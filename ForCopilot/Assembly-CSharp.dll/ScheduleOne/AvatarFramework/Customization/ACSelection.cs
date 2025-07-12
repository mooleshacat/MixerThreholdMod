using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009E5 RID: 2533
	public abstract class ACSelection<T> : MonoBehaviour where T : UnityEngine.Object
	{
		// Token: 0x06004454 RID: 17492 RVA: 0x0011F070 File Offset: 0x0011D270
		protected virtual void Awake()
		{
			for (int i = 0; i < this.Options.Count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ButtonPrefab, base.transform);
				gameObject.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = this.GetOptionLabel(i);
				this.buttons.Add(gameObject);
				int index = i;
				gameObject.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.SelectOption(index, true);
				});
			}
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0011F108 File Offset: 0x0011D308
		public void SelectOption(int index, bool notify = true)
		{
			int selectedOptionIndex = this.SelectedOptionIndex;
			if (index != this.SelectedOptionIndex)
			{
				if (this.SelectedOptionIndex != -1)
				{
					this.SetButtonHighlighted(this.SelectedOptionIndex, false);
				}
				this.SelectedOptionIndex = index;
				this.SetButtonHighlighted(this.SelectedOptionIndex, true);
			}
			else if (this.Nullable)
			{
				this.SetButtonHighlighted(this.SelectedOptionIndex, false);
				this.SelectedOptionIndex = -1;
			}
			if (selectedOptionIndex != this.SelectedOptionIndex && notify)
			{
				this.CallValueChange();
			}
		}

		// Token: 0x06004456 RID: 17494
		public abstract void CallValueChange();

		// Token: 0x06004457 RID: 17495
		public abstract string GetOptionLabel(int index);

		// Token: 0x06004458 RID: 17496
		public abstract int GetAssetPathIndex(string path);

		// Token: 0x06004459 RID: 17497 RVA: 0x0011F181 File Offset: 0x0011D381
		private void SetButtonHighlighted(int buttonIndex, bool h)
		{
			if (buttonIndex == -1)
			{
				return;
			}
			this.buttons[buttonIndex].transform.Find("Indicator").gameObject.SetActive(h);
		}

		// Token: 0x0400312C RID: 12588
		[Header("References")]
		public GameObject ButtonPrefab;

		// Token: 0x0400312D RID: 12589
		[Header("Settings")]
		public int PropertyIndex;

		// Token: 0x0400312E RID: 12590
		public List<T> Options = new List<T>();

		// Token: 0x0400312F RID: 12591
		public bool Nullable = true;

		// Token: 0x04003130 RID: 12592
		public int DefaultOptionIndex;

		// Token: 0x04003131 RID: 12593
		protected List<GameObject> buttons = new List<GameObject>();

		// Token: 0x04003132 RID: 12594
		protected int SelectedOptionIndex = -1;

		// Token: 0x04003133 RID: 12595
		public UnityEvent<T> onValueChange;

		// Token: 0x04003134 RID: 12596
		public UnityEvent<T, int> onValueChangeWithIndex;
	}
}
