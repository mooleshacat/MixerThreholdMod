using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A02 RID: 2562
	[RequireComponent(typeof(Button))]
	[RequireComponent(typeof(EventTrigger))]
	public class ButtonScaler : MonoBehaviour
	{
		// Token: 0x060044EE RID: 17646 RVA: 0x00121620 File Offset: 0x0011F820
		private void Awake()
		{
			this.button = base.GetComponent<Button>();
			EventTrigger component = base.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = 0;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.Hovered();
			});
			component.triggers.Add(entry);
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = 1;
			entry2.callback.AddListener(delegate(BaseEventData data)
			{
				this.HoverEnd();
			});
			component.triggers.Add(entry2);
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x0012169E File Offset: 0x0011F89E
		private void Hovered()
		{
			if (!this.button.interactable)
			{
				return;
			}
			this.SetScale(this.HoverScale);
		}

		// Token: 0x060044F0 RID: 17648 RVA: 0x001216BA File Offset: 0x0011F8BA
		private void HoverEnd()
		{
			if (!this.button.interactable)
			{
				return;
			}
			this.SetScale(1f);
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x001216D8 File Offset: 0x0011F8D8
		private void SetScale(float endScale)
		{
			ButtonScaler.<>c__DisplayClass8_0 CS$<>8__locals1 = new ButtonScaler.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.endScale = endScale;
			if (this.scaleCoroutine != null)
			{
				base.StopCoroutine(this.scaleCoroutine);
			}
			this.scaleCoroutine = base.StartCoroutine(CS$<>8__locals1.<SetScale>g__Routine|0());
		}

		// Token: 0x040031C9 RID: 12745
		public RectTransform ScaleTarget;

		// Token: 0x040031CA RID: 12746
		public float HoverScale = 1.1f;

		// Token: 0x040031CB RID: 12747
		public float ScaleTime = 0.1f;

		// Token: 0x040031CC RID: 12748
		private Coroutine scaleCoroutine;

		// Token: 0x040031CD RID: 12749
		private Button button;
	}
}
