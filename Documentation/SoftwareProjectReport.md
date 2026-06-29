# SURVIVAL ARENA - SOFTWARE ENGINEERING PROJECT DOCUMENTATION

---

| **Project Title** | Survival Arena |
|----------|-------|
| **Student Name** | Yyoung Du |
| **Date** | 1st July |
| **Course** | Software Engineering Stage 6 |
| **Engine** | Unity 6 |
| **Language** | C# |
---

# 1. IDENTIFYING AND DEFINING

## 1.1 Problem Statement

### The Problem: Student Stress and Inadequate Break-Time Coping Mechanisms

**Problem Identification:**

Students, especially senior and university goers, experience high levels of academic and social stress, particularly during exam periods and study cycles. While stress is normal, many students lack healthy, accessible coping mechanisms for short break periods (15-30 minutes) between study sessions. Existing stress-relief options either:
- Require a long time commitment (meditation apps, exercise)
- Involve costs students cannot afford (gym memberships, therapy)
- Contain distracting elements that worsen stress (social media, ads)
- Lack sufficient engagement for meaningful mental breaks

**Evidence of the Problem:**
- 68% of high school students report experiencing stress levels that interfere with schoolwork (Australian Psychological Association)
- Students during exam periods average 2-4 short study break periods per day with no structured relief activity (Personal Experience)
- Free entertainment options (mobile games, social media) often increase stress through ads, microtransactions, and dopamine-chasing mechanics
- Students report difficulty "switching off" after intense study due to lack of satisfying engagement outlets

**Who is Affected:**
- **Primary:** Students aged 14-18 during high-pressure academic periods (exams, assessments, project deadlines)
- **Secondary:** Teachers seeking to support student wellbeing, parents concerned about screen time quality, peers requiring healthy stress-relief mechanisms

**Significance:**
This problem is significant because:
- Unmanaged student stress directly impacts academic performance, sleep quality, and mental health
- Accessible stress-relief improves focus and effectiveness for subsequent study sessions
- Healthy coping mechanisms reduce need for unhealthy alternatives like doomscrolling
- Well-designed entertainment can provide break while maintaining focus

**Why Software Solution?**
A digital game is the appropriate solution because:
- **Immediate:** Provides instant mental break without setup
- **Accessible:** Runs on low end computers; no special hardware; free access also removes financial barrier
- **Time-Efficient:** Designed for 15-30 minute sessions (aligns with natural study break patterns)
- **Engaging:** Interactive mechanics hold attention better than passive media, providing genuine mental break
- **Non-Addictive:** Arcade design focuses on skill/challenge, not manipulation or pay-to-win mechanics
- **Healthy:** No ads, no microtransactions, no social comparison elements that increase stress

---

## 1.2 Project Purpose and Boundaries

**Purpose:**
Develop an engaging, stress-relieving arcade game suitable for students during academic break periods. The game should provide immediate mental engagement, satisfying progression, and healthy challenge without requiring long-term time commitment or financial investment.

**Project Goals:**
1. Create a fully playable game that provides stress relief through engaging mechanics
2. Design progression systems that provide satisfaction and sense of achievement
3. Ensure responsive, skill-based controls that reward player improvement
4. Implement difficulty options for players of all skill levels
5. Create intuitive UI that doesn't create additional frustration
6. Optimize for quick play sessions (15-30 minute target)

**Scope**

**Included:**
- Single-player arcade gameplay focused on skill and progression
- Wave-based combat with increasing difficulty
- Upgrade system providing meaningful progression and sense of achievement
- Multiple difficulty levels for accessibility
- Responsive controls rewarding skill improvement
- Intuitive menus for quick game startup

**Excluded:**
- Multiplayer/competitive features (increase stress through comparison)
- Aggressive monetization or ads (contradicts stress-relief goal)
- Narrative or story elements (add complexit)
- Multiple maps or complex strategies (keeps focus simple for break periods)

**Boundaries:**
- Game sessions designed for 20-40 minute playtimes
- No progression carries between game restarts (prevents addiction)
- No permanent unlocks or cosmetics (prevents FOMO - fear of missing out)
- Single player only (no social pressure)
- Runs locally; no internet connection required

---

## 1.3 Stakeholder Analysis

**Stakeholders Identified:**

### 1. Student Players (Primary Users)
- **Needs:** Stress relief, mental break from academics, engaging but time-limited entertainment
- **Expectations:** Fun, responsive controls, fair difficulty progression, no frustration/ads
- **Influence:** Drove focus on accessibility, clear feedback, satisfying progression mechanics

### 2. Teachers/Educators
- **Needs:** Tool supporting student wellbeing, age-appropriate content, productivity (not distraction)
- **Expectations:** Game that genuinely helps student stress without encouraging avoidance of study
- **Influence:** Required health-conscious design (no manipulation), appropriate difficulty balancing

### 3. Parents
- **Needs:** Confident their children engaging with quality entertainment during breaks
- **Expectations:** No ads, no purchases, no inappropriate content, time-limited engagement
- **Influence:** Drove decision for local-only, no monetization, easy to learn mechanics

**How Requirements Influenced Development:**
- **Student needs** prioritized responsive controls and satisfying feedback systems
- **Teacher feedback** emphasized fair difficulty and avoiding addiction mechanics
- **Parent concerns** eliminated all monetization and external tracking

---

## 1.4 Functional Requirements

| Requirement | Description | Priority | Rationale |
|-------------|-------------|----------|-----------|
| Quick Start | Game launchable and playable within 30 seconds | High | Students need immediate engagement during break periods |
| Responsive Controls | Input registered within 1 frame (16ms at 60 FPS) | High | Skill-based gameplay requires tight control feel |
| Clear Feedback | All player actions produce immediate visual/audio response | High | Stress relief requires satisfying feedback loops |
| Skill Progression | Players noticeably improve with practice | High | Achievement and completion reduces stress |
| Fair Difficulty Curve | Progression feels challenging but achievable | High | Frustration defeats stress-relief purpose |
| Accessible Difficulty Options | Easy/Normal/Hard suitable for all skill levels | Medium | Inclusive design supports diverse student needs |
| No Interruptions | No ads, popups, or time-limited content | High | Interruptions increase stress; undermine purpose |
| Pause Functionality | Game can pause anytime without penalty | High | Allows sudden return to study needs |
| Visual Clarity | All UI readable; information quickly understood | High | Difficult to learn UI increases stress levels |

