using System;
using ScheduleOne.Persistence;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B68 RID: 2920
	public class NewGameScreen : MainMenuScreen
	{
		// Token: 0x06004D9A RID: 19866 RVA: 0x00146A10 File Offset: 0x00144C10
		public void SlotSelected(int slotIndex)
		{
			if (LoadManager.SaveGames[slotIndex] != null)
			{
				this.ConfirmOverwriteScreen.Initialize(slotIndex);
				this.ConfirmOverwriteScreen.Open(true);
				return;
			}
			this.SetupScreen.Initialize(slotIndex);
			this.SetupScreen.Open(false);
			this.Close(false);
		}

		// Token: 0x040039E0 RID: 14816
		public ConfirmOverwriteScreen ConfirmOverwriteScreen;

		// Token: 0x040039E1 RID: 14817
		public SetupScreen SetupScreen;
	}
}
