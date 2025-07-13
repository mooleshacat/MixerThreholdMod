using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C73 RID: 3187
	public class FoliageRustleSound : MonoBehaviour
	{
		// Token: 0x0600599B RID: 22939 RVA: 0x0017A6D0 File Offset: 0x001788D0
		private void Awake()
		{
			base.InvokeRepeating("UpdateActive", UnityEngine.Random.Range(0f, 3f), 3f);
			this.Container.SetActive(false);
		}

		// Token: 0x0600599C RID: 22940 RVA: 0x0017A700 File Offset: 0x00178900
		public void OnTriggerEnter(Collider other)
		{
			if (Time.timeSinceLevelLoad - this.timeOnLastHit > 1f && other.gameObject.layer == LayerMask.NameToLayer("Player"))
			{
				Player componentInParent = other.gameObject.GetComponentInParent<Player>();
				if (componentInParent != null)
				{
					if (componentInParent.IsOwner)
					{
						this.Sound.VolumeMultiplier = Mathf.Clamp01(PlayerSingleton<PlayerMovement>.Instance.Controller.velocity.magnitude / (PlayerMovement.WalkSpeed * PlayerMovement.SprintMultiplier));
					}
					else
					{
						this.Sound.VolumeMultiplier = 1f;
					}
					this.Sound.Play();
					this.timeOnLastHit = Time.timeSinceLevelLoad;
				}
			}
		}

		// Token: 0x0600599D RID: 22941 RVA: 0x0017A7B4 File Offset: 0x001789B4
		private void UpdateActive()
		{
			if (Player.Local == null)
			{
				return;
			}
			float num = Vector3.SqrMagnitude(Player.Local.Avatar.CenterPoint - base.transform.position);
			this.Container.SetActive(num < 900f);
		}

		// Token: 0x040041B8 RID: 16824
		public const float ACTIVATION_RANGE_SQUARED = 900f;

		// Token: 0x040041B9 RID: 16825
		public const float COOLDOWN = 1f;

		// Token: 0x040041BA RID: 16826
		public AudioSourceController Sound;

		// Token: 0x040041BB RID: 16827
		public GameObject Container;

		// Token: 0x040041BC RID: 16828
		private float timeOnLastHit;
	}
}
