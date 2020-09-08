using DG.Tweening;
using UnityEngine;
using XD.Utils;

namespace XD
{
    public enum PLAYER_ACTIONS
    {
        NONE,
        GATHER,
        REPAIR, //TODO: WILL DISAPPEAR -> will be add resource
        UPGRADE, //TODO: WILL DISAPPEAR -> will be add resource
        ADD_RESOURCE,
    }

    public abstract class PlayerAction
    {
        public PlayerCharacter playerCharacter;
        public InteractSystem interactSystem;
        protected IInteractable target;
        public virtual PLAYER_ACTIONS Id => PLAYER_ACTIONS.NONE;

        #region HIT_MANAGEMENT
        protected float hitTime = 0.5f; //time between hits
        protected float startHitTimeOffset = 0.25f; //time it takes since the start of the action to make the first hit
        protected float lastHitTime = float.NegativeInfinity;
        protected float TimeSinceLastHit => TimeUtils.TimeSince(lastHitTime);
        protected bool IsHitReady => TimeSinceLastHit >= hitTime;
        #endregion

        protected void SetTarget(IInteractable interactable) { target = interactable; }

        protected PlayerAction(PlayerCharacter pc)
        {
            playerCharacter = pc;
            interactSystem = pc.interactSystem;
        }

        public virtual void Update()
        {
            playerCharacter.RotateToWards(target.GetInteractLookAt());
            if (IsHitReady) Do();
        }

        public virtual void Start(IInteractable interactable)
        {
            SetTarget(interactable);
            lastHitTime = TimeUtils.GetTime() - startHitTimeOffset;
        }
        public abstract void Do();
        public virtual void Stop()
        {
            SetTarget(null);
            playerCharacter.OnActionEnded(Id);
        }
    }

    public class Repair : PlayerAction
    {
        public Repair(PlayerCharacter pc) : base(pc) {}
        public override PLAYER_ACTIONS Id => PLAYER_ACTIONS.REPAIR;
        public BreakableObject breakable;

        public override void Start(IInteractable interactable)
        {
            base.Start(interactable);
            breakable = target as BreakableObject;
            playerCharacter.animator.SetBool(PlayerAnimParams.ChopWood, true);
            playerCharacter.hammer.SetActive(true);
        }

        public override void Do()
        {
            float repairedAmmount = breakable.Repair(2.0f);
            playerCharacter.inventory.SubstractResource(breakable.repairResource, 1.0f);
            GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.REPAIR_WOOD, playerCharacter.gameObject);
            lastHitTime = TimeUtils.GetTime();
        }

