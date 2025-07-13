using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Handover
{
	// Token: 0x02000B7A RID: 2938
	public class HandoverScreenPriceSelector : MonoBehaviour
	{
		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06004DF2 RID: 19954 RVA: 0x00149BC0 File Offset: 0x00147DC0
		// (set) Token: 0x06004DF3 RID: 19955 RVA: 0x00149BC8 File Offset: 0x00147DC8
		public float Price { get; private set; } = 1f;

		// Token: 0x06004DF4 RID: 19956 RVA: 0x00149BD4 File Offset: 0x00147DD4
		public void SetPrice(float price)
		{
			this.Price = Mathf.Clamp(price, 1f, 9999f);
			this.InputField.SetTextWithoutNotify(this.Price.ToString());
			if (this.onPriceChanged != null)
			{
				this.onPriceChanged.Invoke();
			}
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x00149C23 File Offset: 0x00147E23
		public void RefreshPrice()
		{
			this.OnPriceInputChanged(this.InputField.text);
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x00149C38 File Offset: 0x00147E38
		public void OnPriceInputChanged(string value)
		{
			float value2;
			if (float.TryParse(value, out value2))
			{
				this.Price = Mathf.Clamp(value2, 1f, 9999f);
			}
			this.InputField.SetTextWithoutNotify(this.Price.ToString());
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x00149C7E File Offset: 0x00147E7E
		public void ChangeAmount(float change)
		{
			this.SetPrice(this.Price + change);
		}

		// Token: 0x04003A49 RID: 14921
		public const float MinPrice = 1f;

		// Token: 0x04003A4A RID: 14922
		public const float MaxPrice = 9999f;

		// Token: 0x04003A4B RID: 14923
		public InputField InputField;

		// Token: 0x04003A4D RID: 14925
		public UnityEvent onPriceChanged;
	}
}
