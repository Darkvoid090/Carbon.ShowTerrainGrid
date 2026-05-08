using Mafi;
using Mafi.Base;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Unity;
using System;

namespace Carbon.ShowTerrainGrid;


public sealed class ShowTerrainGridMod : IMod, IDisposable
{
    /// <summary>
    /// Human-readable name of the mod. This value will be showed in-game.
    /// </summary>    
    public ModManifest Manifest { get; }
    public ModJsonConfig JsonConfig { get; }
    public string Name => this.GetType().Namespace.Replace('.', '-');
    public void Dispose() { }
    public void MigrateJsonConfig(VersionSlim version, Dict<string, object> dict) { }

    /// <summary>
    /// Version of the mod, currently unused.
    /// </summary>
    public int Version => 1;


    /// <summary>
    /// Whether this mod is UI only and does not affect game state.
    /// All non-UI mods should not depend on or use Unity and UI-only mods should not contribute to game state changes.
    /// </summary>
    /// <remarks>
    /// UI mods are not instantiated in headless games such as determinism verification.
    /// </remarks>
    public bool IsUiOnly => false;


    /// <summary>
    /// Mod config. If set, its instance will be registered in the resolver.
    /// If mafi-serializable, it will be persisted though saves and this property will be set after load if a config of the same type was loaded from the save file.
    /// </summary>
    public Option<IConfig> ModConfig => this.modConfig;


    private readonly ShowTerrainGridModConfig modConfig;


    /// <summary>
    /// Mod constructor that lists mod dependencies as parameters.
    /// This guarantee that all listed mods will be loaded before this mod.
    /// It is a good idea to depend on both <see cref="CoreMod"/> and <see cref="BaseMod"/>.
    /// </summary>
    public ShowTerrainGridMod(ModManifest manifest,ShowTerrainGridModConfig modConfig)
    {
        Manifest = manifest;
        JsonConfig = new ModJsonConfig(this);
        this.modConfig = modConfig;

    }


    /// <summary>
    /// Register all prototypes of this mod.
    /// </summary>
    /// <param name="registrator"></param>
    void IMod.RegisterPrototypes(ProtoRegistrator registrator)
    {
        // Not used.
    }


    /// <summary>
    /// Registers all dependencies such as components or custom implementations of any dependencies that should override default behaviors.
    /// All prototypes of all mods are registered and prototypes database is locked before this method is called.
    /// </summary>
    /// <param name="depBuilder"></param>
    /// <param name="protosDb"></param>
    /// <param name="gameWasLoaded"></param>
    void IMod.RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded)
    {
        // Not used.
    }


    /// <summary>
    /// Early-initialization that is called right after the dependency resolver is created, before the map is even generated.
    /// Called for both new and loaded games.
    /// </summary>
    /// <remarks>
    /// During load, this is called after initial resolver load is finished, but before init-after-load methods marked with Mafi.Serialization.InitAfterLoadAttribute are called. Use this with care.
    /// </remarks>
    /// <param name="resolver"></param>
    void IMod.EarlyInit(DependencyResolver resolver)
    {
        // Not used.
    }


    /// <summary>
    /// Called exactly once before the game starts and after all the mods are registered and dependency builder is created.
    /// </summary>
    /// <remarks>
    /// This is the only place where mods can do some pre-processing, initialization, and checking after the whole game is loaded and ready to start.
    /// We use this for loading of UI that cannot be done in any other step since not all protos are loaded.
    /// </remarks>
    /// <param name="resolver"></param>
    /// <param name="gameWasLoaded"></param>
    void IMod.Initialize(DependencyResolver resolver, bool gameWasLoaded)
    {
        // Not used.
    }
}
