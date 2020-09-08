using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xamarin;
using Xamarin.Forms;

namespace MyContacts.iOS
{
    public class FormsBuilder
    {
        private IConfiguration _config;

        private List<Action<FormsBuilderContext, IServiceCollection>> _configureServicesActions = new List<Action<FormsBuilderContext, IServiceCollection>>();
        private IServiceFactoryAdapter _serviceProviderFactory = new ServiceFactoryAdapter<IServiceCollection>(new DefaultServiceProviderFactory());
        private Action<Xamarin.Forms.Application> _loadApplication;

        public FormsBuilder()
        {
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();
        }

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        public FormsBuilder UseForms()
        {
            ConfigureServices((context, services) =>
            {
                Forms.Init();
                FormsMaps.Init();
                FormsMaterial.Init();
            });

            return this;
        }

        public FormsBuilder UseApplication<T>(Action<Xamarin.Forms.Application> loadApplication)
            where T : Xamarin.Forms.Application
        {
            ConfigureServices((context, services) =>
            {
                services.AddSingleton<Xamarin.Forms.Application, T>();
            });

            _loadApplication = loadApplication;

            return this;
        }

        public FormsBuilder ConfigureServices(Action<FormsBuilderContext, IServiceCollection> configureDelegate)
        {
            _configureServicesActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        public void Init()
        {
            var services = new ServiceCollection();
            var formsBuilderContext = new FormsBuilderContext(Properties)
            {
                Configuration = _config
            };

            foreach (Action<FormsBuilderContext, IServiceCollection> configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(formsBuilderContext, services);
            }

            object containerBuilder = _serviceProviderFactory.CreateBuilder(services);

            IServiceProvider appServices = _serviceProviderFactory.CreateServiceProvider(containerBuilder);

            Xamarin.Forms.Application app = appServices.GetRequiredService<Xamarin.Forms.Application>();

            _loadApplication?.Invoke(app);
        }
    }

    public class FormsBuilderContext
    {
        public FormsBuilderContext(IDictionary<object, object> properties)
        {
            Properties = properties ?? throw new System.ArgumentNullException(nameof(properties));
        }

        /// <summary>
        /// The <see cref="IConfiguration" /> containing the merged configuration of the application and the <see cref="IHost" />.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary<object, object> Properties { get; }
    }

    internal interface IServiceFactoryAdapter
    {
        object CreateBuilder(IServiceCollection services);

        IServiceProvider CreateServiceProvider(object containerBuilder);
    }

    internal class ServiceFactoryAdapter<TContainerBuilder> : IServiceFactoryAdapter
    {
        private IServiceProviderFactory<TContainerBuilder> _serviceProviderFactory;
        private readonly Func<FormsBuilderContext> _contextResolver;
        private Func<FormsBuilderContext, IServiceProviderFactory<TContainerBuilder>> _factoryResolver;

        public ServiceFactoryAdapter(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
        {
            _serviceProviderFactory = serviceProviderFactory ?? throw new ArgumentNullException(nameof(serviceProviderFactory));
        }

        public ServiceFactoryAdapter(Func<FormsBuilderContext> contextResolver, Func<FormsBuilderContext, IServiceProviderFactory<TContainerBuilder>> factoryResolver)
        {
            _contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));
            _factoryResolver = factoryResolver ?? throw new ArgumentNullException(nameof(factoryResolver));
        }

        public object CreateBuilder(IServiceCollection services)
        {
            if (_serviceProviderFactory == null)
            {
                _serviceProviderFactory = _factoryResolver(_contextResolver());

                if (_serviceProviderFactory == null)
                {
                    throw new InvalidOperationException("The resolver returned a null IServiceProviderFactory");
                }
            }
            return _serviceProviderFactory.CreateBuilder(services);
        }

        public IServiceProvider CreateServiceProvider(object containerBuilder)
        {
            if (_serviceProviderFactory == null)
            {
                throw new InvalidOperationException("CreateBuilder must be called before CreateServiceProvider");
            }

            return _serviceProviderFactory.CreateServiceProvider((TContainerBuilder)containerBuilder);
        }
    }
}

