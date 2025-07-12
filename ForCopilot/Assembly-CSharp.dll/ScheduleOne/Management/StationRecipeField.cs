using System;
using System.Collections.Generic;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.StationFramework;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005B4 RID: 1460
	public class StationRecipeField : ConfigField
	{
		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x0009421C File Offset: 0x0009241C
		// (set) Token: 0x060023EE RID: 9198 RVA: 0x00094224 File Offset: 0x00092424
		public StationRecipe SelectedRecipe { get; protected set; }

		// Token: 0x060023EF RID: 9199 RVA: 0x0009422D File Offset: 0x0009242D
		public StationRecipeField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x0009424C File Offset: 0x0009244C
		public void SetRecipe(StationRecipe recipe, bool network)
		{
			this.SelectedRecipe = recipe;
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onRecipeChanged != null)
			{
				this.onRecipeChanged.Invoke(this.SelectedRecipe);
			}
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x0009427E File Offset: 0x0009247E
		public override bool IsValueDefault()
		{
			return this.SelectedRecipe == null;
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x0009428C File Offset: 0x0009248C
		public StationRecipeFieldData GetData()
		{
			return new StationRecipeFieldData((this.SelectedRecipe != null) ? this.SelectedRecipe.RecipeID.ToString() : "");
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000942B8 File Offset: 0x000924B8
		public void Load(StationRecipeFieldData data)
		{
			if (data != null && !string.IsNullOrEmpty(data.RecipeID))
			{
				this.SelectedRecipe = this.Options.Find((StationRecipe x) => x.RecipeID == data.RecipeID);
			}
		}

		// Token: 0x04001ACE RID: 6862
		public List<StationRecipe> Options = new List<StationRecipe>();

		// Token: 0x04001ACF RID: 6863
		public UnityEvent<StationRecipe> onRecipeChanged = new UnityEvent<StationRecipe>();
	}
}
