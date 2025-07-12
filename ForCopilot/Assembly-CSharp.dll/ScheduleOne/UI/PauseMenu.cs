using System;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.MainMenu;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x02000A51 RID: 2641
	public class PauseMenu : Singleton<PauseMenu>
	{
		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x060046EB RID: 18155 RVA: 0x00129992 File Offset: 0x00127B92
		// (set) Token: 0x060046EC RID: 18156 RVA: 0x0012999A File Offset: 0x00127B9A
		public bool IsPaused { get; protected set; }

		// Token: 0x060046ED RID: 18157 RVA: 0x001299A3 File Offset: 0x00127BA3
		protected override void Awake()
		{
			base.Awake();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), -100);
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x001299BE File Offset: 0x00127BBE
		protected override void Start()
		{
			base.Start();
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x001299E3 File Offset: 0x00127BE3
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (action.exitType == ExitType.RightClick)
			{
				return;
			}
			if (this.justResumed)
			{
				return;
			}
			if (GameInput.IsTyping)
			{
				return;
			}
			if (this.IsPaused)
			{
				this.Resume();
				return;
			}
			this.Pause();
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x00129A1D File Offset: 0x00127C1D
		private void Update()
		{
			bool instanceExists = PlayerSingleton<PlayerCamera>.InstanceExists;
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x00129A25 File Offset: 0x00127C25
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			this.noActiveUIElements = (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0);
			this.justPaused = false;
			this.justResumed = false;
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x00129A50 File Offset: 0x00127C50
		public void Pause()
		{
			Console.Log("Game paused", null);
			this.IsPaused = true;
			this.justPaused = true;
			if (this.FeedbackForm != null)
			{
				this.FeedbackForm.PrepScreenshot();
			}
			if (Singleton<Settings>.InstanceExists && Singleton<Settings>.Instance.PausingFreezesTime)
			{
				Time.timeScale = 0f;
			}
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				this.couldLook = PlayerSingleton<PlayerCamera>.Instance.canLook;
				this.lockedMouse = (Cursor.lockState == CursorLockMode.Locked);
				this.crosshairVisible = Singleton<HUD>.Instance.crosshair.gameObject.activeSelf;
				this.hudVisible = Singleton<HUD>.Instance.canvas.enabled;
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0.075f);
				Singleton<HUD>.Instance.canvas.enabled = false;
			}
			this.Screen.Open(false);
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x00129B64 File Offset: 0x00127D64
		public void Resume()
		{
			Console.Log("Game resumed", null);
			this.IsPaused = false;
			this.justResumed = true;
			if (Singleton<Settings>.InstanceExists && Singleton<Settings>.Instance.PausingFreezesTime)
			{
				if (NetworkSingleton<TimeManager>.Instance.SleepInProgress)
				{
					Time.timeScale = 1f;
				}
				else
				{
					Time.timeScale = 1f;
				}
			}
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				if (this.couldLook)
				{
					PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				}
				if (this.lockedMouse && (!Singleton<CharacterCreator>.InstanceExists || !Singleton<CharacterCreator>.Instance.IsOpen))
				{
					PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				}
				PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.075f);
			}
			if (Singleton<HUD>.InstanceExists)
			{
				Singleton<HUD>.Instance.SetCrosshairVisible(this.crosshairVisible);
				Singleton<HUD>.Instance.canvas.enabled = this.hudVisible;
			}
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.Screen.Close(false);
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x00129C63 File Offset: 0x00127E63
		public void StuckButtonClicked()
		{
			this.Resume();
			PlayerSingleton<PlayerMovement>.Instance.WarpToNavMesh();
		}

		// Token: 0x040033B2 RID: 13234
		public Canvas Canvas;

		// Token: 0x040033B3 RID: 13235
		public RectTransform Container;

		// Token: 0x040033B4 RID: 13236
		public MainMenuScreen Screen;

		// Token: 0x040033B5 RID: 13237
		public FeedbackForm FeedbackForm;

		// Token: 0x040033B6 RID: 13238
		private bool noActiveUIElements = true;

		// Token: 0x040033B7 RID: 13239
		private bool justPaused;

		// Token: 0x040033B8 RID: 13240
		private bool justResumed;

		// Token: 0x040033B9 RID: 13241
		private bool couldLook;

		// Token: 0x040033BA RID: 13242
		private bool lockedMouse;

		// Token: 0x040033BB RID: 13243
		private bool crosshairVisible;

		// Token: 0x040033BC RID: 13244
		private bool hudVisible;

		// Token: 0x040033BD RID: 13245
		public UnityEvent onPause;

		// Token: 0x040033BE RID: 13246
		public UnityEvent onResume;
	}
}
