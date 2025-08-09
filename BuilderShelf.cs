using System;
using System.Collections.Generic;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200055E RID: 1374
public class BuilderShelf : MonoBehaviour
{
	// Token: 0x06002189 RID: 8585 RVA: 0x000B6630 File Offset: 0x000B4830
	public void Init()
	{
		this.shelfSlot = 0;
		this.buildPieceSpawnIndex = 0;
		this.spawnCount = 0;
		this.count = 0;
		this.spawnCosts = new List<BuilderResources>(this.buildPieceSpawns.Count);
		for (int i = 0; i < this.buildPieceSpawns.Count; i++)
		{
			this.count += this.buildPieceSpawns[i].count;
			BuilderPiece component = this.buildPieceSpawns[i].buildPiecePrefab.GetComponent<BuilderPiece>();
			this.spawnCosts.Add(component.cost);
		}
	}

	// Token: 0x0600218A RID: 8586 RVA: 0x000B66CB File Offset: 0x000B48CB
	public bool HasOpenSlot()
	{
		return this.shelfSlot < this.count;
	}

	// Token: 0x0600218B RID: 8587 RVA: 0x000B66DC File Offset: 0x000B48DC
	public void BuildNextPiece(BuilderTable table)
	{
		if (!this.HasOpenSlot())
		{
			return;
		}
		BuilderShelf.BuildPieceSpawn buildPieceSpawn = this.buildPieceSpawns[this.buildPieceSpawnIndex];
		BuilderResources builderResources = this.spawnCosts[this.buildPieceSpawnIndex];
		while (!table.HasEnoughUnreservedResources(builderResources) && this.buildPieceSpawnIndex < this.buildPieceSpawns.Count - 1)
		{
			int num = buildPieceSpawn.count - this.spawnCount;
			this.shelfSlot += num;
			this.spawnCount = 0;
			this.buildPieceSpawnIndex++;
			buildPieceSpawn = this.buildPieceSpawns[this.buildPieceSpawnIndex];
			builderResources = this.spawnCosts[this.buildPieceSpawnIndex];
		}
		if (!table.HasEnoughUnreservedResources(builderResources))
		{
			int num2 = buildPieceSpawn.count - this.spawnCount;
			this.shelfSlot += num2;
			this.spawnCount = 0;
			return;
		}
		int staticHash = buildPieceSpawn.buildPiecePrefab.name.GetStaticHash();
		int num3 = (string.IsNullOrEmpty(buildPieceSpawn.materialID) ? (-1) : buildPieceSpawn.materialID.GetHashCode());
		Vector3 vector;
		Quaternion quaternion;
		this.GetSpawnLocation(this.shelfSlot, buildPieceSpawn, out vector, out quaternion);
		int num4 = table.CreatePieceId();
		table.CreatePiece(staticHash, num4, vector, quaternion, num3, BuilderPiece.State.OnShelf, PhotonNetwork.LocalPlayer);
		this.spawnCount++;
		this.shelfSlot++;
		if (this.spawnCount >= buildPieceSpawn.count)
		{
			this.buildPieceSpawnIndex++;
			this.spawnCount = 0;
		}
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x000B6858 File Offset: 0x000B4A58
	public void InitCount()
	{
		this.count = 0;
		for (int i = 0; i < this.buildPieceSpawns.Count; i++)
		{
			this.count += this.buildPieceSpawns[i].count;
		}
	}

	// Token: 0x0600218D RID: 8589 RVA: 0x000B68A0 File Offset: 0x000B4AA0
	public void BuildItems(BuilderTable table)
	{
		int num = 0;
		this.InitCount();
		for (int i = 0; i < this.buildPieceSpawns.Count; i++)
		{
			BuilderShelf.BuildPieceSpawn buildPieceSpawn = this.buildPieceSpawns[i];
			if (buildPieceSpawn != null && buildPieceSpawn.count != 0)
			{
				int staticHash = buildPieceSpawn.buildPiecePrefab.name.GetStaticHash();
				int num2 = (string.IsNullOrEmpty(buildPieceSpawn.materialID) ? (-1) : buildPieceSpawn.materialID.GetHashCode());
				int num3 = 0;
				while (num3 < buildPieceSpawn.count && num < this.count)
				{
					Vector3 vector;
					Quaternion quaternion;
					this.GetSpawnLocation(num, buildPieceSpawn, out vector, out quaternion);
					int num4 = table.CreatePieceId();
					table.CreatePiece(staticHash, num4, vector, quaternion, num2, BuilderPiece.State.OnShelf, PhotonNetwork.LocalPlayer);
					num++;
					num3++;
				}
			}
		}
	}

	// Token: 0x0600218E RID: 8590 RVA: 0x000B696C File Offset: 0x000B4B6C
	public void GetSpawnLocation(int slot, BuilderShelf.BuildPieceSpawn spawn, out Vector3 spawnPosition, out Quaternion spawnRotation)
	{
		if (this.center == null)
		{
			this.center = base.transform;
		}
		Vector3 vector = spawn.positionOffset;
		Vector3 vector2 = spawn.rotationOffset;
		BuilderPiece component = spawn.buildPiecePrefab.GetComponent<BuilderPiece>();
		if (component != null)
		{
			vector = component.desiredShelfOffset;
			vector2 = component.desiredShelfRotationOffset;
		}
		spawnRotation = this.center.rotation * Quaternion.Euler(vector2);
		float num = (float)slot * this.separation - (float)(this.count - 1) * this.separation / 2f;
		spawnPosition = this.center.position + this.center.rotation * (spawn.localAxis * num + vector);
	}

	// Token: 0x0600218F RID: 8591 RVA: 0x000026E9 File Offset: 0x000008E9
	public BuilderShelf()
	{
	}

	// Token: 0x04002ADF RID: 10975
	private int count;

	// Token: 0x04002AE0 RID: 10976
	public float separation;

	// Token: 0x04002AE1 RID: 10977
	public Transform center;

	// Token: 0x04002AE2 RID: 10978
	public Material overrideMaterial;

	// Token: 0x04002AE3 RID: 10979
	public List<BuilderShelf.BuildPieceSpawn> buildPieceSpawns;

	// Token: 0x04002AE4 RID: 10980
	private List<BuilderResources> spawnCosts;

	// Token: 0x04002AE5 RID: 10981
	private int shelfSlot;

	// Token: 0x04002AE6 RID: 10982
	private int buildPieceSpawnIndex;

	// Token: 0x04002AE7 RID: 10983
	private int spawnCount;

	// Token: 0x0200055F RID: 1375
	[Serializable]
	public class BuildPieceSpawn
	{
		// Token: 0x06002190 RID: 8592 RVA: 0x000B6A3A File Offset: 0x000B4C3A
		public BuildPieceSpawn()
		{
		}

		// Token: 0x04002AE8 RID: 10984
		public GameObject buildPiecePrefab;

		// Token: 0x04002AE9 RID: 10985
		public string materialID;

		// Token: 0x04002AEA RID: 10986
		public int count = 1;

		// Token: 0x04002AEB RID: 10987
		public Vector3 localAxis = Vector3.right;

		// Token: 0x04002AEC RID: 10988
		[Tooltip("Use BuilderPiece:desiredShelfOffset instead")]
		public Vector3 positionOffset;

		// Token: 0x04002AED RID: 10989
		[Tooltip("Use BuilderPiece:desiredShelfRotationOffset instead")]
		public Vector3 rotationOffset;

		// Token: 0x04002AEE RID: 10990
		[Tooltip("Optional Editor Visual")]
		public Mesh previewMesh;
	}
}
