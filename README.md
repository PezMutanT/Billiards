# Objective
This is a snooker game developed by a snooker enthusiast using Unity game engine. The game is aimed to target Android and PC platforms.

It is meant to be used as a vehicle to create a full Unity game while learning about some technological aspects, such as:
- Cinemachine virtual cameras.
- Render-to-texture cameras.
- URP (Universal Render Pipeline).
- Unity lighting systems and baked lights.
- Physics materials properties (bounciness, friction...).
- 3D modeling and UV texture mapping using Blender.
# Snooker rules
This game is implemented following the official snooker rules by The World Professional Billiards and Snooker Association (WPBSA) that can be found here https://wpbsa.com/wp-content/uploads/Rulebook-Website-Updated-May-2022-2.pdf
# How to play
In the game, the player takes the place of two different players alternating turns in order to one of them be the winner of the game (as of now, there is no AI that the player can play against). Current player is marked with an arrow at the bottom scoreboard.
## Rules summary
- Each player must alternate potting a red ball and a colored ball on each of their shots until there are no more balls in play or they miss potting a ball or commit a foul.
- If the first ball touched by the cue ball is other than the ball that needs to be potted (indicated in the bottom-middle of the screen), the player commits a foul and the turn goes to the other player.
- When there are no red balls left, the order in which the rest of the balls have to be potted is: yellow, green, brown, blue, pink and black.
## Aiming
Tap and hold your finger (or mouse) and then drag it around to move the trajectory of the shot. Bear in mind there are two different areas of the screen to do this:
- Left half of the screen is for regular aiming.
- Right half of the screen is for finetuning. You will notice that the trajectory changes much slower than it does when using regular aiming.
## Shooting
There is a "Shoot" button on the bottom-right corner of the screen.
- Tap it once for the shoot gauge to start filling up and down.
- Tap it again when the shoot gauge is appropriate for the shot you are trying to play.
- After tapping the button for the second time, the cue itself makes a little animation for about 1 or 2 seconds, so be patient and wait for the shot to automatically happen.
# TO-DO features
- Implement "ball-in-hand" feature for the player to be able to choose where to respot the cue ball.
- Allow the player to choose impact point on the cue ball in order to apply different kinds of spin to it.
- Post-processing for better visual outcome.
- Adjust shadows in Android build.
- Add cue 3D model.
