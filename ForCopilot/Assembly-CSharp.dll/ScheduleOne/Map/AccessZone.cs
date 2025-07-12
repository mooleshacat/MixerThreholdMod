using System;
using FishNet;
using ScheduleOne.Doors;
using ScheduleOne.Misc;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Map
{
	// Token: 0x02000C69 RID: 3177
	public class AccessZone : MonoBehaviour
	{
		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06005960 RID: 22880 RVA: 0x00179AC3 File Offset: 0x00177CC3
		// (set) Token: 0x06005961 RID: 22881 RVA: 0x00179ACB File Offset: 0x00177CCB
		public bool IsOpen { get; protected set; }

		// Token: 0x06005962 RID: 22882 RVA: 0x00179AD4 File Offset: 0x00177CD4
		protected virtual void Awake()
		{
			this.IsOpen = true;
			this.SetIsOpen(false);
		}

		// Token: 0x06005963 RID: 22883 RVA: 0x00179AE4 File Offset: 0x00177CE4
		public virtual void SetIsOpen(bool open)
		{
			bool isOpen = this.IsOpen;
			this.IsOpen = open;
			foreach (DoorController doorController in this.Doors)
			{
				if (this.IsOpen)
				{
					doorController.PlayerAccess = EDoorAccess.Open;
				}
				else if (this.AllowExitWhenClosed)
				{
					doorController.PlayerAccess = EDoorAccess.ExitOnly;
				}
				else
				{
					doorController.PlayerAccess = EDoorAccess.Locked;
				}
			}
			for (int j = 0; j < this.Lights.Length; j++)
			{
				this.Lights[j].isOn = this.IsOpen;
			}
			if (this.IsOpen && !isOpen && this.onOpen != null)
			{
				this.onOpen.Invoke();
			}
			if (!this.IsOpen && isOpen && this.onClose != null)
			{
				this.onClose.Invoke();
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsOpen && this.AutoCloseDoor)
			{
				foreach (DoorController doorController2 in this.Doors)
				{
					if ((!doorController2.openedByNPC || doorController2.timeSinceNPCSensed >= 1f) && doorController2.IsOpen && ((doorController2.timeSincePlayerSensed > 0.5f && doorController2.playerDetectedSinceOpened) || doorController2.timeSincePlayerSensed > 15f))
					{
						doorController2.SetIsOpen(null, false, EDoorSide.Interior);
					}
				}
			}
		}

		// Token: 0x0400418A RID: 16778
		[Header("Settings")]
		public bool AllowExitWhenClosed;

		// Token: 0x0400418B RID: 16779
		public bool AutoCloseDoor = true;

		// Token: 0x0400418C RID: 16780
		[Header("References")]
		public DoorController[] Doors;

		// Token: 0x0400418D RID: 16781
		public ToggleableLight[] Lights;

		// Token: 0x0400418E RID: 16782
		public UnityEvent onOpen;

		// Token: 0x0400418F RID: 16783
		public UnityEvent onClose;
	}
}
