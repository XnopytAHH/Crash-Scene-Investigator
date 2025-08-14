# Crash Scene Investigator
![Crash Scene Investigator](Assets/Sprites/CSI%20Logo.png)
## Brief overview
Crash Scene Investigator is a game made to educate youth about Road Safety. Investigate crash sites and figure out the culprit, their wrongdoings and convict them!
### Controls
WASD - Movement<br/>
Space - Jump<br/>
E - Interact<br/>
Tab - Open Case File menu<br/>
Esc - Pause <br/>

## Intended platform and hardware
Platform: Windows<br/>
Hardware Specifications: FHD display (1920x1080)<br/>

## Known bugs and limitations
- Final screen showing the wait for game updates screen cannot be interacted with
- If hovering over evidence while transitioning scenes, it will stay in the UI indefinitely
- If interacting with the big boss multiple times after returning to the office, the final results screen will bug out
- Currently only 2 playable levels

## Finite State Machines
### Pedestrian FSM
The pedestrian FSM makes use of the NavMesh Agent component to navigate throughout the scene, following the Humanoid Navmesh and navigates only on sidewalks and crossings. It is put forcibly into Idle when waiting at traffic lights and talks to the player when interacted with<br/>
![Pedestrian FSM](FSM1.png)
### Car FSM
The Car FSM makes use of the NavMesh Agent component to navigate throughout the scene, following the Car Navmesh and navigates only on roads. It is put forcibly into Idle when waiting at traffic lights and stops to honk at player<br/>
![Car FSM](FSM2.png)
### Busybody FSM
The busybody NPC is the only enemy of the game, he acts the same as the pedestrian, but after a while will transition to blocking the player from picking up evidence. After a while more, he picks up the evidence himself and will go back to walking, and the player must retrieve it from the busybody.<br/>
![Busybody FSM](FSM3.png)


## External References
### Models
Trees: [Stylized Tree & Grass Samples(Symphonie Studio): Unity Assets Store](https://assetstore.unity.com/packages/3d/vegetation/trees/stylized-tree-grass-samples-304714)<br/>
### Textures
Stylized Nature Textures: [Stylized Nature Textures(Yoge): Unity Assets Store](https://assetstore.unity.com/packages/2d/textures-materials/stylized-nature-textures-228680)<br/>
### Code
Player Controller: [Starter Assets : Unity Assets Store](https://assetstore.unity.com/packages/essentials/starter-assets-character-controllers-urp-267961?srsltid=AfmBOopwNbftk8lbgmCCX9WvvS_A8_Uv_wj1qGvzOyF7IJB54vSBqTKp)<br/>
Item Outlines: [Quick Outlines (Chris Nolet): Unity Assets Store](https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488?aid=1101l9Bhe )<br/>
### Sounds
Collecting Objectives: [Item pickup](https://pixabay.com/sound-effects/item-pickup-37089/)<br/>
Player Win Sound: [You win sequence 1](https://pixabay.com/sound-effects/you-win-sequence-1-183948/)<br/>

## Game flow/Tutorial
Day 1
Evidences: 
- Headphones next to the victim
- Handphone next to the victim
- Talk to Andy nearby (Closest NPC that does not move)

Answers:
- Pedestrian Fault: Crossing while not focused
- Person at fault: Pedestrian

Day 2
Evidences: <br/>
- Speed camera on the left
- Dashcam dropped in front of the car
- Talk to Sarah nearby (Closest NPC that does not move)

Answers:
- Driver Fault: Speeding through a red light
- Person at fault: Purple car

Rank requirements:<br/>
F - Both Fault and Culprit wrongly identified <br/>
C - Only one Fault or Culprit correctly identified <br/>
B - Both the Fault and Culprit are correctly identified but 2 or less clues found<br/>
A - Previous requirement and all clues found<br/>
S - Previous requirements, but with more than 90s left<br/>

# Please Enjoy :D
