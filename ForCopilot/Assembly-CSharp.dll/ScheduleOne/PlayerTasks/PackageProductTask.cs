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
	// Token: 0x02000365 RID: 869
	public class PackageProductTask : Task
	{
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x0005588D File Offset: 0x00053A8D
		// (set) Token: 0x0600138E RID: 5006 RVA: 0x00055895 File Offset: 0x00053A95
		public override string TaskName { get; protected set; } = "Package product";

		// Token: 0x0600138F RID: 5007 RVA: 0x000558A0 File Offset: 0x00053AA0
		public PackageProductTask(PackagingStation _station)
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
			this.ClickDetectionRadius = 0.02f;
			this.Packaging = UnityEngine.Object.Instantiate<FunctionalPackaging>((this.station.PackagingSlot.ItemInstance.Definition as PackagingDefinition).FunctionalPackaging, this.station.Container);
			this.Packaging.Initialize(this.station, this.station.ActivePackagingAlignent, true);
			base.EnableMultiDragging(this.station.Container, 0.08f);
			int quantity = (this.station.PackagingSlot.ItemInstance.Definition as PackagingDefinition).Quantity;
			for (int i = 0; i < quantity; i++)
			{
				FunctionalProduct functionalProduct = UnityEngine.Object.Instantiate<FunctionalProduct>((this.station.ProductSlot.ItemInstance.Definition as ProductDefinition).FunctionalProduct, this.station.Container);
				functionalProduct.Initialize(this.station, this.station.ProductSlot.ItemInstance, this.station.ActiveProductAlignments[i], true);
				functionalProduct.ClampZ = true;
				functionalProduct.DragProjectionMode = Draggable.EDragProjectionMode.FlatCameraForward;
				this.Products.Add(functionalProduct);
			}
			FunctionalPackaging packaging = this.Packaging;
			packaging.onFullyPacked = (Action)Delegate.Combine(packaging.onFullyPacked, new Action(this.FullyPacked));
			FunctionalPackaging packaging2 = this.Packaging;
			packaging2.onSealed = (Action)Delegate.Combine(packaging2.onSealed, new Action(this.Sealed));
			FunctionalPackaging packaging3 = this.Packaging;
			packaging3.onReachOutput = (Action)Delegate.Combine(packaging3.onReachOutput, new Action(this.ReachedOutput));
			this.station.UpdatePackagingVisuals(this.station.PackagingSlot.Quantity - 1);
			this.station.UpdateProductVisuals(this.station.ProductSlot.Quantity - this.Packaging.Definition.Quantity);
			this.station.SetVisualsLocked(true);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.station.CameraPosition_Task.position, this.station.CameraPosition_Task.rotation, 0.2f, false);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("packaging");
			base.CurrentInstruction = "Place product into packaging";
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00055B40 File Offset: 0x00053D40
		public override void StopTask()
		{
			this.Packaging.Destroy();
			for (int i = 0; i < this.Products.Count; i++)
			{
				UnityEngine.Object.Destroy(this.Products[i].gameObject);
			}
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

		// Token: 0x06001391 RID: 5009 RVA: 0x00055C3C File Offset: 0x00053E3C
		public override void Success()
		{
			this.station.PackSingleInstance();
			base.Success();
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00055C4F File Offset: 0x00053E4F
		private void FullyPacked()
		{
			base.CurrentInstruction = this.Packaging.SealInstruction;
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00055C62 File Offset: 0x00053E62
		private void Sealed()
		{
			base.CurrentInstruction = "Place packaging in hopper";
			this.station.SetHatchOpen(true);
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00055C7B File Offset: 0x00053E7B
		private void ReachedOutput()
		{
			this.Success();
		}

		// Token: 0x040012A7 RID: 4775
		protected PackagingStation station;

		// Token: 0x040012A8 RID: 4776
		protected FunctionalPackaging Packaging;

		// Token: 0x040012A9 RID: 4777
		protected List<FunctionalProduct> Products = new List<FunctionalProduct>();
	}
}
