using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Asteroids
{
  public static class InputManager
  {
    public static List<GestureSample> Gestures;

    static InputManager()
    {
      Gestures = new List<GestureSample>();
    }

    public static KeyboardState GetKeyboardInput()
    {
      return Keyboard.GetState();
    }

    public static GestureType ProcessTouchInput()
    {
      Gestures.Clear();
      while (TouchPanel.IsGestureAvailable)
      {
        Gestures.Add(TouchPanel.ReadGesture());
      }

      return Gestures.Count > 0 ? Gestures[0].GestureType : GestureType.None;
    }
  }
}