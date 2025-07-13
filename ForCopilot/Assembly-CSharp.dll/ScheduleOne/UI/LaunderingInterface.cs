using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.Money;
using ScheduleOne.ObjectScripts.Cash;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A3F RID: 2623
	public class LaunderingInterface : MonoBehaviour
	{
		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06004676 RID: 18038 RVA: 0x0012761B File Offset: 0x0012581B
		protected int maxLaunderAmount
		{
			get
			{
				return (int)Mathf.Min(this.business.appliedLaunderLimit, NetworkSingleton<MoneyManager>.Instance.cashBalance);
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06004677 RID: 18039 RVA: 0x00127638 File Offset: 0x00125838
		// (set) Token: 0x06004678 RID: 18040 RVA: 0x00127640 File Offset: 0x00125840
		public Business business { get; private set; }

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06004679 RID: 18041 RVA: 0x00127649 File Offset: 0x00125849
		public bool isOpen
		{
			get
			{
				return this.canvas != null && this.canvas.gameObject.activeSelf;
			}
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x0012766C File Offset: 0x0012586C
		public void Initialize(Business bus)
		{
			this.business = bus;
			this.intObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.intObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			this.launderCapacityLabel.text = MoneyManager.FormatAmount(this.business.LaunderCapacity, false, false);
			this.canvas.gameObject.SetActive(false);
			this.noEntries.gameObject.SetActive(this.operationToEntry.Count == 0);
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(delegate()
			{
				this.canvas.worldCamera = PlayerSingleton<PlayerCamera>.Instance.Camera;
			}));
			foreach (LaunderingOperation op in this.business.LaunderingOperations)
			{
				this.CreateEntry(op);
			}
			Business.onOperationStarted = (Action<LaunderingOperation>)Delegate.Combine(Business.onOperationStarted, new Action<LaunderingOperation>(this.CreateEntry));
			Business.onOperationStarted = (Action<LaunderingOperation>)Delegate.Combine(Business.onOperationStarted, new Action<LaunderingOperation>(this.UpdateCashStacks));
			Business.onOperationFinished = (Action<LaunderingOperation>)Delegate.Combine(Business.onOperationFinished, new Action<LaunderingOperation>(this.RemoveEntry));
			Business.onOperationFinished = (Action<LaunderingOperation>)Delegate.Combine(Business.onOperationFinished, new Action<LaunderingOperation>(this.UpdateCashStacks));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			this.CloseAmountSelector();
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x00127830 File Offset: 0x00125A30
		private void OnDestroy()
		{
			Business.onOperationStarted = (Action<LaunderingOperation>)Delegate.Remove(Business.onOperationStarted, new Action<LaunderingOperation>(this.CreateEntry));
			Business.onOperationStarted = (Action<LaunderingOperation>)Delegate.Remove(Business.onOperationStarted, new Action<LaunderingOperation>(this.UpdateCashStacks));
			Business.onOperationFinished = (Action<LaunderingOperation>)Delegate.Remove(Business.onOperationFinished, new Action<LaunderingOperation>(this.RemoveEntry));
			Business.onOperationFinished = (Action<LaunderingOperation>)Delegate.Remove(Business.onOperationFinished, new Action<LaunderingOperation>(this.UpdateCashStacks));
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x001278BD File Offset: 0x00125ABD
		protected virtual void MinPass()
		{
			if (this.isOpen)
			{
				this.UpdateTimeline();
				this.RefreshLaunderButton();
				this.UpdateCurrentTotal();
				this.UpdateEntryTimes();
			}
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x001278E0 File Offset: 0x00125AE0
		protected void Exit(ExitAction exit)
		{
			if (exit.Used)
			{
				return;
			}
			if (this.isOpen)
			{
				if (this.amountSelectorScreen.gameObject.activeSelf)
				{
					exit.Used = true;
					this.CloseAmountSelector();
					return;
				}
				if (exit.exitType == ExitType.Escape)
				{
					exit.Used = true;
					this.Close();
				}
			}
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x00127934 File Offset: 0x00125B34
		protected void UpdateTimeline()
		{
			foreach (LaunderingOperation launderingOperation in this.business.LaunderingOperations)
			{
				if (!this.operationToNotch.ContainsKey(launderingOperation))
				{
					RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.timelineNotchPrefab, this.notchContainer).GetComponent<RectTransform>();
					component.Find("Amount").GetComponent<TextMeshProUGUI>().text = MoneyManager.FormatAmount(launderingOperation.amount, false, false);
					this.operationToNotch.Add(launderingOperation, component);
					this.notches.Add(component);
				}
			}
			List<RectTransform> list = (from x in this.operationToNotch
			where this.business.LaunderingOperations.Contains(x.Key)
			select x.Value).ToList<RectTransform>();
			for (int i = 0; i < this.notches.Count; i++)
			{
				if (!list.Contains(this.notches[i]))
				{
					UnityEngine.Object.Destroy(this.notches[i].gameObject);
					this.notches.RemoveAt(i);
					i--;
				}
			}
			foreach (LaunderingOperation launderingOperation2 in this.business.LaunderingOperations)
			{
				this.operationToNotch[launderingOperation2].anchoredPosition = new Vector2(this.notchContainer.rect.width * (float)launderingOperation2.minutesSinceStarted / (float)launderingOperation2.completionTime_Minutes, this.operationToNotch[launderingOperation2].anchoredPosition.y);
			}
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x00127B14 File Offset: 0x00125D14
		protected void UpdateCurrentTotal()
		{
			this.currentTotalAmountLabel.text = MoneyManager.FormatAmount(this.business.currentLaunderTotal, false, false);
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x00127B34 File Offset: 0x00125D34
		private void CreateEntry(LaunderingOperation op)
		{
			if (this.operationToEntry.ContainsKey(op))
			{
				return;
			}
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.entryPrefab, this.entryContainer).GetComponent<RectTransform>();
			component.SetAsLastSibling();
			component.Find("BusinessLabel").GetComponent<TextMeshProUGUI>().text = op.business.PropertyName;
			component.Find("AmountLabel").GetComponent<TextMeshProUGUI>().text = MoneyManager.FormatAmount(op.amount, false, false);
			this.operationToEntry.Add(op, component);
			this.UpdateEntryTimes();
			if (this.noEntries != null)
			{
				this.noEntries.gameObject.SetActive(this.operationToEntry.Count == 0);
			}
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x00127BF0 File Offset: 0x00125DF0
		private void RemoveEntry(LaunderingOperation op)
		{
			if (!this.operationToEntry.ContainsKey(op))
			{
				return;
			}
			RectTransform rectTransform = this.operationToEntry[op];
			if (rectTransform != null)
			{
				UnityEngine.Object.Destroy(rectTransform.gameObject);
			}
			this.operationToEntry.Remove(op);
			this.noEntries.gameObject.SetActive(this.operationToEntry.Count == 0);
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x00127C58 File Offset: 0x00125E58
		private void UpdateEntryTimes()
		{
			foreach (LaunderingOperation launderingOperation in this.operationToEntry.Keys.ToList<LaunderingOperation>())
			{
				if (this.operationToEntry.ContainsKey(launderingOperation))
				{
					if (this.operationToEntry[launderingOperation] == null)
					{
						Console.LogWarning("Entry is null for operation " + launderingOperation.business.PropertyName, null);
					}
					else
					{
						int num = launderingOperation.completionTime_Minutes - launderingOperation.minutesSinceStarted;
						if (num > 60)
						{
							int num2 = Mathf.CeilToInt((float)num / 60f);
							this.operationToEntry[launderingOperation].Find("TimeLabel").GetComponent<TextMeshProUGUI>().text = num2.ToString() + " hours";
						}
						else
						{
							this.operationToEntry[launderingOperation].Find("TimeLabel").GetComponent<TextMeshProUGUI>().text = num.ToString() + " minutes";
						}
					}
				}
			}
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x00127D80 File Offset: 0x00125F80
		private void UpdateCashStacks(LaunderingOperation op)
		{
			float num = this.business.currentLaunderTotal;
			for (int i = 0; i < this.CashStacks.Length; i++)
			{
				if (num <= 0f)
				{
					this.CashStacks[i].ShowAmount(0f);
				}
				else
				{
					float num2 = Mathf.Min(num, 1000f);
					this.CashStacks[i].ShowAmount(num2);
					num -= num2;
				}
			}
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x00127DE8 File Offset: 0x00125FE8
		private void RefreshLaunderButton()
		{
			this.launderButton.interactable = (this.business.currentLaunderTotal < this.business.LaunderCapacity && NetworkSingleton<MoneyManager>.Instance.cashBalance > 10f);
			if (this.business.currentLaunderTotal >= this.business.LaunderCapacity)
			{
				this.insufficientCashLabel.text = "The business is already at maximum laundering capacity.";
				this.insufficientCashLabel.gameObject.SetActive(true);
				return;
			}
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance <= 10f)
			{
				this.insufficientCashLabel.text = "You need at least " + MoneyManager.FormatAmount(10f, false, false) + " cash to launder.";
				this.insufficientCashLabel.gameObject.SetActive(true);
				return;
			}
			this.insufficientCashLabel.gameObject.SetActive(false);
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x00127EC0 File Offset: 0x001260C0
		public void OpenAmountSelector()
		{
			this.amountSelectorScreen.gameObject.SetActive(true);
			int num = Mathf.Clamp(100, 10, this.maxLaunderAmount);
			this.selectedAmountToLaunder = num;
			this.amountSlider.minValue = 10f;
			this.amountSlider.maxValue = (float)this.maxLaunderAmount;
			this.amountSlider.SetValueWithoutNotify((float)num);
			this.amountInputField.SetTextWithoutNotify(num.ToString());
		}

		// Token: 0x06004686 RID: 18054 RVA: 0x00127F36 File Offset: 0x00126136
		public void CloseAmountSelector()
		{
			this.amountSelectorScreen.gameObject.SetActive(false);
		}

		// Token: 0x06004687 RID: 18055 RVA: 0x00127F4C File Offset: 0x0012614C
		public void ConfirmAmount()
		{
			int num = Mathf.Clamp(this.selectedAmountToLaunder, 10, this.maxLaunderAmount);
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance((float)(-(float)num), true, false);
			this.business.StartLaunderingOperation((float)num, 0);
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("LaunderingOperationsStarted");
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("LaunderingOperationsStarted", (value + 1f).ToString(), true);
			this.UpdateTimeline();
			this.UpdateCurrentTotal();
			this.RefreshLaunderButton();
			this.CloseAmountSelector();
		}

		// Token: 0x06004688 RID: 18056 RVA: 0x00127FD1 File Offset: 0x001261D1
		public void SliderValueChanged()
		{
			if (this.ignoreSliderChange)
			{
				this.ignoreSliderChange = false;
				return;
			}
			this.selectedAmountToLaunder = (int)this.amountSlider.value;
			this.amountInputField.SetTextWithoutNotify(this.selectedAmountToLaunder.ToString());
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x0012800C File Offset: 0x0012620C
		public void InputValueChanged()
		{
			this.selectedAmountToLaunder = Mathf.Clamp(int.Parse(this.amountInputField.text), 10, this.maxLaunderAmount);
			this.amountInputField.SetTextWithoutNotify(this.selectedAmountToLaunder.ToString());
			this.amountSlider.SetValueWithoutNotify((float)this.selectedAmountToLaunder);
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x00128064 File Offset: 0x00126264
		public void Hovered()
		{
			if (!this.business.IsOwned || this.isOpen)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (this.business.IsOwned && !this.isOpen)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.intObj.SetMessage("Manage business");
			}
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x001280C4 File Offset: 0x001262C4
		public void Interacted()
		{
			if (this.business.IsOwned && !this.isOpen)
			{
				this.Open();
			}
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x001280E4 File Offset: 0x001262E4
		public virtual void Open()
		{
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.cameraPosition.transform.position, this.cameraPosition.rotation, 0.15f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.15f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.RefreshLaunderButton();
			this.UpdateTimeline();
			this.UpdateCurrentTotal();
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x0012819C File Offset: 0x0012639C
		public virtual void Close()
		{
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.15f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.15f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			base.gameObject.SetActive(false);
		}

		// Token: 0x0400332C RID: 13100
		protected const float fovOverride = 65f;

		// Token: 0x0400332D RID: 13101
		protected const float lerpTime = 0.15f;

		// Token: 0x0400332E RID: 13102
		protected const int minLaunderAmount = 10;

		// Token: 0x04003330 RID: 13104
		[Header("References")]
		[SerializeField]
		protected Transform cameraPosition;

		// Token: 0x04003331 RID: 13105
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x04003332 RID: 13106
		[SerializeField]
		protected Button launderButton;

		// Token: 0x04003333 RID: 13107
		[SerializeField]
		protected GameObject amountSelectorScreen;

		// Token: 0x04003334 RID: 13108
		[SerializeField]
		protected Slider amountSlider;

		// Token: 0x04003335 RID: 13109
		[SerializeField]
		protected TMP_InputField amountInputField;

		// Token: 0x04003336 RID: 13110
		[SerializeField]
		protected RectTransform notchContainer;

		// Token: 0x04003337 RID: 13111
		[SerializeField]
		protected TextMeshProUGUI currentTotalAmountLabel;

		// Token: 0x04003338 RID: 13112
		[SerializeField]
		protected TextMeshProUGUI launderCapacityLabel;

		// Token: 0x04003339 RID: 13113
		[SerializeField]
		protected TextMeshProUGUI insufficientCashLabel;

		// Token: 0x0400333A RID: 13114
		[SerializeField]
		protected RectTransform entryContainer;

		// Token: 0x0400333B RID: 13115
		[SerializeField]
		protected RectTransform noEntries;

		// Token: 0x0400333C RID: 13116
		public CashStackVisuals[] CashStacks;

		// Token: 0x0400333D RID: 13117
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject timelineNotchPrefab;

		// Token: 0x0400333E RID: 13118
		[SerializeField]
		protected GameObject entryPrefab;

		// Token: 0x0400333F RID: 13119
		[Header("UI references")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x04003340 RID: 13120
		private int selectedAmountToLaunder;

		// Token: 0x04003341 RID: 13121
		private Dictionary<LaunderingOperation, RectTransform> operationToNotch = new Dictionary<LaunderingOperation, RectTransform>();

		// Token: 0x04003342 RID: 13122
		private List<RectTransform> notches = new List<RectTransform>();

		// Token: 0x04003343 RID: 13123
		private bool ignoreSliderChange = true;

		// Token: 0x04003344 RID: 13124
		private Dictionary<LaunderingOperation, RectTransform> operationToEntry = new Dictionary<LaunderingOperation, RectTransform>();
	}
}
