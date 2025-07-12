using System;
using ScheduleOne.Vehicles.Modification;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000476 RID: 1142
	[Serializable]
	public class VehicleData : SaveData
	{
		// Token: 0x060016B5 RID: 5813 RVA: 0x000648B4 File Offset: 0x00062AB4
		public VehicleData(Guid guid, string code, Vector3 pos, Quaternion rot, EVehicleColor col, ItemSet vehicleContents)
		{
			this.GUID = guid.ToString();
			this.VehicleCode = code;
			this.Position = pos;
			this.Rotation = rot;
			this.Color = col.ToString();
			this.VehicleContents = vehicleContents;
		}

		// Token: 0x04001505 RID: 5381
		public string GUID;

		// Token: 0x04001506 RID: 5382
		public string VehicleCode;

		// Token: 0x04001507 RID: 5383
		public Vector3 Position;

		// Token: 0x04001508 RID: 5384
		public Quaternion Rotation;

		// Token: 0x04001509 RID: 5385
		public string Color;

		// Token: 0x0400150A RID: 5386
		public ItemSet VehicleContents;
	}
}
