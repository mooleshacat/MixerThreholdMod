using System;
using ScheduleOne.Economy;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006E5 RID: 1765
	public class DialogueController_Supplier : DialogueController
	{
		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x0600306B RID: 12395 RVA: 0x000CB2FD File Offset: 0x000C94FD
		// (set) Token: 0x0600306C RID: 12396 RVA: 0x000CB305 File Offset: 0x000C9505
		public Supplier Supplier { get; private set; }

		// Token: 0x0600306D RID: 12397 RVA: 0x000CB30E File Offset: 0x000C950E
		protected override void Start()
		{
			base.Start();
			this.Supplier = (this.npc as Supplier);
		}
	}
}
