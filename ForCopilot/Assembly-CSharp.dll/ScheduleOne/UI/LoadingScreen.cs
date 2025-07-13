using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using ScheduleOne.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A41 RID: 2625
	public class LoadingScreen : PersistentSingleton<LoadingScreen>
	{
		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06004694 RID: 18068 RVA: 0x00128290 File Offset: 0x00126490
		// (set) Token: 0x06004695 RID: 18069 RVA: 0x00128298 File Offset: 0x00126498
		public bool IsOpen { get; protected set; }

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06004696 RID: 18070 RVA: 0x001282A1 File Offset: 0x001264A1
		public Sprite[] ContextualBackgroundImages
		{
			get
			{
				if (!this.isLoadingTutorial)
				{
					return this.BackgroundImages;
				}
				return this.TutorialBackgroundImages;
			}
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x001282B8 File Offset: 0x001264B8
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<LoadingScreen>.Instance == null || Singleton<LoadingScreen>.Instance != this)
			{
				return;
			}
			this.loadingMessages = this.LoadingMessagesDatabase.Strings;
			this.currentBackgroundImageIndex = UnityEngine.Random.Range(0, this.ContextualBackgroundImages.Length);
			for (int i = 0; i < this.ContextualBackgroundImages.Length; i++)
			{
				int num = UnityEngine.Random.Range(0, this.ContextualBackgroundImages.Length);
				Sprite sprite = this.ContextualBackgroundImages[i];
				this.ContextualBackgroundImages[i] = this.ContextualBackgroundImages[num];
				this.ContextualBackgroundImages[num] = sprite;
			}
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Group.alpha = 0f;
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x00128372 File Offset: 0x00126572
		protected void Update()
		{
			if (this.IsOpen)
			{
				this.LoadStatusLabel.text = Singleton<LoadManager>.Instance.GetLoadStatusText();
			}
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x00128394 File Offset: 0x00126594
		public void Open(bool loadingTutorial = false)
		{
			if (this.IsOpen)
			{
				return;
			}
			this.isLoadingTutorial = loadingTutorial;
			this.TutorialContainer.gameObject.SetActive(loadingTutorial);
			if (loadingTutorial && Singleton<Lobby>.Instance.IsInLobby && Singleton<Lobby>.Instance.PlayerCount > 1)
			{
				this.CoopTutorialHint.gameObject.SetActive(true);
			}
			else
			{
				this.CoopTutorialHint.gameObject.SetActive(false);
			}
			this.LoadingMessageLabel.text = this.loadingMessages[UnityEngine.Random.Range(0, this.loadingMessages.Length)];
			this.IsOpen = true;
			Singleton<MusicPlayer>.Instance.SetTrackEnabled("Loading Screen", true);
			this.Fade(1f);
			this.AnimateBackground();
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x0012844A File Offset: 0x0012664A
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			Singleton<MusicPlayer>.Instance.SetTrackEnabled("Loading Screen", false);
			Singleton<MusicPlayer>.Instance.StopTrack("Loading Screen");
			this.Fade(0f);
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x00128486 File Offset: 0x00126686
		private void AnimateBackground()
		{
			if (this.animateBackgroundRoutine != null)
			{
				base.StopCoroutine(this.animateBackgroundRoutine);
			}
			if (this.scaleBackgroundRoutine != null)
			{
				base.StopCoroutine(this.scaleBackgroundRoutine);
			}
			this.animateBackgroundRoutine = base.StartCoroutine(this.<AnimateBackground>g__Routine|30_0());
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x001284C4 File Offset: 0x001266C4
		private void Fade(float endAlpha)
		{
			LoadingScreen.<>c__DisplayClass31_0 CS$<>8__locals1 = new LoadingScreen.<>c__DisplayClass31_0();
			CS$<>8__locals1.endAlpha = endAlpha;
			CS$<>8__locals1.<>4__this = this;
			if (this.fadeRoutine != null)
			{
				base.StopCoroutine(this.fadeRoutine);
			}
			this.fadeRoutine = base.StartCoroutine(CS$<>8__locals1.<Fade>g__Routine|0());
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x00128513 File Offset: 0x00126713
		[CompilerGenerated]
		private IEnumerator <AnimateBackground>g__Routine|30_0()
		{
			LoadingScreen.<>c__DisplayClass30_0 CS$<>8__locals1 = new LoadingScreen.<>c__DisplayClass30_0();
			this.currentBackgroundImageIndex++;
			this.BackgroundImage1.color = new Color(1f, 1f, 1f, 0f);
			this.BackgroundImage2.color = new Color(1f, 1f, 1f, 0f);
			Image prevImage = null;
			CS$<>8__locals1.nextImage = this.BackgroundImage1;
			while (this.IsOpen || this.Group.alpha > 0f)
			{
				this.currentBackgroundImageIndex %= this.ContextualBackgroundImages.Length;
				CS$<>8__locals1.nextImage.sprite = this.ContextualBackgroundImages[this.currentBackgroundImageIndex];
				this.scaleBackgroundRoutine = base.StartCoroutine(CS$<>8__locals1.<AnimateBackground>g__ScaleRoutine|1(CS$<>8__locals1.nextImage.transform, 10f));
				for (float i = 0f; i < 1f; i += Time.deltaTime)
				{
					if (prevImage != null)
					{
						prevImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, i / 1f));
					}
					CS$<>8__locals1.nextImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, i / 1f));
					yield return new WaitForEndOfFrame();
					if (prevImage != null)
					{
						prevImage.color = new Color(1f, 1f, 1f, 0f);
					}
					CS$<>8__locals1.nextImage.color = new Color(1f, 1f, 1f, 1f);
				}
				yield return new WaitForSeconds(8f);
				prevImage = CS$<>8__locals1.nextImage;
				CS$<>8__locals1.nextImage = ((CS$<>8__locals1.nextImage == this.BackgroundImage1) ? this.BackgroundImage2 : this.BackgroundImage1);
				this.currentBackgroundImageIndex++;
			}
			yield break;
		}

		// Token: 0x04003347 RID: 13127
		public const float FADE_TIME = 0.25f;

		// Token: 0x04003348 RID: 13128
		public const float BACKGROUND_IMAGE_TIME = 8f;

		// Token: 0x04003349 RID: 13129
		public const float BACKGROUND_IMAGE_FADE_TIME = 1f;

		// Token: 0x0400334B RID: 13131
		public StringDatabase LoadingMessagesDatabase;

		// Token: 0x0400334C RID: 13132
		public Sprite[] BackgroundImages;

		// Token: 0x0400334D RID: 13133
		public Sprite[] TutorialBackgroundImages;

		// Token: 0x0400334E RID: 13134
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400334F RID: 13135
		public CanvasGroup Group;

		// Token: 0x04003350 RID: 13136
		public TextMeshProUGUI LoadStatusLabel;

		// Token: 0x04003351 RID: 13137
		public TextMeshProUGUI LoadingMessageLabel;

		// Token: 0x04003352 RID: 13138
		public Image BackgroundImage1;

		// Token: 0x04003353 RID: 13139
		public Image BackgroundImage2;

		// Token: 0x04003354 RID: 13140
		public RectTransform TutorialContainer;

		// Token: 0x04003355 RID: 13141
		public RectTransform CoopTutorialHint;

		// Token: 0x04003356 RID: 13142
		private string[] loadingMessages;

		// Token: 0x04003357 RID: 13143
		private int currentBackgroundImageIndex;

		// Token: 0x04003358 RID: 13144
		private Coroutine fadeRoutine;

		// Token: 0x04003359 RID: 13145
		private Coroutine animateBackgroundRoutine;

		// Token: 0x0400335A RID: 13146
		private Coroutine scaleBackgroundRoutine;

		// Token: 0x0400335B RID: 13147
		private bool isLoadingTutorial;
	}
}
