# Title - Asteroids game using DOTS

## Created by Tor Nordmark

### How did I use DOTS

My usage of DOTS was somewhat limited to just using the Jobs system. My other implementation of improving performance was tweaking of older code and implementation of a pooling system.
Additional implementation include physics multithreading.

### How does it make efficient usage of computer hardware

My main focus was to reduce CPU workload during extended runtime and instead put the load at an early point in the operation.
The two main examples I have of this is the pool system that creates all asteroids I need at the start, instantiating them, but never activating. After that, they are simply activated and moved to the correct location at a regular interval using a repeating invoke.
Second example is using the job system. It sadly did not improve things much and I found out it was limited in its capacity, unable to handle gameobjects or referenced objects in general. All it gave me was making some mathematical equations on separate threads and then send them back to be handled by the main thread. It did however improve perfomance on the Spawn function from 0.09 ms to 0.06 ms.

### How was the code optimized using the profiler

As mentioned my main improvements were to the optimization with implementation of job system and the removal of instantiating during runtime. This was however not a huge improvement and could have been expanded on to include asteroid collision and their movement. Using the job system to move individual asteroids would have worked better than focusing on the spawner.


### Controls
WAD - Movement. AD to rotate, W to move in the direction the ship is facing
SPACE - Shoots