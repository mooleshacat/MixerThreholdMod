using System;
using System.Collections;
using System.Runtime.CompilerServices;
using EasyButtons;
using FishNet;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A17 RID: 2583
	public class DemoIntro : Singleton<DemoIntro>
	{
		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x00123C55 File Offset: 0x00121E55
		// (set) Token: 0x06004585 RID: 17797 RVA: 0x00123C5D File Offset: 0x00121E5D
		public bool IsPlaying { get; protected set; }

		// Token: 0x06004586 RID: 17798 RVA: 0x00123C68 File Offset: 0x00121E68
		private void Update()
		{
			if (this.waitingForCutsceneEnd && !this.Anim.isPlaying)
			{
				this.CutsceneDone();
			}
			if (this.Anim.isPlaying)
			{
				if ((GameInput.GetButton(GameInput.ButtonCode.Jump) || GameInput.GetButton(GameInput.ButtonCode.Submit) || GameInput.GetButton(GameInput.ButtonCode.PrimaryClick)) && this.depressed && this.CurrentStep < this.SkipEvents - 1)
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

		// Token: 0x06004587 RID: 17799 RVA: 0x00123DC4 File Offset: 0x00121FC4
		[Button]
		public void Play()
		{
			this.IsPlaying = true;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<HUD>.Instance.canvas.enabled = false;
			this.Anim.Play();
			base.Invoke("PlayMusic", 1f);
			if (this.onStart != null)
			{
				this.onStart.Invoke();
			}
			this.waitingForCutsceneEnd = true;
			if (InstanceFinder.IsServer && this.onStartAsServer != null)
			{
				this.onStartAsServer.Invoke();
			}
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x00123E69 File Offset: 0x00122069
		private void PlayMusic()
		{
			Singleton<MusicPlayer>.Instance.Tracks.Find((MusicTrack t) => t.TrackName == this.MusicName).GetComponent<AmbientTrack>().ForcePlay();
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x00123E90 File Offset: 0x00122090
		public void ShowAvatar()
		{
			Singleton<CharacterCreator>.Instance.Open(Singleton<CharacterCreator>.Instance.DefaultSettings, false);
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x00123EA8 File Offset: 0x001220A8
		public void CutsceneDone()
		{
			this.waitingForCutsceneEnd = false;
			Singleton<CharacterCreator>.Instance.ShowUI();
			Singleton<CharacterCreator>.Instance.onComplete.AddListener(new UnityAction<BasicAvatarSettings>(this.CharacterCreationDone));
			if (this.onCutsceneDone != null)
			{
				this.onCutsceneDone.Invoke();
			}
			this.IsPlaying = false;
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x00123EFB File Offset: 0x001220FB
		public void PassedStep(int stepIndex)
		{
			this.CurrentStep = stepIndex;
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x00123F04 File Offset: 0x00122104
		public void CharacterCreationDone(BasicAvatarSettings avatar)
		{
			base.StartCoroutine(this.<CharacterCreationDone>g__Wait|26_0());
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x00123F3C File Offset: 0x0012213C
		[CompilerGenerated]
		private IEnumerator <CharacterCreationDone>g__Wait|26_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.5f);
			Player.Local.transform.position = this.PlayerInitialPosition.position;
			Player.Local.transform.rotation = this.PlayerInitialPosition.rotation;
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, false, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
			Singleton<CharacterCreator>.Instance.DisableStuff();
			yield return new WaitForSeconds(0.5f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<HUD>.Instance.canvas.enabled = true;
			Singleton<BlackOverlay>.Instance.Close(1f);
			if (this.onIntroDone != null)
			{
				this.onIntroDone.Invoke();
			}
			if (InstanceFinder.IsServer)
			{
				if (this.onIntroDoneAsServer != null)
				{
					this.onIntroDoneAsServer.Invoke();
				}
				Singleton<SaveManager>.Instance.Save();
			}
			else
			{
				Player.Local.RequestSavePlayer();
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0400323D RID: 12861
		public const float SKIP_TIME = 0.5f;

		// Token: 0x0400323F RID: 12863
		public Animation Anim;

		// Token: 0x04003240 RID: 12864
		public Transform PlayerInitialPosition;

		// Token: 0x04003241 RID: 12865
		public GameObject SkipContainer;

		// Token: 0x04003242 RID: 12866
		public Image SkipDial;

		// Token: 0x04003243 RID: 12867
		public int SkipEvents = 3;

		// Token: 0x04003244 RID: 12868
		public UnityEvent onStart;

		// Token: 0x04003245 RID: 12869
		public UnityEvent onStartAsServer;

		// Token: 0x04003246 RID: 12870
		public UnityEvent onCutsceneDone;

		// Token: 0x04003247 RID: 12871
		public UnityEvent onIntroDone;

		// Token: 0x04003248 RID: 12872
		public UnityEvent onIntroDoneAsServer;

		// Token: 0x04003249 RID: 12873
		private int CurrentStep;

		// Token: 0x0400324A RID: 12874
		public string MusicName;

		// Token: 0x0400324B RID: 12875
		private float currentSkipTime;

		// Token: 0x0400324C RID: 12876
		private bool depressed = true;

		// Token: 0x0400324D RID: 12877
		private bool waitingForCutsceneEnd;
	}
}
