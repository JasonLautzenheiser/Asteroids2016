#region File Description
//-----------------------------------------------------------------------------
// InputManager.cs
//
// Happily based off of and refactored from Monogame sample
// https://github.com/CartBlanche/MonoGame-Samples/blob/master/RolePlayingGame/InputManager.cs
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
  public static class InputManager
  {
    #region Constants

    // The value of an analog control that reads as a "pressed button".
    private const float analogLimit = 0.5f;

    #endregion

    #region Initialization

    // Initializes the default control keys for all actions.
    public static void Initialize()
    {
      ResetActionMaps();
    }

    #endregion

    #region Updating

    // Updates the keyboard and gamepad control states.
    public static void Update()
    {
      // update the keyboard state
      previousKeyboardState = currentKeyboardState;
      currentKeyboardState = Keyboard.GetState();

      // update the gamepad state
      previousGamePadState = currentGamePadState;
      currentGamePadState = GamePad.GetState(PlayerIndex.One);
    }

    #endregion

    #region Action Enumeration

    public enum Action
    {
      Thrust,
      Brake,
      RotateLeft,
      RotateRight,
      Fire,
      Shield,
      Nuke,
      Pause,
      ExitGame, 
      GodMode,
      TotalActionCount
    }


    private static readonly string[] actionNames =
    {
      "Thrust",
      "Brake",
      "Rotate Left",
      "Rotate Right",
      "Fire",
      "Shield",
      "Nuke",
      "Pause",
      "God Mode",
      "Exit Game"
    };

    public static string GetActionName(Action action)
    {
      var index = (int) action;

      if ((index < 0) || (index > actionNames.Length))
      {
        throw new ArgumentException("action");
      }

      return actionNames[index];
    }

    #endregion

    #region Support Types

    // GamePad controls expressed as one type, unified with button semantics.
    public enum GamePadButtons
    {
      Start,
      Back,
      A,
      B,
      X,
      Y,
      Up,
      Down,
      Left,
      Right,
      LeftShoulder,
      RightShoulder,
      LeftTrigger,
      RightTrigger
    }


    // A combination of gamepad and keyboard keys mapped to a particular action.
    public class ActionMap
    {
      // List of GamePad controls to be mapped to a given action.
      public List<GamePadButtons> gamePadButtons = new List<GamePadButtons>();


      // List of Keyboard controls to be mapped to a given action.
      public List<Keys> keyboardKeys = new List<Keys>();
    }

    #endregion

    #region Keyboard Data

    // The state of the keyboard as of the last update.
    private static KeyboardState currentKeyboardState;

    // The state of the keyboard as of the last update.
    public static KeyboardState CurrentKeyboardState => currentKeyboardState;


    // The state of the keyboard as of the previous update.
    private static KeyboardState previousKeyboardState;


    // Check if a key is pressed.
    public static bool IsKeyPressed(Keys key)
    {
      return currentKeyboardState.IsKeyDown(key);
    }


    // Check if a key was just pressed in the most recent update.
    public static bool IsKeyTriggered(Keys key)
    {
      return currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
    }

    #endregion

    #region GamePad Data

    // The state of the gamepad as of the last update.
    private static GamePadState currentGamePadState;

    // The state of the gamepad as of the last update.
    public static GamePadState CurrentGamePadState => currentGamePadState;


    // The state of the gamepad as of the previous update.
    private static GamePadState previousGamePadState;

    #region GamePadButton Pressed Queries

    // Check if the gamepad's Start button is pressed.
    public static bool IsGamePadStartPressed()
    {
      return currentGamePadState.Buttons.Start == ButtonState.Pressed;
    }


    // Check if the gamepad's Back button is pressed.
    public static bool IsGamePadBackPressed()
    {
      return currentGamePadState.Buttons.Back == ButtonState.Pressed;
    }


    // Check if the gamepad's A button is pressed.
    public static bool IsGamePadAPressed()
    {
      return currentGamePadState.Buttons.A == ButtonState.Pressed;
    }


    // Check if the gamepad's B button is pressed.
    public static bool IsGamePadBPressed()
    {
      return currentGamePadState.Buttons.B == ButtonState.Pressed;
    }


    // Check if the gamepad's X button is pressed.
    public static bool IsGamePadXPressed()
    {
      return currentGamePadState.Buttons.X == ButtonState.Pressed;
    }


    // Check if the gamepad's Y button is pressed.
    public static bool IsGamePadYPressed()
    {
      return currentGamePadState.Buttons.Y == ButtonState.Pressed;
    }


    // Check if the gamepad's LeftShoulder button is pressed.
    public static bool IsGamePadLeftShoulderPressed()
    {
      return currentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed;
    }


    // Check if the gamepad's RightShoulder button is pressed.
    public static bool IsGamePadRightShoulderPressed()
    {
      return currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed;
    }


    // Check if Up on the gamepad's directional pad is pressed.
    public static bool IsGamePadDPadUpPressed()
    {
      return currentGamePadState.DPad.Up == ButtonState.Pressed;
    }


    // Check if Down on the gamepad's directional pad is pressed.
    public static bool IsGamePadDPadDownPressed()
    {
      return currentGamePadState.DPad.Down == ButtonState.Pressed;
    }


    // Check if Left on the gamepad's directional pad is pressed.
    public static bool IsGamePadDPadLeftPressed()
    {
      return currentGamePadState.DPad.Left == ButtonState.Pressed;
    }


    // Check if Right on the gamepad's directional pad is pressed.
    public static bool IsGamePadDPadRightPressed()
    {
      return currentGamePadState.DPad.Right == ButtonState.Pressed;
    }


    // Check if the gamepad's left trigger is pressed.
    public static bool IsGamePadLeftTriggerPressed()
    {
      return currentGamePadState.Triggers.Left > analogLimit;
    }


    // Check if the gamepad's right trigger is pressed.
    public static bool IsGamePadRightTriggerPressed()
    {
      return currentGamePadState.Triggers.Right > analogLimit;
    }


    // Check if Up on the gamepad's left analog stick is pressed.
    public static bool IsGamePadLeftStickUpPressed()
    {
      return currentGamePadState.ThumbSticks.Left.Y > analogLimit;
    }


    // Check if Down on the gamepad's left analog stick is pressed.
    public static bool IsGamePadLeftStickDownPressed()
    {
      return -1f*currentGamePadState.ThumbSticks.Left.Y > analogLimit;
    }

    // Check if Left on the gamepad's left analog stick is pressed.
    public static bool IsGamePadLeftStickLeftPressed()
    {
      return -1f*currentGamePadState.ThumbSticks.Left.X > analogLimit;
    }

    // Check if Right on the gamepad's left analog stick is pressed.
    public static bool IsGamePadLeftStickRightPressed()
    {
      return currentGamePadState.ThumbSticks.Left.X > analogLimit;
    }

    // Check if the GamePadKey value specified is pressed.
    private static bool IsGamePadButtonPressed(GamePadButtons gamePadKey)
    {
      switch (gamePadKey)
      {
        case GamePadButtons.Start:
          return IsGamePadStartPressed();

        case GamePadButtons.Back:
          return IsGamePadBackPressed();

        case GamePadButtons.A:
          return IsGamePadAPressed();

        case GamePadButtons.B:
          return IsGamePadBPressed();

        case GamePadButtons.X:
          return IsGamePadXPressed();

        case GamePadButtons.Y:
          return IsGamePadYPressed();

        case GamePadButtons.LeftShoulder:
          return IsGamePadLeftShoulderPressed();

        case GamePadButtons.RightShoulder:
          return IsGamePadRightShoulderPressed();

        case GamePadButtons.LeftTrigger:
          return IsGamePadLeftTriggerPressed();

        case GamePadButtons.RightTrigger:
          return IsGamePadRightTriggerPressed();

        case GamePadButtons.Up:
          return IsGamePadDPadUpPressed() || IsGamePadLeftStickUpPressed();

        case GamePadButtons.Down:
          return IsGamePadDPadDownPressed() || IsGamePadLeftStickDownPressed();

        case GamePadButtons.Left:
          return IsGamePadDPadLeftPressed() || IsGamePadLeftStickLeftPressed();

        case GamePadButtons.Right:
          return IsGamePadDPadRightPressed() || IsGamePadLeftStickRightPressed();
      }

      return false;
    }

    #endregion

    #region GamePadButton Triggered Queries

    // Check if the gamepad's Start button was just pressed.
    public static bool IsGamePadStartTriggered()
    {
      return (currentGamePadState.Buttons.Start == ButtonState.Pressed) && (previousGamePadState.Buttons.Start == ButtonState.Released);
    }

    //Check if the gamepad's Back button was just pressed.
    public static bool IsGamePadBackTriggered()
    {
      return (currentGamePadState.Buttons.Back == ButtonState.Pressed) && (previousGamePadState.Buttons.Back == ButtonState.Released);
    }

    // Check if the gamepad's A button was just pressed.
    public static bool IsGamePadATriggered()
    {
      return (currentGamePadState.Buttons.A == ButtonState.Pressed) && (previousGamePadState.Buttons.A == ButtonState.Released);
    }

    // Check if the gamepad's B button was just pressed.
    public static bool IsGamePadBTriggered()
    {
      return (currentGamePadState.Buttons.B == ButtonState.Pressed) && (previousGamePadState.Buttons.B == ButtonState.Released);
    }

    // Check if the gamepad's X button was just pressed.
    public static bool IsGamePadXTriggered()
    {
      return (currentGamePadState.Buttons.X == ButtonState.Pressed) && (previousGamePadState.Buttons.X == ButtonState.Released);
    }

    // Check if the gamepad's Y button was just pressed.
    public static bool IsGamePadYTriggered()
    {
      return (currentGamePadState.Buttons.Y == ButtonState.Pressed) && (previousGamePadState.Buttons.Y == ButtonState.Released);
    }

    // Check if the gamepad's LeftShoulder button was just pressed.
    public static bool IsGamePadLeftShoulderTriggered()
    {
      return (currentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed) && (previousGamePadState.Buttons.LeftShoulder == ButtonState.Released);
    }

    // Check if the gamepad's RightShoulder button was just pressed.
    public static bool IsGamePadRightShoulderTriggered()
    {
      return (currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed) && (previousGamePadState.Buttons.RightShoulder == ButtonState.Released);
    }

    // Check if Up on the gamepad's directional pad was just pressed.
    public static bool IsGamePadDPadUpTriggered()
    {
      return (currentGamePadState.DPad.Up == ButtonState.Pressed) && (previousGamePadState.DPad.Up == ButtonState.Released);
    }

    // Check if Down on the gamepad's directional pad was just pressed.
    public static bool IsGamePadDPadDownTriggered()
    {
      return (currentGamePadState.DPad.Down == ButtonState.Pressed) && (previousGamePadState.DPad.Down == ButtonState.Released);
    }

    // Check if Left on the gamepad's directional pad was just pressed.
    public static bool IsGamePadDPadLeftTriggered()
    {
      return (currentGamePadState.DPad.Left == ButtonState.Pressed) && (previousGamePadState.DPad.Left == ButtonState.Released);
    }

    // Check if Right on the gamepad's directional pad was just pressed.
    public static bool IsGamePadDPadRightTriggered()
    {
      return (currentGamePadState.DPad.Right == ButtonState.Pressed) && (previousGamePadState.DPad.Right == ButtonState.Released);
    }

    // Check if the gamepad's left trigger was just pressed.
    public static bool IsGamePadLeftTriggerTriggered()
    {
      return (currentGamePadState.Triggers.Left > analogLimit) && (previousGamePadState.Triggers.Left < analogLimit);
    }

    // Check if the gamepad's right trigger was just pressed.
    public static bool IsGamePadRightTriggerTriggered()
    {
      return (currentGamePadState.Triggers.Right > analogLimit) && (previousGamePadState.Triggers.Right < analogLimit);
    }

    // Check if Up on the gamepad's left analog stick was just pressed.
    public static bool IsGamePadLeftStickUpTriggered()
    {
      return (currentGamePadState.ThumbSticks.Left.Y > analogLimit) && (previousGamePadState.ThumbSticks.Left.Y < analogLimit);
    }

    // Check if Down on the gamepad's left analog stick was just pressed.
    public static bool IsGamePadLeftStickDownTriggered()
    {
      return (-1f*currentGamePadState.ThumbSticks.Left.Y > analogLimit) && (-1f*previousGamePadState.ThumbSticks.Left.Y < analogLimit);
    }

    // Check if Left on the gamepad's left analog stick was just pressed.
    public static bool IsGamePadLeftStickLeftTriggered()
    {
      return (-1f*currentGamePadState.ThumbSticks.Left.X > analogLimit) && (-1f*previousGamePadState.ThumbSticks.Left.X < analogLimit);
    }

    // Check if Right on the gamepad's left analog stick was just pressed.
    public static bool IsGamePadLeftStickRightTriggered()
    {
      return (currentGamePadState.ThumbSticks.Left.X > analogLimit) && (previousGamePadState.ThumbSticks.Left.X < analogLimit);
    }

    // Check if the GamePadKey value specified was pressed this frame.
    private static bool IsGamePadButtonTriggered(GamePadButtons gamePadKey)
    {
      switch (gamePadKey)
      {
        case GamePadButtons.Start:
          return IsGamePadStartTriggered();

        case GamePadButtons.Back:
          return IsGamePadBackTriggered();

        case GamePadButtons.A:
          return IsGamePadATriggered();

        case GamePadButtons.B:
          return IsGamePadBTriggered();

        case GamePadButtons.X:
          return IsGamePadXTriggered();

        case GamePadButtons.Y:
          return IsGamePadYTriggered();

        case GamePadButtons.LeftShoulder:
          return IsGamePadLeftShoulderTriggered();

        case GamePadButtons.RightShoulder:
          return IsGamePadRightShoulderTriggered();

        case GamePadButtons.LeftTrigger:
          return IsGamePadLeftTriggerTriggered();

        case GamePadButtons.RightTrigger:
          return IsGamePadRightTriggerTriggered();

        case GamePadButtons.Up:
          return IsGamePadDPadUpTriggered() ||
                 IsGamePadLeftStickUpTriggered();

        case GamePadButtons.Down:
          return IsGamePadDPadDownTriggered() ||
                 IsGamePadLeftStickDownTriggered();

        case GamePadButtons.Left:
          return IsGamePadDPadLeftTriggered() ||
                 IsGamePadLeftStickLeftTriggered();

        case GamePadButtons.Right:
          return IsGamePadDPadRightTriggered() ||
                 IsGamePadLeftStickRightTriggered();
      }

      return false;
    }

    #endregion

    #endregion

    #region Action Mapping

    // The action mappings for the game.
    private static ActionMap[] actionMaps;

    public static ActionMap[] ActionMaps => actionMaps;


    // Reset the action maps to their default values.
    private static void ResetActionMaps()
    {
      actionMaps = new ActionMap[(int) Action.TotalActionCount];


      actionMaps[(int) Action.ExitGame] = new ActionMap();
      actionMaps[(int) Action.ExitGame].keyboardKeys.Add(Keys.Escape);
      actionMaps[(int) Action.ExitGame].gamePadButtons.Add(GamePadButtons.Back);

      actionMaps[(int) Action.Thrust] = new ActionMap();
      actionMaps[(int) Action.Thrust].keyboardKeys.Add(Keys.Up);
      actionMaps[(int) Action.Thrust].gamePadButtons.Add(GamePadButtons.Up);

      actionMaps[(int) Action.Brake] = new ActionMap();
      actionMaps[(int) Action.Brake].keyboardKeys.Add(Keys.Down);
      actionMaps[(int) Action.Brake].gamePadButtons.Add(GamePadButtons.Down);

      actionMaps[(int) Action.Shield] = new ActionMap();
      actionMaps[(int) Action.Shield].keyboardKeys.Add(Keys.S);
      actionMaps[(int) Action.Shield].gamePadButtons.Add(GamePadButtons.A);

      actionMaps[(int) Action.Nuke] = new ActionMap();
      actionMaps[(int) Action.Nuke].keyboardKeys.Add(Keys.N);
      actionMaps[(int) Action.Nuke].gamePadButtons.Add(GamePadButtons.B);

      actionMaps[(int) Action.Fire] = new ActionMap();
      actionMaps[(int) Action.Fire].keyboardKeys.Add(Keys.Space);
      actionMaps[(int) Action.Fire].gamePadButtons.Add(GamePadButtons.RightTrigger);

      actionMaps[(int) Action.Pause] = new ActionMap();
      actionMaps[(int) Action.Pause].keyboardKeys.Add(Keys.P);
      actionMaps[(int) Action.Pause].gamePadButtons.Add(GamePadButtons.Start);

      actionMaps[(int) Action.RotateLeft] = new ActionMap();
      actionMaps[(int) Action.RotateLeft].keyboardKeys.Add(Keys.Left);
      actionMaps[(int) Action.RotateLeft].gamePadButtons.Add(GamePadButtons.Left);

      actionMaps[(int) Action.RotateRight] = new ActionMap();
      actionMaps[(int) Action.RotateRight].keyboardKeys.Add(Keys.Right);
      actionMaps[(int) Action.RotateRight].gamePadButtons.Add(GamePadButtons.Right);

      actionMaps[(int) Action.GodMode] = new ActionMap();
      actionMaps[(int) Action.GodMode].keyboardKeys.Add(Keys.F12);
    }

    // Check if an action has been pressed.
    public static bool IsActionPressed(Action action)
    {
      return IsActionMapPressed(actionMaps[(int) action]);
    }

    // Check if an action was just performed in the most recent update.
    public static bool IsActionTriggered(Action action)
    {
      return IsActionMapTriggered(actionMaps[(int) action]);
    }

    // Check if an action map has been pressed.
    private static bool IsActionMapPressed(ActionMap actionMap)
    {
      if (actionMap.keyboardKeys.Any(t => IsKeyPressed(t))) {
        return true;
      }
      if (currentGamePadState.IsConnected)
      {
        return actionMap.gamePadButtons.Any(t => IsGamePadButtonPressed(t));
      }
      return false;
    }


    // Check if an action map has been triggered this frame.
    private static bool IsActionMapTriggered(ActionMap actionMap)
    {
      if (actionMap.keyboardKeys.Any(t => IsKeyTriggered(t))) {
        return true;
      }
      if (currentGamePadState.IsConnected)
      {
        return actionMap.gamePadButtons.Any(t => IsGamePadButtonTriggered(t));
      }
      return false;
    }

    #endregion
  }
}