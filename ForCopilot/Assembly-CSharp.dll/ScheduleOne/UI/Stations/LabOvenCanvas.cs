using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000AAF RID: 2735
	public class LabOvenCanvas : Singleton<LabOvenCanvas>
	{
		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06004971 RID: 18801 RVA: 0x00134757 File Offset: 0x00132957
		// (set) Token: 0x06004972 RID: 18802 RVA: 0x0013475F File Offset: 0x0013295F
		public bool isOpen { get; protected set; }

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06004973 RID: 18803 RVA: 0x00134768 File Offset: 0x00132968
		// (set) Token: 0x06004974 RID: 18804 RVA: 0x00134770 File Offset: 0x00132970
		public LabOven Oven { get; protected set; }

		// Token: 0x06004975 RID: 18805 RVA: 0x00134779 File Offset: 0x00132979
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x0013479D File Offset: 0x0013299D
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(null, false, true);
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x001347B0 File Offset: 0x001329B0
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				this.BeginButton.interactable = this.CanBegin();
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
					return;
				}
				if (this.Oven.CurrentOperation != null)
				{
					this.ProgressImg.fillAmount = Mathf.Clamp01((float)this.Oven.CurrentOperation.CookProgress / (float)this.Oven.CurrentOperation.GetCookDuration());
					this.BeginButtonLabel.text = "COLLECT";
					if (this.Oven.CurrentOperation.CookProgress < this.Oven.CurrentOperation.GetCookDuration())
					{
						this.InstructionLabel.text = "Cooking in progress...";
						this.InstructionLabel.enabled = true;
						this.ErrorLabel.enabled = false;
						return;
					}
					if (this.DoesOvenOutputHaveSpace())
					{
						this.InstructionLabel.text = "Ready to collect product";
						this.InstructionLabel.enabled = true;
						this.ErrorLabel.enabled = false;
						return;
					}
					this.ErrorLabel.text = "Not enough space in output slot";
					this.ErrorLabel.enabled = true;
					this.InstructionLabel.enabled = false;
					return;
				}
				else
				{
					this.ProgressContainer.gameObject.SetActive(false);
					this.BeginButtonLabel.text = "BEGIN";
					if (this.Oven.IngredientSlot.ItemInstance != null)
					{
						if (this.Oven.IsIngredientCookable())
						{
							this.InstructionLabel.text = "Ready to begin cooking";
							this.InstructionLabel.enabled = true;
							return;
						}
						this.InstructionLabel.enabled = false;
						this.ErrorLabel.enabled = true;
						this.ErrorLabel.text = "Ingredient is not cookable";
						return;
					}
					else
					{
						this.InstructionLabel.text = "Place cookable item in ingredient slot";
						this.InstructionLabel.enabled = true;
						this.ErrorLabel.enabled = false;
					}
				}
			}
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x0013499C File Offset: 0x00132B9C
		public void SetIsOpen(LabOven oven, bool open, bool removeUI = true)
		{
			this.isOpen = open;
			this.Canvas.enabled = open;
			this.Container.gameObject.SetActive(open);
			this.Oven = oven;
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				if (open)
				{
					PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				}
			}
			if (oven != null)
			{
				this.IngredientSlotUI.AssignSlot(oven.IngredientSlot);
				this.OutputSlotUI.AssignSlot(oven.OutputSlot);
			}
			else
			{
				this.IngredientSlotUI.ClearSlot();
				this.OutputSlotUI.ClearSlot();
			}
			if (open)
			{
				this.RefreshActiveOperation();
				this.Update();
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
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), new List<ItemSlot>
				{
					this.Oven.IngredientSlot,
					this.Oven.OutputSlot
				});
			}
			if (this.isOpen)
			{
				Singleton<CompassManager>.Instance.SetVisible(false);
			}
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x00134AE4 File Offset: 0x00132CE4
		public void BeginButtonPressed()
		{
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			if (this.Oven.CurrentOperation != null)
			{
				new FinalizeLabOven(this.Oven);
			}
			else if ((this.Oven.IngredientSlot.ItemInstance.Definition as StorableItemDefinition).StationItem.GetModule<CookableModule>().CookType == CookableModule.ECookableType.Liquid)
			{
				new StartLabOvenTask(this.Oven);
			}
			else
			{
				new LabOvenSolidTask(this.Oven);
			}
			this.SetIsOpen(null, false, false);
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x00134B68 File Offset: 0x00132D68
		public bool CanBegin()
		{
			if (this.Oven == null)
			{
				return false;
			}
			if (this.Oven.CurrentOperation != null)
			{
				return this.Oven.CurrentOperation.CookProgress >= this.Oven.CurrentOperation.GetCookDuration() && this.DoesOvenOutputHaveSpace();
			}
			return this.Oven.IsIngredientCookable();
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x00134BD0 File Offset: 0x00132DD0
		private bool DoesOvenOutputHaveSpace()
		{
			return this.Oven.OutputSlot.GetCapacityForItem(this.Oven.CurrentOperation.Product.GetDefaultInstance(1), false) >= this.Oven.CurrentOperation.Cookable.ProductQuantity;
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x00134C20 File Offset: 0x00132E20
		private void RefreshActiveOperation()
		{
			if (this.Oven.CurrentOperation != null)
			{
				this.IngredientIcon.sprite = this.Oven.CurrentOperation.Ingredient.Icon;
				this.ProductIcon.sprite = this.Oven.CurrentOperation.Product.Icon;
			}
		}

		// Token: 0x04003612 RID: 13842
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003613 RID: 13843
		public GameObject Container;

		// Token: 0x04003614 RID: 13844
		public ItemSlotUI IngredientSlotUI;

		// Token: 0x04003615 RID: 13845
		public ItemSlotUI OutputSlotUI;

		// Token: 0x04003616 RID: 13846
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x04003617 RID: 13847
		public TextMeshProUGUI ErrorLabel;

		// Token: 0x04003618 RID: 13848
		public Button BeginButton;

		// Token: 0x04003619 RID: 13849
		public TextMeshProUGUI BeginButtonLabel;

		// Token: 0x0400361A RID: 13850
		public RectTransform ProgressContainer;

		// Token: 0x0400361B RID: 13851
		public Image IngredientIcon;

		// Token: 0x0400361C RID: 13852
		public Image ProgressImg;

		// Token: 0x0400361D RID: 13853
		public Image ProductIcon;
	}
}
