using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000399 RID: 921
	public class SaveRequest
	{
		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060014F3 RID: 5363 RVA: 0x0005CE78 File Offset: 0x0005B078
		// (set) Token: 0x060014F4 RID: 5364 RVA: 0x0005CE80 File Offset: 0x0005B080
		public string SaveString { get; private set; }

		// Token: 0x060014F5 RID: 5365 RVA: 0x0005CE8C File Offset: 0x0005B08C
		public SaveRequest(ISaveable saveable, string parentFolderPath)
		{
			this.Saveable = saveable;
			this.ParentFolderPath = parentFolderPath;
			this.SaveString = saveable.GetSaveString();
			if (this.SaveString != string.Empty)
			{
				Singleton<SaveManager>.Instance.QueueSaveRequest(this);
				return;
			}
			saveable.CompleteSave(parentFolderPath, false);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0005CEDF File Offset: 0x0005B0DF
		public void Complete()
		{
			Singleton<SaveManager>.Instance.DequeueSaveRequest(this);
			this.Saveable.WriteBaseData(this.ParentFolderPath, this.SaveString);
		}

		// Token: 0x0400138E RID: 5006
		public ISaveable Saveable;

		// Token: 0x0400138F RID: 5007
		public string ParentFolderPath;
	}
}
