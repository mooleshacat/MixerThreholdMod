using System;
using ScheduleOne.Doors;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Property;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Building.Doors
{
	// Token: 0x020007D6 RID: 2006
	public class PropertyDoorController : DoorController
	{
		// Token: 0x06003646 RID: 13894 RVA: 0x000E4A9C File Offset: 0x000E2C9C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Building.Doors.PropertyDoorController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x000E4ABB File Offset: 0x000E2CBB
		public void Unlock()
		{
			this.PlayerAccess = EDoorAccess.Open;
			this.IsUnlocked = true;
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x000E4ACC File Offset: 0x000E2CCC
		private void CheckClose()
		{
			if (!base.IsOpen)
			{
				return;
			}
			if (!this.IsUnlocked)
			{
				return;
			}
			if (base.timeInCurrentState < 2f)
			{
				return;
			}
			Player nearestWantedPlayer = this.GetNearestWantedPlayer();
			if (nearestWantedPlayer == null)
			{
				return;
			}
			if (Vector3.Distance(base.transform.position, nearestWantedPlayer.Avatar.CenterPoint) < 20f)
			{
				base.SetIsOpen_Server(false, EDoorSide.Interior, false);
			}
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x000E4B38 File Offset: 0x000E2D38
		protected override bool CanPlayerAccess(EDoorSide side, out string reason)
		{
			if (side == EDoorSide.Exterior)
			{
				Player nearestWantedPlayer = this.GetNearestWantedPlayer();
				if (nearestWantedPlayer != null && Vector3.Distance(nearestWantedPlayer.transform.position, base.transform.position) < 15f)
				{
					PoliceOfficer nearestOfficer = nearestWantedPlayer.CrimeData.NearestOfficer;
					float num = 100000f;
					if (nearestOfficer != null)
					{
						num = Vector3.Distance(nearestOfficer.Avatar.CenterPoint, nearestWantedPlayer.Avatar.CenterPoint);
					}
					if (nearestWantedPlayer.CrimeData.TimeSinceSighted < 5f || num < 15f)
					{
						reason = "Police are nearby!";
						return false;
					}
				}
			}
			return base.CanPlayerAccess(side, out reason);
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x000E4BE0 File Offset: 0x000E2DE0
		private Player GetNearestWantedPlayer()
		{
			Player player = null;
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if (Player.PlayerList[i].CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None && (player == null || Vector3.Distance(base.transform.position, Player.PlayerList[i].Avatar.CenterPoint) < Vector3.Distance(base.transform.position, player.Avatar.CenterPoint)))
				{
					player = Player.PlayerList[i];
				}
			}
			return player;
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x000E4C7B File Offset: 0x000E2E7B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Building.Doors.PropertyDoorControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Building.Doors.PropertyDoorControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000E4C94 File Offset: 0x000E2E94
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Building.Doors.PropertyDoorControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Building.Doors.PropertyDoorControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x000E4CAD File Offset: 0x000E2EAD
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x000E4CBC File Offset: 0x000E2EBC
		protected override void dll()
		{
			base.Awake();
			this.PlayerAccess = EDoorAccess.ExitOnly;
			if (this.Property != null)
			{
				this.Property.onThisPropertyAcquired.AddListener(new UnityAction(this.Unlock));
			}
			base.InvokeRepeating("CheckClose", 0f, 1f);
		}

		// Token: 0x0400267A RID: 9850
		public const float WANTED_PLAYER_CLOSE_DISTANCE = 20f;

		// Token: 0x0400267B RID: 9851
		public Property Property;

		// Token: 0x0400267C RID: 9852
		private bool IsUnlocked;

		// Token: 0x0400267D RID: 9853
		private bool dll_Excuted;

		// Token: 0x0400267E RID: 9854
		private bool dll_Excuted;
	}
}
