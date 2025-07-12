using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000438 RID: 1080
	[Serializable]
	public class MetaData : SaveData
	{
		// Token: 0x0600166E RID: 5742 RVA: 0x00063DCD File Offset: 0x00061FCD
		public MetaData(DateTimeData creationDate, DateTimeData lastPlayedDate, string creationVersion, string lastSaveVersion, bool playTutorial)
		{
			this.CreationDate = creationDate;
			this.LastPlayedDate = lastPlayedDate;
			this.CreationVersion = creationVersion;
			this.LastSaveVersion = lastSaveVersion;
			this.PlayTutorial = playTutorial;
		}

		// Token: 0x04001447 RID: 5191
		public DateTimeData CreationDate;

		// Token: 0x04001448 RID: 5192
		public DateTimeData LastPlayedDate;

		// Token: 0x04001449 RID: 5193
		public string CreationVersion;

		// Token: 0x0400144A RID: 5194
		public string LastSaveVersion;

		// Token: 0x0400144B RID: 5195
		public bool PlayTutorial;
	}
}
