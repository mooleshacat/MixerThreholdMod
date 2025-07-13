using System;
using System.Runtime.CompilerServices;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000728 RID: 1832
	[RequireComponent(typeof(Light))]
	[ExecuteInEditMode]
	public class OptimizedLight : MonoBehaviour
	{
		// Token: 0x0600319D RID: 12701 RVA: 0x000CF0D3 File Offset: 0x000CD2D3
		public virtual void Awake()
		{
			this._Light = base.GetComponent<Light>();
			this.maxDistanceSquared = this.MaxDistance * this.MaxDistance;
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x000CF0F4 File Offset: 0x000CD2F4
		private void Start()
		{
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				this.<Start>g__Register|7_0();
				return;
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.<Start>g__Register|7_0));
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x000CF124 File Offset: 0x000CD324
		private void OnDestroy()
		{
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.DeregisterMovementEvent(new Action(this.UpdateCull));
			}
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x000CF143 File Offset: 0x000CD343
		public virtual void FixedUpdate()
		{
			if (this._Light != null)
			{
				this._Light.enabled = (this.Enabled && !this.DisabledForOptimization && !this.culled);
			}
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x000CF17C File Offset: 0x000CD37C
		private void UpdateCull()
		{
			if (this == null || base.gameObject == null)
			{
				return;
			}
			this.culled = (Vector3.SqrMagnitude(PlayerSingleton<PlayerCamera>.Instance.transform.position - base.transform.position) > this.maxDistanceSquared * QualitySettings.lodBias);
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x000CF1D9 File Offset: 0x000CD3D9
		public void SetEnabled(bool enabled)
		{
			this.Enabled = enabled;
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x000CF1FC File Offset: 0x000CD3FC
		[CompilerGenerated]
		private void <Start>g__Register|7_0()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.<Start>g__Register|7_0));
			PlayerSingleton<PlayerCamera>.Instance.RegisterMovementEvent(Mathf.RoundToInt(Mathf.Clamp(this.MaxDistance / 10f, 0.5f, 20f)), new Action(this.UpdateCull));
		}

		// Token: 0x040022EA RID: 8938
		public bool Enabled = true;

		// Token: 0x040022EB RID: 8939
		[HideInInspector]
		public bool DisabledForOptimization;

		// Token: 0x040022EC RID: 8940
		[Range(10f, 500f)]
		public float MaxDistance = 100f;

		// Token: 0x040022ED RID: 8941
		public Light _Light;

		// Token: 0x040022EE RID: 8942
		private bool culled;

		// Token: 0x040022EF RID: 8943
		private float maxDistanceSquared;
	}
}
