using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Properties;
using ScheduleOne.Properties.MixMaps;
using ScheduleOne.StationFramework;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Product
{
	// Token: 0x02000933 RID: 2355
	public class ProductManager : NetworkSingleton<ProductManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06003F5F RID: 16223 RVA: 0x0010A24C File Offset: 0x0010844C
		public static bool MethDiscovered
		{
			get
			{
				return ProductManager.DiscoveredProducts.Any((ProductDefinition p) => p.ID == "meth");
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06003F60 RID: 16224 RVA: 0x0010A277 File Offset: 0x00108477
		public static bool CocaineDiscovered
		{
			get
			{
				return ProductManager.DiscoveredProducts.Any((ProductDefinition p) => p.ID == "cocaine");
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06003F61 RID: 16225 RVA: 0x0010A2A2 File Offset: 0x001084A2
		// (set) Token: 0x06003F62 RID: 16226 RVA: 0x0010A2A9 File Offset: 0x001084A9
		public static bool IsAcceptingOrders { get; private set; } = true;

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06003F63 RID: 16227 RVA: 0x0010A2B1 File Offset: 0x001084B1
		// (set) Token: 0x06003F64 RID: 16228 RVA: 0x0010A2B9 File Offset: 0x001084B9
		public NewMixOperation CurrentMixOperation { get; private set; }

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06003F65 RID: 16229 RVA: 0x0010A2C2 File Offset: 0x001084C2
		public bool IsMixingInProgress
		{
			get
			{
				return this.CurrentMixOperation != null;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06003F66 RID: 16230 RVA: 0x0010A2CD File Offset: 0x001084CD
		// (set) Token: 0x06003F67 RID: 16231 RVA: 0x0010A2D5 File Offset: 0x001084D5
		public bool IsMixComplete { get; private set; }

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06003F68 RID: 16232 RVA: 0x0010A2DE File Offset: 0x001084DE
		// (set) Token: 0x06003F69 RID: 16233 RVA: 0x0010A2E6 File Offset: 0x001084E6
		public float TimeSinceProductListingChanged { get; private set; }

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06003F6A RID: 16234 RVA: 0x0010A2EF File Offset: 0x001084EF
		public string SaveFolderName
		{
			get
			{
				return "Products";
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06003F6B RID: 16235 RVA: 0x0010A2EF File Offset: 0x001084EF
		public string SaveFileName
		{
			get
			{
				return "Products";
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06003F6C RID: 16236 RVA: 0x0010A2F6 File Offset: 0x001084F6
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06003F6D RID: 16237 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06003F6E RID: 16238 RVA: 0x0010A2FE File Offset: 0x001084FE
		// (set) Token: 0x06003F6F RID: 16239 RVA: 0x0010A306 File Offset: 0x00108506
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06003F70 RID: 16240 RVA: 0x0010A30F File Offset: 0x0010850F
		// (set) Token: 0x06003F71 RID: 16241 RVA: 0x0010A317 File Offset: 0x00108517
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06003F72 RID: 16242 RVA: 0x0010A320 File Offset: 0x00108520
		// (set) Token: 0x06003F73 RID: 16243 RVA: 0x0010A328 File Offset: 0x00108528
		public bool HasChanged { get; set; } = true;

		// Token: 0x06003F74 RID: 16244 RVA: 0x0010A331 File Offset: 0x00108531
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Product.ProductManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x0010A348 File Offset: 0x00108548
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(new UnityAction(this.Clean));
			foreach (ProductDefinition productDefinition in this.DefaultKnownProducts)
			{
				productDefinition.OnValidate();
				if (this.highestValueProduct == null || productDefinition.MarketValue > this.highestValueProduct.MarketValue)
				{
					this.highestValueProduct = productDefinition;
				}
			}
			foreach (ProductDefinition productDefinition2 in this.AllProducts)
			{
				if (!this.ProductNames.Contains(productDefinition2.Name))
				{
					this.ProductNames.Add(productDefinition2.Name);
				}
				if (!this.ProductPrices.ContainsKey(productDefinition2))
				{
					this.ProductPrices.Add(productDefinition2, productDefinition2.MarketValue);
				}
			}
			NetworkSingleton<TimeManager>.Instance._onSleepEnd.AddListener(new UnityAction(this.OnNewDay));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.OnMinPass));
			foreach (PropertyItemDefinition propertyItemDefinition in this.ValidMixIngredients)
			{
				for (int i = 0; i < propertyItemDefinition.Properties.Count; i++)
				{
					if (!Singleton<PropertyUtility>.Instance.AllProperties.Contains(propertyItemDefinition.Properties[i]))
					{
						string[] array = new string[5];
						array[0] = "Mixer ";
						array[1] = propertyItemDefinition.Name;
						array[2] = " has property ";
						int num = 3;
						Property property = propertyItemDefinition.Properties[i];
						array[num] = ((property != null) ? property.ToString() : null);
						array[4] = " that is not in the valid properties list";
						Console.LogError(string.Concat(array), null);
					}
				}
			}
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x0010A574 File Offset: 0x00108774
		public override void OnStartServer()
		{
			base.OnStartServer();
			for (int i = 0; i < this.DefaultKnownProducts.Count; i++)
			{
				this.SetProductDiscovered(null, this.DefaultKnownProducts[i].ID, false);
			}
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x0010A5B6 File Offset: 0x001087B6
		public override void OnStartClient()
		{
			base.OnStartClient();
			this.RefreshHighestValueProduct();
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x0010A5C4 File Offset: 0x001087C4
		private void Update()
		{
			this.TimeSinceProductListingChanged += Time.deltaTime;
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x0010A5D8 File Offset: 0x001087D8
		private void Clean()
		{
			ProductManager.DiscoveredProducts.Clear();
			ProductManager.ListedProducts.Clear();
			ProductManager.FavouritedProducts.Clear();
			ProductManager.IsAcceptingOrders = true;
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x0010A5FE File Offset: 0x001087FE
		[ServerRpc(RequireOwnership = false)]
		public void SetMethDiscovered()
		{
			this.RpcWriter___Server_SetMethDiscovered_2166136261();
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x0010A606 File Offset: 0x00108806
		[ServerRpc(RequireOwnership = false)]
		public void SetCocaineDiscovered()
		{
			this.RpcWriter___Server_SetCocaineDiscovered_2166136261();
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x0010A610 File Offset: 0x00108810
		public MixerMap GetMixerMap(EDrugType type)
		{
			switch (type)
			{
			case EDrugType.Marijuana:
				return this.WeedMixMap;
			case EDrugType.Methamphetamine:
				return this.MethMixMap;
			case EDrugType.Cocaine:
				return this.CokeMixMap;
			default:
				Console.LogError("No mixer map found for " + type.ToString(), null);
				return null;
			}
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x0010A664 File Offset: 0x00108864
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			foreach (ProductDefinition productDefinition in this.createdProducts)
			{
				if (productDefinition is WeedDefinition)
				{
					WeedDefinition weedDefinition = productDefinition as WeedDefinition;
					WeedAppearanceSettings appearance = new WeedAppearanceSettings(weedDefinition.MainMat.color, weedDefinition.SecondaryMat.color, weedDefinition.LeafMat.color, weedDefinition.StemMat.color);
					List<string> list = new List<string>();
					foreach (Property property in weedDefinition.Properties)
					{
						list.Add(property.ID);
					}
					this.CreateWeed(connection, productDefinition.Name, productDefinition.ID, EDrugType.Marijuana, list, appearance);
				}
				else if (productDefinition is MethDefinition)
				{
					MethDefinition methDefinition = productDefinition as MethDefinition;
					MethAppearanceSettings appearanceSettings = methDefinition.AppearanceSettings;
					List<string> list2 = new List<string>();
					foreach (Property property2 in methDefinition.Properties)
					{
						list2.Add(property2.ID);
					}
					this.CreateMeth(connection, productDefinition.Name, productDefinition.ID, EDrugType.Methamphetamine, list2, appearanceSettings);
				}
				else if (productDefinition is CocaineDefinition)
				{
					CocaineDefinition cocaineDefinition = productDefinition as CocaineDefinition;
					CocaineAppearanceSettings appearanceSettings2 = cocaineDefinition.AppearanceSettings;
					List<string> list3 = new List<string>();
					foreach (Property property3 in cocaineDefinition.Properties)
					{
						list3.Add(property3.ID);
					}
					this.CreateCocaine(connection, productDefinition.Name, productDefinition.ID, EDrugType.Cocaine, list3, appearanceSettings2);
				}
			}
			for (int i = 0; i < this.mixRecipes.Count; i++)
			{
				this.CreateMixRecipe(null, this.mixRecipes[i].Ingredients[1].Items[0].ID, this.mixRecipes[i].Ingredients[0].Items[0].ID, this.mixRecipes[i].Product.Item.ID);
			}
			for (int j = 0; j < ProductManager.DiscoveredProducts.Count; j++)
			{
				this.SetProductDiscovered(connection, ProductManager.DiscoveredProducts[j].ID, false);
			}
			for (int k = 0; k < ProductManager.ListedProducts.Count; k++)
			{
				this.SetProductListed(connection, ProductManager.ListedProducts[k].ID, true);
			}
			for (int l = 0; l < ProductManager.FavouritedProducts.Count; l++)
			{
				this.SetProductFavourited(connection, ProductManager.FavouritedProducts[l].ID, true);
			}
			foreach (KeyValuePair<ProductDefinition, float> keyValuePair in this.ProductPrices)
			{
				this.SetPrice(connection, keyValuePair.Key.ID, keyValuePair.Value);
			}
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x0010AA3C File Offset: 0x00108C3C
		private void OnMinPass()
		{
			if (!NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				return;
			}
			if (GameManager.IS_TUTORIAL)
			{
				return;
			}
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("SecondUniqueProductDiscovered"))
			{
				float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Inventory_OGKush");
				if (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Inventory_Weed_Count") > value)
				{
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("SecondUniqueProductDiscovered", true.ToString(), true);
					if (this.onSecondUniqueProductCreated != null)
					{
						this.onSecondUniqueProductCreated.Invoke();
					}
				}
			}
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x0010AAB8 File Offset: 0x00108CB8
		private void OnNewDay()
		{
			if (InstanceFinder.IsServer && this.CurrentMixOperation != null && !this.IsMixComplete)
			{
				this.SetMixOperation(this.CurrentMixOperation, true);
			}
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x0010AADE File Offset: 0x00108CDE
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetProductListed(string productID, bool listed)
		{
			this.RpcWriter___Server_SetProductListed_310431262(productID, listed);
			this.RpcLogic___SetProductListed_310431262(productID, listed);
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x0010AAFC File Offset: 0x00108CFC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetProductListed(NetworkConnection conn, string productID, bool listed)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetProductListed_619441887(conn, productID, listed);
				this.RpcLogic___SetProductListed_619441887(conn, productID, listed);
			}
			else
			{
				this.RpcWriter___Target_SetProductListed_619441887(conn, productID, listed);
			}
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x0010AB49 File Offset: 0x00108D49
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetProductFavourited(string productID, bool listed)
		{
			this.RpcWriter___Server_SetProductFavourited_310431262(productID, listed);
			this.RpcLogic___SetProductFavourited_310431262(productID, listed);
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x0010AB68 File Offset: 0x00108D68
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetProductFavourited(NetworkConnection conn, string productID, bool fav)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetProductFavourited_619441887(conn, productID, fav);
				this.RpcLogic___SetProductFavourited_619441887(conn, productID, fav);
			}
			else
			{
				this.RpcWriter___Target_SetProductFavourited_619441887(conn, productID, fav);
			}
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x0010ABB5 File Offset: 0x00108DB5
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void DiscoverProduct(string productID)
		{
			this.RpcWriter___Server_DiscoverProduct_3615296227(productID);
			this.RpcLogic___DiscoverProduct_3615296227(productID);
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x0010ABCC File Offset: 0x00108DCC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetProductDiscovered(NetworkConnection conn, string productID, bool autoList)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetProductDiscovered_619441887(conn, productID, autoList);
				this.RpcLogic___SetProductDiscovered_619441887(conn, productID, autoList);
			}
			else
			{
				this.RpcWriter___Target_SetProductDiscovered_619441887(conn, productID, autoList);
			}
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x0010AC19 File Offset: 0x00108E19
		public void SetIsAcceptingOrder(bool accepting)
		{
			ProductManager.IsAcceptingOrders = accepting;
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x0010AC21 File Offset: 0x00108E21
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void CreateWeed_Server(string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			this.RpcWriter___Server_CreateWeed_Server_2331775230(name, id, type, properties, appearance);
			this.RpcLogic___CreateWeed_Server_2331775230(name, id, type, properties, appearance);
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x0010AC58 File Offset: 0x00108E58
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		private void CreateWeed(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateWeed_1777266891(conn, name, id, type, properties, appearance);
				this.RpcLogic___CreateWeed_1777266891(conn, name, id, type, properties, appearance);
			}
			else
			{
				this.RpcWriter___Target_CreateWeed_1777266891(conn, name, id, type, properties, appearance);
			}
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x0010ACC9 File Offset: 0x00108EC9
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void CreateCocaine_Server(string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			this.RpcWriter___Server_CreateCocaine_Server_891166717(name, id, type, properties, appearance);
			this.RpcLogic___CreateCocaine_Server_891166717(name, id, type, properties, appearance);
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x0010AD00 File Offset: 0x00108F00
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		private void CreateCocaine(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateCocaine_1327282946(conn, name, id, type, properties, appearance);
				this.RpcLogic___CreateCocaine_1327282946(conn, name, id, type, properties, appearance);
			}
			else
			{
				this.RpcWriter___Target_CreateCocaine_1327282946(conn, name, id, type, properties, appearance);
			}
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x0010AD71 File Offset: 0x00108F71
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void CreateMeth_Server(string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			this.RpcWriter___Server_CreateMeth_Server_4251728555(name, id, type, properties, appearance);
			this.RpcLogic___CreateMeth_Server_4251728555(name, id, type, properties, appearance);
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x0010ADA8 File Offset: 0x00108FA8
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		private void CreateMeth(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateMeth_1869045686(conn, name, id, type, properties, appearance);
				this.RpcLogic___CreateMeth_1869045686(conn, name, id, type, properties, appearance);
			}
			else
			{
				this.RpcWriter___Target_CreateMeth_1869045686(conn, name, id, type, properties, appearance);
			}
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x0010AE1C File Offset: 0x0010901C
		private void RefreshHighestValueProduct()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			for (int i = 0; i < ProductManager.DiscoveredProducts.Count; i++)
			{
				if (this.highestValueProduct == null || ProductManager.DiscoveredProducts[i].MarketValue > this.highestValueProduct.MarketValue)
				{
					this.highestValueProduct = ProductManager.DiscoveredProducts[i];
				}
			}
			float marketValue = this.highestValueProduct.MarketValue;
			if (marketValue >= 100f)
			{
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.MASTER_CHEF);
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("HighestValueProduct", marketValue.ToString(), true);
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x0010AEB8 File Offset: 0x001090B8
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendMixRecipe(string product, string mixer, string output)
		{
			this.RpcWriter___Server_SendMixRecipe_852232071(product, mixer, output);
			this.RpcLogic___SendMixRecipe_852232071(product, mixer, output);
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x0010AEE0 File Offset: 0x001090E0
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		public void CreateMixRecipe(NetworkConnection conn, string product, string mixer, string output)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateMixRecipe_1410895574(conn, product, mixer, output);
				this.RpcLogic___CreateMixRecipe_1410895574(conn, product, mixer, output);
			}
			else
			{
				this.RpcWriter___Target_CreateMixRecipe_1410895574(conn, product, mixer, output);
			}
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x0010AF3C File Offset: 0x0010913C
		public StationRecipe GetRecipe(string product, string mixer)
		{
			return this.mixRecipes.Find((StationRecipe r) => r.Product.Item.ID == product && r.Ingredients[0].Items[0].ID == mixer);
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x0010AF74 File Offset: 0x00109174
		public StationRecipe GetRecipe(List<Property> productProperties, Property mixerProperty)
		{
			foreach (StationRecipe stationRecipe in this.mixRecipes)
			{
				if (!(stationRecipe == null) && stationRecipe.Ingredients.Count >= 2)
				{
					ItemDefinition item = stationRecipe.Ingredients[0].Item;
					ItemDefinition item2 = stationRecipe.Ingredients[1].Item;
					if (!(item == null) && !(item2 == null))
					{
						PropertyItemDefinition propertyItemDefinition = item as PropertyItemDefinition;
						List<Property> list = (propertyItemDefinition != null) ? propertyItemDefinition.Properties : null;
						PropertyItemDefinition propertyItemDefinition2 = item2 as PropertyItemDefinition;
						List<Property> list2 = (propertyItemDefinition2 != null) ? propertyItemDefinition2.Properties : null;
						if (item2 is ProductDefinition)
						{
							PropertyItemDefinition propertyItemDefinition3 = item2 as PropertyItemDefinition;
							list = ((propertyItemDefinition3 != null) ? propertyItemDefinition3.Properties : null);
							PropertyItemDefinition propertyItemDefinition4 = item as PropertyItemDefinition;
							list2 = ((propertyItemDefinition4 != null) ? propertyItemDefinition4.Properties : null);
						}
						if (list.Count == productProperties.Count && list2.Count == 1)
						{
							bool flag = true;
							for (int i = 0; i < productProperties.Count; i++)
							{
								if (!list.Contains(productProperties[i]))
								{
									flag = false;
									break;
								}
							}
							if (flag && !(list2[0] != mixerProperty))
							{
								return stationRecipe;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x0010B0E8 File Offset: 0x001092E8
		[TargetRpc]
		private void GiveItem(NetworkConnection conn, string id)
		{
			this.RpcWriter___Target_GiveItem_2971853958(conn, id);
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x0010B0F8 File Offset: 0x001092F8
		public ProductDefinition GetKnownProduct(EDrugType type, List<Property> properties)
		{
			foreach (ProductDefinition productDefinition in this.AllProducts)
			{
				if (productDefinition.DrugTypes[0].DrugType == type && productDefinition.Properties.Count == properties.Count)
				{
					int num = 0;
					while (num < properties.Count && productDefinition.Properties.Contains(properties[num]))
					{
						if (num == properties.Count - 1)
						{
							return productDefinition;
						}
						num++;
					}
				}
			}
			return null;
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x0010B1A4 File Offset: 0x001093A4
		public float GetPrice(ProductDefinition product)
		{
			if (product == null)
			{
				Console.LogError("Product is null", null);
				return 1f;
			}
			if (this.ProductPrices.ContainsKey(product))
			{
				return Mathf.Clamp(this.ProductPrices[product], 1f, 999f);
			}
			Console.LogError("Price not found for product: " + product.ID + ". Returning market value", null);
			return Mathf.Clamp(product.MarketValue, 1f, 999f);
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x0010B225 File Offset: 0x00109425
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPrice(string productID, float value)
		{
			this.RpcWriter___Server_SendPrice_606697822(productID, value);
			this.RpcLogic___SendPrice_606697822(productID, value);
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x0010B244 File Offset: 0x00109444
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetPrice(NetworkConnection conn, string productID, float value)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetPrice_4077118173(conn, productID, value);
				this.RpcLogic___SetPrice_4077118173(conn, productID, value);
			}
			else
			{
				this.RpcWriter___Target_SetPrice_4077118173(conn, productID, value);
			}
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x0010B291 File Offset: 0x00109491
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendMixOperation(NewMixOperation operation, bool complete)
		{
			this.RpcWriter___Server_SendMixOperation_3670976965(operation, complete);
			this.RpcLogic___SendMixOperation_3670976965(operation, complete);
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x0010B2AF File Offset: 0x001094AF
		[ObserversRpc(RunLocally = true)]
		private void SetMixOperation(NewMixOperation operation, bool complete)
		{
			this.RpcWriter___Observers_SetMixOperation_3670976965(operation, complete);
			this.RpcLogic___SetMixOperation_3670976965(operation, complete);
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x0010B2D0 File Offset: 0x001094D0
		public string FinishAndNameMix(string productID, string ingredientID, string mixName)
		{
			if (!ProductManager.IsMixNameValid(mixName))
			{
				Console.LogError("Invalid mix name. Using random name", null);
				mixName = Singleton<NewMixScreen>.Instance.GenerateUniqueName(null, EDrugType.Marijuana);
			}
			string text = mixName.ToLower().Replace(" ", string.Empty);
			text = ProductManager.MakeIDFileSafe(text);
			text = text.Replace(" ", string.Empty);
			text = text.Replace("(", string.Empty);
			text = text.Replace(")", string.Empty);
			text = text.Replace("'", string.Empty);
			text = text.Replace("\"", string.Empty);
			text = text.Replace(":", string.Empty);
			text = text.Replace(";", string.Empty);
			text = text.Replace(",", string.Empty);
			text = text.Replace(".", string.Empty);
			text = text.Replace("!", string.Empty);
			text = text.Replace("?", string.Empty);
			this.FinishAndNameMix(productID, ingredientID, mixName, text);
			if (!InstanceFinder.IsServer)
			{
				this.SendFinishAndNameMix(productID, ingredientID, mixName, text);
			}
			return text;
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0010B3F4 File Offset: 0x001095F4
		public static string MakeIDFileSafe(string id)
		{
			id = id.ToLower();
			id = id.Replace(" ", string.Empty);
			id = id.Replace("(", string.Empty);
			id = id.Replace(")", string.Empty);
			id = id.Replace("'", string.Empty);
			id = id.Replace("\"", string.Empty);
			id = id.Replace(":", string.Empty);
			id = id.Replace(";", string.Empty);
			id = id.Replace(",", string.Empty);
			id = id.Replace(".", string.Empty);
			id = id.Replace("!", string.Empty);
			id = id.Replace("?", string.Empty);
			return id;
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x0010B4D0 File Offset: 0x001096D0
		public static bool IsMixNameValid(string mixName)
		{
			return !string.IsNullOrEmpty(mixName) && !string.IsNullOrWhiteSpace(mixName);
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x0010B4E8 File Offset: 0x001096E8
		[ObserversRpc(RunLocally = true)]
		private void FinishAndNameMix(string productID, string ingredientID, string mixName, string mixID)
		{
			this.RpcWriter___Observers_FinishAndNameMix_4237212381(productID, ingredientID, mixName, mixID);
			this.RpcLogic___FinishAndNameMix_4237212381(productID, ingredientID, mixName, mixID);
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x0010B521 File Offset: 0x00109721
		[ServerRpc(RequireOwnership = false)]
		private void SendFinishAndNameMix(string productID, string ingredientID, string mixName, string mixID)
		{
			this.RpcWriter___Server_SendFinishAndNameMix_4237212381(productID, ingredientID, mixName, mixID);
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x0010B539 File Offset: 0x00109739
		public static float CalculateProductValue(ProductDefinition product, float baseValue)
		{
			return ProductManager.CalculateProductValue(baseValue, product.Properties);
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x0010B548 File Offset: 0x00109748
		public static float CalculateProductValue(float baseValue, List<Property> properties)
		{
			float num = baseValue;
			float num2 = 1f;
			for (int i = 0; i < properties.Count; i++)
			{
				if (!(properties[i] == null))
				{
					num += (float)properties[i].ValueChange;
					num += baseValue * properties[i].AddBaseValueMultiple;
					num2 *= properties[i].ValueMultiplier;
				}
			}
			num *= num2;
			return (float)Mathf.RoundToInt(num);
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x0010B5B8 File Offset: 0x001097B8
		public virtual string GetSaveString()
		{
			string[] array = new string[ProductManager.DiscoveredProducts.Count];
			for (int i = 0; i < ProductManager.DiscoveredProducts.Count; i++)
			{
				if (!(ProductManager.DiscoveredProducts[i] == null))
				{
					array[i] = ProductManager.DiscoveredProducts[i].ID;
				}
			}
			string[] array2 = new string[ProductManager.ListedProducts.Count];
			for (int j = 0; j < ProductManager.ListedProducts.Count; j++)
			{
				if (!(ProductManager.ListedProducts[j] == null))
				{
					array2[j] = ProductManager.ListedProducts[j].ID;
				}
			}
			string[] array3 = new string[ProductManager.FavouritedProducts.Count];
			for (int k = 0; k < ProductManager.FavouritedProducts.Count; k++)
			{
				if (!(ProductManager.FavouritedProducts[k] == null))
				{
					array3[k] = ProductManager.FavouritedProducts[k].ID;
				}
			}
			MixRecipeData[] array4 = new MixRecipeData[this.mixRecipes.Count];
			for (int l = 0; l < this.mixRecipes.Count; l++)
			{
				if (!(this.mixRecipes[l] == null))
				{
					if (this.mixRecipes[l].Ingredients.Count < 2)
					{
						Console.LogWarning("Mix recipe has less than 2 ingredients", null);
					}
					else if (this.mixRecipes[l].Product != null)
					{
						try
						{
							array4[l] = new MixRecipeData(this.mixRecipes[l].Ingredients[1].Items[0].ID, this.mixRecipes[l].Ingredients[0].Items[0].ID, this.mixRecipes[l].Product.Item.ID);
						}
						catch (Exception ex)
						{
							string str = "Failed to save mix recipe: ";
							Exception ex2 = ex;
							Console.LogError(str + ((ex2 != null) ? ex2.ToString() : null), null);
						}
					}
				}
			}
			StringIntPair[] array5 = new StringIntPair[this.ProductPrices.Count];
			for (int m = 0; m < this.AllProducts.Count; m++)
			{
				if (!(this.AllProducts[m] == null))
				{
					float f;
					if (this.ProductPrices.ContainsKey(this.AllProducts[m]))
					{
						f = this.ProductPrices[this.AllProducts[m]];
					}
					else
					{
						f = this.AllProducts[m].MarketValue;
					}
					array5[m] = new StringIntPair(this.AllProducts[m].ID, Mathf.RoundToInt(f));
				}
			}
			List<WeedProductData> list = new List<WeedProductData>();
			List<MethProductData> list2 = new List<MethProductData>();
			List<CocaineProductData> list3 = new List<CocaineProductData>();
			for (int n = 0; n < this.createdProducts.Count; n++)
			{
				if (!(this.createdProducts[n] == null))
				{
					switch (this.createdProducts[n].DrugType)
					{
					case EDrugType.Marijuana:
						list.Add((this.createdProducts[n] as WeedDefinition).GetSaveData() as WeedProductData);
						break;
					case EDrugType.Methamphetamine:
						list2.Add((this.createdProducts[n] as MethDefinition).GetSaveData() as MethProductData);
						break;
					case EDrugType.Cocaine:
						list3.Add((this.createdProducts[n] as CocaineDefinition).GetSaveData() as CocaineProductData);
						break;
					default:
						Console.LogError("Product type not supported: " + this.createdProducts[n].DrugType.ToString(), null);
						break;
					}
				}
			}
			return new ProductManagerData(array, array2, this.CurrentMixOperation, this.IsMixComplete, array4, array5, array3, list.ToArray(), list2.ToArray(), list3.ToArray()).GetJson(true);
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x0010BAA4 File Offset: 0x00109CA4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Product.ProductManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Product.ProductManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetMethDiscovered_2166136261));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_SetCocaineDiscovered_2166136261));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SetProductListed_310431262));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetProductListed_619441887));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetProductListed_619441887));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SetProductFavourited_310431262));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_SetProductFavourited_619441887));
			base.RegisterTargetRpc(7U, new ClientRpcDelegate(this.RpcReader___Target_SetProductFavourited_619441887));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_DiscoverProduct_3615296227));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_SetProductDiscovered_619441887));
			base.RegisterTargetRpc(10U, new ClientRpcDelegate(this.RpcReader___Target_SetProductDiscovered_619441887));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_CreateWeed_Server_2331775230));
			base.RegisterTargetRpc(12U, new ClientRpcDelegate(this.RpcReader___Target_CreateWeed_1777266891));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_CreateWeed_1777266891));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_CreateCocaine_Server_891166717));
			base.RegisterTargetRpc(15U, new ClientRpcDelegate(this.RpcReader___Target_CreateCocaine_1327282946));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_CreateCocaine_1327282946));
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_CreateMeth_Server_4251728555));
			base.RegisterTargetRpc(18U, new ClientRpcDelegate(this.RpcReader___Target_CreateMeth_1869045686));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_CreateMeth_1869045686));
			base.RegisterServerRpc(20U, new ServerRpcDelegate(this.RpcReader___Server_SendMixRecipe_852232071));
			base.RegisterTargetRpc(21U, new ClientRpcDelegate(this.RpcReader___Target_CreateMixRecipe_1410895574));
			base.RegisterObserversRpc(22U, new ClientRpcDelegate(this.RpcReader___Observers_CreateMixRecipe_1410895574));
			base.RegisterTargetRpc(23U, new ClientRpcDelegate(this.RpcReader___Target_GiveItem_2971853958));
			base.RegisterServerRpc(24U, new ServerRpcDelegate(this.RpcReader___Server_SendPrice_606697822));
			base.RegisterObserversRpc(25U, new ClientRpcDelegate(this.RpcReader___Observers_SetPrice_4077118173));
			base.RegisterTargetRpc(26U, new ClientRpcDelegate(this.RpcReader___Target_SetPrice_4077118173));
			base.RegisterServerRpc(27U, new ServerRpcDelegate(this.RpcReader___Server_SendMixOperation_3670976965));
			base.RegisterObserversRpc(28U, new ClientRpcDelegate(this.RpcReader___Observers_SetMixOperation_3670976965));
			base.RegisterObserversRpc(29U, new ClientRpcDelegate(this.RpcReader___Observers_FinishAndNameMix_4237212381));
			base.RegisterServerRpc(30U, new ServerRpcDelegate(this.RpcReader___Server_SendFinishAndNameMix_4237212381));
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x0010BD91 File Offset: 0x00109F91
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Product.ProductManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Product.ProductManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x0010BDAA File Offset: 0x00109FAA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0010BDB8 File Offset: 0x00109FB8
		private void RpcWriter___Server_SetMethDiscovered_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x0010BE52 File Offset: 0x0010A052
		public void RpcLogic___SetMethDiscovered_2166136261()
		{
			this.SetProductDiscovered(null, "meth", false);
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0010BE64 File Offset: 0x0010A064
		private void RpcReader___Server_SetMethDiscovered_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetMethDiscovered_2166136261();
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x0010BE84 File Offset: 0x0010A084
		private void RpcWriter___Server_SetCocaineDiscovered_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x0010BF1E File Offset: 0x0010A11E
		public void RpcLogic___SetCocaineDiscovered_2166136261()
		{
			this.SetProductDiscovered(null, "cocaine", false);
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0010BF30 File Offset: 0x0010A130
		private void RpcReader___Server_SetCocaineDiscovered_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetCocaineDiscovered_2166136261();
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x0010BF50 File Offset: 0x0010A150
		private void RpcWriter___Server_SetProductListed_310431262(string productID, bool listed)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteBoolean(listed);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x0010C004 File Offset: 0x0010A204
		public void RpcLogic___SetProductListed_310431262(string productID, bool listed)
		{
			this.SetProductListed(null, productID, listed);
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x0010C010 File Offset: 0x0010A210
		private void RpcReader___Server_SetProductListed_310431262(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string productID = PooledReader0.ReadString();
			bool listed = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetProductListed_310431262(productID, listed);
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x0010C060 File Offset: 0x0010A260
		private void RpcWriter___Observers_SetProductListed_619441887(NetworkConnection conn, string productID, bool listed)
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
			writer.WriteString(productID);
			writer.WriteBoolean(listed);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x0010C124 File Offset: 0x0010A324
		public void RpcLogic___SetProductListed_619441887(NetworkConnection conn, string productID, bool listed)
		{
			ProductDefinition productDefinition = this.AllProducts.Find((ProductDefinition p) => p.ID == productID);
			if (productDefinition == null)
			{
				Console.LogWarning("SetProductListed: product is not found (" + productID + ")", null);
				return;
			}
			if (!ProductManager.DiscoveredProducts.Contains(productDefinition))
			{
				Console.LogWarning("SetProductListed: product is not yet discovered", null);
			}
			if (listed)
			{
				if (!ProductManager.ListedProducts.Contains(productDefinition))
				{
					ProductManager.ListedProducts.Add(productDefinition);
				}
			}
			else if (ProductManager.ListedProducts.Contains(productDefinition))
			{
				ProductManager.ListedProducts.Remove(productDefinition);
			}
			if (NetworkSingleton<VariableDatabase>.InstanceExists && InstanceFinder.IsServer)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("ListedProductsCount", ProductManager.ListedProducts.Count.ToString(), true);
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("OGKushListed", (ProductManager.ListedProducts.Find((ProductDefinition x) => x.ID == "ogkush") != null).ToString(), true);
			}
			this.HasChanged = true;
			this.TimeSinceProductListingChanged = 0f;
			if (listed)
			{
				if (this.onProductListed != null)
				{
					this.onProductListed(productDefinition);
					return;
				}
			}
			else if (this.onProductDelisted != null)
			{
				this.onProductDelisted(productDefinition);
			}
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x0010C280 File Offset: 0x0010A480
		private void RpcReader___Observers_SetProductListed_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool listed = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetProductListed_619441887(null, productID, listed);
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x0010C2D0 File Offset: 0x0010A4D0
		private void RpcWriter___Target_SetProductListed_619441887(NetworkConnection conn, string productID, bool listed)
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
			writer.WriteString(productID);
			writer.WriteBoolean(listed);
			base.SendTargetRpc(4U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x0010C394 File Offset: 0x0010A594
		private void RpcReader___Target_SetProductListed_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool listed = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetProductListed_619441887(base.LocalConnection, productID, listed);
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x0010C3DC File Offset: 0x0010A5DC
		private void RpcWriter___Server_SetProductFavourited_310431262(string productID, bool listed)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteBoolean(listed);
			base.SendServerRpc(5U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x0010C490 File Offset: 0x0010A690
		public void RpcLogic___SetProductFavourited_310431262(string productID, bool listed)
		{
			this.SetProductFavourited(null, productID, listed);
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x0010C49C File Offset: 0x0010A69C
		private void RpcReader___Server_SetProductFavourited_310431262(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string productID = PooledReader0.ReadString();
			bool listed = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetProductFavourited_310431262(productID, listed);
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x0010C4EC File Offset: 0x0010A6EC
		private void RpcWriter___Observers_SetProductFavourited_619441887(NetworkConnection conn, string productID, bool fav)
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
			writer.WriteString(productID);
			writer.WriteBoolean(fav);
			base.SendObserversRpc(6U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x0010C5B0 File Offset: 0x0010A7B0
		public void RpcLogic___SetProductFavourited_619441887(NetworkConnection conn, string productID, bool fav)
		{
			ProductDefinition productDefinition = this.AllProducts.Find((ProductDefinition p) => p.ID == productID);
			if (productDefinition == null)
			{
				Console.LogWarning("SetProductFavourited: product is not found (" + productID + ")", null);
				return;
			}
			if (!ProductManager.DiscoveredProducts.Contains(productDefinition))
			{
				Console.LogWarning("SetProductFavourited: product is not yet discovered", null);
			}
			if (fav)
			{
				if (!ProductManager.FavouritedProducts.Contains(productDefinition))
				{
					ProductManager.FavouritedProducts.Add(productDefinition);
				}
			}
			else if (ProductManager.FavouritedProducts.Contains(productDefinition))
			{
				ProductManager.FavouritedProducts.Remove(productDefinition);
			}
			this.HasChanged = true;
			if (fav)
			{
				if (this.onProductFavourited != null)
				{
					this.onProductFavourited(productDefinition);
					return;
				}
			}
			else if (this.onProductUnfavourited != null)
			{
				this.onProductUnfavourited(productDefinition);
			}
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x0010C688 File Offset: 0x0010A888
		private void RpcReader___Observers_SetProductFavourited_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool fav = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetProductFavourited_619441887(null, productID, fav);
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x0010C6D8 File Offset: 0x0010A8D8
		private void RpcWriter___Target_SetProductFavourited_619441887(NetworkConnection conn, string productID, bool fav)
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
			writer.WriteString(productID);
			writer.WriteBoolean(fav);
			base.SendTargetRpc(7U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x0010C79C File Offset: 0x0010A99C
		private void RpcReader___Target_SetProductFavourited_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool fav = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetProductFavourited_619441887(base.LocalConnection, productID, fav);
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x0010C7E4 File Offset: 0x0010A9E4
		private void RpcWriter___Server_DiscoverProduct_3615296227(string productID)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x0010C88B File Offset: 0x0010AA8B
		public void RpcLogic___DiscoverProduct_3615296227(string productID)
		{
			this.SetProductDiscovered(null, productID, true);
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x0010C898 File Offset: 0x0010AA98
		private void RpcReader___Server_DiscoverProduct_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string productID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___DiscoverProduct_3615296227(productID);
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x0010C8D8 File Offset: 0x0010AAD8
		private void RpcWriter___Observers_SetProductDiscovered_619441887(NetworkConnection conn, string productID, bool autoList)
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
			writer.WriteString(productID);
			writer.WriteBoolean(autoList);
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x0010C99C File Offset: 0x0010AB9C
		public void RpcLogic___SetProductDiscovered_619441887(NetworkConnection conn, string productID, bool autoList)
		{
			ProductDefinition productDefinition = this.AllProducts.Find((ProductDefinition p) => p.ID == productID);
			if (productDefinition == null)
			{
				Console.LogWarning("SetProductDiscovered: product is not found", null);
				return;
			}
			if (!ProductManager.DiscoveredProducts.Contains(productDefinition))
			{
				ProductManager.DiscoveredProducts.Add(productDefinition);
				if (autoList)
				{
					this.SetProductListed(productID, true);
				}
				if (this.onProductDiscovered != null)
				{
					this.onProductDiscovered(productDefinition);
				}
			}
			this.HasChanged = true;
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x0010CA28 File Offset: 0x0010AC28
		private void RpcReader___Observers_SetProductDiscovered_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool autoList = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetProductDiscovered_619441887(null, productID, autoList);
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x0010CA78 File Offset: 0x0010AC78
		private void RpcWriter___Target_SetProductDiscovered_619441887(NetworkConnection conn, string productID, bool autoList)
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
			writer.WriteString(productID);
			writer.WriteBoolean(autoList);
			base.SendTargetRpc(10U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x0010CB3C File Offset: 0x0010AD3C
		private void RpcReader___Target_SetProductDiscovered_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool autoList = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetProductDiscovered_619441887(base.LocalConnection, productID, autoList);
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x0010CB84 File Offset: 0x0010AD84
		private void RpcWriter___Server_CreateWeed_Server_2331775230(string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendServerRpc(11U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x0010CC5F File Offset: 0x0010AE5F
		public void RpcLogic___CreateWeed_Server_2331775230(string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			this.CreateWeed(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x0010CC70 File Offset: 0x0010AE70
		private void RpcReader___Server_CreateWeed_Server_2331775230(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			WeedAppearanceSettings appearance = GeneratedReaders___Internal.Read___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CreateWeed_Server_2331775230(name, id, type, properties, appearance);
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x0010CCF4 File Offset: 0x0010AEF4
		private void RpcWriter___Target_CreateWeed_1777266891(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
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
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendTargetRpc(12U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x0010CDE0 File Offset: 0x0010AFE0
		private void RpcLogic___CreateWeed_1777266891(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			if (Registry.ItemExists(id))
			{
				return;
			}
			WeedDefinition weedDefinition = UnityEngine.Object.Instantiate<WeedDefinition>(this.DefaultWeed);
			weedDefinition.name = name;
			weedDefinition.Name = name;
			weedDefinition.Description = string.Empty;
			weedDefinition.ID = id;
			weedDefinition.Initialize(Singleton<PropertyUtility>.Instance.GetProperties(properties), new List<EDrugType>
			{
				type
			}, appearance);
			this.AllProducts.Add(weedDefinition);
			this.ProductPrices.Add(weedDefinition, weedDefinition.MarketValue);
			this.ProductNames.Add(name);
			this.createdProducts.Add(weedDefinition);
			Singleton<Registry>.Instance.AddToRegistry(weedDefinition);
			weedDefinition.Icon = Singleton<ProductIconManager>.Instance.GenerateIcons(id);
			if (weedDefinition.Icon == null)
			{
				Console.LogError("Failed to generate icons for " + name, null);
			}
			this.SetProductDiscovered(null, id, false);
			this.RefreshHighestValueProduct();
			if (this.onNewProductCreated != null)
			{
				this.onNewProductCreated(weedDefinition);
			}
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x0010CED8 File Offset: 0x0010B0D8
		private void RpcReader___Target_CreateWeed_1777266891(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			WeedAppearanceSettings appearance = GeneratedReaders___Internal.Read___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateWeed_1777266891(base.LocalConnection, name, id, type, properties, appearance);
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x0010CF54 File Offset: 0x0010B154
		private void RpcWriter___Observers_CreateWeed_1777266891(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
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
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendObserversRpc(13U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x0010D040 File Offset: 0x0010B240
		private void RpcReader___Observers_CreateWeed_1777266891(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			WeedAppearanceSettings appearance = GeneratedReaders___Internal.Read___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateWeed_1777266891(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x0010D0C0 File Offset: 0x0010B2C0
		private void RpcWriter___Server_CreateCocaine_Server_891166717(string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendServerRpc(14U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x0010D19B File Offset: 0x0010B39B
		public void RpcLogic___CreateCocaine_Server_891166717(string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			this.CreateCocaine(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x0010D1AC File Offset: 0x0010B3AC
		private void RpcReader___Server_CreateCocaine_Server_891166717(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			CocaineAppearanceSettings appearance = GeneratedReaders___Internal.Read___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CreateCocaine_Server_891166717(name, id, type, properties, appearance);
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x0010D230 File Offset: 0x0010B430
		private void RpcWriter___Target_CreateCocaine_1327282946(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
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
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendTargetRpc(15U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x0010D31C File Offset: 0x0010B51C
		private void RpcLogic___CreateCocaine_1327282946(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			if (Registry.GetItem(id) != null)
			{
				Console.LogError("Product with ID " + id + " already exists", null);
				return;
			}
			CocaineDefinition cocaineDefinition = UnityEngine.Object.Instantiate<CocaineDefinition>(this.DefaultCocaine);
			cocaineDefinition.name = name;
			cocaineDefinition.Name = name;
			cocaineDefinition.Description = string.Empty;
			cocaineDefinition.ID = id;
			cocaineDefinition.Initialize(Singleton<PropertyUtility>.Instance.GetProperties(properties), new List<EDrugType>
			{
				type
			}, appearance);
			this.AllProducts.Add(cocaineDefinition);
			this.ProductPrices.Add(cocaineDefinition, cocaineDefinition.MarketValue);
			this.ProductNames.Add(name);
			this.createdProducts.Add(cocaineDefinition);
			Singleton<Registry>.Instance.AddToRegistry(cocaineDefinition);
			cocaineDefinition.Icon = Singleton<ProductIconManager>.Instance.GenerateIcons(id);
			if (cocaineDefinition.Icon == null)
			{
				Console.LogError("Failed to generate icons for " + name, null);
			}
			this.SetProductDiscovered(null, id, false);
			this.RefreshHighestValueProduct();
			if (this.onNewProductCreated != null)
			{
				this.onNewProductCreated(cocaineDefinition);
			}
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x0010D430 File Offset: 0x0010B630
		private void RpcReader___Target_CreateCocaine_1327282946(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			CocaineAppearanceSettings appearance = GeneratedReaders___Internal.Read___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateCocaine_1327282946(base.LocalConnection, name, id, type, properties, appearance);
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x0010D4AC File Offset: 0x0010B6AC
		private void RpcWriter___Observers_CreateCocaine_1327282946(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
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
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendObserversRpc(16U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x0010D598 File Offset: 0x0010B798
		private void RpcReader___Observers_CreateCocaine_1327282946(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			CocaineAppearanceSettings appearance = GeneratedReaders___Internal.Read___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateCocaine_1327282946(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x0010D618 File Offset: 0x0010B818
		private void RpcWriter___Server_CreateMeth_Server_4251728555(string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendServerRpc(17U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x0010D6F3 File Offset: 0x0010B8F3
		public void RpcLogic___CreateMeth_Server_4251728555(string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			this.CreateMeth(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0010D704 File Offset: 0x0010B904
		private void RpcReader___Server_CreateMeth_Server_4251728555(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			MethAppearanceSettings appearance = GeneratedReaders___Internal.Read___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CreateMeth_Server_4251728555(name, id, type, properties, appearance);
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x0010D788 File Offset: 0x0010B988
		private void RpcWriter___Target_CreateMeth_1869045686(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
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
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendTargetRpc(18U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x0010D874 File Offset: 0x0010BA74
		private void RpcLogic___CreateMeth_1869045686(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			if (Registry.GetItem(id) != null)
			{
				Console.LogError("Product with ID " + id + " already exists", null);
				return;
			}
			MethDefinition methDefinition = UnityEngine.Object.Instantiate<MethDefinition>(this.DefaultMeth);
			methDefinition.name = name;
			methDefinition.Name = name;
			methDefinition.Description = string.Empty;
			methDefinition.ID = id;
			methDefinition.Initialize(Singleton<PropertyUtility>.Instance.GetProperties(properties), new List<EDrugType>
			{
				type
			}, appearance);
			this.AllProducts.Add(methDefinition);
			this.ProductPrices.Add(methDefinition, methDefinition.MarketValue);
			this.ProductNames.Add(name);
			this.createdProducts.Add(methDefinition);
			Singleton<Registry>.Instance.AddToRegistry(methDefinition);
			methDefinition.Icon = Singleton<ProductIconManager>.Instance.GenerateIcons(id);
			if (methDefinition.Icon == null)
			{
				Console.LogError("Failed to generate icons for " + name, null);
			}
			this.SetProductDiscovered(null, id, false);
			this.RefreshHighestValueProduct();
			if (this.onNewProductCreated != null)
			{
				this.onNewProductCreated(methDefinition);
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x0010D988 File Offset: 0x0010BB88
		private void RpcReader___Target_CreateMeth_1869045686(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			MethAppearanceSettings appearance = GeneratedReaders___Internal.Read___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateMeth_1869045686(base.LocalConnection, name, id, type, properties, appearance);
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x0010DA04 File Offset: 0x0010BC04
		private void RpcWriter___Observers_CreateMeth_1869045686(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
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
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendObserversRpc(19U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x0010DAF0 File Offset: 0x0010BCF0
		private void RpcReader___Observers_CreateMeth_1869045686(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			MethAppearanceSettings appearance = GeneratedReaders___Internal.Read___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateMeth_1869045686(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x0010DB70 File Offset: 0x0010BD70
		private void RpcWriter___Server_SendMixRecipe_852232071(string product, string mixer, string output)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(product);
			writer.WriteString(mixer);
			writer.WriteString(output);
			base.SendServerRpc(20U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x0010DC31 File Offset: 0x0010BE31
		public void RpcLogic___SendMixRecipe_852232071(string product, string mixer, string output)
		{
			this.CreateMixRecipe(null, product, mixer, output);
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x0010DC40 File Offset: 0x0010BE40
		private void RpcReader___Server_SendMixRecipe_852232071(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string product = PooledReader0.ReadString();
			string mixer = PooledReader0.ReadString();
			string output = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMixRecipe_852232071(product, mixer, output);
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x0010DCA0 File Offset: 0x0010BEA0
		private void RpcWriter___Target_CreateMixRecipe_1410895574(NetworkConnection conn, string product, string mixer, string output)
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
			writer.WriteString(product);
			writer.WriteString(mixer);
			writer.WriteString(output);
			base.SendTargetRpc(21U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x0010DD70 File Offset: 0x0010BF70
		public void RpcLogic___CreateMixRecipe_1410895574(NetworkConnection conn, string product, string mixer, string output)
		{
			if (string.IsNullOrEmpty(product) || string.IsNullOrEmpty(mixer) || string.IsNullOrEmpty(output))
			{
				Console.LogError(string.Concat(new string[]
				{
					"Invalid mix recipe: Product:",
					product,
					" Mixer:",
					mixer,
					" Output:",
					output
				}), null);
				return;
			}
			StationRecipe x = null;
			for (int i = 0; i < this.mixRecipes.Count; i++)
			{
				if (!(this.mixRecipes[i] == null) && this.mixRecipes[i].Product != null && this.mixRecipes[i].Ingredients.Count >= 2)
				{
					string id = this.mixRecipes[i].Ingredients[0].Items[0].ID;
					string id2 = this.mixRecipes[i].Ingredients[1].Items[0].ID;
					string id3 = this.mixRecipes[i].Product.Item.ID;
					if (id == product && id2 == mixer && id3 == output)
					{
						x = this.mixRecipes[i];
						break;
					}
					if (id2 == product && id == mixer && id3 == output)
					{
						x = this.mixRecipes[i];
						break;
					}
				}
			}
			if (x != null)
			{
				Console.LogWarning("Mix recipe already exists", null);
				return;
			}
			StationRecipe stationRecipe = ScriptableObject.CreateInstance<StationRecipe>();
			ItemDefinition item = Registry.GetItem(product);
			ItemDefinition item2 = Registry.GetItem(mixer);
			if (item == null)
			{
				Console.LogError("Product not found: " + product, null);
				return;
			}
			if (item2 == null)
			{
				Console.LogError("Mixer not found: " + mixer, null);
				return;
			}
			stationRecipe.Ingredients = new List<StationRecipe.IngredientQuantity>();
			stationRecipe.Ingredients.Add(new StationRecipe.IngredientQuantity
			{
				Items = new List<ItemDefinition>
				{
					item
				},
				Quantity = 1
			});
			stationRecipe.Ingredients.Add(new StationRecipe.IngredientQuantity
			{
				Items = new List<ItemDefinition>
				{
					item2
				},
				Quantity = 1
			});
			ItemDefinition item3 = Registry.GetItem(output);
			if (item3 == null)
			{
				Console.LogError("Output item not found: " + output, null);
				return;
			}
			stationRecipe.Product = new StationRecipe.ItemQuantity
			{
				Item = item3,
				Quantity = 1
			};
			stationRecipe.RecipeTitle = stationRecipe.Product.Item.Name;
			stationRecipe.Unlocked = true;
			this.mixRecipes.Add(stationRecipe);
			if (this.onMixRecipeAdded != null)
			{
				this.onMixRecipeAdded(stationRecipe);
			}
			ProductDefinition productDefinition = stationRecipe.Product.Item as ProductDefinition;
			if (productDefinition != null)
			{
				productDefinition.AddRecipe(stationRecipe);
			}
			else
			{
				Console.LogError("Product is not a product definition: " + product, null);
			}
			this.HasChanged = true;
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x0010E084 File Offset: 0x0010C284
		private void RpcReader___Target_CreateMixRecipe_1410895574(PooledReader PooledReader0, Channel channel)
		{
			string product = PooledReader0.ReadString();
			string mixer = PooledReader0.ReadString();
			string output = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateMixRecipe_1410895574(base.LocalConnection, product, mixer, output);
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x0010E0E0 File Offset: 0x0010C2E0
		private void RpcWriter___Observers_CreateMixRecipe_1410895574(NetworkConnection conn, string product, string mixer, string output)
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
			writer.WriteString(product);
			writer.WriteString(mixer);
			writer.WriteString(output);
			base.SendObserversRpc(22U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x0010E1B0 File Offset: 0x0010C3B0
		private void RpcReader___Observers_CreateMixRecipe_1410895574(PooledReader PooledReader0, Channel channel)
		{
			string product = PooledReader0.ReadString();
			string mixer = PooledReader0.ReadString();
			string output = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateMixRecipe_1410895574(null, product, mixer, output);
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x0010E210 File Offset: 0x0010C410
		private void RpcWriter___Target_GiveItem_2971853958(NetworkConnection conn, string id)
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
			writer.WriteString(id);
			base.SendTargetRpc(23U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x0010E2C5 File Offset: 0x0010C4C5
		private void RpcLogic___GiveItem_2971853958(NetworkConnection conn, string id)
		{
			PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(Registry.GetItem(id).GetDefaultInstance(1));
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x0010E2E0 File Offset: 0x0010C4E0
		private void RpcReader___Target_GiveItem_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___GiveItem_2971853958(base.LocalConnection, id);
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x0010E318 File Offset: 0x0010C518
		private void RpcWriter___Server_SendPrice_606697822(string productID, float value)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteSingle(value, 0);
			base.SendServerRpc(24U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x0010E3D1 File Offset: 0x0010C5D1
		public void RpcLogic___SendPrice_606697822(string productID, float value)
		{
			this.SetPrice(null, productID, value);
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x0010E3DC File Offset: 0x0010C5DC
		private void RpcReader___Server_SendPrice_606697822(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string productID = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPrice_606697822(productID, value);
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x0010E430 File Offset: 0x0010C630
		private void RpcWriter___Observers_SetPrice_4077118173(NetworkConnection conn, string productID, float value)
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
			writer.WriteString(productID);
			writer.WriteSingle(value, 0);
			base.SendObserversRpc(25U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x0010E4F8 File Offset: 0x0010C6F8
		public void RpcLogic___SetPrice_4077118173(NetworkConnection conn, string productID, float value)
		{
			ProductDefinition item = Registry.GetItem<ProductDefinition>(productID);
			if (item == null)
			{
				Console.LogError("Product not found: " + productID, null);
				return;
			}
			value = (float)Mathf.RoundToInt(Mathf.Clamp(value, 1f, 999f));
			if (!this.ProductPrices.ContainsKey(item))
			{
				this.ProductPrices.Add(item, value);
				return;
			}
			this.ProductPrices[item] = value;
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x0010E568 File Offset: 0x0010C768
		private void RpcReader___Observers_SetPrice_4077118173(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetPrice_4077118173(null, productID, value);
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0010E5BC File Offset: 0x0010C7BC
		private void RpcWriter___Target_SetPrice_4077118173(NetworkConnection conn, string productID, float value)
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
			writer.WriteString(productID);
			writer.WriteSingle(value, 0);
			base.SendTargetRpc(26U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x0010E684 File Offset: 0x0010C884
		private void RpcReader___Target_SetPrice_4077118173(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetPrice_4077118173(base.LocalConnection, productID, value);
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x0010E6D4 File Offset: 0x0010C8D4
		private void RpcWriter___Server_SendMixOperation_3670976965(NewMixOperation operation, bool complete)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generated(operation);
			writer.WriteBoolean(complete);
			base.SendServerRpc(27U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x0010E788 File Offset: 0x0010C988
		public void RpcLogic___SendMixOperation_3670976965(NewMixOperation operation, bool complete)
		{
			this.SetMixOperation(operation, complete);
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x0010E794 File Offset: 0x0010C994
		private void RpcReader___Server_SendMixOperation_3670976965(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NewMixOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generateds(PooledReader0);
			bool complete = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMixOperation_3670976965(operation, complete);
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x0010E7E4 File Offset: 0x0010C9E4
		private void RpcWriter___Observers_SetMixOperation_3670976965(NewMixOperation operation, bool complete)
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
			writer.Write___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generated(operation);
			writer.WriteBoolean(complete);
			base.SendObserversRpc(28U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x0010E8A7 File Offset: 0x0010CAA7
		private void RpcLogic___SetMixOperation_3670976965(NewMixOperation operation, bool complete)
		{
			this.CurrentMixOperation = operation;
			this.IsMixComplete = complete;
			if (this.CurrentMixOperation != null && this.IsMixComplete && this.onMixCompleted != null)
			{
				this.onMixCompleted(this.CurrentMixOperation);
			}
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x0010E8E0 File Offset: 0x0010CAE0
		private void RpcReader___Observers_SetMixOperation_3670976965(PooledReader PooledReader0, Channel channel)
		{
			NewMixOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generateds(PooledReader0);
			bool complete = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetMixOperation_3670976965(operation, complete);
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x0010E92C File Offset: 0x0010CB2C
		private void RpcWriter___Observers_FinishAndNameMix_4237212381(string productID, string ingredientID, string mixName, string mixID)
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
			writer.WriteString(productID);
			writer.WriteString(ingredientID);
			writer.WriteString(mixName);
			writer.WriteString(mixID);
			base.SendObserversRpc(29U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x0010EA0C File Offset: 0x0010CC0C
		private void RpcLogic___FinishAndNameMix_4237212381(string productID, string ingredientID, string mixName, string mixID)
		{
			if (this.AllProducts.Find((ProductDefinition p) => p.ID == mixID) != null)
			{
				return;
			}
			ProductDefinition productDefinition = Registry.GetItem(productID) as ProductDefinition;
			PropertyItemDefinition propertyItemDefinition = Registry.GetItem(ingredientID) as PropertyItemDefinition;
			if (productDefinition == null || propertyItemDefinition == null)
			{
				Debug.LogError("Product or mixer not found");
				return;
			}
			List<Property> list = PropertyMixCalculator.MixProperties(productDefinition.Properties, propertyItemDefinition.Properties[0], productDefinition.DrugType);
			List<string> list2 = new List<string>();
			foreach (Property property in list)
			{
				list2.Add(property.ID);
			}
			switch (productDefinition.DrugType)
			{
			case EDrugType.Marijuana:
				this.CreateWeed(null, mixName, mixID, EDrugType.Marijuana, list2, WeedDefinition.GetAppearanceSettings(list));
				return;
			case EDrugType.Methamphetamine:
				this.CreateMeth(null, mixName, mixID, EDrugType.Methamphetamine, list2, MethDefinition.GetAppearanceSettings(list));
				return;
			case EDrugType.Cocaine:
				this.CreateCocaine(null, mixName, mixID, EDrugType.Cocaine, list2, CocaineDefinition.GetAppearanceSettings(list));
				return;
			default:
				Console.LogError("Drug type not supported", null);
				return;
			}
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x0010EB5C File Offset: 0x0010CD5C
		private void RpcReader___Observers_FinishAndNameMix_4237212381(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			string ingredientID = PooledReader0.ReadString();
			string mixName = PooledReader0.ReadString();
			string mixID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___FinishAndNameMix_4237212381(productID, ingredientID, mixName, mixID);
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x0010EBCC File Offset: 0x0010CDCC
		private void RpcWriter___Server_SendFinishAndNameMix_4237212381(string productID, string ingredientID, string mixName, string mixID)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteString(ingredientID);
			writer.WriteString(mixName);
			writer.WriteString(mixID);
			base.SendServerRpc(30U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x0010EC9A File Offset: 0x0010CE9A
		private void RpcLogic___SendFinishAndNameMix_4237212381(string productID, string ingredientID, string mixName, string mixID)
		{
			this.FinishAndNameMix(productID, ingredientID, mixName, mixID);
			this.CreateMixRecipe(null, productID, ingredientID, mixID);
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x0010ECB4 File Offset: 0x0010CEB4
		private void RpcReader___Server_SendFinishAndNameMix_4237212381(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string productID = PooledReader0.ReadString();
			string ingredientID = PooledReader0.ReadString();
			string mixName = PooledReader0.ReadString();
			string mixID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendFinishAndNameMix_4237212381(productID, ingredientID, mixName, mixID);
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x0010ED18 File Offset: 0x0010CF18
		protected override void dll()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x04002D48 RID: 11592
		public const int MIN_PRICE = 1;

		// Token: 0x04002D49 RID: 11593
		public const int MAX_PRICE = 999;

		// Token: 0x04002D4A RID: 11594
		public Action<ProductDefinition> onProductDiscovered;

		// Token: 0x04002D4B RID: 11595
		public static List<ProductDefinition> DiscoveredProducts = new List<ProductDefinition>();

		// Token: 0x04002D4C RID: 11596
		public static List<ProductDefinition> ListedProducts = new List<ProductDefinition>();

		// Token: 0x04002D4D RID: 11597
		public static List<ProductDefinition> FavouritedProducts = new List<ProductDefinition>();

		// Token: 0x04002D4F RID: 11599
		public List<ProductDefinition> AllProducts = new List<ProductDefinition>();

		// Token: 0x04002D50 RID: 11600
		public List<ProductDefinition> DefaultKnownProducts = new List<ProductDefinition>();

		// Token: 0x04002D51 RID: 11601
		public List<PropertyItemDefinition> ValidMixIngredients = new List<PropertyItemDefinition>();

		// Token: 0x04002D52 RID: 11602
		public AnimationCurve SampleSuccessCurve;

		// Token: 0x04002D53 RID: 11603
		[Header("Default Products")]
		public WeedDefinition DefaultWeed;

		// Token: 0x04002D54 RID: 11604
		public CocaineDefinition DefaultCocaine;

		// Token: 0x04002D55 RID: 11605
		public MethDefinition DefaultMeth;

		// Token: 0x04002D56 RID: 11606
		[Header("Mix Maps")]
		public MixerMap WeedMixMap;

		// Token: 0x04002D57 RID: 11607
		public MixerMap MethMixMap;

		// Token: 0x04002D58 RID: 11608
		public MixerMap CokeMixMap;

		// Token: 0x04002D59 RID: 11609
		private List<ProductDefinition> createdProducts = new List<ProductDefinition>();

		// Token: 0x04002D5D RID: 11613
		public Action<NewMixOperation> onMixCompleted;

		// Token: 0x04002D5E RID: 11614
		public Action<ProductDefinition> onNewProductCreated;

		// Token: 0x04002D5F RID: 11615
		public Action<ProductDefinition> onProductListed;

		// Token: 0x04002D60 RID: 11616
		public Action<ProductDefinition> onProductDelisted;

		// Token: 0x04002D61 RID: 11617
		public Action<ProductDefinition> onProductFavourited;

		// Token: 0x04002D62 RID: 11618
		public Action<ProductDefinition> onProductUnfavourited;

		// Token: 0x04002D63 RID: 11619
		public UnityEvent onFirstSampleRejection;

		// Token: 0x04002D64 RID: 11620
		public UnityEvent onSecondUniqueProductCreated;

		// Token: 0x04002D65 RID: 11621
		public List<string> ProductNames = new List<string>();

		// Token: 0x04002D66 RID: 11622
		private List<StationRecipe> mixRecipes = new List<StationRecipe>();

		// Token: 0x04002D67 RID: 11623
		public Action<StationRecipe> onMixRecipeAdded;

		// Token: 0x04002D68 RID: 11624
		private Dictionary<ProductDefinition, float> ProductPrices = new Dictionary<ProductDefinition, float>();

		// Token: 0x04002D69 RID: 11625
		private ProductDefinition highestValueProduct;

		// Token: 0x04002D6A RID: 11626
		private ProductManagerLoader loader = new ProductManagerLoader();

		// Token: 0x04002D6E RID: 11630
		private bool dll_Excuted;

		// Token: 0x04002D6F RID: 11631
		private bool dll_Excuted;
	}
}
