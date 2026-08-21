using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingLot {
	public sealed record Decider<TState, TCommand, TEvent>(
		TState Initial,
		Func<TState, TCommand, IReadOnlyList<TEvent>> Decide,
		Func<TState, TEvent, TState> Evolve) {

		public TState Rehydrate(IEnumerable<TEvent> history) =>
			history.Aggregate(Initial, Evolve);
	}
}
