using System;

namespace NetMQ.ReactiveExtensions;

internal static class SerializeExtensions
{
    internal static byte[] Serialize<T>(this T message, RxSerializerType rxSerializerType)
    {
        switch (rxSerializerType)
        {
            case RxSerializerType.Undefined:
                throw new ArgumentOutOfRangeException(nameof(rxSerializerType), rxSerializerType, "Undefined is not a valid option");
                break;
            case RxSerializerType.Protobuf:
                return message.SerializeProtoBuf();
                break;
            case RxSerializerType.NewtonsoftJson:
                return message.SerializeJson();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(rxSerializerType), rxSerializerType, "Not implemented");
        }
    }
}