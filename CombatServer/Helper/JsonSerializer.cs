using System;
using Newtonsoft.Json;

namespace GestionServer.Helper
{
    public static class JsonSerializer
    {
        public static string toJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static E fromJson<E>(string json)
        {
            return JsonConvert.DeserializeObject<E>(json);
        }
    }
}