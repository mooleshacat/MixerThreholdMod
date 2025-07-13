using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using ScheduleOne.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations.Drying_rack
{
	// Token: 0x02000AB2 RID: 2738
	public class DryingOperationUI : MonoBehaviour
	{
		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x060049A5 RID: 18853 RVA: 0x00136170 File Offset: 0x00134370
		// (set) Token: 0x060049A6 RID: 18854 RVA: 0x00136178 File Offset: 0x00134378
		public DryingOperation AssignedOperation { get; protected set; }

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x060049A7 RID: 18855 RVA: 0x00136181 File Offset: 0x00134381
		// (set) Token: 0x060049A8 RID: 18856 RVA: 0x00136189 File Offset: 0x00134389
		public RectTransform Alignment { get; private set; }

		// Token: 0x060049A9 RID: 18857 RVA: 0x00136192 File Offset: 0x00134392
		public void SetOperation(DryingOperation operation)
		{
			this.AssignedOperation = operation;
			this.Icon.sprite = Registry.GetItem(operation.ItemID).Icon;
			this.RefreshQuantity();
			this.UpdatePosition();
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x001361C2 File Offset: 0x001343C2
		public void SetAlignment(RectTransform alignment)
		{
			this.Alignment = alignment;
			base.transform.SetParent(alignment);
			this.UpdatePosition();
		}

		// Token: 0x060049AB RID: 18859 RVA: 0x001361DD File Offset: 0x001343DD
		public void RefreshQuantity()
		{
			this.QuantityLabel.text = this.AssignedOperation.Quantity.ToString() + "x";
		}

		// Token: 0x060049AC RID: 18860 RVA: 0x00136204 File Offset: 0x00134404
		public void Start()
		{
			this.Button.onClick.AddListener(delegate()
			{
				this.Clicked();
			});
		}

		// Token: 0x060049AD RID: 18861 RVA: 0x00136224 File Offset: 0x00134424
		public void UpdatePosition()
		{
			float t = Mathf.Clamp01((float)this.AssignedOperation.Time / 720f);
			int num = Mathf.Clamp(720 - this.AssignedOperation.Time, 0, 720);
			int num2 = num / 60;
			int num3 = num % 60;
			this.Tooltip.text = num2.ToString() + "h " + num3.ToString() + "m until next tier";
			float num4 = -62.5f;
			float b = -num4;
			this.Rect.anchoredPosition = new Vector2(Mathf.Lerp(num4, b, t), 0f);
		}

		// Token: 0x060049AE RID: 18862 RVA: 0x001362C0 File Offset: 0x001344C0
		private void Clicked()
		{
			Singleton<DryingRackCanvas>.Instance.Rack.TryEndOperation(Singleton<DryingRackCanvas>.Instance.Rack.DryingOperations.IndexOf(this.AssignedOperation), true, this.AssignedOperation.GetQuality(), UnityEngine.Random.Range(int.MinValue, int.MaxValue));
		}

		// Token: 0x04003646 RID: 13894
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x04003647 RID: 13895
		public Image Icon;

		// Token: 0x04003648 RID: 13896
		public TextMeshProUGUI QuantityLabel;

		// Token: 0x04003649 RID: 13897
		public Button Button;

		// Token: 0x0400364A RID: 13898
		public Tooltip Tooltip;
	}
}
