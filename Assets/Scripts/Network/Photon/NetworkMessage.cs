using ExitGames.Client.Photon;

/// <summary>
/// Data has to contain 14 or less elements. 
/// Otherwise Serialization/Deserialization will be executed with errors.
/// </summary>
public class NetworkMessage
{
    #region Static fields

    public static readonly byte[] messageData = new byte[16 * 4];

    #endregion

    #region Fields

    public readonly int MessageType;
    public readonly int Addresser; // 0 - broadcast message, meant for everyone
    public readonly float[] Data;

    #endregion

    #region Constructors

    public NetworkMessage(int messageType, float[] data) : this(messageType, 0, data) {}
    
    public NetworkMessage(int messageType, int addresser, float[] data)
    {
        MessageType = messageType;
        Addresser   = addresser;
        Data        = data;
    }

    #endregion

    #region Static methods

    public static short Serialize(StreamBuffer stream, object customobject)
    {
        var message = (NetworkMessage) customobject;

        int dataLength = message.Data.Length;

        short length = (short)((dataLength + 2) * 4);

        lock (messageData)
        {
            byte[] bytes    = messageData;
            int index       = 0;

            Protocol.Serialize(message.MessageType, bytes, ref index);
            Protocol.Serialize(message.Addresser, bytes, ref index);
            
            for(int i = 0; i < dataLength; i++)
            {
                var data = message.Data[i];

                Protocol.Serialize(data, bytes, ref index);
            };

            stream.Write(bytes, 0, length);
        }

        return length;
    }

    public static object Deserialize(StreamBuffer stream, short length)
    {
        NetworkMessage networkMessage;

        lock (messageData)
        {
            stream.Read(messageData, 0, length);
            
            int index = 0;

            Protocol.Deserialize(out int messageType, messageData, ref index);
            Protocol.Deserialize(out int addresser, messageData, ref index);

            int dataLength = length;

            dataLength /= 4;
            dataLength -= 2;

            var data = new float[dataLength];

            for(int i = 0; i < dataLength; i++)
            {
                Protocol.Deserialize(out float value, messageData, ref index);

                data[i] = value;
            };

            networkMessage = new NetworkMessage(messageType, addresser, data);
        }

        return networkMessage;
    }

    #endregion

    #region Methods

    public bool IsForAddresser(int addresser)
    {
        return (Addresser == 0 || Addresser == addresser);
    }

    #endregion
}
