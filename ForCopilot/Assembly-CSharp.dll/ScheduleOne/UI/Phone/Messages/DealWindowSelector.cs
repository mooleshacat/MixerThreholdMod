using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.Messaging;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000B03 RID: 2819
	public class DealWindowSelector : MonoBehaviour
	{
		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06004BA4 RID: 19364 RVA: 0x0013DBF2 File Offset: 0x0013BDF2
		// (set) Token: 0x06004BA5 RID: 19365 RVA: 0x0013DBFA File Offset: 0x0013BDFA
		public bool IsOpen { get; private set; }

		// Token: 0x06004BA6 RID: 19366 RVA: 0x0013DC04 File Offset: 0x0013BE04
		private void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			this.buttons = new WindowSelectorButton[]
			{
				this.MorningButton,
				this.AfternoonButton,
				this.NightButton,
				this.LateNightButton
			};
			WindowSelectorButton[] array = this.buttons;
			for (int i = 0; i < array.Length; i++)
			{
				WindowSelectorButton button = array[i];
				button.OnSelected.AddListener(delegate()
				{
					this.ButtonClicked(button.WindowType);
				});
			}
			this.SetIsOpen(false);
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x0013DCA1 File Offset: 0x0013BEA1
		public void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			action.Used = true;
			this.SetIsOpen(false);
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x0013DCC3 File Offset: 0x0013BEC3
		public void SetIsOpen(bool open)
		{
			this.SetIsOpen(open, null, null);
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x0013DCD0 File Offset: 0x0013BED0
		public void SetIsOpen(bool open, MSGConversation conversation, Action<EDealWindow> callback = null)
		{
			this.IsOpen = open;
			if (open)
			{
				this.UpdateTime();
				this.UpdateWindowValidity();
				conversation.onMessageRendered = (Action)Delegate.Combine(conversation.onMessageRendered, new Action(this.Close));
			}
			else
			{
				callback = null;
				if (conversation != null)
				{
					conversation.onMessageRendered = (Action)Delegate.Remove(conversation.onMessageRendered, new Action(this.Close));
				}
				WindowSelectorButton[] array = this.buttons;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetHoverIndicator(false);
				}
			}
			if (open && NetworkSingleton<GameManager>.Instance.IsTutorial && !this.hintShown)
			{
				this.hintShown = true;
				Singleton<HintDisplay>.Instance.ShowHint_20s("You can complete deals any time within the window you choose. For now, choose the morning window.");
			}
			this.Container.gameObject.SetActive(open);
			this.callback = callback;
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x0013DDA2 File Offset: 0x0013BFA2
		public void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.UpdateTime();
			this.UpdateWindowValidity();
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x0013DDBC File Offset: 0x0013BFBC
		private void UpdateTime()
		{
			this.CurrentTimeLabel.text = TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
			float t = (float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f;
			this.CurrentTimeArm.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, -360f, t));
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x0013DE24 File Offset: 0x0013C024
		private void UpdateWindowValidity()
		{
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				this.MorningButton.SetInteractable(true);
				this.AfternoonButton.SetInteractable(false);
				this.NightButton.SetInteractable(false);
				this.LateNightButton.SetInteractable(false);
				return;
			}
			int dailyMinTotal = NetworkSingleton<TimeManager>.Instance.DailyMinTotal;
			foreach (WindowSelectorButton windowSelectorButton in this.buttons)
			{
				int num = TimeManager.GetMinSumFrom24HourTime(DealWindowInfo.GetWindowInfo(windowSelectorButton.WindowType).EndTime);
				if (dailyMinTotal > num)
				{
					num += 1440;
				}
				windowSelectorButton.SetInteractable(num - dailyMinTotal > 120);
			}
		}

		// Token: 0x06004BAD RID: 19373 RVA: 0x0013DEBF File Offset: 0x0013C0BF
		private void Close()
		{
			this.SetIsOpen(false);
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x0013DEC8 File Offset: 0x0013C0C8
		private void ButtonClicked(EDealWindow window)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (this.OnSelected != null)
			{
				this.OnSelected.Invoke(window);
			}
			if (this.callback != null)
			{
				this.callback(window);
			}
			this.SetIsOpen(false);
		}

		// Token: 0x040037FA RID: 14330
		public const float TIME_ARM_ROTATION_0000 = 0f;

		// Token: 0x040037FB RID: 14331
		public const float TIME_ARM_ROTATION_2400 = -360f;

		// Token: 0x040037FC RID: 14332
		public const int WINDOW_CUTOFF_MINS = 120;

		// Token: 0x040037FD RID: 14333
		public UnityEvent<EDealWindow> OnSelected;

		// Token: 0x040037FF RID: 14335
		[Header("References")]
		public GameObject Container;

		// Token: 0x04003800 RID: 14336
		public WindowSelectorButton MorningButton;

		// Token: 0x04003801 RID: 14337
		public WindowSelectorButton AfternoonButton;

		// Token: 0x04003802 RID: 14338
		public WindowSelectorButton NightButton;

		// Token: 0x04003803 RID: 14339
		public WindowSelectorButton LateNightButton;

		// Token: 0x04003804 RID: 14340
		public RectTransform CurrentTimeArm;

		// Token: 0x04003805 RID: 14341
		public Text CurrentTimeLabel;

		// Token: 0x04003806 RID: 14342
		private Action<EDealWindow> callback;

		// Token: 0x04003807 RID: 14343
		private WindowSelectorButton[] buttons;

		// Token: 0x04003808 RID: 14344
		private bool hintShown;
	}
}
