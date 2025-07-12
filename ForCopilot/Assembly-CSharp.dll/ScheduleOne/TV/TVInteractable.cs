using System;
using ScheduleOne.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x020002B4 RID: 692
	public class TVInteractable : MonoBehaviour
	{
		// Token: 0x06000E81 RID: 3713 RVA: 0x00040528 File Offset: 0x0003E728
		private void Start()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00040562 File Offset: 0x0003E762
		private void Hovered()
		{
			if (this.Interface.CanOpen())
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.IntObj.SetMessage("Use TV");
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0004059A File Offset: 0x0003E79A
		private void Interacted()
		{
			if (this.Interface.CanOpen())
			{
				this.Interface.Open();
			}
		}

		// Token: 0x04000F02 RID: 3842
		public InteractableObject IntObj;

		// Token: 0x04000F03 RID: 3843
		public TVInterface Interface;
	}
}
