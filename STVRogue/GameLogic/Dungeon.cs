using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STVRogue.GameLogic
{

    

    public class Dungeon
    {
        public Node startNode;
        public Node exitNode;
        public uint dungeonLevel;
        
        /// To create a new dungeon with the specified level.
        public Dungeon(uint level)
        {
            Logger.log($"Creating a dungeon of level {level}");
            dungeonLevel = level;
            throw new NotImplementedException();
        }
        
        public List<Node> shortestpath(Node u, Node v) { throw new NotImplementedException(); }
        public void destroy(Bridge b) {
            Logger.log($"Destroying the bridge {b.id}");
            throw new NotImplementedException();
        }

        public uint level(Node d) { throw new NotImplementedException(); }
    } 

    public class Node
    {
        public String id;
        public List<Node> neighbors = new List<Node>();
        public List<Pack> packs = new List<Pack>();
        public List<Item> items = new List<Item>();

        public Node() { }
        public Node(String id) { this.id = id; }

        /* Execute a fight between the player and the packs in this node.
         * Such a fight can take multiple rounds as describe in the Project Document.
         * When during these rounds a monster's HP becomes 0 it is removed from its pack. 
         * And when a pack has no more member, it is removed from the node.
         * A fight terminates when either the node has no more monster-pack, or when
         * the player's HP is reduced to 0. 
         */
        public void fight(Player player)
        {
            throw new NotImplementedException();
        }
    }

    public class Bridge : Node
    {
        public Bridge(String id) : base(id) {  }
    }

}
