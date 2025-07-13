using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Property;
using ScheduleOne.Tiles;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C36 RID: 3126
	public class LaunderingStation : GridItem
	{
		// Token: 0x06005687 RID: 22151 RVA: 0x0016DEBB File Offset: 0x0016C0BB
		public override void InitializeGridItem(ItemInstance instance, Grid grid, Vector2 originCoordinate, int rotation, string GUID)
		{
			bool initialized = base.Initialized;
			base.InitializeGridItem(instance, grid, originCoordinate, rotation, GUID);
			if (!initialized)
			{
				this.Interface.Initialize(base.ParentProperty as Business);
			}
		}

		// Token: 0x06005688 RID: 22152 RVA: 0x0016DEE8 File Offset: 0x0016C0E8
		private void Update()
		{
			if (this.Interface != null && this.Interface.business != null)
			{
				this.CashCounter.IsOn = (this.Interface.business.currentLaunderTotal > 0f);
			}
		}

		// Token: 0x06005689 RID: 22153 RVA: 0x0016DF38 File Offset: 0x0016C138
		public override bool CanBeDestroyed(out string reason)
		{
			reason = string.Empty;
			return false;
		}

		// Token: 0x0600568B RID: 22155 RVA: 0x0016DF42 File Offset: 0x0016C142
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.LaunderingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.LaunderingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600568C RID: 22156 RVA: 0x0016DF5B File Offset: 0x0016C15B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.LaunderingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.LaunderingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600568D RID: 22157 RVA: 0x0016DF74 File Offset: 0x0016C174
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x0016DF82 File Offset: 0x0016C182
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04004006 RID: 16390
		[Header("References")]
		public LaunderingInterface Interface;

		// Token: 0x04004007 RID: 16391
		[SerializeField]
		protected CashCounter CashCounter;

		// Token: 0x04004008 RID: 16392
		private bool dll_Excuted;

		// Token: 0x04004009 RID: 16393
		private bool dll_Excuted;
	}
}
