using Microsoft.Xna.Framework;
using MonoGameLibrary.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels
{
    public abstract class Protagonist : DrawableAnimatableSprite,IDamageable
    {
        protected int defense;
        protected int attack;
        protected int spelldamage;
        protected int Experience;
        protected int hitpoints;

        public Protagonist(Game game) : base(game)
        {
            
        }
        public int HP { get { return hitpoints; } set { hitpoints = value; } }

        public int DEF { get { return defense; } set { defense = value; } }

        public int ATK { get { return attack; } set { attack = value; } }

        public int SPL { get { return spelldamage; } set { spelldamage = value; } }

        public int EXP { get { return Experience; } set { Experience += value; } }

    }
}
