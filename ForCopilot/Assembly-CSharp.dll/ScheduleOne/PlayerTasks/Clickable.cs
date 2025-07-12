using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000350 RID: 848
	public class Clickable : MonoBehaviour
	{
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x0005249A File Offset: 0x0005069A
		// (set) Token: 0x060012F5 RID: 4853 RVA: 0x000524A2 File Offset: 0x000506A2
		public virtual CursorManager.ECursorType HoveredCursor { get; protected set; } = CursorManager.ECursorType.Finger;

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x000524AB File Offset: 0x000506AB
		// (set) Token: 0x060012F7 RID: 4855 RVA: 0x000524B3 File Offset: 0x000506B3
		public Vector3 originalHitPoint { get; protected set; } = Vector3.zero;

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x000524BC File Offset: 0x000506BC
		// (set) Token: 0x060012F9 RID: 4857 RVA: 0x000524C4 File Offset: 0x000506C4
		public bool IsHeld { get; protected set; }

		// Token: 0x060012FA RID: 4858 RVA: 0x000524CD File Offset: 0x000506CD
		private void Awake()
		{
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Task"));
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x000524E4 File Offset: 0x000506E4
		public virtual void StartClick(RaycastHit hit)
		{
			if (this.onClickStart != null)
			{
				this.onClickStart.Invoke(hit);
			}
			this.IsHeld = true;
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00052501 File Offset: 0x00050701
		public virtual void EndClick()
		{
			if (this.onClickEnd != null)
			{
				this.onClickEnd.Invoke();
			}
			this.IsHeld = false;
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0005251D File Offset: 0x0005071D
		public void SetOriginalHitPoint(Vector3 hitPoint)
		{
			this.originalHitPoint = hitPoint;
		}

		// Token: 0x04001217 RID: 4631
		public bool ClickableEnabled = true;

		// Token: 0x04001218 RID: 4632
		public bool AutoCalculateOffset = true;

		// Token: 0x04001219 RID: 4633
		public bool FlattenZOffset;

		// Token: 0x0400121C RID: 4636
		public UnityEvent<RaycastHit> onClickStart;

		// Token: 0x0400121D RID: 4637
		public UnityEvent onClickEnd;
	}
}
