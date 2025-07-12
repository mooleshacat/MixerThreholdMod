using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003E8 RID: 1000
	public class JukeboxLoader : GridItemLoader
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x000619D7 File Offset: 0x0005FBD7
		public override string ItemType
		{
			get
			{
				return typeof(JukeboxData).Name;
			}
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x000619E8 File Offset: 0x0005FBE8
		public override void Load(string mainPath)
		{
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			Jukebox jukebox = gridItem as Jukebox;
			if (jukebox == null)
			{
				Console.LogWarning("Failed to cast grid item to Jukebox", null);
				return;
			}
			JukeboxData data = base.GetData<JukeboxData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load jukebox data", null);
				return;
			}
			Console.Log(string.Format("Loaded jukebox data: {0}", data), null);
			jukebox.SetJukeboxState(null, data.State, true, true);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00061A68 File Offset: 0x0005FC68
		public override void Load(DynamicSaveData data)
		{
			GridItem gridItem = null;
			GridItemData data2;
			if (data.TryExtractBaseData<GridItemData>(out data2))
			{
				gridItem = base.LoadAndCreate(data2);
			}
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			Jukebox jukebox = gridItem as Jukebox;
			if (jukebox == null)
			{
				Console.LogWarning("Failed to cast grid item to Jukebox", null);
				return;
			}
			JukeboxData jukeboxData;
			if (data.TryExtractBaseData<JukeboxData>(out jukeboxData))
			{
				jukebox.SetJukeboxState(null, jukeboxData.State, true, true);
			}
		}
	}
}
