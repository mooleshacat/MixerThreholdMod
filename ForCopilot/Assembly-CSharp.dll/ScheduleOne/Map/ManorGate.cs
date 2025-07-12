using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C75 RID: 3189
	public class ManorGate : Gate
	{
		// Token: 0x060059A5 RID: 22949 RVA: 0x0017AA76 File Offset: 0x00178C76
		protected virtual void Start()
		{
			this.SetIntercomActive(false);
			this.SetEnterable(false);
			base.InvokeRepeating("UpdateDetection", 0f, 0.25f);
		}

		// Token: 0x060059A6 RID: 22950 RVA: 0x0017AA9C File Offset: 0x00178C9C
		private void UpdateDetection()
		{
			bool flag = false;
			if (this.ExteriorVehicleDetector.AreAnyVehiclesOccupied())
			{
				flag = true;
			}
			if (this.ExteriorPlayerDetector.DetectedPlayers.Count > 0)
			{
				flag = true;
			}
			if (this.InteriorVehicleDetector.AreAnyVehiclesOccupied())
			{
				flag = true;
			}
			if (this.InteriorPlayerDetector.DetectedPlayers.Count > 0)
			{
				flag = true;
			}
			if (flag != base.IsOpen)
			{
				if (flag)
				{
					base.Open();
					return;
				}
				base.Close();
			}
		}

		// Token: 0x060059A7 RID: 22951 RVA: 0x0017AB0C File Offset: 0x00178D0C
		public void IntercomBuzzed()
		{
			this.SetIntercomActive(false);
		}

		// Token: 0x060059A8 RID: 22952 RVA: 0x0017AB15 File Offset: 0x00178D15
		public void SetEnterable(bool enterable)
		{
			this.ExteriorPlayerDetector.SetIgnoreNewCollisions(!enterable);
			this.ExteriorVehicleDetector.SetIgnoreNewCollisions(!enterable);
			this.ExteriorVehicleDetector.vehicles.Clear();
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x0017AB45 File Offset: 0x00178D45
		[Button]
		public void ActivateIntercom()
		{
			this.SetIntercomActive(true);
		}

		// Token: 0x060059AA RID: 22954 RVA: 0x0017AB4E File Offset: 0x00178D4E
		public void SetIntercomActive(bool active)
		{
			this.intercomActive = active;
			this.UpdateIntercom();
		}

		// Token: 0x060059AB RID: 22955 RVA: 0x0017AB5D File Offset: 0x00178D5D
		private void UpdateIntercom()
		{
			this.IntercomInt.SetInteractableState(this.intercomActive ? InteractableObject.EInteractableState.Default : InteractableObject.EInteractableState.Disabled);
			this.IntercomLight.enabled = this.intercomActive;
		}

		// Token: 0x040041CB RID: 16843
		[Header("References")]
		public InteractableObject IntercomInt;

		// Token: 0x040041CC RID: 16844
		public Light IntercomLight;

		// Token: 0x040041CD RID: 16845
		public VehicleDetector ExteriorVehicleDetector;

		// Token: 0x040041CE RID: 16846
		public PlayerDetector ExteriorPlayerDetector;

		// Token: 0x040041CF RID: 16847
		public VehicleDetector InteriorVehicleDetector;

		// Token: 0x040041D0 RID: 16848
		public PlayerDetector InteriorPlayerDetector;

		// Token: 0x040041D1 RID: 16849
		private bool intercomActive;
	}
}
