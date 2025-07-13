using System;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000C64 RID: 3172
	public class ToggleableLight : MonoBehaviour
	{
		// Token: 0x06005951 RID: 22865 RVA: 0x001797A4 File Offset: 0x001779A4
		protected virtual void Awake()
		{
			this.constructable = base.GetComponentInParent<Constructable_GridBased>();
			this.SetLights(this.isOn);
		}

		// Token: 0x06005952 RID: 22866 RVA: 0x001797BE File Offset: 0x001779BE
		private void OnValidate()
		{
			if (this.isOn != this.lightsApplied)
			{
				this.SetLights(this.isOn);
			}
		}

		// Token: 0x06005953 RID: 22867 RVA: 0x001797BE File Offset: 0x001779BE
		protected virtual void Update()
		{
			if (this.isOn != this.lightsApplied)
			{
				this.SetLights(this.isOn);
			}
		}

		// Token: 0x06005954 RID: 22868 RVA: 0x001797DA File Offset: 0x001779DA
		public void TurnOn()
		{
			this.isOn = true;
			this.Update();
		}

		// Token: 0x06005955 RID: 22869 RVA: 0x001797E9 File Offset: 0x001779E9
		public void TurnOff()
		{
			this.isOn = false;
			this.Update();
		}

		// Token: 0x06005956 RID: 22870 RVA: 0x001797F8 File Offset: 0x001779F8
		protected virtual void SetLights(bool active)
		{
			this.lightsApplied = this.isOn;
			foreach (OptimizedLight optimizedLight in this.lightSources)
			{
				if (!(optimizedLight == null))
				{
					optimizedLight.Enabled = active;
				}
			}
			Material material = active ? this.lightOnMat : this.lightOffMat;
			foreach (MeshRenderer meshRenderer in this.lightSurfacesMeshes)
			{
				if (!(meshRenderer == null))
				{
					Material[] sharedMaterials = meshRenderer.sharedMaterials;
					sharedMaterials[this.MaterialIndex] = material;
					meshRenderer.materials = sharedMaterials;
				}
			}
		}

		// Token: 0x04004173 RID: 16755
		public bool isOn;

		// Token: 0x04004174 RID: 16756
		[Header("References")]
		[SerializeField]
		protected OptimizedLight[] lightSources;

		// Token: 0x04004175 RID: 16757
		[SerializeField]
		protected MeshRenderer[] lightSurfacesMeshes;

		// Token: 0x04004176 RID: 16758
		public int MaterialIndex;

		// Token: 0x04004177 RID: 16759
		[Header("Materials")]
		[SerializeField]
		protected Material lightOnMat;

		// Token: 0x04004178 RID: 16760
		[SerializeField]
		protected Material lightOffMat;

		// Token: 0x04004179 RID: 16761
		private Constructable_GridBased constructable;

		// Token: 0x0400417A RID: 16762
		private bool lightsApplied;
	}
}
