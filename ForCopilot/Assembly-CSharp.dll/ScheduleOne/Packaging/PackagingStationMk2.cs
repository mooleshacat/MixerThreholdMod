using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.UI;
using ScheduleOne.Variables;

namespace ScheduleOne.Packaging
{
	// Token: 0x020008CD RID: 2253
	public class PackagingStationMk2 : PackagingStation
	{
		// Token: 0x06003CB4 RID: 15540 RVA: 0x000FF708 File Offset: 0x000FD908
		public override void StartTask()
		{
			new PackageProductTaskMk2(this);
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("PackagingStationMk2TutorialDone"))
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("PackagingStationMk2TutorialDone", true.ToString(), true);
				Singleton<TaskManagerUI>.Instance.PackagingStationMK2TutorialDone.Open();
			}
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x000FF75D File Offset: 0x000FD95D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Packaging.PackagingStationMk2Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Packaging.PackagingStationMk2Assembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x000FF776 File Offset: 0x000FD976
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Packaging.PackagingStationMk2Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Packaging.PackagingStationMk2Assembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x000FF78F File Offset: 0x000FD98F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x000FF79D File Offset: 0x000FD99D
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002B79 RID: 11129
		public PackagingTool PackagingTool;

		// Token: 0x04002B7A RID: 11130
		private bool dll_Excuted;

		// Token: 0x04002B7B RID: 11131
		private bool dll_Excuted;
	}
}
