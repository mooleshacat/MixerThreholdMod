using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000AA6 RID: 2726
	public class CauldronCanvas : Singleton<CauldronCanvas>
	{
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06004932 RID: 18738 RVA: 0x0013317C File Offset: 0x0013137C
		// (set) Token: 0x06004933 RID: 18739 RVA: 0x00133184 File Offset: 0x00131384
		public bool isOpen { get; protected set; }

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06004934 RID: 18740 RVA: 0x0013318D File Offset: 0x0013138D
		// (set) Token: 0x06004935 RID: 18741 RVA: 0x00133195 File Offset: 0x00131395
		public Cauldron Cauldron { get; protected set; }

		// Token: 0x06004936 RID: 18742 RVA: 0x0013319E File Offset: 0x0013139E
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x001331C2 File Offset: 0x001313C2
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(null, false, true);
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x001331D4 File Offset: 0x001313D4
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
				}
				Cauldron.EState state = this.Cauldron.GetState();
				if (state == Cauldron.EState.Ready)
				{
					this.InstructionLabel.enabled = false;
					this.BeginButton.interactable = true;
					return;
				}
				if (state == Cauldron.EState.Cooking)
				{
					this.InstructionLabel.text = "Cooking in progress...";
				}
				else if (state == Cauldron.EState.MissingIngredients)
				{
					this.InstructionLabel.text = "Insert <color=#FFC73D>" + 20.ToString() + "x</color> coca leaves and <color=#FFC73D>1x</color> gasoline";
				}
				else if (state == Cauldron.EState.OutputFull)
				{
					this.InstructionLabel.text = "Output is full";
				}
				this.InstructionLabel.enabled = true;
				this.BeginButton.interactable = false;
			}
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x0013329C File Offset: 0x0013149C
		public void SetIsOpen(Cauldron cauldron, bool open, bool removeUI = true)
		{
			this.isOpen = open;
			this.Canvas.enabled = open;
			this.Container.SetActive(open);
			this.Cauldron = cauldron;
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				if (open)
				{
					PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				}
			}
			if (cauldron != null)
			{
				for (int i = 0; i < this.IngredientSlotUIs.Count; i++)
				{
					this.IngredientSlotUIs[i].AssignSlot(cauldron.IngredientSlots[i]);
				}
				this.LiquidSlotUI.AssignSlot(this.Cauldron.LiquidSlot);
				this.OutputSlotUI.AssignSlot(this.Cauldron.OutputSlot);
			}
			else
			{
				foreach (ItemSlotUI itemSlotUI in this.IngredientSlotUIs)
				{
					itemSlotUI.ClearSlot();
				}
				this.LiquidSlotUI.ClearSlot();
				this.OutputSlotUI.ClearSlot();
			}
			if (open)
			{
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			}
			else if (removeUI)
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(open, true);
			if (open)
			{
				List<ItemSlot> list = new List<ItemSlot>();
				list.AddRange(cauldron.IngredientSlots);
				list.Add(cauldron.LiquidSlot);
				list.Add(cauldron.OutputSlot);
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), list);
			}
			if (this.isOpen)
			{
				this.Update();
				Singleton<CompassManager>.Instance.SetVisible(false);
			}
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x00133458 File Offset: 0x00131658
		public void BeginButtonPressed()
		{
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			new CauldronTask(this.Cauldron);
			this.SetIsOpen(null, false, false);
		}

		// Token: 0x040035D8 RID: 13784
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040035D9 RID: 13785
		public GameObject Container;

		// Token: 0x040035DA RID: 13786
		public List<ItemSlotUI> IngredientSlotUIs;

		// Token: 0x040035DB RID: 13787
		public ItemSlotUI LiquidSlotUI;

		// Token: 0x040035DC RID: 13788
		public ItemSlotUI OutputSlotUI;

		// Token: 0x040035DD RID: 13789
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x040035DE RID: 13790
		public Button BeginButton;
	}
}
