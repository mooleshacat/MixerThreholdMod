using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x0200090C RID: 2316
	public class StationItem : MonoBehaviour
	{
		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06003EB5 RID: 16053 RVA: 0x00107D8D File Offset: 0x00105F8D
		// (set) Token: 0x06003EB6 RID: 16054 RVA: 0x00107D95 File Offset: 0x00105F95
		public List<ItemModule> ActiveModules { get; protected set; } = new List<ItemModule>();

		// Token: 0x06003EB7 RID: 16055 RVA: 0x000524CD File Offset: 0x000506CD
		protected virtual void Awake()
		{
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Task"));
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Initialize(StorableItemDefinition itemDefinition)
		{
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x00107DA0 File Offset: 0x00105FA0
		public void ActivateModule<T>() where T : ItemModule
		{
			ItemModule itemModule = this.GetModule<T>();
			if (itemModule == null)
			{
				Console.LogWarning(itemModule.GetType().Name + " is not a valid module for " + base.name, null);
				return;
			}
			this.ActiveModules.Add(itemModule);
			itemModule.ActivateModule(this);
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x001007D2 File Offset: 0x000FE9D2
		public void Destroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x00107DF7 File Offset: 0x00105FF7
		public bool HasModule<T>() where T : ItemModule
		{
			return this.Modules.Exists((ItemModule x) => x.GetType() == typeof(T));
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x00107E23 File Offset: 0x00106023
		public T GetModule<T>() where T : ItemModule
		{
			return (T)((object)this.Modules.Find((ItemModule x) => x.GetType() == typeof(T)));
		}

		// Token: 0x04002CC7 RID: 11463
		public List<ItemModule> Modules;

		// Token: 0x04002CC8 RID: 11464
		public TrashItem TrashPrefab;
	}
}