        public override void Stop()
        {
            base.Stop();
            playerCharacter.animator.SetBool(PlayerAnimParams.ChopWood, false);
            playerCharacter.hammer.SetActive(false);
            breakable = null;
        }
    }

    public class Gather : PlayerAction
    {
        public Gather(PlayerCharacter pc) : base(pc) {}
        public override PLAYER_ACTIONS Id => PLAYER_ACTIONS.GATHER;
        private GathereableResource gathereable;

        public override void Start(IInteractable interactable)
        {
            base.Start(interactable);
            gathereable = interactable as GathereableResource;
            if (gathereable.type == RESOURCE_TYPE.WOOD)
            {
                playerCharacter.animator.SetBool(PlayerAnimParams.ChopWood, true);
                playerCharacter.axe.SetActive(true);
            }
            else
            {
                playerCharacter.animator.SetBool(PlayerAnimParams.Minning, true);
                playerCharacter.pickaxe.SetActive(true);
            }
        }

        public override void Do()
        {
            Vector3 pos = playerCharacter.transform.position + (Vector3.up * 0.5f) + (playerCharacter.transform.forward * 0.5f);
            float gathered = gathereable.Gather(1.0f, pos);
            playerCharacter.inventory.AddResource(gathereable.type, gathered);
            lastHitTime = TimeUtils.GetTime();
        }

        public override void Stop()
        {
            base.Stop();
            if (gathereable.type == RESOURCE_TYPE.WOOD)
            {
                playerCharacter.animator.SetBool(PlayerAnimParams.ChopWood, false);
                playerCharacter.axe.SetActive(false);
            }
            else
            {
                playerCharacter.animator.SetBool(PlayerAnimParams.Minning, false);
                playerCharacter.pickaxe.SetActive(false);
            }
            gathereable = null;
        }
    }

    public class Upgrade : PlayerAction
    {
        const int upgradeHitCount = 3;
        int currentHitCount = 0;
        public Upgrade(PlayerCharacter pc) : base(pc) {}
        public override PLAYER_ACTIONS Id => PLAYER_ACTIONS.UPGRADE;

        public override void Start(IInteractable interactable)
        {
            base.Start(interactable);
            currentHitCount = 0;
            playerCharacter.hammer.SetActive(true);
            playerCharacter.animator.SetBool(PlayerAnimParams.ChopWood, true);
            playerCharacter.hammer.SetActive(true);
        }

        public override void Do()
        {
            currentHitCount++;
            GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.REPAIR_WOOD, playerCharacter.gameObject);
            ParticleSystemEvents.SpawnParticleEvent.Invoke(Constants.Instance.woodHitParticles, target.GetInteractLookAt(), Quaternion.identity);
            if (currentHitCount >= upgradeHitCount)
            {
                Upgradeable upgradeable = target as Upgradeable;
                upgradeable.SpendCurrentRequirements(playerCharacter.inventory);
                upgradeable.Upgrade();
                interactSystem.StopCurrentAction();
            }

            lastHitTime = TimeUtils.GetTime();
        }

        public override void Stop()
        {
            base.Stop();
            playerCharacter.hammer.SetActive(false);
            playerCharacter.animator.SetBool(PlayerAnimParams.ChopWood, false);
        }
    }

    public class AddResource : PlayerAction
    {
        public AddResource(PlayerCharacter pc) : base(pc) {}
        public override PLAYER_ACTIONS Id => PLAYER_ACTIONS.ADD_RESOURCE;
        public Tower tower;

        public override void Start(IInteractable interactable)
        {
            base.Start(interactable);
            playerCharacter.animator.SetBool(PlayerAnimParams.ChopWood, true);
            playerCharacter.hammer.SetActive(true);
            tower = interactable as Tower; 
        }
        
        public override void Do()
        {
            ResourceInventory inventory = playerCharacter.inventory;
            //TODO: this will change because you will only be able to carry one resource at a time
            //wont be this shitty
            if(inventory.HasResource(RESOURCE_TYPE.WOOD))
            {
                inventory.SubstractResource(RESOURCE_TYPE.WOOD, 1.0f);
                tower.AddCharge(Constants.Instance.turretTypesDB.GetTurretTypeData(RESOURCE_TYPE.WOOD));
            }
            else if (inventory.HasResource(RESOURCE_TYPE.STONE))
            {
                inventory.SubstractResource(RESOURCE_TYPE.STONE, 1.0f);
                tower.AddCharge(Constants.Instance.turretTypesDB.GetTurretTypeData(RESOURCE_TYPE.STONE));
            }
            else if (inventory.HasResource(RESOURCE_TYPE.CRYSTAL))
            {
                inventory.SubstractResource(RESOURCE_TYPE.CRYSTAL, 1.0f);
                tower.AddCharge(Constants.Instance.turretTypesDB.GetTurretTypeData(RESOURCE_TYPE.CRYSTAL));
            }

            lastHitTime = TimeUtils.GetTime();
            if (!inventory.HasAnyResource()) interactSystem.StopCurrentAction();
        }

        public override void Stop()
        {
            base.Stop();
            playerCharacter.animator.SetBool(PlayerAnimParams.ChopWood, false);
            playerCharacter.hammer.SetActive(false);
            tower = null;
        }

    }
}
