using System;
using System.Collections.Generic;
using System.IO;

namespace FoolGame
{
    public class ObservableState<T>
    {
        private List<Action<T>> observers = new List<Action<T>>();
        private T value;
        
       public void Observe(Action<T> o)
        {
         observers.Add(o);
         Notify();
        }

       public void emit(T newObject)
       {
           value = newObject;
           Notify();
       }
       
        void Notify()
        {
            observers.ForEach(action => action(value));
        }
    }
}