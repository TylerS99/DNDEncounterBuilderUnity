# DNDEncounterBuilderUnity
This is a small project meant to demonstrate some of my skill with Unity. It is an initiative, movement, and damage combat tracker for use as a companion to tabletop RPG's.

## How to use
As a general rule, right clicking will back out of what is currently being done (deselect a character, close ui, etc.).

In the start scene there is an infinitely scrolling list to select which map to use. Once a map is selected, the user can press start.

Pressing the "Add/Edit" Character button will open up the character creation UI. Entering the stats and the name of the model to use, then clicking "Add" and selecting a location will create a new character and set everything accordingly.

Once all characters have been added, clicking the "Next Turn" button will select the next character in initiative order. 
Clicking anywhere on the ground while a character is selected will move the character as close as possible to that point within its move range (holding shift overrides move range).

Clicking another character while a character is selected will move within melee range of them (hold shift for ranged attack), and open the damage ui.
Posiitive damage is healing, and negative is damage.

Holding shift and clicking the selected character is useful for self-healing.

At any time a character may be selected by clicking on it with nothing selected. This does not affect initiative order.

Clicking the "Add/Edit Character" button while a character is selected brings up the edit character UI. Pressing "Add" will confirm changes, "Remove" will delete the character, and right clicking will cancel.

Pressing the "Switch View" button switches between top-down and isometric views.

Pressing the "Exit" button will remove all characters and return the user to the map selection.
