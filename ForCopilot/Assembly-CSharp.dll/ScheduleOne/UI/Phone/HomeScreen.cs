using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000AE6 RID: 2790
	public class HomeScreen : PlayerSingleton<HomeScreen>
	{
		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06004AD2 RID: 19154 RVA: 0x0013A5AE File Offset: 0x001387AE
		// (set) Token: 0x06004AD3 RID: 19155 RVA: 0x0013A5B6 File Offset: 0x001387B6
		public bool isOpen { get; protected set; } = true;

		// Token: 0x06004AD4 RID: 19156 RVA: 0x0013A5BF File Offset: 0x001387BF
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(true);
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x0013A5D0 File Offset: 0x001387D0
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			if (!IsOwner)
			{
				return;
			}
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			Phone instance2 = PlayerSingleton<Phone>.Instance;
			instance2.onPhoneOpened = (Action)Delegate.Combine(instance2.onPhoneOpened, new Action(this.PhoneOpened));
			Phone instance3 = PlayerSingleton<Phone>.Instance;
			instance3.onPhoneClosed = (Action)Delegate.Combine(instance3.onPhoneClosed, new Action(this.PhoneClosed));
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x0013A65B File Offset: 0x0013885B
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x0013A691 File Offset: 0x00138891
		protected void PhoneOpened()
		{
			if (this.isOpen)
			{
				this.SetCanvasActive(true);
			}
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x0013A6A2 File Offset: 0x001388A2
		protected void PhoneClosed()
		{
			this.delayedSetOpenRoutine = base.StartCoroutine(this.DelayedSetCanvasActive(false, 0.25f));
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x0013A6BC File Offset: 0x001388BC
		private IEnumerator DelayedSetCanvasActive(bool active, float delay)
		{
			yield return new WaitForSeconds(delay);
			this.delayedSetOpenRoutine = null;
			this.SetCanvasActive(active);
			yield break;
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x0013A6D9 File Offset: 0x001388D9
		public void SetIsOpen(bool o)
		{
			this.isOpen = o;
			this.SetCanvasActive(o);
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x0013A6E9 File Offset: 0x001388E9
		public void SetCanvasActive(bool a)
		{
			if (this.delayedSetOpenRoutine != null)
			{
				base.StopCoroutine(this.delayedSetOpenRoutine);
			}
			this.canvas.enabled = a;
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x0013A70C File Offset: 0x0013890C
		protected virtual void Update()
		{
			if (PlayerSingleton<Phone>.Instance.IsOpen && this.isOpen)
			{
				int num = -1;
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					num = 0;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					num = 1;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					num = 2;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					num = 3;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					num = 4;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha6))
				{
					num = 5;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha7))
				{
					num = 6;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha8))
				{
					num = 7;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha9))
				{
					num = 8;
				}
				if (num != -1 && this.appIcons.Count > num)
				{
					this.appIcons[num].onClick.Invoke();
				}
			}
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x0013A7D0 File Offset: 0x001389D0
		protected virtual void MinPass()
		{
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				int num = TimeManager.Get24HourTimeFromMinSum(Mathf.RoundToInt(Mathf.Round((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 60f) * 60f));
				this.timeText.text = TimeManager.Get12HourTime((float)num, true) + " " + NetworkSingleton<TimeManager>.Instance.CurrentDay.ToString();
				return;
			}
			this.timeText.text = TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, true) + " " + NetworkSingleton<TimeManager>.Instance.CurrentDay.ToString();
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x0013A884 File Offset: 0x00138A84
		public Button GenerateAppIcon<T>(App<T> prog) where T : PlayerSingleton<T>
		{
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.appIconPrefab, this.appIconContainer).GetComponent<RectTransform>();
			component.Find("Mask/Image").GetComponent<Image>().sprite = prog.AppIcon;
			component.Find("Label").GetComponent<Text>().text = prog.IconLabel;
			this.appIcons.Add(component.GetComponent<Button>());
			return component.GetComponent<Button>();
		}

		// Token: 0x04003732 RID: 14130
		[Header("References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x04003733 RID: 14131
		[SerializeField]
		protected Text timeText;

		// Token: 0x04003734 RID: 14132
		[SerializeField]
		protected RectTransform appIconContainer;

		// Token: 0x04003735 RID: 14133
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject appIconPrefab;

		// Token: 0x04003736 RID: 14134
		protected List<Button> appIcons = new List<Button>();

		// Token: 0x04003737 RID: 14135
		private Coroutine delayedSetOpenRoutine;
	}
}
