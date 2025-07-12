using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C13 RID: 3091
	public class BrickPressHandle : MonoBehaviour
	{
		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x060053AB RID: 21419 RVA: 0x001616B6 File Offset: 0x0015F8B6
		// (set) Token: 0x060053AC RID: 21420 RVA: 0x001616BE File Offset: 0x0015F8BE
		public bool Interactable { get; private set; }

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x060053AD RID: 21421 RVA: 0x001616C7 File Offset: 0x0015F8C7
		// (set) Token: 0x060053AE RID: 21422 RVA: 0x001616CF File Offset: 0x0015F8CF
		public float CurrentPosition { get; private set; }

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x060053AF RID: 21423 RVA: 0x001616D8 File Offset: 0x0015F8D8
		// (set) Token: 0x060053B0 RID: 21424 RVA: 0x001616E0 File Offset: 0x0015F8E0
		public float TargetPosition { get; private set; }

		// Token: 0x060053B1 RID: 21425 RVA: 0x001616EC File Offset: 0x0015F8EC
		private void Start()
		{
			this.SetPosition(0f);
			this.SetInteractable(false);
			this.HandleClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ClickStart));
			this.HandleClickable.onClickEnd.AddListener(new UnityAction(this.ClickEnd));
		}

		// Token: 0x060053B2 RID: 21426 RVA: 0x00161744 File Offset: 0x0015F944
		private void LateUpdate()
		{
			if (!this.Locked)
			{
				if (this.isMoving)
				{
					Vector3 vector = this.GetPlaneHit() + this.clickOffset;
					float position = 1f - Mathf.Clamp01(Mathf.InverseLerp(Mathf.Min(this.LoweredTransform.position.y, this.RaisedTransform.position.y), Mathf.Max(this.LoweredTransform.position.y, this.RaisedTransform.position.y), vector.y));
					this.SetPosition(position);
				}
				else
				{
					this.SetPosition(Mathf.MoveTowards(this.TargetPosition, 0f, Time.deltaTime));
				}
			}
			this.Move();
		}

		// Token: 0x060053B3 RID: 21427 RVA: 0x00161804 File Offset: 0x0015FA04
		private void Move()
		{
			this.CurrentPosition = Mathf.MoveTowards(this.CurrentPosition, this.TargetPosition, this.MoveSpeed * Time.deltaTime);
			base.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, 360f, this.CurrentPosition));
			if (Mathf.Abs(this.CurrentPosition - this.lastClickPosition) > 0.1666f)
			{
				this.lastClickPosition = this.CurrentPosition;
				this.ClickSound.AudioSource.pitch = Mathf.Lerp(0.7f, 1.1f, this.CurrentPosition);
				this.ClickSound.Play();
			}
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x001618B8 File Offset: 0x0015FAB8
		private void UpdateSound(float difference)
		{
			difference /= 0.05f;
			if (difference < 0f)
			{
				Mathf.Abs(difference);
			}
			if (difference > 0f)
			{
				Mathf.Abs(difference);
			}
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x001618E1 File Offset: 0x0015FAE1
		public void SetPosition(float position)
		{
			this.TargetPosition = position;
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x001618EA File Offset: 0x0015FAEA
		public void SetInteractable(bool e)
		{
			this.Interactable = e;
			this.HandleClickable.ClickableEnabled = e;
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x001618FF File Offset: 0x0015FAFF
		public void ClickStart(RaycastHit hit)
		{
			this.isMoving = true;
			this.clickOffset = this.HandleClickable.transform.position - this.GetPlaneHit();
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x00161929 File Offset: 0x0015FB29
		public void ClickEnd()
		{
			this.isMoving = false;
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x00161934 File Offset: 0x0015FB34
		private Vector3 GetPlaneHit()
		{
			Plane plane = new Plane(this.PlaneNormal.forward, this.PlaneNormal.position);
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x04003E65 RID: 15973
		private float lastClickPosition;

		// Token: 0x04003E66 RID: 15974
		[Header("Settings")]
		public float MoveSpeed = 1f;

		// Token: 0x04003E67 RID: 15975
		public bool Locked;

		// Token: 0x04003E68 RID: 15976
		[Header("References")]
		public Transform PlaneNormal;

		// Token: 0x04003E69 RID: 15977
		public Transform RaisedTransform;

		// Token: 0x04003E6A RID: 15978
		public Transform LoweredTransform;

		// Token: 0x04003E6B RID: 15979
		public Clickable HandleClickable;

		// Token: 0x04003E6C RID: 15980
		public AudioSourceController ClickSound;

		// Token: 0x04003E6D RID: 15981
		private Vector3 clickOffset = Vector3.zero;

		// Token: 0x04003E6E RID: 15982
		private bool isMoving;
	}
}
