﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Utmark.Engine.Camera;
using Utmark.Engine.Settings.Screen;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;
using Utmark_ECS.Map;
using Utmark_ECS.Systems;
using Utmark_ECS.Systems.EventHandlers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.Input;

using Utmark_ECS.Utilities;
using static Utmark_ECS.Enums.EventTypeEnum;
using static Utmark_ECS.Enums.ItemTypeEnum;
using static Utmark_ECS.Enums.TileTypeEnum;

namespace Utmark
{

    public class Main : Game
    {
        private ActionHandler _actionHandler;
        private GraphicsDeviceManager _graphics;
        private ScreenSettings _screenSettings;
        private SpriteFont _font;
        private SpriteFont _runes;
        private DebugLog _debugLog;
        private TileMap _tileMap;
        private InputMapper _inputMapper;
        //private MovementHandler _movementHandler;
        private ComponentManager _componentManager;
        private Rectangle _spriteSourceRect;
        private RenderSystem _renderSystem;
        private InputSystem _inputSystem;
        private SpriteBatch _spriteBatch;
        private CollisionHandler _collisionDetectionSystem;
        private MessageLog _messageLog;
        private EntityManager _entityManager;
        private SpatialGrid _spatialGrid;
        private EventManager _eventManager;
        Dictionary<string, Rectangle> _sprites;
        private Texture2D _spriteSheet;
        private Tile _grass;
        private Camera2D _camera;
        private Vector2 _cameraPosition;
        private Entity player;
        private Entity nPC;
        private Entity item;
        private Entity item2;
        private Entity item3;
        private int _tileSize;
        private ResourceManager _resourceManager;
     
        public Main()
        {
            _messageLog = new MessageLog(_font);
            _eventManager = new EventManager();
            _inputMapper = new InputMapper(_eventManager);
            _spatialGrid = new SpatialGrid(16, _eventManager);
            _graphics = new GraphicsDeviceManager(this);
            _screenSettings = new ScreenSettings(_graphics);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
            _tileSize = GameConstants.GridSize;

        }

        protected override void Initialize()
        {

            _actionHandler = new ActionHandler(_eventManager, _inputMapper);
            // _runes = Content.Load<SpriteFont>("runeFont");

            _font = Content.Load<SpriteFont>("spriteFont");
            _camera = new Camera2D(GraphicsDevice.Viewport);
            _spriteSourceRect = new Rectangle(0, 0, 16, 16);
            _entityManager = new EntityManager(_eventManager, _spatialGrid);
            _componentManager = new ComponentManager(_entityManager, _eventManager); // Then pass it to ComponentManager
            _collisionDetectionSystem = new CollisionHandler(_eventManager, _componentManager);
            _debugLog = new DebugLog(_font, _eventManager);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //_movementHandler = new MovementHandler(_eventManager, _componentManager, _spatialGrid);
            _sprites = new Dictionary<string, Rectangle>
            {
                {"tallGrass", new Rectangle(0, 0, 16, 16)},
                {"shortGrass", new Rectangle(16, 0, 16, 16)},
                {"fern", new Rectangle(32, 0, 16, 16)},
                {"player", new Rectangle(16, 80, 16, 16)},
                {"knife", new Rectangle(32, 64, 16, 16)}
            };
            _spriteSheet = Content.Load<Texture2D>("Images/classic_roguelike16x16");
            _grass = new Tile(TileType.Soil, "tallGrass", Color.DarkOliveGreen, null);
            _tileMap = new TileMap(64, 64, _spatialGrid, _grass);
            _resourceManager = new ResourceManager { SpriteSheet = _spriteSheet, Sprites = _sprites };
            _renderSystem = new RenderSystem(_componentManager, _spriteBatch, _tileMap, _camera, _resourceManager);
            _inputSystem = new InputSystem(_componentManager, _eventManager, _inputMapper);

            base.Initialize();
            _screenSettings.InitializeDefaults();
        }

        protected override void LoadContent()
        {
            nPC = _entityManager.CreateEntity();
            _componentManager.AddComponent(nPC, new PositionComponent(new Vector2(16, 0)));
            _componentManager.AddComponent(nPC, new RenderComponent(_spriteSheet, _sprites["player"], Color.Red, 0f, 1f));
            _componentManager.AddComponent(nPC, new VelocityComponent(new Vector2(0, 0)));
            _componentManager.AddComponent(nPC, new NameComponent("NPCname"));
            item = _entityManager.CreateEntity();
            _componentManager.AddComponent(item, new ItemComponent("Knife", "A small knife used for stuff", ItemType.Weapon));
            _componentManager.AddComponent(item, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f));
            _componentManager.AddComponent(item, new PositionComponent(new Vector2(0, 0)));
            item2 = _entityManager.CreateEntity();
            _componentManager.AddComponent(item2, new ItemComponent("Dagger", "A small knife used for stuff", ItemType.Weapon));
            _componentManager.AddComponent(item2, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f));
            _componentManager.AddComponent(item2, new PositionComponent(new Vector2(256, 128)));
            item3 = _entityManager.CreateEntity();
            _componentManager.AddComponent(item3, new ItemComponent("Sword", "A small knife used for stuff", ItemType.Weapon));
            _componentManager.AddComponent(item3, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f));
            _componentManager.AddComponent(item3, new PositionComponent(new Vector2(128, 256)));
            player = _entityManager.CreateEntity();
            _componentManager.AddComponent(player, new PositionComponent(new Vector2(0, 0)));
            _componentManager.AddComponent(player, new RenderComponent(_spriteSheet, _sprites["player"], Color.White, 0f, 1f));
            _componentManager.AddComponent(player, new InputComponent());
            _componentManager.AddComponent(player, new InventoryComponent());
            _componentManager.AddComponent(player, new VelocityComponent(new Vector2(0, 0)));
            _componentManager.AddComponent(player, new NameComponent("PlayerName"));

        }

        protected override void Update(GameTime gameTime)
        {
            UpdateInputSystem(gameTime);
            UpdateCameraPosition();
            base.Update(gameTime);
        }

        private void UpdateInputSystem(GameTime gameTime)
        {
            //_eventManager.Publish(EventTypes.Message, this, $"Update inputsystem reached");

            _inputSystem.Update(gameTime);
        }

        private void UpdateCameraPosition()
        {
            var playerPositionComponent = _componentManager.GetComponent<PositionComponent>(player);
            if (playerPositionComponent != null)
            {
                _cameraPosition = playerPositionComponent.Position;
            }
            _camera.SetPosition(_cameraPosition);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _renderSystem.Draw();
            _spriteBatch.Begin();
            _messageLog.Draw(_spriteBatch, new Vector2(16, 16));
            _debugLog.Draw(_spriteBatch, new Vector2(128, 16));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}