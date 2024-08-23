using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InyeccionDeDependencias2
{
    class Program
    {
        /// <summary>
        /// Se define el método Main, que es el punto de entrada para la aplicación de consola en C#.
        /// Con "builder" se crea una instancia del generador de aplicación de host.
        /// Se usa builder para configurar los servicios mediante el registro de:
        ///     1. La clase Worker como servicio hospedado.
        ///     2. La interfaz IMessageWriter como servicio singleton con una implementación correspondiente de la clase MessageWriter.
        /// Finalmente se compila el host y se ejecuta.
        /// El host contiene el proveedor de servicios de inyección de dependencias. También contiene el resto de servicios pertinentes que se requieren para crear instancias de Worker automáticamente y proporcionar la implementación de IMessageWriter correspondiente como argumento.
        ///
        /// Supongamos que creo otra clase llamada OtraClase.cs que también recibe como parámetro una instancia de MessageWriter. ¿Automáticamente el contenedor de servicios la inyectaría en OtraClase.cs? No. Para que el contenedor de servicios inyecte una dependencia en una nueva clase, es necesario registrar explícitamente esa clase en el contenedor. Esto se hace utilizando los métodos de extensión como AddSingleton, AddTransient o AddScoped, según el ciclo de vida deseado para la instancia: builder.Services.AddTransient<OtraClase>();
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
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
