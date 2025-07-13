using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dragging;
using ScheduleOne.EntityFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Storage;
using ScheduleOne.UI;
using ScheduleOne.UI.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ScheduleOne.Interaction
{
	// Token: 0x0200064F RID: 1615
	public class InteractionManager : Singleton<InteractionManager>
	{
		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x060029D7 RID: 10711 RVA: 0x000ACEB7 File Offset: 0x000AB0B7
		public LayerMask Interaction_SearchMask
		{
			get
			{
				return this.interaction_SearchMask;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x060029D8 RID: 10712 RVA: 0x000ACEBF File Offset: 0x000AB0BF
		// (set) Token: 0x060029D9 RID: 10713 RVA: 0x000ACEC7 File Offset: 0x000AB0C7
		public bool CanDestroy { get; set; } = true;

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x060029DA RID: 10714 RVA: 0x000ACED0 File Offset: 0x000AB0D0
		// (set) Token: 0x060029DB RID: 10715 RVA: 0x000ACED8 File Offset: 0x000AB0D8
		public InteractableObject hoveredInteractableObject { get; protected set; }

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060029DC RID: 10716 RVA: 0x000ACEE1 File Offset: 0x000AB0E1
		// (set) Token: 0x060029DD RID: 10717 RVA: 0x000ACEE9 File Offset: 0x000AB0E9
		public InteractableObject hoveredValidInteractableObject { get; protected set; }

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x000ACEF2 File Offset: 0x000AB0F2
		// (set) Token: 0x060029DF RID: 10719 RVA: 0x000ACEFA File Offset: 0x000AB0FA
		public InteractableObject interactedObject { get; protected set; }

		// Token: 0x060029E0 RID: 10720 RVA: 0x000ACF04 File Offset: 0x000AB104
		protected override void Start()
		{
			base.Start();
			this.LoadInteractKey();
			Settings instance = Singleton<Settings>.Instance;
			instance.onInputsApplied = (Action)Delegate.Remove(instance.onInputsApplied, new Action(this.LoadInteractKey));
			Settings instance2 = Singleton<Settings>.Instance;
			instance2.onInputsApplied = (Action)Delegate.Combine(instance2.onInputsApplied, new Action(this.LoadInteractKey));
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x000ACF69 File Offset: 0x000AB169
		protected override void OnDestroy()
		{
			if (Singleton<Settings>.InstanceExists)
			{
				Settings instance = Singleton<Settings>.Instance;
				instance.onInputsApplied = (Action)Delegate.Remove(instance.onInputsApplied, new Action(this.LoadInteractKey));
			}
			base.OnDestroy();
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x000ACFA0 File Offset: 0x000AB1A0
		private void LoadInteractKey()
		{
			string text;
			string controlPath;
			InputActionRebindingExtensions.GetBindingDisplayString(this.InteractInput.action, 0, ref text, ref controlPath, 0);
			this.InteractKey = Singleton<InputPromptsManager>.Instance.GetDisplayNameForControlPath(controlPath);
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x000ACFD5 File Offset: 0x000AB1D5
		protected virtual void Update()
		{
			this.timeSinceLastInteractStart += Time.deltaTime;
			if (Singleton<GameInput>.InstanceExists)
			{
				this.CheckRightClick();
			}
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x000ACFF8 File Offset: 0x000AB1F8
		protected virtual void LateUpdate()
		{
			if (!Singleton<GameInput>.InstanceExists)
			{
				return;
			}
			this.interactionDisplayEnabledThisFrame = false;
			this.CheckHover();
			if (this.hoveredInteractableObject != null)
			{
				this.hoveredInteractableObject.Hovered();
			}
			this.CheckInteraction();
			this.interaction_Canvas.enabled = (this.interactionDisplayEnabledThisFrame || this.activeWSlabels.Count > 0);
			this.interactionDisplay_Container.gameObject.SetActive(this.interactionDisplayEnabledThisFrame);
			if (!this.interactionDisplayEnabledThisFrame)
			{
				this.tempDisplayScale = 0.75f;
			}
			for (int i = 0; i < this.activeWSlabels.Count; i++)
			{
				this.activeWSlabels[i].RefreshDisplay();
			}
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x000AD0B0 File Offset: 0x000AB2B0
		protected virtual void CheckHover()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (Singleton<TaskManager>.InstanceExists && Singleton<TaskManager>.Instance.currentTask != null)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (Singleton<ObjectSelector>.InstanceExists && Singleton<ObjectSelector>.Instance.isSelecting)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (Singleton<GameplayMenu>.InstanceExists && Singleton<GameplayMenu>.Instance.IsOpen)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (PlayerSingleton<PlayerMovement>.Instance.currentVehicle != null)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount > 0)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerInventory>.Instance.equippable != null && !PlayerSingleton<PlayerInventory>.Instance.equippable.CanInteractWhenEquipped)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (Player.Local.IsSkating)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (NetworkSingleton<DragManager>.Instance.IsDragging)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			Ray ray = default(Ray);
			EInteractionSearchType einteractionSearchType = this.interactionSearchType;
			if (einteractionSearchType != EInteractionSearchType.CameraForward)
			{
				if (einteractionSearchType != EInteractionSearchType.Mouse)
				{
					Console.LogWarning("EInteractionSearchType type not accounted for", null);
					return;
				}
				ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			}
			else
			{
				ray.origin = PlayerSingleton<PlayerCamera>.Instance.transform.position;
				ray.direction = PlayerSingleton<PlayerCamera>.Instance.transform.forward;
			}
			InteractableObject hoveredInteractableObject = this.hoveredInteractableObject;
			this.hoveredInteractableObject = null;
			RaycastHit[] array = Physics.SphereCastAll(ray, 0.075f, 5f, this.interaction_SearchMask, 2);
			RaycastHit[] array2 = Physics.RaycastAll(ray, 5f, this.interaction_SearchMask, 2);
			if (array.Length != 0)
			{
				Array.Sort<RaycastHit>(array, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
				List<InteractableObject> list = new List<InteractableObject>();
				Dictionary<InteractableObject, RaycastHit> objectHits = new Dictionary<InteractableObject, RaycastHit>();
				foreach (RaycastHit value in array)
				{
					InteractableObject componentInParent = value.collider.GetComponentInParent<InteractableObject>();
					if (componentInParent == null)
					{
						bool flag = false;
						foreach (RaycastHit raycastHit in array2)
						{
							if (raycastHit.collider == value.collider)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					else if (!list.Contains(componentInParent) && componentInParent != null && Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, value.point) <= componentInParent.MaxInteractionRange)
					{
						list.Add(componentInParent);
						objectHits.Add(componentInParent, value);
					}
				}
				list.Sort(delegate(InteractableObject x, InteractableObject y)
				{
					int num = y.Priority.CompareTo(x.Priority);
					if (num == 0)
					{
						return objectHits[x].distance.CompareTo(objectHits[y].distance);
					}
					return num;
				});
				for (int k = 0; k < list.Count; k++)
				{
					RaycastHit raycastHit2 = objectHits[list[k]];
					InteractableObject interactableObject = list[k];
					if (interactableObject == null)
					{
						bool flag2 = false;
						foreach (RaycastHit raycastHit3 in array2)
						{
							if (raycastHit3.collider == raycastHit2.collider)
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							break;
						}
					}
					else
					{
						if (!interactableObject.CheckAngleLimit(ray.origin))
						{
							interactableObject = null;
						}
						if (interactableObject != null && !interactableObject.enabled)
						{
							interactableObject = null;
						}
						if (interactableObject != null && Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, raycastHit2.point) <= interactableObject.MaxInteractionRange)
						{
							this.hoveredInteractableObject = interactableObject;
							if (interactableObject != hoveredInteractableObject)
							{
								this.tempDisplayScale = 1f;
								break;
							}
							break;
						}
					}
				}
			}
			if (this.DEBUG)
			{
				string str = "Hovered interactable object: ";
				InteractableObject hoveredInteractableObject2 = this.hoveredInteractableObject;
				Debug.Log(str + ((hoveredInteractableObject2 != null) ? hoveredInteractableObject2.name : null));
			}
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x000AD4C8 File Offset: 0x000AB6C8
		protected virtual void CheckInteraction()
		{
			this.hoveredValidInteractableObject = null;
			if (this.interactedObject != null && ((this.interactedObject._interactionType == InteractableObject.EInteractionType.Key_Press && !GameInput.GetButton(GameInput.ButtonCode.Interact)) || (this.interactedObject._interactionType == InteractableObject.EInteractionType.LeftMouse_Click && !GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))))
			{
				this.interactedObject.EndInteract();
				this.interactedObject = null;
			}
			if (this.hoveredInteractableObject == null)
			{
				return;
			}
			if (this.hoveredInteractableObject._interactionState == InteractableObject.EInteractableState.Disabled)
			{
				return;
			}
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				return;
			}
			this.hoveredValidInteractableObject = this.hoveredInteractableObject;
			if (GameInput.GetButton(GameInput.ButtonCode.Interact) && this.timeSinceLastInteractStart >= InteractionManager.interactCooldown && this.hoveredInteractableObject._interactionType == InteractableObject.EInteractionType.Key_Press && (!this.hoveredInteractableObject.RequiresUniqueClick || GameInput.GetButtonDown(GameInput.ButtonCode.Interact)))
			{
				this.timeSinceLastInteractStart = 0f;
				this.hoveredInteractableObject.StartInteract();
				this.interactedObject = this.hoveredInteractableObject;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && this.timeSinceLastInteractStart >= InteractionManager.interactCooldown && this.hoveredInteractableObject._interactionType == InteractableObject.EInteractionType.LeftMouse_Click && (!this.hoveredInteractableObject.RequiresUniqueClick || GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick)))
			{
				this.timeSinceLastInteractStart = 0f;
				this.hoveredInteractableObject.StartInteract();
				this.interactedObject = this.hoveredInteractableObject;
			}
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x000AD620 File Offset: 0x000AB820
		protected virtual void CheckRightClick()
		{
			bool flag = false;
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (Singleton<TaskManager>.Instance.currentTask == null && (!PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped || (PlayerSingleton<PlayerInventory>.Instance.equippable != null && PlayerSingleton<PlayerInventory>.Instance.equippable.CanInteractWhenEquipped && PlayerSingleton<PlayerInventory>.Instance.equippable.CanPickUpWhenEquipped)) && PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0 && this.CanDestroy && GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
			{
				BuildableItem hoveredBuildableItem = this.GetHoveredBuildableItem();
				this.GetHoveredPallet();
				this.GetHoveredConstructable();
				if (hoveredBuildableItem != null)
				{
					string text;
					if (hoveredBuildableItem.CanBePickedUp(out text))
					{
						if (this.itemBeingDestroyed == hoveredBuildableItem)
						{
							this.destroyTime += Time.deltaTime;
						}
						this.itemBeingDestroyed = hoveredBuildableItem;
						if (this.destroyTime >= InteractionManager.timeToDestroy)
						{
							this.itemBeingDestroyed.PickupItem();
							this.destroyTime = 0f;
						}
						flag = true;
						Singleton<HUD>.Instance.ShowRadialIndicator(this.destroyTime / InteractionManager.timeToDestroy);
					}
					else
					{
						Singleton<HUD>.Instance.CrosshairText.Show(text, new Color32(byte.MaxValue, 100, 100, byte.MaxValue));
					}
				}
			}
			if (!flag)
			{
				this.destroyTime = 0f;
			}
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x000AD77C File Offset: 0x000AB97C
		protected virtual BuildableItem GetHoveredBuildableItem()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.rightClickRange, out raycastHit, 1 << LayerMask.NameToLayer("Default"), true, 0f))
			{
				return raycastHit.collider.GetComponentInParent<BuildableItem>();
			}
			return null;
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x000AD7C8 File Offset: 0x000AB9C8
		protected virtual Pallet GetHoveredPallet()
		{
			LayerMask layerMask = default(LayerMask) | 1 << LayerMask.NameToLayer("Default");
			layerMask |= 1 << LayerMask.NameToLayer("Pallet");
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.rightClickRange, out raycastHit, layerMask, true, 0f))
			{
				return raycastHit.collider.GetComponentInParent<Pallet>();
			}
			return null;
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x000AD840 File Offset: 0x000ABA40
		protected virtual Constructable GetHoveredConstructable()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.rightClickRange, out raycastHit, 1 << LayerMask.NameToLayer("Default"), true, 0f))
			{
				return raycastHit.collider.GetComponentInParent<Constructable>();
			}
			return null;
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x000AD889 File Offset: 0x000ABA89
		public void SetCanDestroy(bool canDestroy)
		{
			this.CanDestroy = canDestroy;
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x000AD894 File Offset: 0x000ABA94
		public void EnableInteractionDisplay(Vector3 pos, Sprite icon, string spriteText, string message, Color messageColor, Color iconColor)
		{
			this.interactionDisplayEnabledThisFrame = true;
			this.interactionDisplay_Container.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(pos);
			this.interactionDisplay_Icon.gameObject.SetActive(icon != null);
			this.interactionDisplay_Icon.sprite = icon;
			this.interactionDisplay_Icon.color = iconColor;
			this.interactionDisplay_IconText.enabled = (spriteText != string.Empty);
			this.interactionDisplay_IconText.text = spriteText.ToUpper();
			this.interactionDisplay_MessageText.text = message;
			this.interactionDisplay_MessageText.color = messageColor;
			this.interactionDisplay_Container.sizeDelta = new Vector2(60f + this.interactionDisplay_MessageText.preferredWidth, this.interactionDisplay_Container.sizeDelta.y);
			this.backgroundImage.sizeDelta = new Vector2(this.interactionDisplay_MessageText.preferredWidth + 180f, 140f);
			float num = Mathf.Clamp(1f / Vector3.Distance(pos, PlayerSingleton<PlayerCamera>.Instance.transform.position), 0f, 1f) * this.tempDisplayScale * this.displaySizeMultiplier;
			this.interactionDisplay_Container.localScale = new Vector3(num, num, 1f);
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x000AD9DB File Offset: 0x000ABBDB
		public void LerpDisplayScale(float endScale)
		{
			if (this.ILerpDisplayScale_Coroutine != null)
			{
				base.StopCoroutine(this.ILerpDisplayScale_Coroutine);
			}
			this.ILerpDisplayScale_Coroutine = base.StartCoroutine(this.ILerpDisplayScale(this.tempDisplayScale, endScale));
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x000ADA0A File Offset: 0x000ABC0A
		protected IEnumerator ILerpDisplayScale(float startScale, float endScale)
		{
			float lerpTime = Mathf.Abs(startScale - endScale) * 0.75f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.tempDisplayScale = Mathf.Lerp(startScale, endScale, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.tempDisplayScale = endScale;
			this.ILerpDisplayScale_Coroutine = null;
			yield break;
		}

		// Token: 0x04001E69 RID: 7785
		public const float RayRadius = 0.075f;

		// Token: 0x04001E6A RID: 7786
		public const float MaxInteractionRange = 5f;

		// Token: 0x04001E6B RID: 7787
		[SerializeField]
		protected LayerMask interaction_SearchMask;

		// Token: 0x04001E6C RID: 7788
		[SerializeField]
		protected float rightClickRange = 5f;

		// Token: 0x04001E6D RID: 7789
		public EInteractionSearchType interactionSearchType;

		// Token: 0x04001E6E RID: 7790
		public bool DEBUG;

		// Token: 0x04001E70 RID: 7792
		[Header("Visuals Settings")]
		public Color messageColor_Default;

		// Token: 0x04001E71 RID: 7793
		public Color iconColor_Default;

		// Token: 0x04001E72 RID: 7794
		public Color iconColor_Default_Key;

		// Token: 0x04001E73 RID: 7795
		public Color messageColor_Invalid;

		// Token: 0x04001E74 RID: 7796
		public Color iconColor_Invalid;

		// Token: 0x04001E75 RID: 7797
		public Sprite icon_Key;

		// Token: 0x04001E76 RID: 7798
		public Sprite icon_LeftMouse;

		// Token: 0x04001E77 RID: 7799
		public Sprite icon_Cross;

		// Token: 0x04001E78 RID: 7800
		public float displaySizeMultiplier = 1f;

		// Token: 0x04001E79 RID: 7801
		[Header("References")]
		[SerializeField]
		protected Canvas interaction_Canvas;

		// Token: 0x04001E7A RID: 7802
		[SerializeField]
		protected RectTransform interactionDisplay_Container;

		// Token: 0x04001E7B RID: 7803
		[SerializeField]
		protected Image interactionDisplay_Icon;

		// Token: 0x04001E7C RID: 7804
		[SerializeField]
		protected Text interactionDisplay_IconText;

		// Token: 0x04001E7D RID: 7805
		[SerializeField]
		protected Text interactionDisplay_MessageText;

		// Token: 0x04001E7E RID: 7806
		public RectTransform wsLabelContainer;

		// Token: 0x04001E7F RID: 7807
		[SerializeField]
		protected InputActionReference InteractInput;

		// Token: 0x04001E80 RID: 7808
		[HideInInspector]
		public string InteractKey = string.Empty;

		// Token: 0x04001E81 RID: 7809
		[SerializeField]
		protected RectTransform backgroundImage;

		// Token: 0x04001E82 RID: 7810
		[Header("Prefabs")]
		public GameObject WSLabelPrefab;

		// Token: 0x04001E86 RID: 7814
		private bool interactionDisplayEnabledThisFrame;

		// Token: 0x04001E87 RID: 7815
		private BuildableItem itemBeingDestroyed;

		// Token: 0x04001E88 RID: 7816
		private Pallet palletBeingDestroyed;

		// Token: 0x04001E89 RID: 7817
		private Constructable constructableBeingDestroyed;

		// Token: 0x04001E8A RID: 7818
		private float destroyTime;

		// Token: 0x04001E8B RID: 7819
		private float tempDisplayScale = 0.75f;

		// Token: 0x04001E8C RID: 7820
		public static float interactCooldown = 0.1f;

		// Token: 0x04001E8D RID: 7821
		private float timeSinceLastInteractStart;

		// Token: 0x04001E8E RID: 7822
		public List<WorldSpaceLabel> activeWSlabels = new List<WorldSpaceLabel>();

		// Token: 0x04001E8F RID: 7823
		private static float timeToDestroy = 0.5f;

		// Token: 0x04001E90 RID: 7824
		private Coroutine ILerpDisplayScale_Coroutine;
	}
}
