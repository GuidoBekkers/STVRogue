using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Transactions;

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
        private readonly int _maximumRoomCapacity;
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
        public Dungeon(DungeonShapeType shape, int numberOfRooms, int maximumRoomCapacity)
        {
            if (numberOfRooms < 2)
            {
                throw new System.Exception("Not enough number of rooms. " +
                                           "Dungeon must consist of at least 2 rooms "); //wat voor exception?
            }

            random = new Random(5);
            _maximumRoomCapacity = maximumRoomCapacity;
            _numberOfRooms = numberOfRooms;
            
            switch (shape)
            {
                case DungeonShapeType.LINEARshape: 
                    startRoom = new Room("RS", RoomType.STARTroom, 0);
                    exitRoom = new Room("RE", RoomType.EXITroom, 0);
                    rooms.Add(startRoom);
                    Room prevRoom = startRoom;
                    for (int i = 0; i < numberOfRooms - 2; i++)
                    {
                        Room currRoom = new Room("R" + i, RoomType.ORDINARYroom, RoomCapacity());
                        rooms.Add(currRoom);
                        currRoom.Connect(prevRoom);
                        prevRoom = currRoom;
                    }
                    rooms.Add(exitRoom);
                    prevRoom.Connect(exitRoom);
                    break;
                case DungeonShapeType.TREEshape:
                    rooms.Add(startRoom);
                    int remainingRooms = _numberOfRooms - 2;
                    int reservableRooms = remainingRooms;
                    startRoom = TreeGenerator(ref reservableRooms, 
                        ref remainingRooms, true); //numberOfRooms - 2 om boom te garanderen
                    while (remainingRooms > 0)
                    {
                        foreach (Room r in rooms)
                        {
                            if (r.Neighbors.Count < 4)
                            {
                                remainingRooms--;
                                reservableRooms--;
                                Room room = TreeGenerator(ref reservableRooms, 
                                    ref remainingRooms);
                                r.Connect(room);
                                break;
                            }
                        }
                    }
                    break;
                case DungeonShapeType.RANDOMshape:
                    break;
                default:
                    throw new Exception("Invalid shape"); //volgens mij is dit niet nodig
            }
            throw new NotImplementedException();
        }
        
        public Room TreeGenerator(/*int maxDepth,*/ ref int reservableRooms, ref int remainingRooms, bool root = false) 
        {
            Room room = root ? new Room("RS", RoomType.STARTroom, 0) :  
                new Room("R" + remainingRooms,  RoomType.ORDINARYroom, RoomCapacity());
            rooms.Add(room);
            remainingRooms--;

            if (reservableRooms > 0 /*&& maxDepth > 0*/)
            {
                int childsCount = reservableRooms > 2 ? random.Next(4) 
                    : random.Next(reservableRooms + 1); //dit is als er max 4 connecties mogen
                reservableRooms -= childsCount;
                for (int i = 0; i < childsCount; i++)
                {
                    room.Connect(TreeGenerator(ref reservableRooms, ref remainingRooms));
                }
            }

            if (remainingRooms == 0 /* || maxDepth == 1 && remainingRooms == 1 */)
            {
                exitRoom = new Room("RE", RoomType.EXITroom, 0);
                exitRoom.Connect(room);
                rooms.Add(exitRoom);
            }
            

            return room;
        }

        void test() //Dit of onderaan bij laatste kinderen aanplakken.
        {
            while (true)
            {
                int element = random.Next(rooms.Count);
                Room room = rooms.ElementAt(element);
                if (room.Neighbors.Count < 4)
                {
                    exitRoom.Connect(rooms.ElementAt(element));
                    break;
                }
            }
        }

        private int RoomCapacity()
        {
            return random.Next(1, _maximumRoomCapacity + 1);
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
            throw new NotImplementedException();
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
