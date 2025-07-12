using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B5E RID: 2910
	public class Disclaimer : MonoBehaviour
	{
		// Token: 0x06004D70 RID: 19824 RVA: 0x00146000 File Offset: 0x00144200
		private void Awake()
		{
			if (Application.isEditor || Disclaimer.Shown)
			{
				base.gameObject.SetActive(false);
				return;
			}
			Disclaimer.Shown = true;
			this.Group.alpha = 1f;
			this.TextGroup.alpha = 0f;
			this.Fade();
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x00146054 File Offset: 0x00144254
		private void Fade()
		{
			base.StartCoroutine(this.<Fade>g__Fade|5_0());
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x00146076 File Offset: 0x00144276
		[CompilerGenerated]
		private IEnumerator <Fade>g__Fade|5_0()
		{
			while (this.TextGroup.alpha < 1f)
			{
				this.TextGroup.alpha += Time.deltaTime * 2f;
				yield return null;
			}
			for (float i = 0f; i < this.Duration; i += Time.deltaTime)
			{
				if (Input.GetKey(KeyCode.Space))
				{
					IL_FC:
					while (this.Group.alpha > 0f)
					{
						this.Group.alpha -= Time.deltaTime * 2f;
						yield return null;
					}
					base.gameObject.SetActive(false);
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}
			goto IL_FC;
		}

		// Token: 0x040039B2 RID: 14770
		public static bool Shown;

		// Token: 0x040039B3 RID: 14771
		public CanvasGroup Group;

		// Token: 0x040039B4 RID: 14772
		public CanvasGroup TextGroup;

		// Token: 0x040039B5 RID: 14773
		public float Duration = 3.8f;
	}
}
