namespace Player
{
    public abstract class MeleeWeapon : Weapon
    {
        public abstract void Cast();

        public abstract void BuildUp();
        
        public abstract void Active();
        
        public abstract void Recovery();
        
    }

}

