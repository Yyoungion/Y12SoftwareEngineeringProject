# **Software Project Report - A Survival Game**
##### *By Yyoung Du*

<div style="padding-bottom: 40px;"></div>


## **Part 1 - Identifying and Defining**
### 1.1 Problem Statement
---
#### **Outline the problem or opportunity that the project addresses. Consider who is affected by this issue.**
*The Problem:* \
Students and young people today experience high levels of stress and anxiety from academic pressures, social media, and daily challenges. Many lack outlets, taps, that provide  mental breaks and stress relief. While entertainment options exist, there's a need for accessible games that offers an escape and relaxation without requiring expensive hardware or significant time commitments.\

*Who is Affected:*
- Students (ages 13-25) dealing with exam stress, assignment pressure, and academic workload
- Young people experiencing anxiety and needing healthy coping mechanisms
- Casual gamers looking for engaging but not overwhelming gaming experiences during study breaks
- Individuals with limited budgets who cannot afford expensive games or high-end gaming systems

#### **Explain why this problem or opportunity is significant.** 
*Mental Health Impact:* 
- Student mental health has declined significantly, with studies showing increased rates of stress, anxiety, and burnout among young people
- Academic pressure is high, with students experiencing demanding workloads and competitive environments
- Poor mental health directly impacts academic performance, relationships, and wellbeing

*Consequences of Unmanaged Stress:*
- Chronic stress leads to decreased concentration, memory problems, and reduced learning capacity
- Students without healthy coping mechanisms may turn to harmful alternatives or simply burn out
- High stress levels can result in serious mental and physical health issues

*Lack of Breaks and Recreation:*
- Many students don't take adequate breaks, leading to diminishing returns on study time
- Without accessible recreational outlets, stress accumulates rather than being released
- The pressure to be constantly productive leaves little room for necessary downtime

*Widespread Impact:*
- This affects not just individual students but entire school communities and family systems
- Stressed students create stressful environments for peers and teachers

#### **Justify the development of a software solution as an appropriate response.** 
*Accessibility and Low Barrier to Entry:*
- A software game can be distributed digitally at minimal or no cost, making it accessible to students regardless of economic background
- Unlike physical recreation (sports equipment, gym memberships), software requires no additional purchases beyond a basic computer
- Students can access it from home, school, or anywhere they have a device

*Convenience and Flexibility:*
- Software is available on-demand, 24/7, whenever a student needs a mental break
- No need to coordinate with others, travel to locations, or wait for specific times
- Players can engage for 5 minutes or 50 minutes depending on their available time
- Fits naturally into study routines as a break between tasks

*Engaging and Interactive:*
- Unlike passive stress relief (watching videos), games require active engagement which is more effective as distractions from stress
- Interactive gameplay provides mental stimulation that helps reset focus and attention
- The challenge-reward creates genuine enjoyment and satisfaction
&nbsp; \
&nbsp; 
### 1.2 Project Purpose and Boundaries
---
#### **Outline what the project is trying to achieve**
1. *Create a Functional Survival Game*
    - Develop a playable top-down survival game using Unity and C# in Visual Studio Code
    - Implement core survival mechanics including resource management, health/hunger systems, and environmental challenges
    - Design and code enemy AI or hazards that create meaningful gameplay challenges

2. Deliver an Engaging Player Experience
    - Provide accessible, enjoyable entertainment that offers stress relief through engaging gameplay
    - Design easy to understand controls and mechanics that are easy to learn
    - Implement a basic storyline that gives context and purpose to the survival gameplay
&nbsp; \
&nbsp;
### 1.3 Stakeholder Requirements
---
#### **Identify stakeholders (client, users, teacher, peers).**
#### **Describe their needs, expectations, and how these influenced the project direction.**
1. End Users (Players - Students aged 13-25)
    - Needs
        - Easy-to-understand controls and mechanics
        - Engaging gameplay that provides stress relief and entertainment
        - A game that runs smoothly on basic/school computers
        - Clear objectives and feedback on progress
        - Appropriate challenge level - not too easy, not frustratingly difficult

    - Expectations
        - Intuitive user interface that doesn't require lengthy tutorials
        - Responsive controls and fair gameplay mechanics
        - Some replay value or reason to continue playing
        - A complete experience with a beginning, middle, and end
        - Minimal bugs that don't break the game experience

    - Influence on Project Direction:
        - Simple top-down perspective for accessibility
        - Basic graphics that prioritize performance over visual complexity
        - Clear UI showing health, hunger, and other vital information
        - Straightforward gameplay loop that's easy to pick up
        - Short play sessions that fit into study breaks (10-30 minutes)
