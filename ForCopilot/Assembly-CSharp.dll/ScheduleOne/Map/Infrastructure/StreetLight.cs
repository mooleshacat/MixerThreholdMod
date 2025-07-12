using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Lighting;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Map.Infrastructure
{
	// Token: 0x02000C92 RID: 3218
	public class StreetLight : MonoBehaviour
	{
		// Token: 0x06005A3F RID: 23103 RVA: 0x0017C7C0 File Offset: 0x0017A9C0
		protected virtual void Awake()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.UpdateState));
			if (this.BeamTracker != null)
			{
				this.BeamTracker.Override = true;
			}
			this.StartTimeOffset = (int)(Vector3.Distance(base.transform.position, StreetLight.POWER_ORIGIN) / 50f);
		}

		// Token: 0x06005A40 RID: 23104 RVA: 0x0017C830 File Offset: 0x0017AA30
		private void Start()
		{
			this.UpdateState();
		}

		// Token: 0x06005A41 RID: 23105 RVA: 0x0017C838 File Offset: 0x0017AA38
		protected virtual void UpdateState()
		{
			this.SetState(NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(TimeManager.AddMinutesTo24HourTime(this.StartTime, this.StartTimeOffset), TimeManager.AddMinutesTo24HourTime(this.EndTime, this.StartTimeOffset)));
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				this.UpdateShadows();
			}
		}

		// Token: 0x06005A42 RID: 23106 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnDrawGizmos()
		{
		}

		// Token: 0x06005A43 RID: 23107 RVA: 0x0017C884 File Offset: 0x0017AA84
		private void SetState(bool on)
		{
			if (this.BeamTracker != null)
			{
				this.BeamTracker.Enabled = this.isOn;
			}
			float num = 0f;
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				num = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			}
			if (num < this.LightMaxDistance * QualitySettings.lodBias)
			{
				this.Light.enabled = this.isOn;
			}
			else
			{
				this.Light.enabled = false;
			}
			if (on == this.isOn)
			{
				return;
			}
			this.isOn = on;
			if (this.LightRend != null)
			{
				this.LightRend.material = (this.isOn ? this.LightOnMat : this.LightOffMat);
			}
		}

		// Token: 0x06005A44 RID: 23108 RVA: 0x0017C94C File Offset: 0x0017AB4C
		private void UpdateShadows()
		{
			if (!this.ShadowsEnabled)
			{
				this.Light.shadows = LightShadows.None;
				return;
			}
			float num = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			if (num < this.SoftShadowsThreshold * QualitySettings.lodBias)
			{
				this.Light.shadows = LightShadows.Soft;
				return;
			}
			if (num < this.HardShadowsThreshold * QualitySettings.lodBias)
			{
				this.Light.shadows = LightShadows.Hard;
				return;
			}
			this.Light.shadows = LightShadows.None;
		}

		// Token: 0x0400424A RID: 16970
		public static Vector3 POWER_ORIGIN = new Vector3(150f, 0f, -150f);

		// Token: 0x0400424B RID: 16971
		[Header("References")]
		[SerializeField]
		protected MeshRenderer LightRend;

		// Token: 0x0400424C RID: 16972
		[SerializeField]
		protected Light Light;

		// Token: 0x0400424D RID: 16973
		[SerializeField]
		protected VolumetricLightTracker BeamTracker;

		// Token: 0x0400424E RID: 16974
		[Header("Materials")]
		public Material LightOnMat;

		// Token: 0x0400424F RID: 16975
		public Material LightOffMat;

		// Token: 0x04004250 RID: 16976
		[Header("Timing")]
		public int StartTime = 1800;

		// Token: 0x04004251 RID: 16977
		public int EndTime = 600;

		// Token: 0x04004252 RID: 16978
		public int StartTimeOffset;

		// Token: 0x04004253 RID: 16979
		[Header("Settings")]
		public bool ShadowsEnabled = true;

		// Token: 0x04004254 RID: 16980
		public float LightMaxDistance = 80f;

		// Token: 0x04004255 RID: 16981
		public float SoftShadowsThreshold = 12f;

		// Token: 0x04004256 RID: 16982
		public float HardShadowsThreshold = 36f;

		// Token: 0x04004257 RID: 16983
		private bool isOn;
	}
}
