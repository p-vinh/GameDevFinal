using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlankStudio.Constants
{
    public class Constants
    {
        public PlayerState playerState { get; set;}
        public WeaponType weaponType { get; set; }
        public AnimationNames animationNames { get; set; }

        public enum PlayerState
        {
            Idle,
            Run,
            RunNAttack,
            IdleNAttack,
            Death
        }

        public enum AnimationNames
        {
            Idel_1, 
            Idel_2,
            Idel_3,
            Death, 
            Run, 
            SwordAttack, 
            GunShoot, 
            AxeAttack, 
            FlameThrower
        }

        // We should make this a dictionary that holds the stats. Currently it is seperated into two classes
        // One for the weapon type and one for the weapon stats
        public enum WeaponType
        {
            Gun,
            Sword,
            Axe,
            FlameThrower,

        }
    }
}