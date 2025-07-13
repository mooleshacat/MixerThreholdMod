using System;
using ScheduleOne.PlayerTasks;

namespace ScheduleOne.StationFramework
{
	// Token: 0x02000902 RID: 2306
	public class IngredientModule : ItemModule
	{
		// Token: 0x06003E76 RID: 15990 RVA: 0x00107024 File Offset: 0x00105224
		public override void ActivateModule(StationItem item)
		{
			base.ActivateModule(item);
			for (int i = 0; i < this.Pieces.Length; i++)
			{
				this.Pieces[i].GetComponent<DraggableConstraint>().SetContainer(item.transform.parent);
			}
		}

		// Token: 0x04002C88 RID: 11400
		public IngredientPiece[] Pieces;
	}
}
