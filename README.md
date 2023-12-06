# Hand Battle
This is a test submission for Stellar Play
Game rules
- Tap Play in Menu screen
- Choose a hand element of your choice(if you win continuosly you will form a streak)
- Try to beat your previous streak every time
- You can switch ON/OFF the music and sfx separately in the Menu screen
- Inside project if you want to add or remove a Hand element just add or remove the enum for it
- if you are adding a enum you have to set it up inside Rules Scriptable object asset
- You also have to modify the ELement Database Scriptable object asset, you have to set up values and references of related asset for that element

# Design patterns used in the game are
- Service Locator (Gamemanager.cs is Service factory and there are some local and global services, e.g. AudioService and PlayerService are 2 global services which are inititalized in "InitScene")
- Flyweight pattern (To setup rules and Hand element database this pattern is being used)
- Observer pattern (GameplayController.cs and GameLoopController.cs are following this pattern, events are invoked from GameplayController and they are subscribed inside GameLoopController)

# External resources used in peoject other than core unity features
- DoTween plugin from the Asset store of Unity
- Audio and sprite assets form OpenGameArt.org
- Some sprite assets used were found in Google search and downloaded from there

# One fun Idea for its meta is
- Give player option in the menu to take only 25 card of his choice
- Both the player will thus have same number of cards but type would vary
- You have to take atleast one card from each type
- Then the fun part will be "Player has to keep count of his card and opponent's card as well
- In the end Player with most round wins will win the match

# Note
- I tried to write the code as good as possible, but due to my main office work, I was very juggled between work and this test, still I tried to write my Best. Hope you like it. :)
