using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Utmark.Engine.Camera;
using Utmark.Engine.Settings.Screen;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;
using Utmark_ECS.Map;
using Utmark_ECS.Map.MapGenerators;
using Utmark_ECS.Systems;
using Utmark_ECS.Systems.EventHandlers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.Input;
using Utmark_ECS.Systems.Render;
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
        private GraphicsDevice _graphicsDevice;
        private GraphicsDeviceManager _graphics;
        private ScreenSettings _screenSettings;
        private SpriteFont _font;
        private SpriteBatch _spriteBatch;
        private Dictionary<string, Rectangle> _sprites;
        private Texture2D _spriteSheet;
        private Texture2D _pixel;
        private Tile _tallGrass;
        private Tile _shortGrass;
        private Tile _fern;
        private Tile _herb1;
        private Tile _herb2;

        private Camera2D _camera;
        private Vector2 _cameraPosition;
        private Entity player;
        private Entity nPC;
        private Entity item3;
        private Entity item2;
        private Entity item1;
        private Entity item4;

        // Managers and Systems
        private ActionHandler _actionHandler;
        private TileMap _tileMap;
        private InputMapper _inputMapper;
        private MovementSystem _movementSystem;
        private ComponentManager _componentManager;
        private EntityManager _entityManager;
        private SpatialGrid _spatialGrid;
        private EventManager _eventManager;
        private RenderSystem _renderSystem;
        private InputSystem _inputSystem;
        private CollisionHandler _collisionDetectionSystem;
        private ResourceManager _resourceManager;
        private UIManager _uiManager;
        private InventoryUI _inventoryUI;
        private TopUI _topUI;
        private InventorySystem _inventorySystem;
        private ContextMenu _contextMenu;
        private RandomMapGenerator _randomMapGenerator;
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
            _eventManager = new EventManager();
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
            _graphicsDevice = _graphics.GraphicsDevice;
            _font = Content.Load<SpriteFont>("spriteFont");
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _camera = new Camera2D(_graphicsDevice.Viewport);
            _screenSettings.InitializeDefaults();
            _screenWidth = _screenSettings.GetCurrentScreenResolution().width;
            _screenHeight = _screenSettings.GetCurrentScreenResolution().height;
            _pixel = Content.Load<Texture2D>("Images/OnePixel");
            _topUI = new TopUI(_screenWidth, 50, _pixel, _font);
            _inventoryUI = new InventoryUI(0, 0, _pixel, _font, new Vector2(128, 128), player, _eventManager);
            _contextMenu = new ContextMenu(_pixel, _font);
            _uiManager.AddComponent(_topUI);
            _uiManager.AddComponent(_contextMenu);
            _uiManager.AddComponent(_inventoryUI);
            _uiManager.AddComponent(new MessageUI(_font, _eventManager, 0, _screenHeight - 256, _screenWidth, 256, _pixel));
        }

        private void InitializeAssets()
        {
            _sprites = new Dictionary<string, Rectangle>
            {
                {"tallGrass", new Rectangle(0, 0, 16, 16)},
                {"shortGrass", new Rectangle(16, 0, 16, 16)},
                {"fern", new Rectangle(32, 0, 16, 16)},
                {"herb", new Rectangle(192, 0, 16, 16)},
                {"herb2", new Rectangle(206, 0, 16, 16)},
                {"player", new Rectangle(16, 80, 16, 16)},
                {"knife", new Rectangle(32, 64, 16, 16)}
            };
            _spriteSheet = Content.Load<Texture2D>("Images/classic_roguelike16x16");
            _resourceManager = new ResourceManager { SpriteSheet = _spriteSheet, Sprites = _sprites };
        }

        private void InitializeEntities()
        {
            _spatialGrid = new SpatialGrid(Globals.GridSize, _eventManager);
            _entityManager = new EntityManager(_eventManager);
            _tallGrass = new Tile(TileType.Wall, "tallGrass", Color.OliveDrab, null);
            _shortGrass = new Tile(TileType.Soil, "shortGrass", Color.DarkOliveGreen, null);
            _fern = new Tile(TileType.Soil, "fern", Color.DarkGreen, null);
            _herb1 = new Tile(TileType.Soil, "herb", Color.DarkGreen, null);
            _herb2 = new Tile(TileType.Soil, "herb2", Color.DarkGreen, null);
            IMapGenerator _randomMapGenerator = new RandomMapGenerator();

            Tile[] availableTiles = new Tile[] { _shortGrass, _tallGrass, _fern };
            _tileMap = new TileMap(64, 64, _spatialGrid, _randomMapGenerator, (availableTiles));
            _spatialGrid.SetTileMap(_tileMap);
        }

        private void InitializeSystems()
        {
            _componentManager = new ComponentManager(_entityManager, _spatialGrid);
            _spatialGrid.SetComponentManager(_componentManager);
            _collisionDetectionSystem = new CollisionHandler(_eventManager, _componentManager);
            _inventorySystem = new InventorySystem(_componentManager);
            _actionHandler = new ActionHandler(_eventManager, _componentManager, _inventorySystem);
            _renderSystem = new RenderSystem(_componentManager, _spriteBatch, _tileMap, _camera, _resourceManager, _uiManager, _graphics.GraphicsDevice, _screenWidth, _screenHeight);
            _inputSystem = new InputSystem(_eventManager);
            _inputMapper = new InputMapper(_eventManager, _componentManager);
            _movementSystem = new MovementSystem(_componentManager, _spatialGrid, _eventManager);

        }

        protected override void LoadContent()
        {
            InitializeActors();
            InitializeItems();

        }

        private void InitializeItems()
        {
            item1 = _entityManager.CreateEntity();
            _componentManager.AddComponent(item1, new ItemComponent("knife", "A small knife used for stuff", ItemType.Weapon));
            _componentManager.AddComponent(item1, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f, Globals.StandardSize));
            _componentManager.AddComponent(item1, new PositionComponent(new Vector2(825, 120)));
            item4 = _entityManager.CreateEntity();
            _componentManager.AddComponent(item4, new ItemComponent("Sword", "A small knife used for stuff", ItemType.Weapon));
            _componentManager.AddComponent(item4, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f, Globals.StandardSize));
            _componentManager.AddComponent(item4, new PositionComponent(new Vector2(128, 312)));
            item2 = _entityManager.CreateEntity();
            _componentManager.AddComponent(item2, new ItemComponent("knife", "A small knife used for stuff", ItemType.Weapon));
            _componentManager.AddComponent(item2, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f, Globals.StandardSize));
            _componentManager.AddComponent(item2, new PositionComponent(new Vector2(845, 141)));
            item3 = _entityManager.CreateEntity();
            _componentManager.AddComponent(item3, new ItemComponent("Sword", "A small knife used for stuff", ItemType.Weapon));
            _componentManager.AddComponent(item3, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f, Globals.StandardSize));
            _componentManager.AddComponent(item3, new PositionComponent(new Vector2(128, 312)));
        }
        private void InitializeActors()
        {

            player = _entityManager.CreateEntity();
            _componentManager.AddComponent(player, new PositionComponent(new Vector2(0, 0)));
            _componentManager.AddComponent(player, new InputComponent());
            _componentManager.AddComponent(player, new InventoryComponent());
            _componentManager.AddComponent(player, new VelocityComponent(new Vector2(0, 0)));
            _componentManager.AddComponent(player, new NameComponent("Player"));
            _componentManager.AddComponent(player, new RenderComponent(_spriteSheet, _sprites["player"], Color.White, 0f, 1f, Globals.LargeSize));
            nPC = _entityManager.CreateEntity();
            _componentManager.AddComponent(nPC, new PositionComponent(new Vector2(160, 256)));
            _componentManager.AddComponent(nPC, new InventoryComponent());
            _componentManager.AddComponent(nPC, new VelocityComponent(new Vector2(0, 0)));
            _componentManager.AddComponent(nPC, new NameComponent("NPC"));
            _componentManager.AddComponent(nPC, new RenderComponent(_spriteSheet, _sprites["player"], Color.White, 0f, 1f, Globals.StandardSize));
        }

        protected override void Update(GameTime gameTime)
        {

            UpdateInputSystem();
            UpdateCameraPosition();
            base.Update(gameTime);
        }

        private void UpdateInputSystem()
        {

            _inputSystem.Update();
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

            base.Draw(gameTime);
        }
    }
}
