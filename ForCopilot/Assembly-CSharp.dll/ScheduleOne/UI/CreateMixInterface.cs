using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Properties;
using ScheduleOne.Storage;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A0D RID: 2573
	public class CreateMixInterface : Singleton<CreateMixInterface>
	{
		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06004526 RID: 17702 RVA: 0x0012209B File Offset: 0x0012029B
		// (set) Token: 0x06004527 RID: 17703 RVA: 0x001220A3 File Offset: 0x001202A3
		public bool IsOpen { get; private set; }

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06004528 RID: 17704 RVA: 0x001220AC File Offset: 0x001202AC
		private ItemSlot beanSlot
		{
			get
			{
				return this.Storage.ItemSlots[0];
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06004529 RID: 17705 RVA: 0x001220BF File Offset: 0x001202BF
		private ItemSlot mixerSlot
		{
			get
			{
				return this.Storage.ItemSlots[1];
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x0600452A RID: 17706 RVA: 0x001220D2 File Offset: 0x001202D2
		private ItemSlot outputSlot
		{
			get
			{
				return this.Storage.ItemSlots[2];
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x0600452B RID: 17707 RVA: 0x001220E5 File Offset: 0x001202E5
		private ItemSlot productSlot
		{
			get
			{
				return this.Storage.ItemSlots[3];
			}
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x001220F8 File Offset: 0x001202F8
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.enabled = false;
			this.BeansSlot.AssignSlot(this.beanSlot);
			this.MixerSlot.AssignSlot(this.mixerSlot);
			this.OutputSlot.AssignSlot(this.outputSlot);
			this.ProductSlot.AssignSlot(this.productSlot);
			this.beanSlot.AddFilter(new ItemFilter_ID(new List<string>
			{
				"megabean"
			}));
			this.productSlot.AddFilter(new ItemFilter_Category(new List<EItemCategory>
			{
				EItemCategory.Product
			}));
			this.outputSlot.SetIsAddLocked(true);
			this.Storage.onContentsChanged.AddListener(new UnityAction(this.ContentsChanged));
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginPressed));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x001221ED File Offset: 0x001203ED
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

		// Token: 0x0600452E RID: 17710 RVA: 0x00122218 File Offset: 0x00120418
		public void Open()
		{
			this.IsOpen = true;
			this.Canvas.enabled = true;
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0f);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			List<ItemSlot> secondarySlots = new List<ItemSlot>
			{
				this.beanSlot,
				this.productSlot,
				this.mixerSlot
			};
			Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), secondarySlots);
			Singleton<CompassManager>.Instance.SetVisible(false);
			this.ContentsChanged();
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x00122318 File Offset: 0x00120518
		private void ContentsChanged()
		{
			this.UpdateCanBegin();
			this.UpdateOutput();
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x00122328 File Offset: 0x00120528
		private void UpdateCanBegin()
		{
			this.BeanProblemLabel.enabled = !this.HasBeans();
			this.ProductProblemLabel.enabled = !this.HasProduct();
			if (this.HasProduct())
			{
				ProductDefinition productDefinition = this.productSlot.ItemInstance.Definition as ProductDefinition;
				this.ProductPropertiesLabel.text = this.GetPropertyListString(productDefinition.Properties);
				this.ProductPropertiesLabel.enabled = true;
			}
			else
			{
				this.ProductPropertiesLabel.enabled = false;
			}
			if (this.mixerSlot.Quantity == 0)
			{
				this.MixerProblemLabel.text = "Required";
				this.MixerProblemLabel.enabled = true;
			}
			else if (!this.HasMixer())
			{
				this.MixerProblemLabel.text = "Invalid mixer";
				this.MixerProblemLabel.enabled = true;
			}
			else
			{
				this.MixerProblemLabel.enabled = false;
			}
			this.BeginButton.interactable = this.CanBegin();
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x0012241C File Offset: 0x0012061C
		private void UpdateOutput()
		{
			ProductDefinition product = this.GetProduct();
			PropertyItemDefinition mixer = this.GetMixer();
			if (!(product != null) || !(mixer != null))
			{
				this.OutputIcon.enabled = false;
				this.OutputPropertiesLabel.enabled = false;
				this.OutputProblemLabel.enabled = false;
				return;
			}
			List<Property> outputProperties = this.GetOutputProperties(product, mixer);
			ProductDefinition knownProduct = NetworkSingleton<ProductManager>.Instance.GetKnownProduct(product.DrugTypes[0].DrugType, outputProperties);
			if (knownProduct == null)
			{
				this.OutputIcon.sprite = product.Icon;
				this.OutputIcon.color = Color.black;
				this.OutputIcon.enabled = true;
				this.UnknownOutputIcon.gameObject.SetActive(true);
				List<Color32> list = new List<Color32>();
				this.OutputPropertiesLabel.text = string.Empty;
				for (int i = 0; i < outputProperties.Count; i++)
				{
					if (this.OutputPropertiesLabel.text.Length > 0)
					{
						TextMeshProUGUI outputPropertiesLabel = this.OutputPropertiesLabel;
						outputPropertiesLabel.text += "\n";
					}
					if (product.Properties.Contains(outputProperties[i]))
					{
						TextMeshProUGUI outputPropertiesLabel2 = this.OutputPropertiesLabel;
						outputPropertiesLabel2.text += this.GetPropertyString(outputProperties[i]);
					}
					else
					{
						list.Add(outputProperties[i].LabelColor);
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (this.OutputPropertiesLabel.text.Length > 0)
					{
						TextMeshProUGUI outputPropertiesLabel3 = this.OutputPropertiesLabel;
						outputPropertiesLabel3.text += "\n";
					}
					TextMeshProUGUI outputPropertiesLabel4 = this.OutputPropertiesLabel;
					outputPropertiesLabel4.text = outputPropertiesLabel4.text + "<color=#" + ColorUtility.ToHtmlStringRGBA(list[j]) + ">• ?</color>";
				}
				this.OutputPropertiesLabel.enabled = true;
				this.OutputProblemLabel.enabled = false;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.OutputPropertiesLabel.rectTransform);
				return;
			}
			this.OutputIcon.sprite = knownProduct.Icon;
			this.OutputIcon.color = Color.white;
			this.OutputIcon.enabled = true;
			this.UnknownOutputIcon.gameObject.SetActive(false);
			this.OutputPropertiesLabel.text = this.GetPropertyListString(knownProduct.Properties);
			this.OutputPropertiesLabel.enabled = true;
			this.OutputProblemLabel.text = "Mix already known. ";
			this.OutputProblemLabel.enabled = true;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.OutputPropertiesLabel.rectTransform);
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x001226C8 File Offset: 0x001208C8
		private void BeginPressed()
		{
			if (!this.CanBegin())
			{
				return;
			}
			ItemDefinition product = this.GetProduct();
			PropertyItemDefinition mixer = this.GetMixer();
			NewMixOperation operation = new NewMixOperation(product.ID, mixer.ID);
			NetworkSingleton<ProductManager>.Instance.SendMixOperation(operation, false);
			this.beanSlot.ChangeQuantity(-5, false);
			this.productSlot.ChangeQuantity(-1, false);
			this.mixerSlot.ChangeQuantity(-1, false);
			this.Close();
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x00122738 File Offset: 0x00120938
		private List<Property> GetOutputProperties(ProductDefinition product, PropertyItemDefinition mixer)
		{
			List<Property> properties = product.Properties;
			List<Property> properties2 = mixer.Properties;
			return PropertyMixCalculator.MixProperties(properties, properties2[0], product.DrugType);
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x00122764 File Offset: 0x00120964
		private bool IsOutputKnown(out ProductDefinition knownProduct)
		{
			knownProduct = null;
			ProductDefinition product = this.GetProduct();
			PropertyItemDefinition mixer = this.GetMixer();
			if (product != null && mixer != null)
			{
				List<Property> outputProperties = this.GetOutputProperties(product, mixer);
				knownProduct = NetworkSingleton<ProductManager>.Instance.GetKnownProduct(product.DrugTypes[0].DrugType, outputProperties);
			}
			return knownProduct != null;
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x001227C4 File Offset: 0x001209C4
		private string GetPropertyListString(List<Property> properties)
		{
			this.ProductPropertiesLabel.text = "";
			for (int i = 0; i < properties.Count; i++)
			{
				if (i > 0)
				{
					TextMeshProUGUI productPropertiesLabel = this.ProductPropertiesLabel;
					productPropertiesLabel.text += "\n";
				}
				TextMeshProUGUI productPropertiesLabel2 = this.ProductPropertiesLabel;
				productPropertiesLabel2.text += this.GetPropertyString(properties[i]);
			}
			return this.ProductPropertiesLabel.text;
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x0012283F File Offset: 0x00120A3F
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

		// Token: 0x06004537 RID: 17719 RVA: 0x0012287C File Offset: 0x00120A7C
		private bool CanBegin()
		{
			ProductDefinition productDefinition;
			return this.HasBeans() && this.HasProduct() && this.HasMixer() && !this.IsOutputKnown(out productDefinition);
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x001228B0 File Offset: 0x00120AB0
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			if (this.beanSlot.ItemInstance != null)
			{
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.beanSlot.ItemInstance.GetCopy(-1));
				this.beanSlot.ClearStoredInstance(false);
			}
			if (this.productSlot.ItemInstance != null)
			{
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.productSlot.ItemInstance.GetCopy(-1));
				this.productSlot.ClearStoredInstance(false);
			}
			if (this.mixerSlot.ItemInstance != null)
			{
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.mixerSlot.ItemInstance.GetCopy(-1));
				this.mixerSlot.ClearStoredInstance(false);
			}
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, false);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0f);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			Singleton<CompassManager>.Instance.SetVisible(true);
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x00122A03 File Offset: 0x00120C03
		private bool HasProduct()
		{
			return this.GetProduct() != null;
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x00122A11 File Offset: 0x00120C11
		private bool HasBeans()
		{
			return this.beanSlot.Quantity >= 5;
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x00122A24 File Offset: 0x00120C24
		private bool HasMixer()
		{
			return this.GetMixer() != null;
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x00122A32 File Offset: 0x00120C32
		private ProductDefinition GetProduct()
		{
			if (this.productSlot.ItemInstance != null)
			{
				return this.productSlot.ItemInstance.Definition as ProductDefinition;
			}
			return null;
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x00122A58 File Offset: 0x00120C58
		private PropertyItemDefinition GetMixer()
		{
			if (this.mixerSlot.ItemInstance != null)
			{
				PropertyItemDefinition propertyItemDefinition = this.mixerSlot.ItemInstance.Definition as PropertyItemDefinition;
				if (propertyItemDefinition != null && NetworkSingleton<ProductManager>.Instance.ValidMixIngredients.Contains(propertyItemDefinition))
				{
					return propertyItemDefinition;
				}
			}
			return null;
		}

		// Token: 0x040031F3 RID: 12787
		public const int BEAN_REQUIREMENT = 5;

		// Token: 0x040031F5 RID: 12789
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040031F6 RID: 12790
		public ItemSlotUI BeansSlot;

		// Token: 0x040031F7 RID: 12791
		public ItemSlotUI ProductSlot;

		// Token: 0x040031F8 RID: 12792
		public ItemSlotUI MixerSlot;

		// Token: 0x040031F9 RID: 12793
		public ItemSlotUI OutputSlot;

		// Token: 0x040031FA RID: 12794
		public Image OutputIcon;

		// Token: 0x040031FB RID: 12795
		public Button BeginButton;

		// Token: 0x040031FC RID: 12796
		public WorldStorageEntity Storage;

		// Token: 0x040031FD RID: 12797
		public TextMeshProUGUI ProductPropertiesLabel;

		// Token: 0x040031FE RID: 12798
		public TextMeshProUGUI OutputPropertiesLabel;

		// Token: 0x040031FF RID: 12799
		public TextMeshProUGUI BeanProblemLabel;

		// Token: 0x04003200 RID: 12800
		public TextMeshProUGUI ProductProblemLabel;

		// Token: 0x04003201 RID: 12801
		public TextMeshProUGUI MixerProblemLabel;

		// Token: 0x04003202 RID: 12802
		public TextMeshProUGUI OutputProblemLabel;

		// Token: 0x04003203 RID: 12803
		public Transform CameraPosition;

		// Token: 0x04003204 RID: 12804
		public RectTransform UnknownOutputIcon;

		// Token: 0x04003205 RID: 12805
		public UnityEvent onOpen;

		// Token: 0x04003206 RID: 12806
		public UnityEvent onClose;
	}
}
