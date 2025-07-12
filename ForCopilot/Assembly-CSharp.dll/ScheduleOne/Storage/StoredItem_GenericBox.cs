using System;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008F4 RID: 2292
	public class StoredItem_GenericBox : StoredItem
	{
		// Token: 0x06003E24 RID: 15908 RVA: 0x001062F4 File Offset: 0x001044F4
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			this.icon1.sprite = _item.Icon;
			this.icon2.sprite = _item.Icon;
			float num = 0.025f / (_item.Icon.rect.width / 1024f) * this.IconScale;
			this.icon1.transform.localScale = new Vector3(num, num, 1f);
			this.icon2.transform.localScale = new Vector3(num, num, 1f);
		}

		// Token: 0x04002C47 RID: 11335
		private const float ReferenceIconWidth = 1024f;

		// Token: 0x04002C48 RID: 11336
		[Header("References")]
		[SerializeField]
		protected SpriteRenderer icon1;

		// Token: 0x04002C49 RID: 11337
		[SerializeField]
		protected SpriteRenderer icon2;

		// Token: 0x04002C4A RID: 11338
		[Header("Settings")]
		public float IconScale = 0.5f;
	}
}
