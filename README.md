# Warframe Market Helper

This little background application can automatically sync your status in Warframe Market with your in-game status by detecting the currently active window and only setting you as "in game" on the website when in Warframe.

## Setup

Unfortunately, setting up this app requires a few steps. In order to access your Warframe Market account, you need to fetch your JWT authorization token. This can be done in Chrome as follows:

1. Navigate to https://warframe.market/.
2. Right next to the URL, there should be a lock icon. Click it.
3. In the menu that appears, click Cookies.
4. Double-click on the entry named "warframe.market", then again on the one named "Cookies".
5. Select the entry named "JWT" by clicking on it once.
6. Select the entirety of the contents of the "Content" text box. You can do so easily by triple-clicking the box.
7. Copy the contents (Ctrl+C).
8. With Warframe Market Helper running, look in your notification tray (right next to the clock on your task bar) for the app's icon and double-click it.
9. In the big text box named "Token", paste (Ctrl+V) the text you selected earlier.
10. Optionally tick the box "Start with Windows" so that the app is always automatically running in the background.
11. Click OK.

You should be done! If you now login to your account on another device or leave the website displayed on another monitor, you should see your status automatically change as you run the game or even alt-tab out of it.
