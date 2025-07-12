using System;
using System.Linq;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Growing;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Property
{
	// Token: 0x02000847 RID: 2119
	public class Bungalow : Property
	{
		// Token: 0x06003938 RID: 14648 RVA: 0x000F32C1 File Offset: 0x000F14C1
		protected override void Start()
		{
			base.Start();
			base.InvokeRepeating("UpdateVariables", 0f, 0.5f);
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x000F32E0 File Offset: 0x000F14E0
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
			int num5 = (from x in this.BuildableItems
			where x is PackagingStation
			select x).Count<BuildableItem>();
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
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Bungalow_Pots", array.Length.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Bungalow_Soil_Pots", num.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Bungalow_Watered_Pots", num2.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Bungalow_Seed_Pots", num3.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Bungalow_PackagingStations", num5.ToString(), true);
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x000F348F File Offset: 0x000F168F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.BungalowAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.BungalowAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600393C RID: 14652 RVA: 0x000F34A8 File Offset: 0x000F16A8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.BungalowAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.BungalowAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x000F34C1 File Offset: 0x000F16C1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x000F34CF File Offset: 0x000F16CF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002968 RID: 10600
		public Transform ModelContainer;

		// Token: 0x04002969 RID: 10601
		private bool dll_Excuted;

		// Token: 0x0400296A RID: 10602
		private bool dll_Excuted;
	}
}
