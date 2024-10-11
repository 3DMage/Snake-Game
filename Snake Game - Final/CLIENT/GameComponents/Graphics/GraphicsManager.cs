using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameComponents
{
    // Holds data necessary for drawing for all gamestates.
    public static class GraphicsManager
    {
       

        //? TEXTURES ==============================================================================================
        public static Texture2D titleLogo { get; private set; }
        public static Texture2D bodySegmentTexture { get; private set; }
        public static Texture2D tailSegmentTexture { get; private set; }
        public static Texture2D snakeEyesTexture { get; private set; }
        public static Texture2D starCandyTexture { get; private set; }
        public static Texture2D collisionCircleTexture { get; private set; }
        public static Texture2D rectColorTexture { get; private set; }
        public static Texture2D backgroundTexture { get; private set; }
        public static Texture2D menuBackgroundTexture { get; private set; }
        public static Texture2D groundTexture { get; private set; }
        public static Texture2D explosionParticleTexture { get; private set; }
        public static Texture2D tutorialScreenTexture { get; private set; }
        public static Texture2D scoreBoardTexture { get; private set; }
        public static Texture2D animatedFoodSpriteSheet { get; private set; }
        public static Texture2D animatedFoodSpriteSheet2 { get; private set; }
        public static Texture2D animatedFoodSpriteSheet3 { get; private set; }
        public static Texture2D starTexture { get; private set; }
        public static Texture2D gameOverMenu { get; private set; }

        //? TEXTURES ==============================================================================================

        // Load textures from Content.
        private static void LoadTextures()
        {
            titleLogo = ContentManagerHandle.Content.Load<Texture2D>("sprites/titleLogo");
            bodySegmentTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/bodySegment");
            tailSegmentTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/tailSegment");
            snakeEyesTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/snakeEyes");
            starCandyTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/starCandy");
            collisionCircleTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/collisionCircle");
            rectColorTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/rectColor");
            backgroundTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/background");
            menuBackgroundTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/menuBackground");
            groundTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/groundTexture");
            explosionParticleTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/particle1");
            tutorialScreenTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/tutorialScreen");
            scoreBoardTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/scoreBoard");
            animatedFoodSpriteSheet = ContentManagerHandle.Content.Load<Texture2D>("sprites/animatedFood");
            animatedFoodSpriteSheet2 = ContentManagerHandle.Content.Load<Texture2D>("sprites/animatedFood2");
            animatedFoodSpriteSheet3 = ContentManagerHandle.Content.Load<Texture2D>("sprites/animatedFood3");
            starTexture = ContentManagerHandle.Content.Load<Texture2D>("sprites/star");
            gameOverMenu = ContentManagerHandle.Content.Load<Texture2D>("sprites/gameOverMenu");

        }







        //? FONTS =================================================================================================
        public static SpriteFont spaceFont { get; private set; }
        public static SpriteFont spaceFontHeader { get; private set; }
        public static SpriteFont calibriFont { get; private set; }
        public static SpriteFont calibriSmallFont { get; private set; }
        public static SpriteFont calibriHeaderFont { get; private set; }
        //? FONTS =================================================================================================

        // Load fonts from Content.
        private static void LoadFonts()
        {
            spaceFont = ContentManagerHandle.Content.Load<SpriteFont>("fonts/spaceFont");
            spaceFontHeader = ContentManagerHandle.Content.Load<SpriteFont>("fonts/spaceFontHeader");
            calibriFont = ContentManagerHandle.Content.Load<SpriteFont>("fonts/calibri");
            calibriSmallFont = ContentManagerHandle.Content.Load<SpriteFont>("fonts/calibriSmall");
            calibriHeaderFont = ContentManagerHandle.Content.Load<SpriteFont>("fonts/calibriHeader");

        }







        #region DON'T TOUCH
        //? =========== DON'T TOUCH =========== DON'T TOUCH =========== DON'T TOUCH =========== DON'T TOUCH =========== DON'T TOUCH =========== DON'T TOUCH
        // Drawing component
        public static GraphicsDeviceManager graphicsDeviceManager { get; private set; }
        public static GraphicsDevice graphicsDevice { get; private set; }
        public static SpriteBatch spriteBatch { get; private set; }

        // Window rendering
        private static int WINDOW_WIDTH;
        private static int WINDOW_HEIGHT;

        public static int REFERENCE_WINDOW_WIDTH { get; private set; } = 1920;
        public static int REFERENCE_WINDOW_HEIGHT { get; private set; } = 1080;

        public static RenderTarget2D renderTarget;
        public static Rectangle renderScaleRectangle;

        // Bounds of the screen.
        public static Vector2 REFERENCE_WINDOW_DIMENSIONS { get; private set; }

        // Aspect ratio
        public static float aspectRatio { get; private set; }

        // Constructor.
        public static void Initialize(GraphicsDeviceManager graphicsDeviceManager, GraphicsDevice graphicsDevice)
        {
            // Setup core graphics components.
            GraphicsManager.graphicsDeviceManager = graphicsDeviceManager;
            GraphicsManager.graphicsDevice = graphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsManager.graphicsDevice);

            // Initialize window rendering properties
            WINDOW_WIDTH = 1280;
            WINDOW_HEIGHT = 720;

            aspectRatio = REFERENCE_WINDOW_WIDTH / (float)REFERENCE_WINDOW_HEIGHT;

            REFERENCE_WINDOW_DIMENSIONS = new Vector2(REFERENCE_WINDOW_WIDTH, REFERENCE_WINDOW_HEIGHT);

            renderTarget = new RenderTarget2D
            (
            GraphicsManager.graphicsDevice,
            REFERENCE_WINDOW_WIDTH,
            REFERENCE_WINDOW_HEIGHT,
            false,
            SurfaceFormat.Color,
            DepthFormat.None,
            0,
            RenderTargetUsage.DiscardContents
            );
        }

        // Sets back buffer width and height, and configures fullscreen mode.
        public static void ConfigureGraphicsDevice()
        {
            // Set back buffer dimensions
            graphicsDeviceManager.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphicsDeviceManager.PreferredBackBufferHeight = WINDOW_HEIGHT;
            //graphicsDeviceManager.ToggleFullScreen();

            // Apply these settings to the graphics device manager.
            graphicsDeviceManager.ApplyChanges();

            graphicsDevice.RasterizerState = new RasterizerState
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.CullCounterClockwiseFace,
                MultiSampleAntiAlias = true,
            };

            renderScaleRectangle = GetScaleRectangle();
        }

        // Loads all textures from content.
        public static void LoadGraphicsAssets()
        {
            LoadTextures();
            LoadFonts();
        }

        // Swaps render target to draw to.
        public static void DrawToRenderTarget(bool drawToRenderTarget)
        {
            if (drawToRenderTarget == true)
            {
                graphicsDevice.SetRenderTarget(renderTarget);
            }
            else
            {
                graphicsDevice.SetRenderTarget(null);
            }
        }

        // This creates the render target to draw into.
        private static Rectangle GetScaleRectangle()
        {
            var variance = 0.5;
            var actualAspectRatio = graphicsDeviceManager.PreferredBackBufferWidth / (float)graphicsDeviceManager.PreferredBackBufferHeight;
            Rectangle scaleRectangle;

            if (actualAspectRatio <= aspectRatio)
            {
                var presentHeight = (int)(graphicsDeviceManager.PreferredBackBufferWidth / aspectRatio + variance);
                var barHeight = (graphicsDeviceManager.PreferredBackBufferHeight - presentHeight) / 2;
                scaleRectangle = new Rectangle(0, barHeight, GameHandle.game.Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                int presentWidth = (int)(graphicsDeviceManager.PreferredBackBufferHeight * aspectRatio + variance);
                var barWidth = (graphicsDeviceManager.PreferredBackBufferWidth - presentWidth) / 2;
                scaleRectangle = new Rectangle(barWidth, 0, presentWidth, graphicsDeviceManager.PreferredBackBufferHeight);
            }

            return scaleRectangle;
        }
        //? =========== DON'T TOUCH =========== DON'T TOUCH =========== DON'T TOUCH =========== DON'T TOUCH =========== DON'T TOUCH =========== DON'T TOUCH
        #endregion
    }
}