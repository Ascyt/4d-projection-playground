# 4D Projection Playground

## What is this?

This is an orthographic projection of a 4-dimensional hypercube (also called a tesseract, which is basically a cube but in four instead of three dimensions) in 2D.

By introducing another spatial dimension, we are adding a new coordinate to the system, the w axis - instead of just having x, y, z we now have x, y, z, w. However, since our brains are designed to understand 3D and not 4D, it might be really difficult or even impossible for one to truly wrap their mind around this new dimension. 

What you see is a wireframe model of the tesseract. The darker the lines, the "farther away" that line is from the viewport - since to render the cube we have to hide two out of the four dimensions (z and w). In the center you can see the coordinates: <span style="color:#ff0000">x</span> is <span style="color:#ff0000">red</span>, <span style="color:#00ff00">y</span> is <span style="color:#00ff00">green</span>, <span style="color:#0080ff">z</span> is <span style="color:#0080ff">blue</span>, <span style="color:#ffff00">w</span> is <span style="color:#ffff00">yellow</span>. There is also a <span style="color:#ff00ff">magenta point</span>, which is at the position (<span style="color:#ff0000">0.5</span>, <span style="color:#00ff00">0.5</span>, <span style="color:#0080ff">0.5</span>, <span style="color:#ffff00">0.5</span>) at the start.

It's possible to rotate the tesseract using the **interface at the bottom of the screen**. You can click on the "**Randomize Rotation**" button to get a random rotation, or you can manually play around with the six **planes of rotation** (in 4D it's not possible to rotate solely on axes - rotations are fundamentally two-dimensional, which isn't the case in 3D). It is also important to note that the shown axes rotate with the tesseract, and are only aligned with the true axes of the simulation at the start, when no rotation is applied.

It might be difficult to wrap your head around what is happening here - that is understandable. I honestly couldn't either. However, I am fascinated by the fact that the cube actually looks like it's spinning (in a weird way that is), instead of it looking like random lines transforming in random ways, and I think that's pretty cool. 