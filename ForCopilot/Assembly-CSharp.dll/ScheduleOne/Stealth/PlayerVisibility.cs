using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.NPCs;
using ScheduleOne.Vehicles;
using ScheduleOne.Vision;
using UnityEngine;

namespace ScheduleOne.Stealth
{
	// Token: 0x020002D3 RID: 723
	public class PlayerVisibility : NetworkBehaviour
	{
		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x00043C0B File Offset: 0x00041E0B
		// (set) Token: 0x06000F75 RID: 3957 RVA: 0x00043C13 File Offset: 0x00041E13
		public VisionEvent HighestVisionEvent { get; set; }

		// Token: 0x06000F76 RID: 3958 RVA: 0x00043C1C File Offset: 0x00041E1C
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (base.IsOwner)
			{
				this.environmentalVisibility = new VisibilityAttribute("Environmental Brightess", 0f, 1f, -1);
			}
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00043C47 File Offset: 0x00041E47
		private void FixedUpdate()
		{
			this.UpdateEnvironmentalVisibilityAttribute();
			this.CurrentVisibility = this.CalculateVisibility();
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00043C5C File Offset: 0x00041E5C
		private float CalculateVisibility()
		{
			float num = 0f;
			Dictionary<string, float> maxPointsChangesByUniquenessCode = (from UniqueVisibilityAttribute uva in 
				from a in this.activeAttributes
				where a is UniqueVisibilityAttribute
				select a
			group uva by uva.uniquenessCode).ToDictionary((IGrouping<string, UniqueVisibilityAttribute> group) => group.Key, (IGrouping<string, UniqueVisibilityAttribute> group) => group.Max((UniqueVisibilityAttribute uva) => uva.pointsChange));
			this.filteredAttributes = this.activeAttributes.Where(delegate(VisibilityAttribute attr)
			{
				if (attr is UniqueVisibilityAttribute)
				{
					UniqueVisibilityAttribute uniqueVisibilityAttribute = attr as UniqueVisibilityAttribute;
					return uniqueVisibilityAttribute != null && uniqueVisibilityAttribute.pointsChange >= CollectionExtensions.GetValueOrDefault<string, float>(maxPointsChangesByUniquenessCode, uniqueVisibilityAttribute.uniquenessCode, 0f);
				}
				return true;
			}).ToList<VisibilityAttribute>();
			for (int i = 0; i < this.filteredAttributes.Count; i++)
			{
				num += this.filteredAttributes[i].pointsChange;
				if (this.filteredAttributes[i].multiplier != 1f)
				{
					num *= this.filteredAttributes[i].multiplier;
				}
			}
			return Mathf.Clamp(num, 0f, 100f);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00043D9C File Offset: 0x00041F9C
		public VisibilityAttribute GetAttribute(string name)
		{
			return this.activeAttributes.Find((VisibilityAttribute x) => x.name.ToLower() == name.ToLower());
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00043DCD File Offset: 0x00041FCD
		private void UpdateEnvironmentalVisibilityAttribute()
		{
			if (this.environmentalVisibility == null)
			{
				return;
			}
			this.environmentalVisibility.multiplier = Singleton<EnvironmentFX>.Instance.normalizedEnvironmentalBrightness;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00043DF0 File Offset: 0x00041FF0
		public float CalculateExposureToPoint(Vector3 point, float checkRange = 50f, NPC checkingNPC = null)
		{
			float num = 0f;
			if (Vector3.Distance(point, base.transform.position) > checkRange + 1f)
			{
				return 0f;
			}
			List<VisionObscurer> list = new List<VisionObscurer>();
			foreach (Transform transform in this.visibilityPoints)
			{
				float num2 = Vector3.Distance(point, transform.position);
				if (num2 <= checkRange)
				{
					this.hits = Physics.RaycastAll(point, (transform.position - point).normalized, Mathf.Min(checkRange, num2), this.visibilityCheckMask, 2).ToList<RaycastHit>();
					for (int i = 0; i < this.hits.Count; i++)
					{
						LandVehicle componentInParent = this.hits[i].collider.GetComponentInParent<LandVehicle>();
						if (checkingNPC != null && componentInParent != null)
						{
							if (checkingNPC.CurrentVehicle == componentInParent)
							{
								this.hits.RemoveAt(i);
								i--;
							}
						}
						else
						{
							VisionObscurer componentInParent2 = this.hits[i].collider.GetComponentInParent<VisionObscurer>();
							if (componentInParent2 != null)
							{
								if (transform == this.visibilityPoints[1] && !list.Contains(componentInParent2))
								{
									list.Add(componentInParent2);
								}
								this.hits.RemoveAt(i);
								i--;
							}
							else if (this.hits[i].collider.isTrigger)
							{
								this.hits.RemoveAt(i);
								i--;
							}
						}
					}
					if (this.hits.Count > 0)
					{
						Debug.DrawRay(point, this.hits[0].point - point, Color.red, 0.1f);
					}
					else
					{
						Debug.DrawRay(point, (transform.position - point).normalized * num2, Color.green, 0.1f);
						num += 1f / (float)this.visibilityPoints.Count;
					}
				}
			}
			float num3 = 1f;
			for (int j = 0; j < list.Count; j++)
			{
				num3 *= 1f - list[j].ObscuranceAmount;
			}
			return num * num3;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x000440BD File Offset: 0x000422BD
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Stealth.PlayerVisibilityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Stealth.PlayerVisibilityAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x000440D0 File Offset: 0x000422D0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Stealth.PlayerVisibilityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Stealth.PlayerVisibilityAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x000440E3 File Offset: 0x000422E3
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x000440E3 File Offset: 0x000422E3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04000FAD RID: 4013
		public const float MAX_VISIBLITY = 100f;

		// Token: 0x04000FAE RID: 4014
		public float CurrentVisibility;

		// Token: 0x04000FAF RID: 4015
		public List<VisibilityAttribute> activeAttributes = new List<VisibilityAttribute>();

		// Token: 0x04000FB0 RID: 4016
		public List<VisibilityAttribute> filteredAttributes = new List<VisibilityAttribute>();

		// Token: 0x04000FB1 RID: 4017
		[Header("Settings")]
		public LayerMask visibilityCheckMask;

		// Token: 0x04000FB2 RID: 4018
		[Header("References")]
		public List<Transform> visibilityPoints = new List<Transform>();

		// Token: 0x04000FB3 RID: 4019
		private VisibilityAttribute environmentalVisibility;

		// Token: 0x04000FB5 RID: 4021
		private List<RaycastHit> hits;

		// Token: 0x04000FB6 RID: 4022
		private bool dll_Excuted;

		// Token: 0x04000FB7 RID: 4023
		private bool dll_Excuted;
	}
}
