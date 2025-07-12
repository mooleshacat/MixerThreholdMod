using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A66 RID: 2662
	public class HospitalBillScreen : Singleton<HospitalBillScreen>
	{
		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x0600479D RID: 18333 RVA: 0x0012CF6A File Offset: 0x0012B16A
		// (set) Token: 0x0600479E RID: 18334 RVA: 0x0012CF72 File Offset: 0x0012B172
		public bool isOpen { get; protected set; }

		// Token: 0x0600479F RID: 18335 RVA: 0x0012CF7C File Offset: 0x0012B17C
		protected override void Awake()
		{
			base.Awake();
			this.isOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.CanvasGroup.alpha = 0f;
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 20);
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x0012CFF6 File Offset: 0x0012B1F6
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.Close();
			}
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x0012D020 File Offset: 0x0012B220
		private void PlayerSpawned()
		{
			this.PatientNameLabel.text = Player.Local.PlayerName;
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x0012D038 File Offset: 0x0012B238
		public void Open()
		{
			this.isOpen = true;
			this.arrested = Player.Local.IsArrested;
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			this.CanvasGroup.alpha = 1f;
			this.CanvasGroup.interactable = true;
			this.BillNumberLabel.text = UnityEngine.Random.Range(10000000, 100000000).ToString();
			float amount = Mathf.Min(250f, NetworkSingleton<MoneyManager>.Instance.cashBalance);
			this.PaidAmountLabel.text = MoneyManager.FormatAmount(amount, true, false);
			Singleton<PostProcessingManager>.Instance.SetBlur(1f);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			Player.Deactivate(false);
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x0012D104 File Offset: 0x0012B304
		public void Close()
		{
			if (!this.CanvasGroup.interactable || !this.isOpen)
			{
				return;
			}
			this.CanvasGroup.interactable = false;
			float num = Mathf.Min(250f, NetworkSingleton<MoneyManager>.Instance.cashBalance);
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-num, true, false);
			if (this.arrested)
			{
				this.CanvasGroup.alpha = 0f;
				this.Canvas.enabled = false;
				this.Container.gameObject.SetActive(false);
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				this.isOpen = false;
				Singleton<ArrestNoticeScreen>.Instance.Open();
				return;
			}
			base.StartCoroutine(this.<Close>g__CloseRoutine|16_0());
		}

		// Token: 0x060047A5 RID: 18341 RVA: 0x0012D1C2 File Offset: 0x0012B3C2
		[CompilerGenerated]
		private IEnumerator <Close>g__CloseRoutine|16_0()
		{
			float lerpTime = 0.3f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.CanvasGroup.alpha = Mathf.Lerp(1f, 0f, i / lerpTime);
				Singleton<PostProcessingManager>.Instance.SetBlur(this.CanvasGroup.alpha);
				yield return new WaitForEndOfFrame();
			}
			this.CanvasGroup.alpha = 0f;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			Singleton<PostProcessingManager>.Instance.SetBlur(0f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			Player.Activate();
			this.isOpen = false;
			yield break;
		}

		// Token: 0x0400346D RID: 13421
		public const float BILL_COST = 250f;

		// Token: 0x0400346F RID: 13423
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003470 RID: 13424
		public RectTransform Container;

		// Token: 0x04003471 RID: 13425
		public CanvasGroup CanvasGroup;

		// Token: 0x04003472 RID: 13426
		public TextMeshProUGUI PatientNameLabel;

		// Token: 0x04003473 RID: 13427
		public TextMeshProUGUI BillNumberLabel;

		// Token: 0x04003474 RID: 13428
		public TextMeshProUGUI PaidAmountLabel;

		// Token: 0x04003475 RID: 13429
		private bool arrested;
	}
}
