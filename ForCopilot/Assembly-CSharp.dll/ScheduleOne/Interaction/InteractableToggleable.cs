using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Interaction
{
	// Token: 0x0200064D RID: 1613
	public class InteractableToggleable : MonoBehaviour
	{
		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x060029CE RID: 10702 RVA: 0x000ACD0C File Offset: 0x000AAF0C
		// (set) Token: 0x060029CF RID: 10703 RVA: 0x000ACD14 File Offset: 0x000AAF14
		public bool IsActivated { get; private set; }

		// Token: 0x060029D0 RID: 10704 RVA: 0x000ACD1D File Offset: 0x000AAF1D
		public void Start()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x000ACD58 File Offset: 0x000AAF58
		public void Hovered()
		{
			if (Time.time - this.lastActivated < this.CoolDown)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.IntObj.SetMessage(this.IsActivated ? this.DeactivateMessage : this.ActivateMessage);
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x000ACDB3 File Offset: 0x000AAFB3
		public void Interacted()
		{
			this.Toggle();
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x000ACDBC File Offset: 0x000AAFBC
		public void Toggle()
		{
			this.lastActivated = Time.time;
			this.IsActivated = !this.IsActivated;
			if (this.onToggle != null)
			{
				this.onToggle.Invoke();
			}
			if (this.IsActivated)
			{
				this.onActivate.Invoke();
				return;
			}
			this.onDeactivate.Invoke();
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x000ACE18 File Offset: 0x000AB018
		public void SetState(bool activated)
		{
			if (this.IsActivated == activated)
			{
				return;
			}
			this.lastActivated = Time.time;
			this.IsActivated = !this.IsActivated;
			if (this.IsActivated)
			{
				this.onActivate.Invoke();
				return;
			}
			this.onDeactivate.Invoke();
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x000ACE68 File Offset: 0x000AB068
		public void PoliceDetected()
		{
			if (!this.IsActivated)
			{
				this.Toggle();
			}
		}

		// Token: 0x04001E5E RID: 7774
		public string ActivateMessage = "Activate";

		// Token: 0x04001E5F RID: 7775
		public string DeactivateMessage = "Deactivate";

		// Token: 0x04001E60 RID: 7776
		public float CoolDown;

		// Token: 0x04001E61 RID: 7777
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04001E62 RID: 7778
		public UnityEvent onToggle = new UnityEvent();

		// Token: 0x04001E63 RID: 7779
		public UnityEvent onActivate = new UnityEvent();

		// Token: 0x04001E64 RID: 7780
		public UnityEvent onDeactivate = new UnityEvent();

		// Token: 0x04001E65 RID: 7781
		private float lastActivated;
	}
}
