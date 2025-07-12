using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A6E RID: 2670
	public class SleepCanvas : Singleton<SleepCanvas>
	{
		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060047CE RID: 18382 RVA: 0x0012DE3B File Offset: 0x0012C03B
		// (set) Token: 0x060047CF RID: 18383 RVA: 0x0012DE43 File Offset: 0x0012C043
		public bool IsMenuOpen { get; protected set; }

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060047D0 RID: 18384 RVA: 0x0012DE4C File Offset: 0x0012C04C
		// (set) Token: 0x060047D1 RID: 18385 RVA: 0x0012DE54 File Offset: 0x0012C054
		public string QueuedSleepMessage { get; protected set; } = string.Empty;

		// Token: 0x060047D2 RID: 18386 RVA: 0x0012DE60 File Offset: 0x0012C060
		protected override void Awake()
		{
			base.Awake();
			this.IncreaseButton.onClick.AddListener(delegate()
			{
				this.ChangeSleepAmount(1);
			});
			this.DecreaseButton.onClick.AddListener(delegate()
			{
				this.ChangeSleepAmount(-1);
			});
			this.SleepButton.onClick.AddListener(new UnityAction(this.SleepButtonPressed));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
			TimeManager.onSleepStart = (Action)Delegate.Combine(TimeManager.onSleepStart, new Action(this.SleepStart));
			this.TimeLabel.enabled = false;
			this.WakeLabel.enabled = false;
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x0012DF11 File Offset: 0x0012C111
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (this.IsMenuOpen && action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.SetIsOpen(false);
			}
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x0012DF3C File Offset: 0x0012C13C
		public void SetIsOpen(bool open)
		{
			this.IsMenuOpen = open;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			if (open)
			{
				this.Update();
				NetworkSingleton<TimeManager>.Instance.SetWakeTime(this.ClampWakeTime(700));
				this.UpdateTimeLabels();
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				PlayerSingleton<PlayerMovement>.Instance.canMove = false;
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				this.Canvas.enabled = true;
				this.Container.gameObject.SetActive(true);
			}
			else
			{
				Player.Local.CurrentBed = null;
				Player.Local.SetReadyToSleep(false);
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, false);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			}
			this.MenuContainer.gameObject.SetActive(open);
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x0012E060 File Offset: 0x0012C260
		public void Update()
		{
			if (this.IsMenuOpen)
			{
				this.UpdateHourSetting();
				this.UpdateTimeLabels();
				this.UpdateSleepButton();
			}
			if (this.Canvas.enabled)
			{
				this.CurrentTimeLabel.text = TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
			}
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x0012E0B0 File Offset: 0x0012C2B0
		public void AddPostSleepEvent(IPostSleepEvent postSleepEvent)
		{
			Console.Log("Adding post sleep event: " + postSleepEvent.GetType().Name, null);
			this.queuedPostSleepEvents.Add(postSleepEvent);
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x0012E0D9 File Offset: 0x0012C2D9
		private void UpdateHourSetting()
		{
			this.IncreaseButton.interactable = true;
			this.DecreaseButton.interactable = true;
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x0012E0F3 File Offset: 0x0012C2F3
		private void UpdateTimeLabels()
		{
			this.EndTimeLabel.text = TimeManager.Get12HourTime(700f, true);
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x0012E10B File Offset: 0x0012C30B
		private void UpdateSleepButton()
		{
			if (Player.Local.IsReadyToSleep)
			{
				this.SleepButtonLabel.text = "Waiting for other players";
				return;
			}
			this.SleepButtonLabel.text = "Sleep";
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x0012E13C File Offset: 0x0012C33C
		private void ChangeSleepAmount(int change)
		{
			int num = TimeManager.AddMinutesTo24HourTime(700, change * 60);
			num = this.ClampWakeTime(num);
			NetworkSingleton<TimeManager>.Instance.SetWakeTime(num);
			this.UpdateHourSetting();
			this.UpdateTimeLabels();
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x0012E178 File Offset: 0x0012C378
		private int ClampWakeTime(int time)
		{
			int currentTime = NetworkSingleton<TimeManager>.Instance.CurrentTime;
			int time2 = TimeManager.AddMinutesTo24HourTime(currentTime, 60 - currentTime % 100);
			int startTime = TimeManager.AddMinutesTo24HourTime(time2, 240);
			int endTime = TimeManager.AddMinutesTo24HourTime(time2, 720);
			return this.ClampTime(time, startTime, endTime);
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x0012E1C0 File Offset: 0x0012C3C0
		private int ClampTime(int time, int startTime, int endTime)
		{
			if (endTime > startTime)
			{
				if (time < startTime)
				{
					return startTime;
				}
				if (time > endTime)
				{
					return endTime;
				}
			}
			else if (time < startTime && time > endTime)
			{
				int max = TimeManager.AddMinutesTo24HourTime(endTime, 720);
				if (TimeManager.IsGivenTimeWithinRange(time, endTime, max))
				{
					return endTime;
				}
				return startTime;
			}
			return time;
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x0012E200 File Offset: 0x0012C400
		private void SleepButtonPressed()
		{
			Player.Local.SetReadyToSleep(!Player.Local.IsReadyToSleep);
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x0012E21C File Offset: 0x0012C41C
		private void SleepStart()
		{
			Player.Local.SetReadyToSleep(false);
			this.MenuContainer.gameObject.SetActive(false);
			this.IsMenuOpen = false;
			int num = 700;
			this.WakeLabel.text = "Waking up at " + TimeManager.Get12HourTime((float)num, true);
			base.StartCoroutine(this.<SleepStart>g__Sleep|41_0());
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x0012E27C File Offset: 0x0012C47C
		private void LerpBlackOverlay(float transparency, float lerpTime)
		{
			SleepCanvas.<>c__DisplayClass42_0 CS$<>8__locals1 = new SleepCanvas.<>c__DisplayClass42_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.transparency = transparency;
			CS$<>8__locals1.lerpTime = lerpTime;
			if (CS$<>8__locals1.transparency > 0f)
			{
				this.BlackOverlay.enabled = true;
			}
			base.StartCoroutine(CS$<>8__locals1.<LerpBlackOverlay>g__Routine|0());
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x0012E2CC File Offset: 0x0012C4CC
		public void QueueSleepMessage(string message, float displayTime = 3f)
		{
			Console.Log(string.Concat(new string[]
			{
				"Queueing sleep message: ",
				message,
				" for ",
				displayTime.ToString(),
				" seconds"
			}), null);
			this.QueuedSleepMessage = message;
			this.QueuedMessageDisplayTime = displayTime;
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x0012E34E File Offset: 0x0012C54E
		[CompilerGenerated]
		private IEnumerator <SleepStart>g__Sleep|41_0()
		{
			this.BlackOverlay.enabled = true;
			this.SleepMessageLabel.text = string.Empty;
			if (InstanceFinder.IsServer)
			{
				Console.Log("Resetting host sleep done", null);
				NetworkSingleton<TimeManager>.Instance.ResetHostSleepDone();
			}
			Singleton<HUD>.Instance.canvas.enabled = false;
			this.LerpBlackOverlay(1f, 0.5f);
			yield return new WaitForSecondsRealtime(0.5f);
			if (this.onSleepFullyFaded != null)
			{
				this.onSleepFullyFaded.Invoke();
			}
			yield return new WaitForSecondsRealtime(0.5f);
			NetworkSingleton<DailySummary>.Instance.Open();
			yield return new WaitUntil(() => !NetworkSingleton<DailySummary>.Instance.IsOpen);
			this.queuedPostSleepEvents = (from x in this.queuedPostSleepEvents
			orderby x.Order
			select x).ToList<IPostSleepEvent>();
			using (List<IPostSleepEvent>.Enumerator enumerator = this.queuedPostSleepEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SleepCanvas.<>c__DisplayClass41_0 CS$<>8__locals1 = new SleepCanvas.<>c__DisplayClass41_0();
					CS$<>8__locals1.pse = enumerator.Current;
					yield return new WaitForSecondsRealtime(0.5f);
					Console.Log("Running post sleep event: " + CS$<>8__locals1.pse.GetType().Name, null);
					CS$<>8__locals1.pse.StartEvent();
					yield return new WaitUntil(() => !CS$<>8__locals1.pse.IsRunning);
					CS$<>8__locals1 = null;
				}
			}
			List<IPostSleepEvent>.Enumerator enumerator = default(List<IPostSleepEvent>.Enumerator);
			this.queuedPostSleepEvents.Clear();
			if (InstanceFinder.IsServer)
			{
				Console.Log("Marking host sleep done", null);
				NetworkSingleton<TimeManager>.Instance.MarkHostSleepDone();
			}
			else
			{
				this.WaitingForHostLabel.enabled = true;
				yield return new WaitUntil(() => NetworkSingleton<TimeManager>.Instance.HostDailySummaryDone);
				this.WaitingForHostLabel.enabled = false;
			}
			NetworkSingleton<TimeManager>.Instance.FastForwardToWakeTime();
			this.TimeLabel.enabled = true;
			if (InstanceFinder.IsServer)
			{
				Singleton<SaveManager>.Instance.DelayedSave();
			}
			yield return new WaitForSecondsRealtime(1f);
			this.TimeLabel.enabled = false;
			if (this.onSleepEndFade != null)
			{
				this.onSleepEndFade.Invoke();
			}
			if (!string.IsNullOrEmpty(this.QueuedSleepMessage))
			{
				yield return new WaitForSecondsRealtime(0.5f);
				this.SleepMessageLabel.text = this.QueuedSleepMessage;
				this.QueuedSleepMessage = string.Empty;
				this.SleepMessageGroup.alpha = 0f;
				float lerpTime = 0.5f;
				for (float i = 0f; i < lerpTime; i += Time.deltaTime)
				{
					this.SleepMessageGroup.alpha = i / lerpTime;
					yield return new WaitForEndOfFrame();
				}
				this.SleepMessageGroup.alpha = 1f;
				yield return new WaitForSecondsRealtime(this.QueuedMessageDisplayTime);
				for (float i = 0f; i < lerpTime; i += Time.deltaTime)
				{
					this.SleepMessageGroup.alpha = 1f - i / lerpTime;
					yield return new WaitForEndOfFrame();
				}
				this.SleepMessageGroup.alpha = 0f;
				yield return new WaitForSecondsRealtime(0.5f);
			}
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, false, true);
			this.TimeLabel.enabled = false;
			this.WakeLabel.enabled = false;
			if (!NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				Singleton<HUD>.Instance.canvas.enabled = true;
			}
			yield return new WaitForSecondsRealtime(0.1f);
			if (!NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				this.SetIsOpen(false);
			}
			this.LerpBlackOverlay(0f, 0.5f);
			yield break;
			yield break;
		}

		// Token: 0x0400349D RID: 13469
		public const int MaxSleepTime = 12;

		// Token: 0x0400349E RID: 13470
		public const int MinSleepTime = 4;

		// Token: 0x040034A1 RID: 13473
		private float QueuedMessageDisplayTime;

		// Token: 0x040034A2 RID: 13474
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040034A3 RID: 13475
		public RectTransform Container;

		// Token: 0x040034A4 RID: 13476
		public RectTransform MenuContainer;

		// Token: 0x040034A5 RID: 13477
		public TextMeshProUGUI CurrentTimeLabel;

		// Token: 0x040034A6 RID: 13478
		public Button IncreaseButton;

		// Token: 0x040034A7 RID: 13479
		public Button DecreaseButton;

		// Token: 0x040034A8 RID: 13480
		public TextMeshProUGUI EndTimeLabel;

		// Token: 0x040034A9 RID: 13481
		public Button SleepButton;

		// Token: 0x040034AA RID: 13482
		public TextMeshProUGUI SleepButtonLabel;

		// Token: 0x040034AB RID: 13483
		public Image BlackOverlay;

		// Token: 0x040034AC RID: 13484
		public TextMeshProUGUI SleepMessageLabel;

		// Token: 0x040034AD RID: 13485
		public CanvasGroup SleepMessageGroup;

		// Token: 0x040034AE RID: 13486
		public TextMeshProUGUI TimeLabel;

		// Token: 0x040034AF RID: 13487
		public TextMeshProUGUI WakeLabel;

		// Token: 0x040034B0 RID: 13488
		public TextMeshProUGUI WaitingForHostLabel;

		// Token: 0x040034B1 RID: 13489
		public UnityEvent onSleepFullyFaded;

		// Token: 0x040034B2 RID: 13490
		public UnityEvent onSleepEndFade;

		// Token: 0x040034B3 RID: 13491
		private List<IPostSleepEvent> queuedPostSleepEvents = new List<IPostSleepEvent>();
	}
}
