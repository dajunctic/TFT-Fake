# Unity AI Game Creator Skill & Rules

This rule guides the AI agent in transforming raw game ideas into complete Unity projects with AI-powered asset generation, clean architectural patterns, scene blueprints, and development roadmaps tailored for modern Unity development (including Unity 6+).

---

## 1. The Master Pipeline

Always execute and guide development through these 5 phases in order:

```
PHASE 1: IDEATION ──▶ PHASE 2: BLUEPRINT ──▶ PHASE 3: GENERATION ──▶ PHASE 4: ASSEMBLY ──▶ PHASE 5: DEPLOYMENT
  - Game Brief          - GDD & Architecture   - AI Asset Prompts      - ScriptableObjects     - Target Builds
  - Genre Blend         - Scene Blueprints     - 3D Models & Audio     - Core Systems/C#       - QA & Polish
  - Scope Assessment    - Folder Structures    - UI & Sprite Textures  - UI & Audio Layers     - Optimization
```

---

## 2. Phase-by-Phase Instructions

### Phase 1: Ideation & Deep Analysis
Extract the following dimensions from the user's game idea. Infer from context where possible, and only ask about highly ambiguous details:
*   **Genre Blend**: Primary and secondary genre combinations.
*   **Platform**: Recommend PC, Mobile, WebGL, or Console depending on complexity.
*   **Art Style**: Stylized, Low-poly, Realistic, Pixel Art, or Anime.
*   **Core Loop**: The 30-second repeating gameplay action.
*   **Audience & Monetization**: Target demographic and recommended revenue models.
*   **Scope Calibration**: 
    *   *Prototype*: 1-2 weeks (Solo, core loop only)
    *   *Vertical Slice*: 4-6 weeks (Solo-2 devs, 1 complete polished level)
    *   *MVP*: 8-12 weeks (2-4 devs, 3-5 levels, UI, saves)
    *   *Full Release*: 16-24+ weeks (3-8 devs, full features, multiplayer/live-ops)

### Phase 2: Blueprint & Clean Architecture
Provide clean, decoupled, and testable architectures for Unity:
*   **Folder Structure Blueprint**:
    ```
    Assets/_Project/
    ├── Scripts/ (Core, Gameplay, Entity, SkillSystem, UI, Data, Audio, Utilities)
    ├── Prefabs/ (Characters, Environment, UI, VFX)
    ├── Scenes/
    ├── Art/ (Models, Textures, Materials, Animations, UI_Assets)
    ├── Audio/ (Music, SFX, Ambience)
    ├── ScriptableObjects/
    └── Resources/
    ```
*   **Scene Blueprint Setup**: For each scene, specify:
    *   *Environment*: Ground, lighting, post-processing profiles.
    *   *Interactions & AI*: NPC behavior, triggers, event system connections.
    *   *HUD & UI*: Dynamic screen canvas, safe-area compliance.
    *   *Audio & VFX*: Ambient audio loops, background music stems, active particle systems.

### Phase 3: AI-Powered Asset Generation
Generate precise, production-ready prompts for generative AI tools (such as Meshy.ai, Tripo3D, Unity AI Generator, ElevenLabs, and Suno/Udio):

*   **3D Model Prompts**:
    *   *Format*: `[Subject], [Art Style], [Detail level], [Texture type], [Technical constraints (Low-poly/PBR/Animation-ready)]`
    *   *Example*: `"Stylized fantasy chest, hand-painted textures, RPG game asset, low-poly, 3D model, clean UV layout, game-ready"`
*   **Texture & 2D Sprite Prompts**:
    *   *Format*: `[Subject], [Art Style], [Colors], [Negative Prompts], [Tiling option]`
    *   *Example*: `"Tiled dungeon stone floor, hand-painted stylized texture, seamless tileable, warm grays and brown moss, game asset --no photorealistic, blurry"`
*   **Audio & Music Prompts**:
    *   *Format*: `[Mood], [Tempo], [Instruments], [Gameplay purpose], [Looping]`
    *   *Example*: `"Epic orchestral fantasy battle theme, fast tempo 140 BPM, heavy percussion, brass, strings, heroic loopable background music"`

### Phase 4: Assembly & Unity Systems
Ensure best practices during system assembly in Unity:
*   **Decoupled Systems**: Use ScriptableObjects for configuration, game events, and shared variables to avoid tight coupling.
*   **UI System**: Use the standard Unity UI (uGUI) or UI Toolkit with responsive anchoring and flexible layouts.
*   **Input System**: Prefer the new Unity Input System for cross-platform compatibility.
*   **Entity/Skill Architecture**: Build modular components (e.g., using interfaces like `IEntity`, state machines, or node-based logic like `xNode`).

### Phase 5: Deployment & Optimization
*   **Performance Budgets**: Monitor Draw Calls (Batches), SetPass Calls, and Vertex counts.
*   **Asset Compression**: Standardize texture sizes (e.g., 512x512 for UI icons, 2048x2048 for high-detail objects) and use ASTC/DXT compression.
*   **Build Optimization**: Set up IL2CPP scripting backend and enable strict build stripping.
