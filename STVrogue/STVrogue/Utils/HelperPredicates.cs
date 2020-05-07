using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using STVrogue.GameLogic;

namespace STVrogue.Utils
{
    /// <summary>
    /// Contain the forall and exists quantifiers, and some additional predicate
    /// operators.
    /// Also contains a bunch of useful helper predicates.
    /// </summary>
    public class HelperPredicates
    {
        /// <summary>
        /// A forall-quantifier over a collection.
        /// Example: Forall(C, x => x>=0) checks whether all integers in the collection
        /// C are non-negative.
        /// </summary>
        public static bool Forall<T>(ICollection<T> C, Predicate<T> p)
        {
            foreach (T x in C) // not using linq to make the code explicit for you
            {
                if (!p(x)) return false;
            }
            return true;
        }

        /// <summary>
        /// An exist-quantifier over a collection.
        /// Example: Exists(C, x => x>=0) checks whether there is a non-negative integer
        /// in the collection C.
        /// </summary>
        public static bool Exists<T>(ICollection<T> C, Predicate<T> p)
        {
            return !Forall(C, x => !p(x));
        }
        
        /// <summary>
        /// A forall-quantifier over an array.
        /// Example: Forall(a, i => a[i]==0) checks whether for all valid indices i
        /// of the array a, a[i]==0.
        /// </summary>
        public static bool Forall<T>(T[] a, Predicate<int> P) {
            for (int k = 0; k < a.Length; k++)
            {
                if (!P(k)) return false;
            }
            return true;
        }
        
        /// <summary>
        /// An exists-quatifier over an array.
        /// Example: Exists(a, i => a[i]==0) checks whether for all valid indices i
        /// of the array a, a[i]==0.
        /// </summary>
        public static bool Exists<T>(T[] a, Predicate<int> P)
        {
            return !Forall(a, k => !P(k)) ;
        }

        // just for demonstrating the syntax to you:
        private void test()
        {
            List<int> C = new List<int>();
            Forall(C, x=>x >= 0);
            Forall(C, (int x)=>x >= 0) ; // if you need to explicitly specify the type of x
            Exists(C, x=>x < 0);
            Exists(C, (int x)=>x < 0) ;
        }

        /// <summary>
        /// Check if p implies q (which is equivalent to !p or q).
        /// </summary>
        public static bool Imp(bool p, bool q)
        {
            return !p || q;
        }

        /// <summary>
        /// Check if all rooms in a dungeon are reachable from the start-room.
        /// </summary>
        public static bool AllReachableFromStart(Dungeon dungeon)
        {
            return Forall(dungeon.Rooms, room => dungeon.StartRoom.CanReach(room));
        }

        /// <summary>
        /// Check if the given dungeon has a unique start room and a unique exit room.
        /// </summary>
        public static bool HasUniqueStartAndExit(Dungeon dungeon)
        {
            return dungeon.StartRoom != dungeon.ExitRoom
                   && dungeon.StartRoom.RoomType == RoomType.STARTroom
                   && dungeon.ExitRoom.RoomType == RoomType.EXITroom
                   // start and exit rooms should be in the dungeon:
                   && Exists(dungeon.Rooms, room => room == dungeon.StartRoom)
                   && Exists(dungeon.Rooms, room => room == dungeon.ExitRoom)
                   // start is the only start-room, and exit is the only exit-room:
                   && Forall(dungeon.Rooms, room => Imp(room.RoomType == RoomType.STARTroom, room == dungeon.StartRoom))
                   && Forall(dungeon.Rooms, room => Imp(room.RoomType == RoomType.EXITroom, room == dungeon.ExitRoom))
                ;
        }

        /// <summary>
        /// Check if all ids in the given dungeon are indeed unique.
        /// </summary>
        public static bool AllIdsAreUnique(Dungeon dungeon)
        {
            // collect all the ids"
            var ids = new List<string>();
            foreach(Room room in dungeon.Rooms)
            {
                ids.Add(room.Id);
                ids.AddRange(from m in room.Monsters select m.Id);
                ids.AddRange(from i in room.Items select i.Id);
                
            }
            var ids_ = ids.ToArray();
            // unique if forall i,k: ids[i]=ids[k]  ==>  i=k :
            return Forall(ids_, i =>
                   Forall(ids_, k => Imp(ids_[i] == ids_[k], i == k)));
        }

        /// <summary>
        /// Check if the given dungeon forms a list starting at the start-room.
        /// </summary>
        public static bool IsLinear(Dungeon dungeon)
        {
            if (!HasUniqueStartAndExit(dungeon)) return false;
            if (!AllReachableFromStart(dungeon)) return false;
            // all rooms are reachable from the start-room
            // It is sufficient to check that the start and exit room has exactly one neighbor,
            // and all the other rooms have exactly two.
            // If one has less neighbor, or link to itself, some room will be unreachable, which
            // contradict the previous assumotion.
            // More generally, if there is a cycle in the graph, since all rooms have at most two
            // neighbors, it will cause some room to be unreachable.
            return Forall(dungeon.Rooms, room =>
                // start and exit rooms should have exactly one neighbor, which is not itself:
                Imp(room == dungeon.StartRoom || room == dungeon.ExitRoom,
                    room.Neighbors.Count == 1
                    && room.Neighbors.First() != room
                )
                // other rooms should have exactly two neighbors:
                &&
                Imp(room != dungeon.StartRoom && room != dungeon.ExitRoom,
                    room.Neighbors.Count == 2
                )
            );
        }

        /// <summary>
        /// Check if the given dungeon forms a tree rooted at the start-room.
        /// </summary>
        public static bool IsTree(Dungeon dungeon)
        {
            if (!HasUniqueStartAndExit(dungeon)) return false;
            if (!AllReachableFromStart(dungeon)) return false;
            // all rooms are reachable from the start-room
            // we check if there is a cycle using the isCyclic function:
            
            //Mark all the rooms as not visited
            Dictionary<Room, bool> visited = new Dictionary<Room, bool>();
            foreach (Room r in dungeon.Rooms)
                visited[r] = false;
            
            if (isCyclic(dungeon.StartRoom, visited, null))
                return false;

            return true;
        }

        public static bool isCyclic(Room room, Dictionary<Room, bool> visited, Room parent)
        {
            //Mark the current room as visited
            visited[room] = true;
            Room i;

            //Recur for all neighbor rooms to this room
            foreach (Room r in room.Neighbors)
            {
                i = r;
                
                //if a neighbor is not visited, then recur for that neighbor
                if (!visited[i])
                {
                    if (isCyclic(i, visited, room))
                        return true;
                }
                //if a neighbor is visited and not parent of the
                //current room, then there is a cycle
                else if (i != parent)
                    return true;
            }
            return false;
        }

        

    }


}