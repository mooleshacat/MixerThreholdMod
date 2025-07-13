using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EasyButtons;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.Intro
{
	// Token: 0x02000645 RID: 1605
	public class IntroManager : Singleton<IntroManager>
	{
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x060029A6 RID: 10662 RVA: 0x000AC26D File Offset: 0x000AA46D
		// (set) Token: 0x060029A7 RID: 10663 RVA: 0x000AC275 File Offset: 0x000AA475
		public bool IsPlaying { get; protected set; }

		// Token: 0x060029A8 RID: 10664 RVA: 0x000AC27E File Offset: 0x000AA47E
		protected override void Awake()
		{
			base.Awake();
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x000AC298 File Offset: 0x000AA498
		private void Update()
		{
			if (this.Anim.isPlaying)
			{
				if ((GameInput.GetButton(GameInput.ButtonCode.Jump) || GameInput.GetButton(GameInput.ButtonCode.Submit) || GameInput.GetButton(GameInput.ButtonCode.PrimaryClick)) && this.depressed)
				{
					this.currentSkipTime += Time.deltaTime;
					if (this.currentSkipTime >= 0.5f)
					{
						this.currentSkipTime = 0f;
						if (this.IsPlaying)
						{
							Debug.Log("Skipping!");
							int num = this.CurrentStep + 1;
							float time = this.Anim.clip.events[num].time;
							this.Anim[this.Anim.clip.name].time = time;
							this.CurrentStep = num;
							this.depressed = false;
						}
					}
					this.SkipDial.fillAmount = this.currentSkipTime / 0.5f;
					this.SkipContainer.SetActive(true);
					return;
				}
				this.currentSkipTime = 0f;
				this.SkipContainer.SetActive(false);
				if (!GameInput.GetButton(GameInput.ButtonCode.Jump) && !GameInput.GetButton(GameInput.ButtonCode.Submit) && !GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
				{
					this.depressed = true;
				}
			}
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x000AC3C4 File Offset: 0x000AA5C4
		[Button]
		public void Play()
		{
			this.IsPlaying = true;
			NetworkSingleton<TimeManager>.Instance.SetTimeOverridden(true, this.TimeOfDayOverride);
			Console.Log("Starting Intro...", null);
			this.Container.SetActive(true);
			this.rv.ModelContainer.gameObject.SetActive(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<HUD>.Instance.canvas.enabled = false;
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraContainer.position, this.CameraContainer.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.CameraContainer.transform.SetParent(this.CameraContainer);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			GameObject[] disableDuringIntro = this.DisableDuringIntro;
			for (int i = 0; i < disableDuringIntro.Length; i++)
			{
				disableDuringIntro[i].gameObject.SetActive(false);
			}
			base.StartCoroutine(this.<Play>g__Wait|23_0());
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x000AC4DA File Offset: 0x000AA6DA
		private void PlayMusic()
		{
			Singleton<MusicPlayer>.Instance.Tracks.Find((MusicTrack t) => t.TrackName == this.MusicName).GetComponent<MusicTrack>().Enable();
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x000AC504 File Offset: 0x000AA704
		public void CharacterCreationDone(BasicAvatarSettings avatar, List<ClothingInstance> clothes)
		{
			IntroManager.<>c__DisplayClass25_0 CS$<>8__locals1 = new IntroManager.<>c__DisplayClass25_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.clothes = clothes;
			base.StartCoroutine(CS$<>8__locals1.<CharacterCreationDone>g__Wait|0());
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x000AC532 File Offset: 0x000AA732
		public void PassedStep(int stepIndex)
		{
			this.CurrentStep = stepIndex;
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x000AC555 File Offset: 0x000AA755
		[CompilerGenerated]
		private IEnumerator <Play>g__Wait|23_0()
		{
			yield return new WaitUntil(() => Singleton<LoadManager>.Instance.IsGameLoaded);
			this.Anim.Play();
			this.PlayMusic();
			yield return new WaitForSeconds(0.1f);
			yield return new WaitUntil(() => !this.Anim.isPlaying);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, false, false);
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(2f);
			Singleton<CharacterCreator>.Instance.Open(Singleton<CharacterCreator>.Instance.DefaultSettings, true);
			Singleton<CharacterCreator>.Instance.onCompleteWithClothing.AddListener(new UnityAction<BasicAvatarSettings, List<ClothingInstance>>(this.CharacterCreationDone));
			yield return new WaitForSeconds(0.05f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.Container.gameObject.SetActive(false);
			this.rv.ModelContainer.gameObject.SetActive(true);
			PlayerSingleton<PlayerMovement>.Instance.Teleport(NetworkSingleton<GameManager>.Instance.SpawnPoint.position);
			base.transform.forward = NetworkSingleton<GameManager>.Instance.SpawnPoint.forward;
			GameObject[] disableDuringIntro = this.DisableDuringIntro;
			for (int i = 0; i < disableDuringIntro.Length; i++)
			{
				disableDuringIntro[i].gameObject.SetActive(true);
			}
			yield return new WaitForSeconds(1f);
			Singleton<BlackOverlay>.Instance.Close(1f);
			yield break;
		}

		// Token: 0x04001E2C RID: 7724
		public const float SKIP_TIME = 0.5f;

		// Token: 0x04001E2E RID: 7726
		public int CurrentStep;

		// Token: 0x04001E2F RID: 7727
		[Header("Settings")]
		public int TimeOfDayOverride = 2000;

		// Token: 0x04001E30 RID: 7728
		[Header("References")]
		public GameObject Container;

		// Token: 0x04001E31 RID: 7729
		public Transform PlayerInitialPosition;

		// Token: 0x04001E32 RID: 7730
		public Transform PlayerInitialPosition_AfterRVExplosion;

		// Token: 0x04001E33 RID: 7731
		public Transform CameraContainer;

		// Token: 0x04001E34 RID: 7732
		public Animation Anim;

		// Token: 0x04001E35 RID: 7733
		public GameObject SkipContainer;

		// Token: 0x04001E36 RID: 7734
		public Image SkipDial;

		// Token: 0x04001E37 RID: 7735
		public GameObject[] DisableDuringIntro;

		// Token: 0x04001E38 RID: 7736
		public RV rv;

		// Token: 0x04001E39 RID: 7737
		public UnityEvent onIntroDone;

		// Token: 0x04001E3A RID: 7738
		public UnityEvent onIntroDoneAsServer;

		// Token: 0x04001E3B RID: 7739
		public string MusicName;

		// Token: 0x04001E3C RID: 7740
		private float currentSkipTime;

		// Token: 0x04001E3D RID: 7741
		private bool depressed = true;
	}
}
