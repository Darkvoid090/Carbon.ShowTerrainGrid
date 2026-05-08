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
    public event Action<IToolbarItemController>? VisibilityChanged { add { } remove { } }


    [Mafi.Serialization.OnlyForSaveCompatibility]
    public bool DeactivateShortcutsIfNotVisible => false;


    private readonly ShowTerrainGridModConfig modConfig;
    private readonly IActivator terrainGridActivator;


    public ShowTerrainGridToolController(ShowTerrainGridModConfig modConfig, ToolbarHud toolbar, TerrainRenderer terrainRenderer)
    {
        this.modConfig = modConfig;
        this.terrainGridActivator = terrainRenderer.CreateGridLinesActivator();

        LocStr toolName = Loc.Str(id: "Toggle Terrain Grid", enUs: "Toggle Terrain Grid", comment: "ShowTerrainGridTool");
        Button toolButton = toolbar.AddToolButton(name: toolName, controller: this, iconAssetPath: "Assets/Unity/UserInterface/Toolbar/TerrainGrid.svg", order: 1f, shortcut: GetShortcut);
        toolButton.Selected(this.modConfig.IsEnabled);
    }


    private static KeyBindings GetShortcut(ShortcutsManager shortcutsManager)
    {
        return KeyBindings.FromPrimaryKeys(KbCategory.Tools, ShortcutMode.Game, UnityEngine.KeyCode.LeftShift, UnityEngine.KeyCode.F);
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
