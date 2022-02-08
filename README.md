# MinewseeperCoop

The well-known Minesweeper game, only now with multiplayer

Multiplayer was made and tested for playing on a local server (via radmin or hamachi). 
Also, the server and the client are written quite simply, according to the simplest algorithm, 
with my most likely incorrect implementation of the fragments (I just fixed the bug), 
and although I tried to make a handler for all errors, they can still occur. 
However, in general it works, albeit buggy

I drew the graphics myself,
but I tried to make everything look like the original, 
only used more pastel colors

There are no buttons and input fields in the framework (monogame), or I just searched badly. 
Therefore, I wrote everything myself in haste, so you will not see any comments or thoughtful architecture. 
I generally scored on inheritance

Although this was my third attempt, I never thought through the game, and I wrote this code in less than a day (if we take the total time). 
Therefore, the code has few comments, is written ugly, and the algorithms are most likely not optimized. 
Apart from the architecture of the project, I doubt that it is possible to expand the project without tears and pain.

And the last terrible problem is optimization. I have no idea what the app is loading.
Framework rendering, or my algorithms written along the way (which is unlikely, because there should not be a load without pressing). 
In general, it is better to set the maximum size to 12 and maximum 20 bombs.

The interface, as I said, was made badly, so it's hard to understand what's going on there.

upper field - input ip
middle field - enter the margin size
bottom field - enter the number of bombs
