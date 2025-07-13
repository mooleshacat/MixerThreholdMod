using System;
using System.Linq;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Growing;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Variables;

namespace ScheduleOne.Property
{
	// Token: 0x0200085C RID: 2140
	public class Sweatshop : Property
	{
		// Token: 0x06003A15 RID: 14869 RVA: 0x000F32C1 File Offset: 0x000F14C1
		protected override void Start()
		{
			base.Start();
			base.InvokeRepeating("UpdateVariables", 0f, 0.5f);
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x000F5E1C File Offset: 0x000F401C
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
			Pot[] array = (from x in this.BuildableItems
			where x is Pot
			select x as Pot).ToArray<Pot>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = this.Container.GetComponentsInChildren<PackagingStation>().Length;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IsFilledWithSoil)
				{
					num++;
				}
				if (array[i].NormalizedWaterLevel > 0.2f)
				{
					num2++;
				}
				if (array[i].Plant != null)
				{
					num3++;
				}
				if (array[i].AppliedAdditives.Find((Additive x) => x.AdditiveName == "Speed Grow"))
				{
					num4++;
				}
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Sweatshop_Pots", array.Length.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Sweatshop_PackagingStations", num5.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Sweatshop_MixingStations", this.Container.GetComponentsInChildren<MixingStation>().Length.ToString(), true);
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x000F5F82 File Offset: 0x000F4182
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.SweatshopAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.SweatshopAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x000F5F9B File Offset: 0x000F419B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.SweatshopAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.SweatshopAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x000F5FB4 File Offset: 0x000F41B4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x000F5FC2 File Offset: 0x000F41C2
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040029D1 RID: 10705
		private bool dll_Excuted;

		// Token: 0x040029D2 RID: 10706
		private bool dll_Excuted;
	}
}
