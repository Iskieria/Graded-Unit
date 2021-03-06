﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion
#region copyright
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
#endregion

namespace GradedUnitGame
{
    abstract class MenuScreen : GameScreen
    {
        #region attributes
        //loads a new content manager for the menu content
        ContentManager content;

        //creates the menu entries as a new menu entry list
        List<MenuEntry> menuEntries = new List<MenuEntry>();

        //holds the integer for the entry selection
        int selEntry = 0;

        //holds the title of the menu
        string menuTitle;

        //holds the menu music
        SoundEffect menuMusic;
        SoundEffectInstance menuMusicInstance;
        #endregion

        #region initilization
        //loads screen content
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            menuMusic = this.content.Load<SoundEffect>("./Sounds/DST-2ndBallad");
            menuMusicInstance = menuMusic.CreateInstance();

            //plays the menu music, unless music is disabled in menu
            if (OptionsScreen.currentMusic == OptionsScreen.music.Off)
            {
                menuMusicInstance.Stop();
            }
            else if (OptionsScreen.currentMusic == OptionsScreen.music.On && menuMusicInstance.State == SoundState.Stopped)
            {
                menuMusicInstance.IsLooped = true;
                menuMusicInstance.Play();
            }
            else if (menuMusicInstance.State == SoundState.Playing)
            {
                menuMusicInstance.Pause();
            }
        }
        #endregion

        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        //constructor for the menu screen
        public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;

            TransOnTime = TimeSpan.FromSeconds(0.5);
            TransOffTime = TimeSpan.FromSeconds(0.5);
        }

        //Handles player input for the menu screens
           public override void HandleInput(InputState input)
        {
            //moves to previous menu entry
            if (input.IsMenuUp(ConPlayer))
            {
                selEntry--;

                if (selEntry < 0)
                    selEntry = menuEntries.Count - 1;
            }

            //moves to next menu entry
            if (input.IsMenuDown(ConPlayer))
            {
                selEntry++;

                if (selEntry >= menuEntries.Count)
                    selEntry = 0;
            }

            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ConPlayer, out playerIndex))
            {
                OnSelectEntry(selEntry, playerIndex);
            }
            else if (input.IsMenuCancel(ConPlayer, out playerIndex))
            {
                OnCancel(playerIndex);
            }
        }

           //Event handler for when the player selects a menu entry
           protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
           {
               menuEntries[entryIndex].OnSelectEntry(playerIndex);
           }

           //event handler for when the player selects cancel
           protected virtual void OnCancel(PlayerIndex playerIndex)
           {
               Exit();
           }

        //event handler for when the player selects cancel
           protected void OnCancel(object sender, PlayerIndexEventArgs e)
           {
               OnCancel(e.PlayerIndex);
           }

           #region draw & update
           /// <summary>
           ///Allows screen to position menu entries 
           /// </summary>
           protected virtual void UpdateMenuEntryLocations()
           {
               //makes menu slide into place during transitions
               float transOffset = (float)Math.Pow(TransPos, 2);

               //start at Y = 175; each X value is generated per entry
               Vector2 pos = new Vector2(0f, 175f);

               //update each menu entry's location
               for (int i = 0; i < menuEntries.Count; i++)
               {
                   MenuEntry menuEntry = menuEntries[i];

                   //centered horizontally
                   pos.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                   if (ScreenState == ScreenState.TransOn)
                       pos.X -= transOffset * 256;
                   else
                       pos.X += transOffset * 512;

                   //set entry's position
                   menuEntry.Pos = pos;

                   //moves down for next entry the size of this entry
                   pos.Y += menuEntry.GetHeight(this);
               }
           }


           /// <summary>
           /// updates the menu
           /// </summary>
           public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                          bool coveredByOtherScreen)
           {
               base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

               //updates each nested MenuEntry 
               for (int i = 0; i < menuEntries.Count; i++)
               {
                   bool isSelected = IsActive && (i == selEntry);
                   menuEntries[i].Update(this, isSelected, gameTime);

               }
           }


           /// <summary>
           /// draws the menu
           /// </summary>
           public override void Draw(GameTime gameTime)
           {
               //make sure entries are in the right place before drawing them
               UpdateMenuEntryLocations();

               GraphicsDevice graphics = ScreenManager.GraphicsDevice;
               SpriteBatch sBatch = ScreenManager.SpriteBatch;
               SpriteFont font = ScreenManager.Font;

               sBatch.Begin();

               //draw each menu entry
               for (int i = 0; i < menuEntries.Count; i++)
               {
                   MenuEntry menuEntry = menuEntries[i];

                   bool isSelected = IsActive && (i == selEntry);

                   menuEntry.Draw(this, isSelected, gameTime);
               }

               //makes the menu slide into place during transitions
               float transOffset = (float)Math.Pow(TransPos, 2);

               //draws the menu title centered
               Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
               Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
               Color titleColor = new Color(192, 192, 192) * TransAlpha;
               float titleScale = 1.25f;

               titlePosition.Y -= transOffset * 100;

               sBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                      titleOrigin, titleScale, SpriteEffects.None, 0);

               sBatch.End();
           }
           #endregion
    }
           
}
