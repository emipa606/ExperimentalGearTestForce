# Experimental Gear Test Force (Continued) - Copilot Instructions

Welcome to the GitHub Copilot instructions for the RimWorld mod *Experimental Gear Test Force (Continued)*. This document aims to support developers in understanding and contributing to the mod's development by providing an overview of the mod, its features, coding patterns, and other technical details.

## Mod Overview and Purpose

**Experimental Gear Test Force** is a mod for RimWorld initially developed by 小面包 and updated by the community. The mod introduces advanced mechanical and weaponry systems to enhance early-stage gameplay and provide unique tactical advantages. It is particularly focused on armored mechanoids and experimental weapons, incorporating a narrative of historical technological advancements evolving into usable items for RimWorld storytelling.

Key components of the mod include armored mechanoids with special abilities, a selection of industrial-era weapons, and unique apparel with advanced functionalities.

## Key Features and Systems

1. **Armored Mechanoids**:
   - Work mechanoids equipped with strengthened armor.
   - Abilities to throw grenades and release warurchins, providing early-game tactical advantages.

2. **Weapons Under Test**:
   - Represent industrial-era weapons, precursors to electric weapons.
   - Function as missile launchers with unique properties.

3. **Innovative Armor**:
   - Bulletproof vests that throw random grenades.
   - Cloaks that grant invisibility to the user, enhancing stealth gameplay.

4. **DLC and Mod Integration**:
   - Scenarios unlocked through specific DLCs and Vanilla Expanded Framework for enhanced gameplay experiences.
   - Includes a specially designed map featuring a challenging scenario with narrative elements.

## Coding Patterns and Conventions

- **File Organization**: 
  The project is structured with separate directories for C# source files and XML definitions. Relevant files are grouped based on functionality (e.g., apparel, weapons).

- **Naming Conventions**:
  - C# Class names and members follow PascalCase (`CompProperties_SecondaryAmmo`, `WarmupComplete`).
  - XML Defs and file names are in PascalCase (`EX_TF_Apparel.xml`, `ammoDef`).

- **Coding Style**:
  - The project uses consistent indentation and brace styles.
  - Focus on clear method and property names to enhance code readability.

## XML Integration

The mod heavily utilizes XML files for defining items, apparel, and various gameplay elements. Key XML files include:

- `EX_TF_Apparel.xml`: Contains definitions for apparel such as bulletproof vests and cloaks.
- `Rocketpack.xml`: Defines properties for rocket-pack items and related effects.
- Patches: Consist of specific game mechanic modifications (`Human.xml`, `MuzzleFlash.xml`) to integrate new features smoothly.

## Harmony Patching

- Harmony is used for runtime code changes without altering the original binaries.
- The mod implements Harmony patches to enhance existing game functionalities and introduce new behaviors.

## Suggestions for Copilot

- When suggesting code completions, focus on matching the project's existing style and conventions.
- Prioritize intelligence that helps with XML file editing, especially in terms of Defs and attributes common in RimWorld modding.
- For Harmony patches, suggest common patterns of prefix/postfix methods and consider patching priority where necessary.
- Assist in writing C# logic that handles multiple game states and integrates seamlessly with existing RimWorld classes and methods.

Please contribute responsibly by ensuring compatibility and consistency within the mod when utilizing GitHub Copilot suggestions. Adhere to the established patterns and provide thorough testing for any new functionality.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

