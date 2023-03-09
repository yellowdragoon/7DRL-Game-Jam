# 7DRL-Game-Jam
Repo for our team's project for the 7DRL Jam using Unity Engine

## This is the Music Branch
The cache was cleared before the latest commit, but all other files have been committed. When merging, some of these files might not be necessary, but I'll leave that up to someone with better judgement than I. 

### Background Music - Dungeon Theme
The Background Music Game Object has been added which prompts the music to begin upon the object being started. This music loops forever unless a stop condition is activated (such as encountering a boss), which will need to be implemented. There are 3 parameters which allow for the dynamic effect of the adaptive music. They are as follows:

  * __Threat Level__: This parameter goes from 0 to 4. It is meant to be directy proportional to either the amount of enemies currently in the area, or perhaps to the cumulative threat level of all the enemies in the area (for instance some stronger enemies might have a threat level of 2 or 3, raising the threat level more drastically when they are onscreen). This parameter brings in a Choir and a Drumset to indicate threat.

  * __Progress__: This parameter goes from 0 to 4. It is meant to be be directly proportional to the amount of progress the player has made in the game - whether this means distance travelled, time played, items collected or ammassed power is up to the programmer. This parameter brings in a Bass Guitar and a Square Synthesizer to spice up the existing theme.

  * __Tense__: This parameter goes from 0 to 3. It is meant to be directly proportional to how close the player is to a Boss (or perhaps closeness to the fog, if that is implemented). This parameter phases out all percussion and brings in some Tremolo Violins to raise the tension. 

It should be noted that these parameters can have floating point values, however they have been designed in such a way that the whole number values should be the actual values used. However, when incrementing or decrementing these parameters, in order to make sure the change is smooth, it would be better to increment in smaller steps, such as 0.1, over a short timespan, such as 1 second, when going from one level to another, i.e. instead of going from 0 to 1 instantly, it goes 0, 0.1, 0.2, ..., 0.9, 1 over the space of a second. 

It should also be noted that these parameters are not mutually exclusive: for instance, the __Tense__ music at level 3 will sound different if the __Progress__ and __Threat Level__ parameters are at level 4. This means that using different combinations of these parameters can lead to slightly different music, helping to create greater variation in the background theme.

Not every parameter needs to be used, and more can be added or existing ones (such as Progress) edited upon request.

### Boss Theme - War of the Hero
The Boss Music Game Object has been added which will start upon a prompt that needs to be added by the programmer (such as encountering a boss). This music loops forever unless a stop condition is activated (such as defeating the boss), which will need to be implemented. This music is not adaptive.

### Title Theme - Lorem Ipsum
This music has yet to be written, however this music will not be adaptive and will simply play on the title screen of the game, looping until the game is started.
