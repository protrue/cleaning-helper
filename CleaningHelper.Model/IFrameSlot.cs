namespace CleaningHelper.Model
{
    public interface IFrameSlot
    {
        /// <summary>
        /// Имя слота
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Является ли слот результатом
        /// </summary>
        bool IsResult { get; set; }
    }
}