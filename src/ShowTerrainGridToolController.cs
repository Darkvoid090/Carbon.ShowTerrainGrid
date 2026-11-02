using System;

using Mafi;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.Terrain;
using Mafi.Unity.Ui;
using Mafi.Unity.Ui.Hud;
using Mafi.Unity.UiStatic.Toolbar;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.Utils;

namespace Carbon.ShowTerrainGrid;


[GlobalDependency(RegistrationMode.AsEverything, onlyInDebug: false, onlyInDevOnly: false)]
internal class ShowTerrainGridToolController : IToolbarItemController
{
    /// <summary>
    /// Whether the controller can be activated and should be displayed in the menu.
    /// </summary>
    public bool IsVisible => true;
    /// <summary>
    /// Describes how the toolbar item interacts with the input system.
    /// </summary>
    public ControllerConfig Config => ControllerConfig.Mode;

    /// <summary>
    /// Invoked when <see cref="IToolbarItemController.IsVisible"/> changes. The event can be invoked from any thread.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/archive/blogs/trevor/c-warning-cs0067-the-event-event-is-never-used"/>
    public event Action<IToolbarItemController> VisibilityChanged { add { } remove { } }


    [Mafi.Serialization.OnlyForSaveCompatibility]
    public bool DeactivateShortcutsIfNotVisible => false;


    private readonly ShowTerrainGridModConfig modConfig;
    private readonly IActivator terrainGridActivator;


    public ShowTerrainGridToolController(ShowTerrainGridModConfig modConfig, ToolbarHud toolbar, TerrainRenderer terrainRenderer, IShortcutsRegistry shortcutsRegistry)
    {
        this.modConfig = modConfig;
        this.terrainGridActivator = terrainRenderer.CreateGridLinesActivator();

        LocStr toolName = Loc.Str(id: "Toggle Terrain Grid", enUs: "Toggle Terrain Grid", comment: "ShowTerrainGridTool");

        // Since game 0.8.8 key bindings are KbHandles registered with IShortcutsRegistry.
        // The player can rebind the key in the Controls tab (listed under this mod's name).
        Option<KbHandle> shortcut = Option<KbHandle>.None;
        ShowTerrainGridMod mod = ShowTerrainGridMod.Instance;
        if (mod != null)
        {
            // Id must be "<mod id>.<Name>" where Name has only letters, digits and underscores.
            KbHandle toggleKey = KbHandleBuilder
                .Start(mod.Manifest.Id + ".ToggleTerrainGrid", "Toggle Terrain Grid")
                .SetPrimary(KbModifiers.Shift, UnityEngine.KeyCode.F)
                .SetGroup(KbGroups.Tools)
                .SetMode(ShortcutMode.Game)
                .Build();
            shortcutsRegistry.Add(mod, toggleKey);
            shortcut = Option<KbHandle>.Some(toggleKey);
        }

        Button toolButton = toolbar.AddToolButton(name: toolName, controller: this, iconAssetPath: "Assets/Unity/UserInterface/Toolbar/TerrainGrid.svg", order: 1f, shortcut: shortcut);
        toolButton.Selected(this.modConfig.IsEnabled);
    }

    /// <summary>
    /// Called when this input controller is activated by the player. Invoked on the main thread.
    /// </summary>
    public void Activate()
    {
        this.modConfig.IsEnabled = true;
        this.terrainGridActivator.ActivateIfNotActive();
    }


    /// <summary>
    /// Called when this input controller is deactivated by the player. Invoked on the main thread.
    /// </summary>
    public void Deactivate()
    {
        this.modConfig.IsEnabled = false;
        this.terrainGridActivator.DeactivateIfActive();
    }


    /// <summary>
    /// Called every frame when the controller is active. Invoked on the main thread.
    /// </summary>
    /// <returns>Whether input was processed and no other controllers should be updated.</returns>
    public bool InputUpdate() => false;
}
