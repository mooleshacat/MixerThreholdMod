using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B57 RID: 2903
	public class WorldspaceUIElement : MonoBehaviour
	{
		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06004D4F RID: 19791 RVA: 0x00145A5D File Offset: 0x00143C5D
		// (set) Token: 0x06004D50 RID: 19792 RVA: 0x00145A65 File Offset: 0x00143C65
		public bool IsEnabled { get; protected set; }

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06004D51 RID: 19793 RVA: 0x00145A6E File Offset: 0x00143C6E
		public bool IsVisible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x00145A7C File Offset: 0x00143C7C
		public virtual void Show()
		{
			if (this == null || this.Container == null)
			{
				return;
			}
			if (base.gameObject == null)
			{
				return;
			}
			this.IsEnabled = true;
			base.gameObject.SetActive(true);
			this.SetScale(1f, null);
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x00145AD0 File Offset: 0x00143CD0
		public virtual void Hide(Action callback = null)
		{
			if (this == null || this.Container == null)
			{
				return;
			}
			this.IsEnabled = false;
			this.SetScale(0f, delegate
			{
				base.<Hide>g__Done|1();
			});
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x001007D2 File Offset: 0x000FE9D2
		public virtual void Destroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x00145B28 File Offset: 0x00143D28
		public void UpdatePosition(Vector3 worldSpacePosition)
		{
			if (this == null || this.Container == null)
			{
				return;
			}
			if (PlayerSingleton<PlayerCamera>.Instance.transform.InverseTransformPoint(worldSpacePosition).z > 0f)
			{
				this.RectTransform.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(worldSpacePosition);
				this.Container.gameObject.SetActive(true);
				return;
			}
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x00145BA7 File Offset: 0x00143DA7
		public virtual void SetInternalScale(float scale)
		{
			if (this == null || this.Container == null)
			{
				return;
			}
			this.Container.localScale = new Vector3(scale, scale, 1f);
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x00145BD8 File Offset: 0x00143DD8
		private void SetScale(float scale, Action callback)
		{
			WorldspaceUIElement.<>c__DisplayClass17_0 CS$<>8__locals1 = new WorldspaceUIElement.<>c__DisplayClass17_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.scale = scale;
			CS$<>8__locals1.callback = callback;
			if (this == null || this.Container == null)
			{
				return;
			}
			if (this.scaleRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.scaleRoutine);
			}
			if (!base.gameObject.activeInHierarchy)
			{
				this.RectTransform.localScale = new Vector3(CS$<>8__locals1.scale, CS$<>8__locals1.scale, 1f);
				if (CS$<>8__locals1.callback != null)
				{
					CS$<>8__locals1.callback();
				}
				return;
			}
			CS$<>8__locals1.startScale = this.RectTransform.localScale.x;
			CS$<>8__locals1.lerpTime = 0.1f / Mathf.Abs(CS$<>8__locals1.startScale - CS$<>8__locals1.scale);
			this.scaleRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetScale>g__Routine|0());
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void HoverStart()
		{
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void HoverEnd()
		{
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x00145CBC File Offset: 0x00143EBC
		public void SetAssignedNPC(NPC npc)
		{
			if (this == null || this.Container == null)
			{
				return;
			}
			this.AssignedWorkerDisplay.Set(npc);
		}

		// Token: 0x0400399C RID: 14748
		public const float TRANSITION_TIME = 0.1f;

		// Token: 0x0400399E RID: 14750
		[Header("References")]
		public RectTransform RectTransform;

		// Token: 0x0400399F RID: 14751
		public RectTransform Container;

		// Token: 0x040039A0 RID: 14752
		public TextMeshProUGUI TitleLabel;

		// Token: 0x040039A1 RID: 14753
		public AssignedWorkerDisplay AssignedWorkerDisplay;

		// Token: 0x040039A2 RID: 14754
		private Coroutine scaleRoutine;
	}
}
