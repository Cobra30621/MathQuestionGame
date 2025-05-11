# 數學魔法使 Math Magician

A roguelite card game that combines math problem-solving with strategic card battles. Players navigate through procedurally generated maps, solve math questions, and use cards to defeat enemies.

## 📺 Promo Video

[![Watch the Trailer](https://img.youtube.com/vi/QCVSIcKTRp8/0.jpg)](https://youtu.be/QCVSIcKTRp8)

> 🎬 Click the thumbnail above to watch our official gameplay trailer!

## Game Overview

MathQuestionGame is a Unity-based game that combines educational math content with roguelite card game mechanics. Players:

1. Choose a character with unique starting decks and abilities
2. Navigate through procedurally generated maps with different node types
3. Solve math questions to gain advantages in combat
4. Collect and upgrade cards to build a powerful deck
5. Defeat enemies using strategic card play
6. Collect relics that provide passive bonuses
7. Progress through multiple stages with increasing difficulty

## Core Systems

### Game Management

The `GameManager` serves as the central hub for all game systems, providing access to:

- Save/Load functionality
- Item management (Relics, Cards, Currency)
- Map navigation
- Combat and effects
- UI and feedback systems
- Question management

### Math Question System

The `QuestionManager` handles the math question gameplay:

- Generates questions based on difficulty settings
- Manages the question UI flow
- Records player performance
- Provides rewards based on correct answers
- Supports online question downloading

### Card System

The card system is managed by the `CardManager` and includes:

- Player deck management
- Card upgrading and leveling
- Card information and display
- Card effects and interactions

### Combat System

The `CombatManager` handles battle mechanics:

- Turn-based combat flow (player and enemy turns)
- Mana management for card playing
- Character stats and abilities
- Card targeting and effect resolution
- Combat state management (start, player turn, enemy turn, end)
- Reward distribution after combat

### Map System

The `MapManager` controls game progression:

- Procedurally generated maps with different node types
- Multiple maps per stage with increasing difficulty
- Node selection and path tracking
- Special encounters (elite enemies, bosses, events)
- Stage progression

### Character System

Characters in the game include:

- Player characters (allies) with unique starting decks and abilities
- Enemy characters with different attack patterns and abilities
- Health management and status effects

### Reward System

The `RewardManager` handles rewards after completing nodes:

- Card rewards
- Currency rewards
- Relic rewards
- Health restoration

### Save System

The game features a comprehensive save system that stores:

- Current map state and progress
- Player deck and upgrades
- Collected relics
- Character health and stats
- Game settings

## Technical Architecture

The game is built with Unity and follows these architectural patterns:

- Singleton pattern for manager classes
- Scriptable Objects for data management
- Event-driven communication between systems
- Serialization for save/load functionality
- Component-based design for UI elements

## Game Flow

1. **Main Menu**: Players select a character and stage
2. **Map Navigation**: Players choose paths through the map
3. **Encounters**: Based on the node type, players face:
   - Combat encounters with enemies
   - Math question challenges
   - Events with choices
   - Campfires for healing/upgrading
   - Shops for purchasing items
4. **Combat**: Turn-based card battles against enemies
5. **Rewards**: After successful encounters, players receive rewards
6. **Progression**: Players advance through maps and stages until completing the game

## References
- [Documents](https://hackmd.io/@Cobra3279/HJK2qpy9h/%2F7ZU6DAlyRvS1JLM7dXL0dA)

  
