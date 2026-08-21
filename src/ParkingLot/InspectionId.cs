namespace ParkingLot {
	public sealed record InspectionId {
		private readonly string _inspectionId;

		public static InspectionId Create(string inspectionId) => new InspectionId(inspectionId);

		private InspectionId(string inspectionId) => _inspectionId = inspectionId;

		public override string ToString() => _inspectionId;
	}
}
