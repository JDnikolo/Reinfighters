### Script Summaries

#### Combatant Controller
Handles movement, actions, knockback and hit stun of a combatant. The script should be attached to the parent of a Player prefab.

### Auto Controller
Subclass of Combatant Controller for NPC combatants. Currently only selects actions randomly, favoring movement.

### Health
Handles combatant health, destroying them upon reaching 0.

### LifeBarSpriteChanger
Changes the sprite of the "Life Bar" segment of a combatant depending on its health.

### Hit Handler
Resolves weapon hits on players and on other weapons. Hits on players deal damage and cause knockback. Collision between weapons is used to register parries and cause the appropriate hit stun on the parried combatant.

### SwordSwingSO
Serializable Object containing the Transforms for the swing arc, movement speed and rotation speed of combatant sword swings used for combatant actions. Also contains the wind-up and follow through delays of the swing, as well as a active frame delay, which happens at the end of the swing but before its hurtbox disappears.

### Swing Pathfinder
Uses SwordSwingSO to move a combatant's sword and execute combatant actions, activating the sword's collision box (hurtbox) as needed. Each action uses its own SwordSwingSO. 

The first Transform in the swing is considered to be the wind-up position; when the sword reaches that position, it remains there for the duration of the wind-up delay before becoming active and continuing the swing. The last Transform in the swing is the follow-through position, where the hurtbox first remains for the active frame delay, then becomes inactive and finally remains there for the follow-through delay. Then it returns to its neutral position.

The Coroutines handling the swings are mutually exclusive; if a swing is underway, it cannot be cancelled by another. However, If the combatant gets hit, any active swing is cancelled.

