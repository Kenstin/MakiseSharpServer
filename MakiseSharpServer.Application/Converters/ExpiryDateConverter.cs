﻿using System;
using Newtonsoft.Json;

namespace MakiseSharpServer.Application.Converters
{
    public class ExpiryDateConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary as CanConvert is set to false.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTime.Now.AddSeconds((long)reader.Value);
        }

        public override bool CanConvert(Type objectType) => false;
    }
}