using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200045C RID: 1116
	[Serializable]
	public class PlayerData : SaveData
	{
		// Token: 0x06001693 RID: 5779 RVA: 0x0006441E File Offset: 0x0006261E
		public PlayerData(string playerCode, Vector3 playerPos, float playerRot, bool introCompleted)
		{
			this.PlayerCode = playerCode;
			this.Position = playerPos;
			this.Rotation = playerRot;
			this.IntroCompleted = introCompleted;
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0006444E File Offset: 0x0006264E
		public PlayerData()
		{
		}

		// Token: 0x040014B4 RID: 5300
		public string PlayerCode;

		// Token: 0x040014B5 RID: 5301
		public Vector3 Position = Vector3.zero;

		// Token: 0x040014B6 RID: 5302
		public float Rotation;

		// Token: 0x040014B7 RID: 5303
		public bool IntroCompleted;
	}
}
