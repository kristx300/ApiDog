namespace APIDog.Core
{
    /// <summary>
    /// Interface for custom ShowDialog()
    /// </summary>
    /// <typeparam name="T">Type of returned object</typeparam>
    public interface IWindow<T>
    {
        bool? ShowDialog();

        T Data { get; }
        bool CorrectClosing { get; set; }
    }
}