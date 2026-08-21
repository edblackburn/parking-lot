# parking-lot

A simple domain expressed with Event Sourcing. Built with TDD, in modern C#, as a pure `decide`/`evolve` pair rather than a mutable aggregate object:

```
history ──fold(Evolve)──> state
                            │
command ────────────────────┤
                            ↓
                          Decide
                            │
                            ↓
                          events
```

- `Vehicle.Evolve(state, event) -> state` — folds one event onto state. Pure.
- `Vehicle.Decide(state, command) -> events` — decides what happened. Pure.
- `Decider<TState, TCommand, TEvent>` — the reusable pair, generic over any aggregate.

An event-sourced aggregate isn't fundamentally an object — it's a fold plus a decision function. The mutable aggregate class (private `_events`, `TakeChanges()`, `FromStream()`) is one implementation of that algebra, not the algebra itself. This repo demonstrates the algebra directly:

```csharp
var licencePlate = LicencePlate.Create("123-456").OkValue;
var state = Vehicle.Decider.Rehydrate(history);
var events = Vehicle.Decider.Decide(state, new Book(licencePlate, "Kernow", start, EndOf.Week));
```

State, commands, and events are all immutable records; behaviour is pure static functions. No mocks, no fixtures, no persistence — tests call `Decide` directly and assert the events it produces.

## Illegal states are unrepresentable

Two-case unions replace nullable primitives and thrown exceptions at the domain boundary:

- `Result<TOk, TError>` — `LicencePlate.Create` returns this instead of throwing. Callers must handle both branches.
- `RegistrationStatus` — `VehicleState`'s registration is `Unregistered` or `Registered(licencePlate)`, never a nullable `string`.
- Both are thin, domain-named wrappers over one shared internal primitive, `TwoCase<TFirst, TSecond>` (backed by [OneOf](https://github.com/mcintyre321/OneOf)), so the union-handling boilerplate exists once, not twice.

The `Evolve`/`Decide` switch expressions are exhaustive by construction: the project builds with `<Nullable>enable</Nullable>` and treats CS8509/CS8846 (non-exhaustive switch) as errors, so a new `VehicleEvent` or `VehicleCommand` subtype that isn't handled fails the build rather than throwing at runtime.

## Building and testing

Tooling is pinned via [mise](https://mise.jdx.dev/) (`.mise.toml`, `global.json`) — .NET 10, no other install required:

```bash
mise install
dotnet test src/ParkingLot.Tests/ParkingLot.Tests.csproj
```

Tests run on [TUnit](https://tunit.dev/) via the Microsoft.Testing.Platform runner (`dotnet test --coverage` for a coverage report). `VehicleDeciderScenario` gives tests a small `Given(...).When(...)` DSL over `Vehicle.Decider` — no test doubles, since the domain has no ports to substitute.

Feel free to ask questions here: [@ejblackburn](https://twitter.com/ejblackburn)

# Alternative Implementations

Here are some alternative takes. Please feel free to open PRs to include links to other implementations.

* [python](https://github.com/johnbywater/eventsourcing/blob/main/eventsourcing/examples/test_parking_lot.py) by John Bywater ([johnbywater](https://github.com/johnbywater) / [@johnbywater](https://twitter.com/johnbywater)) 
* [C#](https://gist.github.com/skleanthous/11ab343a9358b492bddef04f433580ad) by Savvas Kleanthous ([skleanthous](https://github.com/skleanthous) / [@SKleanthous](https://twitter.com/SKleanthous))
* [F#](https://gist.github.com/thinkbeforecoding/6ed8614f21517665937ddd4ed363be81) by Jérémie Chassaing ([thinkbeforecoding](https://github.com/thinkbeforecoding) / [@thinkb4coding](https://twitter.com/thinkb4coding))
