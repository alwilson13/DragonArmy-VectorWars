# Vector Wars

**Team:** Dragon Army  
**Engine:** Unity  
**Genre:** 2D Top-Down Twin-Stick Shooter  

## Overview

**Vector Wars** is a fast-paced 2D top-down twin-stick shooter where players survive increasingly difficult waves of enemies, collect XP and upgrades, switch weapons, and battle through intense arcade-style combat to reach the final boss and earn the highest score possible.

The game is inspired by classic arcade shooters and focuses on responsive movement, readable combat, wave progression, and simple but energetic visual feedback.

## Key Features

- Top-down twin-stick movement and aiming
- Fast arcade-style shooting combat
- Dodge ability with temporary invincibility
- Multiple enemy types with different behaviors
- Timed enemy wave system
- XP orbs and automatic level-up rewards
- Upgrade orb drops for speed, damage, fire rate, and health
- Weapon pickup system, including spread shot
- Final boss encounter
- Enemy spawn warning indicators
- Damage feedback, explosions, bullet trails, and visual polish
- Launch menu, pause menu, game over screen, and victory screen
- Music and SFX volume controls
- Windows build, WebGL build, and installer support

## Gameplay

The player must survive a sequence of increasingly difficult enemy waves. Enemies spawn over time instead of appearing all at once, giving the game better pacing while still increasing pressure.

Players earn XP by defeating enemies and collecting XP orbs. Leveling up rewards the player with healing or small stat improvements. Upgrade orbs and weapon pickups help the player grow stronger as the waves become more difficult.

The final goal is to survive all waves, defeat the boss, and finish with the highest score possible.

## Controls

| Action | Input |
|---|---|
| Move | WASD / Arrow Keys |
| Aim | Mouse |
| Shoot | Left Mouse Button |
| Dodge | Space |
| Pause | Escape |
| Collect XP / Pickups | Move over the item |

## Enemy Types

| Enemy | Description |
|---|---|
| Basic Chaser | Follows the player directly and applies contact pressure. |
| Charger | Tracks the player, then quickly charges forward. |
| Flanker | Moves around the player to attack from different angles. |
| Pattern Mover | Moves in predefined motion patterns. |
| Arena Patroller | Patrols between arena points and controls space. |
| Boss | Uses multiple attack patterns, including projectiles, ring attacks, and charge attacks. |

## Progression Systems

### XP and Leveling

Players collect XP orbs dropped by enemies. When enough XP is collected, the player levels up. Leveling up always provides a benefit:

- If the player is damaged, they receive healing.
- If the player is already at full health, they receive a small stat upgrade such as speed or damage.
- The player also receives a score bonus.

### Upgrade Orbs

Enemies can drop upgrade orbs that improve player stats:

- Fire rate upgrade
- Bullet damage upgrade
- Movement speed upgrade
- Max health upgrade

### Weapon Pickup

The player can collect a weapon pickup that changes the weapon behavior, such as switching to a spread-shot weapon.

## Development Notes

This project was developed as part of a team-based game development milestone project. The team focused on building a complete playable arcade shooter from prototype to final build.

Major development phases included:

- Greenlight and concept planning
- Pre-production design
- First playable prototype
- Alpha milestone
- Beta polish and bug fixing
- Gold release preparation
- Postmortem reflection

## What Went Well

- The core gameplay loop became playable and fun.
- Simple visuals helped keep combat readable.
- XP, upgrades, and wave progression improved replay value.
- Testing helped identify and fix important gameplay, UI, WebGL, and installer issues.

## Lessons Learned

- Start with a small playable version before adding extra systems.
- Test every feature immediately after adding it.
- Balance gameplay while building, not only at the end.
- Keep communication, Git workflow, and task ownership clear.
- WebGL and Windows builds may behave differently, so both need testing.

## Team Credits

**Dragon Army**

Possible contribution areas included:

- Gameplay programming
- Player movement and combat
- Enemy AI
- Wave and level design
- Boss design
- UI and HUD
- XP and upgrade systems
- Audio implementation
- Visual polish
- Build and deployment
- QA testing and bug fixing
- Project management and Trello/GitHub organization

## Installation

### Windows Build

1. Download the Windows build or installer.
2. If using the zipped build, extract the full folder.
3. Run the game executable.

Important: Unity games require the `.exe`, `_Data` folder, and supporting files to stay together. Do not move only the `.exe`.

### WebGL Build

The game can also be built for WebGL and uploaded to Unity Play or another supported web host.

## Build Instructions

To build the project in Unity:

1. Open the project in Unity.
2. Go to **File → Build Settings** or **Build Profiles**.
3. Add the main scene to the build.
4. Select the target platform:
   - Windows for `.exe`
   - WebGL for browser build
5. Click **Build**.
6. Test the build outside of the Unity Editor.

## Tools Used

- Unity
- C#
- GitHub
- Trello
- Inno Setup
- TextMeshPro
- Unity WebGL

## Repository Structure

The project may include folders such as:

```text
Assets/
├── Audio/
├── Materials/
├── Prefabs/
├── Scenes/
├── Scripts/
├── Sprites/
└── UI/
```

## Status

This project reached a final playable milestone with menus, waves, enemies, upgrades, boss fight, audio, visual feedback, and build support.

## License

This project is for educational and portfolio purposes. Asset ownership and third-party audio or visual assets should be reviewed before public commercial release.
