using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Entities;
using Asteroids.TextEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Managers
{
  public class TextManager : IManager
  {
    private static bool isUpdating;
    private static List<TextEntity> textEntities;
    private static List<TextEntity> addedEntities;

    public TextManager()
    {
      textEntities = new List<TextEntity>();
      addedEntities = new List<TextEntity>();
    }

    public void Add(Entity entity)
    {
      if (entity.GetType().BaseType != typeof(TextEntity)) return;

      var te = (TextEntity) entity;

      if (!isUpdating)
        textEntities.Add(te);
      else
        addedEntities.Add(te);
    }

    public void Draw(SpriteBatch batch)
    {
      foreach (var te in textEntities.OrderByDescending(p=>p.DrawPriority))
      {
        te.Draw(batch);
      }
    }

    public void Update()
    {
      isUpdating = true;
      foreach (var textEntity in textEntities)
      {
        if (textEntity.TempText)
        {
          var elapsed = DateTime.Now - textEntity.DateOfCreation;
          if (elapsed > TimeSpan.FromSeconds(textEntity.TTL))
            textEntity.ReadyToRemove = true;
        }

        textEntity.Update();
      }
      isUpdating = false;

      foreach (var textEntity in addedEntities)
      {
        textEntities.Add(textEntity);
      }

      textEntities.RemoveAll(p => p.ReadyToRemove);

      addedEntities.Clear();
    }

    public void Remove(Type typeToRemove)
    {
      textEntities.RemoveAll(p => p.GetType() == typeToRemove);
    }
  }
}
