using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Employees;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000549 RID: 1353
	public class StartChemistryStationBehaviour : Behaviour
	{
		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x00081E77 File Offset: 0x00080077
		// (set) Token: 0x06001F88 RID: 8072 RVA: 0x00081E7F File Offset: 0x0008007F
		public ChemistryStation targetStation { get; private set; }

		// Token: 0x06001F89 RID: 8073 RVA: 0x00081E88 File Offset: 0x00080088
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.StartChemistryStationBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x00081E9C File Offset: 0x0008009C
		public void SetTargetStation(ChemistryStation station)
		{
			this.targetStation = station;
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x00081EA8 File Offset: 0x000800A8
		protected override void End()
		{
			base.End();
			if (this.beaker != null)
			{
				this.beaker.Destroy();
				this.beaker = null;
			}
			if (this.targetStation != null)
			{
				this.targetStation.StaticBeaker.gameObject.SetActive(true);
			}
			if (this.cookRoutine != null)
			{
				this.StopCook();
			}
			this.Disable();
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x00081F14 File Offset: 0x00080114
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.cookRoutine != null)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!base.Npc.Movement.IsMoving)
			{
				if (this.IsAtStation())
				{
					this.StartCook();
					return;
				}
				base.SetDestination(this.GetStationAccessPoint(), true);
			}
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x00081F66 File Offset: 0x00080166
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			if (this.cookRoutine != null)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.targetStation.UIPoint.position, 5, false);
			}
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x00081F9D File Offset: 0x0008019D
		[ObserversRpc(RunLocally = true)]
		private void StartCook()
		{
			this.RpcWriter___Observers_StartCook_2166136261();
			this.RpcLogic___StartCook_2166136261();
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x00081FAC File Offset: 0x000801AC
		private void SetupBeaker()
		{
			if (this.beaker != null)
			{
				Console.LogWarning("Beaker already exists!", null);
				return;
			}
			this.beaker = this.targetStation.CreateBeaker();
			this.targetStation.StaticBeaker.gameObject.SetActive(false);
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x00081FFC File Offset: 0x000801FC
		private void FillBeaker(StationRecipe recipe, Beaker beaker)
		{
			for (int i = 0; i < recipe.Ingredients.Count; i++)
			{
				StorableItemDefinition storableItemDefinition = null;
				foreach (ItemDefinition itemDefinition in recipe.Ingredients[i].Items)
				{
					StorableItemDefinition storableItemDefinition2 = itemDefinition as StorableItemDefinition;
					for (int j = 0; j < this.targetStation.IngredientSlots.Length; j++)
					{
						if (this.targetStation.IngredientSlots[j].ItemInstance != null && this.targetStation.IngredientSlots[j].ItemInstance.Definition.ID == storableItemDefinition2.ID)
						{
							storableItemDefinition = storableItemDefinition2;
							break;
						}
					}
				}
				if (storableItemDefinition.StationItem == null)
				{
					Console.LogError("Ingredient '" + storableItemDefinition.Name + "' does not have a station item", null);
				}
				else
				{
					StationItem stationItem = storableItemDefinition.StationItem;
					if (!stationItem.HasModule<IngredientModule>())
					{
						if (stationItem.HasModule<PourableModule>())
						{
							PourableModule module = stationItem.GetModule<PourableModule>();
							beaker.Fillable.AddLiquid(module.LiquidType, module.LiquidCapacity_L, module.LiquidColor);
						}
						else
						{
							Console.LogError("Ingredient '" + storableItemDefinition.Name + "' does not have an ingredient or pourable module", null);
						}
					}
				}
			}
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x00082160 File Offset: 0x00080360
		private bool CanCookStart()
		{
			if (this.targetStation == null)
			{
				return false;
			}
			if (((IUsable)this.targetStation).IsInUse && ((IUsable)this.targetStation).NPCUserObject != base.Npc.NetworkObject)
			{
				return false;
			}
			ChemistryStationConfiguration chemistryStationConfiguration = this.targetStation.Configuration as ChemistryStationConfiguration;
			return !(chemistryStationConfiguration.Recipe.SelectedRecipe == null) && this.targetStation.HasIngredientsForRecipe(chemistryStationConfiguration.Recipe.SelectedRecipe);
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x000821EA File Offset: 0x000803EA
		private void StopCook()
		{
			this.targetStation.SetNPCUser(null);
			base.Npc.SetAnimationBool_Networked(null, "UseChemistryStation", false);
			if (this.cookRoutine != null)
			{
				base.StopCoroutine(this.cookRoutine);
				this.cookRoutine = null;
			}
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x00082225 File Offset: 0x00080425
		private Vector3 GetStationAccessPoint()
		{
			if (this.targetStation == null)
			{
				return base.Npc.transform.position;
			}
			return ((ITransitEntity)this.targetStation).AccessPoints[0].position;
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x00082258 File Offset: 0x00080458
		private bool IsAtStation()
		{
			return !(this.targetStation == null) && Vector3.Distance(base.Npc.transform.position, this.GetStationAccessPoint()) < 1f;
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x0008228C File Offset: 0x0008048C
		[CompilerGenerated]
		private IEnumerator <StartCook>g__CookRoutine|15_0()
		{
			base.Npc.Movement.FacePoint(this.targetStation.transform.position, 0.5f);
			yield return new WaitForSeconds(0.5f);
			base.Npc.SetAnimationBool_Networked(null, "UseChemistryStation", true);
			if (!this.CanCookStart())
			{
				this.StopCook();
				base.End_Networked(null);
				yield break;
			}
			this.targetStation.SetNPCUser(base.Npc.NetworkObject);
			StationRecipe recipe = (this.targetStation.Configuration as ChemistryStationConfiguration).Recipe.SelectedRecipe;
			this.SetupBeaker();
			yield return new WaitForSeconds(1f);
			this.FillBeaker(recipe, this.beaker);
			yield return new WaitForSeconds(8f);
			yield return new WaitForSeconds(6f);
			yield return new WaitForSeconds(6f);
			List<ItemInstance> list = new List<ItemInstance>();
			for (int i = 0; i < recipe.Ingredients.Count; i++)
			{
				foreach (ItemDefinition itemDefinition in recipe.Ingredients[i].Items)
				{
					StorableItemDefinition storableItemDefinition = itemDefinition as StorableItemDefinition;
					for (int j = 0; j < this.targetStation.IngredientSlots.Length; j++)
					{
						if (this.targetStation.IngredientSlots[j].ItemInstance != null && this.targetStation.IngredientSlots[j].ItemInstance.Definition.ID == storableItemDefinition.ID)
						{
							list.Add(this.targetStation.IngredientSlots[j].ItemInstance.GetCopy(recipe.Ingredients[i].Quantity));
							this.targetStation.IngredientSlots[j].ChangeQuantity(-recipe.Ingredients[i].Quantity, false);
							break;
						}
					}
				}
			}
			EQuality productQuality = recipe.CalculateQuality(list);
			this.targetStation.SendCookOperation(new ChemistryCookOperation(recipe, productQuality, this.beaker.Container.LiquidColor, this.beaker.Fillable.LiquidContainer.CurrentLiquidLevel, 0));
			this.beaker.Destroy();
			this.beaker = null;
			this.StopCook();
			base.End_Networked(null);
			yield break;
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x0008229B File Offset: 0x0008049B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartChemistryStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartChemistryStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_StartCook_2166136261));
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x000822CB File Offset: 0x000804CB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartChemistryStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartChemistryStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x000822E4 File Offset: 0x000804E4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x000822F4 File Offset: 0x000804F4
		private void RpcWriter___Observers_StartCook_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x0008239D File Offset: 0x0008059D
		private void RpcLogic___StartCook_2166136261()
		{
			if (this.cookRoutine != null)
			{
				return;
			}
			if (this.targetStation == null)
			{
				return;
			}
			this.cookRoutine = base.StartCoroutine(this.<StartCook>g__CookRoutine|15_0());
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x000823CC File Offset: 0x000805CC
		private void RpcReader___Observers_StartCook_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartCook_2166136261();
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x000823F6 File Offset: 0x000805F6
		protected override void dll()
		{
			base.Awake();
			this.chemist = (base.Npc as Chemist);
		}

		// Token: 0x040018B1 RID: 6321
		public const float PLACE_INGREDIENTS_TIME = 8f;

		// Token: 0x040018B2 RID: 6322
		public const float STIR_TIME = 6f;

		// Token: 0x040018B3 RID: 6323
		public const float BURNER_TIME = 6f;

		// Token: 0x040018B5 RID: 6325
		private Chemist chemist;

		// Token: 0x040018B6 RID: 6326
		private Coroutine cookRoutine;

		// Token: 0x040018B7 RID: 6327
		private Beaker beaker;

		// Token: 0x040018B8 RID: 6328
		private bool dll_Excuted;

		// Token: 0x040018B9 RID: 6329
		private bool dll_Excuted;
	}
}
