using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000BA3 RID: 2979
	public class ShopAmountSelector : MonoBehaviour
	{
		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06004F1A RID: 20250 RVA: 0x0014E179 File Offset: 0x0014C379
		// (set) Token: 0x06004F1B RID: 20251 RVA: 0x0014E181 File Offset: 0x0014C381
		public bool IsOpen { get; private set; }

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06004F1C RID: 20252 RVA: 0x0014E18A File Offset: 0x0014C38A
		// (set) Token: 0x06004F1D RID: 20253 RVA: 0x0014E192 File Offset: 0x0014C392
		public int SelectedAmount { get; private set; } = 1;

		// Token: 0x06004F1E RID: 20254 RVA: 0x0014E19C File Offset: 0x0014C39C
		private void Awake()
		{
			this.Container.gameObject.SetActive(false);
			this.InputField.onSubmit.AddListener(new UnityAction<string>(this.OnSubmitted));
			this.InputField.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x0014E1F2 File Offset: 0x0014C3F2
		public void Open()
		{
			this.Container.gameObject.SetActive(true);
			this.Container.SetAsLastSibling();
			this.InputField.text = string.Empty;
			this.InputField.Select();
			this.IsOpen = true;
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x0014E232 File Offset: 0x0014C432
		public void Close()
		{
			this.Container.gameObject.SetActive(false);
			this.IsOpen = false;
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x0014E24C File Offset: 0x0014C44C
		private void OnSubmitted(string value)
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.OnValueChanged(value);
			if (this.onSubmitted != null)
			{
				this.onSubmitted.Invoke(this.SelectedAmount);
			}
			this.Close();
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x0014E280 File Offset: 0x0014C480
		private void OnValueChanged(string value)
		{
			int value2;
			if (int.TryParse(value, out value2))
			{
				this.SelectedAmount = Mathf.Clamp(value2, 1, 999);
				this.InputField.SetTextWithoutNotify(this.SelectedAmount.ToString());
				return;
			}
			this.SelectedAmount = 1;
			this.InputField.SetTextWithoutNotify(string.Empty);
		}

		// Token: 0x04003B4B RID: 15179
		[Header("References")]
		public RectTransform Container;

		// Token: 0x04003B4C RID: 15180
		public TMP_InputField InputField;

		// Token: 0x04003B4D RID: 15181
		public UnityEvent<int> onSubmitted;
	}
}
