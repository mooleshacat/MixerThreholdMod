using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x020008C3 RID: 2243
	public class SoilChunk : Clickable
	{
		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06003C85 RID: 15493 RVA: 0x000FF099 File Offset: 0x000FD299
		// (set) Token: 0x06003C86 RID: 15494 RVA: 0x000FF0A1 File Offset: 0x000FD2A1
		public float CurrentLerp { get; protected set; }

		// Token: 0x06003C87 RID: 15495 RVA: 0x000FF0AA File Offset: 0x000FD2AA
		protected virtual void Awake()
		{
			this.localPos_Start = base.transform.localPosition;
			this.localEulerAngles_Start = base.transform.localEulerAngles;
			this.localScale_Start = base.transform.localScale;
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x000FF0E0 File Offset: 0x000FD2E0
		public void SetLerpedTransform(float _lerp)
		{
			this.CurrentLerp = Mathf.Clamp(_lerp, 0f, 1f);
			base.transform.localPosition = Vector3.Lerp(this.localPos_Start, this.EndTransform.localPosition, this.CurrentLerp);
			base.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(this.localEulerAngles_Start), Quaternion.Euler(this.EndTransform.localEulerAngles), this.CurrentLerp);
			base.transform.localScale = Vector3.Lerp(this.localScale_Start, this.EndTransform.localScale, this.CurrentLerp);
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x000FF182 File Offset: 0x000FD382
		public override void StartClick(RaycastHit hit)
		{
			base.StartClick(hit);
			this.ClickableEnabled = false;
			this.StopLerp();
			this.lerpRoutine = base.StartCoroutine(this.<StartClick>g__Lerp|12_0());
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x000FF1AA File Offset: 0x000FD3AA
		public void StopLerp()
		{
			if (this.lerpRoutine != null)
			{
				base.StopCoroutine(this.lerpRoutine);
			}
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x000FF1D3 File Offset: 0x000FD3D3
		[CompilerGenerated]
		private IEnumerator <StartClick>g__Lerp|12_0()
		{
			for (float i = 0f; i < this.LerpTime; i += Time.deltaTime)
			{
				this.SetLerpedTransform(Mathf.Lerp(0f, 1f, i / this.LerpTime));
				yield return new WaitForEndOfFrame();
			}
			this.SetLerpedTransform(1f);
			this.lerpRoutine = null;
			yield break;
		}

		// Token: 0x04002B59 RID: 11097
		public Transform EndTransform;

		// Token: 0x04002B5A RID: 11098
		public float LerpTime = 0.4f;

		// Token: 0x04002B5B RID: 11099
		private Vector3 localPos_Start;

		// Token: 0x04002B5C RID: 11100
		private Vector3 localEulerAngles_Start;

		// Token: 0x04002B5D RID: 11101
		private Vector3 localScale_Start;

		// Token: 0x04002B5E RID: 11102
		private Coroutine lerpRoutine;
	}
}