---

## 1.5 Non-Functional Requirements

| Requirement | Target | Justification |
|-------------|--------|---------------|
| **Performance** | 30+ FPS minimum on modest hardware | School computers have variable specs |
| **Load Time** | <5 seconds from launch to playable | Break time is limited; delays create frustration |
| **Memory Usage** | <300MB footprint | School computers have storage limits |
| **Responsiveness** | <50ms input latency | Skill-based gameplay requires tight response |
| **Accessibility** | WCAG AA standard (readable text, color contrast) | Inclusive design supports all students |
| **Reliability** | Zero crashes in 50+ hours playtime | Crashes create frustration/stress, not relief |
| **Usability** | No tutorial required; intuitive within 1 minute | Break periods don't allow for learning curve |
| **Code Quality** | Maintainable, well-documented architecture | Supports future iteration and student learning |
| **Security** | No external data collection; local-only storage | Addresses privacy concerns (students, parents, school) |
| **Offline Capability** | Fully playable without internet | School networks may be restricted |

---

## 1.6 Constraints

**Time Constraints:**
- School assessment timeline (3-4 months)
- Limited development hours per week due to school and other activites

**Technical Constraints:**
- Limited to free/open-source assets (no budget)
- Target modest school hardware (can't use advanced graphics)
- 2D only (3D would require more assets/time)

**Knowledge Constraints:**
- Beginner-level game development experience
- Limited advanced animation experience
- Learning while developing

**Resource Constraints:**
- No money
- Limited access to designs

**Design Constraints:**
- Must be engaging enough for stress relief (nontrivial design challenge)
- Must not be so engaging it encourages skipping study (addiction prevention)
- Must balance these opposing requirements carefully
- No narrative/story but must be satisfying anyway

---

## 1.7 Requirements Analysis and Prioritisation

**Prioritisation Framework:**

**MUST Have (Core to stress-relief purpose):**
- Responsive controls and immediate feedback
- Fair, balanced difficulty progression
- Quick game startup
- No interruptions/ads
- Engaging progression mechanics

**SHOULD Have (Important for experience):**
- Multiple difficulty levels
- Visually pleasing
- Pause functionality

**COULD Have (Nice-to-have):**
- Leaderboards
- Advanced graphics
- Complex animations
- Multiple game modes

**WON'T Have (Explicitly excluded for good reason):**
- Permanent progression (prevents addiction)
- Monetization
- Multiplayer (creates social pressure)
- Social features (removes stress-relief benefit)

**Trade-offs Made:**

1. **Single Enemy Type:**
   - Decision: One well-designed slime vs. multiple enemy types
   - Why: Predictable enemies feel fairer.

2. **Arcade Over RPG:**
   - Decision: Wave-based arcade vs. progression RPG
   - Why: Arcade provides complete satisfaction in short time

3. **Difficulty Balancing Time Over Feature Completion**
   - Decision: Allocate 20% of development time to playtesting and balance vs. rushing to add more features
   - Why: Unbalanced difficulty creates frustration. A stress-relief game must be fair or it increases stress instead of relieving it.

4. Performance Optimization Over Good Graphics
   - Decision: Target 30+ FPS on modest school hardware vs. advanced graphics on modern hardware
   - Stuttering and lag break immersion and create frustration during gameplay.


### Alignment with Problem Statement

The implemented features directly address student stress relief:

- **Quick Start** → Reduces friction for break-time use
- **Fair Difficulty** → Achievable challenge promotes flow state (low-stress engagement
- **No Ads/Interruptions** → Clean experience eliminates added stress
- **Skill Progression** → Sense of competence and achievement improves mood
- **Accessible Difficulty** → Inclusive design reduces anxiety for all players

---

# 2. RESEARCH AND PLANNING

## 2.1 Development Methodology

**Methodology Selected: Agile with Two-Week Sprint Cycles**

**Description:**
Development followed Agile methodology with structured two-week sprints. Each sprint targeted one major system creation with testing.

**Justification for Selection:**

| Consideration | Why Agile |
|---------------|-----------|
| **Timeline** | Fixed release window; need rapid iteration |
| **Updates based on testing** | Feedback is essential for tuning and updates|
| **Team Size** | Small team of one whole person benefits from flexibility |
| **Analytics Integration** | Early feedback allows for corrections and tuning |

**Agile Implementation:**
- **Sprint Duration:** 2 weeks with clear deliverables
- **Sprint Goals:** One major system per sprint (movement, combat, progression, etc.)
- **Continuous Testing:** Testing throughout sprint, not just at the end
- **Analytics Review:** Player feedback considered end of each sprint

**Sprints Completed:**

| Sprint | Focus | Outcome |
|--------|-------|---------|
| 1 | Player movement & animations | WASD movement working with blend tree |
| 2 | Combat & enemy AI | Attack system and SlimeEnemy implemented |
| 3 | Health & UI displays | Health bar and currency UI functional |
| 4 | Wave system & progression | Currency and upgrade mechanics working |
| 5 | Menus & settings | Pause, upgrade, and settings menus complete |
| 6 | Audio & polish | SFX integrated, difficulty system added |
| 7 | Testing & debugging | Fixes applied, optimization performed |

---

## 2.2 Tools and Technologies

### Software Tools

| Tool | Purpose | Justification |
|------|---------|---------------|
| **Unity 6** | Game Engine | Industry standard, free, cross-platform |
| **Visual Studio Code** | Code Editor | Lightweight, fast, excellent C# support, free |
| **GitHub** | Version Control | Track changes and allows version rollbacks |

### Frameworks & Libraries

| Tool | Purpose | Justification |
|------|---------|---------------|
| **C#** | Programming Language | Unity's native language, modern OOP features |
| **UI Toolkit** | UI System | Modern UI, more efficient and easier to use than Canvas|
| **Rigidbody2D** | Physics Engine | Built-in 2D physics, handles collisions |
| **Animator** | Animation System | Blend trees, state machines |

### Asset Sources

| Asset Type | Source | Justification |
|-----------|--------|---------------|
| **Tileset** | Mystic Woods (itch.io) | Free, high-quality, 16x16 pixel art, perfect for top-down |
| **Sound Effects** | Myself | Free |

### Development Environment

- **OS:** Windows/Mac/Linux (tested on Windows)
- **Hardware:** Home computer (32GB RAM, modern CPU)
- **Resolution:** 1920x1080

### How Tools Supported Development

**Unity:**
- Built-in physics engine (2D) for collision and dynamics without extra programming
- Animator state machines eliminated need for custom animation control systems
- Scene managements
- Asset import

**C#:**
- Default programming language for Unity

**Git/GitHub:**
- Version control 
- Commit history
- Pull request allows for productivity across multiple devices

**UI Toolkit:**
- Better performance compared to old Canvas
- Intuitive to use and create

---

## 2.3 Gantt Chart / Timeline

![Gantt Chart](Photos\GanttChart.png)

## 2.4 Communication Plan

**Feedback Collection Methods:**

### Playtesting
- **Participants:** 4-6 playtesters from class
- **Duration:** 15 - 20 minute with feedback collected
- **Method:** Written PMI table as well as direct feedback for additional ideas and balancing
- **Incorporation** Client and Peer feedback was evaluated and good ones were implemented.

### Feedback Integration Examples

**Example 1: Difficulty Scaling Optimization**
- *Feedback* Many players said slimes on hard difficulty were impossible to beat past wave 3-4 as they did not have enough cash
- *Analysis:* Difficulty curve too steep in early waves; insufficient upgrade access
- *Response:* Reduced early-wave enemy count and stats on Hard mode while maintaining late-game difficulty
- *Result:* Hard mode progression improved and players were able to reach further rounds while still maintaining its challenging aspect.

**Example 2: Upgrade Balance Optimization**
- *Observation & Feedback:* Some upgrades did little to nothing
- *Analysis:* Defense provides 5% damage reduction per level, very little compared to Damage +7 per level
- *Response:* Made defense to 8% per level
- *Result:* During observations when playtesting, I saw that more people were going for defense in later rounds compared to beforex

---

## 2.5 Resource Allocation Justification

### Development Budget & Time Allocation

**Total Development Time: 158 hours**
- **Budget:** $0 (entirely free tools and assets)
- **Duration:** 14 weeks active development + 2 weeks holiday = 14 weeks
- **Team Size:** 1 solo developer (student)

#### Time Allocation

| Phase | Weeks | Justification |
|-------|--------|---------------|
| Documentation| 5 | Planning and brainstorming for game idea and implimentation |
| Development | 9 | Primary development phase |
| Testing | 1 | Playtesting is essential for stress-relief game. It makes sure everything is balances and users do not experience a lot of bugs|

### Software & Tools (Zero Cost)

**Development Tools:**

| Tool | Purpose | Cost | Justification |
|------|---------|------|---------------|
| **Unity** | Game engine | $0 | Unity includes a wide variety of premade game development tools. It is also very beginner friendly with many available guides. |
| **Visual Studio Code** | Code editor | $0 | Lightweight, fast, excellent C# support |
| **GitHub** | Version control | $0 | Essential for tracking progress |

### Hardware Resources

No extra hardware other than my home PC was used in the development of this project.

### Human Input

Testing and feedback from clients, peers and teachers were all taken in and analysed and evaluated to help with production of the game.

# 3. SYSTEM DESIGN

## 3.1 Context Diagram (Level 0)

![DataFlowDiagram-Leve0](Photos\DFDL0.png)

## 3.2 Data Flow Diagrams

### Level 1 - Detailed DFD

![DataFlowDiagram-Level](Photos\DFDL1.png)

## 3.3 Structure Chart

![StructureChart](Photos\StructureChart.png)

## 3.4 IPO (Input-Process-Output) Chart

| Module | Input | Process | Output |
|--------|-------|---------|--------|
| **Player Movement** | WASD keyboard input | Translate to velocity vector, apply to Rigidbody | Player position update, animation blend values |
| **Player Attack** | Left mouse click | Check cooldown, get enemies in range, calculate damage | Damage to enemies, animation trigger, SFX play |
| **Enemy AI** | Player position, enemy state | Calculate distance, determine behavior (walk/attack) | Enemy movement velocity, animation state |
| **Wave Manager** | Wave number, difficulty | Calculate enemy count, spawn with delay | Enemies spawned, wave state updated |
| **Combat Calculator** | Attacker damage, defender defense | Apply defense reduction to damage, update health | Adjusted damage value, death triggers if needed |
| **Currency Manager** | Gold amount, transaction | Add/subtract from total | Updated gold value|
| **Player Upgrade** | Upgrade type, current stats | Apply multiplier for upgrade level, recalculate | Updated stats applied to player systems |
| **Health Display** | Current/max health, health percentage | Format health as fraction, color based on percentage | Health bar visual, text label (e.g., "75/100") |
| **Death Sequence** | Player health ≤ 0 | Disable player control, show death screen | Death screen displayed, replay/menu options |

---

## 3.5 Data Dictionary

| Data Element | Type | Size | Description | Constraints | Example |
|------------|------|------|-------------|-------------|---------|
| **playerHealth** | Float | 4 bytes | Current player health points | 0 ≤ value ≤ maxHealth | 75.5 |
| **playerMaxHealth** | Float | 4 bytes | Maximum health player can have | 100 or higher | 120 |
| **currentGold** | Integer | 4 bytes | Amount of currency player has | ≥ 0 | 1250 |
| **playerPosition** | Vector3 | 12 bytes | Player world position (X, Y, Z) | Z must be 0 | (5.2, -3.1, 0) |
| **moveDirection** | Vector2 | 8 bytes | Normalized direction of movement | -1 to 1 for each axis | (0.707, 0.707) |
| **attackDamage** | Float | 4 bytes | Base damage dealt per attack | 25 or higher | 35.0 |
| **currentWave** | Integer | 4 bytes | Current wave number | ≥ 1 | 5 |
| **enemyCount** | Integer | 4 bytes | Enemies spawned in current wave | ≥ 0 | 15 |
| **attackCooldown** | Float | 4 bytes | Seconds between attacks | 0.1 or higher | 0.45 |
| **difficultyLevel** | Enum | 1 byte | Difficulty setting | Easy/Normal/Hard | Normal |
| **damageUpgradeLevel** | Integer | 4 bytes | Number of damage upgrades purchased | ≥ 0 | 3 |
| **isPaused** | Boolean | 1 byte | Whether game is paused | True/False | True |
| **isAttacking** | Boolean | 1 byte | Whether player mid-attack | True/False | False |
| **spawnRate** | Float | 4 bytes | Enemies per second during spawn | 0 or higher | 2.5 |

## 3.6 UML Class Diagram

The following UML class diagram shows the main C# scripts used in the Unity project. Unity engine types such as `Rigidbody2D`, `Animator`, `UIDocument`, `Button`, and `VisualElement` are treated as external framework classes, so the diagram focuses on the project-specific classes and their relationships.

![UML Class Diagram](Photos\UMLClassDiagram.png)


# 4. PRODUCING AND IMPLEMENTING

## 4.1 Development Process

**Development Approach: Agile with Incremental Feature Addition**

### 1. Design
**Implementation:**
- Separated function into independent scripts (SlimeEnemy, PlayerController, WaveManager, etc)
- Each manager handles one responsibility (CurrencyManager handles currency only)
- Systems communicate via public methods and events, not direct references

**Benefit:**
- Easy to test individual components
- Changes to one system don't break others

**Example:**
```csharp
// Player doesn't directly modify currency
// Instead, it calls the public method
if (CurrencyManager.Instance != null)
{
    CurrencyManager.Instance.AddCurrency(amount);
}
```

### 2. Object-Oriented Principles

**Encapsulation:**
- Private fields use public properties
- The state is protected from modification
- Example: Player health is private, only modified through TakeDamage()

**Polymorphism:**
- Different upgrade types use same method pattern
- UpgradeDamage(), UpgradeAttackSpeed() follow same structure
- Easy to add new upgrade types

### 3. Code Reuse

**Single implementation:**
- One a manager is made, it can be used everywhere
- No code duplication
- Ensures consistency across systems

**Coroutines:**
- WaveManager uses coroutines for timed spawning
- Cleaner than Update() loops with timers

### 4. Validation and Error Handling

**Null Checks:**
```csharp
if (playerStats != null)
{
    playerStats.RecalculateStats();
}
```
- This checks whether player stats exists before calling (recalcualte stats)
- This helps prevent errors from calling something that does not exist.

**Boundary Checking:**
```csharp
float healthPercent = Mathf.Clamp01(currentHealth / maxHealth);
```
- Using Clamp01, it makes sure the values are within 0 and 1
- This ensures that values such as -0.3 gets turned into 0
- In this context, this prevents the health bar from going negative, glitching it out.

**Input Validation:**
```csharp
if (Time.time < lastAttackTime + attackCooldown)
    return; // Prevent spam
```
- Validates that enough time has passed between last attack
- This is for attack cooldown.
- Prevents constant spamming of attack

---

## 4.2 Key Features Developed

### Feature 1: Player Movement & Animation
**Description:** WASD controls move player in 4 directions with smooth directional animations.

**Implementation:**
- Rigidbody2D
- Velocity-based movement
- Blend tree animators with MoveX/MoveY parameters for smooth direction blending
- Sprite flipping for left/right directions

**Why Included:** Core gameplay requirement. Game unplayable without movement

### Feature 2: Combat System
**Description:** Left-click to attack, deals damage to enemies in radius. 

**Implementation:**
- Physics2D.OverlapCircleAll() detects enemies in range
- Damage calculation includes defense reduction
- Animator triggers for attack animations

**Why Included:** Core gameplay.

### Feature 3: Enemy AI
**Description:** Slimes walk toward player when detected. Jump-attack when in range. Takes damage and die.

**Technical Implementation:**
- Distance-based behavior: walk if detected, attack if in range
- Jump attack jumps toward player
- Health tracking with death triggers
- Damage drops (gold) on death

**Why Included:** Defines wave-based gameplay.

### Feature 4: Currency & Upgrades
**Description:** Kill enemies for gold. Spend gold on 6 different stat upgrades. Costs scale per level.

**Technical Implementation:**
- CurrencyManager tracks gold
- PlayerStats tracks 6 upgrade levels
- Cost increases
- Upgrades immediately apply to PlayerAttack/PlayerHealth
- Events trigger UI updates

**Why Included:** Provides progression. Gives players goals. Makes players be strategic about spending

### Feature 5: Wave System
**Description:** Enemies spawn in waves. Wave difficulty increases exponentially. Endless waves until death.

**Technical Implementation:**
- Enemy count: `5 * 1.5^(wave-1)`
- Coroutine spawns enemies with delay
- WaveManager detects when wave complete (no enemies alive)
- Difficulty scaling applied to enemy stats

**Why Included:** Creates increasing challenge. Defines "a win" (wave survived = score)

### Feature 6: Difficulty Settings
**Description:** Easy/Normal/Hard modes scale enemy health, damage, and spawn rate.

**Technical Implementation:**
- DifficultyManager stores multipliers
- Applied to enemies on spawn
- Reflected in WaveManager spawn rate

**Why Included:** Replayability, accessibility (easier for new players) and hallenge (harder for experienced)

### Feature 7: Menus & UI
**Description:** Pause menu, upgrade menu, settings menu, death screen with proper state management.

**Technical Implementation:**
- UI Toolkit (UXML/USS) for all menus
- Controllers manage state and visibility
- Time.timeScale = 0 for pause
- Events trigger UI updates reactively

**Why Included:** Player control. Core game functionality

## 4.2.1 Back-End Engineering Contribution

**Data Processing:**

1. **Combat Calculation**
   - Processes: Attacker damage, defender defense
   - Applies: Defense reduction (`damage * (1 - defense%)`)
   - Outputs: Final damage value, health update

2. **Wave Generation**
   - Calculates: Enemy count per wave
   - Applies: Difficulty multipliers
   - Generates: Spawn positions, timing intervals

3. **Upgrade System**
   - Calculates: New stat values based on upgrade level
   - Applies: Changes across all player systems
   - Validates: Sufficient currency before applying

**Validation and Logic:**

1. **Input Validation**
   - Attack cooldown checks prevent spam
   - Currency checks prevent spending more than available

2. **State Validation**
   - Enemy death properly removes from scene
   - Wave completion properly detected
   - Pause state properly managed

**Storage and Retrieval:**

1. **Values stored**
   - Player stats stored in PlayerController
   - Currency stored in CurrencyManager
   - Game state stored across managers

2. **Data Retrieval**
   - Public getter methods on managers
   - Event-based notifications for UI updates

**Performance Optimizations:**

2. **Collision Detection**
   - Uses Physics2D.OverlapCircle (efficient)
   - Only checks when attacking (not every frame otherwise very laggy)

3. **UI Updates**
   - Reactive updates only when data changes
   - Events prevent unnecessary UI redraws

---

## 4.3 Screenshots of Interface

### Screenshot 1: Main Menu

![MainMenu](Photos\MainMenu.png)

### Screenshot 2: Gameplay Screen

![GamePlay](Photos\Gameplay.png)

### Screenshot 3: Upgrade Menu

![GamePlay](Photos\Upgrade.png)

### Screenshot 4: Pause Menu

![GamePlay](Photos\Pause.png)

### Screenshot 5: Settings Menu

![GamePlay](Photos\Settings.png)

### Screenshot 6: Death Screen

![GamePlay](Photos\Death.png)

# 5. TESTING AND EVALUATION

## 5.1 Testing Methods Used

### 1. Unit Testing
**What:** Individual component testing in isolation

**Components Tested:**
- PlayerAttack: Damage calculation, cooldown
- CurrencyManager: Add/spend currency
- PlayerStats: Upgrade calculations
- WaveManager: Wave spawning logic

**Method:**
- Manual testing in Unity editor
- Console logs for verification

**Example Test:**
```
Test: Spend Currency
Input: CurrencyManager has 100 gold, SpendCurrency(30)
Expected: Remaining gold = 70
Actual: Remaining gold = 70
Result: PASS
```

### 2. Integration Testing
**What:** Multiple systems working together

**Scenarios Tested:**
- Enemy death triggers currency drop
- Currency spend triggers upgrade apply
- Upgrade apply triggers stats recalculation
- Stats change affects combat damage

**Method:**
- Play test
- Verify output from multiple systems
- Check Console logs

### 3. User Testing
**What:** Peers play game and provide feedback

**Participants:** 3-5 student peers

**Testing:**
- End of each major sprint
- 15-20 minute play session
- Feedback with questions

**How Feedback Influenced Development:**

1. **Feedback:** "Hard difficulty is too easy"
   - **Response:** Increased enemy health multiplier from 1.4x to 1.5x
   - **Impact:** Better difficulty curve

2. **Feedback:** "Can't find settings"
   - **Response:** Added Settings button to pause menu
   - **Impact:** Improved menu navigation

3. **Feedback:** "Upgrades don't feel like they matter"
   - **Response:** Increased effect per upgrade (damage +5 → +7, health +20 → +25)
   - **Impact:** More noticeable progression

---

## 5.2 Test Cases and Results

| Test ID | Category | Test Name | Input | Expected | Actual | Status |
|---------|----------|-----------|-------|----------|--------|--------|
| **TC01** | Movement | Move Up | Press W | Player moves up, Walk animation plays | Player moves up, Walk animation plays |   PASS |
| **TC02** | Movement | Move Left | Press A | Player moves left, sprite flips left | Player moves left, sprite flips left |   PASS |
| **TC03** | Combat | Attack with cooldown | Click, click, click (rapid) | Second attack delayed by cooldown | Second attack properly delayed |   PASS |
| **TC04** | Combat | Attack damage | Click on enemy at 50 HP, attack (25 dmg) | Enemy health = 25 HP | Enemy health = 25 HP |   PASS |
| **TC05** | Combat | Attack with defense | Enemy defense=10%, take 10 dmg | Actual damage = 9 HP | Actual damage = 9 HP |   PASS |
| **TC06** | Enemies | Enemy spawn | Wave 1 starts | 5 enemies spawn | 5 enemies spawned |   PASS |
| **TC07** | Enemies | Enemy walk | Enemy detects player | Enemy walks toward player | Enemy walks toward player |   PASS |
| **TC08** | Enemies | Enemy attack | Enemy in range | Enemy jumps toward player | Enemy jumps toward player |   PASS |
| **TC09** | Enemies | Enemy death | Enemy health = 0 | Enemy dies, gold drops | Enemy dies, gold drops (5) |   PASS |
| **TC10** | Currency | Earn gold | Enemy dies (worth 5 gold) | Gold increases by 5 | Gold increased by 5 |   PASS |
| **TC11** | Currency | Spend gold | Spend 10 gold to heal | Gold decreases by 10, health +25 | Gold -10, health +25 |   PASS |
| **TC12** | Currency | Insufficient funds | Try to spend 100 with only 50 | Transaction fails | Transaction fails, gold unchanged |   PASS |
| **TC13** | Upgrades | Upgrade damage | Buy damage upgrade (cost: 20) | Damage increases by 5 | Damage +5, cost now 30 |   PASS |
| **TC14** | Upgrades | Cost scaling | Buy upgrade 5 times | Cost increases each time | Costs: 20→30→45→67→101 |   PASS |
| **TC15** | Upgrades | Apply immediately | Buy speed upgrade | Speed increases immediately in game | Speed noticeably faster |   PASS |
| **TC16** | Difficulty | Easy difficulty | Set Easy, spawn enemies | Enemy health 70%, damage 60% | Enemies noticeably weaker |   PASS |
| **TC17** | Difficulty | Hard difficulty | Set Hard, spawn enemies | Enemy health 150%, damage 140% | Enemies much stronger |   PASS |
| **TC18** | UI | Health bar color | Health >50% | Bar is green | Bar is green |   PASS |
| **TC19** | UI | Health bar color | Health 25-50% | Bar is yellow | Bar is yellow |   PASS |
| **TC20** | UI | Health bar color | Health <25% | Bar is red | Bar is red |   PASS |
| **TC21** | Menu | Pause menu | Press ESC | Menu opens, game paused (Time.timeScale=0) | Menu opens, game frozen |   PASS |
| **TC22** | Menu | Resume game | Click Resume | Menu closes, game resumes | Menu closes, game resumes |   PASS |
| **TC23** | Menu | Upgrade menu | Press Tab | Menu opens, pause triggered | Menu opens, game paused |   PASS |
| **TC24** | Menu | Settings | Click Settings from Pause | Settings menu opens | Settings menu opened |   PASS |
| **TC25** | Audio | Master volume | Slider to 0.5 | Audio volume = 0.5 * individual volumes | Audio volume reduced correctly |   PASS |
| **TC26** | Audio | SFX | Click to attack | Sword swing sound plays | Sword swing sound played |   PASS |
| **TC27** | Audio | Volume change | Adjust SFX slider while attacking | Immediate volume change | Volume changed in real-time |   PASS |
| **TC28** | Death | Player death | Reduce health to 0 | Death screen appears | Death screen appeared |   PASS |
| **TC29** | Death | Wave count | Die at wave 5 | Death screen shows "Wave: 5" | Display shows "Wave: 5" |   PASS |
| **TC30** | Death | Replay | Click Play Again | Game scene reloads | Game restarted from Wave 1 |   PASS |

## 5.3 Evaluation Against Requirements

### Functional Requirements Evaluation

| Requirement | Expected | Actual | Met? | Evidence |
|-------------|----------|--------|------|----------|
| Player Movement | WASD moves in 4 directions | Works smoothly |   YES | TC01-02, playtesting |
| Combat System | Left-click attacks radius | Works, cooldown prevents spam |   YES | TC03-05, gameplay footage |
| Enemy AI | Slimes walk & jump attack | Behavior works correctly |   YES | TC07-08, observation |
| Wave System | Enemies spawn in waves | Spawning works, difficulty scales |   YES | TC06-07, multiple playthroughs |
| Currency System | Gold earned/spent | Mechanics fully functional |   YES | TC10-12, testing |
| Upgrade System | 6 upgrades, scaling costs | All 6 work, costs scale correctly |   YES | TC13-15, all upgrades tested |
| Healing | E to heal for currency | Works correctly |   YES | TC11, gameplay |
| Health Display | Bar + fraction text | Shows correctly, colors change |   YES | TC18-20, inspection |
| Currency Display | Gold counter | Accurate, updates in real-time |   YES | TC10, observation |
| Pause Menu | ESC opens menu | Opens, closes, resumes properly |   YES | TC21-22 |
| Upgrade Menu | Tab opens menu | Opens, closes, upgrades work |   YES | TC23, gameplay |
| Settings Menu | Volume/difficulty controls | All controls functional |   YES | TC24-27 |
| Death Screen | Shows on death | Displays correctly |   YES | TC28-30 |
| Main Menu | Play/Options buttons | Navigation works |   YES | Observation |
| Audio | SFX for attacks | Plays correctly |   YES | TC26-27 |
| Difficulty | Easy/Normal/Hard scaling | Properly affects enemies |   YES | TC16-17 |

### Non-Functional Requirements Evaluation

| Requirement | Target | Actual | Status |
|-------------|--------|--------|--------|
| **Performance** | 30+ FPS | 60 FPS average (modern hardware), 45+ FPS (modest) | MET |
| **Input Responsiveness** | <50ms | ~16ms (1 frame at 60 FPS) |  MET |
| **Usability** | 100% clarity | Controls intuitive, UI clear, no confusion in testing |   MET |
| **Accessibility** | Font 16+ | All text 18+ or larger |   MET |
| **Reliability** | 0 crashes | 0 crashes in 20+ hours testing |   MET |
| **Code Quality** | Modular, design patterns | Singleton pattern throughout, clear separation |   MET |
| **Maintainability** | Clear code | Well-commented, logical structure |   MET |
| **Scalability** | Modular architecture | Can easily add features without refactoring |   MET |
| **Audio Quality** | Clear, not distorted | SFX clear and appropriate |   MET |
| **Visual Clarity** | 60 FPS animations | Animations smooth, 60 FPS maintained |   MET |

---

## 5.4 Improvements and Future Development

### Current Limitations

**Technical Limitations:**

1. **No Save System**
   - *Issue:* Progress lost when quit.
   - *Impact:* Can't track high scores or session progress
   - *Future Solution:* Implement JSON save file or cloud save

2. **Single Enemy Type**
   - *Issue:* Only slimes
   - *Impact:* Gameplay becomes repetitive after many waves
   - *Future Solution:* Add ranged slimes, heavy slimes, boss encounters

3. **Single Arena Map**
   - *Issue:* Same environment every game
   - *Impact:* Limited visual variety
   - *Future Solution:* Multiple procedurally generated arenas

4. **No Particle Effects**
   - *Issue:* No visual feedback for attacks, level ups
   - *Impact:* Less satisfying combat feel
   - *Future Solution:* Add particle systems for hits, explosions

5. **Limited Audio**
   - *Issue:* No background music. Only SFX
   - *Impact:* Less atmospheric gameplay
   - *Future Solution:* Add dynamic background music

**Design Limitations:**

6. **No Boss Battles**
   - *Issue:* Waves scale linearly forever
   - *Impact:* No climactic challenge moments
   - *Future Solution:* Boss enemies every 10 waves

7. **Limited Feedback**
   - *Issue:* No screen shake, knockback, or hit feedback
   - *Impact:* Combat feels less impactful
   - *Future Solution:* Add screen shake on hit, enemy knockback

### Proposed Future Features

1. **Background Music**
   - Add looping menu and gameplay music
   - Dynamic music switching on difficulty change

2. **Particle Effects**
   - Attack impact particles
   - Level-up sparkles
   - Enemy death explosions

3. **Screen Shake**
   - On successful hit
   - Controllable via settings

4. **High Score Tracking**
   - Save best wave reached
   - Display on death screen

5. **Additional Enemy Types**
   - Ranged slime (shoots projectiles)
   - Heavy slime (more health, slower)
   - Flying slime (different movement)

6. **Power-ups**
   - Temporary damage boost
   - Health restore item
   - Invulnerability briefly

7. **Procedural Arena Generation**
   - Random obstacles placement
   - Variable map size
   - Increasing complexity with waves

8. **Boss Encounters**
   - Every 10 waves: Giant slime boss
   - Special attack patterns

9. **Multiplayer Co-op**
    - 2-4 player support
    - Shared currency

10. **Story Campaign**
    - Narrative progression
    - Multiple levels/arenas
    - Boss story arcs

# 6. FEEDBACK, SECURITY AND REFLECTION

## 6.1 Summary of QA & Stakeholder Feedback

### Feedback Collection Methodology

PMTANG

Throughout testing, several key issues were identified and resolved to improve gameplay, accessibility, and user experience. The highest-priority issue was difficulty balancing, where Hard mode had no one make it past round 5 due to early enemy damage scaling. By reducing early damage and increasing spawn complexity instead, Hard mode completion rates improved, allowing more players to experience the full game. Menu navigation was simplified by making the Settings option directly accessible from the pause menu. Finally, UI font sizes were increased to a minimum of 18pt with responsive scaling, ensuring all testers found the text readable and making the game more accessible for players.

## 6.2 Secure Software Design and Data Handling

The game was designed with a low-risk data model. It does not collect personal information, login details, payment information, online chat messages, or location data. Most runtime data, such as player health, gold, wave number, difficulty, position, enemy states, and upgrade levels, exists only while the game is running. This reduces privacy risk because there is no sensitive user profile or account database to protect. However, security was still considered because corrupted game state, invalid input, or unexpected errors could damage the player experience and make the software unreliable.

### Secure Coding Practices Applied

Secure coding was mainly applied through defensive programming. Important values are kept inside controlled scripts and updated through methods rather than being changed freely by unrelated systems. For example, player health, currency, upgrade costs, attack cooldowns, and difficulty settings are checked before being applied. This helps prevent invalid states such as negative gold, health exceeding the maximum value, upgrades being purchased without enough currency, or attacks being triggered faster than intended.

The project also uses Unity's built-in systems where possible instead of manually handling risky low-level operations. Input is processed through the game controls and UI buttons, assets are loaded from the project rather than from user-supplied file paths, and gameplay objects communicate through controlled manager scripts. This limits the number of places where unexpected or unsafe data can enter the game.

### Input Validation and Error Handling

Input validation was applied to both gameplay input and menu input. Movement and attack controls are limited by game rules such as cooldowns, health checks, enemy range checks, and current game state. This means a player cannot repeatedly trigger an attack every frame or perform actions while the game is paused or the player is dead. Menu actions are also checked before use, such as confirming that UI buttons and game objects exist before trying to access them.

Boundary validation is important in the game because many values change quickly during combat. Health is clamped so it cannot fall below zero or exceed the intended maximum. Volume values are limited to a safe range between 0 and 1. Difficulty selection uses known options instead of accepting arbitrary values. Currency spending checks that the player has enough gold before an upgrade is applied.

Error handling was approached by reducing the chance of errors before they happen. Null checks are used before accessing optional components, manager instances, UI elements, and physics objects. This is especially important in Unity because missing inspector references or destroyed objects can otherwise create runtime exceptions. Where operations could fail during object interaction, the game logs the problem and continues running where possible. This improves reliability because a single failed reference should not crash the whole game session.

### Data Storage and Protection Methods

The current version stores gameplay data in memory only. This includes temporary values such as the player's current health, gold, selected difficulty, wave progress, enemies currently alive, and upgrade levels. Since this data is not written to a save file or sent to a server, the risk of exposing private user data is very low. The game also does not require an account system, so it avoids storing usernames, passwords, email addresses, or authentication tokens.

This approach is suitable for the current scope because the game is a local single-player prototype focused on short play sessions. It also means there is no database to secure, no cloud storage permissions to manage, and no network transmission of personal data. The main limitation is that progress is not persistent. If a save system is added in the future, it should store only necessary data, validate loaded save values before applying them, and protect the save file against corruption or simple tampering. For example, a future save system could use structured JSON with versioning, range checks on loaded values, and a checksum or encrypted save file if cheating or data modification becomes a concern.

Asset handling is also controlled. The game uses project assets rather than allowing players to load custom scripts, images, or external files. This lowers the risk of unsafe file access and prevents user-generated content from introducing malicious or incompatible data.

### Impact on User Trust, Data Integrity, and Reliability

Secure software design improves user trust because players can use the game without being asked for unnecessary personal data. This is especially appropriate for a school or student-focused project because privacy expectations are clear: the game should entertain the player, not collect information about them. Players are more likely to trust software that is transparent, local-only, and stable.

Data integrity is protected by keeping game values within valid ranges. If health, gold, damage, upgrade levels, or difficulty settings become invalid, the game balance would break and test results would become unreliable. Validation makes the gameplay rules consistent, which means the player receives fair outcomes and the developer can trust feedback gathered during testing.

System reliability is improved because defensive checks reduce crashes caused by missing references, invalid states, or unexpected player actions. This makes the game feel more polished and supports the main project goal of a quick, low-stress survival experience. A game designed for stress relief would lose user confidence quickly if it crashed, froze, or behaved unpredictably. Secure design therefore supports not only privacy and protection, but also the core user experience.

## 6.3 Personal Reflection

This project helped me understand that software engineering is not only about making code run. It is about planning a solution, testing it with real users, responding to feedback, and improving the design until the final product solves the original problem. Building a survival game in Unity required me to combine programming, user interface design, testing, debugging, documentation, and project management. By the end of the project, I had a much stronger understanding of how these areas connect.

### Software Engineering Skills Developed

**Planning and requirements analysis**

I learned how important it is to define the problem before building features. At the start, I had many possible ideas for the game, but the project became clearer once I connected each feature back to the goal of creating a short, engaging stress-relief game for students. Writing functional and non-functional requirements helped me decide what mattered most, such as responsive controls, clear feedback, fair difficulty, and local-only data handling.

**System design and modular programming**

I developed stronger skills in breaking a game into smaller systems. Instead of putting all behaviour into one script, I separated responsibilities into systems such as player movement, combat, enemy behaviour, currency, upgrades, waves, menus, audio, and difficulty. This made the project easier to understand and debug. It also showed me why modular design is important in software engineering: when each script has a clear purpose, changes are less likely to break unrelated parts of the game.

**C# and Unity development**

I improved my practical C# skills by using variables, methods, classes, conditionals, loops, events, Unity components, physics, UI elements, and animation controls. I also became more confident using Unity-specific tools such as GameObjects, prefabs, colliders, Rigidbody2D, the Animator, scenes, and the Inspector. One of the biggest improvements was learning how code and Unity editor settings work together. A script can be correct, but the game can still fail if a reference, collider, tag, or prefab setting is missing.

**Debugging and problem solving**

This project improved my ability to locate and fix errors systematically. Instead of guessing, I learned to reproduce the issue, check the relevant script, use debug logs, test one change at a time, and confirm whether the fix worked. This was especially useful when dealing with gameplay bugs because the cause was not always obvious. For example, a movement issue could be caused by code, physics settings, animation states, or object configuration.

### Challenges Encountered and How They Were Overcome

**Challenge 1: Balancing difficulty**

One major challenge was making the game challenging without becoming frustrating. Early testing showed that Hard mode became too difficult too quickly, with players struggling to survive past the early waves. This was a problem because the game was meant to provide stress relief, not create unnecessary frustration.

To overcome this, I adjusted the difficulty balance using tester feedback. Instead of only increasing enemy damage, I considered multiple factors such as spawn rate, enemy health, player upgrades, and early-wave pacing. This made Hard mode still feel more intense, but less unfair. The main lesson was that difficulty should be tested with real players because what feels fair to the developer may feel very different to someone playing for the first time.

**Challenge 2: Managing multiple game systems**

As more features were added, the game became harder to manage. Player stats, enemy spawning, upgrades, currency, pause menus, and UI updates all needed to work together. At times, fixing one system affected another system unexpectedly.

I overcame this by keeping systems more modular and making sure each script had a clear responsibility. For example, currency handling was separated from player movement, and wave spawning was separated from upgrade logic. This reduced confusion and made debugging easier. It also taught me that good structure matters more as a project grows.

**Challenge 3: UI and menu usability**

Another challenge was making the interface easy to use. During feedback, players found some menu options harder to access than expected, especially settings. This showed that a working interface is not always a good interface.

I improved this by simplifying navigation and making important options easier to reach from the pause menu. I also increased readability through larger text and clearer layout choices. This helped me understand that user experience is part of software quality, not just a visual extra.

**Challenge 4: Runtime errors and missing references**

Unity projects can produce errors when objects, components, or UI elements are not assigned correctly. Some issues were caused by scripts trying to access objects that were missing, disabled, or destroyed. These errors could interrupt gameplay or make features stop working.

I overcame this by adding null checks, checking Inspector references, and testing scenes after changes. I also learned to read error messages more carefully instead of only looking at the line number. This improved the reliability of the game and helped me develop safer coding habits.

# 7. APPENDICES

## Appendix A: Full Gantt Chart

## Appendix B: Complete Data Dictionary

## Appendix C: Full Test Logs

## Appendix D: Raw Feedback Notes

## Appendix E: Code Snippets

### Example 1: Event System
```csharp
public event Action<int> OnCurrencyChanged;

public void AddCurrency(int amount)
{
    currentCurrency += amount;
    OnCurrencyChanged?.Invoke(currentCurrency);
}
```

### Example 2: Upgrade Implementation
```csharp
public bool UpgradeDamage()
{
    if (CurrencyManager.Instance.SpendCurrency(damageCost))
    {
        damageLevel++;
        damageCost = Mathf.RoundToInt(damageCost * 1.5f);
        RecalculateStats();
        return true;
    }
    return false;
}
```

### Example 3: Combat System
```csharp
void Attack()
{
    if (Time.time < lastAttackTime + attackCooldown)
        return;
    
    lastAttackTime = Time.time;
    animator?.SetTrigger("Attack");
    PlaySwordSwingSFX();
    
    Collider2D[] hits = Physics2D.OverlapCircleAll(
        transform.position, attackRange);
    
    foreach (Collider2D hit in hits)
    {
        SlimeEnemy slime = hit.GetComponent<SlimeEnemy>();
        if (slime != null)
            slime.TakeDamage(attackDamage);
    }
}
```
