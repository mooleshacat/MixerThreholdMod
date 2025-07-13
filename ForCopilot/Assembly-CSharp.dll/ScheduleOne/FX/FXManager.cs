using System;
using System.Linq;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.FX
{
	// Token: 0x02000658 RID: 1624
	public class FXManager : Singleton<FXManager>
	{
		// Token: 0x06002A25 RID: 10789 RVA: 0x000AE7A1 File Offset: 0x000AC9A1
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x000AE7AC File Offset: 0x000AC9AC
		public void CreateImpactFX(Impact impact)
		{
			AudioClip impactSound = this.GetImpactSound(impact);
			if (impactSound != null)
			{
				this.PlayImpact(impactSound, impact.HitPoint, Mathf.Clamp01(impact.ImpactForce / 400f));
			}
			GameObject impactParticles = this.GetImpactParticles(impact);
			if (impactParticles != null)
			{
				this.PlayParticles(impactParticles, impact.HitPoint, Quaternion.LookRotation(impact.HitPoint));
			}
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x000AE814 File Offset: 0x000ACA14
		public void CreateBulletTrail(Vector3 start, Vector3 dir, float speed, float range, LayerMask mask)
		{
			FXManager.<>c__DisplayClass7_0 CS$<>8__locals1 = new FXManager.<>c__DisplayClass7_0();
			CS$<>8__locals1.start = start;
			CS$<>8__locals1.trail = UnityEngine.Object.Instantiate<TrailRenderer>(this.BulletTrail, NetworkSingleton<GameManager>.Instance.Temp);
			CS$<>8__locals1.trail.transform.position = CS$<>8__locals1.start;
			CS$<>8__locals1.trail.transform.forward = dir;
			CS$<>8__locals1.maxDistance = range;
			RaycastHit raycastHit;
			if (Physics.Raycast(CS$<>8__locals1.start, dir, ref raycastHit, range, mask))
			{
				CS$<>8__locals1.maxDistance = raycastHit.distance;
			}
			Debug.DrawRay(CS$<>8__locals1.start, dir * CS$<>8__locals1.maxDistance, Color.red, 5f);
			base.StartCoroutine(CS$<>8__locals1.<CreateBulletTrail>g__Routine|0());
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x000AE8CC File Offset: 0x000ACACC
		private void PlayImpact(AudioClip clip, Vector3 position, float volume)
		{
			AudioSourceController source = this.GetSource();
			if (source == null)
			{
				Console.LogWarning("No available audio source controller found", null);
				return;
			}
			source.transform.position = position;
			source.AudioSource.clip = clip;
			source.VolumeMultiplier = volume;
			source.AudioSource.Play();
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x000AE91F File Offset: 0x000ACB1F
		private void PlayParticles(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation), 2f);
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x000AE933 File Offset: 0x000ACB33
		private AudioClip GetImpactSound(Impact impact)
		{
			if (!(impact.Hit.collider.GetComponentInParent<NPC>() != null))
			{
				return null;
			}
			if (impact.ImpactType == EImpactType.SharpMetal)
			{
				return FXManager.GetRandomClip(this.SlashImpactClips);
			}
			return FXManager.GetRandomClip(this.PunchImpactsClips);
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x000AE96F File Offset: 0x000ACB6F
		private GameObject GetImpactParticles(Impact impact)
		{
			if (impact.Hit.collider.GetComponentInParent<NPC>() != null)
			{
				return this.PunchParticlePrefab;
			}
			return null;
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x000AE991 File Offset: 0x000ACB91
		private AudioSourceController GetSource()
		{
			return this.ImpactSources.FirstOrDefault((AudioSourceController x) => !x.isPlaying);
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x000AE9BD File Offset: 0x000ACBBD
		private static AudioClip GetRandomClip(AudioClip[] clips)
		{
			return clips[UnityEngine.Random.Range(0, clips.Length)];
		}

		// Token: 0x04001ED3 RID: 7891
		public AudioClip[] PunchImpactsClips;

		// Token: 0x04001ED4 RID: 7892
		public AudioClip[] SlashImpactClips;

		// Token: 0x04001ED5 RID: 7893
		[Header("References")]
		public AudioSourceController[] ImpactSources;

		// Token: 0x04001ED6 RID: 7894
		[Header("Particle Prefabs")]
		public GameObject PunchParticlePrefab;

		// Token: 0x04001ED7 RID: 7895
		[Header("Trails")]
		public TrailRenderer BulletTrail;
	}
}
