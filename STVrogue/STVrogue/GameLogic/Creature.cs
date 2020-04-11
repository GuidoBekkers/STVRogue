using System;
using System.Collections.Generic;

namespace STVrogue.GameLogic
{
    public class Creature : GameEntity
    {

        string name = "goblin";
        int hpMax;
        int hp ;     // current HP, should never exceed HPmax
        bool alive ;
        Room location;
        int attackRating ;
        
        public Creature(string id, String name) : base(id)
        {
            this.name = name;
            // you need to decide how to initialize the other attributes
        }

        public Creature(string id, int hp, int ar) : base(id)
        {
            if(id==null || hp<=0 || ar<=0) 
                throw new ArgumentException();
            this.hp = hp;
            hpMax = hp;
            attackRating = ar;
        }

        #region getters setters
        public string Name => name;

        public int Hp
        {
            get => hp;
            set => hp = value;
        }

        public int HpMax
        {
            get => hpMax;
            set => hpMax = value;
        }

        public bool Alive
        {
            get => alive;
            set => alive = value;
        }

        public Room Location
        {
            get => location;
            set => location = value;
        }

        public int AttackRating
        {
            get => attackRating;
            set => attackRating = value;
        }
        #endregion
        
        /// <summary>
        /// Move this creature to the given room. This is only allowed if r
        /// is a neigboring room of the creature's current location. Also
        /// keep in mind that rooms have capacity.
        /// The metod returns true if the move is successful, else false.
        /// </summary>
        public virtual bool Move(Room r)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Attack the given foe. This is only possible if this creature is alive and
        /// if the foe is in the same room as this creature.
        /// </summary>
        public virtual void Attack(Creature foe)
        {
            throw new NotImplementedException();
        }
       
    }

    /// <summary>
    /// Representing monsters.... you know, those scary things you don't want
    /// to mess with.
    /// </summary>
    public class Monster : Creature
    {
        public Monster(String id, String name) : base(id,name)
        {
        }
        
    }

    public class Player : Creature
    {
        /* kill point */
        int kp = 0;
        List<Item> bag = new List<Item>();

        public Player(String id, String name) : base(id,name)
        {
            // you need to decide how to initialize the other attributes
            throw new NotImplementedException();
        }

        #region getters setters
        public int Kp
        {
            get => kp;
            set => kp = value;
        }

        public List<Item> Bag
        {
            get => bag;
            set => bag = value;
        }
        #endregion

        /// <summary>
        /// Use the given item. We also pass the current turn-number at which
        /// this action happens.
        /// </summary>
        public void Use(long turnNr, Item i)
        {
            throw new NotImplementedException();
        }
    }
    
}
