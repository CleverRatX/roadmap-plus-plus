namespace RoadmapPlusPlus.BuildingBlocks
{
    /// <summary>
    /// Вид ожидаемой ошибки. От него зависит HTTP-статус: 400, 403, 404 или 409.
    /// </summary>
    public enum ErrorType
    {
        Validation,
        Forbidden,
        NotFound,
        Conflict,
    }
}
