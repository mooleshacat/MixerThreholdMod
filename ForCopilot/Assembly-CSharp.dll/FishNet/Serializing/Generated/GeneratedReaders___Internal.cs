using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FishNet.Object;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.Casino;
using ScheduleOne.Clothing;
using ScheduleOne.Combat;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.Doors;
using ScheduleOne.Economy;
using ScheduleOne.Employees;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.Levelling;
using ScheduleOne.Management;
using ScheduleOne.Messaging;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Property;
using ScheduleOne.Quests;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.UI.Handover;
using ScheduleOne.UI.Phone.Messages;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.Modification;
using ScheduleOne.Vision;
using UnityEngine;

namespace FishNet.Serializing.Generated
{
	// Token: 0x02000CBD RID: 3261
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedReaders___Internal
	{
		// Token: 0x06005B6F RID: 23407 RVA: 0x00182168 File Offset: 0x00180368
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeOnce()
		{
			GenericReader<ItemInstance>.Read = new Func<Reader, ItemInstance>(ItemSerializers.ReadItemInstance);
			GenericReader<StorableItemInstance>.Read = new Func<Reader, StorableItemInstance>(ItemSerializers.ReadStorableItemInstance);
			GenericReader<CashInstance>.Read = new Func<Reader, CashInstance>(ItemSerializers.ReadCashInstance);
			GenericReader<QualityItemInstance>.Read = new Func<Reader, QualityItemInstance>(ItemSerializers.ReadQualityItemInstance);
			GenericReader<ClothingInstance>.Read = new Func<Reader, ClothingInstance>(ItemSerializers.ReadClothingInstance);
			GenericReader<ProductItemInstance>.Read = new Func<Reader, ProductItemInstance>(ItemSerializers.ReadProductItemInstance);
			GenericReader<WeedInstance>.Read = new Func<Reader, WeedInstance>(ItemSerializers.ReadWeedInstance);
			GenericReader<MethInstance>.Read = new Func<Reader, MethInstance>(ItemSerializers.ReadMethInstance);
			GenericReader<CocaineInstance>.Read = new Func<Reader, CocaineInstance>(ItemSerializers.ReadCocaineInstance);
			GenericReader<IntegerItemInstance>.Read = new Func<Reader, IntegerItemInstance>(ItemSerializers.ReadIntegerItemInstance);
			GenericReader<WateringCanInstance>.Read = new Func<Reader, WateringCanInstance>(ItemSerializers.ReadWateringCanInstance);
			GenericReader<TrashGrabberInstance>.Read = new Func<Reader, TrashGrabberInstance>(ItemSerializers.ReadTrashGrabberInstance);
			GenericReader<VisionEventReceipt>.Read = new Func<Reader, VisionEventReceipt>(GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generateds);
			GenericReader<PlayerVisualState.EVisualState>.Read = new Func<Reader, PlayerVisualState.EVisualState>(GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generateds);
			GenericReader<VisionCone.EEventLevel>.Read = new Func<Reader, VisionCone.EEventLevel>(GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generateds);
			GenericReader<ContractInfo>.Read = new Func<Reader, ContractInfo>(GeneratedReaders___Internal.Read___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generateds);
			GenericReader<ProductList>.Read = new Func<Reader, ProductList>(GeneratedReaders___Internal.Read___ScheduleOne.Product.ProductListFishNet.Serializing.Generateds);
			GenericReader<ProductList.Entry>.Read = new Func<Reader, ProductList.Entry>(GeneratedReaders___Internal.Read___ScheduleOne.Product.ProductList/EntryFishNet.Serializing.Generateds);
			GenericReader<EQuality>.Read = new Func<Reader, EQuality>(GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds);
			GenericReader<List<ProductList.Entry>>.Read = new Func<Reader, List<ProductList.Entry>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Product.ProductList/Entry>FishNet.Serializing.Generateds);
			GenericReader<QuestWindowConfig>.Read = new Func<Reader, QuestWindowConfig>(GeneratedReaders___Internal.Read___ScheduleOne.Quests.QuestWindowConfigFishNet.Serializing.Generateds);
			GenericReader<GameDateTime>.Read = new Func<Reader, GameDateTime>(GeneratedReaders___Internal.Read___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generateds);
			GenericReader<QuestManager.EQuestAction>.Read = new Func<Reader, QuestManager.EQuestAction>(GeneratedReaders___Internal.Read___ScheduleOne.Quests.QuestManager/EQuestActionFishNet.Serializing.Generateds);
			GenericReader<EQuestState>.Read = new Func<Reader, EQuestState>(GeneratedReaders___Internal.Read___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generateds);
			GenericReader<Impact>.Read = new Func<Reader, Impact>(GeneratedReaders___Internal.Read___ScheduleOne.Combat.ImpactFishNet.Serializing.Generateds);
			GenericReader<EImpactType>.Read = new Func<Reader, EImpactType>(GeneratedReaders___Internal.Read___ScheduleOne.Combat.EImpactTypeFishNet.Serializing.Generateds);
			GenericReader<LandVehicle>.Read = new Func<Reader, LandVehicle>(GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generateds);
			GenericReader<CheckpointManager.ECheckpointLocation>.Read = new Func<Reader, CheckpointManager.ECheckpointLocation>(GeneratedReaders___Internal.Read___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generateds);
			GenericReader<SlotFilter>.Read = new Func<Reader, SlotFilter>(GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generateds);
			GenericReader<SlotFilter.EType>.Read = new Func<Reader, SlotFilter.EType>(GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilter/ETypeFishNet.Serializing.Generateds);
			GenericReader<List<string>>.Read = new Func<Reader, List<string>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds);
			GenericReader<List<EQuality>>.Read = new Func<Reader, List<EQuality>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.EQuality>FishNet.Serializing.Generateds);
			GenericReader<Player>.Read = new Func<Reader, Player>(GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerFishNet.Serializing.Generateds);
			GenericReader<StringIntPair>.Read = new Func<Reader, StringIntPair>(GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.StringIntPairFishNet.Serializing.Generateds);
			GenericReader<StringIntPair[]>.Read = new Func<Reader, StringIntPair[]>(GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generateds);
			GenericReader<Message>.Read = new Func<Reader, Message>(GeneratedReaders___Internal.Read___ScheduleOne.Messaging.MessageFishNet.Serializing.Generateds);
			GenericReader<Message.ESenderType>.Read = new Func<Reader, Message.ESenderType>(GeneratedReaders___Internal.Read___ScheduleOne.Messaging.Message/ESenderTypeFishNet.Serializing.Generateds);
			GenericReader<MessageChain>.Read = new Func<Reader, MessageChain>(GeneratedReaders___Internal.Read___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generateds);
			GenericReader<MSGConversationData>.Read = new Func<Reader, MSGConversationData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.MSGConversationDataFishNet.Serializing.Generateds);
			GenericReader<TextMessageData>.Read = new Func<Reader, TextMessageData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextMessageDataFishNet.Serializing.Generateds);
			GenericReader<TextMessageData[]>.Read = new Func<Reader, TextMessageData[]>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextMessageData[]FishNet.Serializing.Generateds);
			GenericReader<TextResponseData>.Read = new Func<Reader, TextResponseData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextResponseDataFishNet.Serializing.Generateds);
			GenericReader<TextResponseData[]>.Read = new Func<Reader, TextResponseData[]>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextResponseData[]FishNet.Serializing.Generateds);
			GenericReader<Response>.Read = new Func<Reader, Response>(GeneratedReaders___Internal.Read___ScheduleOne.Messaging.ResponseFishNet.Serializing.Generateds);
			GenericReader<List<Response>>.Read = new Func<Reader, List<Response>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generateds);
			GenericReader<List<NetworkObject>>.Read = new Func<Reader, List<NetworkObject>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generateds);
			GenericReader<AdvancedTransitRouteData>.Read = new Func<Reader, AdvancedTransitRouteData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.AdvancedTransitRouteDataFishNet.Serializing.Generateds);
			GenericReader<ManagementItemFilter.EMode>.Read = new Func<Reader, ManagementItemFilter.EMode>(GeneratedReaders___Internal.Read___ScheduleOne.Management.ManagementItemFilter/EModeFishNet.Serializing.Generateds);
			GenericReader<AdvancedTransitRouteData[]>.Read = new Func<Reader, AdvancedTransitRouteData[]>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generateds);
			GenericReader<ERank>.Read = new Func<Reader, ERank>(GeneratedReaders___Internal.Read___ScheduleOne.Levelling.ERankFishNet.Serializing.Generateds);
			GenericReader<FullRank>.Read = new Func<Reader, FullRank>(GeneratedReaders___Internal.Read___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generateds);
			GenericReader<PlayerData>.Read = new Func<Reader, PlayerData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generateds);
			GenericReader<VariableData>.Read = new Func<Reader, VariableData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.VariableDataFishNet.Serializing.Generateds);
			GenericReader<VariableData[]>.Read = new Func<Reader, VariableData[]>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generateds);
			GenericReader<AvatarSettings>.Read = new Func<Reader, AvatarSettings>(GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generateds);
			GenericReader<Eye.EyeLidConfiguration>.Read = new Func<Reader, Eye.EyeLidConfiguration>(GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generateds);
			GenericReader<AvatarSettings.LayerSetting>.Read = new Func<Reader, AvatarSettings.LayerSetting>(GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.AvatarSettings/LayerSettingFishNet.Serializing.Generateds);
			GenericReader<List<AvatarSettings.LayerSetting>>.Read = new Func<Reader, List<AvatarSettings.LayerSetting>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generateds);
			GenericReader<AvatarSettings.AccessorySetting>.Read = new Func<Reader, AvatarSettings.AccessorySetting>(GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.AvatarSettings/AccessorySettingFishNet.Serializing.Generateds);
			GenericReader<List<AvatarSettings.AccessorySetting>>.Read = new Func<Reader, List<AvatarSettings.AccessorySetting>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/AccessorySetting>FishNet.Serializing.Generateds);
			GenericReader<BasicAvatarSettings>.Read = new Func<Reader, BasicAvatarSettings>(GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generateds);
			GenericReader<PlayerCrimeData.EPursuitLevel>.Read = new Func<Reader, PlayerCrimeData.EPursuitLevel>(GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerCrimeData/EPursuitLevelFishNet.Serializing.Generateds);
			GenericReader<Property>.Read = new Func<Reader, Property>(GeneratedReaders___Internal.Read___ScheduleOne.Property.PropertyFishNet.Serializing.Generateds);
			GenericReader<EEmployeeType>.Read = new Func<Reader, EEmployeeType>(GeneratedReaders___Internal.Read___ScheduleOne.Employees.EEmployeeTypeFishNet.Serializing.Generateds);
			GenericReader<EDealWindow>.Read = new Func<Reader, EDealWindow>(GeneratedReaders___Internal.Read___ScheduleOne.Economy.EDealWindowFishNet.Serializing.Generateds);
			GenericReader<HandoverScreen.EHandoverOutcome>.Read = new Func<Reader, HandoverScreen.EHandoverOutcome>(GeneratedReaders___Internal.Read___ScheduleOne.UI.Handover.HandoverScreen/EHandoverOutcomeFishNet.Serializing.Generateds);
			GenericReader<List<ItemInstance>>.Read = new Func<Reader, List<ItemInstance>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.ItemInstance>FishNet.Serializing.Generateds);
			GenericReader<ScheduleOne.Persistence.Datas.CustomerData>.Read = new Func<Reader, ScheduleOne.Persistence.Datas.CustomerData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.CustomerDataFishNet.Serializing.Generateds);
			GenericReader<string[]>.Read = new Func<Reader, string[]>(GeneratedReaders___Internal.Read___System.String[]FishNet.Serializing.Generateds);
			GenericReader<float[]>.Read = new Func<Reader, float[]>(GeneratedReaders___Internal.Read___System.Single[]FishNet.Serializing.Generateds);
			GenericReader<EDrugType>.Read = new Func<Reader, EDrugType>(GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds);
			GenericReader<GameData>.Read = new Func<Reader, GameData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.GameDataFishNet.Serializing.Generateds);
			GenericReader<GameSettings>.Read = new Func<Reader, GameSettings>(GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.GameSettingsFishNet.Serializing.Generateds);
			GenericReader<DeliveryInstance>.Read = new Func<Reader, DeliveryInstance>(GeneratedReaders___Internal.Read___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generateds);
			GenericReader<EDeliveryStatus>.Read = new Func<Reader, EDeliveryStatus>(GeneratedReaders___Internal.Read___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generateds);
			GenericReader<ExplosionData>.Read = new Func<Reader, ExplosionData>(GeneratedReaders___Internal.Read___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generateds);
			GenericReader<PlayingCard.ECardSuit>.Read = new Func<Reader, PlayingCard.ECardSuit>(GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generateds);
			GenericReader<PlayingCard.ECardValue>.Read = new Func<Reader, PlayingCard.ECardValue>(GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generateds);
			GenericReader<NetworkObject[]>.Read = new Func<Reader, NetworkObject[]>(GeneratedReaders___Internal.Read___FishNet.Object.NetworkObject[]FishNet.Serializing.Generateds);
			GenericReader<RTBGameController.EStage>.Read = new Func<Reader, RTBGameController.EStage>(GeneratedReaders___Internal.Read___ScheduleOne.Casino.RTBGameController/EStageFishNet.Serializing.Generateds);
			GenericReader<SlotMachine.ESymbol>.Read = new Func<Reader, SlotMachine.ESymbol>(GeneratedReaders___Internal.Read___ScheduleOne.Casino.SlotMachine/ESymbolFishNet.Serializing.Generateds);
			GenericReader<SlotMachine.ESymbol[]>.Read = new Func<Reader, SlotMachine.ESymbol[]>(GeneratedReaders___Internal.Read___ScheduleOne.Casino.SlotMachine/ESymbol[]FishNet.Serializing.Generateds);
			GenericReader<EDoorSide>.Read = new Func<Reader, EDoorSide>(GeneratedReaders___Internal.Read___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generateds);
			GenericReader<EVehicleColor>.Read = new Func<Reader, EVehicleColor>(GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generateds);
			GenericReader<ParkData>.Read = new Func<Reader, ParkData>(GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generateds);
			GenericReader<EParkingAlignment>.Read = new Func<Reader, EParkingAlignment>(GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.EParkingAlignmentFishNet.Serializing.Generateds);
			GenericReader<TrashContentData>.Read = new Func<Reader, TrashContentData>(GeneratedReaders___Internal.Read___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generateds);
			GenericReader<int[]>.Read = new Func<Reader, int[]>(GeneratedReaders___Internal.Read___System.Int32[]FishNet.Serializing.Generateds);
			GenericReader<Coordinate>.Read = new Func<Reader, Coordinate>(GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generateds);
			GenericReader<WeedAppearanceSettings>.Read = new Func<Reader, WeedAppearanceSettings>(GeneratedReaders___Internal.Read___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generateds);
			GenericReader<CocaineAppearanceSettings>.Read = new Func<Reader, CocaineAppearanceSettings>(GeneratedReaders___Internal.Read___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generateds);
			GenericReader<MethAppearanceSettings>.Read = new Func<Reader, MethAppearanceSettings>(GeneratedReaders___Internal.Read___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generateds);
			GenericReader<NewMixOperation>.Read = new Func<Reader, NewMixOperation>(GeneratedReaders___Internal.Read___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generateds);
			GenericReader<Recycler.EState>.Read = new Func<Reader, Recycler.EState>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generateds);
			GenericReader<Jukebox.JukeboxState>.Read = new Func<Reader, Jukebox.JukeboxState>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Jukebox/JukeboxStateFishNet.Serializing.Generateds);
			GenericReader<Jukebox.ERepeatMode>.Read = new Func<Reader, Jukebox.ERepeatMode>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Jukebox/ERepeatModeFishNet.Serializing.Generateds);
			GenericReader<CoordinateProceduralTilePair>.Read = new Func<Reader, CoordinateProceduralTilePair>(GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateProceduralTilePairFishNet.Serializing.Generateds);
			GenericReader<List<CoordinateProceduralTilePair>>.Read = new Func<Reader, List<CoordinateProceduralTilePair>>(GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generateds);
			GenericReader<ChemistryCookOperation>.Read = new Func<Reader, ChemistryCookOperation>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generateds);
			GenericReader<DryingOperation>.Read = new Func<Reader, DryingOperation>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generateds);
			GenericReader<OvenCookOperation>.Read = new Func<Reader, OvenCookOperation>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generateds);
			GenericReader<MixOperation>.Read = new Func<Reader, MixOperation>(GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generateds);
		}

		// Token: 0x06005B70 RID: 23408 RVA: 0x0018283C File Offset: 0x00180A3C
		public static VisionEventReceipt Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new VisionEventReceipt
			{
				TargetPlayer = reader.ReadNetworkObject(),
				State = GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x06005B71 RID: 23409 RVA: 0x00182894 File Offset: 0x00180A94
		public static PlayerVisualState.EVisualState Generateds(Reader reader)
		{
			return (PlayerVisualState.EVisualState)reader.ReadInt32(1);
		}

		// Token: 0x06005B72 RID: 23410 RVA: 0x001828B0 File Offset: 0x00180AB0
		public static VisionCone.EEventLevel Generateds(Reader reader)
		{
			return (VisionCone.EEventLevel)reader.ReadInt32(1);
		}

		// Token: 0x06005B73 RID: 23411 RVA: 0x001828CC File Offset: 0x00180ACC
		public static ContractInfo Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ContractInfo
			{
				Payment = reader.ReadSingle(0),
				Products = GeneratedReaders___Internal.Read___ScheduleOne.Product.ProductListFishNet.Serializing.Generateds(reader),
				DeliveryLocationGUID = reader.ReadString(),
				DeliveryWindow = GeneratedReaders___Internal.Read___ScheduleOne.Quests.QuestWindowConfigFishNet.Serializing.Generateds(reader),
				Expires = reader.ReadBoolean(),
				ExpiresAfter = reader.ReadInt32(1),
				PickupScheduleIndex = reader.ReadInt32(1),
				IsCounterOffer = reader.ReadBoolean()
			};
		}

		// Token: 0x06005B74 RID: 23412 RVA: 0x001829A0 File Offset: 0x00180BA0
		public static ProductList Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ProductList
			{
				entries = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Product.ProductList/Entry>FishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x06005B75 RID: 23413 RVA: 0x001829E8 File Offset: 0x00180BE8
		public static ProductList.Entry Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ProductList.Entry
			{
				ProductID = reader.ReadString(),
				Quality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(reader),
				Quantity = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005B76 RID: 23414 RVA: 0x00182A58 File Offset: 0x00180C58
		public static EQuality Generateds(Reader reader)
		{
			return (EQuality)reader.ReadInt32(1);
		}

		// Token: 0x06005B77 RID: 23415 RVA: 0x00182A74 File Offset: 0x00180C74
		public static List<ProductList.Entry> List(Reader reader)
		{
			return reader.ReadListAllocated<ProductList.Entry>();
		}

		// Token: 0x06005B78 RID: 23416 RVA: 0x00182A8C File Offset: 0x00180C8C
		public static QuestWindowConfig Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new QuestWindowConfig
			{
				IsEnabled = reader.ReadBoolean(),
				WindowStartTime = reader.ReadInt32(1),
				WindowEndTime = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005B79 RID: 23417 RVA: 0x00182B00 File Offset: 0x00180D00
		public static GameDateTime Generateds(Reader reader)
		{
			return new GameDateTime
			{
				elapsedDays = reader.ReadInt32(1),
				time = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005B7A RID: 23418 RVA: 0x00182B4C File Offset: 0x00180D4C
		public static QuestManager.EQuestAction Generateds(Reader reader)
		{
			return (QuestManager.EQuestAction)reader.ReadInt32(1);
		}

		// Token: 0x06005B7B RID: 23419 RVA: 0x00182B68 File Offset: 0x00180D68
		public static EQuestState Generateds(Reader reader)
		{
			return (EQuestState)reader.ReadInt32(1);
		}

		// Token: 0x06005B7C RID: 23420 RVA: 0x00182B84 File Offset: 0x00180D84
		public static Impact Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new Impact
			{
				HitPoint = reader.ReadVector3(),
				ImpactForceDirection = reader.ReadVector3(),
				ImpactForce = reader.ReadSingle(0),
				ImpactDamage = reader.ReadSingle(0),
				ImpactType = GeneratedReaders___Internal.Read___ScheduleOne.Combat.EImpactTypeFishNet.Serializing.Generateds(reader),
				ImpactSource = reader.ReadNetworkObject(),
				ImpactID = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005B7D RID: 23421 RVA: 0x00182C44 File Offset: 0x00180E44
		public static EImpactType Generateds(Reader reader)
		{
			return (EImpactType)reader.ReadInt32(1);
		}

		// Token: 0x06005B7E RID: 23422 RVA: 0x00182C60 File Offset: 0x00180E60
		public static LandVehicle Generateds(Reader reader)
		{
			return (LandVehicle)reader.ReadNetworkBehaviour();
		}

		// Token: 0x06005B7F RID: 23423 RVA: 0x00182C78 File Offset: 0x00180E78
		public static CheckpointManager.ECheckpointLocation Generateds(Reader reader)
		{
			return (CheckpointManager.ECheckpointLocation)reader.ReadInt32(1);
		}

		// Token: 0x06005B80 RID: 23424 RVA: 0x00182C94 File Offset: 0x00180E94
		public static SlotFilter Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new SlotFilter
			{
				Type = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilter/ETypeFishNet.Serializing.Generateds(reader),
				ItemIDs = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(reader),
				AllowedQualities = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.EQuality>FishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x06005B81 RID: 23425 RVA: 0x00182D00 File Offset: 0x00180F00
		public static SlotFilter.EType Generateds(Reader reader)
		{
			return (SlotFilter.EType)reader.ReadInt32(1);
		}

		// Token: 0x06005B82 RID: 23426 RVA: 0x00182D1C File Offset: 0x00180F1C
		public static List<string> List(Reader reader)
		{
			return reader.ReadListAllocated<string>();
		}

		// Token: 0x06005B83 RID: 23427 RVA: 0x00182D34 File Offset: 0x00180F34
		public static List<EQuality> List(Reader reader)
		{
			return reader.ReadListAllocated<EQuality>();
		}

		// Token: 0x06005B84 RID: 23428 RVA: 0x00182D4C File Offset: 0x00180F4C
		public static Player Generateds(Reader reader)
		{
			return (Player)reader.ReadNetworkBehaviour();
		}

		// Token: 0x06005B85 RID: 23429 RVA: 0x00182D64 File Offset: 0x00180F64
		public static StringIntPair Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new StringIntPair
			{
				String = reader.ReadString(),
				Int = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005B86 RID: 23430 RVA: 0x00182DC0 File Offset: 0x00180FC0
		public static StringIntPair[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<StringIntPair>();
		}

		// Token: 0x06005B87 RID: 23431 RVA: 0x00182DD8 File Offset: 0x00180FD8
		public static Message Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new Message
			{
				messageId = reader.ReadInt32(1),
				text = reader.ReadString(),
				sender = GeneratedReaders___Internal.Read___ScheduleOne.Messaging.Message/ESenderTypeFishNet.Serializing.Generateds(reader),
				endOfGroup = reader.ReadBoolean()
			};
		}

		// Token: 0x06005B88 RID: 23432 RVA: 0x00182E58 File Offset: 0x00181058
		public static Message.ESenderType Generateds(Reader reader)
		{
			return (Message.ESenderType)reader.ReadInt32(1);
		}

		// Token: 0x06005B89 RID: 23433 RVA: 0x00182E74 File Offset: 0x00181074
		public static MessageChain Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new MessageChain
			{
				Messages = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(reader),
				id = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005B8A RID: 23434 RVA: 0x00182ED0 File Offset: 0x001810D0
		public static MSGConversationData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new MSGConversationData
			{
				ConversationIndex = reader.ReadInt32(1),
				Read = reader.ReadBoolean(),
				MessageHistory = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextMessageData[]FishNet.Serializing.Generateds(reader),
				ActiveResponses = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.TextResponseData[]FishNet.Serializing.Generateds(reader),
				IsHidden = reader.ReadBoolean(),
				DataType = reader.ReadString(),
				DataVersion = reader.ReadInt32(1),
				GameVersion = reader.ReadString()
			};
		}

		// Token: 0x06005B8B RID: 23435 RVA: 0x00182FA0 File Offset: 0x001811A0
		public static TextMessageData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new TextMessageData
			{
				Sender = reader.ReadInt32(1),
				MessageID = reader.ReadInt32(1),
				Text = reader.ReadString(),
				EndOfChain = reader.ReadBoolean()
			};
		}

		// Token: 0x06005B8C RID: 23436 RVA: 0x00183028 File Offset: 0x00181228
		public static TextMessageData[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<TextMessageData>();
		}

		// Token: 0x06005B8D RID: 23437 RVA: 0x00183040 File Offset: 0x00181240
		public static TextResponseData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new TextResponseData
			{
				Text = reader.ReadString(),
				Label = reader.ReadString()
			};
		}

		// Token: 0x06005B8E RID: 23438 RVA: 0x00183098 File Offset: 0x00181298
		public static TextResponseData[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<TextResponseData>();
		}

		// Token: 0x06005B8F RID: 23439 RVA: 0x001830B0 File Offset: 0x001812B0
		public static Response Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new Response
			{
				text = reader.ReadString(),
				label = reader.ReadString(),
				disableDefaultResponseBehaviour = reader.ReadBoolean()
			};
		}

		// Token: 0x06005B90 RID: 23440 RVA: 0x0018311C File Offset: 0x0018131C
		public static List<Response> List(Reader reader)
		{
			return reader.ReadListAllocated<Response>();
		}

		// Token: 0x06005B91 RID: 23441 RVA: 0x00183134 File Offset: 0x00181334
		public static List<NetworkObject> List(Reader reader)
		{
			return reader.ReadListAllocated<NetworkObject>();
		}

		// Token: 0x06005B92 RID: 23442 RVA: 0x0018314C File Offset: 0x0018134C
		public static AdvancedTransitRouteData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new AdvancedTransitRouteData
			{
				SourceGUID = reader.ReadString(),
				DestinationGUID = reader.ReadString(),
				FilterMode = GeneratedReaders___Internal.Read___ScheduleOne.Management.ManagementItemFilter/EModeFishNet.Serializing.Generateds(reader),
				FilterItemIDs = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x06005B93 RID: 23443 RVA: 0x001831C8 File Offset: 0x001813C8
		public static ManagementItemFilter.EMode Generateds(Reader reader)
		{
			return (ManagementItemFilter.EMode)reader.ReadInt32(1);
		}

		// Token: 0x06005B94 RID: 23444 RVA: 0x001831E4 File Offset: 0x001813E4
		public static AdvancedTransitRouteData[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<AdvancedTransitRouteData>();
		}

		// Token: 0x06005B95 RID: 23445 RVA: 0x001831FC File Offset: 0x001813FC
		public static ERank Generateds(Reader reader)
		{
			return (ERank)reader.ReadInt32(1);
		}

		// Token: 0x06005B96 RID: 23446 RVA: 0x00183218 File Offset: 0x00181418
		public static FullRank Generateds(Reader reader)
		{
			return new FullRank
			{
				Rank = GeneratedReaders___Internal.Read___ScheduleOne.Levelling.ERankFishNet.Serializing.Generateds(reader),
				Tier = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005B97 RID: 23447 RVA: 0x0018325C File Offset: 0x0018145C
		public static PlayerData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new PlayerData
			{
				PlayerCode = reader.ReadString(),
				Position = reader.ReadVector3(),
				Rotation = reader.ReadSingle(0),
				IntroCompleted = reader.ReadBoolean(),
				DataType = reader.ReadString(),
				DataVersion = reader.ReadInt32(1),
				GameVersion = reader.ReadString()
			};
		}

		// Token: 0x06005B98 RID: 23448 RVA: 0x00183318 File Offset: 0x00181518
		public static VariableData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new VariableData
			{
				Name = reader.ReadString(),
				Value = reader.ReadString(),
				DataType = reader.ReadString(),
				DataVersion = reader.ReadInt32(1),
				GameVersion = reader.ReadString()
			};
		}

		// Token: 0x06005B99 RID: 23449 RVA: 0x001833AC File Offset: 0x001815AC
		public static VariableData[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<VariableData>();
		}

		// Token: 0x06005B9A RID: 23450 RVA: 0x001833C4 File Offset: 0x001815C4
		public static AvatarSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			AvatarSettings avatarSettings = ScriptableObject.CreateInstance<AvatarSettings>();
			avatarSettings.SkinColor = reader.ReadColor(1);
			avatarSettings.Height = reader.ReadSingle(0);
			avatarSettings.Gender = reader.ReadSingle(0);
			avatarSettings.Weight = reader.ReadSingle(0);
			avatarSettings.HairPath = reader.ReadString();
			avatarSettings.HairColor = reader.ReadColor(1);
			avatarSettings.EyebrowScale = reader.ReadSingle(0);
			avatarSettings.EyebrowThickness = reader.ReadSingle(0);
			avatarSettings.EyebrowRestingHeight = reader.ReadSingle(0);
			avatarSettings.EyebrowRestingAngle = reader.ReadSingle(0);
			avatarSettings.LeftEyeLidColor = reader.ReadColor(1);
			avatarSettings.RightEyeLidColor = reader.ReadColor(1);
			avatarSettings.LeftEyeRestingState = GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generateds(reader);
			avatarSettings.RightEyeRestingState = GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generateds(reader);
			avatarSettings.EyeballMaterialIdentifier = reader.ReadString();
			avatarSettings.EyeBallTint = reader.ReadColor(1);
			avatarSettings.PupilDilation = reader.ReadSingle(0);
			avatarSettings.FaceLayerSettings = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generateds(reader);
			avatarSettings.BodyLayerSettings = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generateds(reader);
			avatarSettings.AccessorySettings = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/AccessorySetting>FishNet.Serializing.Generateds(reader);
			avatarSettings.UseCombinedLayer = reader.ReadBoolean();
			avatarSettings.CombinedLayerPath = reader.ReadString();
			return avatarSettings;
		}

		// Token: 0x06005B9B RID: 23451 RVA: 0x001835C4 File Offset: 0x001817C4
		public static Eye.EyeLidConfiguration Generateds(Reader reader)
		{
			return new Eye.EyeLidConfiguration
			{
				topLidOpen = reader.ReadSingle(0),
				bottomLidOpen = reader.ReadSingle(0)
			};
		}

		// Token: 0x06005B9C RID: 23452 RVA: 0x00183610 File Offset: 0x00181810
		public static AvatarSettings.LayerSetting Generateds(Reader reader)
		{
			return new AvatarSettings.LayerSetting
			{
				layerPath = reader.ReadString(),
				layerTint = reader.ReadColor(1)
			};
		}

		// Token: 0x06005B9D RID: 23453 RVA: 0x00183654 File Offset: 0x00181854
		public static List<AvatarSettings.LayerSetting> List(Reader reader)
		{
			return reader.ReadListAllocated<AvatarSettings.LayerSetting>();
		}

		// Token: 0x06005B9E RID: 23454 RVA: 0x0018366C File Offset: 0x0018186C
		public static AvatarSettings.AccessorySetting Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new AvatarSettings.AccessorySetting
			{
				path = reader.ReadString(),
				color = reader.ReadColor(1)
			};
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x001836C8 File Offset: 0x001818C8
		public static List<AvatarSettings.AccessorySetting> List(Reader reader)
		{
			return reader.ReadListAllocated<AvatarSettings.AccessorySetting>();
		}

		// Token: 0x06005BA0 RID: 23456 RVA: 0x001836E0 File Offset: 0x001818E0
		public static BasicAvatarSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			BasicAvatarSettings basicAvatarSettings = ScriptableObject.CreateInstance<BasicAvatarSettings>();
			basicAvatarSettings.Gender = reader.ReadInt32(1);
			basicAvatarSettings.Weight = reader.ReadSingle(0);
			basicAvatarSettings.SkinColor = reader.ReadColor(1);
			basicAvatarSettings.HairStyle = reader.ReadString();
			basicAvatarSettings.HairColor = reader.ReadColor(1);
			basicAvatarSettings.Mouth = reader.ReadString();
			basicAvatarSettings.FacialHair = reader.ReadString();
			basicAvatarSettings.FacialDetails = reader.ReadString();
			basicAvatarSettings.FacialDetailsIntensity = reader.ReadSingle(0);
			basicAvatarSettings.EyeballColor = reader.ReadColor(1);
			basicAvatarSettings.UpperEyeLidRestingPosition = reader.ReadSingle(0);
			basicAvatarSettings.LowerEyeLidRestingPosition = reader.ReadSingle(0);
			basicAvatarSettings.PupilDilation = reader.ReadSingle(0);
			basicAvatarSettings.EyebrowScale = reader.ReadSingle(0);
			basicAvatarSettings.EyebrowThickness = reader.ReadSingle(0);
			basicAvatarSettings.EyebrowRestingHeight = reader.ReadSingle(0);
			basicAvatarSettings.EyebrowRestingAngle = reader.ReadSingle(0);
			basicAvatarSettings.Top = reader.ReadString();
			basicAvatarSettings.TopColor = reader.ReadColor(1);
			basicAvatarSettings.Bottom = reader.ReadString();
			basicAvatarSettings.BottomColor = reader.ReadColor(1);
			basicAvatarSettings.Shoes = reader.ReadString();
			basicAvatarSettings.ShoesColor = reader.ReadColor(1);
			basicAvatarSettings.Headwear = reader.ReadString();
			basicAvatarSettings.HeadwearColor = reader.ReadColor(1);
			basicAvatarSettings.Eyewear = reader.ReadString();
			basicAvatarSettings.EyewearColor = reader.ReadColor(1);
			basicAvatarSettings.Tattoos = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(reader);
			return basicAvatarSettings;
		}

		// Token: 0x06005BA1 RID: 23457 RVA: 0x00183968 File Offset: 0x00181B68
		public static PlayerCrimeData.EPursuitLevel Generateds(Reader reader)
		{
			return (PlayerCrimeData.EPursuitLevel)reader.ReadInt32(1);
		}

		// Token: 0x06005BA2 RID: 23458 RVA: 0x00183984 File Offset: 0x00181B84
		public static Property Generateds(Reader reader)
		{
			return (Property)reader.ReadNetworkBehaviour();
		}

		// Token: 0x06005BA3 RID: 23459 RVA: 0x0018399C File Offset: 0x00181B9C
		public static EEmployeeType Generateds(Reader reader)
		{
			return (EEmployeeType)reader.ReadInt32(1);
		}

		// Token: 0x06005BA4 RID: 23460 RVA: 0x001839B8 File Offset: 0x00181BB8
		public static EDealWindow Generateds(Reader reader)
		{
			return (EDealWindow)reader.ReadInt32(1);
		}

		// Token: 0x06005BA5 RID: 23461 RVA: 0x001839D4 File Offset: 0x00181BD4
		public static HandoverScreen.EHandoverOutcome Generateds(Reader reader)
		{
			return (HandoverScreen.EHandoverOutcome)reader.ReadInt32(1);
		}

		// Token: 0x06005BA6 RID: 23462 RVA: 0x001839F0 File Offset: 0x00181BF0
		public static List<ItemInstance> List(Reader reader)
		{
			return reader.ReadListAllocated<ItemInstance>();
		}

		// Token: 0x06005BA7 RID: 23463 RVA: 0x00183A08 File Offset: 0x00181C08
		public static ScheduleOne.Persistence.Datas.CustomerData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ScheduleOne.Persistence.Datas.CustomerData
			{
				Dependence = reader.ReadSingle(0),
				PurchaseableProducts = GeneratedReaders___Internal.Read___System.String[]FishNet.Serializing.Generateds(reader),
				ProductAffinities = GeneratedReaders___Internal.Read___System.Single[]FishNet.Serializing.Generateds(reader),
				TimeSinceLastDealCompleted = reader.ReadInt32(1),
				TimeSinceLastDealOffered = reader.ReadInt32(1),
				OfferedDeals = reader.ReadInt32(1),
				CompletedDeals = reader.ReadInt32(1),
				IsContractOffered = reader.ReadBoolean(),
				OfferedContract = GeneratedReaders___Internal.Read___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generateds(reader),
				OfferedContractTime = GeneratedReaders___Internal.Read___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generateds(reader),
				TimeSincePlayerApproached = reader.ReadInt32(1),
				TimeSinceInstantDealOffered = reader.ReadInt32(1),
				HasBeenRecommended = reader.ReadBoolean(),
				DataType = reader.ReadString(),
				DataVersion = reader.ReadInt32(1),
				GameVersion = reader.ReadString()
			};
		}

		// Token: 0x06005BA8 RID: 23464 RVA: 0x00183B84 File Offset: 0x00181D84
		public static string[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<string>();
		}

		// Token: 0x06005BA9 RID: 23465 RVA: 0x00183B9C File Offset: 0x00181D9C
		public static float[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<float>();
		}

		// Token: 0x06005BAA RID: 23466 RVA: 0x00183BB4 File Offset: 0x00181DB4
		public static EDrugType Generateds(Reader reader)
		{
			return (EDrugType)reader.ReadInt32(1);
		}

		// Token: 0x06005BAB RID: 23467 RVA: 0x00183BD0 File Offset: 0x00181DD0
		public static GameData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new GameData
			{
				OrganisationName = reader.ReadString(),
				Seed = reader.ReadInt32(1),
				Settings = GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.GameSettingsFishNet.Serializing.Generateds(reader),
				DataType = reader.ReadString(),
				DataVersion = reader.ReadInt32(1),
				GameVersion = reader.ReadString()
			};
		}

		// Token: 0x06005BAC RID: 23468 RVA: 0x00183C7C File Offset: 0x00181E7C
		public static GameSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new GameSettings
			{
				ConsoleEnabled = reader.ReadBoolean()
			};
		}

		// Token: 0x06005BAD RID: 23469 RVA: 0x00183CC4 File Offset: 0x00181EC4
		public static DeliveryInstance Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new DeliveryInstance
			{
				DeliveryID = reader.ReadString(),
				StoreName = reader.ReadString(),
				DestinationCode = reader.ReadString(),
				LoadingDockIndex = reader.ReadInt32(1),
				Items = GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generateds(reader),
				Status = GeneratedReaders___Internal.Read___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generateds(reader),
				TimeUntilArrival = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005BAE RID: 23470 RVA: 0x00183D80 File Offset: 0x00181F80
		public static EDeliveryStatus Generateds(Reader reader)
		{
			return (EDeliveryStatus)reader.ReadInt32(1);
		}

		// Token: 0x06005BAF RID: 23471 RVA: 0x00183D9C File Offset: 0x00181F9C
		public static ExplosionData Generateds(Reader reader)
		{
			return new ExplosionData
			{
				DamageRadius = reader.ReadSingle(0),
				MaxDamage = reader.ReadSingle(0),
				PushForceRadius = reader.ReadSingle(0),
				MaxPushForce = reader.ReadSingle(0)
			};
		}

		// Token: 0x06005BB0 RID: 23472 RVA: 0x00183E14 File Offset: 0x00182014
		public static PlayingCard.ECardSuit Generateds(Reader reader)
		{
			return (PlayingCard.ECardSuit)reader.ReadInt32(1);
		}

		// Token: 0x06005BB1 RID: 23473 RVA: 0x00183E30 File Offset: 0x00182030
		public static PlayingCard.ECardValue Generateds(Reader reader)
		{
			return (PlayingCard.ECardValue)reader.ReadInt32(1);
		}

		// Token: 0x06005BB2 RID: 23474 RVA: 0x00183E4C File Offset: 0x0018204C
		public static NetworkObject[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<NetworkObject>();
		}

		// Token: 0x06005BB3 RID: 23475 RVA: 0x00183E64 File Offset: 0x00182064
		public static RTBGameController.EStage Generateds(Reader reader)
		{
			return (RTBGameController.EStage)reader.ReadInt32(1);
		}

		// Token: 0x06005BB4 RID: 23476 RVA: 0x00183E80 File Offset: 0x00182080
		public static SlotMachine.ESymbol Generateds(Reader reader)
		{
			return (SlotMachine.ESymbol)reader.ReadInt32(1);
		}

		// Token: 0x06005BB5 RID: 23477 RVA: 0x00183E9C File Offset: 0x0018209C
		public static SlotMachine.ESymbol[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<SlotMachine.ESymbol>();
		}

		// Token: 0x06005BB6 RID: 23478 RVA: 0x00183EB4 File Offset: 0x001820B4
		public static EDoorSide Generateds(Reader reader)
		{
			return (EDoorSide)reader.ReadInt32(1);
		}

		// Token: 0x06005BB7 RID: 23479 RVA: 0x00183ED0 File Offset: 0x001820D0
		public static EVehicleColor Generateds(Reader reader)
		{
			return (EVehicleColor)reader.ReadInt32(1);
		}

		// Token: 0x06005BB8 RID: 23480 RVA: 0x00183EEC File Offset: 0x001820EC
		public static ParkData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ParkData
			{
				lotGUID = reader.ReadGuid(),
				spotIndex = reader.ReadInt32(1),
				alignment = GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.EParkingAlignmentFishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x06005BB9 RID: 23481 RVA: 0x00183F5C File Offset: 0x0018215C
		public static EParkingAlignment Generateds(Reader reader)
		{
			return (EParkingAlignment)reader.ReadInt32(1);
		}

		// Token: 0x06005BBA RID: 23482 RVA: 0x00183F78 File Offset: 0x00182178
		public static TrashContentData Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new TrashContentData
			{
				TrashIDs = GeneratedReaders___Internal.Read___System.String[]FishNet.Serializing.Generateds(reader),
				TrashQuantities = GeneratedReaders___Internal.Read___System.Int32[]FishNet.Serializing.Generateds(reader)
			};
		}

		// Token: 0x06005BBB RID: 23483 RVA: 0x00183FD0 File Offset: 0x001821D0
		public static int[] Generateds(Reader reader)
		{
			return reader.ReadArrayAllocated<int>();
		}

		// Token: 0x06005BBC RID: 23484 RVA: 0x00183FE8 File Offset: 0x001821E8
		public static Coordinate Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new Coordinate
			{
				x = reader.ReadInt32(1),
				y = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005BBD RID: 23485 RVA: 0x0018404C File Offset: 0x0018224C
		public static WeedAppearanceSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new WeedAppearanceSettings
			{
				MainColor = reader.ReadColor32(),
				SecondaryColor = reader.ReadColor32(),
				LeafColor = reader.ReadColor32(),
				StemColor = reader.ReadColor32()
			};
		}

		// Token: 0x06005BBE RID: 23486 RVA: 0x001840C8 File Offset: 0x001822C8
		public static CocaineAppearanceSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new CocaineAppearanceSettings
			{
				MainColor = reader.ReadColor32(),
				SecondaryColor = reader.ReadColor32()
			};
		}

		// Token: 0x06005BBF RID: 23487 RVA: 0x00184120 File Offset: 0x00182320
		public static MethAppearanceSettings Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new MethAppearanceSettings
			{
				MainColor = reader.ReadColor32(),
				SecondaryColor = reader.ReadColor32()
			};
		}

		// Token: 0x06005BC0 RID: 23488 RVA: 0x00184178 File Offset: 0x00182378
		public static NewMixOperation Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new NewMixOperation
			{
				ProductID = reader.ReadString(),
				IngredientID = reader.ReadString()
			};
		}

		// Token: 0x06005BC1 RID: 23489 RVA: 0x001841D0 File Offset: 0x001823D0
		public static Recycler.EState Generateds(Reader reader)
		{
			return (Recycler.EState)reader.ReadInt32(1);
		}

		// Token: 0x06005BC2 RID: 23490 RVA: 0x001841EC File Offset: 0x001823EC
		public static Jukebox.JukeboxState Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new Jukebox.JukeboxState
			{
				CurrentVolume = reader.ReadInt32(1),
				IsPlaying = reader.ReadBoolean(),
				CurrentTrackTime = reader.ReadSingle(0),
				TrackOrder = GeneratedReaders___Internal.Read___System.Int32[]FishNet.Serializing.Generateds(reader),
				CurrentTrackOrderIndex = reader.ReadInt32(1),
				Shuffle = reader.ReadBoolean(),
				RepeatMode = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Jukebox/ERepeatModeFishNet.Serializing.Generateds(reader),
				Sync = reader.ReadBoolean()
			};
		}

		// Token: 0x06005BC3 RID: 23491 RVA: 0x001842C0 File Offset: 0x001824C0
		public static Jukebox.ERepeatMode Generateds(Reader reader)
		{
			return (Jukebox.ERepeatMode)reader.ReadInt32(1);
		}

		// Token: 0x06005BC4 RID: 23492 RVA: 0x001842DC File Offset: 0x001824DC
		public static CoordinateProceduralTilePair Generateds(Reader reader)
		{
			return new CoordinateProceduralTilePair
			{
				coord = GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generateds(reader),
				tileParent = reader.ReadNetworkObject(),
				tileIndex = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005BC5 RID: 23493 RVA: 0x00184334 File Offset: 0x00182534
		public static List<CoordinateProceduralTilePair> List(Reader reader)
		{
			return reader.ReadListAllocated<CoordinateProceduralTilePair>();
		}

		// Token: 0x06005BC6 RID: 23494 RVA: 0x0018434C File Offset: 0x0018254C
		public static ChemistryCookOperation Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new ChemistryCookOperation
			{
				RecipeID = reader.ReadString(),
				ProductQuality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(reader),
				StartLiquidColor = reader.ReadColor(1),
				LiquidLevel = reader.ReadSingle(0),
				CurrentTime = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005BC7 RID: 23495 RVA: 0x001843E8 File Offset: 0x001825E8
		public static DryingOperation Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new DryingOperation
			{
				ItemID = reader.ReadString(),
				Quantity = reader.ReadInt32(1),
				StartQuality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(reader),
				Time = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005BC8 RID: 23496 RVA: 0x00184470 File Offset: 0x00182670
		public static OvenCookOperation Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new OvenCookOperation
			{
				IngredientID = reader.ReadString(),
				IngredientQuality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(reader),
				IngredientQuantity = reader.ReadInt32(1),
				ProductID = reader.ReadString(),
				CookProgress = reader.ReadInt32(1)
			};
		}

		// Token: 0x06005BC9 RID: 23497 RVA: 0x00184508 File Offset: 0x00182708
		public static MixOperation Generateds(Reader reader)
		{
			bool flag = reader.ReadBoolean();
			if (flag)
			{
				return null;
			}
			return new MixOperation
			{
				ProductID = reader.ReadString(),
				ProductQuality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(reader),
				IngredientID = reader.ReadString(),
				Quantity = reader.ReadInt32(1)
			};
		}
	}
}
