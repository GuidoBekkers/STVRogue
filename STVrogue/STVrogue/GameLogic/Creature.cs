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
            Alive = true;
        }

        public Creature(string id, int hp, int ar) : base(id)
        {
            if(id==null || hp<=0 || ar<=0) 
                throw new ArgumentException();
            this.hp = hp;
            hpMax = hp;
            attackRating = ar;
            Alive = true;
        }
        
        public Creature(string id, string name, int hp, int ar) : base(id)
        {
            if(id==null || hp<=0 || ar<=0) 
                throw new ArgumentException();
            this.name = name;
            this.hp = hp;
            hpMax = hp;
            attackRating = ar;
            Alive = true;
        }

        #region getters setters
        public string Name => name;

        // Set hp to hpMax if set to higher then hpMax otherwise normal set
        public int Hp
        {
            get => hp;
            set
            {
                if (value > HpMax)
                {
                    hp = HpMax;
                }
                else
                {
                    hp = value;
                }
            }
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
        ///
        /// The code below provides a base implementation. You may have to override
        /// it for Player as Player is not restricted by room capacity.
        /// </summary>
        public virtual void Move(Room r)
        {
            if (!Location.Neighbors.Contains(r))
                throw new ArgumentException();
            if (r.Monsters.Count >= r.Capacity)
                throw new ArgumentException();
            r.Monsters.Add(this);
            Location.Monsters.Remove(this);
            Location = r;
        }
        
        /// <summary>
        /// Attack the given foe. This is only possible if this creature is alive and
        /// if the foe is in the same room as this creature.
        /// The code below provides a base implementation of this method. You may have
        /// to override this for Player.
        /// </summary>
        public virtual void Attack(Creature foe)
        {
            if (!Alive || Location != foe.Location || !foe.Alive)
                throw new ArgumentException();
            foe.Hp = Math.Max(0,foe.Hp - AttackRating);
            if (foe.Hp == 0) foe.Alive = false;
        }
       
    }

    /// <summary>
    /// Representing monsters.... you know, those scary things you don't want
    /// to mess with.
    /// </summary>
    public class Monster : Creature
    {
        public Monster(string id, string name, int hp, int ar) : base(id, name, hp, ar)
        {
        }
    }

    public class Player : Creature
    {
        /* kill point */
        int kp = 0;
        List<Item> bag = new List<Item>();

        /// <summary>
        /// True if the player is enraged. The player enters this state whenever it uses a rage potion.
        /// The effect last for 5 turns including the turn when the potion is used.
        /// </summary>
        bool enraged = false;

        // Constructor for player
        public Player(string id, string name, int hp, int ar) : base(id, name, hp, ar)
        {
            
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
        
        public bool Enraged
        {
            get => enraged;
            set => enraged = value;
        }
        #endregion

        /// <summary>
        /// Use the given item. We also pass the current turn-number at which
        /// this action happens.
        /// </summary>
        public void Use(long turnNr, Item i)
        {
            throw new NotImplementedException();
            // TODO
        }

        // Player moves, only to neighboring rooms
        public override void Move(Room r)
        {
            if (!Location.Neighbors.Contains(r))
                throw new ArgumentException();
            Location = r;
        }
        
        // Player attacks a creature
        public override void Attack(Creature foe)
        {
            base.Attack(foe);
            
            // If it kill the other creature, kill points increase
            if (foe.Hp == 0)
            {
                Kp++;
            }
        }
    }
    
}
