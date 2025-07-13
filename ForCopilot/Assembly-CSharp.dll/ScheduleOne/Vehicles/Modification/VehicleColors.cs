using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Vehicles.Modification
{
	// Token: 0x02000822 RID: 2082
	public class VehicleColors : Singleton<VehicleColors>
	{
		// Token: 0x0600389C RID: 14492 RVA: 0x000EEB04 File Offset: 0x000ECD04
		public string GetColorName(EVehicleColor c)
		{
			return this.colorLibrary.Find((VehicleColors.VehicleColorData x) => x.color == c).colorName;
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x000EEB3C File Offset: 0x000ECD3C
		public Color32 GetColorUIColor(EVehicleColor c)
		{
			return this.colorLibrary.Find((VehicleColors.VehicleColorData x) => x.color == c).UIColor;
		}

		// Token: 0x0400288A RID: 10378
		public List<VehicleColors.VehicleColorData> colorLibrary = new List<VehicleColors.VehicleColorData>();

		// Token: 0x02000823 RID: 2083
		[Serializable]
		public class VehicleColorData
		{
			// Token: 0x0400288B RID: 10379
			public EVehicleColor color;

			// Token: 0x0400288C RID: 10380
			public string colorName;

			// Token: 0x0400288D RID: 10381
			public Material material;

			// Token: 0x0400288E RID: 10382
			public Color32 UIColor = Color.white;
		}
	}
}
