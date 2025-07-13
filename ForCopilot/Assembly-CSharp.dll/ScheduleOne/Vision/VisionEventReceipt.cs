using System;
using FishNet.Object;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Vision
{
	// Token: 0x02000293 RID: 659
	[Serializable]
	public class VisionEventReceipt
	{
		// Token: 0x06000DCF RID: 3535 RVA: 0x0003D451 File Offset: 0x0003B651
		public VisionEventReceipt(NetworkObject targetPlayer, PlayerVisualState.EVisualState state)
		{
			this.TargetPlayer = targetPlayer;
			this.State = state;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0000494F File Offset: 0x00002B4F
		public VisionEventReceipt()
		{
		}

		// Token: 0x04000E36 RID: 3638
		public NetworkObject TargetPlayer;

		// Token: 0x04000E37 RID: 3639
		public PlayerVisualState.EVisualState State;
	}
}
