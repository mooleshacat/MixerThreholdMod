using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x020006CB RID: 1739
	public class SlidingDoor : MonoBehaviour
	{
		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x000C92E5 File Offset: 0x000C74E5
		// (set) Token: 0x06002FDF RID: 12255 RVA: 0x000C92ED File Offset: 0x000C74ED
		public bool IsOpen { get; protected set; }

		// Token: 0x06002FE0 RID: 12256 RVA: 0x000C92F6 File Offset: 0x000C74F6
		public virtual void Opened(EDoorSide openSide)
		{
			this.IsOpen = true;
			this.Move();
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x000C9305 File Offset: 0x000C7505
		public virtual void Closed()
		{
			this.IsOpen = false;
			this.Move();
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x000C9314 File Offset: 0x000C7514
		private void Move()
		{
			if (this.MoveRoutine != null)
			{
				base.StopCoroutine(this.MoveRoutine);
			}
			this.MoveRoutine = base.StartCoroutine(this.<Move>g__Move|12_0());
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x000C934F File Offset: 0x000C754F
		[CompilerGenerated]
		private IEnumerator <Move>g__Move|12_0()
		{
			Vector3 start = this.DoorTransform.position;
			Vector3 end = this.IsOpen ? this.OpenPosition.position : this.ClosedPosition.position;
			for (float i = 0f; i < this.SlideDuration; i += Time.deltaTime)
			{
				this.DoorTransform.position = Vector3.Lerp(start, end, this.SlideCurve.Evaluate(i / this.SlideDuration));
				yield return new WaitForEndOfFrame();
			}
			this.DoorTransform.position = end;
			this.MoveRoutine = null;
			yield break;
		}

		// Token: 0x040021B1 RID: 8625
		[Header("Settings")]
		public Transform DoorTransform;

		// Token: 0x040021B2 RID: 8626
		public Transform ClosedPosition;

		// Token: 0x040021B3 RID: 8627
		public Transform OpenPosition;

		// Token: 0x040021B4 RID: 8628
		public float SlideDuration = 3f;

		// Token: 0x040021B5 RID: 8629
		public AnimationCurve SlideCurve;

		// Token: 0x040021B6 RID: 8630
		private Coroutine MoveRoutine;
	}
}
