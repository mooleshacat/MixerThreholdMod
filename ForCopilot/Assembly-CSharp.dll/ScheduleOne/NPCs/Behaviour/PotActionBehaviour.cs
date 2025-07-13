using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Equipping;
using ScheduleOne.Growing;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.Soil;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200053F RID: 1343
	public class PotActionBehaviour : Behaviour
	{
		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x0007FDE4 File Offset: 0x0007DFE4
		// (set) Token: 0x06001F13 RID: 7955 RVA: 0x0007FDEC File Offset: 0x0007DFEC
		public bool Initialized { get; protected set; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001F14 RID: 7956 RVA: 0x0007FDF5 File Offset: 0x0007DFF5
		// (set) Token: 0x06001F15 RID: 7957 RVA: 0x0007FDFD File Offset: 0x0007DFFD
		public Pot AssignedPot { get; protected set; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001F16 RID: 7958 RVA: 0x0007FE06 File Offset: 0x0007E006
		// (set) Token: 0x06001F17 RID: 7959 RVA: 0x0007FE0E File Offset: 0x0007E00E
		public PotActionBehaviour.EActionType CurrentActionType { get; protected set; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001F18 RID: 7960 RVA: 0x0007FE17 File Offset: 0x0007E017
		// (set) Token: 0x06001F19 RID: 7961 RVA: 0x0007FE1F File Offset: 0x0007E01F
		public PotActionBehaviour.EState CurrentState { get; protected set; }

		// Token: 0x06001F1A RID: 7962 RVA: 0x0007FE28 File Offset: 0x0007E028
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.PotActionBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0007FE3C File Offset: 0x0007E03C
		public virtual void Initialize(Pot pot, PotActionBehaviour.EActionType actionType)
		{
			if (this.botanist.DEBUG)
			{
				Debug.Log("PotActionBehaviour.Initialize: " + ((pot != null) ? pot.ToString() : null) + " - " + actionType.ToString());
			}
			this.AssignedPot = pot;
			this.CurrentActionType = actionType;
			this.Initialized = true;
			this.CurrentState = PotActionBehaviour.EState.Idle;
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0007FEA0 File Offset: 0x0007E0A0
		protected override void Begin()
		{
			base.Begin();
			this.StartAction();
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x0007FEAE File Offset: 0x0007E0AE
		protected override void Resume()
		{
			base.Resume();
			this.StartAction();
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0007FEBC File Offset: 0x0007E0BC
		protected override void Pause()
		{
			base.Pause();
			this.StopAllActions();
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0007C40F File Offset: 0x0007A60F
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x0007FECA File Offset: 0x0007E0CA
		protected override void End()
		{
			base.End();
			this.StopAllActions();
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0007FED8 File Offset: 0x0007E0D8
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.Npc.behaviour.DEBUG_MODE)
			{
				Console.Log("Current state: " + this.CurrentState.ToString(), null);
				Console.Log("Is walking: " + base.Npc.Movement.IsMoving.ToString(), null);
			}
			if (this.CurrentState == PotActionBehaviour.EState.Idle)
			{
				if (!this.DoesTaskTypeRequireSupplies(this.CurrentActionType) || base.Npc.Inventory.GetMaxItemCount(this.GetRequiredItemIDs()) > 0)
				{
					if (this.IsAtPot())
					{
						this.PerformAction();
						return;
					}
					this.WalkToPot();
					return;
				}
				else
				{
					if (this.AssignedPot == null)
					{
						string str = "PotActionBehaviour.ActiveMinPass: No pot assigned for botanist ";
						Botanist botanist = this.botanist;
						Console.LogWarning(str + ((botanist != null) ? botanist.ToString() : null), null);
						base.Disable_Networked(null);
						return;
					}
					if (this.IsAtSupplies())
					{
						if (this.DoesBotanistHaveMaterialsForTask(this.botanist, this.AssignedPot, this.CurrentActionType, this.AdditiveNumber))
						{
							this.GrabItem();
							return;
						}
						this.StopPerformAction();
						base.Disable_Networked(null);
						return;
					}
					else
					{
						this.WalkToSupplies();
					}
				}
			}
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x00080014 File Offset: 0x0007E214
		private void StartAction()
		{
			if (!this.AreActionConditionsMet())
			{
				string str = "PotActionBehaviour.StartAction: Conditions not met for action ";
				string str2 = this.CurrentActionType.ToString();
				string str3 = " on pot ";
				Pot assignedPot = this.AssignedPot;
				Console.LogWarning(str + str2 + str3 + ((assignedPot != null) ? assignedPot.ToString() : null), null);
				base.Disable_Networked(null);
				return;
			}
			if (!this.DoesBotanistHaveMaterialsForTask(base.Npc as Botanist, this.AssignedPot, this.CurrentActionType, this.AdditiveNumber))
			{
				string str4 = "PotActionBehaviour.StartAction: Botanist does not have materials for action ";
				string str5 = this.CurrentActionType.ToString();
				string str6 = " on pot ";
				Pot assignedPot2 = this.AssignedPot;
				Console.LogWarning(str4 + str5 + str6 + ((assignedPot2 != null) ? assignedPot2.ToString() : null), null);
				base.Disable_Networked(null);
				return;
			}
			if (this.botanist.DEBUG)
			{
				string str7 = "PotActionBehaviour.StartAction: Starting action ";
				string str8 = this.CurrentActionType.ToString();
				string str9 = " on pot ";
				Pot assignedPot3 = this.AssignedPot;
				Console.Log(str7 + str8 + str9 + ((assignedPot3 != null) ? assignedPot3.ToString() : null), null);
			}
			this.CurrentState = PotActionBehaviour.EState.Idle;
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x00080124 File Offset: 0x0007E324
		private void StopAllActions()
		{
			if (this.walkToSuppliesRoutine != null)
			{
				base.StopCoroutine(this.walkToSuppliesRoutine);
				this.walkToSuppliesRoutine = null;
			}
			if (this.grabRoutine != null)
			{
				base.StopCoroutine(this.grabRoutine);
				this.grabRoutine = null;
			}
			if (this.walkToPotRoutine != null)
			{
				base.StopCoroutine(this.walkToPotRoutine);
				this.walkToPotRoutine = null;
			}
			if (this.performActionRoutine != null)
			{
				this.StopPerformAction();
			}
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x00080190 File Offset: 0x0007E390
		public void WalkToSupplies()
		{
			if (!base.Npc.Movement.CanGetTo(this.AssignedPot, 1f))
			{
				string str = "PotActionBehaviour.WalkToPot: Can't get to pot ";
				Pot assignedPot = this.AssignedPot;
				Console.LogWarning(str + ((assignedPot != null) ? assignedPot.ToString() : null), null);
				base.Disable_Networked(null);
				return;
			}
			if (this.botanist.DEBUG)
			{
				string str2 = "PotActionBehaviour.WalkToSupplies: Walking to supplies for action ";
				string str3 = this.CurrentActionType.ToString();
				string str4 = " on pot ";
				Pot assignedPot2 = this.AssignedPot;
				Debug.Log(str2 + str3 + str4 + ((assignedPot2 != null) ? assignedPot2.ToString() : null));
			}
			this.CurrentState = PotActionBehaviour.EState.WalkingToSupplies;
			this.walkToSuppliesRoutine = base.StartCoroutine(this.<WalkToSupplies>g__Routine|38_0());
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x00080244 File Offset: 0x0007E444
		public void GrabItem()
		{
			if (this.botanist.DEBUG)
			{
				string str = "PotActionBehaviour.GrabItem: Grabbing item for action ";
				string str2 = this.CurrentActionType.ToString();
				string str3 = " on pot ";
				Pot assignedPot = this.AssignedPot;
				Debug.Log(str + str2 + str3 + ((assignedPot != null) ? assignedPot.ToString() : null));
			}
			this.CurrentState = PotActionBehaviour.EState.GrabbingSupplies;
			this.grabRoutine = base.StartCoroutine(this.<GrabItem>g__Routine|39_0());
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x000802B4 File Offset: 0x0007E4B4
		public void WalkToPot()
		{
			if (!base.Npc.Movement.CanGetTo(this.GetPotAccessPoint().position, 1f))
			{
				string str = "PotActionBehaviour.WalkToPot: Can't get to pot ";
				Pot assignedPot = this.AssignedPot;
				Console.LogWarning(str + ((assignedPot != null) ? assignedPot.ToString() : null), null);
				base.Disable_Networked(null);
				return;
			}
			if (this.botanist.DEBUG)
			{
				string str2 = "PotActionBehaviour.WalkToPot: Walking to pot ";
				Pot assignedPot2 = this.AssignedPot;
				Debug.Log(str2 + ((assignedPot2 != null) ? assignedPot2.ToString() : null));
			}
			this.CurrentState = PotActionBehaviour.EState.WalkingToPot;
			this.walkToPotRoutine = base.StartCoroutine(this.<WalkToPot>g__Routine|40_0());
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x00080354 File Offset: 0x0007E554
		public void PerformAction()
		{
			if (this.botanist.DEBUG)
			{
				string str = "PotActionBehaviour.PerformAction: Performing action ";
				string str2 = this.CurrentActionType.ToString();
				string str3 = " on pot ";
				Pot assignedPot = this.AssignedPot;
				Debug.Log(str + str2 + str3 + ((assignedPot != null) ? assignedPot.ToString() : null));
			}
			this.CurrentState = PotActionBehaviour.EState.PerformingAction;
			this.performActionRoutine = base.StartCoroutine(this.<PerformAction>g__Routine|41_0());
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x000803C4 File Offset: 0x0007E5C4
		private void CompleteAction()
		{
			if (this.AssignedPot == null)
			{
				string str = "PotActionBehaviour.CompleteAction: No pot assigned for botanist ";
				Botanist botanist = this.botanist;
				Console.LogWarning(str + ((botanist != null) ? botanist.ToString() : null), null);
				return;
			}
			ItemInstance itemInstance = null;
			string[] requiredItemIDs = this.GetRequiredItemIDs();
			for (int i = 0; i < requiredItemIDs.Length; i++)
			{
				itemInstance = base.Npc.Inventory.GetFirstItem(requiredItemIDs[i], null);
				if (itemInstance != null)
				{
					break;
				}
			}
			if (this.DoesTaskTypeRequireSupplies(this.CurrentActionType) && itemInstance == null)
			{
				string str2 = "PotActionBehaviour.CompleteAction: No item held for action ";
				string str3 = this.CurrentActionType.ToString();
				string str4 = " on pot ";
				Pot assignedPot = this.AssignedPot;
				Console.LogWarning(str2 + str3 + str4 + ((assignedPot != null) ? assignedPot.ToString() : null), null);
				return;
			}
			ItemInstance itemInstance2 = null;
			switch (this.CurrentActionType)
			{
			case PotActionBehaviour.EActionType.PourSoil:
			{
				SoilDefinition soilDefinition = itemInstance.Definition as SoilDefinition;
				if (soilDefinition == null)
				{
					string str5 = "PotActionBehaviour.CompleteAction: Required item is not soil for action ";
					string str6 = this.CurrentActionType.ToString();
					string str7 = " on pot ";
					Pot assignedPot2 = this.AssignedPot;
					Console.LogWarning(str5 + str6 + str7 + ((assignedPot2 != null) ? assignedPot2.ToString() : null), null);
					return;
				}
				this.AssignedPot.AddSoil(this.AssignedPot.SoilCapacity);
				this.AssignedPot.SetSoilID(soilDefinition.ID);
				this.AssignedPot.SetSoilUses(soilDefinition.Uses);
				NetworkSingleton<TrashManager>.Instance.CreateTrashItem((soilDefinition.Equippable as Equippable_Soil).PourablePrefab.TrashItem.ID, base.transform.position + Vector3.up * 0.5f, UnityEngine.Random.rotation, default(Vector3), "", false);
				break;
			}
			case PotActionBehaviour.EActionType.SowSeed:
				this.AssignedPot.PlantSeed(null, itemInstance.ID, 0f, -1f, -1f);
				NetworkSingleton<TrashManager>.Instance.CreateTrashItem((itemInstance.Definition as SeedDefinition).FunctionSeedPrefab.TrashPrefab.ID, base.transform.position + Vector3.up * 0.5f, UnityEngine.Random.rotation, default(Vector3), "", false);
				break;
			case PotActionBehaviour.EActionType.Water:
			{
				float num = UnityEngine.Random.Range(this.botanist.TARGET_WATER_LEVEL_MIN, this.botanist.TARGET_WATER_LEVEL_MAX);
				this.AssignedPot.ChangeWaterAmount(num * this.AssignedPot.WaterCapacity - this.AssignedPot.WaterLevel);
				break;
			}
			case PotActionBehaviour.EActionType.ApplyAdditive:
				this.AssignedPot.ApplyAdditive(null, (itemInstance.Definition as AdditiveDefinition).AdditivePrefab.AssetPath, true);
				NetworkSingleton<TrashManager>.Instance.CreateTrashItem(((itemInstance.Definition as AdditiveDefinition).Equippable as Equippable_Additive).PourablePrefab.TrashItem.ID, base.transform.position + Vector3.up * 0.5f, UnityEngine.Random.rotation, default(Vector3), "", false);
				break;
			case PotActionBehaviour.EActionType.Harvest:
				if (!this.DoesPotHaveValidDestination(this.AssignedPot))
				{
					string str8 = "PotActionBehaviour.CompleteAction: Pot ";
					Pot assignedPot3 = this.AssignedPot;
					Console.LogWarning(str8 + ((assignedPot3 != null) ? assignedPot3.ToString() : null) + " does not have a valid destination", null);
					return;
				}
				itemInstance2 = this.AssignedPot.Plant.GetHarvestedProduct(this.AssignedPot.Plant.ActiveHarvestables.Count);
				this.AssignedPot.ResetPot();
				break;
			}
			if (itemInstance != null)
			{
				itemInstance.ChangeQuantity(-1);
			}
			if (this.CurrentActionType == PotActionBehaviour.EActionType.Harvest)
			{
				((ITransitEntity)this.AssignedPot).InsertItemIntoOutput(itemInstance2, null);
				TransitRoute route = new TransitRoute(this.AssignedPot, (this.AssignedPot.Configuration as PotConfiguration).Destination.SelectedObject as ITransitEntity);
				this.botanist.MoveItemBehaviour.Initialize(route, itemInstance2, -1, true);
				this.botanist.MoveItemBehaviour.Enable_Networked(null);
			}
			base.Disable_Networked(null);
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x000807C8 File Offset: 0x0007E9C8
		private void StopPerformAction()
		{
			if (this.CurrentActionType == PotActionBehaviour.EActionType.SowSeed && base.Npc.Avatar.Anim.IsCrouched)
			{
				base.Npc.SetCrouched_Networked(false);
			}
			this.CurrentState = PotActionBehaviour.EState.Idle;
			if (this.performActionRoutine != null)
			{
				base.StopCoroutine(this.performActionRoutine);
				this.performActionRoutine = null;
			}
			if (this.currentActionEquippable != null)
			{
				base.Npc.SetEquippable_Networked(null, string.Empty);
				this.currentActionEquippable = null;
			}
			if (this.currentActionAnimation != string.Empty)
			{
				base.Npc.SetAnimationBool_Networked(null, this.currentActionAnimation, false);
				this.currentActionAnimation = string.Empty;
			}
			if (this.AssignedPot != null && this.AssignedPot.NPCUserObject == this.botanist.NetworkObject)
			{
				this.AssignedPot.SetNPCUser(null);
			}
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x000808B0 File Offset: 0x0007EAB0
		private string GetActionAnimation(PotActionBehaviour.EActionType actionType)
		{
			switch (actionType)
			{
			case PotActionBehaviour.EActionType.PourSoil:
				return "PourItem";
			case PotActionBehaviour.EActionType.SowSeed:
				return "PatSoil";
			case PotActionBehaviour.EActionType.Water:
				return "PourItem";
			case PotActionBehaviour.EActionType.ApplyAdditive:
				return "PourItem";
			case PotActionBehaviour.EActionType.Harvest:
				return "Snipping";
			default:
				return string.Empty;
			}
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x00080900 File Offset: 0x0007EB00
		private AvatarEquippable GetActionEquippable(PotActionBehaviour.EActionType actionType)
		{
			switch (actionType)
			{
			case PotActionBehaviour.EActionType.SowSeed:
				return null;
			case PotActionBehaviour.EActionType.Water:
				return this.WateringCanEquippable;
			case PotActionBehaviour.EActionType.Harvest:
				return this.TrimmersEquippable;
			}
			ItemInstance itemInstance = null;
			string[] requiredItemIDs = this.GetRequiredItemIDs();
			for (int i = 0; i < requiredItemIDs.Length; i++)
			{
				itemInstance = base.Npc.Inventory.GetFirstItem(requiredItemIDs[i], null);
				if (itemInstance != null)
				{
					break;
				}
			}
			if (itemInstance == null)
			{
				return null;
			}
			Equippable_Viewmodel equippable_Viewmodel = itemInstance.Equippable as Equippable_Viewmodel;
			if (equippable_Viewmodel == null)
			{
				return null;
			}
			return equippable_Viewmodel.AvatarEquippable;
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x00080984 File Offset: 0x0007EB84
		public float GetWaitTime(PotActionBehaviour.EActionType actionType)
		{
			switch (actionType)
			{
			case PotActionBehaviour.EActionType.PourSoil:
				return this.botanist.SOIL_POUR_TIME;
			case PotActionBehaviour.EActionType.SowSeed:
				return this.botanist.SEED_SOW_TIME;
			case PotActionBehaviour.EActionType.Water:
				return this.botanist.WATER_POUR_TIME;
			case PotActionBehaviour.EActionType.ApplyAdditive:
				return this.botanist.ADDITIVE_POUR_TIME;
			case PotActionBehaviour.EActionType.Harvest:
				return this.botanist.HARVEST_TIME;
			default:
				Console.LogWarning("Can't find wait time for " + actionType.ToString(), null);
				return 10f;
			}
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x00080A0D File Offset: 0x0007EC0D
		public bool CanGetToSupplies()
		{
			return base.Npc.Movement.CanGetTo((this.botanist.Configuration as BotanistConfiguration).Supplies.SelectedObject as ITransitEntity, 1f);
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x00080A43 File Offset: 0x0007EC43
		private bool IsAtSupplies()
		{
			return NavMeshUtility.IsAtTransitEntity(this.GetSuppliesAsTransitEntity(), base.Npc, 0.4f);
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x00080A5C File Offset: 0x0007EC5C
		private ITransitEntity GetSuppliesAsTransitEntity()
		{
			if ((this.botanist.Configuration as BotanistConfiguration).Supplies.SelectedObject == null)
			{
				string str = "PotActionBehaviour.GetSuppliesAsTransitEntity: No supplies selected for botanist ";
				Botanist botanist = this.botanist;
				Console.LogWarning(str + ((botanist != null) ? botanist.ToString() : null), null);
				return null;
			}
			return (this.botanist.Configuration as BotanistConfiguration).Supplies.SelectedObject as ITransitEntity;
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x00080ACE File Offset: 0x0007ECCE
		public bool CanGetToPot()
		{
			return this.GetPotAccessPoint() != null;
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x00080ADC File Offset: 0x0007ECDC
		private Transform GetPotAccessPoint()
		{
			if (this.AssignedPot == null)
			{
				string str = "PotActionBehaviour.GetpotAccessPoint: No pot selected for botanist ";
				Botanist botanist = this.botanist;
				Console.LogWarning(str + ((botanist != null) ? botanist.ToString() : null), null);
				return null;
			}
			Transform accessPoint = NavMeshUtility.GetAccessPoint(this.AssignedPot, base.Npc);
			if (accessPoint == null)
			{
				string str2 = "PotActionBehaviour.GetpotAccessPoint: No access point found for pot ";
				Pot assignedPot = this.AssignedPot;
				Console.LogWarning(str2 + ((assignedPot != null) ? assignedPot.ToString() : null), null);
				return this.AssignedPot.transform;
			}
			return accessPoint;
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x00080B68 File Offset: 0x0007ED68
		private bool IsAtPot()
		{
			if (this.AssignedPot == null)
			{
				string str = "PotActionBehaviour.IsAtpot: No pot selected for botanist ";
				Botanist botanist = this.botanist;
				Console.LogWarning(str + ((botanist != null) ? botanist.ToString() : null), null);
				return false;
			}
			return NavMeshUtility.IsAtTransitEntity(this.AssignedPot, base.Npc, 0.4f);
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x00080BC0 File Offset: 0x0007EDC0
		private string[] GetRequiredItemIDs(PotActionBehaviour.EActionType actionType, Pot pot)
		{
			PotConfiguration potConfiguration = pot.Configuration as PotConfiguration;
			switch (actionType)
			{
			case PotActionBehaviour.EActionType.PourSoil:
				return new string[]
				{
					"soil",
					"longlifesoil",
					"extralonglifesoil"
				};
			case PotActionBehaviour.EActionType.SowSeed:
				if (potConfiguration.Seed.SelectedItem == null)
				{
					return Singleton<Registry>.Instance.Seeds.ConvertAll<string>((SeedDefinition x) => x.ID).ToArray();
				}
				return new string[]
				{
					potConfiguration.Seed.SelectedItem.ID
				};
			case PotActionBehaviour.EActionType.ApplyAdditive:
				if (this.AdditiveNumber == 1)
				{
					return new string[]
					{
						potConfiguration.Additive1.SelectedItem.ID
					};
				}
				if (this.AdditiveNumber == 2)
				{
					return new string[]
					{
						potConfiguration.Additive2.SelectedItem.ID
					};
				}
				if (this.AdditiveNumber == 3)
				{
					return new string[]
					{
						potConfiguration.Additive3.SelectedItem.ID
					};
				}
				break;
			}
			return new string[0];
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x00080CE5 File Offset: 0x0007EEE5
		private string[] GetRequiredItemIDs()
		{
			return this.GetRequiredItemIDs(this.CurrentActionType, this.AssignedPot);
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x00080CFC File Offset: 0x0007EEFC
		private bool AreActionConditionsMet()
		{
			switch (this.CurrentActionType)
			{
			case PotActionBehaviour.EActionType.PourSoil:
				return this.CanPotHaveSoilPour(this.AssignedPot);
			case PotActionBehaviour.EActionType.SowSeed:
				return this.CanPotHaveSeedSown(this.AssignedPot);
			case PotActionBehaviour.EActionType.Water:
				return this.CanPotBeWatered(this.AssignedPot, 1f);
			case PotActionBehaviour.EActionType.ApplyAdditive:
			{
				int num;
				return this.CanPotHaveAdditiveApplied(this.AssignedPot, out num);
			}
			case PotActionBehaviour.EActionType.Harvest:
				return this.CanPotBeHarvested(this.AssignedPot);
			default:
				return false;
			}
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x00080D77 File Offset: 0x0007EF77
		public bool DoesTaskTypeRequireSupplies(PotActionBehaviour.EActionType actionType)
		{
			return actionType - PotActionBehaviour.EActionType.PourSoil <= 1 || actionType == PotActionBehaviour.EActionType.ApplyAdditive;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x00080D88 File Offset: 0x0007EF88
		public bool DoesBotanistHaveMaterialsForTask(Botanist botanist, Pot pot, PotActionBehaviour.EActionType actionType, int additiveNumber = -1)
		{
			switch (actionType)
			{
			case PotActionBehaviour.EActionType.PourSoil:
			{
				ItemInstance soilInSupplies = this.GetSoilInSupplies();
				return (soilInSupplies != null && base.Npc.Inventory.GetCapacityForItem(soilInSupplies) > 0) || base.Npc.Inventory.GetMaxItemCount(this.GetRequiredItemIDs(actionType, pot)) > 0;
			}
			case PotActionBehaviour.EActionType.SowSeed:
			{
				ItemInstance seedInSupplies = this.GetSeedInSupplies(pot);
				return (seedInSupplies != null && base.Npc.Inventory.GetCapacityForItem(seedInSupplies) > 0) || base.Npc.Inventory.GetMaxItemCount(this.GetRequiredItemIDs(actionType, pot)) > 0;
			}
			case PotActionBehaviour.EActionType.ApplyAdditive:
			{
				ItemInstance additiveInSupplies = this.GetAdditiveInSupplies(pot, additiveNumber);
				return (additiveInSupplies != null && base.Npc.Inventory.GetCapacityForItem(additiveInSupplies) > 0) || base.Npc.Inventory.GetMaxItemCount(this.GetRequiredItemIDs(actionType, pot)) > 0;
			}
			}
			return true;
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x00080E6C File Offset: 0x0007F06C
		private ItemInstance GetSoilInSupplies()
		{
			ItemInstance itemInSupplies = this.botanist.GetItemInSupplies("soil");
			if (itemInSupplies != null)
			{
				return itemInSupplies;
			}
			ItemInstance itemInSupplies2 = this.botanist.GetItemInSupplies("longlifesoil");
			if (itemInSupplies2 != null)
			{
				return itemInSupplies2;
			}
			ItemInstance itemInSupplies3 = this.botanist.GetItemInSupplies("extralonglifesoil");
			if (itemInSupplies3 != null)
			{
				return itemInSupplies3;
			}
			return null;
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x00080EBC File Offset: 0x0007F0BC
		private ItemInstance GetSeedInSupplies(Pot pot)
		{
			PotConfiguration potConfiguration = pot.Configuration as PotConfiguration;
			if (potConfiguration.Seed.SelectedItem == null)
			{
				return this.botanist.GetSeedInSupplies();
			}
			return this.botanist.GetItemInSupplies(potConfiguration.Seed.SelectedItem.ID);
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x00080F10 File Offset: 0x0007F110
		private ItemInstance GetAdditiveInSupplies(Pot pot, int additiveNumber)
		{
			PotConfiguration potConfiguration = pot.Configuration as PotConfiguration;
			ItemDefinition selectedItem;
			if (additiveNumber == 1)
			{
				selectedItem = potConfiguration.Additive1.SelectedItem;
			}
			else if (additiveNumber == 2)
			{
				selectedItem = potConfiguration.Additive2.SelectedItem;
			}
			else
			{
				if (additiveNumber != 3)
				{
					Console.LogWarning("PotActionBehaviour.DoesBotanistHaveMaterialsForTask: Invalid additive number " + additiveNumber.ToString(), null);
					return null;
				}
				selectedItem = potConfiguration.Additive3.SelectedItem;
			}
			if (selectedItem == null)
			{
				return null;
			}
			return this.botanist.GetItemInSupplies(selectedItem.ID);
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x00080F96 File Offset: 0x0007F196
		public bool CanPotBeWatered(Pot pot, float threshold)
		{
			return !((IUsable)pot).IsInUse && pot.IsFilledWithSoil && !(pot.Plant == null) && pot.WaterLevel <= threshold;
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x00080FC8 File Offset: 0x0007F1C8
		public bool CanPotHaveSoilPour(Pot pot)
		{
			return !((IUsable)pot).IsInUse && !pot.IsFilledWithSoil;
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x00080FDF File Offset: 0x0007F1DF
		public bool CanPotHaveSeedSown(Pot pot)
		{
			return !((IUsable)pot).IsInUse && pot.IsFilledWithSoil && !(pot.Plant != null);
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x00081008 File Offset: 0x0007F208
		public bool CanPotHaveAdditiveApplied(Pot pot, out int additiveNumber)
		{
			additiveNumber = -1;
			if (((IUsable)pot).IsInUse)
			{
				return false;
			}
			if (!pot.IsFilledWithSoil)
			{
				return false;
			}
			if (pot.Plant == null)
			{
				return false;
			}
			PotConfiguration potConfiguration = pot.Configuration as PotConfiguration;
			if (potConfiguration.Additive1.SelectedItem != null && pot.GetAdditive((potConfiguration.Additive1.SelectedItem as AdditiveDefinition).AdditivePrefab.AdditiveName) == null)
			{
				additiveNumber = 1;
				return true;
			}
			if (potConfiguration.Additive2.SelectedItem != null && pot.GetAdditive((potConfiguration.Additive2.SelectedItem as AdditiveDefinition).AdditivePrefab.AdditiveName) == null)
			{
				additiveNumber = 2;
				return true;
			}
			if (potConfiguration.Additive3.SelectedItem != null && pot.GetAdditive((potConfiguration.Additive3.SelectedItem as AdditiveDefinition).AdditivePrefab.AdditiveName) == null)
			{
				additiveNumber = 3;
				return true;
			}
			return false;
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x00081109 File Offset: 0x0007F309
		public bool CanPotBeHarvested(Pot pot)
		{
			if (((IUsable)pot).IsInUse)
			{
				return false;
			}
			if (pot.Plant == null)
			{
				return false;
			}
			EntityConfiguration configuration = pot.Configuration;
			return pot.Plant.IsFullyGrown;
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x0008113C File Offset: 0x0007F33C
		public bool DoesPotHaveValidDestination(Pot pot)
		{
			PotConfiguration potConfiguration = pot.Configuration as PotConfiguration;
			return !(potConfiguration.Destination.SelectedObject == null) && (potConfiguration.Destination.SelectedObject as ITransitEntity).GetInputCapacityForItem(pot.Plant.GetHarvestedProduct(1), this.botanist, true) >= pot.Plant.ActiveHarvestables.Count;
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000811C1 File Offset: 0x0007F3C1
		[CompilerGenerated]
		private IEnumerator <WalkToSupplies>g__Routine|38_0()
		{
			base.SetDestination(this.GetSuppliesAsTransitEntity(), true);
			yield return new WaitForEndOfFrame();
			yield return new WaitUntil(() => !base.Npc.Movement.IsMoving);
			this.CurrentState = PotActionBehaviour.EState.Idle;
			this.walkToSuppliesRoutine = null;
			yield break;
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x000811D0 File Offset: 0x0007F3D0
		[CompilerGenerated]
		private IEnumerator <GrabItem>g__Routine|39_0()
		{
			base.Npc.Movement.FacePoint((this.botanist.Configuration as BotanistConfiguration).Supplies.SelectedObject.transform.position, 0.5f);
			base.Npc.Avatar.Anim.ResetTrigger("GrabItem");
			base.Npc.Avatar.Anim.SetTrigger("GrabItem");
			float seconds = 0.5f;
			yield return new WaitForSeconds(seconds);
			if (!this.DoesBotanistHaveMaterialsForTask(this.botanist, this.AssignedPot, this.CurrentActionType, this.AdditiveNumber))
			{
				string str = "Botanist does not have materials for action ";
				string str2 = this.CurrentActionType.ToString();
				string str3 = " on pot ";
				Pot assignedPot = this.AssignedPot;
				Console.LogWarning(str + str2 + str3 + ((assignedPot != null) ? assignedPot.ToString() : null), null);
				this.grabRoutine = null;
				this.CurrentState = PotActionBehaviour.EState.Idle;
				yield break;
			}
			if (!this.AreActionConditionsMet())
			{
				string str4 = "Conditions not met for action ";
				string str5 = this.CurrentActionType.ToString();
				string str6 = " on pot ";
				Pot assignedPot2 = this.AssignedPot;
				Console.LogWarning(str4 + str5 + str6 + ((assignedPot2 != null) ? assignedPot2.ToString() : null), null);
				this.grabRoutine = null;
				this.CurrentState = PotActionBehaviour.EState.Idle;
				yield break;
			}
			ItemSlot itemSlot = null;
			string[] requiredItemIDs = this.GetRequiredItemIDs();
			for (int i = 0; i < requiredItemIDs.Length; i++)
			{
				itemSlot = ((this.botanist.Configuration as BotanistConfiguration).Supplies.SelectedObject as ITransitEntity).GetFirstSlotContainingItem(requiredItemIDs[i], ITransitEntity.ESlotType.Both);
				if (itemSlot != null)
				{
					break;
				}
			}
			ItemInstance itemInstance = (itemSlot != null) ? itemSlot.ItemInstance : null;
			if (itemInstance == null)
			{
				string str7 = "PotActionBehaviour.GrabItem: No item found for action ";
				string str8 = this.CurrentActionType.ToString();
				string str9 = " on pot ";
				Pot assignedPot3 = this.AssignedPot;
				Console.LogWarning(str7 + str8 + str9 + ((assignedPot3 != null) ? assignedPot3.ToString() : null), null);
				this.grabRoutine = null;
				this.CurrentState = PotActionBehaviour.EState.Idle;
				yield break;
			}
			base.Npc.Inventory.InsertItem(itemInstance.GetCopy(1), true);
			itemSlot.ChangeQuantity(-1, false);
			yield return new WaitForSeconds(0.5f);
			this.grabRoutine = null;
			this.CurrentState = PotActionBehaviour.EState.Idle;
			yield break;
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x000811DF File Offset: 0x0007F3DF
		[CompilerGenerated]
		private IEnumerator <WalkToPot>g__Routine|40_0()
		{
			base.SetDestination(this.GetPotAccessPoint().position, true);
			yield return new WaitForEndOfFrame();
			yield return new WaitUntil(() => !base.Npc.Movement.IsMoving);
			this.CurrentState = PotActionBehaviour.EState.Idle;
			this.walkToPotRoutine = null;
			yield break;
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x000811EE File Offset: 0x0007F3EE
		[CompilerGenerated]
		private IEnumerator <PerformAction>g__Routine|41_0()
		{
			this.AssignedPot.SetNPCUser(this.botanist.NetworkObject);
			base.Npc.Movement.FacePoint(this.AssignedPot.transform.position, 0.5f);
			string actionAnimation = this.GetActionAnimation(this.CurrentActionType);
			if (actionAnimation != string.Empty)
			{
				this.currentActionAnimation = actionAnimation;
				base.Npc.SetAnimationBool_Networked(null, actionAnimation, true);
			}
			if (this.CurrentActionType == PotActionBehaviour.EActionType.SowSeed && !base.Npc.Avatar.Anim.IsCrouched)
			{
				base.Npc.SetCrouched_Networked(true);
			}
			AvatarEquippable actionEquippable = this.GetActionEquippable(this.CurrentActionType);
			if (actionEquippable != null)
			{
				this.currentActionEquippable = base.Npc.SetEquippable_Networked_Return(null, actionEquippable.AssetPath);
			}
			float waitTime = this.GetWaitTime(this.CurrentActionType);
			for (float i = 0f; i < waitTime; i += Time.deltaTime)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.AssignedPot.transform.position, 0, false);
				yield return new WaitForEndOfFrame();
			}
			this.StopPerformAction();
			this.CompleteAction();
			yield break;
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x000811FD File Offset: 0x0007F3FD
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PotActionBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PotActionBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x00081216 File Offset: 0x0007F416
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PotActionBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PotActionBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x0008122F File Offset: 0x0007F42F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x0008123D File Offset: 0x0007F43D
		protected override void dll()
		{
			base.Awake();
			this.botanist = (base.Npc as Botanist);
		}

		// Token: 0x0400187C RID: 6268
		[HideInInspector]
		public int AdditiveNumber = -1;

		// Token: 0x0400187D RID: 6269
		[Header("Equippables")]
		public AvatarEquippable WateringCanEquippable;

		// Token: 0x0400187E RID: 6270
		public AvatarEquippable TrimmersEquippable;

		// Token: 0x0400187F RID: 6271
		private Botanist botanist;

		// Token: 0x04001880 RID: 6272
		private Coroutine walkToSuppliesRoutine;

		// Token: 0x04001881 RID: 6273
		private Coroutine grabRoutine;

		// Token: 0x04001882 RID: 6274
		private Coroutine walkToPotRoutine;

		// Token: 0x04001883 RID: 6275
		private Coroutine performActionRoutine;

		// Token: 0x04001884 RID: 6276
		private string currentActionAnimation = string.Empty;

		// Token: 0x04001885 RID: 6277
		private AvatarEquippable currentActionEquippable;

		// Token: 0x04001886 RID: 6278
		private bool dll_Excuted;

		// Token: 0x04001887 RID: 6279
		private bool dll_Excuted;

		// Token: 0x02000540 RID: 1344
		public enum EActionType
		{
			// Token: 0x04001889 RID: 6281
			None,
			// Token: 0x0400188A RID: 6282
			PourSoil,
			// Token: 0x0400188B RID: 6283
			SowSeed,
			// Token: 0x0400188C RID: 6284
			Water,
			// Token: 0x0400188D RID: 6285
			ApplyAdditive,
			// Token: 0x0400188E RID: 6286
			Harvest
		}

		// Token: 0x02000541 RID: 1345
		public enum EState
		{
			// Token: 0x04001890 RID: 6288
			Idle,
			// Token: 0x04001891 RID: 6289
			WalkingToSupplies,
			// Token: 0x04001892 RID: 6290
			GrabbingSupplies,
			// Token: 0x04001893 RID: 6291
			WalkingToPot,
			// Token: 0x04001894 RID: 6292
			PerformingAction,
			// Token: 0x04001895 RID: 6293
			WalkingToDestination
		}
	}
}
