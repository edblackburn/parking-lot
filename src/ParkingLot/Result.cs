using System;
using OneOf.Types;

namespace ParkingLot {
	public sealed record Result<TOk, TError> {
		private readonly TwoCase<TOk, Error<TError>> _value;

		public static Result<TOk, TError> Ok(TOk value) => new Result<TOk, TError>(TwoCase<TOk, Error<TError>>.First(value));

		public static Result<TOk, TError> Error(TError error) =>
			new Result<TOk, TError>(TwoCase<TOk, Error<TError>>.Second(new Error<TError>(error)));

		private Result(TwoCase<TOk, Error<TError>> value) => _value = value;

		public bool IsOk => _value.IsFirst;

		public TResult Match<TResult>(Func<TOk, TResult> onOk, Func<TError, TResult> onError) =>
			_value.Match(onOk, error => onError(error.Value));

		public TOk OkValue => _value.AsFirst(
			() => new InvalidOperationException("Result is an error; there is no ok value."));

		public TError ErrorValue => _value.AsSecond(
			() => new InvalidOperationException("Result is ok; there is no error value.")).Value;
	}
}
