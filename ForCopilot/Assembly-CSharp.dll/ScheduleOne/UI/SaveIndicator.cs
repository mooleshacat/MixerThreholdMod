using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x02000A87 RID: 2695
	public class SaveIndicator : MonoBehaviour
	{
		// Token: 0x0600487E RID: 18558 RVA: 0x001304EC File Offset: 0x0012E6EC
		public void Awake()
		{
			this.Canvas.enabled = false;
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x001304FA File Offset: 0x0012E6FA
		public void Start()
		{
			Singleton<SaveManager>.Instance.onSaveStart.AddListener(new UnityAction(this.Display));
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x00130517 File Offset: 0x0012E717
		public void OnDestroy()
		{
			if (Singleton<SaveManager>.InstanceExists)
			{
				Singleton<SaveManager>.Instance.onSaveStart.RemoveListener(new UnityAction(this.Display));
			}
		}

		// Token: 0x06004881 RID: 18561 RVA: 0x0013053B File Offset: 0x0012E73B
		public void Display()
		{
			base.StartCoroutine(this.<Display>g__Routine|6_0());
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x0013054A File Offset: 0x0012E74A
		[CompilerGenerated]
		private IEnumerator <Display>g__Routine|6_0()
		{
			this.Canvas.enabled = true;
			this.Icon.gameObject.SetActive(true);
			while (Singleton<SaveManager>.Instance.IsSaving)
			{
				this.Icon.Rotate(Vector3.forward, 360f * Time.unscaledDeltaTime);
				yield return new WaitForEndOfFrame();
			}
			this.Icon.gameObject.SetActive(false);
			this.Anim.Play();
			yield return new WaitForSecondsRealtime(5f);
			this.Canvas.enabled = false;
			yield break;
		}

		// Token: 0x04003528 RID: 13608
		public Canvas Canvas;

		// Token: 0x04003529 RID: 13609
		public RectTransform Icon;

		// Token: 0x0400352A RID: 13610
		public Animation Anim;
	}
}
