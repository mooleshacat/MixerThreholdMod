using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003B9 RID: 953
	public class VehicleLoader : Loader
	{
		// Token: 0x06001549 RID: 5449 RVA: 0x0005F374 File Offset: 0x0005D574
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, "Vehicle", out text))
			{
				VehicleData vehicleData = null;
				try
				{
					vehicleData = JsonUtility.FromJson<VehicleData>(text);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (vehicleData != null)
				{
					NetworkSingleton<VehicleManager>.Instance.SpawnAndLoadVehicle(vehicleData, mainPath, true);
				}
			}
		}
	}
}
