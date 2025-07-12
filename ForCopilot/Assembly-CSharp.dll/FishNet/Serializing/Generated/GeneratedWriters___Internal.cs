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
	// Token: 0x02000CBC RID: 3260
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedWriters___Internal
	{
		// Token: 0x06005B14 RID: 23316 RVA: 0x0017FCBC File Offset: 0x0017DEBC
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeOnce()
		{
			GenericWriter<ItemInstance>.Write = new Action<Writer, ItemInstance>(ItemSerializers.WriteItemInstance);
			GenericWriter<StorableItemInstance>.Write = new Action<Writer, StorableItemInstance>(ItemSerializers.WriteStorableItemInstance);
			GenericWriter<CashInstance>.Write = new Action<Writer, CashInstance>(ItemSerializers.WriteCashInstance);
			GenericWriter<QualityItemInstance>.Write = new Action<Writer, QualityItemInstance>(ItemSerializers.WriteQualityItemInstance);
			GenericWriter<ClothingInstance>.Write = new Action<Writer, ClothingInstance>(ItemSerializers.WriteClothingInstance);
			GenericWriter<ProductItemInstance>.Write = new Action<Writer, ProductItemInstance>(ItemSerializers.WriteProductItemInstance);
			GenericWriter<WeedInstance>.Write = new Action<Writer, WeedInstance>(ItemSerializers.WriteWeedInstance);
			GenericWriter<MethInstance>.Write = new Action<Writer, MethInstance>(ItemSerializers.WriteMethInstance);
			GenericWriter<CocaineInstance>.Write = new Action<Writer, CocaineInstance>(ItemSerializers.WriteCocaineInstance);
			GenericWriter<IntegerItemInstance>.Write = new Action<Writer, IntegerItemInstance>(ItemSerializers.WriteIntegerItemInstance);
			GenericWriter<WateringCanInstance>.Write = new Action<Writer, WateringCanInstance>(ItemSerializers.WriteWateringCanInstance);
			GenericWriter<TrashGrabberInstance>.Write = new Action<Writer, TrashGrabberInstance>(ItemSerializers.WriteTrashGrabberInstance);
			GenericWriter<VisionEventReceipt>.Write = new Action<Writer, VisionEventReceipt>(GeneratedWriters___Internal.Write___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generated);
			GenericWriter<PlayerVisualState.EVisualState>.Write = new Action<Writer, PlayerVisualState.EVisualState>(GeneratedWriters___Internal.Write___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generated);
			GenericWriter<VisionCone.EEventLevel>.Write = new Action<Writer, VisionCone.EEventLevel>(GeneratedWriters___Internal.Write___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generated);
			GenericWriter<ContractInfo>.Write = new Action<Writer, ContractInfo>(GeneratedWriters___Internal.Write___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generated);
			GenericWriter<ProductList>.Write = new Action<Writer, ProductList>(GeneratedWriters___Internal.Write___ScheduleOne.Product.ProductListFishNet.Serializing.Generated);
			GenericWriter<ProductList.Entry>.Write = new Action<Writer, ProductList.Entry>(GeneratedWriters___Internal.Write___ScheduleOne.Product.ProductList/EntryFishNet.Serializing.Generated);
			GenericWriter<EQuality>.Write = new Action<Writer, EQuality>(GeneratedWriters___Internal.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated);
			GenericWriter<List<ProductList.Entry>>.Write = new Action<Writer, List<ProductList.Entry>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.Product.ProductList/Entry>FishNet.Serializing.Generated);
			GenericWriter<QuestWindowConfig>.Write = new Action<Writer, QuestWindowConfig>(GeneratedWriters___Internal.Write___ScheduleOne.Quests.QuestWindowConfigFishNet.Serializing.Generated);
			GenericWriter<GameDateTime>.Write = new Action<Writer, GameDateTime>(GeneratedWriters___Internal.Write___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generated);
			GenericWriter<QuestManager.EQuestAction>.Write = new Action<Writer, QuestManager.EQuestAction>(GeneratedWriters___Internal.Write___ScheduleOne.Quests.QuestManager/EQuestActionFishNet.Serializing.Generated);
			GenericWriter<EQuestState>.Write = new Action<Writer, EQuestState>(GeneratedWriters___Internal.Write___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generated);
			GenericWriter<Impact>.Write = new Action<Writer, Impact>(GeneratedWriters___Internal.Write___ScheduleOne.Combat.ImpactFishNet.Serializing.Generated);
			GenericWriter<EImpactType>.Write = new Action<Writer, EImpactType>(GeneratedWriters___Internal.Write___ScheduleOne.Combat.EImpactTypeFishNet.Serializing.Generated);
			GenericWriter<LandVehicle>.Write = new Action<Writer, LandVehicle>(GeneratedWriters___Internal.Write___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generated);
			GenericWriter<CheckpointManager.ECheckpointLocation>.Write = new Action<Writer, CheckpointManager.ECheckpointLocation>(GeneratedWriters___Internal.Write___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generated);
			GenericWriter<SlotFilter>.Write = new Action<Writer, SlotFilter>(GeneratedWriters___Internal.Write___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generated);
			GenericWriter<SlotFilter.EType>.Write = new Action<Writer, SlotFilter.EType>(GeneratedWriters___Internal.Write___ScheduleOne.ItemFramework.SlotFilter/ETypeFishNet.Serializing.Generated);
			GenericWriter<List<string>>.Write = new Action<Writer, List<string>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated);
			GenericWriter<List<EQuality>>.Write = new Action<Writer, List<EQuality>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.EQuality>FishNet.Serializing.Generated);
			GenericWriter<Player>.Write = new Action<Writer, Player>(GeneratedWriters___Internal.Write___ScheduleOne.PlayerScripts.PlayerFishNet.Serializing.Generated);
			GenericWriter<StringIntPair>.Write = new Action<Writer, StringIntPair>(GeneratedWriters___Internal.Write___ScheduleOne.DevUtilities.StringIntPairFishNet.Serializing.Generated);
			GenericWriter<StringIntPair[]>.Write = new Action<Writer, StringIntPair[]>(GeneratedWriters___Internal.Write___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generated);
			GenericWriter<Message>.Write = new Action<Writer, Message>(GeneratedWriters___Internal.Write___ScheduleOne.Messaging.MessageFishNet.Serializing.Generated);
			GenericWriter<Message.ESenderType>.Write = new Action<Writer, Message.ESenderType>(GeneratedWriters___Internal.Write___ScheduleOne.Messaging.Message/ESenderTypeFishNet.Serializing.Generated);
			GenericWriter<MessageChain>.Write = new Action<Writer, MessageChain>(GeneratedWriters___Internal.Write___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generated);
			GenericWriter<MSGConversationData>.Write = new Action<Writer, MSGConversationData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.MSGConversationDataFishNet.Serializing.Generated);
			GenericWriter<TextMessageData>.Write = new Action<Writer, TextMessageData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.TextMessageDataFishNet.Serializing.Generated);
			GenericWriter<TextMessageData[]>.Write = new Action<Writer, TextMessageData[]>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.TextMessageData[]FishNet.Serializing.Generated);
			GenericWriter<TextResponseData>.Write = new Action<Writer, TextResponseData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.TextResponseDataFishNet.Serializing.Generated);
			GenericWriter<TextResponseData[]>.Write = new Action<Writer, TextResponseData[]>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.TextResponseData[]FishNet.Serializing.Generated);
			GenericWriter<Response>.Write = new Action<Writer, Response>(GeneratedWriters___Internal.Write___ScheduleOne.Messaging.ResponseFishNet.Serializing.Generated);
			GenericWriter<List<Response>>.Write = new Action<Writer, List<Response>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generated);
			GenericWriter<List<NetworkObject>>.Write = new Action<Writer, List<NetworkObject>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generated);
			GenericWriter<AdvancedTransitRouteData>.Write = new Action<Writer, AdvancedTransitRouteData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.AdvancedTransitRouteDataFishNet.Serializing.Generated);
			GenericWriter<ManagementItemFilter.EMode>.Write = new Action<Writer, ManagementItemFilter.EMode>(GeneratedWriters___Internal.Write___ScheduleOne.Management.ManagementItemFilter/EModeFishNet.Serializing.Generated);
			GenericWriter<AdvancedTransitRouteData[]>.Write = new Action<Writer, AdvancedTransitRouteData[]>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generated);
			GenericWriter<ERank>.Write = new Action<Writer, ERank>(GeneratedWriters___Internal.Write___ScheduleOne.Levelling.ERankFishNet.Serializing.Generated);
			GenericWriter<FullRank>.Write = new Action<Writer, FullRank>(GeneratedWriters___Internal.Write___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generated);
			GenericWriter<PlayerData>.Write = new Action<Writer, PlayerData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generated);
			GenericWriter<VariableData>.Write = new Action<Writer, VariableData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.VariableDataFishNet.Serializing.Generated);
			GenericWriter<VariableData[]>.Write = new Action<Writer, VariableData[]>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generated);
			GenericWriter<AvatarSettings>.Write = new Action<Writer, AvatarSettings>(GeneratedWriters___Internal.Write___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generated);
			GenericWriter<Eye.EyeLidConfiguration>.Write = new Action<Writer, Eye.EyeLidConfiguration>(GeneratedWriters___Internal.Write___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generated);
			GenericWriter<AvatarSettings.LayerSetting>.Write = new Action<Writer, AvatarSettings.LayerSetting>(GeneratedWriters___Internal.Write___ScheduleOne.AvatarFramework.AvatarSettings/LayerSettingFishNet.Serializing.Generated);
			GenericWriter<List<AvatarSettings.LayerSetting>>.Write = new Action<Writer, List<AvatarSettings.LayerSetting>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generated);
			GenericWriter<AvatarSettings.AccessorySetting>.Write = new Action<Writer, AvatarSettings.AccessorySetting>(GeneratedWriters___Internal.Write___ScheduleOne.AvatarFramework.AvatarSettings/AccessorySettingFishNet.Serializing.Generated);
			GenericWriter<List<AvatarSettings.AccessorySetting>>.Write = new Action<Writer, List<AvatarSettings.AccessorySetting>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/AccessorySetting>FishNet.Serializing.Generated);
			GenericWriter<BasicAvatarSettings>.Write = new Action<Writer, BasicAvatarSettings>(GeneratedWriters___Internal.Write___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generated);
			GenericWriter<PlayerCrimeData.EPursuitLevel>.Write = new Action<Writer, PlayerCrimeData.EPursuitLevel>(GeneratedWriters___Internal.Write___ScheduleOne.PlayerScripts.PlayerCrimeData/EPursuitLevelFishNet.Serializing.Generated);
			GenericWriter<Property>.Write = new Action<Writer, Property>(GeneratedWriters___Internal.Write___ScheduleOne.Property.PropertyFishNet.Serializing.Generated);
			GenericWriter<EEmployeeType>.Write = new Action<Writer, EEmployeeType>(GeneratedWriters___Internal.Write___ScheduleOne.Employees.EEmployeeTypeFishNet.Serializing.Generated);
			GenericWriter<EDealWindow>.Write = new Action<Writer, EDealWindow>(GeneratedWriters___Internal.Write___ScheduleOne.Economy.EDealWindowFishNet.Serializing.Generated);
			GenericWriter<HandoverScreen.EHandoverOutcome>.Write = new Action<Writer, HandoverScreen.EHandoverOutcome>(GeneratedWriters___Internal.Write___ScheduleOne.UI.Handover.HandoverScreen/EHandoverOutcomeFishNet.Serializing.Generated);
			GenericWriter<List<ItemInstance>>.Write = new Action<Writer, List<ItemInstance>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.ItemInstance>FishNet.Serializing.Generated);
			GenericWriter<ScheduleOne.Persistence.Datas.CustomerData>.Write = new Action<Writer, ScheduleOne.Persistence.Datas.CustomerData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.CustomerDataFishNet.Serializing.Generated);
			GenericWriter<string[]>.Write = new Action<Writer, string[]>(GeneratedWriters___Internal.Write___System.String[]FishNet.Serializing.Generated);
			GenericWriter<float[]>.Write = new Action<Writer, float[]>(GeneratedWriters___Internal.Write___System.Single[]FishNet.Serializing.Generated);
			GenericWriter<EDrugType>.Write = new Action<Writer, EDrugType>(GeneratedWriters___Internal.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated);
			GenericWriter<GameData>.Write = new Action<Writer, GameData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.Datas.GameDataFishNet.Serializing.Generated);
			GenericWriter<GameSettings>.Write = new Action<Writer, GameSettings>(GeneratedWriters___Internal.Write___ScheduleOne.DevUtilities.GameSettingsFishNet.Serializing.Generated);
			GenericWriter<DeliveryInstance>.Write = new Action<Writer, DeliveryInstance>(GeneratedWriters___Internal.Write___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generated);
			GenericWriter<EDeliveryStatus>.Write = new Action<Writer, EDeliveryStatus>(GeneratedWriters___Internal.Write___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generated);
			GenericWriter<ExplosionData>.Write = new Action<Writer, ExplosionData>(GeneratedWriters___Internal.Write___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generated);
			GenericWriter<PlayingCard.ECardSuit>.Write = new Action<Writer, PlayingCard.ECardSuit>(GeneratedWriters___Internal.Write___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generated);
			GenericWriter<PlayingCard.ECardValue>.Write = new Action<Writer, PlayingCard.ECardValue>(GeneratedWriters___Internal.Write___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generated);
			GenericWriter<NetworkObject[]>.Write = new Action<Writer, NetworkObject[]>(GeneratedWriters___Internal.Write___FishNet.Object.NetworkObject[]FishNet.Serializing.Generated);
			GenericWriter<RTBGameController.EStage>.Write = new Action<Writer, RTBGameController.EStage>(GeneratedWriters___Internal.Write___ScheduleOne.Casino.RTBGameController/EStageFishNet.Serializing.Generated);
			GenericWriter<SlotMachine.ESymbol>.Write = new Action<Writer, SlotMachine.ESymbol>(GeneratedWriters___Internal.Write___ScheduleOne.Casino.SlotMachine/ESymbolFishNet.Serializing.Generated);
			GenericWriter<SlotMachine.ESymbol[]>.Write = new Action<Writer, SlotMachine.ESymbol[]>(GeneratedWriters___Internal.Write___ScheduleOne.Casino.SlotMachine/ESymbol[]FishNet.Serializing.Generated);
			GenericWriter<EDoorSide>.Write = new Action<Writer, EDoorSide>(GeneratedWriters___Internal.Write___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generated);
			GenericWriter<EVehicleColor>.Write = new Action<Writer, EVehicleColor>(GeneratedWriters___Internal.Write___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generated);
			GenericWriter<ParkData>.Write = new Action<Writer, ParkData>(GeneratedWriters___Internal.Write___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generated);
			GenericWriter<EParkingAlignment>.Write = new Action<Writer, EParkingAlignment>(GeneratedWriters___Internal.Write___ScheduleOne.Vehicles.EParkingAlignmentFishNet.Serializing.Generated);
			GenericWriter<TrashContentData>.Write = new Action<Writer, TrashContentData>(GeneratedWriters___Internal.Write___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generated);
			GenericWriter<int[]>.Write = new Action<Writer, int[]>(GeneratedWriters___Internal.Write___System.Int32[]FishNet.Serializing.Generated);
			GenericWriter<Coordinate>.Write = new Action<Writer, Coordinate>(GeneratedWriters___Internal.Write___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generated);
			GenericWriter<WeedAppearanceSettings>.Write = new Action<Writer, WeedAppearanceSettings>(GeneratedWriters___Internal.Write___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generated);
			GenericWriter<CocaineAppearanceSettings>.Write = new Action<Writer, CocaineAppearanceSettings>(GeneratedWriters___Internal.Write___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generated);
			GenericWriter<MethAppearanceSettings>.Write = new Action<Writer, MethAppearanceSettings>(GeneratedWriters___Internal.Write___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generated);
			GenericWriter<NewMixOperation>.Write = new Action<Writer, NewMixOperation>(GeneratedWriters___Internal.Write___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generated);
			GenericWriter<Recycler.EState>.Write = new Action<Writer, Recycler.EState>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generated);
			GenericWriter<Jukebox.JukeboxState>.Write = new Action<Writer, Jukebox.JukeboxState>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.Jukebox/JukeboxStateFishNet.Serializing.Generated);
			GenericWriter<Jukebox.ERepeatMode>.Write = new Action<Writer, Jukebox.ERepeatMode>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.Jukebox/ERepeatModeFishNet.Serializing.Generated);
			GenericWriter<CoordinateProceduralTilePair>.Write = new Action<Writer, CoordinateProceduralTilePair>(GeneratedWriters___Internal.Write___ScheduleOne.Tiles.CoordinateProceduralTilePairFishNet.Serializing.Generated);
			GenericWriter<List<CoordinateProceduralTilePair>>.Write = new Action<Writer, List<CoordinateProceduralTilePair>>(GeneratedWriters___Internal.Write___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generated);
			GenericWriter<ChemistryCookOperation>.Write = new Action<Writer, ChemistryCookOperation>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generated);
			GenericWriter<DryingOperation>.Write = new Action<Writer, DryingOperation>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generated);
			GenericWriter<OvenCookOperation>.Write = new Action<Writer, OvenCookOperation>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generated);
			GenericWriter<MixOperation>.Write = new Action<Writer, MixOperation>(GeneratedWriters___Internal.Write___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generated);
		}

		// Token: 0x06005B15 RID: 23317 RVA: 0x00180390 File Offset: 0x0017E590
		public static void Generated(this Writer writer, VisionEventReceipt value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteNetworkObject(value.TargetPlayer);
			writer.Write___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generated(value.State);
		}

		// Token: 0x06005B16 RID: 23318 RVA: 0x001803E8 File Offset: 0x0017E5E8
		public static void Generated(this Writer writer, PlayerVisualState.EVisualState value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B17 RID: 23319 RVA: 0x00180408 File Offset: 0x0017E608
		public static void Generated(this Writer writer, VisionCone.EEventLevel value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B18 RID: 23320 RVA: 0x00180428 File Offset: 0x0017E628
		public static void Generated(this Writer writer, ContractInfo value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteSingle(value.Payment, 0);
			writer.Write___ScheduleOne.Product.ProductListFishNet.Serializing.Generated(value.Products);
			writer.WriteString(value.DeliveryLocationGUID);
			writer.Write___ScheduleOne.Quests.QuestWindowConfigFishNet.Serializing.Generated(value.DeliveryWindow);
			writer.WriteBoolean(value.Expires);
			writer.WriteInt32(value.ExpiresAfter, 1);
			writer.WriteInt32(value.PickupScheduleIndex, 1);
			writer.WriteBoolean(value.IsCounterOffer);
		}

		// Token: 0x06005B19 RID: 23321 RVA: 0x001804FC File Offset: 0x0017E6FC
		public static void Generated(this Writer writer, ProductList value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Product.ProductList/Entry>FishNet.Serializing.Generated(value.entries);
		}

		// Token: 0x06005B1A RID: 23322 RVA: 0x00180544 File Offset: 0x0017E744
		public static void Generated(this Writer writer, ProductList.Entry value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.ProductID);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value.Quality);
			writer.WriteInt32(value.Quantity, 1);
		}

		// Token: 0x06005B1B RID: 23323 RVA: 0x001805B4 File Offset: 0x0017E7B4
		public static void Generated(this Writer writer, EQuality value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B1C RID: 23324 RVA: 0x001805D4 File Offset: 0x0017E7D4
		public static void List(this Writer writer, List<ProductList.Entry> value)
		{
			writer.WriteList<ProductList.Entry>(value);
		}

		// Token: 0x06005B1D RID: 23325 RVA: 0x001805F0 File Offset: 0x0017E7F0
		public static void Generated(this Writer writer, QuestWindowConfig value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteBoolean(value.IsEnabled);
			writer.WriteInt32(value.WindowStartTime, 1);
			writer.WriteInt32(value.WindowEndTime, 1);
		}

		// Token: 0x06005B1E RID: 23326 RVA: 0x00180664 File Offset: 0x0017E864
		public static void Generated(this Writer writer, GameDateTime value)
		{
			writer.WriteInt32(value.elapsedDays, 1);
			writer.WriteInt32(value.time, 1);
		}

		// Token: 0x06005B1F RID: 23327 RVA: 0x001806A0 File Offset: 0x0017E8A0
		public static void Generated(this Writer writer, QuestManager.EQuestAction value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B20 RID: 23328 RVA: 0x001806C0 File Offset: 0x0017E8C0
		public static void Generated(this Writer writer, EQuestState value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B21 RID: 23329 RVA: 0x001806E0 File Offset: 0x0017E8E0
		public static void Generated(this Writer writer, Impact value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteVector3(value.HitPoint);
			writer.WriteVector3(value.ImpactForceDirection);
			writer.WriteSingle(value.ImpactForce, 0);
			writer.WriteSingle(value.ImpactDamage, 0);
			writer.Write___ScheduleOne.Combat.EImpactTypeFishNet.Serializing.Generated(value.ImpactType);
			writer.WriteNetworkObject(value.ImpactSource);
			writer.WriteInt32(value.ImpactID, 1);
		}

		// Token: 0x06005B22 RID: 23330 RVA: 0x001807A4 File Offset: 0x0017E9A4
		public static void Generated(this Writer writer, EImpactType value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B23 RID: 23331 RVA: 0x001807C4 File Offset: 0x0017E9C4
		public static void Generated(this Writer writer, LandVehicle value)
		{
			writer.WriteNetworkBehaviour(value);
		}

		// Token: 0x06005B24 RID: 23332 RVA: 0x001807E0 File Offset: 0x0017E9E0
		public static void Generated(this Writer writer, CheckpointManager.ECheckpointLocation value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B25 RID: 23333 RVA: 0x00180800 File Offset: 0x0017EA00
		public static void Generated(this Writer writer, SlotFilter value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.Write___ScheduleOne.ItemFramework.SlotFilter/ETypeFishNet.Serializing.Generated(value.Type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(value.ItemIDs);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.EQuality>FishNet.Serializing.Generated(value.AllowedQualities);
		}

		// Token: 0x06005B26 RID: 23334 RVA: 0x0018086C File Offset: 0x0017EA6C
		public static void Generated(this Writer writer, SlotFilter.EType value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B27 RID: 23335 RVA: 0x0018088C File Offset: 0x0017EA8C
		public static void List(this Writer writer, List<string> value)
		{
			writer.WriteList<string>(value);
		}

		// Token: 0x06005B28 RID: 23336 RVA: 0x001808A8 File Offset: 0x0017EAA8
		public static void List(this Writer writer, List<EQuality> value)
		{
			writer.WriteList<EQuality>(value);
		}

		// Token: 0x06005B29 RID: 23337 RVA: 0x001808C4 File Offset: 0x0017EAC4
		public static void Generated(this Writer writer, Player value)
		{
			writer.WriteNetworkBehaviour(value);
		}

		// Token: 0x06005B2A RID: 23338 RVA: 0x001808E0 File Offset: 0x0017EAE0
		public static void Generated(this Writer writer, StringIntPair value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.String);
			writer.WriteInt32(value.Int, 1);
		}

		// Token: 0x06005B2B RID: 23339 RVA: 0x00180940 File Offset: 0x0017EB40
		public static void Generated(this Writer writer, StringIntPair[] value)
		{
			writer.WriteArray<StringIntPair>(value);
		}

		// Token: 0x06005B2C RID: 23340 RVA: 0x0018095C File Offset: 0x0017EB5C
		public static void Generated(this Writer writer, Message value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteInt32(value.messageId, 1);
			writer.WriteString(value.text);
			writer.Write___ScheduleOne.Messaging.Message/ESenderTypeFishNet.Serializing.Generated(value.sender);
			writer.WriteBoolean(value.endOfGroup);
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x001809E0 File Offset: 0x0017EBE0
		public static void Generated(this Writer writer, Message.ESenderType value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B2E RID: 23342 RVA: 0x00180A00 File Offset: 0x0017EC00
		public static void Generated(this Writer writer, MessageChain value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(value.Messages);
			writer.WriteInt32(value.id, 1);
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x00180A60 File Offset: 0x0017EC60
		public static void Generated(this Writer writer, MSGConversationData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteInt32(value.ConversationIndex, 1);
			writer.WriteBoolean(value.Read);
			writer.Write___ScheduleOne.Persistence.Datas.TextMessageData[]FishNet.Serializing.Generated(value.MessageHistory);
			writer.Write___ScheduleOne.Persistence.Datas.TextResponseData[]FishNet.Serializing.Generated(value.ActiveResponses);
			writer.WriteBoolean(value.IsHidden);
			writer.WriteString(value.DataType);
			writer.WriteInt32(value.DataVersion, 1);
			writer.WriteString(value.GameVersion);
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x00180B30 File Offset: 0x0017ED30
		public static void Generated(this Writer writer, TextMessageData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteInt32(value.Sender, 1);
			writer.WriteInt32(value.MessageID, 1);
			writer.WriteString(value.Text);
			writer.WriteBoolean(value.EndOfChain);
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x00180BB8 File Offset: 0x0017EDB8
		public static void Generated(this Writer writer, TextMessageData[] value)
		{
			writer.WriteArray<TextMessageData>(value);
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x00180BD4 File Offset: 0x0017EDD4
		public static void Generated(this Writer writer, TextResponseData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.Text);
			writer.WriteString(value.Label);
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x00180C2C File Offset: 0x0017EE2C
		public static void Generated(this Writer writer, TextResponseData[] value)
		{
			writer.WriteArray<TextResponseData>(value);
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x00180C48 File Offset: 0x0017EE48
		public static void Generated(this Writer writer, Response value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.text);
			writer.WriteString(value.label);
			writer.WriteBoolean(value.disableDefaultResponseBehaviour);
		}

		// Token: 0x06005B35 RID: 23349 RVA: 0x00180CB4 File Offset: 0x0017EEB4
		public static void List(this Writer writer, List<Response> value)
		{
			writer.WriteList<Response>(value);
		}

		// Token: 0x06005B36 RID: 23350 RVA: 0x00180CD0 File Offset: 0x0017EED0
		public static void List(this Writer writer, List<NetworkObject> value)
		{
			writer.WriteList<NetworkObject>(value);
		}

		// Token: 0x06005B37 RID: 23351 RVA: 0x00180CEC File Offset: 0x0017EEEC
		public static void Generated(this Writer writer, AdvancedTransitRouteData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.SourceGUID);
			writer.WriteString(value.DestinationGUID);
			writer.Write___ScheduleOne.Management.ManagementItemFilter/EModeFishNet.Serializing.Generated(value.FilterMode);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(value.FilterItemIDs);
		}

		// Token: 0x06005B38 RID: 23352 RVA: 0x00180D68 File Offset: 0x0017EF68
		public static void Generated(this Writer writer, ManagementItemFilter.EMode value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B39 RID: 23353 RVA: 0x00180D88 File Offset: 0x0017EF88
		public static void Generated(this Writer writer, AdvancedTransitRouteData[] value)
		{
			writer.WriteArray<AdvancedTransitRouteData>(value);
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x00180DA4 File Offset: 0x0017EFA4
		public static void Generated(this Writer writer, ERank value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B3B RID: 23355 RVA: 0x00180DC4 File Offset: 0x0017EFC4
		public static void Generated(this Writer writer, FullRank value)
		{
			writer.Write___ScheduleOne.Levelling.ERankFishNet.Serializing.Generated(value.Rank);
			writer.WriteInt32(value.Tier, 1);
		}

		// Token: 0x06005B3C RID: 23356 RVA: 0x00180DFC File Offset: 0x0017EFFC
		public static void Generated(this Writer writer, PlayerData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.PlayerCode);
			writer.WriteVector3(value.Position);
			writer.WriteSingle(value.Rotation, 0);
			writer.WriteBoolean(value.IntroCompleted);
			writer.WriteString(value.DataType);
			writer.WriteInt32(value.DataVersion, 1);
			writer.WriteString(value.GameVersion);
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x00180EB8 File Offset: 0x0017F0B8
		public static void Generated(this Writer writer, VariableData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.Name);
			writer.WriteString(value.Value);
			writer.WriteString(value.DataType);
			writer.WriteInt32(value.DataVersion, 1);
			writer.WriteString(value.GameVersion);
		}

		// Token: 0x06005B3E RID: 23358 RVA: 0x00180F4C File Offset: 0x0017F14C
		public static void Generated(this Writer writer, VariableData[] value)
		{
			writer.WriteArray<VariableData>(value);
		}

		// Token: 0x06005B3F RID: 23359 RVA: 0x00180F68 File Offset: 0x0017F168
		public static void Generated(this Writer writer, AvatarSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteColor(value.SkinColor, 1);
			writer.WriteSingle(value.Height, 0);
			writer.WriteSingle(value.Gender, 0);
			writer.WriteSingle(value.Weight, 0);
			writer.WriteString(value.HairPath);
			writer.WriteColor(value.HairColor, 1);
			writer.WriteSingle(value.EyebrowScale, 0);
			writer.WriteSingle(value.EyebrowThickness, 0);
			writer.WriteSingle(value.EyebrowRestingHeight, 0);
			writer.WriteSingle(value.EyebrowRestingAngle, 0);
			writer.WriteColor(value.LeftEyeLidColor, 1);
			writer.WriteColor(value.RightEyeLidColor, 1);
			writer.Write___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generated(value.LeftEyeRestingState);
			writer.Write___ScheduleOne.AvatarFramework.Eye/EyeLidConfigurationFishNet.Serializing.Generated(value.RightEyeRestingState);
			writer.WriteString(value.EyeballMaterialIdentifier);
			writer.WriteColor(value.EyeBallTint, 1);
			writer.WriteSingle(value.PupilDilation, 0);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generated(value.FaceLayerSettings);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/LayerSetting>FishNet.Serializing.Generated(value.BodyLayerSettings);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.AvatarFramework.AvatarSettings/AccessorySetting>FishNet.Serializing.Generated(value.AccessorySettings);
			writer.WriteBoolean(value.UseCombinedLayer);
			writer.WriteString(value.CombinedLayerPath);
		}

		// Token: 0x06005B40 RID: 23360 RVA: 0x0018116C File Offset: 0x0017F36C
		public static void Generated(this Writer writer, Eye.EyeLidConfiguration value)
		{
			writer.WriteSingle(value.topLidOpen, 0);
			writer.WriteSingle(value.bottomLidOpen, 0);
		}

		// Token: 0x06005B41 RID: 23361 RVA: 0x001811A8 File Offset: 0x0017F3A8
		public static void Generated(this Writer writer, AvatarSettings.LayerSetting value)
		{
			writer.WriteString(value.layerPath);
			writer.WriteColor(value.layerTint, 1);
		}

		// Token: 0x06005B42 RID: 23362 RVA: 0x001811E0 File Offset: 0x0017F3E0
		public static void List(this Writer writer, List<AvatarSettings.LayerSetting> value)
		{
			writer.WriteList<AvatarSettings.LayerSetting>(value);
		}

		// Token: 0x06005B43 RID: 23363 RVA: 0x001811FC File Offset: 0x0017F3FC
		public static void Generated(this Writer writer, AvatarSettings.AccessorySetting value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.path);
			writer.WriteColor(value.color, 1);
		}

		// Token: 0x06005B44 RID: 23364 RVA: 0x0018125C File Offset: 0x0017F45C
		public static void List(this Writer writer, List<AvatarSettings.AccessorySetting> value)
		{
			writer.WriteList<AvatarSettings.AccessorySetting>(value);
		}

		// Token: 0x06005B45 RID: 23365 RVA: 0x00181278 File Offset: 0x0017F478
		public static void Generated(this Writer writer, BasicAvatarSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteInt32(value.Gender, 1);
			writer.WriteSingle(value.Weight, 0);
			writer.WriteColor(value.SkinColor, 1);
			writer.WriteString(value.HairStyle);
			writer.WriteColor(value.HairColor, 1);
			writer.WriteString(value.Mouth);
			writer.WriteString(value.FacialHair);
			writer.WriteString(value.FacialDetails);
			writer.WriteSingle(value.FacialDetailsIntensity, 0);
			writer.WriteColor(value.EyeballColor, 1);
			writer.WriteSingle(value.UpperEyeLidRestingPosition, 0);
			writer.WriteSingle(value.LowerEyeLidRestingPosition, 0);
			writer.WriteSingle(value.PupilDilation, 0);
			writer.WriteSingle(value.EyebrowScale, 0);
			writer.WriteSingle(value.EyebrowThickness, 0);
			writer.WriteSingle(value.EyebrowRestingHeight, 0);
			writer.WriteSingle(value.EyebrowRestingAngle, 0);
			writer.WriteString(value.Top);
			writer.WriteColor(value.TopColor, 1);
			writer.WriteString(value.Bottom);
			writer.WriteColor(value.BottomColor, 1);
			writer.WriteString(value.Shoes);
			writer.WriteColor(value.ShoesColor, 1);
			writer.WriteString(value.Headwear);
			writer.WriteColor(value.HeadwearColor, 1);
			writer.WriteString(value.Eyewear);
			writer.WriteColor(value.EyewearColor, 1);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(value.Tattoos);
		}

		// Token: 0x06005B46 RID: 23366 RVA: 0x00181500 File Offset: 0x0017F700
		public static void Generated(this Writer writer, PlayerCrimeData.EPursuitLevel value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B47 RID: 23367 RVA: 0x00181520 File Offset: 0x0017F720
		public static void Generated(this Writer writer, Property value)
		{
			writer.WriteNetworkBehaviour(value);
		}

		// Token: 0x06005B48 RID: 23368 RVA: 0x0018153C File Offset: 0x0017F73C
		public static void Generated(this Writer writer, EEmployeeType value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B49 RID: 23369 RVA: 0x0018155C File Offset: 0x0017F75C
		public static void Generated(this Writer writer, EDealWindow value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B4A RID: 23370 RVA: 0x0018157C File Offset: 0x0017F77C
		public static void Generated(this Writer writer, HandoverScreen.EHandoverOutcome value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B4B RID: 23371 RVA: 0x0018159C File Offset: 0x0017F79C
		public static void List(this Writer writer, List<ItemInstance> value)
		{
			writer.WriteList<ItemInstance>(value);
		}

		// Token: 0x06005B4C RID: 23372 RVA: 0x001815B8 File Offset: 0x0017F7B8
		public static void Generated(this Writer writer, ScheduleOne.Persistence.Datas.CustomerData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteSingle(value.Dependence, 0);
			writer.Write___System.String[]FishNet.Serializing.Generated(value.PurchaseableProducts);
			writer.Write___System.Single[]FishNet.Serializing.Generated(value.ProductAffinities);
			writer.WriteInt32(value.TimeSinceLastDealCompleted, 1);
			writer.WriteInt32(value.TimeSinceLastDealOffered, 1);
			writer.WriteInt32(value.OfferedDeals, 1);
			writer.WriteInt32(value.CompletedDeals, 1);
			writer.WriteBoolean(value.IsContractOffered);
			writer.Write___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generated(value.OfferedContract);
			writer.Write___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generated(value.OfferedContractTime);
			writer.WriteInt32(value.TimeSincePlayerApproached, 1);
			writer.WriteInt32(value.TimeSinceInstantDealOffered, 1);
			writer.WriteBoolean(value.HasBeenRecommended);
			writer.WriteString(value.DataType);
			writer.WriteInt32(value.DataVersion, 1);
			writer.WriteString(value.GameVersion);
		}

		// Token: 0x06005B4D RID: 23373 RVA: 0x00181734 File Offset: 0x0017F934
		public static void Generated(this Writer writer, string[] value)
		{
			writer.WriteArray<string>(value);
		}

		// Token: 0x06005B4E RID: 23374 RVA: 0x00181750 File Offset: 0x0017F950
		public static void Generated(this Writer writer, float[] value)
		{
			writer.WriteArray<float>(value);
		}

		// Token: 0x06005B4F RID: 23375 RVA: 0x0018176C File Offset: 0x0017F96C
		public static void Generated(this Writer writer, EDrugType value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B50 RID: 23376 RVA: 0x0018178C File Offset: 0x0017F98C
		public static void Generated(this Writer writer, GameData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.OrganisationName);
			writer.WriteInt32(value.Seed, 1);
			writer.Write___ScheduleOne.DevUtilities.GameSettingsFishNet.Serializing.Generated(value.Settings);
			writer.WriteString(value.DataType);
			writer.WriteInt32(value.DataVersion, 1);
			writer.WriteString(value.GameVersion);
		}

		// Token: 0x06005B51 RID: 23377 RVA: 0x00181838 File Offset: 0x0017FA38
		public static void Generated(this Writer writer, GameSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteBoolean(value.ConsoleEnabled);
		}

		// Token: 0x06005B52 RID: 23378 RVA: 0x00181880 File Offset: 0x0017FA80
		public static void Generated(this Writer writer, DeliveryInstance value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.DeliveryID);
			writer.WriteString(value.StoreName);
			writer.WriteString(value.DestinationCode);
			writer.WriteInt32(value.LoadingDockIndex, 1);
			writer.Write___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generated(value.Items);
			writer.Write___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generated(value.Status);
			writer.WriteInt32(value.TimeUntilArrival, 1);
		}

		// Token: 0x06005B53 RID: 23379 RVA: 0x0018193C File Offset: 0x0017FB3C
		public static void Generated(this Writer writer, EDeliveryStatus value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B54 RID: 23380 RVA: 0x0018195C File Offset: 0x0017FB5C
		public static void Generated(this Writer writer, ExplosionData value)
		{
			writer.WriteSingle(value.DamageRadius, 0);
			writer.WriteSingle(value.MaxDamage, 0);
			writer.WriteSingle(value.PushForceRadius, 0);
			writer.WriteSingle(value.MaxPushForce, 0);
		}

		// Token: 0x06005B55 RID: 23381 RVA: 0x001819C8 File Offset: 0x0017FBC8
		public static void Generated(this Writer writer, PlayingCard.ECardSuit value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B56 RID: 23382 RVA: 0x001819E8 File Offset: 0x0017FBE8
		public static void Generated(this Writer writer, PlayingCard.ECardValue value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B57 RID: 23383 RVA: 0x00181A08 File Offset: 0x0017FC08
		public static void Generated(this Writer writer, NetworkObject[] value)
		{
			writer.WriteArray<NetworkObject>(value);
		}

		// Token: 0x06005B58 RID: 23384 RVA: 0x00181A24 File Offset: 0x0017FC24
		public static void Generated(this Writer writer, RTBGameController.EStage value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B59 RID: 23385 RVA: 0x00181A44 File Offset: 0x0017FC44
		public static void Generated(this Writer writer, SlotMachine.ESymbol value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x00181A64 File Offset: 0x0017FC64
		public static void Generated(this Writer writer, SlotMachine.ESymbol[] value)
		{
			writer.WriteArray<SlotMachine.ESymbol>(value);
		}

		// Token: 0x06005B5B RID: 23387 RVA: 0x00181A80 File Offset: 0x0017FC80
		public static void Generated(this Writer writer, EDoorSide value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B5C RID: 23388 RVA: 0x00181AA0 File Offset: 0x0017FCA0
		public static void Generated(this Writer writer, EVehicleColor value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B5D RID: 23389 RVA: 0x00181AC0 File Offset: 0x0017FCC0
		public static void Generated(this Writer writer, ParkData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteGuidAllocated(value.lotGUID);
			writer.WriteInt32(value.spotIndex, 1);
			writer.Write___ScheduleOne.Vehicles.EParkingAlignmentFishNet.Serializing.Generated(value.alignment);
		}

		// Token: 0x06005B5E RID: 23390 RVA: 0x00181B30 File Offset: 0x0017FD30
		public static void Generated(this Writer writer, EParkingAlignment value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B5F RID: 23391 RVA: 0x00181B50 File Offset: 0x0017FD50
		public static void Generated(this Writer writer, TrashContentData value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.Write___System.String[]FishNet.Serializing.Generated(value.TrashIDs);
			writer.Write___System.Int32[]FishNet.Serializing.Generated(value.TrashQuantities);
		}

		// Token: 0x06005B60 RID: 23392 RVA: 0x00181BA8 File Offset: 0x0017FDA8
		public static void Generated(this Writer writer, int[] value)
		{
			writer.WriteArray<int>(value);
		}

		// Token: 0x06005B61 RID: 23393 RVA: 0x00181BC4 File Offset: 0x0017FDC4
		public static void Generated(this Writer writer, Coordinate value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteInt32(value.x, 1);
			writer.WriteInt32(value.y, 1);
		}

		// Token: 0x06005B62 RID: 23394 RVA: 0x00181C28 File Offset: 0x0017FE28
		public static void Generated(this Writer writer, WeedAppearanceSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteColor32(value.MainColor);
			writer.WriteColor32(value.SecondaryColor);
			writer.WriteColor32(value.LeafColor);
			writer.WriteColor32(value.StemColor);
		}

		// Token: 0x06005B63 RID: 23395 RVA: 0x00181CA4 File Offset: 0x0017FEA4
		public static void Generated(this Writer writer, CocaineAppearanceSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteColor32(value.MainColor);
			writer.WriteColor32(value.SecondaryColor);
		}

		// Token: 0x06005B64 RID: 23396 RVA: 0x00181CFC File Offset: 0x0017FEFC
		public static void Generated(this Writer writer, MethAppearanceSettings value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteColor32(value.MainColor);
			writer.WriteColor32(value.SecondaryColor);
		}

		// Token: 0x06005B65 RID: 23397 RVA: 0x00181D54 File Offset: 0x0017FF54
		public static void Generated(this Writer writer, NewMixOperation value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.ProductID);
			writer.WriteString(value.IngredientID);
		}

		// Token: 0x06005B66 RID: 23398 RVA: 0x00181DAC File Offset: 0x0017FFAC
		public static void Generated(this Writer writer, Recycler.EState value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B67 RID: 23399 RVA: 0x00181DCC File Offset: 0x0017FFCC
		public static void Generated(this Writer writer, Jukebox.JukeboxState value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteInt32(value.CurrentVolume, 1);
			writer.WriteBoolean(value.IsPlaying);
			writer.WriteSingle(value.CurrentTrackTime, 0);
			writer.Write___System.Int32[]FishNet.Serializing.Generated(value.TrackOrder);
			writer.WriteInt32(value.CurrentTrackOrderIndex, 1);
			writer.WriteBoolean(value.Shuffle);
			writer.Write___ScheduleOne.ObjectScripts.Jukebox/ERepeatModeFishNet.Serializing.Generated(value.RepeatMode);
			writer.WriteBoolean(value.Sync);
		}

		// Token: 0x06005B68 RID: 23400 RVA: 0x00181EA0 File Offset: 0x001800A0
		public static void Generated(this Writer writer, Jukebox.ERepeatMode value)
		{
			writer.WriteInt32((int)value, 1);
		}

		// Token: 0x06005B69 RID: 23401 RVA: 0x00181EC0 File Offset: 0x001800C0
		public static void Generated(this Writer writer, CoordinateProceduralTilePair value)
		{
			writer.Write___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generated(value.coord);
			writer.WriteNetworkObject(value.tileParent);
			writer.WriteInt32(value.tileIndex, 1);
		}

		// Token: 0x06005B6A RID: 23402 RVA: 0x00181F08 File Offset: 0x00180108
		public static void List(this Writer writer, List<CoordinateProceduralTilePair> value)
		{
			writer.WriteList<CoordinateProceduralTilePair>(value);
		}

		// Token: 0x06005B6B RID: 23403 RVA: 0x00181F24 File Offset: 0x00180124
		public static void Generated(this Writer writer, ChemistryCookOperation value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.RecipeID);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value.ProductQuality);
			writer.WriteColor(value.StartLiquidColor, 1);
			writer.WriteSingle(value.LiquidLevel, 0);
			writer.WriteInt32(value.CurrentTime, 1);
		}

		// Token: 0x06005B6C RID: 23404 RVA: 0x00181FC4 File Offset: 0x001801C4
		public static void Generated(this Writer writer, DryingOperation value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.ItemID);
			writer.WriteInt32(value.Quantity, 1);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value.StartQuality);
			writer.WriteInt32(value.Time, 1);
		}

		// Token: 0x06005B6D RID: 23405 RVA: 0x0018204C File Offset: 0x0018024C
		public static void Generated(this Writer writer, OvenCookOperation value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.IngredientID);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value.IngredientQuality);
			writer.WriteInt32(value.IngredientQuantity, 1);
			writer.WriteString(value.ProductID);
			writer.WriteInt32(value.CookProgress, 1);
		}

		// Token: 0x06005B6E RID: 23406 RVA: 0x001820E4 File Offset: 0x001802E4
		public static void Generated(this Writer writer, MixOperation value)
		{
			if (value == null)
			{
				writer.WriteBoolean(true);
				return;
			}
			writer.WriteBoolean(false);
			writer.WriteString(value.ProductID);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value.ProductQuality);
			writer.WriteString(value.IngredientID);
			writer.WriteInt32(value.Quantity, 1);
		}
	}
}
