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
    private KeyboardState lastState;

    private TimeSpan pausePosition;

    public SpriteBatch GameSprite => spriteBatch;

    public GameCore()
    {
      Instance = this;
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      graphics.PreferredBackBufferWidth = 1024;
      graphics.PreferredBackBufferHeight = 768;
    }

    protected override void Initialize()
    {
      base.Initialize();

      ParticleManager = new ParticleManager<ParticleState>(1024 * 20, ParticleState.UpdateParticle);
      TextManager = new TextManager();

      // setup static text entities
      var lt = new LivesText(new Vector2(20, 10));
      var sht = new ShieldsText(new Vector2(20,30));
      var st = new ScoreText(new Vector2(20, 50));
      TextManager.Add(lt);
      TextManager.Add(sht);
      TextManager.Add(st);


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

      KeyboardState state = Keyboard.GetState();

      if (state.IsKeyDown(Keys.Escape))
        Exit();

      // God Mode
      if (state.IsKeyDown(Keys.F12) && lastState.IsKeyUp(Keys.F12))
      {
        PlayerStatus.GodMode = !PlayerStatus.GodMode;
        if (PlayerStatus.GodMode)
          TextManager.Add(new GenericText("GodMode", new Vector2(Viewport.Width - 100, Viewport.Height - 20), "God Mode", Color.Red));
        else
          TextManager.Remove("GodMode");
      }

      if (state.IsKeyDown(Keys.P) && lastState.IsKeyUp(Keys.P))
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

      lastState = state;


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
