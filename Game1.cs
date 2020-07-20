using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Glider_Simulation_V1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    ///

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Terrain terrain;
        Effect effects;
        Camera camera;
        Point saveMousePoint;
        bool moveMode = false;
        float scrollRate = 10.0f;
        MouseState previousMouse;
        Point screenCenter;
        Glider glider;
        Skybox skybox;

        private SpriteFont font;
        private SpriteFont font1;
        private SpriteFont font2;

        int finaldisplaytimer;

        private float radius = 48;
        //public static float angle2 = MathHelper.ToRadians(90);
        private float angle = 0;
        public static float angle2;

        private float originx = 60; // change the start coordinates
        private float originz = 70; // change the start coordinates

        private float anglefactor = 0.01f;
        private float dragforce = 1.0f;

        private float accelerationfactor; //= 0.5f;
        private float dragfactor; //= 0.05f;

        private float velocity;
        private float timeincrement;
        private float timeelapsed;
        private float changetimespan=20;

        bool displaytimeelapsed = false;

        private Userinput userInput;
        private Calculations calculationsUtils;

        public Game1(Userinput ui)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            userInput = ui;

           

        }
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1536;  
            graphics.PreferredBackBufferHeight = 864;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Flight Simulation";
            camera = new Camera(
                   new Vector3(60f, 16f, 64f), MathHelper.ToRadians(-30), 0f, 32f, 192f, 128f, GraphicsDevice.Viewport.AspectRatio, 0.1f, 512f);
            screenCenter.X = this.Window.ClientBounds.Width / 2;
            screenCenter.Y = this.Window.ClientBounds.Height / 2;
            this.IsMouseVisible = true;
            previousMouse = Mouse.GetState();
            Mouse.SetPosition(screenCenter.X, screenCenter.Y);
            calculationsUtils = new Calculations(userInput); 

            accelerationfactor = calculationsUtils.getThrustFactor() - calculationsUtils.getRealAccelerationFactor();
            dragfactor = calculationsUtils.getRealDragFactor() + calculationsUtils.getRealWeightFactor();

            dragfactor = dragfactor * dragforce;
            anglefactor = anglefactor * accelerationfactor;

            base.Initialize();
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            terrain = new Terrain(
              GraphicsDevice,
              Content.Load<Texture2D>(@"Textures\heightmap"),
              Content.Load<Texture2D>(@"Textures\Grass"),
              32f,
              128,
              128,
              30f);
            effects = Content.Load<Effect>(@"Effects/TerrainEffect");
            Vector3 Position = new Vector3(10, 64, 10); // need to change this position vector in the update section

            glider = new Glider(GraphicsDevice, Content.Load<Model>(@"Models\glider"), Position);
            skybox = new Skybox(@"Textures\grassbox", Content);

            font = Content.Load<SpriteFont>(@"Fonts\SpriteFont1");
            font1 = Content.Load<SpriteFont>(@"Fonts\SpriteFont2");
            font2 = Content.Load<SpriteFont>(@"SpriteFont1");
        }
        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if(changetimespan < 60 && changetimespan > 1)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    changetimespan += 0.02f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {

                    changetimespan -= 0.02f;

                }
                IsFixedTimeStep = true;
                TargetElapsedTime = TimeSpan.FromMilliseconds(changetimespan);


            }


            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }


            if (this.IsActive)
            {
                MouseState mouse = Mouse.GetState();
                if (moveMode)
                {
                    camera.Rotation += MathHelper.ToRadians(
                        (mouse.X - screenCenter.X) / 2f);
                    camera.Elevation += MathHelper.ToRadians(
                        (mouse.Y - screenCenter.Y) / 2f);
                    Mouse.SetPosition(screenCenter.X, screenCenter.Y);
                }
                if (mouse.RightButton == ButtonState.Pressed)
                {
                    if (!moveMode &&
                        previousMouse.RightButton == ButtonState.Released)
                    {
                        if (graphics.GraphicsDevice.Viewport.Bounds.Contains(
                            new Point(mouse.X, mouse.Y)))
                        {
                            moveMode = true;
                            saveMousePoint.X = mouse.X;
                            saveMousePoint.Y = mouse.Y;
                            Mouse.SetPosition(screenCenter.X, screenCenter.Y);
                            this.IsMouseVisible = false;
                        }
                    }
                }
                else
                {
                    if (moveMode)
                    {
                        moveMode = false;
                        Mouse.SetPosition(saveMousePoint.X, saveMousePoint.Y);
                        this.IsMouseVisible = true;
                    }
                }
                if (mouse.ScrollWheelValue - previousMouse.ScrollWheelValue != 0)
                {
                    float wheelChange = mouse.ScrollWheelValue -
                        previousMouse.ScrollWheelValue;
                    camera.ViewDistance -= (wheelChange / 120) * scrollRate;
                }
                previousMouse = mouse;
            }
            if (userInput.launchheight > 0f)
            {
                // calculate the acceleration factor later
                float X = originx + (float)Math.Cos(angle) * this.radius;
                float Z = originz + (float)Math.Sin(angle) * this.radius;
                glider.Position = new Vector3(X, userInput.launchheight, Z);  //change each X/Y/Z seperately...
                angle = angle + anglefactor;
                userInput.launchheight = userInput.launchheight - dragfactor;
                if (angle >= 360.0f)
                    angle = 0f;
                if (userInput.launchheight <= 0f)
                    userInput.launchheight = 0f;
            }
            else
            {
                displaytimeelapsed = true;
            }

            angle2 = -angle - MathHelper.ToRadians(90); //(float)(Math.Atan2(glider.Position.Z, glider.Position.X));

          


            

            camera.targetPosition = glider.Position;

            

            if(displaytimeelapsed == false)
            {

                timeelapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            


            if (userInput.launchheight > 0f)
            {

                timeincrement += (float)0.01;

                velocity = userInput.initialvelocity + (-accelerationfactor * timeincrement);


            }

            if (userInput.fuel >=0)
            {
                userInput.fuel = userInput.fuel - (userInput.thrust/6000);
            }

            if (userInput.fuel <= 0)
            {
                userInput.thrust = 0;

            }



            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

           
            RasterizerState originalRasterizerState = graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rasterizerState;

            skybox.Draw(camera.View, camera.Projection, camera.Position);

            terrain.Draw(camera, effects);
            glider.Draw(camera);

            if (userInput.launchheight > 0f)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Drag Factor " + dragfactor.ToString(), new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(font, "Acceleration Factor " + accelerationfactor.ToString(), new Vector2(0, 20), Color.White);
                spriteBatch.DrawString(font, "Height " + userInput.launchheight.ToString(), new Vector2(0, 40), Color.White);
                spriteBatch.DrawString(font, "Velocity " + velocity.ToString(), new Vector2(0, 60), Color.White);
                spriteBatch.DrawString(font, "Flight Angle " + angle.ToString(), new Vector2(0, 80), Color.White);
                spriteBatch.DrawString(font, "Glider Position " + glider.Position.ToString(), new Vector2(0, 100), Color.White);
                spriteBatch.DrawString(font, "Direction Angle " + angle2.ToString(), new Vector2(0, 120), Color.White);
                spriteBatch.DrawString(font, "Thrust Factor " + calculationsUtils.getThrustFactor().ToString(), new Vector2(0, 160), Color.White);
                spriteBatch.DrawString(font, "Weight Factor " + calculationsUtils.getRealWeightFactor().ToString(), new Vector2(0, 180), Color.White);
                spriteBatch.DrawString(font, "Time Elapsed " + timeelapsed, new Vector2(0, 140), Color.White);
                spriteBatch.DrawString(font, "Fuel Level " + userInput.fuel, new Vector2(0, 200), Color.White);
                spriteBatch.DrawString(font, "GameTime " + changetimespan, new Vector2(0, 240), Color.White);
                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.DrawString(font, "User Inputs: ", new Vector2(1275, 0), Color.White);
                spriteBatch.DrawString(font, "Glider Mass " + userInput.glidermass.ToString(), new Vector2(1275, 20), Color.White);
                spriteBatch.DrawString(font, "Drag Coefficient " + userInput.dragcoef.ToString(), new Vector2(1275, 40), Color.White);
                spriteBatch.DrawString(font, "Height " + userInput.launchheight.ToString(), new Vector2(1275, 60), Color.White);
                spriteBatch.DrawString(font, "Temperature " + userInput.temperature.ToString(), new Vector2(1275, 80), Color.White);
                spriteBatch.DrawString(font, "Initial Velocity " + userInput.initialvelocity.ToString(), new Vector2(1275, 100), Color.White);
                spriteBatch.DrawString(font, "Cross-Sectional Area " + userInput.crosssectionalarea.ToString(), new Vector2(1275, 120), Color.White);
                spriteBatch.DrawString(font, "Thrust " + userInput.thrust.ToString(), new Vector2(1275, 140), Color.White);
                spriteBatch.DrawString(font, "Fuel " + userInput.fuel.ToString(), new Vector2(1275, 160), Color.White);
                spriteBatch.End();

            }
               else
            {
                if (finaldisplaytimer <= 100)
                {
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font1, "END OF SIMULATION", new Vector2(500, 350), Color.White);
                    spriteBatch.End();
                    finaldisplaytimer += 1;
                }
                else
                {


                    
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font2, "Final Velocity " + velocity.ToString(), new Vector2(350, 150), Color.White);
                    spriteBatch.DrawString(font2, "Time Elapsed " + timeelapsed, new Vector2(350, 200), Color.White);
                    spriteBatch.DrawString(font2, "Position of Landing " + glider.Position.ToString(), new Vector2(350, 250), Color.White);
                    
                    spriteBatch.End();


                }





               


            }

            base.Draw(gameTime);
        }
    }
}
