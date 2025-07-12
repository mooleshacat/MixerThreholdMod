using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000535 RID: 1333
	public class MoveItemBehaviour : Behaviour
	{
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x0007E33D File Offset: 0x0007C53D
		// (set) Token: 0x06001E91 RID: 7825 RVA: 0x0007E345 File Offset: 0x0007C545
		public bool Initialized { get; protected set; }

		// Token: 0x06001E92 RID: 7826 RVA: 0x0007E350 File Offset: 0x0007C550
		public void Initialize(TransitRoute route, ItemInstance _itemToRetrieveTemplate, int _maxMoveAmount = -1, bool _skipPickup = false)
		{
			string str;
			if (!this.IsTransitRouteValid(route, _itemToRetrieveTemplate, out str))
			{
				Console.LogError("Invalid transit route for move item behaviour! Reason: " + str, null);
				return;
			}
			this.assignedRoute = route;
			this.itemToRetrieveTemplate = _itemToRetrieveTemplate;
			this.maxMoveAmount = _maxMoveAmount;
			if (base.Npc.behaviour.DEBUG_MODE)
			{
				Console.Log(string.Concat(new string[]
				{
					"MoveItemBehaviour initialized with route: ",
					route.Source.Name,
					" -> ",
					route.Destination.Name,
					" for item: ",
					_itemToRetrieveTemplate.ID
				}), null);
			}
			this.skipPickup = _skipPickup;
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x0007E3F8 File Offset: 0x0007C5F8
		public void Resume(TransitRoute route, ItemInstance _itemToRetrieveTemplate, int _maxMoveAmount = -1)
		{
			this.assignedRoute = route;
			this.itemToRetrieveTemplate = _itemToRetrieveTemplate;
			this.maxMoveAmount = _maxMoveAmount;
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x0007E40F File Offset: 0x0007C60F
		protected override void Begin()
		{
			base.Begin();
			this.StartTransit();
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x0007E41D File Offset: 0x0007C61D
		protected override void Pause()
		{
			base.Pause();
			this.StopCurrentActivity();
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x0007E42B File Offset: 0x0007C62B
		protected override void Resume()
		{
			base.Resume();
			this.StartTransit();
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x0007E439 File Offset: 0x0007C639
		protected override void End()
		{
			base.End();
			this.skipPickup = false;
			this.EndTransit();
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x0007C40F File Offset: 0x0007A60F
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0007E450 File Offset: 0x0007C650
		private void StartTransit()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.Npc.Inventory.GetIdenticalItemAmount(this.itemToRetrieveTemplate) == 0)
			{
				string text;
				if (!this.IsTransitRouteValid(this.assignedRoute, this.itemToRetrieveTemplate, out text))
				{
					Console.LogWarning("Invalid transit route for move item behaviour!", null);
					base.Disable_Networked(null);
					return;
				}
			}
			else
			{
				ItemInstance firstIdenticalItem = base.Npc.Inventory.GetFirstIdenticalItem(this.itemToRetrieveTemplate, new NPCInventory.ItemFilter(this.IsNpcInventoryItemValid));
				if (base.Npc.behaviour.DEBUG_MODE)
				{
					string str = "Moving item: ";
					ItemInstance itemInstance = firstIdenticalItem;
					Console.Log(str + ((itemInstance != null) ? itemInstance.ToString() : null), null);
				}
				if (!this.IsDestinationValid(this.assignedRoute, firstIdenticalItem))
				{
					Console.LogWarning("Invalid transit route for move item behaviour!", null);
					base.Disable_Networked(null);
					return;
				}
			}
			this.currentState = MoveItemBehaviour.EState.Idle;
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0007E524 File Offset: 0x0007C724
		private bool IsNpcInventoryItemValid(ItemInstance item)
		{
			return this.assignedRoute.Destination.GetInputCapacityForItem(item, base.Npc, true) != 0;
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0007E544 File Offset: 0x0007C744
		private void EndTransit()
		{
			this.StopCurrentActivity();
			if (this.assignedRoute != null && base.Npc != null && this.assignedRoute.Destination != null)
			{
				this.assignedRoute.Destination.RemoveSlotLocks(base.Npc.NetworkObject);
			}
			this.Initialized = false;
			this.assignedRoute = null;
			this.itemToRetrieveTemplate = null;
			this.grabbedAmount = 0;
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0007E5B4 File Offset: 0x0007C7B4
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.assignedRoute.AreEntitiesNonNull())
			{
				Console.LogWarning("Transit route entities are null!", null);
				base.Disable_Networked(null);
				return;
			}
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("State: " + this.currentState.ToString(), null);
				Console.Log("Moving: " + base.Npc.Movement.IsMoving.ToString(), null);
			}
			if (this.currentState == MoveItemBehaviour.EState.Idle)
			{
				if (base.Npc.Inventory.GetIdenticalItemAmount(this.itemToRetrieveTemplate) > 0 && this.grabbedAmount > 0)
				{
					if (this.IsAtDestination())
					{
						this.PlaceItem();
						return;
					}
					this.WalkToDestination();
					return;
				}
				else
				{
					if (this.skipPickup)
					{
						this.TakeItem();
						this.skipPickup = false;
						return;
					}
					if (this.IsAtSource())
					{
						this.GrabItem();
						return;
					}
					this.WalkToSource();
				}
			}
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0007E6B0 File Offset: 0x0007C8B0
		public void WalkToSource()
		{
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("MoveItemBehaviour.WalkToSource", null);
			}
			if (!base.Npc.Movement.CanGetTo(this.GetSourceAccessPoint(this.assignedRoute).position, 1f))
			{
				Console.LogWarning("MoveItemBehaviour.WalkToSource: Can't get to source", null);
				base.Disable_Networked(null);
				return;
			}
			this.currentState = MoveItemBehaviour.EState.WalkingToSource;
			this.walkToSourceRoutine = base.StartCoroutine(this.<WalkToSource>g__Routine|26_0());
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x0007E729 File Offset: 0x0007C929
		public void GrabItem()
		{
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("MoveItemBehaviour.GrabItem", null);
			}
			this.currentState = MoveItemBehaviour.EState.Grabbing;
			this.grabRoutine = base.StartCoroutine(this.<GrabItem>g__Routine|27_0());
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x0007E75C File Offset: 0x0007C95C
		private void TakeItem()
		{
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("MoveItemBehaviour.TakeItem", null);
			}
			int amountToGrab = this.GetAmountToGrab();
			if (amountToGrab == 0)
			{
				Console.LogWarning("Amount to grab is 0!", null);
				return;
			}
			ItemSlot firstSlotContainingTemplateItem = this.assignedRoute.Source.GetFirstSlotContainingTemplateItem(this.itemToRetrieveTemplate, ITransitEntity.ESlotType.Output);
			ItemInstance copy = ((firstSlotContainingTemplateItem != null) ? firstSlotContainingTemplateItem.ItemInstance : null).GetCopy(amountToGrab);
			this.grabbedAmount = amountToGrab;
			firstSlotContainingTemplateItem.ChangeQuantity(-amountToGrab, false);
			base.Npc.Inventory.InsertItem(copy, true);
			this.assignedRoute.Destination.ReserveInputSlotsForItem(copy, base.Npc.NetworkObject);
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x0007E800 File Offset: 0x0007CA00
		public void WalkToDestination()
		{
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("MoveItemBehaviour.WalkToDestination", null);
			}
			if (!base.Npc.Movement.CanGetTo(this.GetDestinationAccessPoint(this.assignedRoute).position, 1f))
			{
				Console.LogWarning("MoveItemBehaviour.WalkToDestination: Can't get to destination", null);
				base.Disable_Networked(null);
				return;
			}
			this.currentState = MoveItemBehaviour.EState.WalkingToDestination;
			this.walkToDestinationRoutine = base.StartCoroutine(this.<WalkToDestination>g__Routine|29_0());
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0007E879 File Offset: 0x0007CA79
		public void PlaceItem()
		{
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("MoveItemBehaviour.PlaceItem", null);
			}
			this.currentState = MoveItemBehaviour.EState.Placing;
			this.placingRoutine = base.StartCoroutine(this.<PlaceItem>g__Routine|30_0());
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x0007E8AC File Offset: 0x0007CAAC
		private int GetAmountToGrab()
		{
			ItemSlot firstSlotContainingTemplateItem = this.assignedRoute.Source.GetFirstSlotContainingTemplateItem(this.itemToRetrieveTemplate, ITransitEntity.ESlotType.Output);
			ItemInstance itemInstance = (firstSlotContainingTemplateItem != null) ? firstSlotContainingTemplateItem.ItemInstance : null;
			if (itemInstance == null)
			{
				return 0;
			}
			int num = itemInstance.Quantity;
			if (this.maxMoveAmount > 0)
			{
				num = Mathf.Min(this.maxMoveAmount, num);
			}
			int inputCapacityForItem = this.assignedRoute.Destination.GetInputCapacityForItem(itemInstance, base.Npc, true);
			return Mathf.Min(num, inputCapacityForItem);
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0007E924 File Offset: 0x0007CB24
		private void StopCurrentActivity()
		{
			switch (this.currentState)
			{
			case MoveItemBehaviour.EState.WalkingToSource:
				if (this.walkToSourceRoutine != null)
				{
					base.StopCoroutine(this.walkToSourceRoutine);
				}
				break;
			case MoveItemBehaviour.EState.Grabbing:
				if (this.grabRoutine != null)
				{
					base.StopCoroutine(this.grabRoutine);
				}
				break;
			case MoveItemBehaviour.EState.WalkingToDestination:
				if (this.walkToDestinationRoutine != null)
				{
					base.StopCoroutine(this.walkToDestinationRoutine);
				}
				break;
			case MoveItemBehaviour.EState.Placing:
				if (this.placingRoutine != null)
				{
					base.StopCoroutine(this.placingRoutine);
				}
				break;
			}
			this.currentState = MoveItemBehaviour.EState.Idle;
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x0007E9B4 File Offset: 0x0007CBB4
		public bool IsTransitRouteValid(TransitRoute route, string itemID, out string invalidReason)
		{
			invalidReason = string.Empty;
			if (route == null)
			{
				invalidReason = "Route is null!";
				return false;
			}
			if (!route.AreEntitiesNonNull())
			{
				invalidReason = "Entities are null!";
				return false;
			}
			ItemSlot firstSlotContainingItem = route.Source.GetFirstSlotContainingItem(itemID, ITransitEntity.ESlotType.Output);
			ItemInstance itemInstance = (firstSlotContainingItem != null) ? firstSlotContainingItem.ItemInstance : null;
			if (itemInstance == null || itemInstance.Quantity <= 0)
			{
				invalidReason = "Item is null or quantity is 0!";
				return false;
			}
			if (!this.IsDestinationValid(route, itemInstance))
			{
				invalidReason = "Can't access source, destination or destination is full!";
				return false;
			}
			if (base.Npc.Inventory.GetCapacityForItem(itemInstance) == 0)
			{
				invalidReason = "Npc inventory doesn't have capacity!";
				return false;
			}
			return true;
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x0007EA44 File Offset: 0x0007CC44
		public bool IsTransitRouteValid(TransitRoute route, ItemInstance templateItem, out string invalidReason)
		{
			invalidReason = string.Empty;
			if (route == null)
			{
				invalidReason = "Route is null!";
				return false;
			}
			if (!route.AreEntitiesNonNull())
			{
				invalidReason = "Entities are null!";
				return false;
			}
			ItemSlot firstSlotContainingTemplateItem = route.Source.GetFirstSlotContainingTemplateItem(templateItem, ITransitEntity.ESlotType.Output);
			ItemInstance itemInstance = (firstSlotContainingTemplateItem != null) ? firstSlotContainingTemplateItem.ItemInstance : null;
			if (itemInstance == null || itemInstance.Quantity <= 0)
			{
				invalidReason = "Item is null or quantity is 0!";
				return false;
			}
			if (!this.IsDestinationValid(route, itemInstance))
			{
				invalidReason = "Can't access source, destination or destination is full!";
				return false;
			}
			return true;
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x0007EAB8 File Offset: 0x0007CCB8
		public bool IsTransitRouteValid(TransitRoute route, string itemID)
		{
			string text;
			return this.IsTransitRouteValid(route, itemID, out text);
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x0007EAD0 File Offset: 0x0007CCD0
		public bool IsDestinationValid(TransitRoute route, ItemInstance item)
		{
			if (route.Destination.GetInputCapacityForItem(item, base.Npc, true) == 0)
			{
				Console.LogWarning("Destination has no capacity for item!", null);
				return false;
			}
			if (!this.CanGetToDestination(route))
			{
				Console.LogWarning("Cannot get to destination!", null);
				return false;
			}
			if (!this.CanGetToSource(route))
			{
				Console.LogWarning("Cannot get to source!", null);
				return false;
			}
			return true;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x0007EB2C File Offset: 0x0007CD2C
		public bool CanGetToSource(TransitRoute route)
		{
			return this.GetSourceAccessPoint(route) != null;
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x0007EB3B File Offset: 0x0007CD3B
		private Transform GetSourceAccessPoint(TransitRoute route)
		{
			return NavMeshUtility.GetAccessPoint(route.Source, base.Npc);
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x0007EB4E File Offset: 0x0007CD4E
		private bool IsAtSource()
		{
			return NavMeshUtility.IsAtTransitEntity(this.assignedRoute.Source, base.Npc, 0.4f);
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x0007EB6B File Offset: 0x0007CD6B
		public bool CanGetToDestination(TransitRoute route)
		{
			return this.GetDestinationAccessPoint(route) != null;
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x0007EB7A File Offset: 0x0007CD7A
		private Transform GetDestinationAccessPoint(TransitRoute route)
		{
			if (route.Destination == null)
			{
				Console.LogWarning("Destination is null!", null);
				return null;
			}
			return NavMeshUtility.GetAccessPoint(route.Destination, base.Npc);
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x0007EBA4 File Offset: 0x0007CDA4
		private bool IsAtDestination()
		{
			if (base.beh.DEBUG_MODE)
			{
				ITransitEntity destination = this.assignedRoute.Destination;
				Console.Log("Destination: " + destination.Name, null);
				foreach (Transform transform in destination.AccessPoints)
				{
					Debug.DrawLine(base.Npc.transform.position, transform.position, Color.red, 0.1f);
				}
			}
			return NavMeshUtility.IsAtTransitEntity(this.assignedRoute.Destination, base.Npc, 0.4f);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0007EC3C File Offset: 0x0007CE3C
		public MoveItemData GetSaveData()
		{
			if (!base.Active || this.grabbedAmount == 0)
			{
				return null;
			}
			string templateItemJson = string.Empty;
			if (this.itemToRetrieveTemplate != null)
			{
				templateItemJson = this.itemToRetrieveTemplate.GetItemData().GetJson(false);
			}
			return new MoveItemData(templateItemJson, this.grabbedAmount, (this.assignedRoute.Source as IGUIDRegisterable).GUID, (this.assignedRoute.Destination as IGUIDRegisterable).GUID);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x0007ECB4 File Offset: 0x0007CEB4
		public void Load(MoveItemData moveItemData)
		{
			if (moveItemData == null)
			{
				return;
			}
			if (moveItemData.GrabbedItemQuantity == 0 || string.IsNullOrEmpty(moveItemData.TemplateItemJSON))
			{
				return;
			}
			ITransitEntity @object = GUIDManager.GetObject<ITransitEntity>(new Guid(moveItemData.SourceGUID));
			ITransitEntity object2 = GUIDManager.GetObject<ITransitEntity>(new Guid(moveItemData.DestinationGUID));
			if (@object == null)
			{
				Console.LogWarning("Failed to load source transit entity", null);
				return;
			}
			if (object2 == null)
			{
				Console.LogWarning("Failed to load destination transit entity", null);
				return;
			}
			TransitRoute route = new TransitRoute(@object, object2);
			this.grabbedAmount = moveItemData.GrabbedItemQuantity;
			Debug.Log("Resuming move item behaviour");
			ItemInstance itemInstance = ItemDeserializer.LoadItem(moveItemData.TemplateItemJSON);
			if (itemInstance != null)
			{
				this.Resume(route, itemInstance, -1);
				base.Enable_Networked(null);
			}
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x0007ED67 File Offset: 0x0007CF67
		[CompilerGenerated]
		private IEnumerator <WalkToSource>g__Routine|26_0()
		{
			base.SetDestination(this.GetSourceAccessPoint(this.assignedRoute).position, true);
			yield return new WaitForSeconds(0.5f);
			yield return new WaitUntil(() => !base.Npc.Movement.IsMoving);
			this.currentState = MoveItemBehaviour.EState.Idle;
			this.walkToSourceRoutine = null;
			yield break;
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x0007ED8B File Offset: 0x0007CF8B
		[CompilerGenerated]
		private IEnumerator <GrabItem>g__Routine|27_0()
		{
			Transform sourceAccessPoint = this.GetSourceAccessPoint(this.assignedRoute);
			if (sourceAccessPoint == null)
			{
				Console.LogWarning("Could not find source access point!", null);
				this.grabRoutine = null;
				base.Disable_Networked(null);
				yield break;
			}
			base.Npc.Movement.FaceDirection(sourceAccessPoint.forward, 0.5f);
			base.Npc.SetAnimationTrigger_Networked(null, "GrabItem");
			float seconds = 0.5f;
			yield return new WaitForSeconds(seconds);
			string str;
			if (!this.IsTransitRouteValid(this.assignedRoute, this.itemToRetrieveTemplate, out str))
			{
				Console.LogWarning("Transit route no longer valid! Reason: " + str, null);
				this.grabRoutine = null;
				base.Disable_Networked(null);
				yield break;
			}
			this.TakeItem();
			yield return new WaitForSeconds(0.5f);
			this.grabRoutine = null;
			this.currentState = MoveItemBehaviour.EState.Idle;
			yield break;
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x0007ED9A File Offset: 0x0007CF9A
		[CompilerGenerated]
		private IEnumerator <WalkToDestination>g__Routine|29_0()
		{
			base.SetDestination(this.GetDestinationAccessPoint(this.assignedRoute).position, true);
			yield return new WaitForSeconds(0.5f);
			yield return new WaitUntil(() => !base.Npc.Movement.IsMoving);
			this.currentState = MoveItemBehaviour.EState.Idle;
			this.walkToDestinationRoutine = null;
			yield break;
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x0007EDA9 File Offset: 0x0007CFA9
		[CompilerGenerated]
		private IEnumerator <PlaceItem>g__Routine|30_0()
		{
			if (this.GetDestinationAccessPoint(this.assignedRoute) != null)
			{
				base.Npc.Movement.FaceDirection(this.GetDestinationAccessPoint(this.assignedRoute).forward, 0.5f);
			}
			base.Npc.SetAnimationTrigger_Networked(null, "GrabItem");
			float seconds = 0.5f;
			yield return new WaitForSeconds(seconds);
			this.assignedRoute.Destination.RemoveSlotLocks(base.Npc.NetworkObject);
			ItemInstance firstIdenticalItem = base.Npc.Inventory.GetFirstIdenticalItem(this.itemToRetrieveTemplate, null);
			if (firstIdenticalItem != null && this.grabbedAmount > 0)
			{
				ItemInstance copy = firstIdenticalItem.GetCopy(this.grabbedAmount);
				if (this.assignedRoute.Destination.GetInputCapacityForItem(copy, base.Npc, true) >= this.grabbedAmount)
				{
					this.assignedRoute.Destination.InsertItemIntoInput(copy, base.Npc);
				}
				else
				{
					Console.LogWarning("Destination does not have enough capacity for item! Attempting to return item to source.", null);
					if (this.assignedRoute.Source.GetOutputCapacityForItem(copy, base.Npc) >= this.grabbedAmount)
					{
						this.assignedRoute.Source.InsertItemIntoOutput(copy, base.Npc);
					}
					else
					{
						Console.LogWarning("Source does not have enough capacity for item! Item will be lost.", null);
					}
				}
				firstIdenticalItem.ChangeQuantity(-this.grabbedAmount);
			}
			else
			{
				Console.LogWarning("Could not find carried item to place!", null);
			}
			yield return new WaitForSeconds(0.5f);
			this.placingRoutine = null;
			this.currentState = MoveItemBehaviour.EState.Idle;
			base.Disable_Networked(null);
			yield break;
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x0007EDB8 File Offset: 0x0007CFB8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.MoveItemBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.MoveItemBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x0007EDD1 File Offset: 0x0007CFD1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.MoveItemBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.MoveItemBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x0007EDEA File Offset: 0x0007CFEA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x0007EDF8 File Offset: 0x0007CFF8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001845 RID: 6213
		private TransitRoute assignedRoute;

		// Token: 0x04001846 RID: 6214
		private ItemInstance itemToRetrieveTemplate;

		// Token: 0x04001847 RID: 6215
		private int grabbedAmount;

		// Token: 0x04001848 RID: 6216
		private int maxMoveAmount = -1;

		// Token: 0x04001849 RID: 6217
		private MoveItemBehaviour.EState currentState;

		// Token: 0x0400184A RID: 6218
		private Coroutine walkToSourceRoutine;

		// Token: 0x0400184B RID: 6219
		private Coroutine grabRoutine;

		// Token: 0x0400184C RID: 6220
		private Coroutine walkToDestinationRoutine;

		// Token: 0x0400184D RID: 6221
		private Coroutine placingRoutine;

		// Token: 0x0400184E RID: 6222
		private bool skipPickup;

		// Token: 0x0400184F RID: 6223
		private bool dll_Excuted;

		// Token: 0x04001850 RID: 6224
		private bool dll_Excuted;

		// Token: 0x02000536 RID: 1334
		public enum EState
		{
			// Token: 0x04001852 RID: 6226
			Idle,
			// Token: 0x04001853 RID: 6227
			WalkingToSource,
			// Token: 0x04001854 RID: 6228
			Grabbing,
			// Token: 0x04001855 RID: 6229
			WalkingToDestination,
			// Token: 0x04001856 RID: 6230
			Placing
		}
	}
}
