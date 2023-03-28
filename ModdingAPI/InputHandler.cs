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

        /// <summary>
        /// The axis code for each input action
        /// </summary>
        public enum AxisCode
        {
#pragma warning disable CS1591
            MoveHorizontal = 0,
            MoveVertical = 4,
            MoveRHorizontal = 21,
            MoveRVertical = 20,
            UIHorizontal = 48,
            UIVertical = 49,
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
        /// <param name="button">The button code to check for</param>
        /// <returns>The button status</returns>
        public bool GetButton(ButtonCode button)
        {
            return Rewired != null && Rewired.GetButton((int)button);
        }

        /// <summary>
        /// Checks whether a button was pressed during this frame
        /// </summary>
        /// <param name="button">The button code to check for</param>
        /// <returns>The button status</returns>
        public bool GetButtonDown(ButtonCode button)
        {
            return Rewired != null && Rewired.GetButtonDown((int)button);
        }

        /// <summary>
        /// Checks whether a button was released during this frame
        /// </summary>
        /// <param name="button">The button code to check for</param>
        /// <returns>The button status</returns>
        public bool GetButtonUp(ButtonCode button)
        {
            return Rewired != null && Rewired.GetButtonUp((int)button);
        }

        /// <summary>
        /// Checks the direction that an axis is held at from -1 to 1
        /// </summary>
        /// <param name="axis">The axis code the check for</param>
        /// <param name="useRawInput">Whether to use the raw axis data</param>
        /// <returns>The axis status</returns>
        public float GetAxis(AxisCode axis, bool useRawInput)
        {
            return Rewired != null && useRawInput ? Rewired.GetAxisRaw((int)axis) : Rewired.GetAxis((int)axis);
        }

        /// <summary>
        /// Checks the direction that an axis was held at on the last frame from -1 to 1
        /// </summary>
        /// <param name="axis">The axis code the check for</param>
        /// <param name="useRawInput">Whether to use the raw axis data</param>
        /// <returns>The axis status</returns>
        public float GetAxisPrevious(AxisCode axis, bool useRawInput)
        {
            return Rewired != null && useRawInput ? Rewired.GetAxisRawPrev((int)axis) : Rewired.GetAxisPrev((int)axis);
        }

        /// <summary>
        /// Checks the time that an axis has been held
        /// </summary>
        /// <param name="axis">The axis code the check for</param>
        /// <param name="useRawInput">Whether to use the raw axis data</param>
        /// <returns>The axis status</returns>
        public float GetAxisTimeActive(AxisCode axis, bool useRawInput)
        {
            return Rewired != null && useRawInput ? Rewired.GetAxisRawTimeActive((int)axis) : Rewired.GetAxisTimeActive((int)axis);
        }
    }
}
