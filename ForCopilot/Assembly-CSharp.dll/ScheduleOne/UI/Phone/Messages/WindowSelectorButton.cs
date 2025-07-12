using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000B0F RID: 2831
	public class WindowSelectorButton : MonoBehaviour
	{
		// Token: 0x06004BDD RID: 19421 RVA: 0x0013EC03 File Offset: 0x0013CE03
		private void Awake()
		{
			this.HoverIndicator.gameObject.SetActive(true);
			this.HoverIndicator.localScale = Vector3.one;
			this.Button.onClick.AddListener(new UnityAction(this.Clicked));
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x0013EC42 File Offset: 0x0013CE42
		public void SetInteractable(bool interactable)
		{
			this.Button.interactable = interactable;
			this.InactiveOverlay.SetActive(!interactable);
			if (!interactable)
			{
				this.SetHoverIndicator(false);
			}
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x0013EC69 File Offset: 0x0013CE69
		public void HoverStart()
		{
			if (!this.Button.interactable)
			{
				return;
			}
			this.SetHoverIndicator(true);
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x0013EC80 File Offset: 0x0013CE80
		public void HoverEnd()
		{
			if (!this.Button.interactable)
			{
				return;
			}
			this.SetHoverIndicator(false);
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x0013EC97 File Offset: 0x0013CE97
		public void Clicked()
		{
			if (this.OnSelected != null)
			{
				this.OnSelected.Invoke();
			}
		}

		// Token: 0x06004BE2 RID: 19426 RVA: 0x0013ECAC File Offset: 0x0013CEAC
		public void SetHoverIndicator(bool shown)
		{
			WindowSelectorButton.<>c__DisplayClass13_0 CS$<>8__locals1 = new WindowSelectorButton.<>c__DisplayClass13_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.shown = shown;
			if (this.hoverRoutine != null)
			{
				base.StopCoroutine(this.hoverRoutine);
			}
			this.hoverRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetHoverIndicator>g__Routine|0());
		}

		// Token: 0x0400385B RID: 14427
		public const float SELECTION_INDICATOR_SCALE = 1.1f;

		// Token: 0x0400385C RID: 14428
		public const float INDICATOR_LERP_TIME = 0.075f;

		// Token: 0x0400385D RID: 14429
		public UnityEvent OnSelected;

		// Token: 0x0400385E RID: 14430
		public EDealWindow WindowType;

		// Token: 0x0400385F RID: 14431
		[Header("References")]
		public Button Button;

		// Token: 0x04003860 RID: 14432
		public GameObject InactiveOverlay;

		// Token: 0x04003861 RID: 14433
		public RectTransform HoverIndicator;

		// Token: 0x04003862 RID: 14434
		private Coroutine hoverRoutine;
	}
}
