using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1Levels
{
    public interface ISubject
    {
        void Attach(IObserver o);
        void Deatach(IObserver o);
        void Notify(string s);
        void Notify(Rectangle r);
    }
}