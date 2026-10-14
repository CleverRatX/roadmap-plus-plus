namespace RoadmapPlusPlus.BuildingBlocks
{
    /// <summary>
    /// Ожидаемая ошибка: узел не найден, нет прав, неверные данные, конфликт.
    /// Возвращается через <see cref="Result"/>; исключения остаются для непредвиденного.
    /// </summary>
    public sealed record Error
    {
        private Error( ErrorType type, string code, string message, IReadOnlyDictionary<string, string[]> fields )
        {
            Type = type;
            Code = code;
            Message = message;
            Fields = fields;
        }

        public ErrorType Type { get; }

        /// <summary>Стабильный машинный код вида <c>planning.item_not_found</c>: по нему клиент различает ошибки.</summary>
        public string Code { get; }

        /// <summary>Текст для человека.</summary>
        public string Message { get; }

        /// <summary>Ошибки по полям запроса: имя поля → сообщения. Заполняется только у <see cref="ErrorType.Validation"/>.</summary>
        public IReadOnlyDictionary<string, string[]> Fields { get; }

        public static Error Validation( string code, string message, IReadOnlyDictionary<string, string[]>? fields = null )
        {
            return new Error( ErrorType.Validation, code, message, fields ?? new Dictionary<string, string[]>() );
        }

        public static Error Forbidden( string code, string message )
        {
            return new Error( ErrorType.Forbidden, code, message, new Dictionary<string, string[]>() );
        }

        public static Error NotFound( string code, string message )
        {
            return new Error( ErrorType.NotFound, code, message, new Dictionary<string, string[]>() );
        }

        public static Error Conflict( string code, string message )
        {
            return new Error( ErrorType.Conflict, code, message, new Dictionary<string, string[]>() );
        }
    }
}
