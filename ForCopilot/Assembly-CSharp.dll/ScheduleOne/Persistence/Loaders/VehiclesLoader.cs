using System;
using System.Collections.Generic;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003BA RID: 954
	public class VehiclesLoader : Loader
	{
		// Token: 0x0600154B RID: 5451 RVA: 0x0005F3F0 File Offset: 0x0005D5F0
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, true))
			{
				VehicleCollectionData vehicleCollectionData = null;
				try
				{
					vehicleCollectionData = JsonUtility.FromJson<VehicleCollectionData>(text);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading data: " + ex.Message);
				}
				if (vehicleCollectionData != null && vehicleCollectionData.Vehicles != null)
				{
					foreach (VehicleData data in vehicleCollectionData.Vehicles)
					{
						NetworkSingleton<VehicleManager>.Instance.SpawnAndLoadVehicle(data, string.Empty, true);
					}
					return;
				}
			}
			else
			{
				if (!Directory.Exists(mainPath))
				{
					return;
				}
				Console.Log("Loading legacy vehicles at: " + mainPath, null);
				List<DirectoryInfo> directories = base.GetDirectories(mainPath);
				VehicleLoader loader = new VehicleLoader();
				for (int j = 0; j < directories.Count; j++)
				{
					new LoadRequest(directories[j].FullName, loader);
				}
			}
		}
	}
}
