namespace InyeccionDeDependencias2
{
    public class MessageWriter : IMessageWriter
    {
        public void Write(string message)
        {
            Console.WriteLine($"El mensaje es: \"{message}\")");
        }
    }
}
