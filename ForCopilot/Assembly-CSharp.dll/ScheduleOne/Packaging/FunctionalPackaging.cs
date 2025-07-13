using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Audio;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x020008D9 RID: 2265
	public class FunctionalPackaging : Draggable
	{
		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06003CE3 RID: 15587 RVA: 0x00100674 File Offset: 0x000FE874
		// (set) Token: 0x06003CE4 RID: 15588 RVA: 0x0010067C File Offset: 0x000FE87C
		public bool IsSealed { get; protected set; }

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x00100685 File Offset: 0x000FE885
		// (set) Token: 0x06003CE6 RID: 15590 RVA: 0x0010068D File Offset: 0x000FE88D
		public bool IsFull { get; protected set; }

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06003CE7 RID: 15591 RVA: 0x00100696 File Offset: 0x000FE896
		// (set) Token: 0x06003CE8 RID: 15592 RVA: 0x0010069E File Offset: 0x000FE89E
		public bool ReachedOutput { get; protected set; }

		// Token: 0x06003CE9 RID: 15593 RVA: 0x001006A8 File Offset: 0x000FE8A8
		public virtual void Initialize(PackagingStation _station, Transform alignment, bool align = true)
		{
			this.station = _station;
			if (align)
			{
				this.AlignTo(alignment);
			}
			this.ClickableEnabled = false;
			base.Rb.isKinematic = true;
			if (this.VelocityCalculator == null)
			{
				this.VelocityCalculator = base.gameObject.AddComponent<SmoothedVelocityCalculator>();
				this.VelocityCalculator.MaxReasonableVelocity = 2f;
			}
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x00100708 File Offset: 0x000FE908
		public void AlignTo(Transform alignment)
		{
			base.transform.rotation = alignment.rotation * (Quaternion.Inverse(this.AlignmentPoint.rotation) * base.transform.rotation);
			Vector3 b = base.transform.position - this.AlignmentPoint.position;
			base.transform.position = alignment.position + b;
			if (base.Rb == null)
			{
				base.Rb = base.GetComponent<Rigidbody>();
			}
			if (base.Rb != null)
			{
				base.Rb.position = base.transform.position;
				base.Rb.rotation = base.transform.rotation;
			}
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x001007D2 File Offset: 0x000FE9D2
		public virtual void Destroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x001007E0 File Offset: 0x000FE9E0
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.IsFull)
			{
				return;
			}
			foreach (FunctionalProduct functionalProduct in this.productContactTime.Keys.ToList<FunctionalProduct>())
			{
				if (!(functionalProduct.Rb == null) && this.productContactTime[functionalProduct] > this.ProductContactTime && !this.PackedProducts.Contains(functionalProduct) && !functionalProduct.IsHeld)
				{
					this.PackProduct(functionalProduct);
				}
			}
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x00100884 File Offset: 0x000FEA84
		protected virtual void PackProduct(FunctionalProduct product)
		{
			product.ClickableEnabled = false;
			product.ClampZ = false;
			UnityEngine.Object.Destroy(product.Rb);
			product.transform.SetParent(base.transform);
			if (this.ProductAlignmentPoints.Length > this.PackedProducts.Count)
			{
				product.transform.position = this.ProductAlignmentPoints[this.PackedProducts.Count].position;
				product.transform.rotation = this.ProductAlignmentPoints[this.PackedProducts.Count].rotation;
			}
			this.PackedProducts.Add(product);
			if (this.PackedProducts.Count >= this.Definition.Quantity && !this.IsFull)
			{
				this.FullyPacked();
			}
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x00100948 File Offset: 0x000FEB48
		protected virtual void FullyPacked()
		{
			this.IsFull = true;
			if (this.onFullyPacked != null)
			{
				this.onFullyPacked();
			}
			foreach (FunctionalProduct functionalProduct in this.PackedProducts)
			{
				UnityEngine.Object.Destroy(functionalProduct.Rb);
			}
			if (this.AutoEnableSealing)
			{
				this.EnableSealing();
			}
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x001009C8 File Offset: 0x000FEBC8
		protected virtual void OnTriggerStay(Collider other)
		{
			if (this.station == null)
			{
				return;
			}
			FunctionalProduct componentInParent = other.GetComponentInParent<FunctionalProduct>();
			if (componentInParent != null && componentInParent.IsHeld)
			{
				return;
			}
			if (componentInParent != null)
			{
				if (!this.productContactTime.ContainsKey(componentInParent))
				{
					this.productContactTime.Add(componentInParent, 0f);
				}
				Vector3 velocity = componentInParent.VelocityCalculator.Velocity;
				Vector3 velocity2 = this.VelocityCalculator.Velocity;
				Vector3 vector = velocity - velocity2;
				Debug.DrawRay(componentInParent.transform.position, velocity, Color.red);
				Debug.DrawRay(base.transform.position, velocity2, Color.blue);
				if (vector.magnitude < this.ProductContactMaxVelocity)
				{
					Dictionary<FunctionalProduct, float> dictionary = this.productContactTime;
					FunctionalProduct key = componentInParent;
					dictionary[key] += Time.fixedDeltaTime;
				}
			}
			if (other.gameObject.name == this.station.OutputCollider.name && !this.ReachedOutput && this.IsSealed && !base.IsHeld)
			{
				this.ReachedOutput = true;
				if (this.onReachOutput != null)
				{
					this.onReachOutput();
				}
			}
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x00100AFA File Offset: 0x000FECFA
		protected virtual void EnableSealing()
		{
			this.ClickableEnabled = true;
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x00100B04 File Offset: 0x000FED04
		public virtual void Seal()
		{
			this.IsSealed = true;
			foreach (FunctionalProduct functionalProduct in this.PackedProducts)
			{
				Collider[] componentsInChildren = functionalProduct.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
			}
			if (this.SealSound != null)
			{
				this.SealSound.Play();
			}
			this.HoveredCursor = CursorManager.ECursorType.OpenHand;
			this.ClickableEnabled = true;
			base.Rb.isKinematic = false;
			if (this.onSealed != null)
			{
				this.onSealed();
			}
		}

		// Token: 0x04002BC7 RID: 11207
		[Header("Settings")]
		public string SealInstruction = "Seal packaging";

		// Token: 0x04002BC8 RID: 11208
		public bool AutoEnableSealing = true;

		// Token: 0x04002BC9 RID: 11209
		public float ProductContactTime = 0.1f;

		// Token: 0x04002BCA RID: 11210
		public float ProductContactMaxVelocity = 0.3f;

		// Token: 0x04002BCB RID: 11211
		[Header("References")]
		public PackagingDefinition Definition;

		// Token: 0x04002BCC RID: 11212
		public Transform AlignmentPoint;

		// Token: 0x04002BCD RID: 11213
		public Transform[] ProductAlignmentPoints;

		// Token: 0x04002BCE RID: 11214
		public AudioSourceController SealSound;

		// Token: 0x04002BCF RID: 11215
		protected List<FunctionalProduct> PackedProducts = new List<FunctionalProduct>();

		// Token: 0x04002BD0 RID: 11216
		public Action onFullyPacked;

		// Token: 0x04002BD1 RID: 11217
		public Action onSealed;

		// Token: 0x04002BD2 RID: 11218
		public Action onReachOutput;

		// Token: 0x04002BD3 RID: 11219
		private PackagingStation station;

		// Token: 0x04002BD4 RID: 11220
		private Dictionary<FunctionalProduct, float> productContactTime = new Dictionary<FunctionalProduct, float>();

		// Token: 0x04002BD5 RID: 11221
		private SmoothedVelocityCalculator VelocityCalculator;
	}
}
