using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace InyeccionDeDependencias2
{
    /// <summary>
    /// Sin inyección de dependencias, la clase Worker NO tenía parámetros y además creaba de forma rígida una instancia de la clase MessageWriter para enviar el mensaje. Pero con inyección de dependencias, la clase Worker recibe inyectada como argumento una instancia de la clase MessageWriter.
    /// Entonces, cuando se crea una instancia de Worker, el contenedor de servicios (que es el Host en el método Main de la clase Program) busca una implementación registrada para IMessageWriter. El contenedor la crea (si aún no lo ha hecho) y la inyecta en el constructor de Worker.
    /// </summary>
    /// <param name="messageWriter"></param>
    public sealed class Worker(IMessageWriter messageWriter) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                messageWriter.Write($"La hora exacta: {DateTimeOffset.Now}");
                await Task.Delay(1_000, stoppingToken);
            }
        }
    }

}
