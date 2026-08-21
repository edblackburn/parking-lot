using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingLot {
	public static class Vehicle {
		public static readonly Decider<VehicleState, VehicleCommand, VehicleEvent> Decider =
			new Decider<VehicleState, VehicleCommand, VehicleEvent>(
				Initial: VehicleState.Initial,
				Decide: Decide,
				Evolve: Evolve);

		public static VehicleState Evolve(VehicleState state, VehicleEvent @event) =>
			@event switch {
				VehicleRegistered e => state with { Registration = RegistrationStatus.Registered(e.LicencePlate) },
				VehicleBooked e => state with {
					Bookings = state.Bookings.Add(
						new Booking(e.Location, new Period(e.Start, e.Finish)))
				},
				InspectionFailed _ => state,
				null => throw new ArgumentNullException(nameof(@event)),
				_ => throw new ArgumentOutOfRangeException(
					nameof(@event),
					@event,
					$"Unhandled {nameof(VehicleEvent)} subtype '{@event.GetType().Name}' — add a case to {nameof(Evolve)}.")
			};

		public static IReadOnlyList<VehicleEvent> Decide(VehicleState state, VehicleCommand command) =>
			command switch {
				Book c => DecideBook(state, c),
				Inspect c => DecideInspect(state, c),
				null => throw new ArgumentNullException(nameof(command)),
				_ => throw new ArgumentOutOfRangeException(
					nameof(command),
					command,
					$"Unhandled {nameof(VehicleCommand)} subtype '{command.GetType().Name}' — add a case to {nameof(Decide)}.")
			};

		private static IReadOnlyList<VehicleEvent> DecideBook(VehicleState state, Book command) {
			var finish = command.Product(command.Start);
			var booked = new VehicleBooked(
				LicencePlate: command.LicencePlate.ToString(),
				Location: command.Location,
				Start: command.Start,
				Finish: finish);

			return state.Registration.IsRegistered
				? new VehicleEvent[] { booked }
				: new VehicleEvent[] { new VehicleRegistered(command.LicencePlate.ToString()), booked };
		}

		private static IReadOnlyList<VehicleEvent> DecideInspect(VehicleState state, Inspect command) {
			var registeredToInspectedVehicle = state.Registration.Match(
				onUnregistered: () => false,
				onRegistered: plate => plate == command.LicencePlate.ToString());

			var when = command.When.ToUnixTimeSeconds();
			var covered = registeredToInspectedVehicle
				&& state.Bookings.Any(booking => booking.Covers(command.Location, when));

			return covered
				? Array.Empty<VehicleEvent>()
				: new VehicleEvent[] {
					new InspectionFailed(
						InspectionId: command.InspectionId.ToString(),
						LicencePlate: command.LicencePlate.ToString(),
						Location: command.Location)
				};
		}
	}
}
