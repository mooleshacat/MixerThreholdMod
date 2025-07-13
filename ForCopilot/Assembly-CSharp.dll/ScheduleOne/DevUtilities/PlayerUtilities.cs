using System;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000733 RID: 1843
	public static class PlayerUtilities
	{
		// Token: 0x060031D3 RID: 12755 RVA: 0x000D00E1 File Offset: 0x000CE2E1
		public static void OpenMenu()
		{
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x000D0119 File Offset: 0x000CE319
		public static void CloseMenu(bool reenableLookInstantly = false, bool reenableInventory = true)
		{
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			if (reenableLookInstantly)
			{
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			}
			if (reenableInventory)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			}
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
		}
	}
}
