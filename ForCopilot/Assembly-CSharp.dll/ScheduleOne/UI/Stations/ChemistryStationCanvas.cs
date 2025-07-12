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
	// Token: 0x02000AA7 RID: 2727
	public class ChemistryStationCanvas : Singleton<ChemistryStationCanvas>
	{
		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x0600493C RID: 18748 RVA: 0x00133482 File Offset: 0x00131682
		// (set) Token: 0x0600493D RID: 18749 RVA: 0x0013348A File Offset: 0x0013168A
		public bool isOpen { get; protected set; }

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x0600493E RID: 18750 RVA: 0x00133493 File Offset: 0x00131693
		// (set) Token: 0x0600493F RID: 18751 RVA: 0x0013349B File Offset: 0x0013169B
		public ChemistryStation ChemistryStation { get; protected set; }

		// Token: 0x06004940 RID: 18752 RVA: 0x001334A4 File Offset: 0x001316A4
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
			for (int i = 0; i < this.Recipes.Count; i++)
			{
				StationRecipeEntry component = UnityEngine.Object.Instantiate<StationRecipeEntry>(this.RecipeEntryPrefab, this.RecipeContainer).GetComponent<StationRecipeEntry>();
				component.AssignRecipe(this.Recipes[i]);
				this.recipeEntries.Add(component);
			}
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x0013351E File Offset: 0x0013171E
		protected override void Start()
		{
			base.Start();
			this.Close(false);
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x00133530 File Offset: 0x00131730
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				if (this.ChemistryStation.CurrentCookOperation != null)
				{
					this.BeginButton.interactable = (this.ChemistryStation.CurrentCookOperation.CurrentTime >= this.ChemistryStation.CurrentCookOperation.Recipe.CookTime_Mins);
					this.BeginButton.gameObject.SetActive(false);
				}
				else
				{
					this.BeginButton.interactable = (this.selectedRecipe != null && this.selectedRecipe.IsValid && this.ChemistryStation.DoesOutputHaveSpace(this.selectedRecipe.Recipe));
					this.BeginButton.gameObject.SetActive(true);
				}
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
				}
				this.UpdateInput();
				this.UpdateUI();
			}
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x00133615 File Offset: 0x00131815
		private void LateUpdate()
		{
			if (!this.isOpen)
			{
				return;
			}
			if (this.selectedRecipe != null)
			{
				this.SelectionIndicator.position = this.selectedRecipe.transform.position;
			}
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x0013364C File Offset: 0x0013184C
		private void UpdateUI()
		{
			this.ErrorLabel.enabled = false;
			if (this.ChemistryStation.CurrentCookOperation != null)
			{
				this.CookingInProgressContainer.gameObject.SetActive(true);
				this.RecipeSelectionContainer.gameObject.SetActive(false);
				if (this.ChemistryStation.CurrentCookOperation.CurrentTime >= this.ChemistryStation.CurrentCookOperation.Recipe.CookTime_Mins)
				{
					this.InProgressLabel.text = "Ready to finish";
				}
				else
				{
					this.InProgressLabel.text = "Cooking in progress...";
				}
				if (this.InProgressRecipeEntry.Recipe != this.ChemistryStation.CurrentCookOperation.Recipe)
				{
					this.InProgressRecipeEntry.AssignRecipe(this.ChemistryStation.CurrentCookOperation.Recipe);
					return;
				}
			}
			else
			{
				this.RecipeSelectionContainer.gameObject.SetActive(true);
				this.CookingInProgressContainer.gameObject.SetActive(false);
				if (this.selectedRecipe != null && !this.ChemistryStation.DoesOutputHaveSpace(this.selectedRecipe.Recipe))
				{
					this.ErrorLabel.text = "Output slot does not have enough space";
					this.ErrorLabel.enabled = true;
				}
			}
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x00133788 File Offset: 0x00131988
		private void UpdateInput()
		{
			if (this.selectedRecipe != null)
			{
				if (GameInput.MouseScrollDelta < 0f || GameInput.GetButtonDown(GameInput.ButtonCode.Backward) || Input.GetKeyDown(KeyCode.DownArrow))
				{
					if (this.recipeEntries.IndexOf(this.selectedRecipe) < this.recipeEntries.Count - 1)
					{
						StationRecipeEntry stationRecipeEntry = this.recipeEntries[this.recipeEntries.IndexOf(this.selectedRecipe) + 1];
						if (stationRecipeEntry.IsValid)
						{
							this.SetSelectedRecipe(stationRecipeEntry);
							return;
						}
					}
				}
				else if ((GameInput.MouseScrollDelta > 0f || GameInput.GetButtonDown(GameInput.ButtonCode.Forward) || Input.GetKeyDown(KeyCode.UpArrow)) && this.recipeEntries.IndexOf(this.selectedRecipe) > 0)
				{
					StationRecipeEntry stationRecipeEntry2 = this.recipeEntries[this.recipeEntries.IndexOf(this.selectedRecipe) - 1];
					if (stationRecipeEntry2.IsValid)
					{
						this.SetSelectedRecipe(stationRecipeEntry2);
					}
				}
			}
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x0013387C File Offset: 0x00131A7C
		public void Open(ChemistryStation station)
		{
			this.isOpen = true;
			this.ChemistryStation = station;
			this.UpdateUI();
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			}
			for (int i = 0; i < station.IngredientSlots.Length; i++)
			{
				this.InputSlotUIs[i].AssignSlot(station.IngredientSlots[i]);
				ItemSlot itemSlot = station.IngredientSlots[i];
				itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.StationSlotsChanged));
			}
			this.OutputSlotUI.AssignSlot(station.OutputSlot);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			List<ItemSlot> list = new List<ItemSlot>();
			list.AddRange(station.IngredientSlots);
			list.Add(station.OutputSlot);
			Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), list);
			Singleton<CompassManager>.Instance.SetVisible(false);
			this.StationSlotsChanged();
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x001339A8 File Offset: 0x00131BA8
		public void Close(bool removeUI)
		{
			this.isOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			}
			for (int i = 0; i < this.InputSlotUIs.Length; i++)
			{
				this.InputSlotUIs[i].ClearSlot();
				if (this.ChemistryStation != null)
				{
					ItemSlot itemSlot = this.ChemistryStation.IngredientSlots[i];
					itemSlot.onItemDataChanged = (Action)Delegate.Remove(itemSlot.onItemDataChanged, new Action(this.StationSlotsChanged));
				}
			}
			this.OutputSlotUI.ClearSlot();
			if (removeUI)
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			this.ChemistryStation = null;
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x00133A77 File Offset: 0x00131C77
		public void BeginButtonPressed()
		{
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			new UseChemistryStationTask(this.ChemistryStation, this.selectedRecipe.Recipe);
			this.Close(false);
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x00133AA4 File Offset: 0x00131CA4
		private void StationSlotsChanged()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			for (int i = 0; i < this.InputSlotUIs.Length; i++)
			{
				if (this.InputSlotUIs[i].assignedSlot.ItemInstance != null)
				{
					list.Add(this.InputSlotUIs[i].assignedSlot.ItemInstance);
				}
			}
			for (int j = 0; j < this.recipeEntries.Count; j++)
			{
				this.recipeEntries[j].RefreshValidity(list);
			}
			this.SortRecipes(list);
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x00133B28 File Offset: 0x00131D28
		private void SortRecipes(List<ItemInstance> ingredients)
		{
			Dictionary<StationRecipeEntry, float> recipes = new Dictionary<StationRecipeEntry, float>();
			for (int i = 0; i < this.recipeEntries.Count; i++)
			{
				float ingredientsMatchDelta = this.recipeEntries[i].GetIngredientsMatchDelta(ingredients);
				recipes.Add(this.recipeEntries[i], ingredientsMatchDelta);
			}
			this.recipeEntries.Sort((StationRecipeEntry a, StationRecipeEntry b) => recipes[b].CompareTo(recipes[a]));
			for (int j = 0; j < this.recipeEntries.Count; j++)
			{
				this.recipeEntries[j].transform.SetAsLastSibling();
			}
			if (this.recipeEntries.Count > 0 && this.recipeEntries[0].IsValid)
			{
				this.SetSelectedRecipe(this.recipeEntries[0]);
				return;
			}
			this.SetSelectedRecipe(null);
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x00133C08 File Offset: 0x00131E08
		private void SetSelectedRecipe(StationRecipeEntry entry)
		{
			this.selectedRecipe = entry;
			if (entry != null)
			{
				this.SelectionIndicator.position = entry.transform.position;
				this.SelectionIndicator.gameObject.SetActive(true);
				return;
			}
			this.SelectionIndicator.gameObject.SetActive(false);
		}

		// Token: 0x040035E1 RID: 13793
		public List<StationRecipe> Recipes = new List<StationRecipe>();

		// Token: 0x040035E2 RID: 13794
		[Header("Prefabs")]
		public StationRecipeEntry RecipeEntryPrefab;

		// Token: 0x040035E3 RID: 13795
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040035E4 RID: 13796
		public RectTransform Container;

		// Token: 0x040035E5 RID: 13797
		public RectTransform InputSlotsContainer;

		// Token: 0x040035E6 RID: 13798
		public ItemSlotUI[] InputSlotUIs;

		// Token: 0x040035E7 RID: 13799
		public ItemSlotUI OutputSlotUI;

		// Token: 0x040035E8 RID: 13800
		public RectTransform RecipeSelectionContainer;

		// Token: 0x040035E9 RID: 13801
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x040035EA RID: 13802
		public Button BeginButton;

		// Token: 0x040035EB RID: 13803
		public RectTransform SelectionIndicator;

		// Token: 0x040035EC RID: 13804
		public RectTransform RecipeContainer;

		// Token: 0x040035ED RID: 13805
		public RectTransform CookingInProgressContainer;

		// Token: 0x040035EE RID: 13806
		public StationRecipeEntry InProgressRecipeEntry;

		// Token: 0x040035EF RID: 13807
		public TextMeshProUGUI InProgressLabel;

		// Token: 0x040035F0 RID: 13808
		public TextMeshProUGUI ErrorLabel;

		// Token: 0x040035F1 RID: 13809
		private List<StationRecipeEntry> recipeEntries = new List<StationRecipeEntry>();

		// Token: 0x040035F2 RID: 13810
		private StationRecipeEntry selectedRecipe;
	}
}
