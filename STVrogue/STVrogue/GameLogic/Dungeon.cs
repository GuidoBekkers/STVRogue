using System;
using System.Collections.Generic;
using System.Linq;
using STVrogue.Utils;


namespace STVrogue.GameLogic
{
    
    /// <summary>
    /// Representing a dungeon. A dungeon consists of rooms, connected to from a graph.
    /// It has one unique starting room and one unique exit room. All rooms should be
    /// reachable from the starting room.
    /// </summary>
    public class Dungeon
    {
        HashSet<Room> rooms = new HashSet<Room>();
        private HashSet<Monster> monsters = new HashSet<Monster>();
        private readonly int _maxRoomCap;
        private int _numberOfRooms;
        Room startRoom;
        Room exitRoom;
        private readonly Random random;

        protected Dungeon() { }
        
        /// <summary>
        /// Create a dungeon with the indicated number of rooms and the indicated shape.
        /// A dungeon shape can be "linear" (list-shaped), "tree", or "random".
        /// A dungeon should have
        /// a unique start-room and a unique exit-room. All rooms in the dungeon must be
        /// reachable from the start-room.
        /// Each room is set to have a random capacity between 1 and the given maximum-capacity.
        /// Start and exit-rooms should have capacity 0.
        /// </summary>
        public Dungeon(DungeonShapeType shape, int numberOfRooms, int maxRoomCap)
        {
            //precondition
            if (numberOfRooms < 3)
            {
                throw new ArgumentException();
            }
            
            random = new Random(5);
            _maxRoomCap = maxRoomCap;
            _numberOfRooms = numberOfRooms;
            
            switch (shape)
            {
                case DungeonShapeType.LINEARshape: 
                    LinearDungeon();
                    break;
                case DungeonShapeType.TREEshape:
                    startRoom = BinaryTreeDungeon(0);
                    break;
                case DungeonShapeType.RANDOMshape:
                    LinearDungeon();
                    Room[] roomsArray = rooms.ToArray();
                    for (int i = 0; i < _numberOfRooms / 3; i++)
                    {
                        roomsArray[random.Next(0, _numberOfRooms / 2)].Connect(roomsArray[random.Next(_numberOfRooms / 2 + 1, _numberOfRooms)]);
                    }
                    rooms = roomsArray.ToHashSet();
                    break;
            }
        }

        public void LinearDungeon()
        {
            startRoom = new Room("RS", RoomType.STARTroom, 0);
            exitRoom = new Room("RE", RoomType.EXITroom, 0);
            rooms.Add(startRoom);
            Room prevRoom = startRoom;
            for (int i = 0; i < _numberOfRooms - 2; i++)
            {
                Room currRoom = new Room(IdFactory.GetRoomId(), RoomType.ORDINARYroom, RoomCap());
                rooms.Add(currRoom);
                currRoom.Connect(prevRoom);
                prevRoom = currRoom;
            }
            rooms.Add(exitRoom);
            prevRoom.Connect(exitRoom);
        }

        public Room BinaryTreeDungeon(int i)
        {
            Room room;
            if (i == 0)
            {
                room = new Room("RS", RoomType.STARTroom, 0);
            }
            else if (i == _numberOfRooms - 1)
            {
                room = new Room("RE", RoomType.EXITroom, 0);
                ExitRoom = room;
            }
            else
            {
                room = new Room(IdFactory.GetRoomId(),  RoomType.ORDINARYroom, RoomCap());
            }
            rooms.Add(room);
            
            int j = 2 * i + 1;
            int k = 2 * i + 2;
            if (j < _numberOfRooms )
            {
                room.Connect(BinaryTreeDungeon(j));
            }
            if (k < _numberOfRooms)
            {
                room.Connect(BinaryTreeDungeon(k));
            }

            return room;
        }

        private int RoomCap()
        {
            return random.Next(1, _maxRoomCap + 1);
        }
        
        
        #region getters & setters
        public HashSet<Room> Rooms
        { 
            get => rooms;
            set => rooms = value;
        }

        public Room StartRoom
        {
            get => startRoom;
            set => startRoom = value;
        }

        public Room ExitRoom
        {
            get => exitRoom;
            set => exitRoom = value;
        }
        #endregion

