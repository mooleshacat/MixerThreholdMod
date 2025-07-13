using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScheduleOne
{
	// Token: 0x02000279 RID: 633
	public class GameInput : PersistentSingleton<GameInput>
	{
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000D3A RID: 3386 RVA: 0x0003A714 File Offset: 0x00038914
		public static Vector2 MouseDelta
		{
			get
			{
				return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x0003A72F File Offset: 0x0003892F
		public static Vector3 MousePosition
		{
			get
			{
				return Input.mousePosition;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x0003A736 File Offset: 0x00038936
		public static float MouseScrollDelta
		{
			get
			{
				return Input.GetAxis("Mouse ScrollWheel");
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0003A742 File Offset: 0x00038942
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0003A74C File Offset: 0x0003894C
		protected override void Start()
		{
			base.Start();
			if (Singleton<GameInput>.Instance == null || Singleton<GameInput>.Instance != this)
			{
				return;
			}
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(delegate()
			{
				GameInput.exitListeners.Clear();
			});
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0003A7A8 File Offset: 0x000389A8
		private void OnApplicationFocus(bool focus)
		{
			if (!focus)
			{
				foreach (GameInput.ButtonCode item in this.buttonsDown)
				{
					this.buttonsUpThisFrame.Add(item);
				}
				this.buttonsDown.Clear();
			}
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0003A810 File Offset: 0x00038A10
		public static bool GetButton(GameInput.ButtonCode buttonCode)
		{
			return Singleton<GameInput>.Instance.buttonsDown.Contains(buttonCode);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0003A822 File Offset: 0x00038A22
		public static bool GetButtonDown(GameInput.ButtonCode buttonCode)
		{
			return Singleton<GameInput>.Instance.buttonsDownThisFrame.Contains(buttonCode);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0003A834 File Offset: 0x00038A34
		public static bool GetButtonUp(GameInput.ButtonCode buttonCode)
		{
			return Singleton<GameInput>.Instance.buttonsUpThisFrame.Contains(buttonCode);
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0003A848 File Offset: 0x00038A48
		protected virtual void Update()
		{
			if (!Singleton<GameInput>.InstanceExists)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Escape) || GameInput.GetButtonDown(GameInput.ButtonCode.Back))
			{
				this.Exit(GameInput.GetButtonDown(GameInput.ButtonCode.Escape) ? ExitType.Escape : ExitType.RightClick);
			}
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && !Input.GetMouseButton(0))
			{
				Console.LogWarning("Mouse button (0) sticking detected!", null);
				this.OnPrimaryClick();
			}
			if (GameInput.GetButton(GameInput.ButtonCode.SecondaryClick) && !Input.GetMouseButton(1))
			{
				Console.LogWarning("Mouse button (1) sticking detected!", null);
				this.OnSecondaryClick();
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0003A8C4 File Offset: 0x00038AC4
		private void Exit(ExitType type)
		{
			ExitAction exitAction = new ExitAction();
			exitAction.exitType = type;
			for (int i = 0; i < GameInput.exitListeners.Count; i++)
			{
				bool used = exitAction.Used;
				GameInput.exitListeners[GameInput.exitListeners.Count - (1 + i)].listenerFunction(exitAction);
				if (exitAction.Used)
				{
				}
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0003A927 File Offset: 0x00038B27
		private void LateUpdate()
		{
			this.buttonsDownThisFrame.Clear();
			this.buttonsUpThisFrame.Clear();
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0003A940 File Offset: 0x00038B40
		public void ExitAll()
		{
			int num = 20;
			while (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount > 0)
			{
				num--;
				if (num <= 0)
				{
					Console.LogError("Failed to exit from all active UI elements.", null);
					for (int i = 0; i < PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount; i++)
					{
						Debug.LogError(PlayerSingleton<PlayerCamera>.Instance.activeUIElements[i]);
					}
					return;
				}
				this.Exit(ExitType.Escape);
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0003A9A4 File Offset: 0x00038BA4
		private void OnMotion(InputValue value)
		{
			GameInput.MotionAxis = value.Get<Vector2>();
			if (GameInput.MotionAxis.x > 0f)
			{
				if (!this.buttonsDown.Contains(GameInput.ButtonCode.Right))
				{
					this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Right);
					this.buttonsDown.Add(GameInput.ButtonCode.Right);
				}
			}
			else if (this.buttonsDown.Contains(GameInput.ButtonCode.Right))
			{
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Right);
				this.buttonsDown.Remove(GameInput.ButtonCode.Right);
			}
			if (GameInput.MotionAxis.x < 0f)
			{
				if (!this.buttonsDown.Contains(GameInput.ButtonCode.Left))
				{
					this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Left);
					this.buttonsDown.Add(GameInput.ButtonCode.Left);
				}
			}
			else if (this.buttonsDown.Contains(GameInput.ButtonCode.Left))
			{
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Left);
				this.buttonsDown.Remove(GameInput.ButtonCode.Left);
			}
			if (GameInput.MotionAxis.y > 0f)
			{
				if (!this.buttonsDown.Contains(GameInput.ButtonCode.Forward))
				{
					this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Forward);
					this.buttonsDown.Add(GameInput.ButtonCode.Forward);
				}
			}
			else if (this.buttonsDown.Contains(GameInput.ButtonCode.Forward))
			{
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Forward);
				this.buttonsDown.Remove(GameInput.ButtonCode.Forward);
			}
			if (GameInput.MotionAxis.y < 0f)
			{
				if (!this.buttonsDown.Contains(GameInput.ButtonCode.Backward))
				{
					this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Backward);
					this.buttonsDown.Add(GameInput.ButtonCode.Backward);
					return;
				}
			}
			else if (this.buttonsDown.Contains(GameInput.ButtonCode.Backward))
			{
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Backward);
				this.buttonsDown.Remove(GameInput.ButtonCode.Backward);
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0003AB3C File Offset: 0x00038D3C
		private void OnPrimaryClick()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.PrimaryClick))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.PrimaryClick);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.PrimaryClick);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.PrimaryClick);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.PrimaryClick);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0003AB8C File Offset: 0x00038D8C
		private void OnSecondaryClick()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.SecondaryClick))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.SecondaryClick);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.SecondaryClick);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.SecondaryClick);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.SecondaryClick);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0003ABDC File Offset: 0x00038DDC
		private void OnTertiaryClick()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.TertiaryClick))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.TertiaryClick);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.TertiaryClick);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.TertiaryClick);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.TertiaryClick);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0003AC2C File Offset: 0x00038E2C
		private void OnJump()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.Jump))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.Jump);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Jump);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.Jump);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Jump);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0003AC7C File Offset: 0x00038E7C
		private void OnCrouch()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.Crouch))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.Crouch);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Crouch);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.Crouch);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Crouch);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0003ACCC File Offset: 0x00038ECC
		private void OnSprint()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.Sprint))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.Sprint);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Sprint);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.Sprint);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Sprint);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0003AD20 File Offset: 0x00038F20
		private void OnEscape()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.Escape))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.Escape);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Escape);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.Escape);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Escape);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0003AD74 File Offset: 0x00038F74
		private void OnBack()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.Back))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.Back);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Back);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.Back);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Back);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0003ADC8 File Offset: 0x00038FC8
		private void OnInteract()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.Interact))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.Interact);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Interact);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.Interact);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Interact);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0003AE1C File Offset: 0x0003901C
		private void OnSubmit()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.Submit))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.Submit);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Submit);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.Submit);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Submit);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0003AE70 File Offset: 0x00039070
		private void OnTogglePhone()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.TogglePhone))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.TogglePhone);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.TogglePhone);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.TogglePhone);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.TogglePhone);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0003AEC4 File Offset: 0x000390C4
		private void OnToggleLights()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.ToggleLights))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.ToggleLights);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.ToggleLights);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.ToggleLights);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.ToggleLights);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0003AF18 File Offset: 0x00039118
		private void OnHandbrake()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.Handbrake))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.Handbrake);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Handbrake);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.Handbrake);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Handbrake);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0003AF6C File Offset: 0x0003916C
		private void OnRotateLeft()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.RotateLeft))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.RotateLeft);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.RotateLeft);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.RotateLeft);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.RotateLeft);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0003AFC0 File Offset: 0x000391C0
		private void OnRotateRight()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.RotateRight))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.RotateRight);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.RotateRight);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.RotateRight);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.RotateRight);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0003B014 File Offset: 0x00039214
		private void OnManagementMode()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.ManagementMode))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.ManagementMode);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.ManagementMode);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.ManagementMode);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.ManagementMode);
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0003B068 File Offset: 0x00039268
		private void OnOpenMap()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.OpenMap))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.OpenMap);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.OpenMap);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.OpenMap);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.OpenMap);
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0003B0BC File Offset: 0x000392BC
		private void OnOpenJournal()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.OpenJournal))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.OpenJournal);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.OpenJournal);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.OpenJournal);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.OpenJournal);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0003B110 File Offset: 0x00039310
		private void OnOpenTexts()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.OpenTexts))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.OpenTexts);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.OpenTexts);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.OpenTexts);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.OpenTexts);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0003B164 File Offset: 0x00039364
		private void OnQuickMove()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.QuickMove))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.QuickMove);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.QuickMove);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.QuickMove);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.QuickMove);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0003B1B8 File Offset: 0x000393B8
		private void OnToggleFlashlight()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.ToggleFlashlight))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.ToggleFlashlight);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.ToggleFlashlight);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.ToggleFlashlight);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.ToggleFlashlight);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0003B20C File Offset: 0x0003940C
		private void OnViewAvatar()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.ViewAvatar))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.ViewAvatar);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.ViewAvatar);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.ViewAvatar);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.ViewAvatar);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0003B260 File Offset: 0x00039460
		private void OnReload()
		{
			if (this.buttonsDown.Contains(GameInput.ButtonCode.Reload))
			{
				this.buttonsDown.Remove(GameInput.ButtonCode.Reload);
				this.buttonsUpThisFrame.Add(GameInput.ButtonCode.Reload);
				return;
			}
			this.buttonsDown.Add(GameInput.ButtonCode.Reload);
			this.buttonsDownThisFrame.Add(GameInput.ButtonCode.Reload);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0003B2B4 File Offset: 0x000394B4
		public static void RegisterExitListener(GameInput.ExitDelegate listener, int priority = 0)
		{
			GameInput.ExitListener exitListener = new GameInput.ExitListener();
			exitListener.listenerFunction = listener;
			exitListener.priority = priority;
			for (int i = 0; i < GameInput.exitListeners.Count; i++)
			{
				if (priority <= GameInput.exitListeners[i].priority)
				{
					GameInput.exitListeners.Insert(i, exitListener);
					return;
				}
			}
			GameInput.exitListeners.Add(exitListener);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0003B318 File Offset: 0x00039518
		public static void DeregisterExitListener(GameInput.ExitDelegate listener)
		{
			for (int i = 0; i < GameInput.exitListeners.Count; i++)
			{
				if (GameInput.exitListeners[i].listenerFunction == listener)
				{
					GameInput.exitListeners.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0003B361 File Offset: 0x00039561
		public InputAction GetAction(GameInput.ButtonCode code)
		{
			return this.PlayerInput.currentActionMap.FindAction(code.ToString(), false);
		}

		// Token: 0x04000D8B RID: 3467
		public static List<GameInput.ExitListener> exitListeners = new List<GameInput.ExitListener>();

		// Token: 0x04000D8C RID: 3468
		public PlayerInput PlayerInput;

		// Token: 0x04000D8D RID: 3469
		public static bool IsTyping = false;

		// Token: 0x04000D8E RID: 3470
		public static Vector2 MotionAxis = Vector2.zero;

		// Token: 0x04000D8F RID: 3471
		private List<GameInput.ButtonCode> buttonsDownThisFrame = new List<GameInput.ButtonCode>();

		// Token: 0x04000D90 RID: 3472
		private List<GameInput.ButtonCode> buttonsDown = new List<GameInput.ButtonCode>();

		// Token: 0x04000D91 RID: 3473
		private List<GameInput.ButtonCode> buttonsUpThisFrame = new List<GameInput.ButtonCode>();

		// Token: 0x0200027A RID: 634
		public enum ButtonCode
		{
			// Token: 0x04000D93 RID: 3475
			PrimaryClick,
			// Token: 0x04000D94 RID: 3476
			SecondaryClick,
			// Token: 0x04000D95 RID: 3477
			TertiaryClick,
			// Token: 0x04000D96 RID: 3478
			Forward,
			// Token: 0x04000D97 RID: 3479
			Backward,
			// Token: 0x04000D98 RID: 3480
			Left,
			// Token: 0x04000D99 RID: 3481
			Right,
			// Token: 0x04000D9A RID: 3482
			Jump,
			// Token: 0x04000D9B RID: 3483
			Crouch,
			// Token: 0x04000D9C RID: 3484
			Sprint,
			// Token: 0x04000D9D RID: 3485
			Escape,
			// Token: 0x04000D9E RID: 3486
			Back,
			// Token: 0x04000D9F RID: 3487
			Interact,
			// Token: 0x04000DA0 RID: 3488
			Submit,
			// Token: 0x04000DA1 RID: 3489
			TogglePhone,
			// Token: 0x04000DA2 RID: 3490
			ToggleLights,
			// Token: 0x04000DA3 RID: 3491
			Handbrake,
			// Token: 0x04000DA4 RID: 3492
			RotateLeft,
			// Token: 0x04000DA5 RID: 3493
			RotateRight,
			// Token: 0x04000DA6 RID: 3494
			ManagementMode,
			// Token: 0x04000DA7 RID: 3495
			OpenMap,
			// Token: 0x04000DA8 RID: 3496
			OpenJournal,
			// Token: 0x04000DA9 RID: 3497
			OpenTexts,
			// Token: 0x04000DAA RID: 3498
			QuickMove,
			// Token: 0x04000DAB RID: 3499
			ToggleFlashlight,
			// Token: 0x04000DAC RID: 3500
			ViewAvatar,
			// Token: 0x04000DAD RID: 3501
			Reload
		}

		// Token: 0x0200027B RID: 635
		public class ExitListener
		{
			// Token: 0x04000DAE RID: 3502
			public GameInput.ExitDelegate listenerFunction;

			// Token: 0x04000DAF RID: 3503
			public int priority;
		}

		// Token: 0x0200027C RID: 636
		// (Invoke) Token: 0x06000D66 RID: 3430
		public delegate void ExitDelegate(ExitAction exitAction);
	}
}
