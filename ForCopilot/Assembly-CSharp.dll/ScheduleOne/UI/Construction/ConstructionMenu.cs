using System;
using System.Collections.Generic;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.Construction;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Construction
{
	// Token: 0x02000BCC RID: 3020
	public class ConstructionMenu : Singleton<ConstructionMenu>
	{
		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06005029 RID: 20521 RVA: 0x001530DB File Offset: 0x001512DB
		// (set) Token: 0x0600502A RID: 20522 RVA: 0x001530E3 File Offset: 0x001512E3
		public bool isOpen { get; protected set; }

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x0600502B RID: 20523 RVA: 0x001530EC File Offset: 0x001512EC
		public Constructable SelectedConstructable
		{
			get
			{
				return this.selectedConstructable;
			}
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x001530F4 File Offset: 0x001512F4
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(false);
			ConstructionManager instance = Singleton<ConstructionManager>.Instance;
			instance.onConstructionModeEnabled = (Action)Delegate.Combine(instance.onConstructionModeEnabled, new Action(delegate()
			{
				this.SetIsOpen(true);
			}));
			ConstructionManager instance2 = Singleton<ConstructionManager>.Instance;
			instance2.onConstructionModeDisabled = (Action)Delegate.Combine(instance2.onConstructionModeDisabled, new Action(delegate()
			{
				this.SetIsOpen(false);
			}));
			ConstructionManager instance3 = Singleton<ConstructionManager>.Instance;
			instance3.onNewConstructableBuilt = (ConstructionManager.ConstructableNotification)Delegate.Combine(instance3.onNewConstructableBuilt, new ConstructionManager.ConstructableNotification(this.OnConstructableBuilt));
			ConstructionManager instance4 = Singleton<ConstructionManager>.Instance;
			instance4.onConstructableMoved = (ConstructionManager.ConstructableNotification)Delegate.Combine(instance4.onConstructableMoved, new ConstructionManager.ConstructableNotification(this.SelectConstructable));
			this.GenerateCategories();
			this.SelectCategory(this.categories[0].categoryName);
			this.SetupListings();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), -1);
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x001531DB File Offset: 0x001513DB
		private void Exit(ExitAction exit)
		{
			if (exit.Used)
			{
				return;
			}
			if (this.selectedConstructable != null)
			{
				exit.Used = true;
				this.DeselectConstructable();
			}
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x00153201 File Offset: 0x00151401
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				this.CheckConstructableSelection();
			}
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x00153211 File Offset: 0x00151411
		private void SetupListings()
		{
			this.AddListing("small_shed", 2500f, "Multipurpose");
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x00153228 File Offset: 0x00151428
		private void AddListing(string ID, float price, string category)
		{
			if (Registry.GetConstructable(ID) == null)
			{
				Console.LogWarning("ID not valid!", null);
				return;
			}
			ConstructionMenu.ConstructionMenuCategory constructionMenuCategory = this.categories.Find((ConstructionMenu.ConstructionMenuCategory x) => x.categoryName.ToLower() == category.ToLower());
			if (constructionMenuCategory == null)
			{
				Console.LogWarning("Category not found!", null);
				return;
			}
			new ConstructionMenu.ConstructionMenuListing(ID, price, constructionMenuCategory);
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x0015328C File Offset: 0x0015148C
		private void SetIsOpen(bool open)
		{
			if (open)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			}
			else
			{
				this.DeselectConstructable();
				if (PlayerSingleton<PlayerCamera>.InstanceExists)
				{
					PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				}
			}
			this.isOpen = open;
			this.canvas.enabled = open;
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnConstructableBuilt(Constructable c)
		{
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x001532DE File Offset: 0x001514DE
		public void ClearSelectedListing()
		{
			if (this.selectedListing != null)
			{
				this.selectedListing.ListingUnselected();
				this.selectedListing = null;
				Singleton<ConstructionManager>.Instance.StopConstructableDeploy();
			}
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x00153304 File Offset: 0x00151504
		public void ListingClicked(ConstructionMenu.ConstructionMenuListing listing)
		{
			this.ClearSelectedListing();
			this.DeselectConstructable();
			Singleton<ConstructionManager>.Instance.DeployConstructable(listing);
			this.selectedListing = listing;
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x00153324 File Offset: 0x00151524
		public bool IsHoveringUI()
		{
			List<RaycastResult> list = new List<RaycastResult>();
			PointerEventData pointerEventData = new PointerEventData(this.eventSystem);
			pointerEventData.position = Input.mousePosition;
			this.raycaster.Raycast(pointerEventData, list);
			return list.Count > 0;
		}

		// Token: 0x06005036 RID: 20534 RVA: 0x00153369 File Offset: 0x00151569
		public void MoveButtonPressed()
		{
			if (this.selectedConstructable != null && this.selectedConstructable is Constructable_GridBased)
			{
				Singleton<ConstructionManager>.Instance.MoveConstructable(this.selectedConstructable as Constructable_GridBased);
				this.DeselectConstructable();
			}
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x000045B1 File Offset: 0x000027B1
		public void CustomizeButtonPressed()
		{
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x001533A4 File Offset: 0x001515A4
		public void BulldozeButtonPressed()
		{
			if (this.selectedConstructable != null)
			{
				Constructable constructable = this.selectedConstructable;
				if (!this.selectedConstructable.CanBeDestroyed())
				{
					Console.Log("Can't be destroyed!", null);
					return;
				}
				this.DeselectConstructable();
				constructable.DestroyConstructable(true);
			}
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x001533EC File Offset: 0x001515EC
		private void CheckConstructableSelection()
		{
			if (this.IsHoveringUI())
			{
				return;
			}
			if (Singleton<ConstructionManager>.Instance.isDeployingConstructable)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
			{
				Constructable hoveredConstructable = this.GetHoveredConstructable();
				if (hoveredConstructable != null)
				{
					if (this.selectedConstructable == hoveredConstructable)
					{
						this.DeselectConstructable();
						return;
					}
					this.SelectConstructable(hoveredConstructable);
					return;
				}
				else if (this.selectedConstructable != null)
				{
					this.DeselectConstructable();
				}
			}
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x00153458 File Offset: 0x00151658
		public void SelectConstructable(Constructable c)
		{
			this.SelectConstructable(c, true);
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x00153464 File Offset: 0x00151664
		public void SelectConstructable(Constructable c, bool focusCameraTo)
		{
			if (!c.CanBeSelected())
			{
				return;
			}
			if (focusCameraTo)
			{
				this.selectedConstructable = c;
			}
			Singleton<BirdsEyeView>.Instance.SlideCameraOrigin(c.GetCosmeticCenter(), c.GetBoundingBoxLongestSide() * 1.75f, 0f);
			this.infoPopup_ConstructableName.text = c.ConstructableName;
			this.infoPopup_Description.text = c.ConstructableDescription;
			List<Button> list = new List<Button>();
			if (c is Constructable_GridBased)
			{
				this.SetButtonInteractable(this.moveButton, true, this.iconColor_Unselected);
				list.Add(this.moveButton);
			}
			else
			{
				this.moveButton.gameObject.SetActive(false);
			}
			this.customizeButton.gameObject.SetActive(false);
			string empty = string.Empty;
			list.Add(this.destroyButton);
			if (c.CanBeDestroyed(out empty))
			{
				this.destroyButton.GetComponent<Tooltip>().text = "Bulldoze";
				this.SetButtonInteractable(this.destroyButton, true, new Color32(byte.MaxValue, 110, 80, byte.MaxValue));
			}
			else
			{
				this.destroyButton.GetComponent<Tooltip>().text = "Cannot bulldoze (" + empty + ")";
				this.SetButtonInteractable(this.destroyButton, false, new Color32(byte.MaxValue, 110, 80, byte.MaxValue));
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((float)(-(float)list.Count) * 50f * 0.5f + 50f * ((float)i + 0.5f), -25f);
				list[i].gameObject.SetActive(true);
			}
			if (Singleton<FeaturesManager>.Instance.activeConstructable != this.selectedConstructable)
			{
				Singleton<FeaturesManager>.Instance.Activate(this.selectedConstructable);
			}
			this.infoPopup.gameObject.SetActive(true);
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x0015364C File Offset: 0x0015184C
		private void SetButtonInteractable(Button b, bool interactable, Color32 iconDefaultColor)
		{
			b.interactable = interactable;
			if (interactable)
			{
				b.transform.Find("Outline/Background/Icon").GetComponent<Image>().color = iconDefaultColor;
				return;
			}
			b.transform.Find("Outline/Background/Icon").GetComponent<Image>().color = new Color32(200, 200, 200, byte.MaxValue);
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x001536BC File Offset: 0x001518BC
		public void DeselectConstructable()
		{
			this.selectedConstructable = null;
			this.infoPopup.gameObject.SetActive(false);
			if (Singleton<FeaturesManager>.Instance.isActive)
			{
				Singleton<FeaturesManager>.Instance.Deactivate();
			}
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x001536EC File Offset: 0x001518EC
		private Constructable GetHoveredConstructable()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(1000f, out raycastHit, 1 << LayerMask.NameToLayer("Default"), true, 0f))
			{
				return raycastHit.collider.GetComponentInParent<Constructable>();
			}
			return null;
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x00153734 File Offset: 0x00151934
		private void GenerateCategories()
		{
			for (int i = 0; i < this.categories.Count; i++)
			{
				Button component = UnityEngine.Object.Instantiate<GameObject>(this.categoryButtonPrefab, this.categoryButtonContainer).GetComponent<Button>();
				component.GetComponent<RectTransform>().anchoredPosition = new Vector2((0.5f + (float)(i % 3)) * 50f, -(0.5f + (float)(i / 3)) * 50f);
				component.transform.Find("Outline/Background/Icon").GetComponent<Image>().sprite = this.categories[i].categoryIcon;
				string catName = this.categories[i].categoryName;
				component.onClick.AddListener(delegate()
				{
					this.SelectCategory(catName);
				});
				component.GetComponent<Tooltip>().text = this.categories[i].categoryName;
				this.categories[i].button = component;
				RectTransform component2 = UnityEngine.Object.Instantiate<GameObject>(this.categoryContainerPrefab, this.categoryContainer).GetComponent<RectTransform>();
				component2.name = this.categories[i].categoryName;
				component2.gameObject.SetActive(false);
				this.categories[i].container = component2;
			}
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x00153888 File Offset: 0x00151A88
		public void SelectCategory(string categoryName)
		{
			this.ClearSelectedListing();
			ConstructionMenu.ConstructionMenuCategory constructionMenuCategory = this.categories.Find((ConstructionMenu.ConstructionMenuCategory x) => x.categoryName.ToLower() == categoryName.ToLower());
			if (this.selectedCategory != null)
			{
				this.selectedCategory.button.transform.Find("Outline/Background/Icon").GetComponent<Image>().color = this.iconColor_Unselected;
				this.selectedCategory.button.interactable = true;
				this.selectedCategory.container.gameObject.SetActive(false);
			}
			constructionMenuCategory.button.interactable = false;
			constructionMenuCategory.button.transform.Find("Outline/Background/Icon").GetComponent<Image>().color = this.iconColor_Selected;
			constructionMenuCategory.container.gameObject.SetActive(true);
			this.categoryNameDisplay.text = constructionMenuCategory.categoryName;
			this.selectedCategory = constructionMenuCategory;
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x00153974 File Offset: 0x00151B74
		public float GetListingPrice(string id)
		{
			for (int i = 0; i < this.categories.Count; i++)
			{
				for (int j = 0; j < this.categories[i].listings.Count; j++)
				{
					if (this.categories[i].listings[j].ID == id)
					{
						return this.categories[i].listings[j].price;
					}
				}
			}
			Console.LogWarning("Failed to get listing price for ID: " + id, null);
			return 0f;
		}

		// Token: 0x04003C26 RID: 15398
		public List<ConstructionMenu.ConstructionMenuCategory> categories = new List<ConstructionMenu.ConstructionMenuCategory>();

		// Token: 0x04003C27 RID: 15399
		[Header("References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x04003C28 RID: 15400
		[SerializeField]
		protected GraphicRaycaster raycaster;

		// Token: 0x04003C29 RID: 15401
		[SerializeField]
		protected Transform categoryButtonContainer;

		// Token: 0x04003C2A RID: 15402
		[SerializeField]
		protected RectTransform categoryContainer;

		// Token: 0x04003C2B RID: 15403
		[SerializeField]
		protected Text categoryNameDisplay;

		// Token: 0x04003C2C RID: 15404
		[SerializeField]
		protected RectTransform infoPopup;

		// Token: 0x04003C2D RID: 15405
		[SerializeField]
		protected TextMeshProUGUI infoPopup_ConstructableName;

		// Token: 0x04003C2E RID: 15406
		[SerializeField]
		protected EventSystem eventSystem;

		// Token: 0x04003C2F RID: 15407
		[SerializeField]
		protected Button destroyButton;

		// Token: 0x04003C30 RID: 15408
		[SerializeField]
		protected Button customizeButton;

		// Token: 0x04003C31 RID: 15409
		[SerializeField]
		protected Button moveButton;

		// Token: 0x04003C32 RID: 15410
		[SerializeField]
		protected TextMeshProUGUI infoPopup_Description;

		// Token: 0x04003C33 RID: 15411
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject categoryButtonPrefab;

		// Token: 0x04003C34 RID: 15412
		[SerializeField]
		protected GameObject categoryContainerPrefab;

		// Token: 0x04003C35 RID: 15413
		public GameObject listingPrefab;

		// Token: 0x04003C36 RID: 15414
		[Header("Settings")]
		[SerializeField]
		protected Color iconColor_Unselected;

		// Token: 0x04003C37 RID: 15415
		[SerializeField]
		protected Color iconColor_Selected;

		// Token: 0x04003C38 RID: 15416
		public Color listingOutlineColor_Unselected;

		// Token: 0x04003C39 RID: 15417
		public Color listingOutlineColor_Selected;

		// Token: 0x04003C3A RID: 15418
		private ConstructionMenu.ConstructionMenuCategory selectedCategory;

		// Token: 0x04003C3B RID: 15419
		private ConstructionMenu.ConstructionMenuListing selectedListing;

		// Token: 0x04003C3C RID: 15420
		private Constructable selectedConstructable;

		// Token: 0x02000BCD RID: 3021
		[Serializable]
		public class ConstructionMenuCategory
		{
			// Token: 0x04003C3D RID: 15421
			public string categoryName = "Category";

			// Token: 0x04003C3E RID: 15422
			public Sprite categoryIcon;

			// Token: 0x04003C3F RID: 15423
			[HideInInspector]
			public Button button;

			// Token: 0x04003C40 RID: 15424
			[HideInInspector]
			public RectTransform container;

			// Token: 0x04003C41 RID: 15425
			[HideInInspector]
			public List<ConstructionMenu.ConstructionMenuListing> listings = new List<ConstructionMenu.ConstructionMenuListing>();
		}

		// Token: 0x02000BCE RID: 3022
		public class ConstructionMenuListing
		{
			// Token: 0x06005046 RID: 20550 RVA: 0x00153A52 File Offset: 0x00151C52
			public ConstructionMenuListing(string id, float _price, ConstructionMenu.ConstructionMenuCategory _cat)
			{
				this.ID = id;
				this.price = _price;
				this.category = _cat;
				this.category.listings.Add(this);
				this.CreateUI();
			}

			// Token: 0x06005047 RID: 20551 RVA: 0x00153A94 File Offset: 0x00151C94
			private void CreateUI()
			{
				int num = this.category.listings.IndexOf(this);
				this.entry = UnityEngine.Object.Instantiate<GameObject>(Singleton<ConstructionMenu>.Instance.listingPrefab, this.category.container).GetComponent<RectTransform>();
				this.entry.anchoredPosition = new Vector2((0.5f + (float)num) * this.entry.sizeDelta.x, this.entry.anchoredPosition.y);
				this.entry.Find("Content/Icon").GetComponent<Image>().sprite = Registry.GetConstructable(this.ID).ConstructableIcon;
				this.entry.Find("Content/Name").GetComponent<Text>().text = Registry.GetConstructable(this.ID).ConstructableName;
				this.entry.Find("Content/Price").GetComponent<Text>().text = MoneyManager.FormatAmount(this.price, false, false);
				this.entry.GetComponent<Button>().onClick.AddListener(new UnityAction(this.ListingClicked));
			}

			// Token: 0x06005048 RID: 20552 RVA: 0x00153BAD File Offset: 0x00151DAD
			private void ListingClicked()
			{
				if (this.isSelected)
				{
					Singleton<ConstructionMenu>.Instance.ClearSelectedListing();
					return;
				}
				Singleton<ConstructionMenu>.Instance.ListingClicked(this);
				this.SetSelected(true);
			}

			// Token: 0x06005049 RID: 20553 RVA: 0x00153BD4 File Offset: 0x00151DD4
			public void ListingUnselected()
			{
				this.SetSelected(false);
			}

			// Token: 0x0600504A RID: 20554 RVA: 0x00153BE0 File Offset: 0x00151DE0
			public void SetSelected(bool selected)
			{
				this.isSelected = selected;
				if (selected)
				{
					this.entry.Find("Content/Outline").GetComponent<Image>().color = Singleton<ConstructionMenu>.Instance.listingOutlineColor_Selected;
					this.entry.Find("Content/Name").GetComponent<Text>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
					return;
				}
				this.entry.Find("Content/Outline").GetComponent<Image>().color = Singleton<ConstructionMenu>.Instance.listingOutlineColor_Unselected;
				this.entry.Find("Content/Name").GetComponent<Text>().color = new Color32(50, 50, 50, byte.MaxValue);
			}

			// Token: 0x04003C42 RID: 15426
			public string ID = string.Empty;

			// Token: 0x04003C43 RID: 15427
			public float price;

			// Token: 0x04003C44 RID: 15428
			public ConstructionMenu.ConstructionMenuCategory category;

			// Token: 0x04003C45 RID: 15429
			public RectTransform entry;

			// Token: 0x04003C46 RID: 15430
			public bool isSelected;
		}
	}
}
