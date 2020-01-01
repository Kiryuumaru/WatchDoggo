using System;
using System.Collections.Generic;
using System.Text;

namespace DoggoWire.Abstraction
{
    public interface IStorage
    {
        void SetValue(string key, object value);
        object GetValue(string key);
    }
}
