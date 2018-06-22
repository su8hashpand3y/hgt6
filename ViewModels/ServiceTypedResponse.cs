namespace HGT6.ViewModels
{
    public class ServiceTypedResponse<T>
    {
        public string Status { get; set; }
        public T Message { get; set; }
    }
}