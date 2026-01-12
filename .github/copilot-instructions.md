# GitHub Copilot Instructions for Experimental Gear Test Force (Continued)

## Mod Overview and Purpose
The "Experimental Gear Test Force (Continued)" is a continuation update of a mod by 小面包. This mod introduces a selection of advanced and experimental technologies into the RimWorld universe, providing players with a variety of new strategic options and scenarios. The mod focuses on specialized mechanoids and armaments to enhance gameplay, particularly in the early stages, and expands upon technological evolution within the game's lore.

## Key Features and Systems
- **Armored Mechanoids**: Includes basic work mechanoids with enhanced armor capabilities allowing them to throw grenades and deploy warurchins, providing an edge in early game scenarios.
- **Weaponry**: Adds an industrial era experimental weapon, serving as a precursor to electric weapons, doubling as an LGIS missile launcher.
- **Armor**: Introduces a bulletproof vest with the unique ability to throw random grenades and a compound cloak granting invisibility to the wearer.
- **DLC and Scenario**: If all RimWorld DLCs and the Vanilla Expanded Framework are installed, the mod unlocks a challenging scenario: two hungry refugees surviving in an abandoned county on a specially crafted map.

## Coding Patterns and Conventions
- Classes are organized in files by functionality, often using the `verb` and `comp` pattern to extend weapon capabilities and introduce new mechanics.
- Use of `DefModExtension` and `CompProperties` to add modular properties to game entities for enhanced configurability.
- Private methods follow the convention of functional prefixes (e.g., `Try`, `Load`, `Switch`) for clarity in operation.

## XML Integration
The mod heavily integrates XML for defining new entities, weapons, clothing, and scenarios. XML files should follow proper schema definitions used in RimWorld modding. It’s crucial to maintain XML structure integrity for seamless integration with the game's data system.

## Harmony Patching
Harmony patches are essential to modifying game functions. Ensure all Harmony patches are applied correctly by following a strict pattern for prefix, postfix, and transpile method integration. Maintain modular and reversible code patches to avoid conflicts with other mods.

## Suggestions for Copilot
- When generating new classes, focus on leveraging existing patterns such as `Verb` and `CompProperties` as starting templates.
- For Harmony patches, ensure Copilot suggestions maintain the mod's non-destructive operation principle by only extending or augmenting existing game functionality.
- Guidance for XML modifications can be assisted by Copilot, adhering to common RimWorld modding structures for item and recipe definitions.
- When creating scenarios or map-related functionality, ensure that Copilot suggestions incorporate modular and scalable code for ease of adjustment and testing. 

## Compatibility and Issues
- **Combat Extended (CE)**: Most features are compatible except certain bulletproof equipment skills.
- If errors occur, verify the mod's activities by running it with only its dependencies. Use tools like RimSort for mod sorting and the Log Uploader for error documentation.
- For unresolved issues or contributions, always refer to the mod’s Discord channel and GitHub repository for community support and updates.
