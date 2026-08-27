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

Custom 2D movement with collision handling.

The Player can move, jump, fall, wall slide, wall jump, and dash.

The Player can get stunned.

The combat system allows the Player and Enemy to attack each other, take damage, and get stunned.

Each attack can have different properties such as damage, knockback, and hitstun.

Hitting a valid target or taking damage can trigger hitstop.

An Enemy can detect a target, move toward the target, and attack it.

## Architecture


### 1. Overall Structure

The framework defines what an `Entity` is and separates it from what an `Entity` can do.

Instead of putting combat logic such as health, hitstun, and knockback directly inside `Player` and `Enemy`, I create small components and attach them to an Entity to add different capabilities.

This means an Entity can use the capabilities provided by the components it owns. Thanks to that, an object's own logic can stay separate from the logic of its reusable capabilities.


### 2. Movement and Combat

The goal of the framework is to let the Player move and attack at the same time.

Adding more states such as `MoveAttack`, `JumpAttack`, or `WallAttack` would make the state system harder to extend. So instead of doing that, I separate the combat system from movement and let both systems run in parallel.

So, how do these two systems work together?

They work together quite simply.

For the Player, the Movement FSM does not need to know how combat works. The current state only exposes whether attacking is allowed. The Player passes this information to the combat system and also decides when to request an attack from player input.

For the Enemy, its own target detection and combat conditions decide when to request an attack.

The combat system handles how the attack is executed.

An attack can start when:

- Attacking is allowed.
- The owner requests an attack.
- The attack system is ready for a new attack.

Because of this, movement and combat do not need to know each other's internal logic. The owner (`Player` or `Enemy`) is responsible for deciding when it wants to attack, while the combat system is responsible for executing that attack.


### 3. Hit Flow

The attack process works like this:

```text
Attack
  ->
AttackHitBox detects a target
  ->
AttackEffectProcessor sends the attack information to the target
  ->
HitReceiver checks whether the target accepts the hit
  ->
Is the attack accepted?
 - No -> Nothing happens
 - Yes
      -> Target applies:
         Damage
         Knockback
         Hitstun
         Hit Reaction
      -> Attacker applies its own hit response
```
`AttackHitBox` only detects a potential target. It does not decide what happens to that target.

After that, `AttackEffectProcessor` requests the target's `HitReceiver` to receive the attack. The `HitReceiver` decides whether the hit is accepted and applies effects such as damage, knockback, hitstun, and hit reaction.

If the hit is accepted, the attacker can then apply its own response, such as recoil or pogo.

Because of this, the attacker does not need to directly change or know the internal state of the target. If the target has a special condition that prevents it from receiving a hit, it can simply reject the hit.

## Design Philosophy

This project is mainly a learning project.

I do not want to build every possible system before the game actually needs it. Instead, I try to solve the current problem first, test the result, and only redesign the system when a real limitation appears.

The framework is meant to grow together with the game built on top of it.

## What I Learned

How to code in Unity/C#.

How to separate responsibilities between different gameplay systems.

How important it is to make a system reusable.

What information a system should expose, and which systems need to know about each other to work together.

There are many solutions to a problem, but choosing the one that fits my style and my system is more important.

There is no easy way around a problem by avoiding it.

## Next Steps

The framework phase is now intentionally paused.

The next goal is to use the current systems to build a real playable game.
