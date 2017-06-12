using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1Levels
{
    public interface IObserver
    {
        void ObserverUpdate(Object sender, Object message);
    }
}