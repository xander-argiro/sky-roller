# Changelog
Added a "slow down" hazard that slows the movement speed of the player

Added a "shove" hazard that moves the player to the side

Added a "game over" state with a restart and quit game button

Added endless procedural generation
- There are currently only 4 chunks that can generate
- Each chunk has an "exit" and an "entrance" that can be left, right, or middle
- The previously generated chunk must have an exit that lines up with the entrance of the next chunk
