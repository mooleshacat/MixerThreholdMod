using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks.Tasks;
using ScheduleOne.Product;
using ScheduleOne.Properties;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000AB0 RID: 2736
	public class MixingStationCanvas : Singleton<MixingStationCanvas>
	{
		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x0600497E RID: 18814 RVA: 0x00134C82 File Offset: 0x00132E82
		// (set) Token: 0x0600497F RID: 18815 RVA: 0x00134C8A File Offset: 0x00132E8A
		public bool isOpen { get; protected set; }

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06004980 RID: 18816 RVA: 0x00134C93 File Offset: 0x00132E93
		// (set) Token: 0x06004981 RID: 18817 RVA: 0x00134C9B File Offset: 0x00132E9B
		public MixingStation MixingStation { get; protected set; }

		// Token: 0x06004982 RID: 18818 RVA: 0x00134CA4 File Offset: 0x00132EA4
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x00134CC8 File Offset: 0x00132EC8
		protected override void Start()
		{
			base.Start();
			this.isOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(true);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x00134D08 File Offset: 0x00132F08
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
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			action.Used = true;
			if (Singleton<NewMixScreen>.Instance.IsOpen)
			{
				Singleton<NewMixScreen>.Instance.Close();
			}
			this.Close(true);
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x00134D55 File Offset: 0x00132F55
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
					return;
				}
				this.UpdateInput();
				this.UpdateUI();
			}
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x000045B1 File Offset: 0x000027B1
		private void UpdateUI()
		{
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x00134D88 File Offset: 0x00132F88
		private void UpdateInput()
		{
			this.UpdateDisplayMode();
			this.UpdateInstruction();
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x00134D98 File Offset: 0x00132F98
		public void Open(MixingStation station)
		{
			this.isOpen = true;
			this.MixingStation = station;
			this.UpdateUI();
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("MixingHintsShown"))
			{
				this.MixerHint.gameObject.SetActive(true);
				this.ProductHint.gameObject.SetActive(true);
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("MixingHintsShown", true.ToString(), true);
			}
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			}
			this.ProductSlotUI.AssignSlot(station.ProductSlot);
			this.IngredientSlotUI.AssignSlot(station.MixerSlot);
			this.OutputSlotUI.AssignSlot(station.OutputSlot);
			ItemSlot productSlot = station.ProductSlot;
			productSlot.onItemDataChanged = (Action)Delegate.Combine(productSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			ItemSlot mixerSlot = station.MixerSlot;
			mixerSlot.onItemDataChanged = (Action)Delegate.Combine(mixerSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			ItemSlot outputSlot = station.OutputSlot;
			outputSlot.onItemDataChanged = (Action)Delegate.Combine(outputSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			List<ItemSlot> list = new List<ItemSlot>();
			list.Add(station.ProductSlot);
			list.Add(station.MixerSlot);
			list.Add(station.OutputSlot);
			Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), list);
			this.UpdateDisplayMode();
			this.UpdateInstruction();
			this.UpdatePreview();
			this.UpdateBeginButton();
			ProductDefinition productDefinition;
			if (station.IsMixingDone && !station.CurrentMixOperation.IsOutputKnown(out productDefinition))
			{
				List<Property> properties;
				station.CurrentMixOperation.GetOutput(out properties);
				ProductDefinition item = Registry.GetItem<ProductDefinition>(this.MixingStation.CurrentMixOperation.ProductID);
				station.DiscoveryBox.ShowProduct(item, properties);
				station.DiscoveryBox.transform.SetParent(PlayerSingleton<PlayerCamera>.Instance.transform);
				station.DiscoveryBox.transform.localPosition = station.DiscoveryBoxOffset;
				station.DiscoveryBox.transform.localRotation = station.DiscoveryBoxRotation;
				float productMarketValue = ProductManager.CalculateProductValue(item.BasePrice, properties);
				Singleton<NewMixScreen>.Instance.Open(properties, item.DrugType, productMarketValue);
				NewMixScreen instance = Singleton<NewMixScreen>.Instance;
				instance.onMixNamed = (Action<string>)Delegate.Remove(instance.onMixNamed, new Action<string>(this.MixNamed));
				NewMixScreen instance2 = Singleton<NewMixScreen>.Instance;
				instance2.onMixNamed = (Action<string>)Delegate.Combine(instance2.onMixNamed, new Action<string>(this.MixNamed));
			}
			else
			{
				station.onMixDone.RemoveListener(new UnityAction(this.MixingDone));
				station.onMixDone.AddListener(new UnityAction(this.MixingDone));
			}
			Singleton<CompassManager>.Instance.SetVisible(false);
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x001350AC File Offset: 0x001332AC
		public void Close(bool enablePlayerControl = true)
		{
			this.isOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			}
			this.ProductSlotUI.ClearSlot();
			this.IngredientSlotUI.ClearSlot();
			this.OutputSlotUI.ClearSlot();
			ItemSlot productSlot = this.MixingStation.ProductSlot;
			productSlot.onItemDataChanged = (Action)Delegate.Remove(productSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			ItemSlot mixerSlot = this.MixingStation.MixerSlot;
			mixerSlot.onItemDataChanged = (Action)Delegate.Remove(mixerSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			ItemSlot outputSlot = this.MixingStation.OutputSlot;
			outputSlot.onItemDataChanged = (Action)Delegate.Remove(outputSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			this.MixingStation.onMixDone.RemoveListener(new UnityAction(this.MixingDone));
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			if (enablePlayerControl)
			{
				this.MixingStation.Close();
				this.MixingStation = null;
			}
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x001351E0 File Offset: 0x001333E0
		private void MixingDone()
		{
			ProductDefinition productDefinition;
			if (this.MixingStation.IsMixingDone && !this.MixingStation.CurrentMixOperation.IsOutputKnown(out productDefinition))
			{
				List<Property> properties;
				this.MixingStation.CurrentMixOperation.GetOutput(out properties);
				ProductDefinition item = Registry.GetItem<ProductDefinition>(this.MixingStation.CurrentMixOperation.ProductID);
				this.MixingStation.DiscoveryBox.ShowProduct(item, properties);
				this.MixingStation.DiscoveryBox.transform.SetParent(PlayerSingleton<PlayerCamera>.Instance.transform);
				this.MixingStation.DiscoveryBox.transform.localPosition = this.MixingStation.DiscoveryBoxOffset;
				this.MixingStation.DiscoveryBox.transform.localRotation = this.MixingStation.DiscoveryBoxRotation;
				float productMarketValue = ProductManager.CalculateProductValue(item.BasePrice, properties);
				Singleton<NewMixScreen>.Instance.Open(properties, item.DrugType, productMarketValue);
				NewMixScreen instance = Singleton<NewMixScreen>.Instance;
				instance.onMixNamed = (Action<string>)Delegate.Remove(instance.onMixNamed, new Action<string>(this.MixNamed));
				NewMixScreen instance2 = Singleton<NewMixScreen>.Instance;
				instance2.onMixNamed = (Action<string>)Delegate.Combine(instance2.onMixNamed, new Action<string>(this.MixNamed));
			}
			this.UpdateDisplayMode();
			this.UpdateInstruction();
			this.UpdatePreview();
			this.UpdateBeginButton();
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x00135334 File Offset: 0x00133534
		private void StationContentsChanged()
		{
			this.UpdateDisplayMode();
			this.UpdatePreview();
			this.UpdateBeginButton();
			if (this.MixingStation.ProductSlot.Quantity > 0)
			{
				this.ProductHint.gameObject.SetActive(false);
			}
			if (this.MixingStation.MixerSlot.Quantity > 0)
			{
				this.MixerHint.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x0013539C File Offset: 0x0013359C
		private void UpdateDisplayMode()
		{
			this.TitleContainer.gameObject.SetActive(true);
			this.MainContainer.gameObject.SetActive(true);
			this.OutputSlotUI.gameObject.SetActive(false);
			if (this.MixingStation.OutputSlot.Quantity > 0)
			{
				this.MainContainer.gameObject.SetActive(false);
				this.OutputSlotUI.gameObject.SetActive(true);
				return;
			}
			ProductDefinition productDefinition;
			if (this.MixingStation.CurrentMixOperation != null && this.MixingStation.IsMixingDone && !this.MixingStation.CurrentMixOperation.IsOutputKnown(out productDefinition))
			{
				this.TitleContainer.gameObject.SetActive(false);
				this.MainContainer.gameObject.SetActive(false);
				this.OutputSlotUI.gameObject.SetActive(false);
				return;
			}
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00135474 File Offset: 0x00133674
		private void UpdateInstruction()
		{
			this.InstructionLabel.enabled = true;
			if (this.MixingStation.OutputSlot.Quantity > 0)
			{
				this.InstructionLabel.text = "Collect output";
				return;
			}
			if (this.MixingStation.CurrentMixOperation != null)
			{
				this.InstructionLabel.text = "Mixing in progress...";
				return;
			}
			if (!this.MixingStation.CanStartMix())
			{
				this.InstructionLabel.text = "Insert unpackaged product and mixing ingredient";
				return;
			}
			this.InstructionLabel.enabled = false;
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x001354FC File Offset: 0x001336FC
		private void UpdatePreview()
		{
			ProductDefinition product = this.MixingStation.GetProduct();
			PropertyItemDefinition mixer = this.MixingStation.GetMixer();
			if (product != null)
			{
				this.ProductPropertiesLabel.text = this.GetPropertyListString(product.Properties);
				this.ProductPropertiesLabel.enabled = true;
			}
			else
			{
				this.ProductPropertiesLabel.enabled = false;
			}
			if (mixer == null && this.MixingStation.MixerSlot.Quantity > 0)
			{
				this.IngredientProblemLabel.enabled = true;
			}
			else
			{
				this.IngredientProblemLabel.enabled = false;
			}
			this.UnknownOutputIcon.gameObject.SetActive(false);
			if (!(product != null) || !(mixer != null))
			{
				this.PreviewIcon.enabled = false;
				this.PreviewLabel.enabled = false;
				this.PreviewPropertiesLabel.enabled = false;
				return;
			}
			List<Property> outputProperties = this.GetOutputProperties(product, mixer);
			ProductDefinition knownProduct = NetworkSingleton<ProductManager>.Instance.GetKnownProduct(product.DrugTypes[0].DrugType, outputProperties);
			if (knownProduct == null)
			{
				this.PreviewIcon.sprite = product.Icon;
				this.PreviewIcon.color = Color.black;
				this.PreviewIcon.enabled = true;
				this.PreviewLabel.text = "Unknown";
				this.PreviewLabel.enabled = true;
				this.UnknownOutputIcon.gameObject.SetActive(true);
				this.PreviewPropertiesLabel.text = string.Empty;
				for (int i = 0; i < outputProperties.Count; i++)
				{
					if (product.Properties.Contains(outputProperties[i]))
					{
						if (this.PreviewPropertiesLabel.text.Length > 0)
						{
							TextMeshProUGUI previewPropertiesLabel = this.PreviewPropertiesLabel;
							previewPropertiesLabel.text += "\n";
						}
						TextMeshProUGUI previewPropertiesLabel2 = this.PreviewPropertiesLabel;
						previewPropertiesLabel2.text += this.GetPropertyString(outputProperties[i]);
					}
					else
					{
						if (this.PreviewPropertiesLabel.text.Length > 0)
						{
							TextMeshProUGUI previewPropertiesLabel3 = this.PreviewPropertiesLabel;
							previewPropertiesLabel3.text += "\n";
						}
						TextMeshProUGUI previewPropertiesLabel4 = this.PreviewPropertiesLabel;
						previewPropertiesLabel4.text = previewPropertiesLabel4.text + "<color=#" + ColorUtility.ToHtmlStringRGBA(outputProperties[i].LabelColor) + ">• ?</color>";
					}
				}
				this.PreviewPropertiesLabel.enabled = true;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.PreviewPropertiesLabel.rectTransform);
				return;
			}
			this.PreviewIcon.sprite = knownProduct.Icon;
			this.PreviewIcon.color = Color.white;
			this.PreviewIcon.enabled = true;
			this.PreviewLabel.text = knownProduct.Name;
			this.PreviewLabel.enabled = true;
			this.UnknownOutputIcon.gameObject.SetActive(false);
			this.PreviewPropertiesLabel.text = this.GetPropertyListString(knownProduct.Properties);
			this.PreviewPropertiesLabel.enabled = true;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.PreviewPropertiesLabel.rectTransform);
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x0013580C File Offset: 0x00133A0C
		private string GetPropertyListString(List<Property> properties)
		{
			string text = "";
			for (int i = 0; i < properties.Count; i++)
			{
				if (i > 0)
				{
					text += "\n";
				}
				text += this.GetPropertyString(properties[i]);
			}
			return text;
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x0012283F File Offset: 0x00120A3F
		private string GetPropertyString(Property property)
		{
			return string.Concat(new string[]
			{
				"<color=#",
				ColorUtility.ToHtmlStringRGBA(property.LabelColor),
				">• ",
				property.Name,
				"</color>"
			});
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x00135858 File Offset: 0x00133A58
		private List<Property> GetOutputProperties(ProductDefinition product, PropertyItemDefinition mixer)
		{
			List<Property> properties = product.Properties;
			List<Property> properties2 = mixer.Properties;
			return PropertyMixCalculator.MixProperties(properties, properties2[0], product.DrugType);
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x00135884 File Offset: 0x00133A84
		private bool IsOutputKnown(out ProductDefinition knownProduct)
		{
			knownProduct = null;
			ProductDefinition product = this.MixingStation.GetProduct();
			PropertyItemDefinition mixer = this.MixingStation.GetMixer();
			if (product != null && mixer != null)
			{
				List<Property> outputProperties = this.GetOutputProperties(product, mixer);
				knownProduct = NetworkSingleton<ProductManager>.Instance.GetKnownProduct(product.DrugTypes[0].DrugType, outputProperties);
			}
			return knownProduct != null;
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x001358F0 File Offset: 0x00133AF0
		private void UpdateBeginButton()
		{
			if (this.MixingStation.CurrentMixOperation != null || this.MixingStation.OutputSlot.Quantity > 0)
			{
				this.BeginButton.gameObject.SetActive(false);
				return;
			}
			this.BeginButton.gameObject.SetActive(true);
			this.BeginButton.interactable = this.MixingStation.CanStartMix();
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00135958 File Offset: 0x00133B58
		public void BeginButtonPressed()
		{
			int mixQuantity = this.MixingStation.GetMixQuantity();
			if (mixQuantity <= 0)
			{
				Console.LogWarning("Failed to start mixing operation, not enough ingredients or output slot is full", null);
				return;
			}
			bool flag = false;
			if (Application.isEditor && Input.GetKey(KeyCode.R))
			{
				flag = true;
			}
			if (this.MixingStation.RequiresIngredientInsertion && !flag)
			{
				MixingStation mixingStation = this.MixingStation;
				this.Close(false);
				new UseMixingStationTask(mixingStation);
				return;
			}
			ProductItemInstance productItemInstance = this.MixingStation.ProductSlot.ItemInstance as ProductItemInstance;
			string id = this.MixingStation.MixerSlot.ItemInstance.ID;
			this.MixingStation.ProductSlot.ChangeQuantity(-mixQuantity, false);
			this.MixingStation.MixerSlot.ChangeQuantity(-mixQuantity, false);
			this.StartMixOperation(new MixOperation(productItemInstance.ID, productItemInstance.Quality, id, mixQuantity));
			this.Close(true);
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x00135A30 File Offset: 0x00133C30
		public void StartMixOperation(MixOperation mixOperation)
		{
			this.MixingStation.SendMixingOperation(mixOperation, 0);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Mixing_Operations_Started", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Mixing_Operations_Started") + 1f).ToString(), true);
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00135A78 File Offset: 0x00133C78
		private void MixNamed(string mixName)
		{
			if (this.MixingStation == null)
			{
				Console.LogWarning("Mixing station is null, cannot finish mix operation", null);
				return;
			}
			if (this.MixingStation.CurrentMixOperation == null)
			{
				Console.LogWarning("Mixing station current mix operation is null, cannot finish mix operation", null);
				return;
			}
			NetworkSingleton<ProductManager>.Instance.FinishAndNameMix(this.MixingStation.CurrentMixOperation.ProductID, this.MixingStation.CurrentMixOperation.IngredientID, mixName);
			this.MixingStation.TryCreateOutputItems();
			this.MixingStation.DiscoveryBox.gameObject.SetActive(false);
			this.UpdateDisplayMode();
		}

		// Token: 0x04003620 RID: 13856
		[Header("Prefabs")]
		public StationRecipeEntry RecipeEntryPrefab;

		// Token: 0x04003621 RID: 13857
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003622 RID: 13858
		public RectTransform Container;

		// Token: 0x04003623 RID: 13859
		public ItemSlotUI ProductSlotUI;

		// Token: 0x04003624 RID: 13860
		public TextMeshProUGUI ProductPropertiesLabel;

		// Token: 0x04003625 RID: 13861
		public ItemSlotUI IngredientSlotUI;

		// Token: 0x04003626 RID: 13862
		public TextMeshProUGUI IngredientProblemLabel;

		// Token: 0x04003627 RID: 13863
		public ItemSlotUI PreviewSlotUI;

		// Token: 0x04003628 RID: 13864
		public Image PreviewIcon;

		// Token: 0x04003629 RID: 13865
		public TextMeshProUGUI PreviewLabel;

		// Token: 0x0400362A RID: 13866
		public RectTransform UnknownOutputIcon;

		// Token: 0x0400362B RID: 13867
		public TextMeshProUGUI PreviewPropertiesLabel;

		// Token: 0x0400362C RID: 13868
		public ItemSlotUI OutputSlotUI;

		// Token: 0x0400362D RID: 13869
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x0400362E RID: 13870
		public RectTransform TitleContainer;

		// Token: 0x0400362F RID: 13871
		public RectTransform MainContainer;

		// Token: 0x04003630 RID: 13872
		public Button BeginButton;

		// Token: 0x04003631 RID: 13873
		public RectTransform ProductHint;

		// Token: 0x04003632 RID: 13874
		public RectTransform MixerHint;

		// Token: 0x04003633 RID: 13875
		private StationRecipe selectedRecipe;
	}
}
