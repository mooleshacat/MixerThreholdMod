using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007FD RID: 2045
	public class ForkliftCamera : VehicleCamera
	{
		// Token: 0x0600372C RID: 14124 RVA: 0x000E8448 File Offset: 0x000E6648
		protected override void Update()
		{
			base.Update();
			this.forkliftCamActive = false;
			if (this.vehicle.localPlayerIsDriver && Input.GetKey(KeyCode.LeftShift))
			{
				this.forkliftCamActive = true;
			}
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x000E8478 File Offset: 0x000E6678
		protected override void LateUpdate()
		{
			base.LateUpdate();
			this.guidanceLight.enabled = false;
			if (this.vehicle.localPlayerIsDriver && this.forkliftCamActive)
			{
				PlayerSingleton<PlayerCamera>.Instance.transform.position = this.forkCamPos.position;
				PlayerSingleton<PlayerCamera>.Instance.transform.rotation = this.forkCamPos.rotation;
				this.guidanceLight.enabled = true;
			}
		}

		// Token: 0x04002756 RID: 10070
		[Header("Forklift References")]
		[SerializeField]
		protected Transform forkCamPos;

		// Token: 0x04002757 RID: 10071
		[SerializeField]
		protected Light guidanceLight;

		// Token: 0x04002758 RID: 10072
		protected bool forkliftCamActive;
	}
}
