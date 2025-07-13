using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000443 RID: 1091
	[Serializable]
	public class MoveItemData
	{
		// Token: 0x0600167A RID: 5754 RVA: 0x00063FE0 File Offset: 0x000621E0
		public MoveItemData(string templateItemJson, int grabbedItemQuantity, Guid sourceGUID, Guid destinationGUID)
		{
			this.TemplateItemJSON = templateItemJson;
			this.GrabbedItemQuantity = grabbedItemQuantity;
			this.SourceGUID = sourceGUID.ToString();
			this.DestinationGUID = destinationGUID.ToString();
		}

		// Token: 0x0400146C RID: 5228
		public string TemplateItemJSON = string.Empty;

		// Token: 0x0400146D RID: 5229
		public int GrabbedItemQuantity;

		// Token: 0x0400146E RID: 5230
		public string SourceGUID;

		// Token: 0x0400146F RID: 5231
		public string DestinationGUID;
	}
}
