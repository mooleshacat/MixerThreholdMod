using System;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Properties;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BF6 RID: 3062
	public class Bed : NetworkBehaviour
	{
		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x0600521C RID: 21020 RVA: 0x0015AE63 File Offset: 0x00159063
		public Employee AssignedEmployee
		{
			get
			{
				if (!(this.EmployeeStationThing != null))
				{
					return null;
				}
				return this.EmployeeStationThing.AssignedEmployee;
			}
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x0015AE80 File Offset: 0x00159080
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.Bed_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x0015AE94 File Offset: 0x00159094
		public void Hovered()
		{
			if (Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (this.AssignedEmployee != null)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				this.intObj.SetMessage("Assigned to " + this.AssignedEmployee.fullName);
				return;
			}
			string message;
			if (this.CanSleep(out message))
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.intObj.SetMessage("Sleep");
				return;
			}
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
			this.intObj.SetMessage(message);
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x0015AF34 File Offset: 0x00159134
		public void Interacted()
		{
			Player.Local.CurrentBed = base.NetworkObject;
			Singleton<SleepCanvas>.Instance.SetIsOpen(true);
		}

		// Token: 0x06005220 RID: 21024 RVA: 0x0015AF54 File Offset: 0x00159154
		private bool CanSleep(out string noSleepReason)
		{
			noSleepReason = string.Empty;
			if (GameManager.IS_TUTORIAL)
			{
				return true;
			}
			if (!NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(1800, 400))
			{
				noSleepReason = "Can't sleep before " + TimeManager.Get12HourTime(1800f, true);
				return false;
			}
			if (Player.Local.ConsumedProduct != null)
			{
				if ((Player.Local.ConsumedProduct.Definition as ProductDefinition).Properties.Exists((Property x) => x.GetType() == typeof(Energizing)))
				{
					noSleepReason = "Can't sleep while energized!";
					return false;
				}
			}
			if (Player.Local.ConsumedProduct != null)
			{
				if ((Player.Local.ConsumedProduct.Definition as ProductDefinition).Properties.Exists((Property x) => x.GetType() == typeof(Athletic)))
				{
					noSleepReason = "Can't sleep while athletic!";
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005221 RID: 21025 RVA: 0x0015B04C File Offset: 0x0015924C
		public void UpdateMaterial()
		{
			if (this.BlanketMesh == null)
			{
				return;
			}
			Material material = this.DefaultBlanket;
			if (this.AssignedEmployee != null)
			{
				switch (this.AssignedEmployee.EmployeeType)
				{
				case EEmployeeType.Botanist:
					material = this.BotanistBlanket;
					break;
				case EEmployeeType.Handler:
					material = this.PackagerBlanket;
					break;
				case EEmployeeType.Chemist:
					material = this.ChemistBlanket;
					break;
				case EEmployeeType.Cleaner:
					material = this.CleanerBlanket;
					break;
				}
			}
			this.BlanketMesh.material = material;
		}

		// Token: 0x06005223 RID: 21027 RVA: 0x0015B0CF File Offset: 0x001592CF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BedAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BedAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06005224 RID: 21028 RVA: 0x0015B0E2 File Offset: 0x001592E2
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.BedAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.BedAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06005225 RID: 21029 RVA: 0x0015B0F5 File Offset: 0x001592F5
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x0015B103 File Offset: 0x00159303
		private void dll()
		{
			this.UpdateMaterial();
		}

		// Token: 0x04003D89 RID: 15753
		public const int MIN_SLEEP_TIME = 1800;

		// Token: 0x04003D8A RID: 15754
		public const float SLEEP_TIME_SCALE = 1f;

		// Token: 0x04003D8B RID: 15755
		[Header("References")]
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x04003D8C RID: 15756
		public EmployeeHome EmployeeStationThing;

		// Token: 0x04003D8D RID: 15757
		public MeshRenderer BlanketMesh;

		// Token: 0x04003D8E RID: 15758
		[Header("Materials")]
		public Material DefaultBlanket;

		// Token: 0x04003D8F RID: 15759
		public Material BotanistBlanket;

		// Token: 0x04003D90 RID: 15760
		public Material ChemistBlanket;

		// Token: 0x04003D91 RID: 15761
		public Material PackagerBlanket;

		// Token: 0x04003D92 RID: 15762
		public Material CleanerBlanket;

		// Token: 0x04003D93 RID: 15763
		private bool dll_Excuted;

		// Token: 0x04003D94 RID: 15764
		private bool dll_Excuted;
	}
}
