using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x020008C7 RID: 2247
	public class FilledPackaging_StoredItem : StoredItem
	{
		// Token: 0x06003C97 RID: 15511 RVA: 0x000FF2F4 File Offset: 0x000FD4F4
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			(base.item as ProductItemInstance).SetupPackagingVisuals(this.Visuals);
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x000FF318 File Offset: 0x000FD518
		public override GameObject CreateGhostModel(ItemInstance _item, Transform parent)
		{
			GameObject gameObject = base.CreateGhostModel(_item, parent);
			(_item as ProductItemInstance).SetupPackagingVisuals(gameObject.GetComponent<FilledPackaging_StoredItem>().Visuals);
			return gameObject;
		}

		// Token: 0x04002B65 RID: 11109
		public FilledPackagingVisuals Visuals;
	}
}
