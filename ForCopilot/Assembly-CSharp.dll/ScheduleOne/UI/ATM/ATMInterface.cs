using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.ATM
{
	// Token: 0x02000B94 RID: 2964
	public class ATMInterface : MonoBehaviour
	{
		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06004E85 RID: 20101 RVA: 0x0014BAA6 File Offset: 0x00149CA6
		// (set) Token: 0x06004E86 RID: 20102 RVA: 0x0014BAAE File Offset: 0x00149CAE
		public bool isOpen { get; protected set; }

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06004E87 RID: 20103 RVA: 0x0014BAB7 File Offset: 0x00149CB7
		private float relevantBalance
		{
			get
			{
				if (!this.depositing)
				{
					return NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance;
				}
				return NetworkSingleton<MoneyManager>.Instance.cashBalance;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06004E88 RID: 20104 RVA: 0x0014BAD6 File Offset: 0x00149CD6
		private static float remainingAllowedDeposit
		{
			get
			{
				return 10000f - ATM.WeeklyDepositSum;
			}
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x0014BAE4 File Offset: 0x00149CE4
		private void Awake()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x0014BB31 File Offset: 0x00149D31
		private void OnDestroy()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x0014BB54 File Offset: 0x00149D54
		protected virtual void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 2);
			this.activeScreen = this.menuScreen;
			this.canvas.enabled = false;
			for (int i = 0; i < this.amountButtons.Count; i++)
			{
				int fuckYou = i;
				this.amountButtons[i].onClick.AddListener(delegate()
				{
					this.AmountSelected(fuckYou);
				});
				if (i == this.amountButtons.Count - 1)
				{
					this.amountButtons[i].transform.Find("Text").GetComponent<Text>().text = "ALL ()";
				}
				else
				{
					this.amountButtons[i].transform.Find("Text").GetComponent<Text>().text = MoneyManager.FormatAmount((float)ATMInterface.amounts[i], false, false);
				}
			}
			this.depositLimitContainer.gameObject.SetActive(true);
		}

		// Token: 0x06004E8C RID: 20108 RVA: 0x0014BC5F File Offset: 0x00149E5F
		private void PlayerSpawned()
		{
			this.canvas.worldCamera = PlayerSingleton<PlayerCamera>.Instance.Camera;
		}

		// Token: 0x06004E8D RID: 20109 RVA: 0x0014BC78 File Offset: 0x00149E78
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				this.onlineBalanceText.text = MoneyManager.FormatAmount(NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance, false, false);
				this.cleanCashText.text = MoneyManager.FormatAmount(NetworkSingleton<MoneyManager>.Instance.cashBalance, false, false);
				this.depositLimitText.text = MoneyManager.FormatAmount(ATM.WeeklyDepositSum, false, false) + " / " + MoneyManager.FormatAmount(10000f, false, false);
				if (ATM.WeeklyDepositSum >= 10000f)
				{
					this.depositLimitText.color = new Color32(byte.MaxValue, 75, 75, byte.MaxValue);
				}
				else
				{
					this.depositLimitText.color = Color.white;
				}
				if (this.activeScreen == this.amountSelectorScreen)
				{
					if (this.depositing)
					{
						this.amountButtons[this.amountButtons.Count - 1].transform.Find("Text").GetComponent<Text>().text = "MAX (" + MoneyManager.FormatAmount(Mathf.Min(NetworkSingleton<MoneyManager>.Instance.cashBalance, ATMInterface.remainingAllowedDeposit), false, false) + ")";
					}
					this.UpdateAvailableAmounts();
					this.confirmAmountButton.interactable = (this.relevantBalance > 0f);
					if (this.depositing)
					{
						if (this.selectedAmountIndex == ATMInterface.amounts.Length)
						{
							this.confirmButtonText.text = "DEPOSIT ALL";
						}
						else
						{
							this.confirmButtonText.text = "DEPOSIT " + MoneyManager.FormatAmount(this.selectedAmount, false, false);
						}
					}
					else
					{
						this.confirmButtonText.text = "WITHDRAW " + MoneyManager.FormatAmount(this.selectedAmount, false, false);
					}
					if (this.relevantBalance < ATMInterface.GetAmountFromIndex(this.selectedAmountIndex, this.depositing))
					{
						this.DefaultAmountSelection();
					}
				}
				if (this.activeScreen == this.menuScreen)
				{
					this.menu_DepositButton.interactable = (ATM.WeeklyDepositSum < 10000f);
				}
				if (this.activeScreen == this.processingScreen)
				{
					this.processingScreenIndicator.localEulerAngles = new Vector3(0f, 0f, this.processingScreenIndicator.localEulerAngles.z - Time.deltaTime * 360f);
				}
			}
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x0014BED0 File Offset: 0x0014A0D0
		protected virtual void LateUpdate()
		{
			if (this.isOpen && this.activeScreen == this.amountSelectorScreen)
			{
				if (this.selectedAmountIndex == -1)
				{
					this.selectedButtonIndicator.gameObject.SetActive(false);
					return;
				}
				this.selectedButtonIndicator.anchoredPosition = this.amountButtons[this.selectedAmountIndex].GetComponent<RectTransform>().anchoredPosition;
				this.selectedButtonIndicator.gameObject.SetActive(true);
			}
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x0014BF4C File Offset: 0x0014A14C
		public virtual void SetIsOpen(bool o)
		{
			if (o == this.isOpen)
			{
				return;
			}
			this.isOpen = o;
			this.canvas.enabled = this.isOpen;
			EventSystem.current.SetSelectedGameObject(null);
			if (this.isOpen)
			{
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				Singleton<HUD>.Instance.SetCrosshairVisible(false);
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				this.SetActiveScreen(this.menuScreen);
				return;
			}
			this.atm.Exit();
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x0014BFF0 File Offset: 0x0014A1F0
		public virtual void Exit(ExitAction action)
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
				if (this.activeScreen == this.menuScreen || this.activeScreen == this.successScreen)
				{
					this.SetIsOpen(false);
					return;
				}
				if (this.activeScreen == this.amountSelectorScreen)
				{
					this.SetActiveScreen(this.menuScreen);
				}
			}
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x0014C06C File Offset: 0x0014A26C
		public void SetActiveScreen(RectTransform screen)
		{
			this.menuScreen.gameObject.SetActive(false);
			this.amountSelectorScreen.gameObject.SetActive(false);
			this.processingScreen.gameObject.SetActive(false);
			this.successScreen.gameObject.SetActive(false);
			this.activeScreen = screen;
			this.activeScreen.gameObject.SetActive(true);
			if (this.activeScreen == this.menuScreen)
			{
				this.menu_TitleText.text = "Hello, " + Player.Local.PlayerName;
				this.menu_DepositButton.Select();
				return;
			}
			if (this.activeScreen == this.amountSelectorScreen)
			{
				this.UpdateAvailableAmounts();
				this.DefaultAmountSelection();
				return;
			}
			if (this.activeScreen == this.successScreen)
			{
				this.doneButton.Select();
			}
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x0014C154 File Offset: 0x0014A354
		private void DefaultAmountSelection()
		{
			if (this.amountButtons[0].interactable)
			{
				this.amountButtons[0].Select();
				this.AmountSelected(0);
				return;
			}
			if (this.amountButtons[this.amountButtons.Count - 1].interactable && this.relevantBalance > 0f)
			{
				this.amountButtons[this.amountButtons.Count - 1].Select();
				this.AmountSelected(this.amountButtons.Count - 1);
				return;
			}
			this.AmountSelected(-1);
			for (int i = 0; i < this.amountButtons.Count; i++)
			{
			}
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x0014C207 File Offset: 0x0014A407
		public void DepositButtonPressed()
		{
			this.amountSelectorTitle.text = "Select amount to deposit";
			this.depositing = true;
			this.SetActiveScreen(this.amountSelectorScreen);
		}

		// Token: 0x06004E94 RID: 20116 RVA: 0x0014C22C File Offset: 0x0014A42C
		public void WithdrawButtonPressed()
		{
			this.amountSelectorTitle.text = "Select amount to withdraw";
			this.depositing = false;
			this.amountButtons[this.amountButtons.Count - 1].transform.Find("Text").GetComponent<Text>().text = MoneyManager.FormatAmount((float)ATMInterface.amounts[ATMInterface.amounts.Length - 1], false, false);
			this.SetActiveScreen(this.amountSelectorScreen);
		}

		// Token: 0x06004E95 RID: 20117 RVA: 0x0014C2A4 File Offset: 0x0014A4A4
		public void CancelAmountSelection()
		{
			this.SetActiveScreen(this.menuScreen);
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x0014C2B2 File Offset: 0x0014A4B2
		public void AmountSelected(int amountIndex)
		{
			this.selectedAmountIndex = amountIndex;
			this.SetSelectedAmount(ATMInterface.GetAmountFromIndex(amountIndex, this.depositing));
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x0014C2D0 File Offset: 0x0014A4D0
		private void SetSelectedAmount(float amount)
		{
			float max;
			if (this.depositing)
			{
				max = Mathf.Min(NetworkSingleton<MoneyManager>.Instance.cashBalance, ATMInterface.remainingAllowedDeposit);
			}
			else
			{
				max = NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance;
			}
			this.selectedAmount = Mathf.Clamp(amount, 0f, max);
			this.amountLabelText.text = MoneyManager.FormatAmount(this.selectedAmount, false, false);
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x0014C338 File Offset: 0x0014A538
		public static float GetAmountFromIndex(int index, bool depositing)
		{
			if (index == -1 || index >= ATMInterface.amounts.Length)
			{
				return 0f;
			}
			if (depositing && index == ATMInterface.amounts.Length - 1)
			{
				return Mathf.Min(NetworkSingleton<MoneyManager>.Instance.cashBalance, ATMInterface.remainingAllowedDeposit);
			}
			return (float)ATMInterface.amounts[index];
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x0014C388 File Offset: 0x0014A588
		private void UpdateAvailableAmounts()
		{
			for (int i = 0; i < ATMInterface.amounts.Length; i++)
			{
				if (this.depositing && i == ATMInterface.amounts.Length - 1)
				{
					this.amountButtons[this.amountButtons.Count - 1].interactable = (this.relevantBalance > 0f && ATMInterface.remainingAllowedDeposit > 0f);
					return;
				}
				if (this.depositing)
				{
					this.amountButtons[i].interactable = (this.relevantBalance >= (float)ATMInterface.amounts[i] && ATM.WeeklyDepositSum + (float)ATMInterface.amounts[i] <= 10000f);
				}
				else
				{
					this.amountButtons[i].interactable = (this.relevantBalance >= (float)ATMInterface.amounts[i]);
				}
			}
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x0014C465 File Offset: 0x0014A665
		public void AmountConfirmed()
		{
			base.StartCoroutine(this.ProcessTransaction(this.selectedAmount, this.depositing));
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x0014C480 File Offset: 0x0014A680
		public void ChangeAmount(float amount)
		{
			this.selectedAmountIndex = -1;
			this.SetSelectedAmount(this.selectedAmount + amount);
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x0014C497 File Offset: 0x0014A697
		protected IEnumerator ProcessTransaction(float amount, bool depositing)
		{
			this.SetActiveScreen(this.processingScreen);
			yield return new WaitForSeconds(1f);
			this.CompleteSound.Play();
			if (depositing)
			{
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= amount)
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-amount, true, false);
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Cash Deposit", amount, 1f, string.Empty);
					ATM.WeeklyDepositSum += amount;
					this.successScreenSubtitle.text = "You have deposited " + MoneyManager.FormatAmount(amount, false, false);
					this.SetActiveScreen(this.successScreen);
				}
				else
				{
					this.SetActiveScreen(this.menuScreen);
				}
			}
			else if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= amount)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(amount, true, false);
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Cash Withdrawal", -amount, 1f, string.Empty);
				this.successScreenSubtitle.text = "You have withdrawn " + MoneyManager.FormatAmount(amount, false, false);
				this.SetActiveScreen(this.successScreen);
			}
			else
			{
				this.SetActiveScreen(this.menuScreen);
			}
			yield break;
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x0014C4B4 File Offset: 0x0014A6B4
		public void DoneButtonPressed()
		{
			this.SetIsOpen(false);
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x0014C2A4 File Offset: 0x0014A4A4
		public void ReturnToMenuButtonPressed()
		{
			this.SetActiveScreen(this.menuScreen);
		}

		// Token: 0x04003AD0 RID: 15056
		[Header("References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x04003AD1 RID: 15057
		[SerializeField]
		protected ATM atm;

		// Token: 0x04003AD2 RID: 15058
		[SerializeField]
		protected AudioSourceController CompleteSound;

		// Token: 0x04003AD3 RID: 15059
		[Header("Menu")]
		[SerializeField]
		protected RectTransform menuScreen;

		// Token: 0x04003AD4 RID: 15060
		[SerializeField]
		protected Text menu_TitleText;

		// Token: 0x04003AD5 RID: 15061
		[SerializeField]
		protected Button menu_DepositButton;

		// Token: 0x04003AD6 RID: 15062
		[SerializeField]
		protected Button menu_WithdrawButton;

		// Token: 0x04003AD7 RID: 15063
		[Header("Top bar")]
		[SerializeField]
		protected Text depositLimitText;

		// Token: 0x04003AD8 RID: 15064
		[SerializeField]
		protected Text onlineBalanceText;

		// Token: 0x04003AD9 RID: 15065
		[SerializeField]
		protected Text cleanCashText;

		// Token: 0x04003ADA RID: 15066
		[SerializeField]
		protected RectTransform depositLimitContainer;

		// Token: 0x04003ADB RID: 15067
		[Header("Amount screen")]
		[SerializeField]
		protected RectTransform amountSelectorScreen;

		// Token: 0x04003ADC RID: 15068
		[SerializeField]
		protected Text amountSelectorTitle;

		// Token: 0x04003ADD RID: 15069
		[SerializeField]
		protected List<Button> amountButtons = new List<Button>();

		// Token: 0x04003ADE RID: 15070
		[SerializeField]
		protected Text amountLabelText;

		// Token: 0x04003ADF RID: 15071
		[SerializeField]
		protected RectTransform amountBackground;

		// Token: 0x04003AE0 RID: 15072
		[SerializeField]
		protected RectTransform selectedButtonIndicator;

		// Token: 0x04003AE1 RID: 15073
		[SerializeField]
		protected Button confirmAmountButton;

		// Token: 0x04003AE2 RID: 15074
		[SerializeField]
		protected Text confirmButtonText;

		// Token: 0x04003AE3 RID: 15075
		[Header("Processing screen")]
		[SerializeField]
		protected RectTransform processingScreen;

		// Token: 0x04003AE4 RID: 15076
		[SerializeField]
		protected RectTransform processingScreenIndicator;

		// Token: 0x04003AE5 RID: 15077
		[Header("Success screen")]
		[SerializeField]
		protected RectTransform successScreen;

		// Token: 0x04003AE6 RID: 15078
		[SerializeField]
		protected Text successScreenSubtitle;

		// Token: 0x04003AE7 RID: 15079
		[SerializeField]
		protected Button doneButton;

		// Token: 0x04003AE9 RID: 15081
		private RectTransform activeScreen;

		// Token: 0x04003AEA RID: 15082
		public static int[] amounts = new int[]
		{
			20,
			50,
			100,
			500,
			1000,
			5000
		};

		// Token: 0x04003AEB RID: 15083
		private bool depositing = true;

		// Token: 0x04003AEC RID: 15084
		private int selectedAmountIndex;

		// Token: 0x04003AED RID: 15085
		private float selectedAmount;
	}
}
