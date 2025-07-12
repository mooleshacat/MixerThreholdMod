using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vision
{
	// Token: 0x02000288 RID: 648
	[RequireComponent(typeof(Light))]
	public class LightVisibilityAffector : MonoBehaviour
	{
		// Token: 0x06000D8A RID: 3466 RVA: 0x0003BA3E File Offset: 0x00039C3E
		protected virtual void Awake()
		{
			this.light = base.GetComponent<Light>();
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0003BA6C File Offset: 0x00039C6C
		private void PlayerSpawned()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			PlayerSingleton<PlayerMovement>.Instance.RegisterMovementEvent(this.updateDistanceThreshold, new Action(this.UpdateVisibility));
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0003BAAB File Offset: 0x00039CAB
		private void OnDestroy()
		{
			if (PlayerSingleton<PlayerMovement>.Instance != null)
			{
				PlayerSingleton<PlayerMovement>.Instance.DeregisterMovementEvent(new Action(this.UpdateVisibility));
			}
			this.ClearAttribute();
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0003BAD8 File Offset: 0x00039CD8
		protected virtual void UpdateVisibility()
		{
			if (this.light == null || base.gameObject == null)
			{
				return;
			}
			if (!this.light.enabled || !base.gameObject.activeInHierarchy)
			{
				this.ClearAttribute();
				return;
			}
			if (Player.Local == null)
			{
				return;
			}
			float num = Player.Local.Visibility.CalculateExposureToPoint(base.transform.position, this.light.range, null);
			if (num == 0f)
			{
				this.ClearAttribute();
				return;
			}
			float num2 = Mathf.Pow(1f - Mathf.Clamp(Vector3.Distance(base.transform.position, Player.Local.Avatar.CenterPoint) / this.light.range, 0f, 1f), 2f);
			float num3 = 1f - Singleton<EnvironmentFX>.Instance.normalizedEnvironmentalBrightness;
			float num4 = 1f;
			if (this.light.type == LightType.Spot)
			{
				float num5 = Vector3.Angle(base.transform.forward, (Player.Local.Avatar.CenterPoint - base.transform.position).normalized);
				if (num5 > this.light.spotAngle * 0.5f)
				{
					num4 = 0f;
				}
				else
				{
					float num6 = this.light.spotAngle * 0.5f - num5;
					float num7 = this.light.spotAngle * 0.5f - this.light.innerSpotAngle * 0.5f;
					num4 = Mathf.Clamp(num6 / num7, 0f, 1f);
				}
			}
			float visibity = num * num2 * this.light.intensity * num3 * num4 * ((this.light.type == LightType.Spot) ? 10f : 15f) * this.EffectMultiplier;
			this.UpdateAttribute(visibity);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0003BCBC File Offset: 0x00039EBC
		private void UpdateAttribute(float visibity)
		{
			if (visibity <= 0f)
			{
				this.ClearAttribute();
				return;
			}
			if (this.attribute != null)
			{
				this.attribute.pointsChange = visibity;
				return;
			}
			if (this.uniquenessCode != string.Empty)
			{
				this.attribute = new UniqueVisibilityAttribute("Light Exposure (" + base.gameObject.name + ")", visibity, this.uniquenessCode, 1f, -1);
				return;
			}
			this.attribute = new VisibilityAttribute("Light Exposure (" + base.gameObject.name + ")", visibity, 1f, -1);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0003BD5E File Offset: 0x00039F5E
		private void ClearAttribute()
		{
			if (this.attribute == null)
			{
				return;
			}
			this.attribute.Delete();
			this.attribute = null;
		}

		// Token: 0x04000DF2 RID: 3570
		public const float PointLightEffect = 15f;

		// Token: 0x04000DF3 RID: 3571
		public const float SpotLightEffect = 10f;

		// Token: 0x04000DF4 RID: 3572
		[Header("Settings")]
		public float EffectMultiplier = 1f;

		// Token: 0x04000DF5 RID: 3573
		public string uniquenessCode = "Light";

		// Token: 0x04000DF6 RID: 3574
		[Tooltip("How far does the player have to move for visibility to be recalculated?")]
		public int updateDistanceThreshold = 1;

		// Token: 0x04000DF7 RID: 3575
		protected Light light;

		// Token: 0x04000DF8 RID: 3576
		protected VisibilityAttribute attribute;
	}
}
