using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A5D RID: 2653
	public class ArrestScreen : Singleton<ArrestScreen>
	{
		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06004762 RID: 18274 RVA: 0x0012C3A8 File Offset: 0x0012A5A8
		// (set) Token: 0x06004763 RID: 18275 RVA: 0x0012C3B0 File Offset: 0x0012A5B0
		public bool isOpen { get; protected set; }

		// Token: 0x06004764 RID: 18276 RVA: 0x0012C3B9 File Offset: 0x0012A5B9
		protected override void Awake()
		{
			base.Awake();
			this.canvas.enabled = false;
			this.group.alpha = 0f;
			this.group.interactable = false;
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x0012C3E9 File Offset: 0x0012A5E9
		private void Continue()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.isOpen = false;
			base.StartCoroutine(this.<Continue>g__Routine|9_0());
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x0012C408 File Offset: 0x0012A608
		private void LoadSaveClicked()
		{
			this.Close();
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x0012C410 File Offset: 0x0012A610
		public void Open()
		{
			if (this.isOpen)
			{
				return;
			}
			this.isOpen = true;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.Sound.Play();
			base.StartCoroutine(this.<Open>g__Routine|11_0());
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x0012C44A File Offset: 0x0012A64A
		public void Close()
		{
			this.isOpen = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			Singleton<PostProcessingManager>.Instance.SetBlur(0f);
			this.canvas.enabled = false;
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x0012C486 File Offset: 0x0012A686
		[CompilerGenerated]
		private IEnumerator <Continue>g__Routine|9_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.5f);
			this.Close();
			Singleton<ArrestNoticeScreen>.Instance.Open();
			Player.Local.Free();
			Player.Local.Health.SetHealth(100f);
			yield return new WaitForSeconds(2f);
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			yield break;
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x0012C495 File Offset: 0x0012A695
		[CompilerGenerated]
		private IEnumerator <Open>g__Routine|11_0()
		{
			yield return new WaitForSeconds(0.5f);
			this.Anim.Play();
			this.canvas.enabled = true;
			float lerpTime = 0.75f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				Singleton<PostProcessingManager>.Instance.SetBlur(i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			Singleton<PostProcessingManager>.Instance.SetBlur(1f);
			yield return new WaitForSeconds(3f);
			this.Continue();
			yield break;
		}

		// Token: 0x04003436 RID: 13366
		[Header("References")]
		public Canvas canvas;

		// Token: 0x04003437 RID: 13367
		public CanvasGroup group;

		// Token: 0x04003438 RID: 13368
		public AudioSourceController Sound;

		// Token: 0x04003439 RID: 13369
		public Animation Anim;
	}
}
