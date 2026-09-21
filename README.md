# 🥤 Vending Machine From Hell

A small one-day Unity game created as an architectural and gameplay experiment.

You operate a vending machine during a strange night shift, serving products to increasingly impatient customers before they lose their patience.

This project was intentionally kept small. The goal was not to create a commercial game, but to have fun while exploring how a Unity project can evolve from simple gameplay requirements into a clean, decoupled architecture.

---

## 🎮 About

**Vending Machine From Hell** is a short arcade-style prototype built around a simple gameplay loop:

1. A customer arrives.
2. The customer requests a product.
3. The player delivers a product through the vending machine.
4. Correct deliveries earn money.
5. Wrong products generate mistakes.
6. Customers lose patience over time.
7. When the night ends, the player receives a summary.

Some customers are more impatient than others, but their increased difficulty comes with a higher reward.

The entire game can be played in a short session, making it a deliberately small and focused project.

---

## 🧠 Main Goal

The main purpose of this project was **not content production**.

It was an opportunity to experiment with:

- Responsibility separation
- Event-driven communication
- ScriptableObjects as data/configuration
- Composition over unnecessary inheritance
- Gameplay state vs transient events
- UI observing gameplay rather than controlling it
- Audio reacting to gameplay events
- Avoiding premature abstractions
- Allowing architecture to emerge from actual problems

One of the main rules during development was:

> **Don't create an abstraction until the project gives us a reason to need it.**

---

## 🏗️ Architecture

The project is intentionally divided into small systems with clear responsibilities.

```text
GameManager
    │
    ├── Game state
    ├── Night lifecycle
    ├── Money
    ├── Customers served
    ├── Mistakes
    └── Timer
             │
             ▼
      CustomerManager
             │
             ├── Creates customers
             ├── Removes customers
             ├── Tracks current customer
             └── Coordinates customer lifecycle
                    │
                    ▼
                 Customer
                    │
                    ├── Patience
                    ├── Current order
                    ├── Product validation
                    ├── Reward calculation
                    └── Customer behavior
                           │
                           ▼
                      CustomerData
                           │
                           ├── Patience
                           ├── Reward multiplier
                           └── Sprite

ProductData
    │
    ├── Product name
    ├── Icon
    └── Price

Order
    │
    └── Represents the customer's request

OrderGenerator
    │
    └── Creates orders from available products

VendingMachine
    │
    └── Delivers products to the current customer

GameUI
    │
    └── Observes gameplay and presents information

AudioManager
    │
    └── Observes gameplay events and plays sounds
```

---

## 📡 Event-Driven Communication

Gameplay systems communicate through C# events where appropriate.

For example:

```text
Customer
    │
    └── ProductServed
             │
             ▼
      CustomerManager
             │
             ├── GameUI
             └── AudioManager
```

This allows UI and audio systems to react to gameplay without the gameplay code needing to know about them.

The same approach is used for events such as:

- `NightStarted`
- `NightEnded`
- `ProductServed`
- `WrongProductAttempted`
- `CustomerSpawned`
- `CustomerPatienceExpired`
- `MoneyChanged`
- `CustomersServedChanged`
- `MistakesChanged`
- `TimeRemainingChanged`

---

## 💾 ScriptableObjects

Two important parts of the game are data-driven through `ScriptableObject` assets.

### Products

`ProductData` contains the configuration for each product:

```text
ProductData
├── Product Name
├── Icon
└── Price
```

Current products include:

- Water
- Soda
- Chocolate

### Customers

`CustomerData` contains configuration for customer types:

```text
CustomerData
├── Patience
├── Reward Multiplier
└── Sprite
```

For example:

```text
Normal Customer
├── Patience: 10
└── Reward Multiplier: 1.0x

Impatient Customer
├── Patience: 6
└── Reward Multiplier: 1.5x
```

This allows new customer types to be created through data rather than requiring new branches inside the manager.

---

## 🔊 Audio

The game uses event-driven sound effects for gameplay events.

Current effects include:

- Night started
- Customer spawned
- Correct product
- Wrong product
- Customer patience expired
- Night ended
- Product button click

Ambient audio and suspense music are handled separately through looping `AudioSource` components.

The audio system does not need to be called directly by gameplay systems for gameplay events; it observes the relevant events instead.

---

## 🖥️ UI

The UI displays:

- Current money
- Customers served
- Mistakes
- Night timer
- Current customer order
- Product icon
- Customer patience
- Low-patience warning
- Gameplay feedback
- End-of-night results

The UI is intentionally treated as a presentation layer.

It observes gameplay state/events rather than becoming responsible for gameplay rules.

---

## 🛠️ Technology

- **Unity 6**
- **C#**
- **Unity Input System**
- **TextMeshPro**
- **ScriptableObjects**
- **C# Events / Delegates**
- **2D Unity workflow**

---

## 📁 Project Structure

```text
Assets
└── _Game
    ├── Scripts
    │   ├── Core
    │   ├── Customer
    │   ├── Products
    │   └── UI
    │
    ├── Prefabs
    ├── UI
    ├── Art
    └── Audio
```

---

## 🎯 Project Scope

This project was intentionally designed as a **one-day experiment**.

There are several possible directions for future development, such as:

- Multiple-product orders
- Quantity-based orders
- More customer types
- Additional customer reactions
- Combo systems
- Upgrades
- More visual polish
- Additional audio feedback
- More environmental details

However, these features are intentionally left outside the current scope.

The project already fulfilled its original purpose: **build something small, functional and fun while observing architecture emerge naturally from real development problems.**

---

## 📚 What This Project Demonstrated

The most valuable result of the project was not the amount of content produced.

It was learning when **not** to add complexity.

During development, several potential abstractions were considered and deliberately rejected because the project did not need them yet.

That led to a relatively simple architecture where each system has a reason to exist.

The project reinforced a simple principle:

> **Good architecture is not about having more abstractions. It's about giving each responsibility a clear home.**

---

## 🏁 Status

**Completed — September 2026**

This project is considered finished as a learning experiment.

It may be revisited someday for fun or experimentation, but its primary purpose has been fulfilled.

---

## 👤 Author

**Ramiro Mares de Oliveira**

Programmer focused on C#, game development, and software architecture.