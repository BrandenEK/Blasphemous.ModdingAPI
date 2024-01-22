
namespace Blasphemous.ModdingAPI.Input;

/// <summary>
/// The button code for each input action
/// </summary>
public enum ButtonCode
{
#pragma warning disable CS1591
    // Face buttons
    Attack = 5,
    Jump = 6,
    RangedAttack = 57,
    Interact = 8,
    // Triggers and bumpers
    Dash = 7,
    Flask = 23,
    Prayer = 25,
    Parry = 38,
    // Inventory navigation
    InventoryLeft = 28,
    InventoryRight = 29,
    InventoryScrollUp = 43,
    InventoryScrollDown = 45,
    InventoryLore = 64,
    // Dialog
    DialogNext = 35,
    DialogSkip = 39,
    // UI navigation
    UISubmit = 50,
    UICancel = 51,
    UIContextual = 52,
    UICenter = 60,
    UIOptions = 61,
    // Misc.
    Pause = 10,
    Inventory = 22,
    GrabCancel = 65,
#pragma warning restore CS1591
}
