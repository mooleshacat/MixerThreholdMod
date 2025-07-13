using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x020008C8 RID: 2248
	public class FunctionalBaggie : FunctionalPackaging
	{
		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06003C9A RID: 15514 RVA: 0x000FF34D File Offset: 0x000FD54D
		// (set) Token: 0x06003C9B RID: 15515 RVA: 0x000FF355 File Offset: 0x000FD555
		public override CursorManager.ECursorType HoveredCursor { get; protected set; } = CursorManager.ECursorType.Finger;

		// Token: 0x06003C9C RID: 15516 RVA: 0x000FF360 File Offset: 0x000FD560
		public void SetClosed(float closedDelta)
		{
			this.ClosedDelta = closedDelta;
			SkinnedMeshRenderer[] bagMeshes = this.BagMeshes;
			for (int i = 0; i < bagMeshes.Length; i++)
			{
				bagMeshes[i].SetBlendShapeWeight(0, closedDelta * 100f);
			}
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x000FF399 File Offset: 0x000FD599
		public override void StartClick(RaycastHit hit)
		{
			if (base.IsFull && this.ClosedDelta == 0f)
			{
				this.ClickableEnabled = false;
				if (!base.IsSealed)
				{
					this.Seal();
				}
			}
			base.StartClick(hit);
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x000FF3CC File Offset: 0x000FD5CC
		public override void Seal()
		{
			base.Seal();
			this.FunnelCollidersContainer.gameObject.SetActive(false);
			this.DynamicCollider.enabled = true;
			base.StartCoroutine(this.<Seal>g__Routine|11_0());
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x000FF3FE File Offset: 0x000FD5FE
		protected override void FullyPacked()
		{
			base.FullyPacked();
			this.FullyPackedBlocker.SetActive(true);
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x000FF421 File Offset: 0x000FD621
		[CompilerGenerated]
		private IEnumerator <Seal>g__Routine|11_0()
		{
			float lerpTime = 0.25f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.SetClosed(i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.SetClosed(1f);
			yield break;
		}

		// Token: 0x04002B66 RID: 11110
		public SkinnedMeshRenderer[] BagMeshes;

		// Token: 0x04002B67 RID: 11111
		public GameObject FunnelCollidersContainer;

		// Token: 0x04002B68 RID: 11112
		public GameObject FullyPackedBlocker;

		// Token: 0x04002B69 RID: 11113
		public Collider DynamicCollider;

		// Token: 0x04002B6B RID: 11115
		private float ClosedDelta;
	}
}
