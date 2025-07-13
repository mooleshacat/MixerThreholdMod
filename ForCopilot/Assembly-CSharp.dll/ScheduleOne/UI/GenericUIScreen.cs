using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x02000A2E RID: 2606
	public class GenericUIScreen : MonoBehaviour
	{
		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06004618 RID: 17944 RVA: 0x001263C3 File Offset: 0x001245C3
		// (set) Token: 0x06004619 RID: 17945 RVA: 0x001263CB File Offset: 0x001245CB
		public bool IsOpen { get; private set; }

		// Token: 0x0600461A RID: 17946 RVA: 0x001263D4 File Offset: 0x001245D4
		private void Awake()
		{
			if (this.UseExitActions)
			{
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), this.ExitActionPriority);
			}
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x001263F8 File Offset: 0x001245F8
		public void Open()
		{
			this.IsOpen = true;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(this.Name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			if (this.onOpen != null)
			{
				this.onOpen.Invoke();
			}
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x0012645C File Offset: 0x0012465C
		public void Close()
		{
			this.IsOpen = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(this.Name);
			if (this.ReenableControlsOnClose)
			{
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			}
			if (this.ReenableInventoryOnClose)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			}
			if (!this.ReenableEquippingOnClose)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			}
			if (this.onClose != null)
			{
				this.onClose.Invoke();
			}
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x001264E1 File Offset: 0x001246E1
		private void Exit(ExitAction action)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (action.Used)
			{
				return;
			}
			if (this.CanExitWithRightClick || action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.Close();
			}
		}

		// Token: 0x040032C8 RID: 13000
		[Header("Settings")]
		public string Name;

		// Token: 0x040032C9 RID: 13001
		public bool UseExitActions = true;

		// Token: 0x040032CA RID: 13002
		public int ExitActionPriority;

		// Token: 0x040032CB RID: 13003
		public bool CanExitWithRightClick = true;

		// Token: 0x040032CC RID: 13004
		public bool ReenableControlsOnClose = true;

		// Token: 0x040032CD RID: 13005
		public bool ReenableInventoryOnClose = true;

		// Token: 0x040032CE RID: 13006
		public bool ReenableEquippingOnClose = true;

		// Token: 0x040032CF RID: 13007
		public UnityEvent onOpen;

		// Token: 0x040032D0 RID: 13008
		public UnityEvent onClose;
	}
}
