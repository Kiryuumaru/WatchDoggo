using DoggoWire.Abstraction;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDoggo
{
    internal class Storage : IStorage
    {
        private static readonly RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WatchDoggo");

        public object GetValue(string key)
        {
            return registryKey.GetValue(key);
        }

        public void SetValue(string key, object value)
        {
            registryKey.SetValue(key, value);
        }
    }
}
