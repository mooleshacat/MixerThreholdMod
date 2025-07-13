using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200044C RID: 1100
	public class JukeboxData : GridItemData
	{
		// Token: 0x06001683 RID: 5763 RVA: 0x000641E1 File Offset: 0x000623E1
		public JukeboxData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, Jukebox.JukeboxState state) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.State = state;
		}

		// Token: 0x0400148D RID: 5261
		public Jukebox.JukeboxState State;
	}
}
