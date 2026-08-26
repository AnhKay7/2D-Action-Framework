## About

This project started as a small project to learn Unity and practice making a 2D combat game. It was inspired by Hollow Knight.

During development, especially while building the player's movement system, I realized that many of the systems could be reused. Therefore, the project gradually became a small framework for 2D action games, focusing on movement and combat systems.

The current goal is to use this framework to build a real game and extend it only when the game needs it.

## Current Status

**Current milestone: Movement + FSM + Combat Foundation Complete**

The Movement/FSM systems are now stable and can serve as the foundation for gameplay.

The basic combat foundation is now complete. The Player and the Enemy have a basic combat loop including attacks, damage, knockback, and hitstun.

The next phase is to use this framework to build a real game, starting with animation and gameplay integration.

## Features

- Custom 2D movement with collision handling.
- The Player can move, jump, fall, wall slide, wall jump, and dash.
- The Player can get stunned.
- The combat system allows the Player and Enemy to attack each other, take damage, and get stunned.
- Each attacks can have different properties such as damage, knockback, and hitstun.
- Hitting a valid target or taking damage can trigger hitstop.
- An Enemy can detect a target, move toward the target, and attack it.
