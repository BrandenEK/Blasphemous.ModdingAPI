using Rewired;

namespace ModdingAPI
{
    /// <summary>
    /// A class for handling player input
    /// </summary>
    public class InputHandler
    {
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
            UIVertical = 49,
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

        private Player rewired;
        private Player Rewired
        {
            get
            {
                if (rewired == null && ReInput.players != null && ReInput.players.GetPlayer(0) != null)
                    rewired = ReInput.players.GetPlayer(0);
                return rewired;
            }
        }

        /// <summary>
        /// Checks whether a button is currently being held down
        /// </summary>
        /// <param name="btn">The button code to check for</param>
        /// <returns>The button status</returns>
        public bool GetButton(int btn)
        {
            return Rewired != null && Rewired.GetButton(btn);
        }
        /// <summary>
        /// Checks whether a button is currently being held down
        /// </summary>
        /// <param name="btn">The button code to check for</param>
        /// <returns>The button status</returns>
        public bool GetButton(ButtonCode btn)
        {
            return GetButton((int)btn);
        }

        /// <summary>
        /// Checks whether a button was pressed during this frame
        /// </summary>
        /// <param name="btn">The button code to check for</param>
        /// <returns>The button status</returns>
        public bool GetButtonDown(int btn)
        {
            return Rewired != null && Rewired.GetButtonDown(btn);
        }
        /// <summary>
        /// Checks whether a button was pressed during this frame
        /// </summary>
        /// <param name="btn">The button code to check for</param>
        /// <returns>The button status</returns>
        public bool GetButtonDown(ButtonCode btn)
        {
            return GetButtonDown((int)btn);
        }

        /// <summary>
        /// Checks whether a button was released during this frame
        /// </summary>
        /// <param name="btn">The button code to check for</param>
        /// <returns>The button status</returns>
        public bool GetButtonUp(int btn)
        {
            return Rewired != null && Rewired.GetButtonUp(btn);
        }
        /// <summary>
        /// Checks whether a button was released during this frame
        /// </summary>
        /// <param name="btn">The button code to check for</param>
        /// <returns>The button status</returns>
        public bool GetButtonUp(ButtonCode btn)
        {
            return GetButtonUp((int)btn);
        }
    }
}
