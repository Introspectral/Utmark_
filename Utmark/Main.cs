using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using Utmark_ECS.UI;
using Utmark_ECS.UI.UI_Elements;
using Utmark_ECS.Utilities;
using static Utmark_ECS.Enums.ItemTypeEnum;
using static Utmark_ECS.Enums.TileTypeEnum;

namespace Utmark
{
    public class Main : Game
    {
        #region Fields

        private GraphicsDeviceManager _graphics;
        private ScreenSettings _screenSettings;
        private SpriteFont _font;
        private SpriteBatch _spriteBatch;
        private Rectangle _spriteSourceRect;
        private Dictionary<string, Rectangle> _sprites;
        private Texture2D _spriteSheet;
        private Texture2D _pixel;
        private Tile _grass;
        private Camera2D _camera;
        private Vector2 _cameraPosition;
        private Entity player;
        private Entity nPC;
        private Entity item3;
        private Entity item2;
        private int _tileSize;

        // Managers and Systems
        private ActionHandler _actionHandler;
        private TileMap _tileMap;
        private InputMapper _inputMapper;
        private ComponentManager _componentManager;
        private EntityManager _entityManager;
        private SpatialGrid _spatialGrid;
        private EventManager _eventManager;
        private RenderSystem _renderSystem;
        private InputSystem _inputSystem;
        private CollisionHandler _collisionDetectionSystem;
        private MessageLog _messageLog;
        private ResourceManager _resourceManager;
        private UIManager _uiManager;
        private int _screenWidth;
        private int _screenHeight;
        #endregion

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            _screenSettings = new ScreenSettings(_graphics);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
            _tileSize = GameConstants.GridSize;
            _eventManager = new EventManager();
            _inputMapper = new InputMapper(_eventManager);
            _uiManager = new UIManager();
        }

        protected override void Initialize()
        {
            InitializeDisplay();
            InitializeAssets();
            InitializeEntities();
            InitializeSystems();
            base.Initialize();
        }

        private void InitializeDisplay()
        {
            _font = Content.Load<SpriteFont>("spriteFont");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new Camera2D(GraphicsDevice.Viewport);
            _spriteSourceRect = new Rectangle(0, 0, 16, 16);
            _screenSettings.InitializeDefaults();
            _screenWidth = _screenSettings.GetCurrentScreenResolution().width;
            _screenHeight = _screenSettings.GetCurrentScreenResolution().height;
            _pixel = Content.Load<Texture2D>("Images/OnePixel");
            var topUI = new TopUI(_screenWidth, 50, _pixel, _font);
            
            _uiManager.AddComponent(topUI);
            _uiManager.AddComponent(new MessageUI(_font,_eventManager, 0, _screenHeight - 256, _screenWidth, 256, _pixel));
        }

        private void InitializeAssets()
        {
            _sprites = new Dictionary<string, Rectangle>
            {
                {"tallGrass", new Rectangle(0, 0, 16, 16)},
                {"shortGrass", new Rectangle(16, 0, 16, 16)},
                {"fern", new Rectangle(32, 0, 16, 16)},
                {"player", new Rectangle(16, 80, 16, 16)},
                {"knife", new Rectangle(32, 64, 16, 16)}
            };
            _spriteSheet = Content.Load<Texture2D>("Images/classic_roguelike16x16");
            _resourceManager = new ResourceManager { SpriteSheet = _spriteSheet, Sprites = _sprites };
        }

        private void InitializeEntities()
        {
            _spatialGrid = new SpatialGrid(16, _eventManager);
            _entityManager = new EntityManager(_eventManager, _spatialGrid);
            _grass = new Tile(TileType.Soil, "tallGrass", Color.DarkOliveGreen, null);
            _tileMap = new TileMap(264, 264, _spatialGrid, _grass);
            _spatialGrid.SetTileMap(_tileMap);
        }

        private void InitializeSystems()
        {
            _componentManager = new ComponentManager(_entityManager, _eventManager, _tileMap, _spatialGrid);
            _spatialGrid.SetComponentManager(_componentManager);
            _componentManager.SetTileMapAndSpatialGrid(_tileMap, _spatialGrid);
            _collisionDetectionSystem = new CollisionHandler(_eventManager, _componentManager);
            _actionHandler = new ActionHandler(_eventManager, _componentManager);
            _renderSystem = new RenderSystem(_componentManager, _spriteBatch, _tileMap, _camera, _resourceManager);
            _inputSystem = new InputSystem(_componentManager, _eventManager, _inputMapper);

        }

        protected override void LoadContent()
        {

            item2 = _entityManager.CreateEntity();
            _componentManager.AddComponent(item2, new ItemComponent("knife", "A small knife used for stuff", ItemType.Weapon));
            _componentManager.AddComponent(item2, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f));
            _componentManager.AddComponent(item2, new PositionComponent(new Vector2(885, 160)));
            item3 = _entityManager.CreateEntity();
            _componentManager.AddComponent(item3, new ItemComponent("Sword", "A small knife used for stuff", ItemType.Weapon));
            _componentManager.AddComponent(item3, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f));
            _componentManager.AddComponent(item3, new PositionComponent(new Vector2(128, 312)));
            player = _entityManager.CreateEntity();
            _componentManager.AddComponent(player, new PositionComponent(new Vector2(512, 512)));
            _componentManager.AddComponent(player, new InputComponent());
            _componentManager.AddComponent(player, new InventoryComponent());
            _componentManager.AddComponent(player, new VelocityComponent(new Vector2(0, 0)));
            _componentManager.AddComponent(player, new NameComponent("Player"));
            _componentManager.AddComponent(player, new RenderComponent(_spriteSheet, _sprites["player"], Color.White, 0f, 1f));
            nPC = _entityManager.CreateEntity();
            _componentManager.AddComponent(nPC, new PositionComponent(new Vector2(160, 256)));
            _componentManager.AddComponent(nPC, new InventoryComponent());
            _componentManager.AddComponent(nPC, new VelocityComponent(new Vector2(0, 0)));
            _componentManager.AddComponent(nPC, new NameComponent("NPC"));
            _componentManager.AddComponent(nPC, new RenderComponent(_spriteSheet, _sprites["player"], Color.White, 0f, 1f));

        }

        protected override void Update(GameTime gameTime)
        {

            UpdateInputSystem(gameTime);
            UpdateCameraPosition();
            base.Update(gameTime);
        }

        private void UpdateInputSystem(GameTime gameTime)
        {
            _uiManager.Update(gameTime);
            _inputSystem.Update(gameTime);
        }

        private void UpdateCameraPosition()
        {
            var playerPositionComponent = _componentManager.GetComponent<PositionComponent>(player);
            if (playerPositionComponent != null)
            {
                _cameraPosition = playerPositionComponent.Position;
                _camera.SetPosition(_cameraPosition);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _renderSystem.Draw();
            _spriteBatch.Begin();
            _uiManager.Draw(_spriteBatch);
            //_messageLog.Draw(_spriteBatch, new Vector2(16, 16));
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
