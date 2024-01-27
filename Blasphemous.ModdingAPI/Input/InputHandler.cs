using Rewired;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Input;

/// <summary>
/// Provides access to custom keybindings and better input
/// </summary>
public class InputHandler
{
    private readonly BlasMod _mod;

    private bool _registered = false;
    private readonly Dictionary<string, KeyCode> _keybindings = [];

    private Player rewired;
    private Player Rewired => rewired ??= ReInput.players?.GetPlayer(0);

    internal InputHandler(BlasMod mod) => _mod = mod;

    // Keys

    /// <summary>
    /// Checks whether the key was held on this frame
    /// </summary>
    public bool GetKey(string action)
    {
        return _keybindings.TryGetValue(action, out KeyCode key) && UnityEngine.Input.GetKey(key);
    }

    /// <summary>
    /// Checks whether the key was pressed on this frame
    /// </summary>
    public bool GetKeyDown(string action)
    {
        return _keybindings.TryGetValue(action, out KeyCode key) && UnityEngine.Input.GetKeyDown(key);
    }

    /// <summary>
    /// Checks whether the key was released on this frame
    /// </summary>
    public bool GetKeyUp(string action)
    {
        return _keybindings.TryGetValue(action, out KeyCode key) && UnityEngine.Input.GetKeyUp(key);
    }

    // Buttons

    /// <summary>
    /// Checks whether the button was held on this frame
    /// </summary>
    public bool GetButton(ButtonCode button)
    {
        return Rewired != null && Rewired.GetButton((int)button);
    }

    /// <summary>
    /// Checks whether the button was pressed on this frame
    /// </summary>
    public bool GetButtonDown(ButtonCode button)
    {
        return Rewired != null && Rewired.GetButtonDown((int)button);
    }

    /// <summary>
    /// Checks whether the button was released on this frame
    /// </summary>
    public bool GetButtonUp(ButtonCode button)
    {
        return Rewired != null && Rewired.GetButtonUp((int)button);
    }

    // Axes

    /// <summary>
    /// Checks the current direction of this axis
    /// </summary>
    public float GetAxis(AxisCode axis, bool useRawInput)
    {
        return Rewired == null ? 0 : useRawInput ? Rewired.GetAxisRaw((int)axis) : Rewired.GetAxis((int)axis);
    }

    /// <summary>
    /// Checks the direction of this axis on the last frame
    /// </summary>
    public float GetAxisPrevious(AxisCode axis, bool useRawInput)
    {
        return Rewired == null ? 0 : useRawInput ? Rewired.GetAxisRawPrev((int)axis) : Rewired.GetAxisPrev((int)axis);
    }

    /// <summary>
    /// Checks the time that this axis has been held
    /// </summary>
    public float GetAxisTimeActive(AxisCode axis, bool useRawInput)
    {
        return Rewired == null ? 0 : useRawInput ? Rewired.GetAxisRawTimeActive((int)axis) : Rewired.GetAxisTimeActive((int)axis);
    }

    // Custom keybindings

    /// <summary>
    /// Specifies which keybindings will be loaded and registers their defaults
    /// </summary>
    public void RegisterDefaultKeybindings(Dictionary<string, KeyCode> defaults)
    {
        if (_registered)
        {
            _mod.LogWarning("InputHandler has already been registered!");
            return;
        }
        _registered = true;

        foreach (var mapping in defaults)
        {
            _keybindings.Add(mapping.Key, mapping.Value);
        }

        DeserializeKeybindings(_mod.FileHandler.LoadKeybindings());
        _mod.FileHandler.SaveKeybindings(SerializeKeyBindings());
    }

    /// <summary>
    /// When saving the keybindings to a file, convert them to a list of strings
    /// </summary>
    private string[] SerializeKeyBindings()
    {
        string[] keys = new string[_keybindings.Count];
        int currentIdx = 0;

        foreach (var mapping in _keybindings)
        {
            keys[currentIdx++] = $"{mapping.Key}: {mapping.Value}";
        }

        return keys;
    }

    /// <summary>
    /// When loading the keybindings from a file, convert and validate their keycodes
    /// </summary>
    private void DeserializeKeybindings(string[] keys)
    {
        foreach (string line in keys)
        {
            // Skip lines without a colon
            int colon = line.IndexOf(':');
            if (colon < 0)
                continue;

            // Get action and key for each pair
            string key = line.Substring(0, colon).Trim();
            string term = line.Substring(colon + 1).Trim();

            // If the keybinding wasn't in the defaults, skip
            if (!_keybindings.ContainsKey(key))
            {
                continue;
            }

            try
            {
                // Update the valid keybinding
                object keycode = System.Enum.Parse(typeof(KeyCode), term);
                _keybindings[key] = (KeyCode)keycode;
            }
            catch
            {
                // If the keybinding was not a valid type, skip
                _mod.LogError($"Keybinding '{key}' is invalid.  Using default instead.");
                continue;
            }
        }
    }
}
