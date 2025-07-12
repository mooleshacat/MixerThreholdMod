using System;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Police
{
	// Token: 0x02000348 RID: 840
	public class Investigation
	{
		// Token: 0x17000378 RID: 888
		// (get) Token: 0x0600126D RID: 4717 RVA: 0x0004F8EA File Offset: 0x0004DAEA
		// (set) Token: 0x0600126E RID: 4718 RVA: 0x0004F8F2 File Offset: 0x0004DAF2
		public float CurrentProgress { get; protected set; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x0004F8FB File Offset: 0x0004DAFB
		// (set) Token: 0x06001270 RID: 4720 RVA: 0x0004F903 File Offset: 0x0004DB03
		public Player Target { get; protected set; }

		// Token: 0x06001271 RID: 4721 RVA: 0x0004F90C File Offset: 0x0004DB0C
		public Investigation(Player target)
		{
			this.Target = target;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0004F91B File Offset: 0x0004DB1B
		public void ChangeProgress(float progress)
		{
			this.CurrentProgress += progress;
		}
	}
}
