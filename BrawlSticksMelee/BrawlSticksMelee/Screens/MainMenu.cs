using Microsoft.Xna.Framework;
using System;
using System.Windows.Forms;

namespace BrawlSticksMelee.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class MainMenu : MenuScreen
    {
        private BackgroundScreen background;

        public bool isActive = true;

        public int selection;

        public MainMenu(BackgroundScreen bg) : base("Brawl Sticks Melee")
        {
            background = bg;

            var singlePlayerMenuEntry = new MenuEntry("Single Player");
            var versusPlayerMenuEntry = new MenuEntry("Versus");
            //var instructionsMenuEntry = new MenuEntry("How to Play");
            // var creditsMenuEntry = new MenuEntry("Credits");
            var exitMenuEntry = new MenuEntry("Exit");

            singlePlayerMenuEntry.Selected += SinglePlayerMenuEntrySelected;
            versusPlayerMenuEntry.Selected += VersusPlayerMenuEntrySelected;
            //instructionsMenuEntry.Selected += InstructionsMenuEntrySelected;
            //creditsMenuEntry.Selected += CreditsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(singlePlayerMenuEntry);
            MenuEntries.Add(versusPlayerMenuEntry);
            //MenuEntries.Add(instructionsMenuEntry);
            //MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        private void SinglePlayerMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            this.ExitScreen();
            background.ExitScreen();
            isActive = false;
            selection = 10;
        }

        private void VersusPlayerMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            this.ExitScreen();
            background.ExitScreen();
            isActive = false;
            selection = -4;
        }


        private void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new Credits(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit the game?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
