using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C34 RID: 3124
	public class LabOvenWireTray : MonoBehaviour
	{
		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600566D RID: 22125 RVA: 0x0016DAEE File Offset: 0x0016BCEE
		// (set) Token: 0x0600566E RID: 22126 RVA: 0x0016DAF6 File Offset: 0x0016BCF6
		public bool Interactable { get; private set; }

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x0600566F RID: 22127 RVA: 0x0016DAFF File Offset: 0x0016BCFF
		// (set) Token: 0x06005670 RID: 22128 RVA: 0x0016DB07 File Offset: 0x0016BD07
		public float TargetPosition { get; private set; }

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06005671 RID: 22129 RVA: 0x0016DB10 File Offset: 0x0016BD10
		// (set) Token: 0x06005672 RID: 22130 RVA: 0x0016DB18 File Offset: 0x0016BD18
		public float ActualPosition { get; private set; }

		// Token: 0x06005673 RID: 22131 RVA: 0x0016DB21 File Offset: 0x0016BD21
		private void Start()
		{
			this.SetPosition(0f);
			this.SetInteractable(false);
		}

		// Token: 0x06005674 RID: 22132 RVA: 0x0016DB38 File Offset: 0x0016BD38
		private void LateUpdate()
		{
			if (this.isMoving)
			{
				Vector3 position = this.GetPlaneHit() + this.clickOffset;
				float num = this.PlaneNormal.InverseTransformPoint(position).y;
				Debug.Log("Hit offset: " + num.ToString());
				num = Mathf.Clamp01(Mathf.InverseLerp(-0.25f, 0.24f, num));
				this.TargetPosition = num;
			}
			this.Move();
			this.ClampAngle();
		}

		// Token: 0x06005675 RID: 22133 RVA: 0x0016DBB0 File Offset: 0x0016BDB0
		private void Move()
		{
			Vector3 b = Vector3.Lerp(this.ClosedPosition.localPosition, this.OpenPosition.localPosition, this.TargetPosition);
			this.Tray.localPosition = Vector3.Lerp(this.Tray.localPosition, b, Time.deltaTime * this.MoveSpeed);
			this.ActualPosition = Mathf.Lerp(this.ActualPosition, this.TargetPosition, Time.deltaTime * this.MoveSpeed);
		}

		// Token: 0x06005676 RID: 22134 RVA: 0x0016DC2C File Offset: 0x0016BE2C
		private void ClampAngle()
		{
			float max = this.DoorClampCurve.Evaluate(this.OvenDoor.ActualPosition);
			this.ActualPosition = Mathf.Clamp(this.ActualPosition, 0f, max);
			Vector3 localPosition = Vector3.Lerp(this.ClosedPosition.localPosition, this.OpenPosition.localPosition, this.ActualPosition);
			this.Tray.localPosition = localPosition;
		}

		// Token: 0x06005677 RID: 22135 RVA: 0x0016DC95 File Offset: 0x0016BE95
		public void SetInteractable(bool interactable)
		{
			this.Interactable = interactable;
		}

		// Token: 0x06005678 RID: 22136 RVA: 0x0016DC9E File Offset: 0x0016BE9E
		public void SetPosition(float position)
		{
			this.TargetPosition = position;
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x0016DCA7 File Offset: 0x0016BEA7
		public void ClickStart(RaycastHit hit)
		{
			this.isMoving = true;
		}

		// Token: 0x0600567A RID: 22138 RVA: 0x0016DCB0 File Offset: 0x0016BEB0
		private Vector3 GetPlaneHit()
		{
			Plane plane = new Plane(this.PlaneNormal.forward, this.PlaneNormal.position);
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x0016DD02 File Offset: 0x0016BF02
		public void ClickEnd()
		{
			this.isMoving = false;
		}

		// Token: 0x04003FEF RID: 16367
		public const float HIT_OFFSET_MAX = 0.24f;

		// Token: 0x04003FF0 RID: 16368
		public const float HIT_OFFSET_MIN = -0.25f;

		// Token: 0x04003FF4 RID: 16372
		[Header("References")]
		public Transform Tray;

		// Token: 0x04003FF5 RID: 16373
		public Transform PlaneNormal;

		// Token: 0x04003FF6 RID: 16374
		public Transform ClosedPosition;

		// Token: 0x04003FF7 RID: 16375
		public Transform OpenPosition;

		// Token: 0x04003FF8 RID: 16376
		public LabOvenDoor OvenDoor;

		// Token: 0x04003FF9 RID: 16377
		[Header("Settings")]
		public float MoveSpeed = 2f;

		// Token: 0x04003FFA RID: 16378
		public AnimationCurve DoorClampCurve;

		// Token: 0x04003FFB RID: 16379
		private Vector3 clickOffset = Vector3.zero;

		// Token: 0x04003FFC RID: 16380
		private bool isMoving;
	}
}
