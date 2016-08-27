using System;
using System.Collections.Generic;

namespace Shared.Additional
{
    public class DynamicProperties
    {
        private Dictionary<PropertyConstants, object> _properties = new Dictionary<PropertyConstants, object>();
        public int Count  { get { return _properties.Count; }}

        public void AddProperty(PropertyConstants key, object value)
        {
            _properties.Add(key, value);
        }

        public bool ExistsKey(PropertyConstants key)
        {
            if (_properties.ContainsKey(key))
                return true;
            return false;
        }

        public object GetValue(PropertyConstants key)
        {
            return _properties[key];
        }

        public void ChangeValue(PropertyConstants key, object value)
        {
            if(!ExistsKey(key))
                throw new Exception("Key " + key + " was not found.");
            _properties[key] = value;
        }

        public void Clear()
        {
            _properties.Clear();
        }

        public void RemoveKey(PropertyConstants key)
        {
            _properties.Remove(key);
        }
    }
}
