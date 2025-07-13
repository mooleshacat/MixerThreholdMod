using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FluffyUnderware.DevTools.Extensions;
using GameKit.Utilities;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.UI.Input;
using ScheduleOne.UI.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A59 RID: 2649
	public class PickpocketScreen : Singleton<PickpocketScreen>
	{
		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06004736 RID: 18230 RVA: 0x0012B28C File Offset: 0x0012948C
		// (set) Token: 0x06004737 RID: 18231 RVA: 0x0012B294 File Offset: 0x00129494
		public bool IsOpen { get; private set; }

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06004738 RID: 18232 RVA: 0x0012B29D File Offset: 0x0012949D
		// (set) Token: 0x06004739 RID: 18233 RVA: 0x0012B2A5 File Offset: 0x001294A5
		public bool TutorialOpen { get; private set; }

		// Token: 0x0600473A RID: 18234 RVA: 0x0012B2AE File Offset: 0x001294AE
		protected override void Awake()
		{
			base.Awake();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x0012B2E8 File Offset: 0x001294E8
		public void Open(NPC _npc)
		{
			this.IsOpen = true;
			this.npc = _npc;
			this.npc.SetIsBeingPickPocketed(true);
			Singleton<GameInput>.Instance.ExitAll();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0f);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			Player.Local.VisualState.ApplyState("pickpocketing", PlayerVisualState.EVisualState.Pickpocketing, 0f);
			ItemSlot[] array = _npc.Inventory.ItemSlots.ToArray();
			Arrays.Shuffle<ItemSlot>(array);
			for (int i = 0; i < this.Slots.Length; i++)
			{
				if (i < array.Length)
				{
					this.Slots[i].AssignSlot(array[i]);
				}
				else
				{
					this.Slots[i].ClearSlot();
				}
			}
			Singleton<ItemUIManager>.Instance.EnableQuickMove(new List<ItemSlot>(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots()), array.ToList<ItemSlot>());
			for (int j = 0; j < this.Slots.Length; j++)
			{
				ItemSlotUI itemSlotUI = this.Slots[j];
				this.SetSlotLocked(j, true);
				if (itemSlotUI.assignedSlot == null || itemSlotUI.assignedSlot.Quantity == 0)
				{
					this.GreenAreas[j].gameObject.SetActive(false);
				}
				else
				{
					float monetaryValue = itemSlotUI.assignedSlot.ItemInstance.GetMonetaryValue();
					float x = Mathf.Lerp(this.GreenAreaMaxWidth, this.GreenAreaMinWidth, Mathf.Pow(Mathf.Clamp01(monetaryValue / this.ValueDivisor), 0.3f));
					RectTransform rectTransform = this.GreenAreas[j];
					rectTransform.sizeDelta = new Vector2(x, rectTransform.sizeDelta.y);
					rectTransform.gameObject.SetActive(true);
					rectTransform.anchoredPosition = new Vector2(37.5f + 90f * (float)j, rectTransform.anchoredPosition.y);
				}
			}
			this.InputPrompt.SetLabel("Stop Arrow");
			this.isFail = false;
			this.isSliding = true;
			this.sliderPosition = 0f;
			this.slideDirection = 1;
			this.slideTimeMultiplier = 1f;
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x0012B546 File Offset: 0x00129746
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
				this.Close();
			}
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x0012B570 File Offset: 0x00129770
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (this.isFail)
			{
				return;
			}
			if (Player.Local.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.Close();
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Jump))
			{
				if (this.isSliding)
				{
					this.StopArrow();
				}
				else
				{
					this.InputPrompt.SetLabel("Stop Arrow");
					this.isSliding = true;
					ItemSlotUI hoveredSlot = this.GetHoveredSlot();
					if (((hoveredSlot != null) ? hoveredSlot.assignedSlot : null) != null)
					{
						this.GreenAreas[ArrayExt.IndexOf<ItemSlotUI>(this.Slots, this.GetHoveredSlot())].gameObject.SetActive(false);
					}
				}
			}
			if (this.isSliding)
			{
				this.slideTimeMultiplier = Mathf.Clamp(this.slideTimeMultiplier + Time.deltaTime / 20f, 0f, this.SlideTimeMaxMultiplier);
				if (this.slideDirection == 1)
				{
					this.sliderPosition = Mathf.Clamp01(this.sliderPosition + Time.deltaTime / this.SlideTime * this.slideTimeMultiplier);
					if (this.sliderPosition >= 1f)
					{
						this.slideDirection = -1;
					}
				}
				else
				{
					this.sliderPosition = Mathf.Clamp01(this.sliderPosition - Time.deltaTime / this.SlideTime * this.slideTimeMultiplier);
					if (this.sliderPosition <= 0f)
					{
						this.slideDirection = 1;
					}
				}
			}
			this.Slider.value = this.sliderPosition;
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x0012B6D0 File Offset: 0x001298D0
		private void StopArrow()
		{
			if (this.onStop != null)
			{
				this.onStop.Invoke();
			}
			this.isSliding = false;
			ItemSlotUI hoveredSlot = this.GetHoveredSlot();
			this.InputPrompt.SetLabel("Continue");
			if (hoveredSlot != null)
			{
				NetworkSingleton<LevelManager>.Instance.AddXP(2);
				this.SetSlotLocked(ArrayExt.IndexOf<ItemSlotUI>(this.Slots, hoveredSlot), false);
				Customer component = this.npc.GetComponent<Customer>();
				if (component != null && component.TimeSinceLastDealCompleted < 60 && hoveredSlot.assignedSlot != null && hoveredSlot.assignedSlot.ItemInstance != null && hoveredSlot.assignedSlot.ItemInstance is ProductItemInstance)
				{
					Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.INDIAN_DEALER);
				}
				if (this.onHitGreen != null)
				{
					this.onHitGreen.Invoke();
					return;
				}
			}
			else
			{
				this.Fail();
			}
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x0012B7A4 File Offset: 0x001299A4
		public void SetSlotLocked(int index, bool locked)
		{
			this.Slots[index].Rect.Find("Locked").gameObject.SetActive(locked);
			this.Slots[index].assignedSlot.SetIsAddLocked(locked);
			this.Slots[index].assignedSlot.SetIsRemovalLocked(locked);
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x0012B7FC File Offset: 0x001299FC
		private ItemSlotUI GetHoveredSlot()
		{
			for (int i = 0; i < this.GreenAreas.Length; i++)
			{
				if (this.GreenAreas[i].gameObject.activeSelf)
				{
					float num = this.GetGreenAreaNormalizedPosition(i) - this.GetGreenAreaNormalizedWidth(i) / 2f;
					float num2 = this.GetGreenAreaNormalizedPosition(i) + this.GetGreenAreaNormalizedWidth(i) / 2f;
					if (this.Slider.value >= num - this.Tolerance && this.Slider.value <= num2 + this.Tolerance)
					{
						return this.Slots[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0012B893 File Offset: 0x00129A93
		private void Fail()
		{
			this.isFail = true;
			if (this.onFail != null)
			{
				this.onFail.Invoke();
			}
			base.StartCoroutine(this.<Fail>g__FailCoroutine|40_0());
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x0012B8BC File Offset: 0x00129ABC
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			for (int i = 0; i < this.Slots.Length; i++)
			{
				if (this.Slots[i].assignedSlot != null)
				{
					this.Slots[i].assignedSlot.SetIsRemovalLocked(false);
				}
			}
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0f);
			Player.Local.VisualState.RemoveState("pickpocketing", 0f);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			this.npc.SetIsBeingPickPocketed(false);
			if (this.isFail)
			{
				this.npc.responses.PlayerFailedPickpocket(Player.Local);
				this.npc.Inventory.ExpirePickpocket();
			}
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x0012B9D6 File Offset: 0x00129BD6
		private void OpenTutorial()
		{
			this.TutorialOpen = true;
			this.TutorialContainer.gameObject.SetActive(true);
			this.TutorialAnimation.Play();
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x0012B9FC File Offset: 0x00129BFC
		public void CloseTutorial()
		{
			this.TutorialOpen = false;
			this.TutorialContainer.gameObject.SetActive(false);
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x0012BA16 File Offset: 0x00129C16
		private float GetGreenAreaNormalizedPosition(int index)
		{
			return this.GreenAreas[index].anchoredPosition.x / this.SliderContainer.sizeDelta.x;
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x0012BA3B File Offset: 0x00129C3B
		private float GetGreenAreaNormalizedWidth(int index)
		{
			return this.GreenAreas[index].sizeDelta.x / this.SliderContainer.sizeDelta.x;
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x0012BAC7 File Offset: 0x00129CC7
		[CompilerGenerated]
		private IEnumerator <Fail>g__FailCoroutine|40_0()
		{
			yield return new WaitForSeconds(0.9f);
			if (this.IsOpen)
			{
				this.Close();
			}
			yield break;
		}

		// Token: 0x04003408 RID: 13320
		public const int PICKPOCKET_XP = 2;

		// Token: 0x0400340B RID: 13323
		[Header("Settings")]
		public float GreenAreaMaxWidth = 70f;

		// Token: 0x0400340C RID: 13324
		public float GreenAreaMinWidth = 5f;

		// Token: 0x0400340D RID: 13325
		public float SlideTime = 1f;

		// Token: 0x0400340E RID: 13326
		public float SlideTimeMaxMultiplier = 2f;

		// Token: 0x0400340F RID: 13327
		public float ValueDivisor = 300f;

		// Token: 0x04003410 RID: 13328
		public float Tolerance = 0.01f;

		// Token: 0x04003411 RID: 13329
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003412 RID: 13330
		public RectTransform Container;

		// Token: 0x04003413 RID: 13331
		public ItemSlotUI[] Slots;

		// Token: 0x04003414 RID: 13332
		public RectTransform[] GreenAreas;

		// Token: 0x04003415 RID: 13333
		public Animation TutorialAnimation;

		// Token: 0x04003416 RID: 13334
		public RectTransform TutorialContainer;

		// Token: 0x04003417 RID: 13335
		public RectTransform SliderContainer;

		// Token: 0x04003418 RID: 13336
		public Slider Slider;

		// Token: 0x04003419 RID: 13337
		public InputPrompt InputPrompt;

		// Token: 0x0400341A RID: 13338
		public UnityEvent onFail;

		// Token: 0x0400341B RID: 13339
		public UnityEvent onStop;

		// Token: 0x0400341C RID: 13340
		public UnityEvent onHitGreen;

		// Token: 0x0400341D RID: 13341
		private NPC npc;

		// Token: 0x0400341E RID: 13342
		private bool isSliding;

		// Token: 0x0400341F RID: 13343
		private int slideDirection = 1;

		// Token: 0x04003420 RID: 13344
		private float sliderPosition;

		// Token: 0x04003421 RID: 13345
		private float slideTimeMultiplier = 1f;

		// Token: 0x04003422 RID: 13346
		private bool isFail;
	}
}
