using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Vehicles
{
	// Token: 0x0200080D RID: 2061
	public class VehicleAudio : MonoBehaviour
	{
		// Token: 0x0600381B RID: 14363 RVA: 0x000EC7AC File Offset: 0x000EA9AC
		protected virtual void Awake()
		{
			if (this.Vehicle != null)
			{
				this.Vehicle.onVehicleStart.AddListener(new UnityAction(this.EngineStart));
				this.Vehicle.onVehicleStop.AddListener(new UnityAction(this.EngineStart));
			}
			if (this.Lights != null)
			{
				this.Lights.onHeadlightsOn.AddListener(new UnityAction(this.HeadlightsToggledOn));
				this.Lights.onHeadlightsOff.AddListener(new UnityAction(this.HeadlightsToggledOff));
			}
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void EngineStart()
		{
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void EngineStop()
		{
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void HeadlightsToggledOn()
		{
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void HeadlightsToggledOff()
		{
		}

		// Token: 0x040027E3 RID: 10211
		[Header("Refererences")]
		public LandVehicle Vehicle;

		// Token: 0x040027E4 RID: 10212
		public VehicleLights Lights;

		// Token: 0x040027E5 RID: 10213
		[Header("Sounds")]
		public AudioSource EngineStartSound;

		// Token: 0x040027E6 RID: 10214
		public AudioSource EngineStopSound;

		// Token: 0x040027E7 RID: 10215
		public AudioSource HeadlightsOnSound;

		// Token: 0x040027E8 RID: 10216
		public AudioSource HeadlightsOffSound;

		// Token: 0x040027E9 RID: 10217
		public AudioSource HornSound;
	}
}
