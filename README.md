# Survival Arena
---

## How to Run
### Run a built executable

If you've downloaded the build folder (Assets/Builds/AlphaTestv1.1):

1. Extract the build folder.
2. Run the `.exe`.
3. No installation needed — just double-click to launch.

---

## First-Time Setup

The game has a simple account system so it can track your high score:

1. From the main menu, click **Play**.
2. On the login screen, click **CREATE NEW ACCOUNT**.
3. Enter a username (3–20 characters) and a password (6+ characters), confirm the password, then click **CREATE ACCOUNT**.

Next time you play, log in with the same username and password — your high score and stats carry over.

Account data is stored locally in `Assets/LoginDetails/players.json`.

---

## Controls

| Action | Input |
|---|---|
| Move | `W` `A` `S` `D` |
| Attack | Left Mouse Click |
| Heal (costs 10 gold) | `E` |
| Pause menu | `ESC` |
| Upgrade menu | `Tab` |

---

## How to Play

1. **Survive waves.** Slimes spawn from the edges of the arena in increasing numbers each wave.
2. **Kill slimes for gold.** Every kill drops gold, which appears top-right.
3. **Spend gold to survive longer:**
   - Press `E` to heal 25 HP for 10 gold.
   - Press `Tab` to open the upgrade menu and permanently boost your stats:
     - **Damage** — hit harder
     - **Attack Speed** — attack more often
     - **Move Speed** — move faster
     - **Defense** — take less damage
     - **Max Health** — higher HP pool
     - **Coin Multiplier** — earn more gold per kill
   - Upgrade costs increase each time you buy one, so plan your priorities.
4. **Watch your health bar** (top-left) — it goes green → yellow → red as you take damage.
5. **Survive as many waves as possible.** When you die, you'll see your final wave reached and gold earned, plus whether you set a new personal best.

---

## Difficulty Settings

From the pause menu (`ESC`) → **Settings**, you can change difficulty at any time:

| Difficulty | Enemy Health | Enemy Damage | Spawn Rate |
|---|---|---|---|
| Easy | 70% | 60% | 1.2x |
| Normal | 100% | 100% | 1.5x |
| Hard | 150% | 140% | 2.0x |

The same menu also has volume sliders (Master / Music / SFX) and a screen shake toggle.

---

## System Requirements

- **OS:** Windows
- **RAM:** 1 GB minimum (Not tested
- **Storage:** ~10MB
- Runs comfortably at 60 FPS on ancient hardware
