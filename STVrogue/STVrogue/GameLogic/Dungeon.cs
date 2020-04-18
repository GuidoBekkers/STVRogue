using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;

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
        Room startRoom;
        Room exitRoom ;
        
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
            throw new NotImplementedException();
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
