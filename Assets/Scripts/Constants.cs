using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlankStudio.Constants
{
    public class Constants
    {
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

        public enum WeaponType
        {
            Gun,
            Sword,
            Axe,
            FlameThrower,

        }
    }
}