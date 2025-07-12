using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Growing;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Variables;

namespace ScheduleOne.Property
{
	// Token: 0x0200084E RID: 2126
	public class MotelRoom : Property
	{
		// Token: 0x0600397C RID: 14716 RVA: 0x000F32C1 File Offset: 0x000F14C1
		protected override void Start()
		{
			base.Start();
			base.InvokeRepeating("UpdateVariables", 0f, 0.5f);
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x000F40B8 File Offset: 0x000F22B8
		private void UpdateVariables()
		{
			if (!NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			Pot[] componentsInChildren = this.Container.GetComponentsInChildren<Pot>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = this.Container.GetComponentsInChildren<PackagingStation>().Length;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].IsFilledWithSoil)
				{
					num++;
				}
				if (componentsInChildren[i].NormalizedWaterLevel > 0.2f)
				{
					num2++;
				}
				if (componentsInChildren[i].Plant != null)
				{
					num3++;
				}
				if (componentsInChildren[i].AppliedAdditives.Find((Additive x) => x.AdditiveName == "Speed Grow"))
				{
					num4++;
				}
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Motel_Pots", componentsInChildren.Length.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Motel_Soil_Pots", num.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Motel_Watered_Pots", num2.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Motel_Seed_Pots", num3.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Motel_PackagingStations", num5.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Motel_MixingStations", this.Container.GetComponentsInChildren<MixingStation>().Length.ToString(), true);
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x000F421B File Offset: 0x000F241B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.MotelRoomAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.MotelRoomAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x000F4234 File Offset: 0x000F2434
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.MotelRoomAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.MotelRoomAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x000F424D File Offset: 0x000F244D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x000F425B File Offset: 0x000F245B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002985 RID: 10629
		private bool dll_Excuted;

		// Token: 0x04002986 RID: 10630
		private bool dll_Excuted;
	}
}
