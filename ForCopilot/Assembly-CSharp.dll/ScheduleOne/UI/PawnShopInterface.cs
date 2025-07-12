using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Items;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A52 RID: 2642
	public class PawnShopInterface : Singleton<PawnShopInterface>
	{
		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x060046F6 RID: 18166 RVA: 0x00129C84 File Offset: 0x00127E84
		// (set) Token: 0x060046F7 RID: 18167 RVA: 0x00129C8C File Offset: 0x00127E8C
		public bool IsOpen { get; private set; }

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x060046F8 RID: 18168 RVA: 0x00129C95 File Offset: 0x00127E95
		// (set) Token: 0x060046F9 RID: 18169 RVA: 0x00129C9D File Offset: 0x00127E9D
		public float SelectedPayment { get; private set; }

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x060046FA RID: 18170 RVA: 0x00129CA6 File Offset: 0x00127EA6
		// (set) Token: 0x060046FB RID: 18171 RVA: 0x00129CAE File Offset: 0x00127EAE
		public float NPCAnger { get; private set; }

		// Token: 0x060046FC RID: 18172 RVA: 0x00129CB8 File Offset: 0x00127EB8
		protected override void Awake()
		{
			base.Awake();
			this.PawnSlots = new ItemSlot[5];
			for (int i = 0; i < 5; i++)
			{
				this.PawnSlots[i] = new ItemSlot();
				this.PawnSlots[i].AddFilter(new ItemFilter_LegalStatus(ELegalStatus.Legal));
				ItemFilter_ID itemFilter_ID = new ItemFilter_ID(new List<string>
				{
					"cash"
				});
				itemFilter_ID.IsWhitelist = false;
				this.PawnSlots[i].AddFilter(itemFilter_ID);
				ItemSlot itemSlot = this.PawnSlots[i];
				itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.PawnSlotChanged));
				this.Slots[i].AssignSlot(this.PawnSlots[i]);
			}
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
			this.StartButton.onClick.AddListener(new UnityAction(this.StartButtonPressed));
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x00129DC0 File Offset: 0x00127FC0
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.OnMinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.OnMinPass));
			TimeManager instance3 = NetworkSingleton<TimeManager>.Instance;
			instance3.onDayPass = (Action)Delegate.Remove(instance3.onDayPass, new Action(this.OnDayPass));
			TimeManager instance4 = NetworkSingleton<TimeManager>.Instance;
			instance4.onDayPass = (Action)Delegate.Combine(instance4.onDayPass, new Action(this.OnDayPass));
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x00129E6C File Offset: 0x0012806C
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.OnMinPass));
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onDayPass = (Action)Delegate.Remove(instance2.onDayPass, new Action(this.OnDayPass));
			}
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x00129ED4 File Offset: 0x001280D4
		public void Open()
		{
			this.IsOpen = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			Singleton<ItemUIManager>.Instance.EnableQuickMove(new List<ItemSlot>(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots()), this.PawnSlots.ToList<ItemSlot>());
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			this.UpdateValueRangeLabels();
			this.CurrentState = PawnShopInterface.EState.WaitingForOffer;
			this.ResetUI();
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x00129F9C File Offset: 0x0012819C
		public void Close(bool returnItemsToPlayer)
		{
			this.ResetUI();
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0f);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			if (this.routine != null)
			{
				base.StopCoroutine(this.routine);
				this.routine = null;
			}
			if (returnItemsToPlayer)
			{
				foreach (ItemSlot itemSlot in this.PawnSlots)
				{
					if (itemSlot.ItemInstance != null)
					{
						PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(itemSlot.ItemInstance.GetCopy(-1));
					}
				}
			}
			ItemSlot[] pawnSlots = this.PawnSlots;
			for (int i = 0; i < pawnSlots.Length; i++)
			{
				pawnSlots[i].ClearStoredInstance(false);
			}
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x0012A0A8 File Offset: 0x001282A8
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.Used = true;
				if (this.CurrentState == PawnShopInterface.EState.Negotiating)
				{
					this.EndNegotiation();
					return;
				}
				this.Close(true);
			}
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x0012A0E3 File Offset: 0x001282E3
		private void OnMinPass()
		{
			this.ChangeAnger(-0.0013888889f);
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x0012A0F0 File Offset: 0x001282F0
		private void OnDayPass()
		{
			this.SetAngeredToday(false);
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x0012A0FC File Offset: 0x001282FC
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (Player.Local.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.Close(true);
				return;
			}
			if (this.CurrentState == PawnShopInterface.EState.WaitingForOffer)
			{
				this.StartButton.interactable = (this.GetPawnItems().Count > 0);
			}
			else if (this.CurrentState == PawnShopInterface.EState.Negotiating)
			{
				if (Mathf.Abs(this.SelectedPayment - this.LastShopOffer) <= 0.5f)
				{
					this.AcceptCounterButtonLabel.text = "ACCEPT";
				}
				else
				{
					this.AcceptCounterButtonLabel.text = "COUNTER";
				}
			}
			this.AngerSlider.value = Mathf.Lerp(this.AngerSlider.value, 0.1f + this.NPCAnger * 0.9f, Time.deltaTime * 2f);
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x0012A1CC File Offset: 0x001283CC
		private List<ItemInstance> GetPawnItems()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			foreach (ItemSlot itemSlot in this.PawnSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					list.Add(itemSlot.ItemInstance);
				}
			}
			return list;
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x0012A20D File Offset: 0x0012840D
		private void PawnSlotChanged()
		{
			this.UpdateValueRangeLabels();
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x0012A218 File Offset: 0x00128418
		private void UpdateValueRangeLabels()
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < this.PawnSlots.Length; i++)
			{
				if (this.PawnSlots[i].ItemInstance == null)
				{
					this.ValueRangeLabels[i].enabled = false;
				}
				else
				{
					StorableItemDefinition storableItemDefinition = this.PawnSlots[i].ItemInstance.Definition as StorableItemDefinition;
					float num3 = storableItemDefinition.BasePurchasePrice * storableItemDefinition.ResellMultiplier * (float)this.PawnSlots[i].ItemInstance.Quantity;
					float num4 = num3 * 0.5f;
					float num5 = num3 * 2f;
					this.ValueRangeLabels[i].text = string.Format("{0} - {1}", MoneyManager.FormatAmount(num4, false, false), MoneyManager.FormatAmount(num5, false, false));
					num += num4;
					num2 += num5;
				}
			}
			this.TotalValueLabel.text = "Total: <color=#FFD755>" + string.Format("{0} - {1}", MoneyManager.FormatAmount(num, false, false), MoneyManager.FormatAmount(num2, false, false)) + "</color>";
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x0012A31A File Offset: 0x0012851A
		public void StartButtonPressed()
		{
			this.StartNegotiation();
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x0012A322 File Offset: 0x00128522
		private void StartNegotiation()
		{
			if (this.CurrentState != PawnShopInterface.EState.WaitingForOffer)
			{
				return;
			}
			this.CurrentState = PawnShopInterface.EState.Negotiating;
			this.CurrentNegotiationRound = 0;
			this.LastRefusedAmount = float.MaxValue;
			this.routine = base.StartCoroutine(this.<StartNegotiation>g__NegotiationRoutine|67_0());
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x0012A358 File Offset: 0x00128558
		private void PlayShopResponse(PawnShopInterface.EShopResponse response, float counter)
		{
			switch (response)
			{
			case PawnShopInterface.EShopResponse.Accept:
			{
				string text = this.AcceptLines[UnityEngine.Random.Range(0, this.AcceptLines.Length)];
				this.PawnShopNPC.dialogueHandler.ShowWorldspaceDialogue(text, 30f);
				return;
			}
			case PawnShopInterface.EShopResponse.Counter:
				this.CounterLines[UnityEngine.Random.Range(0, this.CounterLines.Length)].Replace("<AMOUNT>", MoneyManager.FormatAmount(counter, false, false));
				return;
			case PawnShopInterface.EShopResponse.Refusal:
			{
				string text2 = this.RefusalLines[UnityEngine.Random.Range(0, this.RefusalLines.Length)];
				text2 = text2.Replace("<AMOUNT>", MoneyManager.FormatAmount(counter, false, false));
				this.PawnShopNPC.dialogueHandler.ShowWorldspaceDialogue(text2, 30f);
				this.PawnShopNPC.PlayVO(EVOLineType.No);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x0012A41C File Offset: 0x0012861C
		private PawnShopInterface.EShopResponse EvaluateCounter(float lastShopOffer, float playerOffer, out float counterAmount, out float angerChange)
		{
			counterAmount = playerOffer;
			angerChange = 0f;
			float num = playerOffer / this.InitialShopOffer;
			float num2 = playerOffer / lastShopOffer;
			Console.Log("Original ratio: " + num.ToString() + " - Last ratio: " + num2.ToString(), null);
			float num3 = Mathf.Clamp01(2f - (num + num2) / 2f);
			float num4 = Mathf.Clamp(num3, 0f, 0.9f);
			num4 *= Mathf.Clamp01(1f - this.NPCAnger * 0.5f);
			num4 *= Mathf.Clamp01(1f - (float)this.CurrentNegotiationRound * 0.1f);
			Console.Log("Accept chance: " + num4.ToString(), null);
			float num5 = UnityEngine.Random.Range(0f, 1f);
			angerChange = Mathf.Clamp01(1f - num3) * 0.7f;
			if (playerOffer <= lastShopOffer)
			{
				return PawnShopInterface.EShopResponse.Accept;
			}
			if (playerOffer >= this.LastRefusedAmount)
			{
				counterAmount = lastShopOffer;
				return PawnShopInterface.EShopResponse.Refusal;
			}
			if (num5 > num4)
			{
				this.LastRefusedAmount = playerOffer;
				counterAmount = lastShopOffer;
				return PawnShopInterface.EShopResponse.Refusal;
			}
			float num6 = Mathf.Sqrt(num4);
			if (UnityEngine.Random.Range(0f, 1f) <= num6)
			{
				angerChange *= 0.5f;
				return PawnShopInterface.EShopResponse.Accept;
			}
			angerChange *= 0.75f;
			float offer = Mathf.Lerp(lastShopOffer, playerOffer, UnityEngine.Random.Range(0f, num3));
			counterAmount = this.RoundOffer(offer);
			return PawnShopInterface.EShopResponse.Counter;
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x0012A575 File Offset: 0x00128775
		private void EndNegotiation()
		{
			if (this.routine != null)
			{
				base.StopCoroutine(this.routine);
				this.routine = null;
			}
			this.CurrentState = PawnShopInterface.EState.WaitingForOffer;
			this.PawnShopNPC.dialogueHandler.HideWorldspaceDialogue();
			this.ResetUI();
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x0012A5B0 File Offset: 0x001287B0
		public void PaymentSubmitted(string value)
		{
			float selectedPayment;
			if (float.TryParse(value, out selectedPayment))
			{
				this.SetSelectedPayment(selectedPayment);
				return;
			}
			this.SetSelectedPayment(this.SelectedPayment);
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x0012A5DB File Offset: 0x001287DB
		public void ChangePayment(float change)
		{
			this.SetSelectedPayment(this.SelectedPayment + change);
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x0012A5EC File Offset: 0x001287EC
		public void SetSelectedPayment(float amount)
		{
			Console.Log("Setting selected payment: " + amount.ToString(), null);
			this.SelectedPayment = (float)Mathf.RoundToInt(Mathf.Clamp(amount, 1f, 999999f));
			this.OfferInputField.SetTextWithoutNotify(this.SelectedPayment.ToString());
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x0012A645 File Offset: 0x00128845
		public void SetPlayerResponse(PawnShopInterface.EPlayerResponse response)
		{
			Console.Log("Player response: " + response.ToString(), null);
			this.PlayerResponse = response;
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x0012A66B File Offset: 0x0012886B
		public void AcceptOrCounter()
		{
			if (Mathf.Abs(this.SelectedPayment - this.LastShopOffer) <= 0.5f)
			{
				this.SetPlayerResponse(PawnShopInterface.EPlayerResponse.Accept);
				return;
			}
			this.SetPlayerResponse(PawnShopInterface.EPlayerResponse.Counter);
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x0012A695 File Offset: 0x00128895
		public void Cancel()
		{
			this.SetPlayerResponse(PawnShopInterface.EPlayerResponse.Cancel);
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x0012A6A0 File Offset: 0x001288A0
		private void ChangeAnger(float change)
		{
			this.NPCAnger = Mathf.Clamp01(this.NPCAnger + change);
			if (this.NPCAnger >= 0.8f)
			{
				this.PawnShopNPC.Avatar.EmotionManager.AddEmotionOverride("Angry", "pawn_angry", 0f, 2);
			}
			else if (this.NPCAnger >= 0.5f)
			{
				this.PawnShopNPC.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "pawn_annoyed", 0f, 1);
			}
			else
			{
				this.PawnShopNPC.Avatar.EmotionManager.RemoveEmotionOverride("pawn_angry");
				this.PawnShopNPC.Avatar.EmotionManager.RemoveEmotionOverride("pawn_annoyed");
			}
			if (this.NPCAnger >= 1f)
			{
				Console.Log("NPC is angry! Closing shop.", null);
				this.SetAngeredToday(true);
				if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
				{
					string text = this.AngeredLines[UnityEngine.Random.Range(0, this.AngeredLines.Length)];
					this.PawnShopNPC.dialogueHandler.ShowWorldspaceDialogue(text, 5f);
				}
				else
				{
					string text2 = this.CrashOutLines[UnityEngine.Random.Range(0, this.CrashOutLines.Length)];
					this.PawnShopNPC.dialogueHandler.ShowWorldspaceDialogue(text2, 5f);
					this.PawnShopNPC.behaviour.CombatBehaviour.SetTarget(null, Player.Local.NetworkObject);
					this.PawnShopNPC.behaviour.CombatBehaviour.Enable_Networked(null);
				}
				this.PawnShopNPC.PlayVO(EVOLineType.Angry);
				this.Close(true);
			}
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x0012A838 File Offset: 0x00128A38
		private void SetAngeredToday(bool angered)
		{
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("PawnShopAngeredToday", angered.ToString(), true);
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x0012A854 File Offset: 0x00128A54
		private void Think()
		{
			string text = this.ThinkLines[UnityEngine.Random.Range(0, this.ThinkLines.Length)];
			this.PawnShopNPC.dialogueHandler.ShowWorldspaceDialogue(text, 3f);
			this.PawnShopNPC.PlayVO(EVOLineType.Think);
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x0012A89C File Offset: 0x00128A9C
		private void SetOffer(float amount)
		{
			Console.Log("Setting offer: " + amount.ToString(), null);
			string text = this.OfferLines[UnityEngine.Random.Range(0, this.OfferLines.Length)];
			text = text.Replace("<AMOUNT>", MoneyManager.FormatAmount(amount, false, false));
			this.LastShopOffer = amount;
			this.SetSelectedPayment(amount);
			this.PawnShopNPC.dialogueHandler.ShowWorldspaceDialogue(text, 30f);
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x0012A910 File Offset: 0x00128B10
		private void FinalizeDeal(float amount)
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(amount, true, true);
			string text = this.DealFinalizedLines[UnityEngine.Random.Range(0, this.DealFinalizedLines.Length)];
			this.PawnShopNPC.dialogueHandler.ShowWorldspaceDialogue(text, 5f);
			this.PawnShopNPC.PlayVO(EVOLineType.Acknowledge);
			this.Close(false);
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x0012A96C File Offset: 0x00128B6C
		private float GetTotalValue()
		{
			float num = 0f;
			foreach (ItemSlot itemSlot in this.PawnSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					num += this.GetItemValue(itemSlot.ItemInstance);
				}
			}
			return num;
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x0012A9B0 File Offset: 0x00128BB0
		private float RoundOffer(float offer)
		{
			if (offer <= 25f)
			{
				return offer;
			}
			if (offer <= 100f)
			{
				return Mathf.Round(offer / 5f) * 5f;
			}
			if (offer <= 1000f)
			{
				return Mathf.Round(offer / 10f) * 10f;
			}
			if (offer <= 10000f)
			{
				return Mathf.Round(offer / 50f) * 50f;
			}
			if (offer <= 100000f)
			{
				return Mathf.Round(offer / 100f) * 100f;
			}
			if (offer <= 1000000f)
			{
				return Mathf.Round(offer / 500f) * 500f;
			}
			return Mathf.Round(offer / 1000f) * 1000f;
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x0012AA60 File Offset: 0x00128C60
		private float GetItemValue(ItemInstance item)
		{
			StorableItemDefinition storableItemDefinition = item.Definition as StorableItemDefinition;
			float num = storableItemDefinition.BasePurchasePrice * storableItemDefinition.ResellMultiplier * (float)item.Quantity;
			int hashCode = ((((int)item.Name[0] + item.Name.Length > 1) ? item.Name[1].ToString() : "A") + NetworkSingleton<TimeManager>.Instance.DayIndex.ToString()).GetHashCode();
			float time = Mathf.Lerp(0.5f, 2f, Mathf.InverseLerp(-2.1474836E+09f, 2.1474836E+09f, (float)hashCode));
			float num2 = this.RandomCurve.Evaluate(time);
			Console.Log("Value multiplier: " + time.ToString() + " -> " + num2.ToString(), null);
			return num * num2;
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x0012AB38 File Offset: 0x00128D38
		private void ResetUI()
		{
			this.Step1CanvasGroup.alpha = 1f;
			this.Step1CanvasGroup.interactable = true;
			this.Step1CanvasGroup.blocksRaycasts = true;
			this.Step2CanvasGroup.alpha = 0f;
			this.Step2CanvasGroup.interactable = false;
			this.Step2CanvasGroup.blocksRaycasts = false;
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x0012AB9D File Offset: 0x00128D9D
		[CompilerGenerated]
		private IEnumerator <StartNegotiation>g__NegotiationRoutine|67_0()
		{
			this.Step1Animation.Play(this.FadeOutAnim.name);
			this.Think();
			yield return new WaitForSeconds(this.Step1Animation[this.FadeOutAnim.name].length);
			yield return new WaitForSeconds(0.75f);
			this.InitialShopOffer = this.RoundOffer(this.GetTotalValue());
			this.SetOffer(this.InitialShopOffer);
			this.SetPlayerResponse(PawnShopInterface.EPlayerResponse.None);
			this.Step2Animation.Play(this.FadeInAnim.name);
			yield return new WaitUntil(() => this.PlayerResponse > PawnShopInterface.EPlayerResponse.None);
			switch (this.PlayerResponse)
			{
			case PawnShopInterface.EPlayerResponse.Accept:
				this.FinalizeDeal(this.InitialShopOffer);
				yield break;
			case PawnShopInterface.EPlayerResponse.Cancel:
				this.EndNegotiation();
				yield break;
			}
			for (;;)
			{
				this.Step2Animation.Play(this.FadeOutAnim.name);
				float counter;
				float change;
				PawnShopInterface.EShopResponse shopResponse = this.EvaluateCounter(this.LastShopOffer, this.SelectedPayment, out counter, out change);
				Console.Log(string.Concat(new string[]
				{
					"Shop response: ",
					shopResponse.ToString(),
					" - Counter: ",
					counter.ToString(),
					" - Anger change: ",
					change.ToString()
				}), null);
				this.ChangeAnger(change);
				if (this.NPCAnger >= 1f)
				{
					break;
				}
				this.Think();
				yield return new WaitForSeconds(this.Step1Animation[this.FadeOutAnim.name].length);
				yield return new WaitForSeconds(0.75f);
				this.SetOffer(counter);
				this.SetPlayerResponse(PawnShopInterface.EPlayerResponse.None);
				this.PlayShopResponse(shopResponse, counter);
				this.Step2Animation.Play(this.FadeInAnim.name);
				yield return new WaitUntil(() => this.PlayerResponse > PawnShopInterface.EPlayerResponse.None);
				switch (this.PlayerResponse)
				{
				case PawnShopInterface.EPlayerResponse.Accept:
					goto IL_2C0;
				case PawnShopInterface.EPlayerResponse.Cancel:
					goto IL_2CE;
				}
				this.CurrentNegotiationRound++;
			}
			yield break;
			IL_2C0:
			this.FinalizeDeal(this.SelectedPayment);
			yield break;
			IL_2CE:
			this.EndNegotiation();
			yield break;
			yield break;
		}

		// Token: 0x040033BF RID: 13247
		public const float PAYMENT_MIN = 1f;

		// Token: 0x040033C0 RID: 13248
		public const float PAYMENT_MAX = 999999f;

		// Token: 0x040033C1 RID: 13249
		public const float THINK_TIME = 0.75f;

		// Token: 0x040033C2 RID: 13250
		public const float MIN_VALUE_MULTIPLIER = 0.5f;

		// Token: 0x040033C3 RID: 13251
		public const float MAX_VALUE_MULTIPLIER = 2f;

		// Token: 0x040033C4 RID: 13252
		public const int PAWN_SLOT_COUNT = 5;

		// Token: 0x040033C6 RID: 13254
		private PawnShopInterface.EState CurrentState;

		// Token: 0x040033C7 RID: 13255
		private PawnShopInterface.EPlayerResponse PlayerResponse;

		// Token: 0x040033C8 RID: 13256
		private int CurrentNegotiationRound;

		// Token: 0x040033C9 RID: 13257
		private float InitialShopOffer;

		// Token: 0x040033CA RID: 13258
		private float LastShopOffer;

		// Token: 0x040033CB RID: 13259
		private float LastRefusedAmount;

		// Token: 0x040033CC RID: 13260
		public NPC PawnShopNPC;

		// Token: 0x040033CF RID: 13263
		public AnimationCurve RandomCurve;

		// Token: 0x040033D0 RID: 13264
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040033D1 RID: 13265
		public RectTransform Container;

		// Token: 0x040033D2 RID: 13266
		public ItemSlotUI[] Slots;

		// Token: 0x040033D3 RID: 13267
		public TextMeshProUGUI[] ValueRangeLabels;

		// Token: 0x040033D4 RID: 13268
		public TextMeshProUGUI TotalValueLabel;

		// Token: 0x040033D5 RID: 13269
		public Button StartButton;

		// Token: 0x040033D6 RID: 13270
		public Animation Step1Animation;

		// Token: 0x040033D7 RID: 13271
		public CanvasGroup Step1CanvasGroup;

		// Token: 0x040033D8 RID: 13272
		public Animation Step2Animation;

		// Token: 0x040033D9 RID: 13273
		public CanvasGroup Step2CanvasGroup;

		// Token: 0x040033DA RID: 13274
		public AnimationClip FadeInAnim;

		// Token: 0x040033DB RID: 13275
		public AnimationClip FadeOutAnim;

		// Token: 0x040033DC RID: 13276
		public TMP_InputField OfferInputField;

		// Token: 0x040033DD RID: 13277
		public Slider AngerSlider;

		// Token: 0x040033DE RID: 13278
		public TextMeshProUGUI AcceptCounterButtonLabel;

		// Token: 0x040033DF RID: 13279
		[Header("Settings")]
		public string[] OfferLines;

		// Token: 0x040033E0 RID: 13280
		public string[] ThinkLines;

		// Token: 0x040033E1 RID: 13281
		public string[] AcceptLines;

		// Token: 0x040033E2 RID: 13282
		public string[] CounterLines;

		// Token: 0x040033E3 RID: 13283
		public string[] RefusalLines;

		// Token: 0x040033E4 RID: 13284
		public string[] DealFinalizedLines;

		// Token: 0x040033E5 RID: 13285
		public string[] AngeredLines;

		// Token: 0x040033E6 RID: 13286
		public string[] CrashOutLines;

		// Token: 0x040033E7 RID: 13287
		private ItemSlot[] PawnSlots;

		// Token: 0x040033E8 RID: 13288
		private Coroutine routine;

		// Token: 0x02000A53 RID: 2643
		public enum EState
		{
			// Token: 0x040033EA RID: 13290
			WaitingForOffer,
			// Token: 0x040033EB RID: 13291
			Negotiating
		}

		// Token: 0x02000A54 RID: 2644
		public enum EPlayerResponse
		{
			// Token: 0x040033ED RID: 13293
			None,
			// Token: 0x040033EE RID: 13294
			Accept,
			// Token: 0x040033EF RID: 13295
			Counter,
			// Token: 0x040033F0 RID: 13296
			Cancel
		}

		// Token: 0x02000A55 RID: 2645
		public enum EShopResponse
		{
			// Token: 0x040033F2 RID: 13298
			Accept,
			// Token: 0x040033F3 RID: 13299
			Counter,
			// Token: 0x040033F4 RID: 13300
			Refusal
		}
	}
}
