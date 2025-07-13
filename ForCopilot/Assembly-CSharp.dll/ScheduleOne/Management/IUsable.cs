using System;
using FishNet.Object;

namespace ScheduleOne.Management
{
	// Token: 0x020005BA RID: 1466
	public interface IUsable
	{
		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x0009494D File Offset: 0x00092B4D
		bool IsInUse
		{
			get
			{
				return this.NPCUserObject != null || this.PlayerUserObject != null;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06002425 RID: 9253
		// (set) Token: 0x06002426 RID: 9254
		NetworkObject NPCUserObject { get; set; }

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06002427 RID: 9255
		// (set) Token: 0x06002428 RID: 9256
		NetworkObject PlayerUserObject { get; set; }

		// Token: 0x06002429 RID: 9257
		void SetPlayerUser(NetworkObject playerObject);

		// Token: 0x0600242A RID: 9258
		void SetNPCUser(NetworkObject playerObject);
	}
}
