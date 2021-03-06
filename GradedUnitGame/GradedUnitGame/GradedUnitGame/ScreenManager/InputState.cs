﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion
#region copyright
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
#endregion

namespace GradedUnitGame
{
   public class InputState
    {
       //holds the maximum allowed number of inputs
        public const int MaxInputs = 2;

       //holds the current keyboard state
        public readonly KeyboardState[] CurrentKeyboardStates;
       //holds the current gamepad state
        public readonly GamePadState[] CurrentGamePadStates;

       //holds the last keyboard state
        public readonly KeyboardState[] LastKeyboardStates;
       //holds the last gamepad state
        public readonly GamePadState[] LastGamePadStates;

       //holds if a gamepad was ever connected
        public readonly bool[] GamePadWasConnected;
       
       //constructor
        public InputState()
        {
            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];

            LastKeyboardStates = new KeyboardState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];
        }

        /// <summary>
        /// reads latest state of keyboard & gamepad.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                LastKeyboardStates[i] = CurrentKeyboardStates[i];
                LastGamePadStates[i] = CurrentGamePadStates[i];

                CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                //keeps track of whether a gamepad was ever plugged in, to detect if it gets disconnected
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }                                                                       
        }

             public bool IsNewKeyPress(Keys key, PlayerIndex? conPlayer,
                                            out PlayerIndex playerIndex)
        {
            if (conPlayer.HasValue)
            {
                // Reads input from a specified player.
                playerIndex = conPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentKeyboardStates[i].IsKeyDown(key) &&
                        LastKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                // Accept input from any player.
                return (IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Two, out playerIndex)); 
            }
        }


        /// <summary>
        /// Helper for checking if a button was pressed during update.
        /// </summary>
        public bool IsNewButtonPress(Buttons button, PlayerIndex? conPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (conPlayer.HasValue)
            {
                // Reads input from a specified player.
                playerIndex = conPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button) &&
                        LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                // Accept input from any player.
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Two, out playerIndex)); 
            }
        }


        /// <summary>
        ///  determines if player has fired the laser
       /// </summary>
        public bool isFired(PlayerIndex? conPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Space, conPlayer, out playerIndex) ||
                IsNewKeyPress(Keys.F, conPlayer, out playerIndex) ||
                IsNewButtonPress(Buttons.A, conPlayer, out playerIndex);
              
        }
       public bool isp2Fired(PlayerIndex?conPlayer)
        {
            PlayerIndex playerIndex;
            return IsNewKeyPress(Keys.NumPad0, conPlayer, out playerIndex);
        }

        /// <summary>
        /// Checks for "menu select" input
        /// conPlayer specifies which player to read input from
        /// </summary>
        public bool IsMenuSelect(PlayerIndex? conPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Space, conPlayer, out playerIndex) ||
                   IsNewKeyPress(Keys.Enter, conPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.A, conPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Start, conPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for  "menu cancel" input 
        /// conPlayer specifies which player to read input from
        /// </summary>
        public bool IsMenuCancel(PlayerIndex? conPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Escape, conPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.B, conPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Back, conPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for "menu up" input 
        /// conPlayer specifies which player to read from
        /// </summary>
        public bool IsMenuUp(PlayerIndex? conPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Up, conPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.DPadUp, conPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.LeftThumbstickUp, conPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for "menu down" input
        /// conPlayer specifies which player to read input from
        /// </summary>
        public bool IsMenuDown(PlayerIndex? conPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Down, conPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.DPadDown, conPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.LeftThumbstickDown, conPlayer, out playerIndex);
        }

        /// <summary>
        /// Checks for "pause game" input
        /// conPlayer specifies which player to read from
        /// </summary>
        public bool IsPauseGame(PlayerIndex? conPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Escape, conPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Back, conPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Start, conPlayer, out playerIndex);
        }

    }

}