&nbsp; \
&nbsp;
### 1.4 Functional Requirements
---
#### **List and describe what the system must do**
1. Player Movement and Control
    - The system must allow the player to move their character in four directions (up, down, left, right) or eight directions (including diagonals) using keyboard input (WASD or arrow keys)
    - The player character must respond immediately to input with smooth, predictable movement
    - Movement speed must be consistent and controllable
    - The system must prevent the player from moving through solid objects, walls, or boundaries

2. Health System
    - The system must track and display the player's current health value
    - Health must decrease when the player takes damage from enemies or the environment
    - The system must allow health to be restored through consumable items  (health packs, food, etc.) or natural regeneration
    - When health reaches zero, the player needs to die
    - Health values must be visible to the player through a UI element (health bar, numerical display, or hearts)

3. Hunger Meter System
    - The system must track a hunger meter that decreases over time
    - The hunger meter must be replenished by consuming food items
    - When hunger reaches critical levels, the system must apply penalties (health loss, reduced movement speed, etc.)
    - Hunger levels must be clearly displayed to the player via UI
    - The system must warn the player when hunger is critically low

4. Resource Collection
    - The system must allow players to collect resources from the environment (food, crafting materials, health items)
    - Resources must be added to the player's inventory when collected
    - The system must provide visual/audio feedback when resources are collected
    - Collectible resources must disappear from the game world after being picked up
    - The system must spawn or place resources at appropriate locations in the game world

5. Inventory Management
    - The system must have an inventory that stores collected items
    - The inventory must have a maximum capacity (limited slots or weight)
    - Players must be able to view their current inventory through a UI screen
    - The system must allow players to use/consume items from their inventory
    - Items must be removed from inventory when used or consumed

6. Crafting System (Optional but recommended)
    - The system must allow players to combine resources to create new items
    - A crafting interface must display available recipes based on current inventory
    - The system must consume the required materials when crafting succeeds
    - The crafted item must be added to the player's inventory
    - The system must prevent crafting if required materials are unavailable

7. Enemy
    - The system must spawn enemies or environmental hazards at designated locations
    - Enemies must have basic pathing (patrol, chase player, attack)

&nbsp; \
&nbsp;
### 1.5 Non-Functional Requirements
---
#### **List and describe system qualities**
1. Performance
    - Frame Rate: The game must maintain a consistent fps of at least 30 FPS
    - Load Times: The game must be able to load from the main menu within 10 seconds
    - Response Time: Player input must be displayed through the screen within 100 milliseconds
    - Memory Usage: The game must not exceed 2GB of RAM usage during normal gameplay
    - Optimization: The game must not experience frame drops or stuttering

2. Usability
    - Learning Curve: New players must be able to understand basic controls and mechanics
    - UI Readability: All text must be readable from a standard viewing distance
    - Navigation: Players must be able to access the pause menu, inventory, and return to main menu
    - Consistency: Controls and UI elements must remain consistent throughout the game (same button always does the same action)
    - Error Prevention: The system must prevent common user errors

3. Security
    - Data Integrity: Save files must be stored securely and be validated to help prevent corruption
    - No Exploits: The game must not allow players to exploit bugs to gain unfair advantages that break the game
    - Privacy: No personal data or identifying information is collected, stored, or transmitted (single-player, offline game)

4. Reliability
    - Stability: The game must run without crashes, freezing, or stuttering
    - Error Handling: The system must handle errors without crashing
    - Save Reliability: Save/load functionality must work 100% of the time
    - Consistent Behavior: Game mechanics must behave identically every time
    - Recovery: If the game encounters an error, it must either recover automatically or provide clear instructions to the player on how to resolve the issue
    - Reproducibility: Game behavior must be predictable and reproducible

