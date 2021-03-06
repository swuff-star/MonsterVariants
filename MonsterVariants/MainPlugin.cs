﻿using BepInEx;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterVariants
{
    [BepInPlugin("com.rob.MonsterVariants", "MonsterVariants", "1.2.0")]

    public class MainPlugin : BaseUnityPlugin
    {
        public static MainPlugin instance;

        internal static GameObject missileLauncherDisplayPrefab; // gotta cache this for lemurians

        public void Awake()
        {
            instance = this;

            // load assets and config before doing anything
            //  currently not loading any assets so no point running this
            //Modules.Assets.PopulateAssets();
            Modules.Config.ReadConfig();
            Modules.Skills.RegisterSkills();

            RegisterVariants();
        }

        internal static void RegisterVariants()
        {
            // gather materials and meshes to use for variants
            ItemDisplayRuleSet itemDisplayRuleSet = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;
            Material lunarGolemMat = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/CharacterBodies/LunarGolemBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
            Material perforatorMat = UnityEngine.Object.Instantiate(itemDisplayRuleSet.FindItemDisplayRuleGroup("FireballsOnHit").rules[0].followerPrefab.GetComponentInChildren<MeshRenderer>().material);
            //Material glandMat = UnityEngine.Object.Instantiate(itemDisplayRuleSet.FindItemDisplayRuleGroup("BeetleGland").rules[0].followerPrefab.GetComponentInChildren<MeshRenderer>().material);
            Material visionsMat = UnityEngine.Object.Instantiate(itemDisplayRuleSet.FindItemDisplayRuleGroup("LunarPrimaryReplacement").rules[0].followerPrefab.GetComponentInChildren<MeshRenderer>().material);
            Material ghostMat = Resources.Load<Material>("Materials/matGhostEffect");
            Material shatterspleenMat = UnityEngine.Object.Instantiate(itemDisplayRuleSet.FindItemDisplayRuleGroup("BleedOnHitAndExplode").rules[0].followerPrefab.GetComponentInChildren<MeshRenderer>().material);
            Material solusMat = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/CharacterBodies/RoboBallBossBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
            missileLauncherDisplayPrefab = itemDisplayRuleSet.FindEquipmentDisplayRuleGroup("CommandMissile").rules[0].followerPrefab;

            //Mesh beedlMesh = Modules.Assets.armoredMesh;
            //Mesh beedlSpeedMesh = Modules.Assets.speedyBeetleMesh;

            // add simple variants
            AddSimpleVariant("Beetle", Modules.Config.armoredBeetleSpawnRate.Value, MonsterVariantTier.Common, GroundSizeModifier(1.5f), 3f, 1f, 1f, 1f, 1f, 0f, 20);// Armored
            AddSimpleVariant("Golem", Modules.Config.fullAutoGolemSpawnRate.Value, MonsterVariantTier.Rare, null, 1f, 1f, 8f, 0.8f, 1f, -20f, 20, lunarGolemMat);// Full-Auto
            AddSimpleVariant("Golem", Modules.Config.titanletSpawnRate.Value, MonsterVariantTier.Uncommon, GroundSizeModifier(2.5f), 4f, 0.5f, 1f, 3f, 1f, 0f);// Titanlet
            AddSimpleVariant("BeetleGuard", Modules.Config.beetleGuardBruteSpawnRate.Value, MonsterVariantTier.Uncommon, GroundSizeModifier(1.1f), 2f, 0.5f, 0.9f, 1.4f, 1f, 10f); // Brute
            AddSimpleVariant("Bison", Modules.Config.speedyBisonSpawnRate.Value, MonsterVariantTier.Common, null, 1f, 4f, 1f, 1f, 1f, 0f); // Speedy
            AddSimpleVariant("Bison", Modules.Config.speedyBisonSpawnRate.Value, MonsterVariantTier.Uncommon, GroundSizeModifier(1.25f), 3f, 1f, 1f, 1f, 1f, 20f, 0, lunarGolemMat); // Albino

            AddSimpleVariant("Titan", Modules.Config.golemletSpawnRate.Value, MonsterVariantTier.Common, GroundSizeModifier(0.3f), 1f, 5f, 1f, 1f, 1f, 0f, 0); // Golemlet
            AddSimpleVariant("Titan", Modules.Config.colossalTitanSpawnRate.Value, MonsterVariantTier.Rare, GroundSizeModifier(3f), 3f, 0.5f, 1f, 2f, 1f, 50f, 3); // Colossus

            // flushed
            AddSimpleVariant("BeetleQueen2", 2, MonsterVariantTier.Common, GroundSizeModifier(0.25f), 0.8f, 5f, 1f, 1f, 1f, 0f); // Stupid


            // Generic CDR inventory
            ItemInfo[] rapidFireInventory = SimpleInventory("AlienHead", 20);

            // Full Shield inventory
            ItemInfo[] fullShieldInventory = SimpleInventory("ShieldOnly");

            // SPEED inventory
            ItemInfo[] speedInventory = SimpleInventory("Hoof", 5);

            // Cursed Jellyfish inventory
            //  i sincerely apologize
            ItemInfo[] hellInventory = new ItemInfo[]
            {
                SimpleItem("IceRing", 2),
                SimpleItem("SprintOutOfCombat", 3),
                SimpleItem("Crowbar", 1)
            };

            // Infernal Wisp inventory
            ItemInfo[] wispInventory = new ItemInfo[]
            {
                SimpleItem("CritGlasses", 10),
                SimpleItem("Behemoth", 3)
            };

            // Clotted Imp inventory
            ItemInfo[] clottedInventory = new ItemInfo[]
            {
                SimpleItem("UtilitySkillMagazine", 2),
                SimpleItem("CritGlasses", 4),
                SimpleItem("BleedOnHitAndExplode", 1)
            };

            // Artillery Vulture inventory
            ItemInfo[] artilleryInventory = new ItemInfo[]
            {
                SimpleItem("Clover", 3),
                SimpleItem("Missile", 1),
                SimpleItem("Behemoth", 3)
            };

            // Speedy Beetle
            AddVariant(new MonsterVariantInfo
            {
                bodyName = "Beetle",
                spawnRate = Modules.Config.speedyBeetleSpawnRate.Value,
                variantTier = MonsterVariantTier.Common,
                sizeModifier = null,
                healthMultiplier = 1f,
                moveSpeedMultiplier = 1f,
                attackSpeedMultiplier = 3f,
                damageMultiplier = 1f,
                armorMultiplier = 1f,
                armorBonus = 0f,
                customInventory = speedInventory,
                meshReplacement = null,
                materialReplacement = null,
                skillReplacement = null
            });

            // Flamethrower Lemurian
            AddVariant(new MonsterVariantInfo
            {
                bodyName = "Lemurian",
                spawnRate = Modules.Config.flamethrowerLemurianSpawnRate.Value,
                variantTier = MonsterVariantTier.Rare,
                sizeModifier = GroundSizeModifier(1.1f),
                healthMultiplier = 1f,
                moveSpeedMultiplier = 1f,
                attackSpeedMultiplier = 30f,
                damageMultiplier = 0.4f,
                armorMultiplier = 1f,
                armorBonus = 0f,
                customInventory = rapidFireInventory,
                meshReplacement = null,
                materialReplacement = SimpleMaterialReplacement(perforatorMat),
                skillReplacement = null
            });

            // Badass Lemurian
            AddVariant(new MonsterVariantInfo
            {
                bodyName = "Lemurian",
                spawnRate = Modules.Config.badassLemurianSpawnRate.Value,
                variantTier = MonsterVariantTier.Common,
                sizeModifier = null,
                healthMultiplier = 1f,
                moveSpeedMultiplier = 1f,
                attackSpeedMultiplier = 1f,
                damageMultiplier = 1f,
                armorMultiplier = 1f,
                armorBonus = 0f,
                customInventory = null,
                meshReplacement = null,
                materialReplacement = null,
                skillReplacement = PrimaryReplacement(Modules.Skills.missileLaunchDef)
            });

            // Nuclear Jellyfish
            AddVariant(new MonsterVariantInfo
            {
                bodyName = "Jellyfish",
                spawnRate = Modules.Config.nuclearJellyfishSpawnRate.Value,
                variantTier = MonsterVariantTier.Common,
                sizeModifier = FlyingSizeModifier(2f),
                healthMultiplier = 15f,
                moveSpeedMultiplier = 0.75f,
                attackSpeedMultiplier = 1f,
                damageMultiplier = 6f,
                armorMultiplier = 1f,
                armorBonus = 20f,
                customInventory = null,
                meshReplacement = null,
                materialReplacement = SimpleMaterialReplacement(visionsMat),
                skillReplacement = SecondaryReplacement(Modules.Skills.nuclearNovaDef)
            });

            // M.O.A.J
            AddVariant(new MonsterVariantInfo
            {
                bodyName = "Jellyfish",
                spawnRate = Modules.Config.MOAJSpawnRate.Value,
                variantTier = MonsterVariantTier.Rare,
                sizeModifier = FlyingSizeModifier(4f),
                healthMultiplier = 40f,
                moveSpeedMultiplier = 0.4f,
                attackSpeedMultiplier = 1f,
                damageMultiplier = 1f,
                armorMultiplier = 1f,
                armorBonus = 80f,
                customInventory = null,
                meshReplacement = null,
                materialReplacement = SimpleMaterialReplacement(solusMat),
                skillReplacement = SecondaryReplacement(Modules.Skills.spawnNovaDef)
            });

            // Overcharged Golem
            AddVariant(new MonsterVariantInfo
            {
                bodyName = "Golem",
                spawnRate = Modules.Config.overchargedGolemSpawnRate.Value,
                variantTier = MonsterVariantTier.Common,
                sizeModifier = null,
                healthMultiplier = 1f,
                moveSpeedMultiplier = 1f,
                attackSpeedMultiplier = 1f,
                damageMultiplier = 1f,
                armorMultiplier = 1f,
                armorBonus = 0f,
                customInventory = fullShieldInventory,
                meshReplacement = null,
                materialReplacement = null,
                skillReplacement = null
            });

            // Cursed Jellyfish
            AddVariant(new MonsterVariantInfo
            {
                bodyName = "Jellyfish",
                spawnRate = Modules.Config.cursedJellyfishSpawnRate.Value,
                variantTier = MonsterVariantTier.Rare,
                sizeModifier = null,
                healthMultiplier = 0.8f,
                moveSpeedMultiplier = 1f,
                attackSpeedMultiplier = 1.5f,
                damageMultiplier = 1f,
                armorMultiplier = 1f,
                armorBonus = 0f,
                customInventory = hellInventory,
                meshReplacement = null,
                materialReplacement = SimpleMaterialReplacement(perforatorMat),
                skillReplacement = null
            });

            // Spectral Jellyfish
            AddVariant(new MonsterVariantInfo
            {
                bodyName = "Jellyfish",
                spawnRate = Modules.Config.spectralJellyfishSpawnRate.Value,
                variantTier = MonsterVariantTier.Rare,
                sizeModifier = FlyingSizeModifier(0.8f),
                healthMultiplier = 0.5f,
                moveSpeedMultiplier = 1.5f,
                attackSpeedMultiplier = 1f,
                damageMultiplier = 0.5f,
                armorMultiplier = 1f,
                armorBonus = 0f,
                customInventory = SimpleInventory("ExtraLife"),
                meshReplacement = null,
                materialReplacement = SimpleMaterialReplacement(ghostMat),
                skillReplacement = null
            });

            // Infernal Wisp
            AddVariant(new MonsterVariantInfo
            {
                bodyName = "Wisp",
                spawnRate = Modules.Config.infernalWispSpawnRate.Value,
                variantTier = MonsterVariantTier.Rare,
                sizeModifier = FlyingSizeModifier(1.2f),
                healthMultiplier = 1f,
                moveSpeedMultiplier = 0.8f,
                attackSpeedMultiplier = 0.6f,
                damageMultiplier = 2f,
                armorMultiplier = 1f,
                armorBonus = 0f,
                customInventory = wispInventory,
                meshReplacement = null,
                materialReplacement = null,
                skillReplacement = null
            });

            // Clotted Imp
            AddVariant(new MonsterVariantInfo
            {
                bodyName = "Imp",
                spawnRate = Modules.Config.clottedImpSpawnRate.Value,
                variantTier = MonsterVariantTier.Uncommon,
                sizeModifier = GroundSizeModifier(0.8f),
                healthMultiplier = 0.8f,
                moveSpeedMultiplier = 1f,
                attackSpeedMultiplier = 1f,
                damageMultiplier = 1f,
                armorMultiplier = 1f,
                armorBonus = 0f,
                customInventory = clottedInventory,
                meshReplacement = null,
                materialReplacement = SimpleMaterialReplacement(shatterspleenMat),
                skillReplacement = null
            });

            // Artillery Vulture
            AddVariant(new MonsterVariantInfo
            {
                bodyName = "Vulture",
                spawnRate = Modules.Config.artilleryVultureSpawnRate.Value,
                variantTier = MonsterVariantTier.Uncommon,
                sizeModifier = GroundSizeModifier(1.25f),
                healthMultiplier = 1f,
                moveSpeedMultiplier = 1f,
                attackSpeedMultiplier = 4f,
                damageMultiplier = 0.5f,
                armorMultiplier = 1f,
                armorBonus = 100f,
                customInventory = artilleryInventory,
                meshReplacement = null,
                materialReplacement = MultiMaterialReplacement(new Dictionary<int, Material> { { 0, perforatorMat }, { 2, solusMat } }),
                skillReplacement = null
            });
        }
       
        // helper for simplifying mat replacements
        public static MonsterMaterialReplacement[] SimpleMaterialReplacement(Material newMaterial)
        {
            return SimpleMaterialReplacement(newMaterial, 0);
        }

        public static MonsterMaterialReplacement[] SimpleMaterialReplacement(Material newMaterial, int index)
        {
            MonsterMaterialReplacement replacement = ScriptableObject.CreateInstance<MonsterMaterialReplacement>();
            replacement.rendererIndex = index;
            replacement.material = newMaterial;

            MonsterMaterialReplacement[] matReplacement = new MonsterMaterialReplacement[]
            {
                replacement
            };

            return matReplacement;
        }

        public static MonsterMaterialReplacement SingleMaterialReplacement(Material newMaterial, int index)
        {
            MonsterMaterialReplacement replacement = ScriptableObject.CreateInstance<MonsterMaterialReplacement>();
            replacement.rendererIndex = index;
            replacement.material = newMaterial;

            return replacement;
        }

        // helper for multiple material replacement, takes a dictionary with <renderer,material>
        public static MonsterMaterialReplacement[] MultiMaterialReplacement(Dictionary<int, Material> newMaterials)
        {
            List<MonsterMaterialReplacement> matReplacement = new List<MonsterMaterialReplacement>();
            foreach (KeyValuePair<int, Material> kvp in newMaterials)
            {
                MonsterMaterialReplacement replacement = SingleMaterialReplacement(kvp.Value, kvp.Key);
                matReplacement.Add(replacement);
            }

            return matReplacement.ToArray();
        }

        // helpers for adding simple variants
        internal static void AddSimpleVariant(string bodyName, float spawnRate, MonsterVariantTier tier, MonsterSizeModifier size, float health, float moveSpeed, float attackSpeed, float damage, float armor, float armorBonus)
        {
            AddSimpleVariant(bodyName, spawnRate, tier, size, health, moveSpeed, attackSpeed, damage, armor, armorBonus, 0, null, null);
        }

        internal static void AddSimpleVariant(string bodyName, float spawnRate, MonsterVariantTier tier, MonsterSizeModifier size, float health, float moveSpeed, float attackSpeed, float damage, float armor, float armorBonus, int alienHeads)
        {
            AddSimpleVariant(bodyName, spawnRate, tier, size, health, moveSpeed, attackSpeed, damage, armor, armorBonus, alienHeads, null, null);
        }

        internal static void AddSimpleVariant(string bodyName, float spawnRate, MonsterVariantTier tier, MonsterSizeModifier size, float health, float moveSpeed, float attackSpeed, float damage, float armor, float armorBonus, int alienHeads, Material replacementMaterial)
        {
            AddSimpleVariant(bodyName, spawnRate, tier, size, health, moveSpeed, attackSpeed, damage, armor, armorBonus, alienHeads, replacementMaterial, null);
        }

        internal static void AddSimpleVariant(string bodyName, float spawnRate, MonsterVariantTier tier, MonsterSizeModifier size, float health, float moveSpeed, float attackSpeed, float damage, float armor, float armorBonus, int alienHeads, Mesh replacementMesh)
        {
            AddSimpleVariant(bodyName, spawnRate, tier, size, health, moveSpeed, attackSpeed, damage, armor, armorBonus, alienHeads, null, replacementMesh);
        }

        internal static void AddSimpleVariant(string bodyName, float spawnRate, MonsterVariantTier tier, MonsterSizeModifier size, float health, float moveSpeed, float attackSpeed, float damage, float armor, float armorBonus, int alienHeads, Material replacementMaterial, Mesh replacementMesh)
        {
            MonsterMaterialReplacement[] replacementMats = null;
            if (replacementMaterial != null) replacementMats = SimpleMaterialReplacement(replacementMaterial);

            MonsterVariantInfo newInfo = new MonsterVariantInfo
            {
                bodyName = bodyName,
                spawnRate = spawnRate,
                variantTier = tier,
                sizeModifier = size,
                healthMultiplier = health,
                moveSpeedMultiplier = moveSpeed,
                attackSpeedMultiplier = attackSpeed,
                damageMultiplier = damage,
                armorMultiplier = armor,
                armorBonus = armorBonus,
                customInventory = SimpleInventory("AlienHead", alienHeads),
                meshReplacement = null,
                materialReplacement = replacementMats,
                skillReplacement = null,
                buff = BuffIndex.None
            };

            Components.VariantHandler variantHandler = Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyName + "Body").AddComponent<Components.VariantHandler>();
            variantHandler.Init(newInfo);
        }

        // helper to simplify adding a variant
        internal static void AddVariant(MonsterVariantInfo info)
        {
            Components.VariantHandler variantHandler = Resources.Load<GameObject>("Prefabs/CharacterBodies/" + info.bodyName + "Body").AddComponent<Components.VariantHandler>();
            variantHandler.Init(info);
        }

        // helper to simplify creating an inventory with one item
        internal static ItemInfo[] SimpleInventory(string itemName)
        {
            return SimpleInventory(itemName, 1);
        }

        internal static ItemInfo[] SimpleInventory(string itemName, int itemCount)
        {
            ItemInfo info = SimpleItem(itemName, itemCount);

            List<ItemInfo> infos = new List<ItemInfo>();

            infos.Add(info);

            ItemInfo[] newInfos = infos.ToArray();

            return newInfos;
        }

        internal static ItemInfo SimpleItem(string itemName)
        {
            return SimpleItem(itemName, 1);
        }

        internal static ItemInfo SimpleItem(string itemName, int itemCount)
        {
            ItemInfo info = ScriptableObject.CreateInstance<ItemInfo>();
            info.itemString = itemName;
            info.count = itemCount;

            return info;
        }

        // all these aren't needed but it's nice to keep things clean up there
        internal static MonsterSkillReplacement[] PrimaryReplacement(SkillDef skill)
        {
            MonsterSkillReplacement skillReplacement = ScriptableObject.CreateInstance<MonsterSkillReplacement>();
            skillReplacement.skillSlot = SkillSlot.Primary;
            skillReplacement.skillDef = skill;

            return new MonsterSkillReplacement[]
            {
                skillReplacement
            };
        }

        internal static MonsterSkillReplacement[] SecondaryReplacement(SkillDef skill)
        {
            MonsterSkillReplacement skillReplacement = ScriptableObject.CreateInstance<MonsterSkillReplacement>();
            skillReplacement.skillSlot = SkillSlot.Secondary;
            skillReplacement.skillDef = skill;

            return new MonsterSkillReplacement[]
            {
                skillReplacement
            };
        }

        internal static MonsterSkillReplacement[] UtilityReplacement(SkillDef skill)
        {
            MonsterSkillReplacement skillReplacement = ScriptableObject.CreateInstance<MonsterSkillReplacement>();
            skillReplacement.skillSlot = SkillSlot.Utility;
            skillReplacement.skillDef = skill;

            return new MonsterSkillReplacement[]
            {
                skillReplacement
            };
        }

        internal static MonsterSkillReplacement[] SpecialReplacement(SkillDef skill)
        {
            MonsterSkillReplacement skillReplacement = ScriptableObject.CreateInstance<MonsterSkillReplacement>();
            skillReplacement.skillSlot = SkillSlot.Special;
            skillReplacement.skillDef = skill;

            return new MonsterSkillReplacement[]
            {
                skillReplacement
            };
        }

        // eh don't need these but same as above
        internal static MonsterSizeModifier GroundSizeModifier(float newSize)
        {
            MonsterSizeModifier sizeModifier = ScriptableObject.CreateInstance<MonsterSizeModifier>();
            sizeModifier.newSize = newSize;
            sizeModifier.scaleCollider = false;

            return sizeModifier;
        }

        internal static MonsterSizeModifier FlyingSizeModifier(float newSize)
        {
            MonsterSizeModifier sizeModifier = ScriptableObject.CreateInstance<MonsterSizeModifier>();
            sizeModifier.newSize = newSize;
            sizeModifier.scaleCollider = true;

            return sizeModifier;
        }
    }
}