        /// <summary>
        /// Populate the dungeon with the specified number of monsters and items.
        /// They are dropped in random locations. Keep in mind that the number of
        /// monsters in a room should not exceed the room's capacity. There are also
        /// other constraints; see the Project Document.
        ///
        /// Note that it is not always possible to populate the dungeon according to
        /// the specified parameters. E.g. in a dungeon with N rooms whose capacity
        /// are between 0 and k, it is definitely not possible to populate it with
        /// (N-2)*k monsters or more.
        /// The method returns true if it manages to populate the dungeon as specified,
        /// else it returns false.
        /// If it is impossible to do it, it returns false.
        /// </summary>
        public bool SeedMonstersAndItems(int numberOfMonster, int numberOfHealingPotion, int numberOfRagePotion)
        {
            //There must be at least 1 healing and rage potion
            if (numberOfHealingPotion < 1 || numberOfRagePotion < 1)
                return false;
            
            //Count the max monsters we can spawn near the exit
            int maxExitMonsters = MaxNeighMonsters(exitRoom);
            
            //Check the lowest capacity room of exit neighbors
            int lowestCap = LowestNeighCap(exitRoom);
            //Calculate the max monsters we can spawn not near the exit
            int maxNonExitMonsters = 0;
            foreach (Room room in Rooms
                .Where(x => !x.Neighbors.Contains(exitRoom)))
            {
                int cap = room.Capacity;
                maxNonExitMonsters += cap >= lowestCap ? lowestCap - 1 : cap;
            }
            
            //Check if there is enough capacity to seed the monsters 
            if (numberOfMonster - maxExitMonsters > maxNonExitMonsters)
                return false;
            
            //Seed the monsters
            SeedMonsters(lowestCap, numberOfMonster, true);
            
            //Seed the items
            SeedItems(numberOfHealingPotion, numberOfRagePotion);

            return true;
        }
        
        //Returns the maximum monsters that can be seeded in room.Neighbors
        private int MaxNeighMonsters(Room room)
        {
            int maxExitMonsters = 0;
            foreach (Room neigh in room.Neighbors)
            {
                maxExitMonsters += neigh.Capacity;
            }

            return maxExitMonsters;
        }
        
        //Returns the lowest capacity of room.Neighbors
        private int LowestNeighCap(Room room)
        {
            int lowestCap = _maxRoomCap;
            foreach (Room neigh in room.Neighbors)
            {
                int cap = neigh.Capacity;
                if (cap < lowestCap)
                    lowestCap = cap;
            }

            return lowestCap;
        }
        
        //Fills the dungeon with monsters
        private void SeedMonsters(int lowestCap, int numberOfMonsters, bool firstRecur = false)
        {
            if (numberOfMonsters <= 0)
            {
                return;
            }

            int counter = 0; //used for unique IDs
            int seedAmount; //the amount of monsters we seed in a room
            int cap; //the max amount of monsters we can seed in a room
            int remainingMonsters = numberOfMonsters; //used to keep track of the monsters that need to be seeded

            //Fill the exit neighbors with monsters.
            foreach (Room neigh in exitRoom.Neighbors)
            {
                if (remainingMonsters <= 0)
                    return;

                cap = neigh.Capacity - neigh.Monsters.Count; //the max amount of monsters we can seed in this room
                if (cap > remainingMonsters)
                    seedAmount = remainingMonsters;
                else
                    seedAmount = firstRecur ? random.Next(lowestCap, cap + 1) : random.Next(cap + 1);

                for (int i = 0; i < seedAmount; i++)
                {
                    neigh.Monsters.Add(CreateMonster());
                    counter++;
                    remainingMonsters--;
                }
            }

            //Fill the other rooms
            foreach (Room r in rooms.Where(r => !r.Neighbors.Contains(exitRoom)))
            {
                if (remainingMonsters <= 0)
                    return;
                
                int maxCap = r.Capacity >= lowestCap ? lowestCap - 1 : r.Capacity;
                cap = maxCap - r.Monsters.Count;
                if (cap > remainingMonsters)
                    seedAmount = remainingMonsters;
                else
                    seedAmount = random.Next(cap + 1);
                for (int j = 0; j < seedAmount; j++)
                {
                    r.Monsters.Add(CreateMonster());
                    counter++;
                    remainingMonsters--;
                }
            }
            
            SeedMonsters(lowestCap, remainingMonsters);
        }

        //Returns a monster
        private Monster CreateMonster()
        {
            int hp = random.Next(50, 101); //TODO: tweak HP 
            int ar = random.Next(1, 11); //TODO: tweak AR  
            Monster m = new Monster(IdFactory.GetCreatureId(), "goblin", hp, ar); //ALLE MONSTERS HETEN NU GOBLIN
            monsters.Add(m);
            return m;
        }

