﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model.Data;

namespace Assets.Scripts.Model.Items
{
    /// <summary>
    /// A Node represents a point in grid space, allowing arcs to 
    /// connect between them.
    /// </summary>
    public class Node : IBoardItem
    {
        public Point Position { get; private set; }
        public bool IsEnabled { get { return true; } }
        public int Length { get { return 0; } }
        public Direction Direction { get { return Direction.None; } }

        private readonly Dictionary<Direction, Field> _fields =
            new Dictionary<Direction, Field>();
        public Dictionary<Direction, Field> Fields { get { return _fields; } }

        public IEnumerable<Field> Connections
        {
            get
            {
                return _fields.Values
                    .Where(field => field.HasArc && !field.Arc.IsPulled);
            }
        }
        
        public bool Final { get; set; }

        public Node(Point position, bool final = false)
        {
            Position = position;
            Final = final;
        }

        public bool HasConnection(Direction direction)
        {
            Field field;
            if (!_fields.TryGetValue(direction, out field)) return false;
            return !field.HasArc;
        }

        public void DisconnectField(Direction direction)
        {
            Fields[direction].DisconnectNodes();
        }
        
        public Direction GetDirection(Node end)
        {
            var diff = end.Position - Position;
            return diff.ToDirection;
        }

        public int GetDistance(Node end)
        {
            var diff = end.Position - Position;
            return diff.x != 0 ? diff.x : diff.y;
        }
    }
}