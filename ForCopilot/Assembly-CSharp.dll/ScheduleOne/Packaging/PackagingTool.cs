using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using TMPro;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x020008CE RID: 2254
	public class PackagingTool : MonoBehaviour
	{
		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06003CBA RID: 15546 RVA: 0x000FF7B1 File Offset: 0x000FD9B1
		// (set) Token: 0x06003CBB RID: 15547 RVA: 0x000FF7B9 File Offset: 0x000FD9B9
		public bool ReceiveInput { get; private set; }

		// Token: 0x06003CBC RID: 15548 RVA: 0x000FF7C4 File Offset: 0x000FD9C4
		public void Initialize(Task _task, FunctionalPackaging packaging, int packagingQuantity, ProductItemInstance product, int productQuantity)
		{
			this.task = _task;
			this.ReceiveInput = true;
			this.LeftButton.ClickableEnabled = true;
			this.RightButton.ClickableEnabled = true;
			this.DropButton.ClickableEnabled = true;
			this.LoadPackaging(packaging, packagingQuantity);
			this.LoadProduct(product, productQuantity);
			int num = Mathf.RoundToInt(180f / this.DeployAngle);
			for (int i = 0; i < num; i++)
			{
				this.CheckDeployPackaging();
				this.Rotate(this.DeployAngle);
			}
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x000FF848 File Offset: 0x000FDA48
		public void Deinitialize()
		{
			this.ReceiveInput = false;
			if (this.LeftButton.IsHeld)
			{
				this.task.ForceEndClick(this.LeftButton);
			}
			if (this.RightButton.IsHeld)
			{
				this.task.ForceEndClick(this.RightButton);
			}
			if (this.DropButton.IsHeld)
			{
				this.task.ForceEndClick(this.DropButton);
			}
			this.LeftButton.ClickableEnabled = false;
			this.RightButton.ClickableEnabled = false;
			this.DropButton.ClickableEnabled = false;
			for (int i = 0; i < this.ProductInstances.Count; i++)
			{
				UnityEngine.Object.Destroy(this.ProductInstances[i].gameObject);
			}
			this.ProductInstances.Clear();
			for (int j = 0; j < this.PackagingInstances.Count; j++)
			{
				UnityEngine.Object.Destroy(this.PackagingInstances[j].Container.gameObject);
			}
			this.PackagingInstances.Clear();
			for (int k = 0; k < this.FinalizedPackaging.Count; k++)
			{
				UnityEngine.Object.Destroy(this.FinalizedPackaging[k].gameObject);
			}
			this.FinalizedPackaging.Clear();
			if (this.finalizeCoroutine != null)
			{
				base.StopCoroutine(this.finalizeCoroutine);
				this.finalizeCoroutine = null;
			}
			this.UnloadPackaging();
			this.UnloadProduct();
			this.task = null;
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x000FF9B2 File Offset: 0x000FDBB2
		private void LoadPackaging(FunctionalPackaging prefab, int quantity)
		{
			this.PackagingPrefab = prefab;
			this.ConcealedPackaging = quantity;
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x000FF9C2 File Offset: 0x000FDBC2
		private void UnloadPackaging()
		{
			this.PackagingPrefab = null;
			this.ConcealedPackaging = 0;
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x000FF9D2 File Offset: 0x000FDBD2
		private void LoadProduct(ProductItemInstance product, int quantity)
		{
			this.ProductItem = product;
			this.ProductPrefab = (product.Definition as ProductDefinition).FunctionalProduct;
			this.ProductInHopper = quantity;
			this.UpdateScreen();
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x000FF9FE File Offset: 0x000FDBFE
		private void UnloadProduct()
		{
			this.ProductPrefab = null;
			this.ProductInHopper = 0;
			this.UpdateScreen();
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x000FFA14 File Offset: 0x000FDC14
		public void Update()
		{
			this.timeSinceLastDrop += Time.deltaTime;
			this.UpdateInput();
			this.UpdateConveyor();
			if (this.ConcealedPackaging > 0)
			{
				this.CheckDeployPackaging();
			}
			if (this.DropButton.IsHeld && this.ProductInHopper > 0 && this.timeSinceLastDrop > this.DropCooldown)
			{
				this.DropProduct();
			}
			if (Mathf.Abs(this.conveyorVelocity) > 0f && !this.MotorSound.isPlaying)
			{
				this.MotorSound.Play();
			}
			this.MotorSound.VolumeMultiplier = Mathf.Abs(this.conveyorVelocity);
			this.MotorSound.PitchMultiplier = Mathf.Lerp(0.7f, 1f, Mathf.Abs(this.conveyorVelocity));
			if (this.MotorSound.VolumeMultiplier <= 0f)
			{
				this.MotorSound.Stop();
			}
			else if (this.MotorSound.VolumeMultiplier > 0f && !this.MotorSound.isPlaying)
			{
				this.MotorSound.Play();
			}
			this.CheckFinalize();
			this.CheckInsertions();
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x000FFB34 File Offset: 0x000FDD34
		private void UpdateInput()
		{
			this.directionInput = 0;
			if (!this.ReceiveInput)
			{
				return;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.Left))
			{
				if (!this.LeftButton.IsHeld)
				{
					this.leftDown = true;
					this.task.ForceStartClick(this.LeftButton);
				}
			}
			else if (this.leftDown)
			{
				this.leftDown = false;
				this.task.ForceEndClick(this.LeftButton);
			}
			if (GameInput.GetButton(GameInput.ButtonCode.Right))
			{
				if (!this.RightButton.IsHeld)
				{
					this.rightDown = true;
					this.task.ForceStartClick(this.RightButton);
				}
			}
			else if (this.rightDown)
			{
				this.rightDown = false;
				this.task.ForceEndClick(this.RightButton);
			}
			if (GameInput.GetButton(GameInput.ButtonCode.Jump))
			{
				if (!this.DropButton.IsHeld)
				{
					this.dropDown = true;
					this.task.ForceStartClick(this.DropButton);
				}
			}
			else if (this.dropDown)
			{
				this.dropDown = false;
				this.task.ForceEndClick(this.DropButton);
			}
			if (this.LeftButton.IsHeld)
			{
				this.directionInput--;
			}
			if (this.RightButton.IsHeld)
			{
				this.directionInput++;
			}
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x000FFC74 File Offset: 0x000FDE74
		private void UpdateScreen()
		{
			this.ProductCountText.text = this.ProductInHopper.ToString();
			this.ProductCountText.gameObject.SetActive(this.ProductInHopper > 0);
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x000FFCA8 File Offset: 0x000FDEA8
		private void UpdateConveyor()
		{
			float num = Mathf.MoveTowards(this.conveyorVelocity, (float)this.directionInput, this.ConveyorAcceleration * Time.deltaTime);
			this.conveyorVelocity = num;
			this.Rotate(this.conveyorVelocity * this.ConveyorSpeed * Time.deltaTime);
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x000FFCF4 File Offset: 0x000FDEF4
		private void Rotate(float angle)
		{
			this.ConveyorModel.Rotate(Vector3.forward, -angle);
			for (int i = 0; i < this.PackagingInstances.Count; i++)
			{
				this.PackagingInstances[i].ChangePosition(angle);
			}
			this.PackagingInstances.Sort((PackagingTool.PackagingInstance a, PackagingTool.PackagingInstance b) => a.AnglePosition.CompareTo(b.AnglePosition));
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x000FFD68 File Offset: 0x000FDF68
		private void CheckDeployPackaging()
		{
			if (this.PackagingInstances.Count > 0 && (this.PackagingInstances[0].AnglePosition < this.DeployAngle || this.PackagingInstances[this.PackagingInstances.Count - 1].AnglePosition > 360f - this.DeployAngle))
			{
				return;
			}
			this.DeployPackaging();
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x000FFDD0 File Offset: 0x000FDFD0
		private void CheckFinalize()
		{
			if (this.finalizeCoroutine != null)
			{
				return;
			}
			for (int i = 0; i < this.PackagingInstances.Count; i++)
			{
				if (this.PackagingInstances[i].Packaging.IsFull && this.PackagingInstances[i].AnglePosition > 255f && this.PackagingInstances[i].AnglePosition < 270f)
				{
					this.Finalize(this.PackagingInstances[i]);
					return;
				}
			}
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x000FFE58 File Offset: 0x000FE058
		private void Finalize(PackagingTool.PackagingInstance instance)
		{
			PackagingTool.<>c__DisplayClass66_0 CS$<>8__locals1 = new PackagingTool.<>c__DisplayClass66_0();
			CS$<>8__locals1.instance = instance;
			CS$<>8__locals1.<>4__this = this;
			this.finalizeInstance = CS$<>8__locals1.instance;
			this.finalizeCoroutine = base.StartCoroutine(CS$<>8__locals1.<Finalize>g__FinalizeRoutine|0());
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x000FFE98 File Offset: 0x000FE098
		private void DropProduct()
		{
			if (this.ProductInHopper <= 0)
			{
				return;
			}
			this.timeSinceLastDrop = 0f;
			this.ProductInHopper--;
			this.UpdateScreen();
			this.DropSound.Play();
			FunctionalProduct functionalProduct = UnityEngine.Object.Instantiate<FunctionalProduct>(this.ProductPrefab, this.HopperDropPoint.position, this.HopperDropPoint.rotation);
			functionalProduct.Initialize(this.ProductItem);
			functionalProduct.transform.SetParent(this.ProductContainer);
			functionalProduct.ClampZ = true;
			functionalProduct.DragProjectionMode = Draggable.EDragProjectionMode.FlatCameraForward;
			functionalProduct.Rb.collisionDetectionMode = 2;
			functionalProduct.Rb.AddForce(Vector3.down * this.ProductInitialForce, 2);
			functionalProduct.Rb.AddTorque(UnityEngine.Random.insideUnitSphere * this.ProductRandomTorque, 2);
			this.ProductInstances.Add(functionalProduct);
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x000FFF78 File Offset: 0x000FE178
		private void CheckInsertions()
		{
			for (int i = 0; i < this.ProductInstances.Count; i++)
			{
				if (!(this.ProductInstances[i].Rb == null) && !this.ProductInstances[i].Rb.isKinematic && this.HopperInputCollider.bounds.Contains(this.ProductInstances[i].transform.position))
				{
					this.InsertIntoHopper(this.ProductInstances[i]);
					i--;
				}
			}
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x00100010 File Offset: 0x000FE210
		private void InsertIntoHopper(FunctionalProduct product)
		{
			this.ProductInHopper++;
			this.UpdateScreen();
			if (product.IsHeld)
			{
				this.task.ForceEndClick(product);
			}
			UnityEngine.Object.Destroy(product.gameObject);
			this.ProductInstances.Remove(product);
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x00100060 File Offset: 0x000FE260
		private void DeployPackaging()
		{
			if (this.ConcealedPackaging <= 0)
			{
				return;
			}
			this.ConcealedPackaging--;
			GameObject gameObject = new GameObject("Packaging Container");
			gameObject.transform.SetParent(this.PackagingContainer);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			FunctionalPackaging functionalPackaging = UnityEngine.Object.Instantiate<FunctionalPackaging>(this.PackagingPrefab, gameObject.transform);
			functionalPackaging.AutoEnableSealing = false;
			functionalPackaging.Initialize(this.Station, null, false);
			functionalPackaging.Rb.collisionDetectionMode = 2;
			if (functionalPackaging is FunctionalBaggie)
			{
				functionalPackaging.transform.position = this.BaggieStartPoint.position;
				functionalPackaging.Rb.position = this.BaggieStartPoint.position;
				functionalPackaging.transform.rotation = this.BaggieStartPoint.rotation;
				functionalPackaging.Rb.rotation = this.BaggieStartPoint.rotation;
			}
			else if (functionalPackaging is FunctionalJar)
			{
				functionalPackaging.transform.position = this.JarStartPoint.position;
				functionalPackaging.Rb.position = this.JarStartPoint.position;
				functionalPackaging.transform.rotation = this.JarStartPoint.rotation;
				functionalPackaging.Rb.rotation = this.JarStartPoint.rotation;
			}
			else
			{
				Console.LogError("Unknown packaging type!", null);
			}
			PackagingTool.PackagingInstance packagingInstance = new PackagingTool.PackagingInstance();
			packagingInstance.Container = gameObject.transform;
			packagingInstance.ContainerRb = gameObject.AddComponent<Rigidbody>();
			packagingInstance.ContainerRb.isKinematic = true;
			packagingInstance.ContainerRb.useGravity = false;
			packagingInstance.ContainerRb.collisionDetectionMode = 2;
			packagingInstance.Packaging = functionalPackaging;
			Console.Log("Deployed packaging", null);
			this.PackagingInstances.Insert(0, packagingInstance);
		}

		// Token: 0x04002B7D RID: 11133
		private const float FinalizeRange_Min = 255f;

		// Token: 0x04002B7E RID: 11134
		private const float FinalizeRange_Max = 270f;

		// Token: 0x04002B7F RID: 11135
		[Header("Settings")]
		public float ConveyorSpeed = 1f;

		// Token: 0x04002B80 RID: 11136
		public float ConveyorAcceleration = 1f;

		// Token: 0x04002B81 RID: 11137
		public float BaggieRadius = 0.3f;

		// Token: 0x04002B82 RID: 11138
		public float JarRadius = 0.35f;

		// Token: 0x04002B83 RID: 11139
		public float DeployAngle = 60f;

		// Token: 0x04002B84 RID: 11140
		public float ProductInitialForce = 0.2f;

		// Token: 0x04002B85 RID: 11141
		public float ProductRandomTorque = 0.5f;

		// Token: 0x04002B86 RID: 11142
		public float KickForce = 1f;

		// Token: 0x04002B87 RID: 11143
		public float DropCooldown = 0.25f;

		// Token: 0x04002B88 RID: 11144
		[Header("References")]
		public PackagingStation Station;

		// Token: 0x04002B89 RID: 11145
		public Transform ConveyorModel;

		// Token: 0x04002B8A RID: 11146
		public Animation DoorAnim;

		// Token: 0x04002B8B RID: 11147
		public Animation CapAnim;

		// Token: 0x04002B8C RID: 11148
		public Animation SealAnim;

		// Token: 0x04002B8D RID: 11149
		public Animation KickAnim;

		// Token: 0x04002B8E RID: 11150
		public Clickable LeftButton;

		// Token: 0x04002B8F RID: 11151
		public Clickable RightButton;

		// Token: 0x04002B90 RID: 11152
		public Clickable DropButton;

		// Token: 0x04002B91 RID: 11153
		public Transform PackagingContainer;

		// Token: 0x04002B92 RID: 11154
		public TextMeshPro ProductCountText;

		// Token: 0x04002B93 RID: 11155
		public Transform HopperDropPoint;

		// Token: 0x04002B94 RID: 11156
		public Transform BaggieStartPoint;

		// Token: 0x04002B95 RID: 11157
		public Transform JarStartPoint;

		// Token: 0x04002B96 RID: 11158
		public Transform ProductContainer;

		// Token: 0x04002B97 RID: 11159
		public Transform KickOrigin;

		// Token: 0x04002B98 RID: 11160
		public SphereCollider HopperInputCollider;

		// Token: 0x04002B99 RID: 11161
		public AudioSourceController KickSound;

		// Token: 0x04002B9A RID: 11162
		public AudioSourceController MotorSound;

		// Token: 0x04002B9B RID: 11163
		public AudioSourceController DropSound;

		// Token: 0x04002B9C RID: 11164
		private FunctionalPackaging PackagingPrefab;

		// Token: 0x04002B9D RID: 11165
		private int ConcealedPackaging;

		// Token: 0x04002B9E RID: 11166
		private ProductItemInstance ProductItem;

		// Token: 0x04002B9F RID: 11167
		private FunctionalProduct ProductPrefab;

		// Token: 0x04002BA0 RID: 11168
		private int ProductInHopper;

		// Token: 0x04002BA1 RID: 11169
		private List<PackagingTool.PackagingInstance> PackagingInstances = new List<PackagingTool.PackagingInstance>();

		// Token: 0x04002BA2 RID: 11170
		private List<FunctionalProduct> ProductInstances = new List<FunctionalProduct>();

		// Token: 0x04002BA3 RID: 11171
		private List<FunctionalPackaging> FinalizedPackaging = new List<FunctionalPackaging>();

		// Token: 0x04002BA4 RID: 11172
		private float conveyorVelocity;

		// Token: 0x04002BA5 RID: 11173
		private int directionInput;

		// Token: 0x04002BA6 RID: 11174
		private Task task;

		// Token: 0x04002BA7 RID: 11175
		private PackagingTool.PackagingInstance finalizeInstance;

		// Token: 0x04002BA8 RID: 11176
		private Coroutine finalizeCoroutine;

		// Token: 0x04002BA9 RID: 11177
		private bool leftDown;

		// Token: 0x04002BAA RID: 11178
		private bool rightDown;

		// Token: 0x04002BAB RID: 11179
		private bool dropDown;

		// Token: 0x04002BAC RID: 11180
		private float timeSinceLastDrop = 10f;

		// Token: 0x020008CF RID: 2255
		public class PackagingInstance
		{
			// Token: 0x06003CCF RID: 15567 RVA: 0x001002C8 File Offset: 0x000FE4C8
			public void ChangePosition(float angleDelta)
			{
				this.AnglePosition += angleDelta;
				this.AnglePosition = Mathf.Repeat(this.AnglePosition, 360f);
				Quaternion rhs = Quaternion.Euler(0f, -this.AnglePosition, 0f);
				Quaternion quaternion = this.Container.parent.rotation * rhs;
				this.ContainerRb.MoveRotation(quaternion);
			}

			// Token: 0x04002BAD RID: 11181
			public Transform Container;

			// Token: 0x04002BAE RID: 11182
			public Rigidbody ContainerRb;

			// Token: 0x04002BAF RID: 11183
			public FunctionalPackaging Packaging;

			// Token: 0x04002BB0 RID: 11184
			public float AnglePosition;
		}
	}
}
