using System;
using System.Collections;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x020009C0 RID: 2496
	public class Handgun : AvatarRangedWeapon
	{
		// Token: 0x06004394 RID: 17300 RVA: 0x0011BE90 File Offset: 0x0011A090
		public override void Shoot(Vector3 endPoint)
		{
			base.Shoot(endPoint);
			this.Anim.Play();
			this.ShellParticles.Play();
			this.SmokeParticles.Play();
			Player componentInParent = base.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent.IsOwner)
			{
				return;
			}
			if (this.flashRoutine != null)
			{
				base.StopCoroutine(this.flashRoutine);
			}
			this.flashRoutine = base.StartCoroutine(this.Flash(endPoint));
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0011BF06 File Offset: 0x0011A106
		private IEnumerator Flash(Vector3 endPoint)
		{
			float num = 0.06f;
			this.FlashObject.localEulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(0f, 360f));
			this.FlashObject.gameObject.SetActive(true);
			Transform transform = UnityEngine.Object.Instantiate<GameObject>(this.RayPrefab, GameObject.Find("_Temp").transform).transform;
			UnityEngine.Object.Destroy(transform.gameObject, num);
			transform.transform.position = (this.MuzzlePoint.position + endPoint) / 2f;
			transform.transform.LookAt(endPoint);
			transform.transform.localScale = new Vector3(1f, 1f, Vector3.Distance(this.MuzzlePoint.position, endPoint));
			yield return new WaitForSeconds(num);
			this.FlashObject.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04003061 RID: 12385
		[Header("References")]
		public Animation Anim;

		// Token: 0x04003062 RID: 12386
		public ParticleSystem ShellParticles;

		// Token: 0x04003063 RID: 12387
		public ParticleSystem SmokeParticles;

		// Token: 0x04003064 RID: 12388
		public Transform FlashObject;

		// Token: 0x04003065 RID: 12389
		[Header("Prefabs")]
		public GameObject RayPrefab;

		// Token: 0x04003066 RID: 12390
		private Coroutine flashRoutine;
	}
}
