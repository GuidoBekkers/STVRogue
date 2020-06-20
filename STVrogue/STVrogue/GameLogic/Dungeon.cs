using System;
using System.Collections.Generic;
using System.Linq;
using STVrogue.Utils;
using static STVrogue.Utils.RandomFactory;


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
        HashSet<Monster> monsters = new HashSet<Monster>();
        HashSet<Item> items = new HashSet<Item>();
        private readonly int _maxRoomCap;
        private int _numberOfRooms;
        Room startRoom;
        Room exitRoom;

        protected Dungeon(){}
        
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
            if (numberOfRooms < 3 || maxRoomCap < 1)
            {
                throw new ArgumentException("invalid number of rooms or invalid maximum capacity");
            }
            
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
                        roomsArray[Game.random.Next(0, _numberOfRooms / 2)].Connect(roomsArray[Game.random.Next(_numberOfRooms / 2 + 1, _numberOfRooms)]);
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
            return Game.random.Next(1, _maxRoomCap + 1);
        }
        
        
        #region getters & setters
        public HashSet<Room> Rooms
        { 
            get => rooms;
            set => rooms = value;
        }

        public HashSet<Monster> Monsters
        {
            get => monsters;
        }
        
        public HashSet<Item> Items
        {
            get => items;
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
            //There must be at least 1 healing, 1 rage potion and 1 monster
            if (numberOfHealingPotion < 1 || numberOfRagePotion < 1)
                return false;
            
            //If there are less monsters then exit room neighbors, there won't be strict more monsters in the exitroom
            //neighbors than the non exitroom neighbors
            if (numberOfMonster < exitRoom.Neighbors.Count)
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
            SeedMonsters(0, numberOfMonster, true);
            
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
        private void SeedMonsters(int currentCap, int remainingMonsters, bool firstRecur = false)
        {
            //base case
            if (remainingMonsters <= 0)
                return; 
            
            int seedAmount = 0; //the amount of monsters we seed in a room
            int cap; //the max amount of monsters we can seed in a room
            int exitRoomNeigh = exitRoom.Neighbors.Count;

            //Fill the exit neighbors with monsters.
            foreach (Room r in exitRoom.Neighbors.Where(r => r != startRoom))
            {
                cap = r.Capacity - r.Monsters.Count; //the max amount of monsters we can seed in this room
                if (firstRecur)
                {
                    int maxSeed = remainingMonsters - exitRoomNeigh >=
                                  r.Capacity
                        ? r.Capacity
                        : remainingMonsters - exitRoomNeigh + 1;
                    seedAmount = Game.random.Next(1, maxSeed + 1);
                }
                else
                {
                    if(remainingMonsters == 0 || cap == 0)
                        seedAmount = 0;
                    else if (cap >= remainingMonsters)
                        seedAmount = Game.random.Next(1, remainingMonsters);
                    else 
                        seedAmount = Game.random.Next(1, cap + 1);
                }

                for (int i = 0; i < seedAmount; i++)
                {
                    r.Monsters.Add(CreateMonster(r));
                    remainingMonsters--;
                }
                
                exitRoomNeigh--;
            }

            currentCap = LowestMonstersAmount(_maxRoomCap, exitRoom.Neighbors);

            //Fill the other rooms
            foreach (Room r in rooms.Where(r => !r.Neighbors.Contains(exitRoom) && r != startRoom))
            {
                if (r.Capacity == r.Monsters.Count)
                    continue;
                int maxCap = r.Capacity >= currentCap ? currentCap - 1 : r.Capacity;
                cap = maxCap - r.Monsters.Count;
                if (cap == 0 || remainingMonsters == 0)
                    seedAmount = 0;
                else if (cap >= remainingMonsters)
                    seedAmount = remainingMonsters;
                else 
                    seedAmount = Game.random.Next(1, cap + 1);
                
                for (int j = 0; j < seedAmount; j++)
                {
                    r.Monsters.Add(CreateMonster(r));
                    remainingMonsters--;
                }
            }
            
            SeedMonsters(currentCap, remainingMonsters);
        }
        
        //Returns the lowest amount of monsters in a hash set of rooms
        private int LowestMonstersAmount(int maxCap, HashSet<Room> rooms)
        {
            int lowestMonstersAmount = maxCap; 
            foreach (Room r in rooms.Where(r => r != startRoom))
            {
                if (r.Monsters.Count < lowestMonstersAmount)
                {
                    lowestMonstersAmount = r.Monsters.Count;
                }
            }
            
            return lowestMonstersAmount;
        }

        //Returns a monster
        private Monster CreateMonster(Room r)
        {
            int hp = Game.random.Next(50, 101); //TODO: tweak HP 
            int ar = Game.random.Next(1, 11); //TODO: tweak AR  
            Monster m = new Monster(IdFactory.GetCreatureId(), "goblin", hp, ar); //ALLE MONSTERS HETEN NU GOBLIN
            m.Location = r;
            monsters.Add(m);
            return m;
        }
        
        public void SeedItems(int numberOfHealingPotion, int numberOfRagePotion)
        {
            Room[] rndmRooms = rooms.OrderBy(x => Game.random.Next()).ToArray();
            int remainingHealingPotions = numberOfHealingPotion;
            int remainingRagePotions = numberOfRagePotion;
            
            //make sure there is at least one healing and rage potion in startRoom.Neighbors
            for (int i = 0; i < rndmRooms.Length; i++)
            {
                if (rndmRooms[i].Neighbors.Contains(startRoom) && rndmRooms[i] != ExitRoom)
                {
                    rndmRooms[i].Items.Add(new HealingPotion(IdFactory.GetItemId(), Game.random.Next(1, 101)));
                    rndmRooms[i].Items.Add(new RagePotion(IdFactory.GetItemId()));
                    remainingHealingPotions--;
                    remainingRagePotions--;  
                    break;
                }
            }

            int emptyRooms = rooms.Count / 2 - 2; //The amount of rooms without potions besides exit and start
            int seedRooms = rooms.Count - (emptyRooms + 2);

            foreach (Room r in rndmRooms)
            {
                if (remainingHealingPotions == 0 && remainingRagePotions == 0)
                    break;
                
                if (r == startRoom || r == exitRoom)
                {
                    continue;
                }
                if (emptyRooms > 0 && r.Items.Count == 0)
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
                        int seedAmount = remainingHealingPotions / seedRooms;
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
                        int seedAmount = remainingRagePotions / seedRooms;
                        AddRagePotions(r, seedAmount);
                        remainingRagePotions -= seedAmount;
                    }
                }
                seedRooms--;
            }

            rooms = rndmRooms.ToHashSet();
        }

        //Adds random amount of potions to a room, returns id for unique ids
        public void AddHealingPotions(Room room, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Item item = new HealingPotion(IdFactory.GetItemId(), Game.random.Next(1, 101));
                room.Items.Add(item);
                items.Add(item);
            }
        }

        public void AddRagePotions(Room room, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Item item = new RagePotion(IdFactory.GetItemId());
                room.Items.Add(item);
                items.Add(item);
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
            this.Capacity = capacity;
        }

        #region getters and setters

        /// <summary>
        /// The number of monsters in this room cannot exceed this capacity.
        /// </summary>
        public int Capacity { get; }

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