        public void SeedItems(int numberOfHealingPotion, int numberOfRagePotion)
        {
            Room[] rndmRooms = rooms.OrderBy(x => random.Next()).ToArray();
            int remainingHealingPotions = numberOfHealingPotion;
            int remainingRagePotions = numberOfRagePotion;
            
            //make sure there is at least one healing and rage potion in startRoom.Neighbors
            for (int i = 0; i < rndmRooms.Length; i++)
            {
                Room r = rndmRooms[i];
                if (r.Neighbors.Contains(startRoom))
                {
                    r.Items.Add(new HealingPotion(IdFactory.GetItemId(), random.Next(1, 101)));
                    r.Items.Add(new RagePotion(IdFactory.GetItemId()));
                    remainingHealingPotions--;
                    remainingRagePotions--;
                    break;
                }
            }

            int emptyRooms = rooms.Count / 2 - 2; //The amount of rooms without potions besides exit and start
            int seedRooms = rooms.Count - emptyRooms - 2;

            for (int i = 0; i < rndmRooms.Length; i++)
            {
                if (remainingHealingPotions <= 0 && remainingRagePotions <= 0)
                    break;
                
                Room r = rndmRooms[i];
                if (r == startRoom || r == exitRoom)
                {
                    continue;
                }
                if (emptyRooms > 0)
                {
                    emptyRooms--;
                    continue;
                }

                if (remainingHealingPotions > 0)
                {
                    if (seedRooms >= numberOfHealingPotion)
                    {
                        AddHealingPotions(r, 1);
                        remainingHealingPotions--;
                    }
                    else
                    {
                        int seedAmount = numberOfHealingPotion / seedRooms + numberOfHealingPotion % seedRooms;
                        AddHealingPotions(r, seedAmount);
                        remainingHealingPotions -= seedAmount;
                    }
                }
                
                if (remainingRagePotions > 0)
                {
                    if (seedRooms >= numberOfRagePotion)
                    {
                        AddRagePotions(r, 1);
                        remainingRagePotions--;
                    }
                    else
                    {
                        int seedAmount = numberOfRagePotion / seedRooms + numberOfRagePotion % seedRooms;
                        AddRagePotions(r, seedAmount);
                        remainingRagePotions -= seedAmount;
                    }
                }
                seedRooms--;
            }
        }

        //Adds random amount of potions to a room, returns id for unique ids
        public void AddHealingPotions(Room room, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                room.Items.Add(new HealingPotion(IdFactory.GetItemId(), random.Next(1, 101)));
            }
        }

        public void AddRagePotions(Room room, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                room.Items.Add(new RagePotion(IdFactory.GetItemId()));
            }
        }
    }

    [Serializable()]
    public enum DungeonShapeType
    {
        LINEARshape, 
        TREEshape,
        RANDOMshape
    }
    
    /// <summary>
    /// Representing different types of rooms.
    /// </summary>
    public enum RoomType
    {
        STARTroom,  // the starting room of the player. 
        EXITroom,   // representing the player's final destination.
        ORDINARYroom  // the type of the rest of the rooms. 
    }

    /// <summary>
    /// Representing a room in a dungeon.
    /// </summary>
    public class Room : GameEntity
    {
        /// <summary>
        /// The number of monsters in this room cannot exceed this capacity.
        /// </summary>
        int capacity;
        
        /// <summary>
        /// The type of this node: either start-node, exit-node, or common-node.
        /// </summary>
        RoomType roomType ;
        
        /// <summary>
        /// Neighbors are nodes that are considered connected to this node.
        /// The connection is bidirectional. If u is in this.neighbors of this room,
        /// you have to make sure that this room is also in u.neighbors.
        /// </summary>
        HashSet<Room> neighbors = new HashSet<Room>();
        HashSet<Creature> monsters = new HashSet<Creature>();
        HashSet<Item> items = new HashSet<Item>();
        
        public Room(string uniqueId, RoomType roomTy, int capacity) : base(uniqueId)
        {
            this.roomType = roomTy;
            this.capacity = capacity;
        }

        #region getters and setters
        public int Capacity => capacity;
        public HashSet<Room> Neighbors => neighbors;
        public HashSet<Creature> Monsters => monsters;

        public HashSet<Item> Items => items;

        public RoomType RoomType
        {
            get => roomType;
            set => roomType = value;
        }
        #endregion

        /// <summary>
        /// To add the given room as a neighbor of this room.
        /// </summary>
        public void Connect(Room r)
        {
            neighbors.Add(r); r.neighbors.Add(this);
        }

        /// <summary>
        /// To disconnect the given room. That is, the room r will no longer be a
        /// neighbor of this room.
        /// </summary>
        public void Disconnect(Room r)
        {
            neighbors.Remove(r); r.neighbors.Remove(this);
        }

        /// <summary>
        /// return the set of all rooms which are reachable from this room.
        /// </summary>
        public List<Room> ReachableRooms()
        {
            Room x = this;
            List<Room> seen = new List<Room>();
            List<Room> todo = new List<Room>();
            todo.Add(x);
            while (todo.Count > 0)
            {
                x = todo[0] ; todo.RemoveAt(0) ;
                seen.Add(x);
                foreach (Room y in x.neighbors)
                {
                    if (seen.Contains(y) || todo.Contains(y)) continue;
                    todo.Add(y);
                }
            }
            return seen;
        }

        /// <summary>
        /// Check if the given room is reachable from this room.
        /// </summary>
        public bool CanReach(Room r)
        {
            return ReachableRooms().Contains(r); // not the most efficient way of checking it btw
        }
    }



}
