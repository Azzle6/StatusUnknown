using System.Collections;
using UnityEngine;

namespace Player
{
    public abstract class MeleeWeapon : Weapon
    {
        public Coroutine cooldownCoroutine;
        public abstract void Cast();

        public abstract void BuildUp();
        
        public abstract void Active();
        
        public abstract void Recovery();

        public abstract IEnumerator Cooldown();

    }

}

