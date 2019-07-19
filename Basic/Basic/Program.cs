using Autofac;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Basic
{
    class Program
    {

        static void Main(string[] args)
        {

            // Basic registration where further dependencies of the object to be created.
            var containerBuilder = new ContainerBuilder();

            // All ILog will be catered as Console Log
            containerBuilder.RegisterType<ConsoleLog>().As<ILog>();
            containerBuilder.RegisterType<Engine>();
            containerBuilder.RegisterType<Car>();

            IContainer container = containerBuilder.Build();

            // In this scenario, the constructor with maximum parameters data is selected.
            var car = container.Resolve<Car>();

            // This will have instance of Console Log.
            var log = container.Resolve<ILog>();

            car.GoAhead();

            Console.WriteLine("Selecting constructor");

            var builder = new ContainerBuilder();
            // AsSelf also registers the class
            builder.RegisterType<EmailLog>().As<ILog>().AsSelf();
            builder.RegisterType<ConsoleLog>().As<ILog>();
            builder.RegisterType<Engine>();
            builder.RegisterType<Car>().UsingConstructor(typeof(Engine));

            IContainer container1 = builder.Build();

            var car1 = container1.Resolve<Car>();

            car1.GoAhead();

            Console.WriteLine("Registering instances");

            builder = new ContainerBuilder();

            var logger = new ConsoleLog();

            builder.RegisterInstance(logger).As<ILog>();
            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            container = builder.Build();

            car = container.Resolve<Car>();

            car.GoAhead();

            Console.WriteLine("Lambda Expressions");

            builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLog>().As<ILog>();
            builder.Register((IComponentContext c) => new Engine(c.Resolve<ILog>(), 123));
            builder.RegisterType<Car>();

            container = builder.Build();

            car = container.Resolve<Car>();

            car.GoAhead();

            //Console.WriteLine("Generics");

            //builder = new ContainerBuilder();

            //builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));

            Console.ReadKey();
        }
    }

    interface ILog
    {
        void WriteData();
    }

    class ConsoleLog : ILog
    {
        public void WriteData()
        {
            Console.WriteLine("Console Log");
        }
    }

    class EmailLog : ILog
    {
        public void WriteData()
        {
            Console.WriteLine("Email Log");
        }
    }

    class Engine
    {
        public int Id { get; set; }
        public ILog Logger { get; set; }

        public Engine(ILog log)
        {
            Logger = log;
            Id = 100;
        }

        public Engine(ILog log, int id)
        {
            Logger = log;
            Id = id;
        }

        public void EngineInformation()
        {
            Console.WriteLine("Engine Id {0}", Id);
            Logger.WriteData();
        }
    }

    class Car
    {
        public ILog Logger { get; set; }
        public Engine Engine { get; set; }

        public Car(Engine engine)
        {
            this.Engine = engine;
            Logger = new EmailLog();
        }

        public Car(Engine engine, ILog log)
        {
            this.Engine = engine;
            Logger = log;
        }

        public void GoAhead()
        {
            Engine.EngineInformation();
            Logger.WriteData();
        }
    }
}
