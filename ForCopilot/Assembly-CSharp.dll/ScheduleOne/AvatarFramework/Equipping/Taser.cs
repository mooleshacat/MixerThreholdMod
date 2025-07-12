using System;
using System.Collections;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x020009C2 RID: 2498
	public class Taser : AvatarRangedWeapon
	{
		// Token: 0x0600439D RID: 17309 RVA: 0x0011C06E File Offset: 0x0011A26E
		public override void Equip(Avatar _avatar)
		{
			base.Equip(_avatar);
			this.FlashObject.gameObject.SetActive(false);
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x0011C088 File Offset: 0x0011A288
		public override void Shoot(Vector3 endPoint)
		{
			base.Shoot(endPoint);
			if (this.flashRoutine != null)
			{
				base.StopCoroutine(this.flashRoutine);
			}
			this.ChargeSound.Stop();
			this.flashRoutine = base.StartCoroutine(this.Flash(endPoint));
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x0011C0C3 File Offset: 0x0011A2C3
		public override void SetIsRaised(bool raised)
		{
			base.SetIsRaised(raised);
			if (base.IsRaised)
			{
				this.ChargeSound.Play();
				return;
			}
			this.ChargeSound.Stop();
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x0011C0EB File Offset: 0x0011A2EB
		private IEnumerator Flash(Vector3 endPoint)
		{
			float t = 0.2f;
			this.FlashObject.gameObject.SetActive(true);
			Transform transform = UnityEngine.Object.Instantiate<GameObject>(this.RayPrefab, GameObject.Find("_Temp").transform).transform;
			UnityEngine.Object.Destroy(transform.gameObject, t);
			transform.transform.position = (this.MuzzlePoint.position + endPoint) / 2f;
			transform.transform.LookAt(endPoint);
			transform.transform.localScale = new Vector3(1f, 1f, Vector3.Distance(this.MuzzlePoint.position, endPoint));
			yield return new WaitForSeconds(0.2f);
			this.FlashObject.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0400306B RID: 12395
		public const float TaseDuration = 2f;

		// Token: 0x0400306C RID: 12396
		public const float TaseMoveSpeedMultiplier = 0.5f;

		// Token: 0x0400306D RID: 12397
		[Header("References")]
		public GameObject FlashObject;

		// Token: 0x0400306E RID: 12398
		public AudioSourceController ChargeSound;

		// Token: 0x0400306F RID: 12399
		[Header("Prefabs")]
		public GameObject RayPrefab;

		// Token: 0x04003070 RID: 12400
		private Coroutine flashRoutine;
	}
}
