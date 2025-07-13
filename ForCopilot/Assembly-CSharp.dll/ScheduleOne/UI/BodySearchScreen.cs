using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009FE RID: 2558
	public class BodySearchScreen : Singleton<BodySearchScreen>
	{
		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x060044D2 RID: 17618 RVA: 0x00120AEF File Offset: 0x0011ECEF
		// (set) Token: 0x060044D3 RID: 17619 RVA: 0x00120AF7 File Offset: 0x0011ECF7
		public bool IsOpen { get; private set; }

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x060044D4 RID: 17620 RVA: 0x00120B00 File Offset: 0x0011ED00
		// (set) Token: 0x060044D5 RID: 17621 RVA: 0x00120B08 File Offset: 0x0011ED08
		public bool TutorialOpen { get; private set; }

		// Token: 0x060044D6 RID: 17622 RVA: 0x00120B14 File Offset: 0x0011ED14
		protected override void Start()
		{
			base.Start();
			if (Player.Local != null)
			{
				this.SetupSlots();
			}
			else
			{
				Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.SetupSlots));
			}
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x00120B7C File Offset: 0x0011ED7C
		private void SetupSlots()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.SetupSlots));
			for (int i = 0; i < 8; i++)
			{
				ItemSlotUI slot = UnityEngine.Object.Instantiate<ItemSlotUI>(this.ItemSlotPrefab, this.SlotContainer);
				slot.AssignSlot(PlayerSingleton<PlayerInventory>.Instance.hotbarSlots[i]);
				this.slots.Add(slot);
				EventTrigger eventTrigger = slot.Rect.gameObject.AddComponent<EventTrigger>();
				eventTrigger.triggers = new List<EventTrigger.Entry>();
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = 2;
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					this.SlotHeld(slot);
				});
				eventTrigger.triggers.Add(entry);
				EventTrigger.Entry entry2 = new EventTrigger.Entry();
				entry2.eventID = 3;
				entry2.callback.AddListener(delegate(BaseEventData data)
				{
					this.SlotReleased(slot);
				});
				eventTrigger.triggers.Add(entry2);
			}
			this.defaultSlotColor = this.slots[0].normalColor;
			this.defaultSlotHighlightColor = this.slots[0].highlightColor;
		}

		// Token: 0x060044D8 RID: 17624 RVA: 0x00120CC4 File Offset: 0x0011EEC4
		private void Update()
		{
			if (this.hoveredSlot != null)
			{
				this.hoveredSlot.SetHighlighted(this.hoveredSlot != this.concealedSlot);
			}
			if (this.IsOpen)
			{
				if (GameInput.GetButton(GameInput.ButtonCode.Jump))
				{
					this.speedBoost = Mathf.MoveTowards(this.speedBoost, 2.5f, Time.deltaTime * 6f);
				}
				else
				{
					this.speedBoost = Mathf.MoveTowards(this.speedBoost, 0f, Time.deltaTime * 6f);
				}
				if (Player.Local != null && Player.Local.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
				{
					this.Close(false);
				}
			}
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x00120D74 File Offset: 0x0011EF74
		public void Open(NPC _searcher, float searchTime = 0f)
		{
			BodySearchScreen.<>c__DisplayClass37_0 CS$<>8__locals1 = new BodySearchScreen.<>c__DisplayClass37_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.searchTime = searchTime;
			this.IsOpen = true;
			this.searcher = _searcher;
			Singleton<GameInput>.Instance.ExitAll();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0f);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			for (int i = 0; i < this.slots.Count; i++)
			{
				if (this.slots[i].assignedSlot.ItemInstance != null && this.slots[i].assignedSlot.ItemInstance.Definition.legalStatus != ELegalStatus.Legal)
				{
					this.slots[i].SetNormalColor(this.SlotRedColor);
					this.slots[i].SetHighlightColor(this.SlotHighlightRedColor);
				}
				else
				{
					this.slots[i].SetNormalColor(this.defaultSlotColor);
					this.slots[i].SetHighlightColor(this.defaultSlotHighlightColor);
				}
				this.slots[i].SetHighlighted(false);
			}
			this.concealedSlot = null;
			base.StartCoroutine(CS$<>8__locals1.<Open>g__Search|0());
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x00120EE9 File Offset: 0x0011F0E9
		private bool IsSlotConcealed(ItemSlotUI slot)
		{
			return this.concealedSlot == slot;
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x00120EF7 File Offset: 0x0011F0F7
		private void ItemDetected(ItemSlotUI slot)
		{
			this.IndicatorAnimation.Play("Police icon discover");
			if (this.onSearchFail != null)
			{
				this.onSearchFail.Invoke();
			}
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x00120F20 File Offset: 0x0011F120
		public void SlotHeld(ItemSlotUI ui)
		{
			this.concealedSlot = ui;
			Image[] componentsInChildren = ui.ItemContainer.GetComponentsInChildren<Image>();
			this.defaultItemIconColors = new Color[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.defaultItemIconColors[i] = componentsInChildren[i].color;
				componentsInChildren[i].color = Color.black;
			}
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x00120F80 File Offset: 0x0011F180
		public void SlotReleased(ItemSlotUI ui)
		{
			this.concealedSlot = null;
			Image[] componentsInChildren = ui.ItemContainer.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = this.defaultItemIconColors[i];
			}
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x00120FC4 File Offset: 0x0011F1C4
		public void Close(bool clear)
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			if (clear && this.onSearchClear != null)
			{
				this.onSearchClear.Invoke();
			}
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x00121061 File Offset: 0x0011F261
		private void OpenTutorial()
		{
			this.TutorialOpen = true;
			this.TutorialContainer.gameObject.SetActive(true);
			this.TutorialAnimation.Play();
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x00121087 File Offset: 0x0011F287
		public void CloseTutorial()
		{
			this.TutorialOpen = false;
			this.TutorialContainer.gameObject.SetActive(false);
		}

		// Token: 0x040031A3 RID: 12707
		public const float MAX_SPEED_BOOST = 2.5f;

		// Token: 0x040031A6 RID: 12710
		public Color SlotRedColor = new Color(1f, 0f, 0f, 0.5f);

		// Token: 0x040031A7 RID: 12711
		public Color SlotHighlightRedColor = new Color(1f, 0f, 0f, 0.5f);

		// Token: 0x040031A8 RID: 12712
		public float GapTime = 0.2f;

		// Token: 0x040031A9 RID: 12713
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040031AA RID: 12714
		public RectTransform Container;

		// Token: 0x040031AB RID: 12715
		public RectTransform MinigameController;

		// Token: 0x040031AC RID: 12716
		public RectTransform SlotContainer;

		// Token: 0x040031AD RID: 12717
		public ItemSlotUI ItemSlotPrefab;

		// Token: 0x040031AE RID: 12718
		public RectTransform SearchIndicator;

		// Token: 0x040031AF RID: 12719
		public RectTransform SearchIndicatorStart;

		// Token: 0x040031B0 RID: 12720
		public RectTransform SearchIndicatorEnd;

		// Token: 0x040031B1 RID: 12721
		public Animation IndicatorAnimation;

		// Token: 0x040031B2 RID: 12722
		public Animation TutorialAnimation;

		// Token: 0x040031B3 RID: 12723
		public RectTransform TutorialContainer;

		// Token: 0x040031B4 RID: 12724
		public Animation ResetAnimation;

		// Token: 0x040031B5 RID: 12725
		private List<ItemSlotUI> slots = new List<ItemSlotUI>();

		// Token: 0x040031B6 RID: 12726
		public UnityEvent onSearchClear;

		// Token: 0x040031B7 RID: 12727
		public UnityEvent onSearchFail;

		// Token: 0x040031B8 RID: 12728
		private Color defaultSlotColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x040031B9 RID: 12729
		private Color defaultSlotHighlightColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x040031BA RID: 12730
		private ItemSlotUI concealedSlot;

		// Token: 0x040031BB RID: 12731
		private ItemSlotUI hoveredSlot;

		// Token: 0x040031BC RID: 12732
		private Color[] defaultItemIconColors;

		// Token: 0x040031BD RID: 12733
		private float speedBoost;

		// Token: 0x040031BE RID: 12734
		private NPC searcher;
	}
}
