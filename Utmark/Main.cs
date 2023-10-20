using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
using Utmark_ECS.Systems.EventSystem.EventType;
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
        private ComponentManager ComponentManager;
        private EntityManager EntityManager;
        private SystemManager SystemManager;
        private EventManager EventManager;
        private ActionHandler _actionHandler;
        private TileMap _tileMap;
        private SpatialGrid _spatialGrid;
        private RenderSystem _renderSystem;
        private CollisionHandler _collisionDetectionSystem;
        private TileMapResource _resourceManager;
        private UIManager _uiManager;
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
            EventManager = new EventManager();
            _uiManager = new UIManager();
            EventManager.Subscribe<MouseScrollEventData>(OnScroll);
        }

        private void OnScroll(MouseScrollEventData data)
        {
            if (data.Delta == 120)
                _camera.SetZoom(2f);
            else if (data.Delta == -120)
            {
                _camera.SetZoom(1.6f);
            }
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
            _contextMenu = new ContextMenu(_pixel, _font, EventManager);
            _uiManager.AddComponent(_topUI);
            _uiManager.AddComponent(new MessageUI(_font, EventManager, 0, _screenHeight - 256, _screenWidth, 256, _pixel));
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
            _resourceManager = new TileMapResource { SpriteSheet = _spriteSheet, Sprites = _sprites };
        }

        private void InitializeEntities()
        {
            _spatialGrid = new SpatialGrid(Globals.GridSize, EventManager);
            _tallGrass = new Tile(TileType.Wall, "tallGrass", Color.OliveDrab, null);
            _shortGrass = new Tile(TileType.Soil, "shortGrass", Color.DarkOliveGreen, null);
            _fern = new Tile(TileType.Soil, "fern", Color.DarkGreen, null);
            _herb1 = new Tile(TileType.Soil, "herb", Color.DarkGreen, null);
            _herb2 = new Tile(TileType.Soil, "herb2", Color.DarkGreen, null);
            IMapGenerator _randomMapGenerator = new RandomMapGenerator();
            Tile[] availableTiles = new Tile[] { _shortGrass, _tallGrass, _fern };
            _tileMap = new TileMap(64, 64, _spatialGrid, _randomMapGenerator, (availableTiles));
        }

        private void InitializeSystems()
        {
            EntityManager = new EntityManager(EventManager);
            ComponentManager = new ComponentManager(EntityManager, _spatialGrid);
            SystemManager = new SystemManager();
            _inventorySystem = new InventorySystem(ComponentManager);
            _actionHandler = new ActionHandler(EventManager, ComponentManager, _inventorySystem);
            _contextMenu = new ContextMenu(_pixel, _font, EventManager);

            SystemManager.AddSystem(new InputSystem(EventManager));
            SystemManager.AddSystem(new InventorySystem(ComponentManager));
            SystemManager.AddSystem(new RenderSystem(ComponentManager, _tileMap, _camera, _resourceManager, _uiManager, _graphics.GraphicsDevice, _screenWidth, _screenHeight, _contextMenu));
            SystemManager.AddSystem(new InputMapper(EventManager, ComponentManager));
            SystemManager.AddSystem(new CollisionHandler(EventManager, ComponentManager));
            SystemManager.AddSystem(new MovementSystem(ComponentManager, _spatialGrid, EventManager));
        }

        protected override void LoadContent()
        {
            InitializeActors();
            InitializeItems();
        }

        private void InitializeItems()
        {
            item1 = EntityManager.CreateEntity();
            ComponentManager.AddComponent(item1, new ItemComponent("knife", "A small knife used for stuff", ItemType.Weapon));
            ComponentManager.AddComponent(item1, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f, Globals.StandardSize));
            ComponentManager.AddComponent(item1, new PositionComponent(new Vector2(825, 120)));
            item4 = EntityManager.CreateEntity();
            ComponentManager.AddComponent(item4, new ItemComponent("Sword", "A small knife used for stuff", ItemType.Weapon));
            ComponentManager.AddComponent(item4, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f, Globals.StandardSize));
            ComponentManager.AddComponent(item4, new PositionComponent(new Vector2(128, 312)));
            item2 = EntityManager.CreateEntity();
            ComponentManager.AddComponent(item2, new ItemComponent("knife", "A small knife used for stuff", ItemType.Weapon));
            ComponentManager.AddComponent(item2, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f, Globals.StandardSize));
            ComponentManager.AddComponent(item2, new PositionComponent(new Vector2(845, 141)));
            item3 = EntityManager.CreateEntity();
            ComponentManager.AddComponent(item3, new ItemComponent("Sword", "A small knife used for stuff", ItemType.Weapon));
            ComponentManager.AddComponent(item3, new RenderComponent(_spriteSheet, _sprites["knife"], Color.Gray, 0f, 0f, Globals.StandardSize));
            ComponentManager.AddComponent(item3, new PositionComponent(new Vector2(128, 312)));
        }
        private void InitializeActors()
        {
            player = EntityManager.CreateEntity();
            ComponentManager.AddComponent(player, new PositionComponent(new Vector2(726, 560)));
            ComponentManager.AddComponent(player, new InputComponent());
            ComponentManager.AddComponent(player, new InventoryComponent());
            ComponentManager.AddComponent(player, new VelocityComponent(new Vector2(0, 0)));
            ComponentManager.AddComponent(player, new NameComponent("Player"));
            ComponentManager.AddComponent(player, new RenderComponent(_spriteSheet, _sprites["player"], Color.White, 0f, 1f, Globals.LargeSize));
            nPC = EntityManager.CreateEntity();
            ComponentManager.AddComponent(nPC, new PositionComponent(new Vector2(160, 256)));
            ComponentManager.AddComponent(nPC, new InventoryComponent());
            ComponentManager.AddComponent(nPC, new VelocityComponent(new Vector2(0, 0)));
            ComponentManager.AddComponent(nPC, new NameComponent("NPC"));
            ComponentManager.AddComponent(nPC, new RenderComponent(_spriteSheet, _sprites["player"], Color.White, 0f, 1f, Globals.StandardSize));
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateInputSystem(gameTime);
            UpdateCameraPosition();
            _contextMenu.Update(gameTime);
            base.Update(gameTime);
        }

        private void UpdateInputSystem(GameTime gameTime)
        {
            SystemManager.UpdateSystems(gameTime);
        }

        private void UpdateCameraPosition()
        {
            var playerPositionComponent = ComponentManager.GetComponent<PositionComponent>(player);
            if (playerPositionComponent != null)
            {
                _cameraPosition = playerPositionComponent.Position;
                _camera.SetPosition(_cameraPosition);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SystemManager.DrawSystems(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
