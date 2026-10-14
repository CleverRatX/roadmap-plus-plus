using System.Diagnostics.CodeAnalysis;

namespace RoadmapPlusPlus.BuildingBlocks
{
    /// <summary>
    /// Итог операции без значения: успех или <see cref="BuildingBlocks.Error"/>.
    /// Из метода можно вернуть <see cref="BuildingBlocks.Error"/> напрямую — он превратится в неуспешный результат.
    /// </summary>
    public class Result
    {
        protected Result( Error? error )
        {
            Error = error;
        }

        public Error? Error { get; }

        [MemberNotNullWhen( false, nameof( Error ) )]
        public bool IsSuccess => Error is null;

        public static Result Success()
        {
            return new Result( null );
        }

        public static implicit operator Result( Error error )
        {
            ArgumentNullException.ThrowIfNull( error );
            return new Result( error );
        }
    }

    /// <summary>
    /// Итог операции со значением: <see cref="Value"/> или <see cref="Result.Error"/>.
    /// Из метода можно вернуть и значение, и <see cref="BuildingBlocks.Error"/> напрямую.
    /// </summary>
    public sealed class Result<T> : Result
    {
        private readonly T? _value;

        private Result( T value )
            : base( null )
        {
            _value = value;
        }

        private Result( Error error )
            : base( error )
        {
        }

        /// <summary>Значение успешного результата. У неуспешного бросает <see cref="InvalidOperationException"/>: сначала проверьте <see cref="Result.IsSuccess"/>.</summary>
        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException( $"Результат с ошибкой {Error.Code} не содержит значения." );

        public static implicit operator Result<T>( T value )
        {
            return new Result<T>( value );
        }

        public static implicit operator Result<T>( Error error )
        {
            ArgumentNullException.ThrowIfNull( error );
            return new Result<T>( error );
        }
    }
}
