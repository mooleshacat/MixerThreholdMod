using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C32 RID: 3122
	public class LabOvenDoor : MonoBehaviour
	{
		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x0600565A RID: 22106 RVA: 0x0016D77E File Offset: 0x0016B97E
		// (set) Token: 0x0600565B RID: 22107 RVA: 0x0016D786 File Offset: 0x0016B986
		public bool Interactable { get; private set; }

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x0600565C RID: 22108 RVA: 0x0016D78F File Offset: 0x0016B98F
		// (set) Token: 0x0600565D RID: 22109 RVA: 0x0016D797 File Offset: 0x0016B997
		public float TargetPosition { get; private set; }

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x0600565E RID: 22110 RVA: 0x0016D7A0 File Offset: 0x0016B9A0
		// (set) Token: 0x0600565F RID: 22111 RVA: 0x0016D7A8 File Offset: 0x0016B9A8
		public float ActualPosition { get; private set; }

		// Token: 0x06005660 RID: 22112 RVA: 0x0016D7B4 File Offset: 0x0016B9B4
		private void Start()
		{
			this.SetPosition(0f);
			this.SetInteractable(false);
			this.HandleClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ClickStart));
			this.HandleClickable.onClickEnd.AddListener(new UnityAction(this.ClickEnd));
		}

		// Token: 0x06005661 RID: 22113 RVA: 0x0016D80C File Offset: 0x0016BA0C
		private void LateUpdate()
		{
			if (this.isMoving)
			{
				Vector3 position = this.GetPlaneHit() + this.clickOffset;
				float num = this.PlaneNormal.InverseTransformPoint(position).y;
				num = Mathf.Clamp01(Mathf.InverseLerp(-0.25f, 0.24f, num));
				this.SetPosition(this.HitMapCurve.Evaluate(num));
			}
			this.Move();
		}

		// Token: 0x06005662 RID: 22114 RVA: 0x0016D874 File Offset: 0x0016BA74
		private void Move()
		{
			float y = Mathf.Lerp(90f, 10f, this.TargetPosition);
			Quaternion b = Quaternion.Euler(0f, y, 0f);
			this.Door.localRotation = Quaternion.Lerp(this.Door.localRotation, b, Time.deltaTime * this.DoorMoveSpeed);
			this.ActualPosition = Mathf.Lerp(this.ActualPosition, this.TargetPosition, Time.deltaTime * this.DoorMoveSpeed);
		}

		// Token: 0x06005663 RID: 22115 RVA: 0x0016D8F3 File Offset: 0x0016BAF3
		public void SetInteractable(bool interactable)
		{
			this.Interactable = interactable;
			this.HandleClickable.ClickableEnabled = interactable;
		}

		// Token: 0x06005664 RID: 22116 RVA: 0x0016D908 File Offset: 0x0016BB08
		public void SetPosition(float newPosition)
		{
			float targetPosition = this.TargetPosition;
			this.TargetPosition = newPosition;
			if (targetPosition == 0f && newPosition > 0.02f)
			{
				this.OpenSound.Play();
				return;
			}
			if (targetPosition >= 0.98f && newPosition < 0.98f)
			{
				this.CloseSound.Play();
				return;
			}
			if (targetPosition > 0.01f && newPosition <= 0.001f)
			{
				this.ShutSound.Play();
			}
		}

		// Token: 0x06005665 RID: 22117 RVA: 0x0016D976 File Offset: 0x0016BB76
		public void ClickStart(RaycastHit hit)
		{
			this.isMoving = true;
			this.clickOffset = this.HandleClickable.transform.position - this.GetPlaneHit();
		}

		// Token: 0x06005666 RID: 22118 RVA: 0x0016D9A0 File Offset: 0x0016BBA0
		private Vector3 GetPlaneHit()
		{
			Plane plane = new Plane(this.PlaneNormal.forward, this.PlaneNormal.position);
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x06005667 RID: 22119 RVA: 0x0016D9F2 File Offset: 0x0016BBF2
		public void ClickEnd()
		{
			this.isMoving = false;
		}

		// Token: 0x04003FD3 RID: 16339
		public const float HIT_OFFSET_MAX = 0.24f;

		// Token: 0x04003FD4 RID: 16340
		public const float HIT_OFFSET_MIN = -0.25f;

		// Token: 0x04003FD5 RID: 16341
		public const float DOOR_ANGLE_CLOSED = 90f;

		// Token: 0x04003FD6 RID: 16342
		public const float DOOR_ANGLE_OPEN = 10f;

		// Token: 0x04003FDA RID: 16346
		[Header("References")]
		public Clickable HandleClickable;

		// Token: 0x04003FDB RID: 16347
		public Transform Door;

		// Token: 0x04003FDC RID: 16348
		public Transform PlaneNormal;

		// Token: 0x04003FDD RID: 16349
		public AnimationCurve HitMapCurve;

		// Token: 0x04003FDE RID: 16350
		[Header("Sounds")]
		public AudioSourceController OpenSound;

		// Token: 0x04003FDF RID: 16351
		public AudioSourceController CloseSound;

		// Token: 0x04003FE0 RID: 16352
		public AudioSourceController ShutSound;

		// Token: 0x04003FE1 RID: 16353
		[Header("Settings")]
		public float DoorMoveSpeed = 2f;

		// Token: 0x04003FE2 RID: 16354
		private Vector3 clickOffset = Vector3.zero;

		// Token: 0x04003FE3 RID: 16355
		private bool isMoving;
	}
}
