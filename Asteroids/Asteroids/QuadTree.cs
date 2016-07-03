using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Asteroids
{
  public class QuadTree
  {
    private const int MAX_OBJECTS = 10;

    private int level;
    private List<Entity> objects;
    private Rectangle bounds;
    private QuadTree[] nodes;

    public QuadTree(int level, Rectangle bounds)
    {
      this.level = level;
      this.bounds = bounds;
      objects = new List<Entity>();
      nodes = new QuadTree[4];
    }

    public void Clear()
    {
      objects.Clear();
      for (int i = 0; i < nodes.Length; i++)
      {
        if (nodes[i] == null) continue;
        nodes[i].Clear();
        nodes[i] = null;
      }
    }

    public void Split()
    {
      int subWidth = bounds.Width/2;
      int subHeight = bounds.Height/2;
      int x = bounds.X;
      int y = bounds.Y;

      nodes[0] = new QuadTree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
      nodes[1] = new QuadTree(level + 1, new Rectangle(x, y, subWidth, subHeight));
      nodes[2] = new QuadTree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
      nodes[3] = new QuadTree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
    }

    private int getIndex(Rectangle pRect)
    {
      int index = -1;
      double verticalMidpoint = bounds.X + bounds.Width/2;
      double horizontalMidpoint = bounds.Y + bounds.Height/2;

      bool topQuadrant = pRect.Y < horizontalMidpoint && pRect.Y + pRect.Height < horizontalMidpoint;
      bool bottomQuadrant = pRect.Y > horizontalMidpoint;

      if (pRect.X < verticalMidpoint && pRect.X + pRect.Width < verticalMidpoint)
      {
        if (topQuadrant)
          index = 1;
        else if (bottomQuadrant)
          index = 2;
      }
      else if (pRect.X > verticalMidpoint)
      {
        if (topQuadrant)
          index = 0;
        else if (bottomQuadrant)
          index = 3;
      }
      return index;
    }

    public void Insert(Entity pObject)
    {
      if (nodes[0] != null)
      {
        int index = getIndex(pObject.GetBoundingRectangle());
        if (index != -1)
        {
          nodes[index].Insert(pObject);
          return;
        }
      }
      objects.Add(pObject);

      if (objects.Count > MAX_OBJECTS)
      {
        if (nodes[0] == null)
          Split();

        int i = 0;
        while (i < objects.Count)
        {
          int index = getIndex(objects[i].GetBoundingRectangle());
          if (index != -1)
          {
            var tObject = objects[i];
            objects.Remove(objects[i]);
            nodes[index].Insert(tObject);
          }
          else
          {
            i++;
          }
        }
      }
    }

    public List<Entity> Retrieve(List<Entity> returnObjects, Rectangle pRect)
    {
      int index = getIndex(pRect);

      if (index != -1 && nodes[0] != null)
        nodes[index].Retrieve(returnObjects, pRect);

      returnObjects.AddRange(objects);

      return returnObjects;
    }
  }
}
