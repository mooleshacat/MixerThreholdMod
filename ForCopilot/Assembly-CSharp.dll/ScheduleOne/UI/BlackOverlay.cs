using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009FC RID: 2556
	public class BlackOverlay : Singleton<BlackOverlay>
	{
		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x060044C5 RID: 17605 RVA: 0x00120914 File Offset: 0x0011EB14
		// (set) Token: 0x060044C6 RID: 17606 RVA: 0x0012091C File Offset: 0x0011EB1C
		public bool isShown { get; protected set; }

		// Token: 0x060044C7 RID: 17607 RVA: 0x00120925 File Offset: 0x0011EB25
		protected override void Awake()
		{
			base.Awake();
			this.isShown = false;
			this.canvas.enabled = false;
			this.group.alpha = 0f;
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x00120950 File Offset: 0x0011EB50
		public void Open(float fadeTime = 0.5f)
		{
			this.isShown = true;
			this.canvas.enabled = true;
			if (this.fadeRoutine != null)
			{
				base.StopCoroutine(this.fadeRoutine);
			}
			this.fadeRoutine = base.StartCoroutine(this.Fade(1f, fadeTime));
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x0012099C File Offset: 0x0011EB9C
		public void Close(float fadeTime = 0.5f)
		{
			this.isShown = false;
			if (this.fadeRoutine != null)
			{
				base.StopCoroutine(this.fadeRoutine);
			}
			this.fadeRoutine = base.StartCoroutine(this.Fade(0f, fadeTime));
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x001209D1 File Offset: 0x0011EBD1
		private IEnumerator Fade(float endOpacity, float fadeTime)
		{
			float start = this.group.alpha;
			for (float i = 0f; i < fadeTime; i += Time.deltaTime)
			{
				this.group.alpha = Mathf.Lerp(start, endOpacity, i / fadeTime);
				yield return new WaitForEndOfFrame();
			}
			this.group.alpha = endOpacity;
			if (endOpacity == 0f)
			{
				this.canvas.enabled = false;
			}
			this.fadeRoutine = null;
			yield break;
		}

		// Token: 0x04003199 RID: 12697
		[Header("References")]
		public Canvas canvas;

		// Token: 0x0400319A RID: 12698
		public CanvasGroup group;

		// Token: 0x0400319B RID: 12699
		private Coroutine fadeRoutine;
	}
}
