using Mafi;
using Mafi.Core.Mods;
using Mafi.Serialization;

namespace Carbon.ShowTerrainGrid;


[GlobalDependency(RegistrationMode.AsEverything, onlyInDebug: false, onlyInDevOnly: false)]
[GenerateSerializer(customClassDataSerialization: false, serializeAsSingleton: null, newInVersion: 0)]
public class ShowTerrainGridModConfig : IModConfig
{
    public static readonly int ConfigVersion = 1;

    public bool IsEnabled { get; set; }
    public string Shortcut { get; set; } = "Shift+F";


    #region Serialization stuff

    public static void Serialize(ShowTerrainGridModConfig value, BlobWriter writer)
    {
        if (writer.TryStartClassSerialization(value) is false)
            return;

        static void SerializeAction(object value, BlobWriter writer) => ActualSerialize((ShowTerrainGridModConfig)value, writer);
        writer.EnqueueDataSerialization(value, SerializeAction);
    }


    private static void ActualSerialize(ShowTerrainGridModConfig modConfig, BlobWriter writer)
    {
        writer.WriteInt(ConfigVersion);
        writer.WriteBool(modConfig.IsEnabled);
    }


    public static ShowTerrainGridModConfig Deserialize(BlobReader reader)
    {
        if (reader.TryStartClassDeserialization(out ShowTerrainGridModConfig modConfig) is false)
            return modConfig;

        static void DeserializeAction(object value, BlobReader reader) => ActualDeserialize((ShowTerrainGridModConfig)value, reader);
        reader.EnqueueDataDeserialization(modConfig, DeserializeAction, parent: null);
        return modConfig;
    }


    private static void ActualDeserialize(ShowTerrainGridModConfig modConfig, BlobReader reader)
    {
        int configVersion = reader.ReadInt();
        modConfig.IsEnabled = reader.ReadBool();
    }

    #endregion Serialization stuff
}
