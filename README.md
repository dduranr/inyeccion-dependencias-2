# Inyección de dependencias

La inyección de dependencias es una forma de implementar el principio SOLID: *inversión de dependencia*, que consiste en que las entidades (clases, objetos, etc) deben depender de abstracciones y no de implementaciones concretas.

Este proyecto es de **C#**, y está basado en el ejemplo de la documentación oficial [learn.microsoft.com](https://learn.microsoft.com/es-es/dotnet/core/extensions/dependency-injection)

El objetivo de este proyecto es que cada segundo se imprima un mensaje en consola.

En C# y .NET, por ejemplo, se puede usar el paquete DependencyInjection. Éste es un contenedor de inversión de control. Es decir, es un contenedor de objetos que se encarga de crear y destruir las instancias de las clases. Y, además, gestiona las dependencias entre ellas. La idea es que el programador configure todas las clases que entran en juego en la aplicación y su ciclo de vida. Después, el contenedor se encarga de crear las instancias de las clases y de inyectar las dependencias entre ellas cuando sea necesario. En el siguiente ejemplo de una aplicación de consola c# se usa este inyector de dependencias (DependencyInjection):

1. Se define el método Main, que es el punto de entrada para la aplicación de consola en C#.
2. Con "builder" se crea una instancia del generador de aplicación de host
	1. Este host es un contenedor de servicios.
	2. El host contiene el proveedor de servicios de inyección de dependencias. También contiene el resto de servicios pertinentes que se requieren para crear instancias de Worker automáticamente y proporcionar la implementación de IMessageWriter correspondiente como argumento.
3. Se usa builder para configurar los servicios mediante el registro de:
    1. La clase Worker como servicio hospedado.
    2. La interfaz IMessageWriter como servicio singleton con una implementación correspondiente de la clase MessageWriter.
4. Finalmente se compila el host y se ejecuta.

Supongamos que creo otra clase llamada OtraClase.cs que también recibe como parámetro una instancia de MessageWriter. ¿Automáticamente el contenedor de servicios la inyectaría en OtraClase.cs? No. Para que el contenedor de servicios inyecte una dependencia en una nueva clase, es necesario registrar explícitamente esa nueva clase en el contenedor. Esto se hace utilizando los métodos de extensión como AddSingleton, AddTransient o AddScoped, según el ciclo de vida deseado para la instancia: builder.Services.AddTransient<OtraClase>();

**Clase Program**

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    namespace InyeccionDeDependencias2
    {
        class Program
        {
            static void Main(string[] args)
            {
                HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

                builder.Services.AddHostedService<Worker>();
                builder.Services.AddSingleton<IMessageWriter, MessageWriter>();

                using IHost host = builder.Build();

                host.Run();
            }
        }
    }


**Iterface IMessageWriter**

	public interface IMessageWriter
	{
	    void Write(string message);
	}


**Clase MessageWriter**

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


**Clase Worker**

	namespace InyeccionDeDependencias2
	{
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

