using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1Levels
{
    public interface IDamageable
    {
        int HP
        {
            get;
            set;
        }
        int DEF
        {
            get;
            set;
        }
        int ATK
        {
            get;
            set;
        }
        int SPL
        {
            get;
            set;
        }
        int EXP
        {
            get;
            set;
        }

    }
}