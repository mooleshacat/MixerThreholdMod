using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000408 RID: 1032
	public class GameData : SaveData
	{
		// Token: 0x0600162F RID: 5679 RVA: 0x000638A5 File Offset: 0x00061AA5
		public GameData(string organisationName, int seed, GameSettings settings)
		{
			this.OrganisationName = organisationName;
			this.Seed = seed;
			this.Settings = settings;
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x000638C2 File Offset: 0x00061AC2
		public GameData()
		{
			this.OrganisationName = "Organisation";
			this.Seed = 0;
		}

		// Token: 0x040013FC RID: 5116
		public string OrganisationName;

		// Token: 0x040013FD RID: 5117
		public int Seed;

		// Token: 0x040013FE RID: 5118
		public GameSettings Settings;
	}
}
