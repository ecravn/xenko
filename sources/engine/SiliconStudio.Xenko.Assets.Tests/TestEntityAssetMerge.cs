﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System.Linq;
using NUnit.Framework;
using SiliconStudio.Assets;
using SiliconStudio.Xenko.Assets.Entities;
using SiliconStudio.Xenko.Engine;

namespace SiliconStudio.Xenko.Assets.Tests
{
    [TestFixture]
    public class TestEntityAssetMerge
    {

        [Test]
        public void TestChildAsset()
        {
            // Create an Entity child asset

            // base: EA, EB, EC
            // newAsset: EA'(base: EA), EB'(base: EB), EC'(base: EC)

            var entityA = new Entity() { Name = "A" };
            var entityB = new Entity() { Name = "B" };
            var entityC = new Entity() { Name = "C" };

            // Create Base Asset
            var baseAsset = new EntityAsset();
            baseAsset.Hierarchy.Entities.Add(new EntityDesign(entityA, new EntityDesignData()));
            baseAsset.Hierarchy.Entities.Add(new EntityDesign(entityB, new EntityDesignData()));
            baseAsset.Hierarchy.Entities.Add(new EntityDesign(entityC, new EntityDesignData()));
            baseAsset.Hierarchy.RootEntities.Add(entityA.Id);
            baseAsset.Hierarchy.RootEntities.Add(entityB.Id);
            baseAsset.Hierarchy.RootEntities.Add(entityC.Id);

            var baseAssetItem = new AssetItem("base", baseAsset);

            // Create new Asset (from base)
            var newAsset = (EntityAsset)baseAssetItem.CreateChildAsset();

            // On a derive asset all entities must have a base value and base must come from baseAsset
            Assert.True(newAsset.Hierarchy.Entities.All(item => item.Design.BaseId.HasValue && baseAsset.Hierarchy.Entities.ContainsKey(item.Design.BaseId.Value)));

            // Verify that we have exactly the same number of entities
            Assert.AreEqual(baseAsset.Hierarchy.RootEntities.Count, newAsset.Hierarchy.RootEntities.Count);
            Assert.AreEqual(baseAsset.Hierarchy.Entities.Count, newAsset.Hierarchy.Entities.Count);

            // Verify that baseId and newId is correctly setup
            var entityAInNew = newAsset.Hierarchy.Entities.FirstOrDefault(item => item.Design.BaseId.Value == entityA.Id && item.Entity.Id != item.Design.BaseId.Value);
            Assert.NotNull(entityAInNew);

            var entityBInNew = newAsset.Hierarchy.Entities.FirstOrDefault(item => item.Design.BaseId.Value == entityB.Id && item.Entity.Id != item.Design.BaseId.Value);
            Assert.NotNull(entityBInNew);

            var entityCInNew = newAsset.Hierarchy.Entities.FirstOrDefault(item => item.Design.BaseId.Value == entityC.Id && item.Entity.Id != item.Design.BaseId.Value);
            Assert.NotNull(entityCInNew);

            // Verify that RootEntities are also correctly mapped
            Assert.AreEqual(entityAInNew.Entity.Id, newAsset.Hierarchy.RootEntities[0]);
            Assert.AreEqual(entityBInNew.Entity.Id, newAsset.Hierarchy.RootEntities[1]);
            Assert.AreEqual(entityCInNew.Entity.Id, newAsset.Hierarchy.RootEntities[2]);
        }

        [Test]
        public void TestSimpleMerge()
        {
            // Test merging a simple Entity Asset that has 3 entities
            // base: EA, EB, EC
            // newBase: EA, EB, EC, ED
            // newAsset: EA, EB, EC
            //
            // Result Merge: EA, EB, EC, ED

            var entityA = new Entity() { Name = "A" };
            var entityB = new Entity() { Name = "B" };
            var entityC = new Entity() { Name = "C" };

            // Create Base Asset
            var baseAsset = new EntityAsset();
            baseAsset.Hierarchy.Entities.Add(new EntityDesign(entityA, new EntityDesignData()));
            baseAsset.Hierarchy.Entities.Add(new EntityDesign(entityB, new EntityDesignData()));
            baseAsset.Hierarchy.Entities.Add(new EntityDesign(entityC, new EntityDesignData()));
            baseAsset.Hierarchy.RootEntities.Add(entityA.Id);
            baseAsset.Hierarchy.RootEntities.Add(entityB.Id);
            baseAsset.Hierarchy.RootEntities.Add(entityC.Id);

            var baseAssetItem = new AssetItem("base", baseAsset);

            // Create new Base Asset
            var entityD = new Entity() { Name = "D" };
            var newBaseAsset = (EntityAsset)AssetCloner.Clone(baseAsset);
            newBaseAsset.Hierarchy.Entities.Add(new EntityDesign(entityD, new EntityDesignData()));
            newBaseAsset.Hierarchy.RootEntities.Add(entityD.Id);

            // Create new Asset (from base)
            var newAsset = (EntityAsset)baseAssetItem.CreateChildAsset();

            // Merge entities (NOTE: it is important to clone baseAsset/newBaseAsset)
            var entityMerge = new EntityAssetMerge((EntityAssetBase)AssetCloner.Clone(baseAsset), newAsset, (EntityAssetBase)AssetCloner.Clone(newBaseAsset), null);
            entityMerge.Merge();

            // Both root and entities must be the same
            Assert.AreEqual(4, newAsset.Hierarchy.RootEntities.Count);
            Assert.AreEqual(4, newAsset.Hierarchy.Entities.Count);

            // All entities must have a base value
            Assert.True(newAsset.Hierarchy.Entities.All(item => item.Design.BaseId.HasValue));

            var entityAInNewAsset = newAsset.Hierarchy.Entities.Where(item => item.Design.BaseId.Value == entityA.Id).Select(item => item.Entity).FirstOrDefault();
            Assert.NotNull(entityAInNewAsset);
            var entityBInNewAsset = newAsset.Hierarchy.Entities.Where(item => item.Design.BaseId.Value == entityB.Id).Select(item => item.Entity).FirstOrDefault();
            Assert.NotNull(entityBInNewAsset);
            var entityCInNewAsset = newAsset.Hierarchy.Entities.Where(item => item.Design.BaseId.Value == entityC.Id).Select(item => item.Entity).FirstOrDefault();
            Assert.NotNull(entityCInNewAsset);

            var entityDInNewAsset = newAsset.Hierarchy.Entities.Where(item => item.Design.BaseId.Value == entityD.Id).Select(item => item.Entity).FirstOrDefault();
            Assert.NotNull(entityDInNewAsset);

            // Hierarchy must be: EA, EB, EC, ED
            Assert.AreEqual(entityAInNewAsset.Id, newAsset.Hierarchy.RootEntities[0]);
            Assert.AreEqual(entityBInNewAsset.Id, newAsset.Hierarchy.RootEntities[1]);
            Assert.AreEqual(entityCInNewAsset.Id, newAsset.Hierarchy.RootEntities[2]);
            Assert.AreEqual(entityDInNewAsset.Id, newAsset.Hierarchy.RootEntities[3]);
        }
    }
}