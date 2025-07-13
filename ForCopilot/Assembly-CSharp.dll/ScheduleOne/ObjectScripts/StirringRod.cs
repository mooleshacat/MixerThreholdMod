using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C24 RID: 3108
	public class StirringRod : MonoBehaviour
	{
		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06005508 RID: 21768 RVA: 0x00167923 File Offset: 0x00165B23
		// (set) Token: 0x06005509 RID: 21769 RVA: 0x0016792B File Offset: 0x00165B2B
		public bool Interactable { get; private set; }

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x0600550A RID: 21770 RVA: 0x00167934 File Offset: 0x00165B34
		// (set) Token: 0x0600550B RID: 21771 RVA: 0x0016793C File Offset: 0x00165B3C
		public float CurrentStirringSpeed { get; private set; }

		// Token: 0x0600550C RID: 21772 RVA: 0x00167948 File Offset: 0x00165B48
		private void Start()
		{
			this.SetInteractable(true);
			this.Clickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ClickStart));
			this.Clickable.onClickEnd.AddListener(new UnityAction(this.ClickEnd));
		}

		// Token: 0x0600550D RID: 21773 RVA: 0x00167994 File Offset: 0x00165B94
		private void Update()
		{
			float volumeMultiplier = Mathf.MoveTowards(this.StirSound.VolumeMultiplier, this.CurrentStirringSpeed, Time.deltaTime * 4f);
			this.StirSound.VolumeMultiplier = volumeMultiplier;
			if (this.StirSound.VolumeMultiplier > 0f && !this.StirSound.AudioSource.isPlaying)
			{
				this.StirSound.AudioSource.Play();
				return;
			}
			if (this.StirSound.VolumeMultiplier == 0f)
			{
				this.StirSound.AudioSource.Stop();
			}
		}

		// Token: 0x0600550E RID: 21774 RVA: 0x00167A28 File Offset: 0x00165C28
		private void LateUpdate()
		{
			if (this.isMoving)
			{
				Vector3 forward = this.Container.forward;
				Vector3 planeHit = this.GetPlaneHit();
				float d = Vector3.SignedAngle(this.PlaneNormal.forward, planeHit - this.PlaneNormal.position, this.PlaneNormal.up);
				Quaternion b = this.PlaneNormal.rotation * Quaternion.Euler(Vector3.up * d);
				this.Container.rotation = Quaternion.Lerp(this.Container.rotation, b, Time.deltaTime * this.LerpSpeed);
				float f = Vector3.SignedAngle(forward, this.Container.forward, this.PlaneNormal.up);
				this.CurrentStirringSpeed = Mathf.Clamp01(Mathf.Abs(f) / 20f);
				this.RodPivot.localEulerAngles = new Vector3(7f * (1f - this.CurrentStirringSpeed), 0f, 0f);
				return;
			}
			this.CurrentStirringSpeed = 0f;
		}

		// Token: 0x0600550F RID: 21775 RVA: 0x00167B33 File Offset: 0x00165D33
		public void SetInteractable(bool e)
		{
			this.Interactable = e;
			this.Clickable.ClickableEnabled = e;
		}

		// Token: 0x06005510 RID: 21776 RVA: 0x00167B48 File Offset: 0x00165D48
		public void ClickStart(RaycastHit hit)
		{
			this.isMoving = true;
			this.clickOffset = this.Clickable.transform.position - this.GetPlaneHit();
		}

		// Token: 0x06005511 RID: 21777 RVA: 0x00167B74 File Offset: 0x00165D74
		private Vector3 GetPlaneHit()
		{
			Plane plane = new Plane(this.PlaneNormal.up, this.PlaneNormal.position);
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x00167BC6 File Offset: 0x00165DC6
		public void ClickEnd()
		{
			this.isMoving = false;
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x001007D2 File Offset: 0x000FE9D2
		public void Destroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04003F30 RID: 16176
		public const float MAX_STIR_RATE = 20f;

		// Token: 0x04003F31 RID: 16177
		public const float MAX_PIVOT_ANGLE = 7f;

		// Token: 0x04003F34 RID: 16180
		public float LerpSpeed = 10f;

		// Token: 0x04003F35 RID: 16181
		[Header("References")]
		public Clickable Clickable;

		// Token: 0x04003F36 RID: 16182
		public Transform PlaneNormal;

		// Token: 0x04003F37 RID: 16183
		public Transform Container;

		// Token: 0x04003F38 RID: 16184
		public Transform RodPivot;

		// Token: 0x04003F39 RID: 16185
		public AudioSourceController StirSound;

		// Token: 0x04003F3A RID: 16186
		private Vector3 clickOffset = Vector3.zero;

		// Token: 0x04003F3B RID: 16187
		private bool isMoving;
	}
}
