namespace ParkingLot
{
	public class InspectionId {
		private readonly string _licencePlate;

		public static InspectionId Create(string inspectionId) => new InspectionId(inspectionId);

		private InspectionId(string licencePlate) => _licencePlate = licencePlate;

		public override string ToString() => _licencePlate;
	}
}
