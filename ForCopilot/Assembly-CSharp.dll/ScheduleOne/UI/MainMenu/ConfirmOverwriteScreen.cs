using System;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B5C RID: 2908
	public class ConfirmOverwriteScreen : MainMenuScreen
	{
		// Token: 0x06004D6A RID: 19818 RVA: 0x00145F7B File Offset: 0x0014417B
		public void Initialize(int index)
		{
			this.slotIndex = index;
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x00145F84 File Offset: 0x00144184
		public void Confirm()
		{
			this.Close(false);
			this.SetupScreen.Initialize(this.slotIndex);
			this.SetupScreen.Open(false);
		}

		// Token: 0x040039AF RID: 14767
		public SetupScreen SetupScreen;

		// Token: 0x040039B0 RID: 14768
		private int slotIndex;
	}
}
