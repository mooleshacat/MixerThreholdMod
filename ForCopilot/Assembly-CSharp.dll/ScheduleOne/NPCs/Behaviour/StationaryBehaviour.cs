using System;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200056C RID: 1388
	public class StationaryBehaviour : Behaviour
	{
		// Token: 0x0600215C RID: 8540 RVA: 0x00083F5B File Offset: 0x0008215B
		protected override void Begin()
		{
			base.Begin();
			base.Npc.Movement.Stop();
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000899F8 File Offset: 0x00087BF8
		protected override void Resume()
		{
			base.Resume();
			base.Npc.Movement.Stop();
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x00089A10 File Offset: 0x00087C10
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StationaryBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StationaryBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x00089A29 File Offset: 0x00087C29
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StationaryBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StationaryBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x00089A42 File Offset: 0x00087C42
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x00089A50 File Offset: 0x00087C50
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400198E RID: 6542
		private bool dll_Excuted;

		// Token: 0x0400198F RID: 6543
		private bool dll_Excuted;
	}
}
