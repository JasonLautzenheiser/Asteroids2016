using System;
using Asteroids.Entities;
using Asteroids.Entities.Player;
using Asteroids.Managers;
using Asteroids.Powerups;
using Asteroids.TextEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Asteroids
{
  public class GameCore : Game
  {
    public static GameCore Instance { get; private set; }
    public static Viewport Viewport => Instance.GraphicsDevice.Viewport;
    public static Vector2 ScreenSize => new Vector2(Viewport.Width, Viewport.Height);
    public static GameTime GameTime { get; private set; }
    public static ParticleManager<ParticleState> ParticleManager { get; private set; }
    public static TextManager TextManager { get; private set; }

    Song song;

    private readonly GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    private bool gamePaused;

    private TimeSpan pausePosition;

    public SpriteBatch GameSprite => spriteBatch;

    public GameCore()
    {
      Instance = this;
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      graphics.PreferredBackBufferWidth = 1920;
      graphics.PreferredBackBufferHeight = 1080;
    }

    protected override void Initialize()
    {
      InputManager.Initialize();

      base.Initialize();

      IsFixedTimeStep = false;
      

      ParticleManager = new ParticleManager<ParticleState>(1024 * 20, ParticleState.UpdateParticle);
      TextManager = new TextManager();

      // setup static text entities
      var lt = new LivesText(new Vector2(20, 10));
      var sht = new ShieldsText(new Vector2(20,50));
      var st = new ScoreText(new Vector2(20, 30));
      var nt = new NukeText(new Vector2(20, 70));
      TextManager.Add(lt);
      TextManager.Add(sht);
      TextManager.Add(st);
      TextManager.Add(nt);

      var fps = new FrameRateText(new Vector2(Viewport.Width - 100, 10));
      TextManager.Add(fps);



      //      quadTree = new QuadTree(0, GraphicsDevice.Viewport.Bounds);

      EntityManager.Add(new Starfield());
      EntityManager.Add(Ship.Instance);

      song = Song.FromUri("Music", new Uri("content/Music.mp3", UriKind.Relative));
      MediaPlayer.IsRepeating = true;
      MediaPlayer.Volume = .3f;
      MediaPlayer.Play(song);
    }

    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      Art.Load(Content);
      SoundEffects.Load(Content);
      Font.Load(Content);
    }

    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }


    protected override void Update(GameTime gameTime)
    {
      GameTime = gameTime;

      InputManager.Update();

      if (InputManager.IsActionTriggered(InputManager.Action.ExitGame))
        Exit();

      // God Mode
      if (InputManager.IsActionTriggered(InputManager.Action.GodMode))
      {
        PlayerStatus.GodMode = !PlayerStatus.GodMode;
        if (PlayerStatus.GodMode)
          TextManager.Add(new GenericText("GodMode", new Vector2(Viewport.Width - 100, Viewport.Height - 20), "God Mode", Color.Red));
        else
          TextManager.Remove("GodMode");
      }



      if (PlayerStatus.GodMode)
      {
        if (InputManager.IsActionTriggered(InputManager.Action.GMNukePower))
        {
          PowerUpSpawner.Create(new Vector2(Viewport.Width/2 - 65, 80), PowerUpTypes.Nuke);
        }
      }


      if (InputManager.IsActionTriggered(InputManager.Action.Pause))
      {
        gamePaused = !gamePaused;
        if (gamePaused)
        {
          TextManager.Add(new PauseText(new Vector2(Viewport.Width / 2 - 65, 10)));
          pausePosition = MediaPlayer.PlayPosition;
          MediaPlayer.Stop();
        }
        else
        {
          TextManager.Remove(typeof(PauseText));
          MediaPlayer.Play(song);
        }
      }

      if (!gamePaused)
      {
        EntityManager.Update();
        EnemySpawner.Update();
        ParticleManager.Update();
        PlayerStatus.Update();
        TextManager.Update();
      }



      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
      ParticleManager.Draw(spriteBatch);
      spriteBatch.End();

      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
      EntityManager.Draw(spriteBatch);
      spriteBatch.End();

      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
      TextManager.Draw(spriteBatch);
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
