using System.Collections.Generic;
using System.Linq;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;

namespace ParkingLot.Tests {
	public sealed class VehicleDeciderScenario {
		private readonly IReadOnlyList<VehicleEvent> _history;

		private VehicleDeciderScenario(IReadOnlyList<VehicleEvent> history) => _history = history;

		public static VehicleDeciderScenario Given(params VehicleEvent[] history) =>
			new VehicleDeciderScenario(history);

		public IReadOnlyList<VehicleEvent> When(VehicleCommand command) {
			var state = Vehicle.Decider.Rehydrate(_history);
			return Vehicle.Decider.Decide(state, command);
		}
	}
}
