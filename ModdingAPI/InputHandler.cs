using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rewired;

namespace ModdingAPI
{
    /// <summary>
    /// A class for handling player input
    /// </summary>
    public class InputHandler
    {
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
        /// Checks whether a button was pressed during this frame
        /// </summary>
        /// <param name="btn">The button code to check for</param>
        /// <returns>The button status</returns>
        public bool GetButtonDown(int btn)
        {
            return Rewired != null && Rewired.GetButtonDown(btn);
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
    }
}
