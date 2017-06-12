using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels
{

    public enum LevelScreenState { TransitionTo, TransitionFrom, CurrentLevel, NotOnScreen }


    public class Level : TextureSprite
    {

        public int LevelIndex;

        public Vector2 GridIndex { get; set; }
        public Vector2 DrawOffset { get; set; }

        static int levelCount;

        public LevelScreenState LevelScreenState;

        public EnemyManager em;

        Hero hero;

        public Level(Game game, int GridX, int GridY, Hero h) : base(game)
        {
            levelCount++;
            this.LevelIndex = levelCount;
            this.LevelScreenState = LevelScreenState.NotOnScreen;
            GridIndex = new Vector2(GridX, GridY);
            this.hero = h;
        }


        protected override void LoadContent()
        {
            em = new EnemyManager(this.Game, this.hero);
            em.Initialize();
            this.Game.Components.Add(em);
            this.em.Enabled = false;
            this.em.Visible = false;  

            base.LoadContent();
            switch(LevelIndex)
            {
                case 1:
                    this.Texture = this.Game.Content.Load<Texture2D>("forest");
                    break;
                case 2:
                    this.Texture = this.Game.Content.Load<Texture2D>("rock");
                    break;
                case 3:
                    this.Texture = this.Game.Content.Load<Texture2D>("testmap1");
                    break;
                case 4:
                    this.Texture = this.Game.Content.Load<Texture2D>("map");
                    break;
                case 5:
                    this.Texture = this.Game.Content.Load<Texture2D>("Barren");
                    break;
                case 6:
                    this.Texture = this.Game.Content.Load<Texture2D>("Grass Lands");
                    break;
                case 7:
                    this.Texture = this.Game.Content.Load<Texture2D>("forest");
                    break;
                case 8:
                    this.Texture = this.Game.Content.Load<Texture2D>("rock");
                    break;
                case 9:
                    this.Texture = this.Game.Content.Load<Texture2D>("testmap1");
                    break;

            }
        }

        protected override void DrawTexture()
        {
            
            spriteBatch.Draw(this.Texture, this.Location + this.DrawOffset, Color.White);
        }

    }
}
