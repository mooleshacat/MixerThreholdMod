using System;
using System.Collections.Generic;
using System.IO;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200039D RID: 925
	public class DeliveriesLoader : Loader
	{
		// Token: 0x060014FF RID: 5375 RVA: 0x0005D0E8 File Offset: 0x0005B2E8
		public override void Load(string mainPath)
		{
			bool flag = false;
			string text;
			if (base.TryLoadFile(Path.Combine(mainPath, "Deliveries"), out text, true) || base.TryLoadFile(mainPath, out text, true))
			{
				DeliveriesData deliveriesData = null;
				try
				{
					deliveriesData = JsonUtility.FromJson<DeliveriesData>(text);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading data: " + ex.Message);
				}
				if (deliveriesData != null && deliveriesData.ActiveDeliveries != null)
				{
					foreach (DeliveryInstance delivery in deliveriesData.ActiveDeliveries)
					{
						NetworkSingleton<DeliveryManager>.Instance.SendDelivery(delivery);
					}
					if (deliveriesData.DeliveryVehicles != null)
					{
						flag = true;
						foreach (VehicleData data in deliveriesData.DeliveryVehicles)
						{
							NetworkSingleton<VehicleManager>.Instance.LoadVehicle(data, mainPath);
						}
					}
				}
			}
			if (!flag && Directory.Exists(mainPath))
			{
				Console.Log("Loading legacy delivery vehicles at: " + mainPath, null);
				string parentPath = Path.Combine(mainPath, "DeliveryVehicles");
				List<DirectoryInfo> directories = base.GetDirectories(parentPath);
				for (int j = 0; j < directories.Count; j++)
				{
					this.LoadVehicle(directories[j].FullName);
				}
			}
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0005D220 File Offset: 0x0005B420
		public void LoadVehicle(string vehiclePath)
		{
			Console.Log("Loading delivery vehicle: " + vehiclePath, null);
			string text;
			if (base.TryLoadFile(vehiclePath, "Vehicle", out text))
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
				string str3 = "Data: ";
				VehicleData vehicleData2 = vehicleData;
				Console.Log(str3 + ((vehicleData2 != null) ? vehicleData2.ToString() : null), null);
				if (vehicleData != null)
				{
					NetworkSingleton<VehicleManager>.Instance.LoadVehicle(vehicleData, vehiclePath);
				}
			}
		}
	}
}
