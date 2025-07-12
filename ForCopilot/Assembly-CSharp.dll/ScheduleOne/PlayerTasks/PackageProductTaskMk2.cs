using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Packaging;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.UI;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000366 RID: 870
	public class PackageProductTaskMk2 : Task
	{
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x00055C83 File Offset: 0x00053E83
		// (set) Token: 0x06001396 RID: 5014 RVA: 0x00055C8B File Offset: 0x00053E8B
		public override string TaskName { get; protected set; } = "Package product";

		// Token: 0x06001397 RID: 5015 RVA: 0x00055C94 File Offset: 0x00053E94
		public PackageProductTaskMk2(PackagingStationMk2 _station)
		{
			if (_station == null)
			{
				Console.LogError("Station is null!", null);
				return;
			}
			if (_station.GetState(PackagingStation.EMode.Package) != PackagingStation.EState.CanBegin)
			{
				Console.LogError("Station not ready to begin packaging!", null);
				return;
			}
			this.station = _station;
			this.ClickDetectionRadius = 0.01f;
			base.EnableMultiDragging(this.station.PackagingTool.ProductContainer, 0.08f);
			int quantity = _station.ProductSlot.Quantity;
			int quantity2 = _station.PackagingSlot.Quantity;
			int quantity3 = (_station.PackagingSlot.ItemInstance.Definition as PackagingDefinition).Quantity;
			int num = Mathf.Min(quantity, quantity2 * quantity3);
			num -= num % quantity3;
			int num2 = Mathf.CeilToInt((float)num / (float)quantity3);
			this.station.UpdatePackagingVisuals(this.station.PackagingSlot.Quantity - num2);
			this.station.UpdateProductVisuals(this.station.ProductSlot.Quantity - num2);
			this.station.SetVisualsLocked(true);
			FunctionalPackaging functionalPackaging = (_station.PackagingSlot.ItemInstance.Definition as PackagingDefinition).FunctionalPackaging;
			this.station.PackagingTool.Initialize(this, functionalPackaging, num2, _station.ProductSlot.ItemInstance as ProductItemInstance, num);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.station.CameraPosition_Task.position, this.station.CameraPosition_Task.rotation, 0.2f, false);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("packagingmk2");
			base.CurrentInstruction = "Insert product into packaging";
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00055E50 File Offset: 0x00054050
		public override void StopTask()
		{
			this.station.PackagingTool.Deinitialize();
			this.station.SetVisualsLocked(false);
			this.station.SetHatchOpen(false);
			this.station.UpdateProductVisuals();
			this.station.UpdatePackagingVisuals();
			base.StopTask();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.station.CameraPosition.position, this.station.CameraPosition.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.2f);
			if (this.Outcome == Task.EOutcome.Success && this.station.GetState(PackagingStation.EMode.Package) == PackagingStation.EState.CanBegin)
			{
				new PackageProductTask(this.station);
				return;
			}
			Singleton<PackagingStationCanvas>.Instance.SetIsOpen(this.station, true, true);
		}

		// Token: 0x040012AB RID: 4779
		protected PackagingStationMk2 station;

		// Token: 0x040012AC RID: 4780
		protected FunctionalPackaging Packaging;

		// Token: 0x040012AD RID: 4781
		protected List<FunctionalProduct> Products = new List<FunctionalProduct>();
	}
}
