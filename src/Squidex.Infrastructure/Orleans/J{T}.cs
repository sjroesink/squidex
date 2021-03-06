﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orleans.CodeGeneration;
using Orleans.Serialization;
using Squidex.Infrastructure.Log;

namespace Squidex.Infrastructure.Orleans
{
    public struct J<T>
    {
        public T Value { get; }

        [JsonConstructor]
        public J(T value)
        {
            Value = value;
        }

        public static implicit operator T(J<T> value)
        {
            return value.Value;
        }

        public static implicit operator J<T>(T d)
        {
            return new J<T>(d);
        }

        public override string ToString()
        {
            return Value?.ToString() ?? string.Empty;
        }

        public static Task<J<T>> AsTask(T value)
        {
            return Task.FromResult<J<T>>(value);
        }

        [CopierMethod]
        public static object Copy(object input, ICopyContext context)
        {
            return input;
        }

        [SerializerMethod]
        public static void Serialize(object input, ISerializationContext context, Type expected)
        {
            using (Profile.Method(nameof(J)))
            {
                var stream = new MemoryStream();

                using (var writer = new JsonTextWriter(new StreamWriter(stream)))
                {
                    J.Serializer.Serialize(writer, input);

                    writer.Flush();
                }

                var outBytes = stream.ToArray();

                context.StreamWriter.Write(outBytes.Length);
                context.StreamWriter.Write(outBytes);
            }
        }

        [DeserializerMethod]
        public static object Deserialize(Type expected, IDeserializationContext context)
        {
            using (Profile.Method(nameof(J)))
            {
                var outLength = context.StreamReader.ReadInt();
                var outBytes = context.StreamReader.ReadBytes(outLength);

                var stream = new MemoryStream(outBytes);

                using (var reader = new JsonTextReader(new StreamReader(stream)))
                {
                    return J.Serializer.Deserialize(reader, expected);
                }
            }
        }
    }
}
