using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C23 RID: 3107
	public class LabStand : MonoBehaviour
	{
		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x060054FA RID: 21754 RVA: 0x00167526 File Offset: 0x00165726
		// (set) Token: 0x060054FB RID: 21755 RVA: 0x0016752E File Offset: 0x0016572E
		public bool Interactable { get; private set; }

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x060054FC RID: 21756 RVA: 0x00167537 File Offset: 0x00165737
		// (set) Token: 0x060054FD RID: 21757 RVA: 0x0016753F File Offset: 0x0016573F
		public float CurrentPosition { get; private set; } = 1f;

		// Token: 0x060054FE RID: 21758 RVA: 0x00167548 File Offset: 0x00165748
		private void Start()
		{
			this.SetPosition(1f);
			this.SetInteractable(false);
			this.HandleClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ClickStart));
			this.HandleClickable.onClickEnd.AddListener(new UnityAction(this.ClickEnd));
		}

		// Token: 0x060054FF RID: 21759 RVA: 0x001675A0 File Offset: 0x001657A0
		private void LateUpdate()
		{
			if (this.isMoving)
			{
				Vector3 vector = this.GetPlaneHit() + this.clickOffset;
				float position = Mathf.Clamp01(Mathf.InverseLerp(Mathf.Min(this.LoweredTransform.position.y, this.RaisedTransform.position.y), Mathf.Max(this.LoweredTransform.position.y, this.RaisedTransform.position.y), vector.y));
				this.SetPosition(position);
			}
			this.Highlight.gameObject.SetActive(this.Interactable && !this.isMoving);
			this.Move();
			this.Funnel.gameObject.SetActive(this.FunnelEnabled && this.CurrentPosition < this.FunnelThreshold);
		}

		// Token: 0x06005500 RID: 21760 RVA: 0x0016767C File Offset: 0x0016587C
		private void Move()
		{
			float y = this.GripTransform.localPosition.y;
			Vector3 b = Vector3.Lerp(this.LoweredTransform.localPosition, this.RaisedTransform.localPosition, this.CurrentPosition);
			Quaternion b2 = Quaternion.Lerp(this.LoweredTransform.localRotation, this.RaisedTransform.localRotation, this.CurrentPosition);
			this.GripTransform.localPosition = Vector3.Lerp(this.GripTransform.localPosition, b, Time.deltaTime * this.MoveSpeed);
			this.GripTransform.localRotation = Quaternion.Lerp(this.GripTransform.localRotation, b2, Time.deltaTime * this.MoveSpeed);
			float num = this.GripTransform.localPosition.y - y;
			this.SpinnyThingy.Rotate(Vector3.up, num * 1800f, Space.Self);
			this.UpdateSound(num);
		}

		// Token: 0x06005501 RID: 21761 RVA: 0x00167760 File Offset: 0x00165960
		private void UpdateSound(float difference)
		{
			difference /= 0.05f;
			float num = 0f;
			if (difference < 0f)
			{
				num = Mathf.Abs(difference);
			}
			float num2 = 0f;
			if (difference > 0f)
			{
				num2 = Mathf.Abs(difference);
			}
			this.LowerSound.VolumeMultiplier = num;
			this.RaiseSound.VolumeMultiplier = num2;
			if (num > 0f && !this.LowerSound.AudioSource.isPlaying)
			{
				this.LowerSound.Play();
			}
			else if (num == 0f)
			{
				this.LowerSound.Stop();
			}
			if (num2 > 0f && !this.RaiseSound.AudioSource.isPlaying)
			{
				this.RaiseSound.Play();
				return;
			}
			if (num2 == 0f)
			{
				this.RaiseSound.Stop();
			}
		}

		// Token: 0x06005502 RID: 21762 RVA: 0x0016782B File Offset: 0x00165A2B
		public void SetPosition(float position)
		{
			this.CurrentPosition = position;
		}

		// Token: 0x06005503 RID: 21763 RVA: 0x00167834 File Offset: 0x00165A34
		public void SetInteractable(bool e)
		{
			this.Interactable = e;
			this.HandleClickable.ClickableEnabled = e;
			if (this.Interactable)
			{
				this.Anim.Play();
				return;
			}
			this.Anim.Stop();
		}

		// Token: 0x06005504 RID: 21764 RVA: 0x00167869 File Offset: 0x00165A69
		public void ClickStart(RaycastHit hit)
		{
			this.isMoving = true;
			this.clickOffset = this.HandleClickable.transform.position - this.GetPlaneHit();
		}

		// Token: 0x06005505 RID: 21765 RVA: 0x00167894 File Offset: 0x00165A94
		private Vector3 GetPlaneHit()
		{
			Plane plane = new Plane(this.PlaneNormal.forward, this.PlaneNormal.position);
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x001678E6 File Offset: 0x00165AE6
		public void ClickEnd()
		{
			this.isMoving = false;
		}

		// Token: 0x04003F20 RID: 16160
		[Header("Settings")]
		public float MoveSpeed = 1f;

		// Token: 0x04003F21 RID: 16161
		public bool FunnelEnabled;

		// Token: 0x04003F22 RID: 16162
		public float FunnelThreshold = 0.05f;

		// Token: 0x04003F23 RID: 16163
		[Header("References")]
		public Animation Anim;

		// Token: 0x04003F24 RID: 16164
		public Transform GripTransform;

		// Token: 0x04003F25 RID: 16165
		public Transform SpinnyThingy;

		// Token: 0x04003F26 RID: 16166
		public Transform RaisedTransform;

		// Token: 0x04003F27 RID: 16167
		public Transform LoweredTransform;

		// Token: 0x04003F28 RID: 16168
		public Transform PlaneNormal;

		// Token: 0x04003F29 RID: 16169
		public Clickable HandleClickable;

		// Token: 0x04003F2A RID: 16170
		public Transform Funnel;

		// Token: 0x04003F2B RID: 16171
		public GameObject Highlight;

		// Token: 0x04003F2C RID: 16172
		public AudioSourceController LowerSound;

		// Token: 0x04003F2D RID: 16173
		public AudioSourceController RaiseSound;

		// Token: 0x04003F2E RID: 16174
		private Vector3 clickOffset = Vector3.zero;

		// Token: 0x04003F2F RID: 16175
		private bool isMoving;
	}
}
