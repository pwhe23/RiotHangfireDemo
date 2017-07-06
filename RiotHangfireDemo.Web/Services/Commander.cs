using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Newtonsoft.Json;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    /// <summary>
    /// Allow execution of ICommands by MediatR using name and json.
    /// </summary>
    public class Commander : ICommander
    {
        private static Dictionary<string, Type> _commands;
        private readonly Dispatcher _dispatcher;

        public Commander(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Find all ICommand classes in provided Assemblies which can be executed.
        /// </summary>
        public static void Initialize(params Assembly[] assemblies)
        {
            var requestType = typeof(IRequest<>);
            _commands = assemblies
                .SelectMany(x => x.ExportedTypes)
                .Where(x => x.IsClass
                            && !x.IsAbstract
                            && !x.IsInterface
                            && !x.IsNested
                            && x.HasOpenInterface(requestType))
                .ToDictionary(x => x.Name, x => x);
        }

        /// <summary>
        /// Execute command by Name and Json
        /// </summary>
        public object Execute(string commandName, string commandJson)
        {
            var commandType = _commands[commandName];

            if (string.IsNullOrWhiteSpace(commandJson))
            {
                commandJson = "{}"; // so JsonConvert doesn't crash if passed null
            }

            var command = JsonConvert.DeserializeObject(commandJson, commandType);
            var result = Execute(command);

            return result;
        }

        /// <summary>
        /// Translate object-typed command to IRequest type using Reflection so it can be
        /// executed by MediatR.
        /// </summary>
        public object Execute(object command)
        {
            if (command == null)
                return null;

            try
            {
                var mediator = this;

                var requestInterface = command
                    .GetType()
                    .GetInterface("IRequest`1");

                var executeMethod = mediator
                    .GetType()
                    .GetMethod(nameof(Send))
                    .MakeGenericMethod(requestInterface.GetGenericArguments());

                var result = executeMethod.Invoke(mediator, new[] { command });
                return result;
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"ERROR on request type {command.GetType()}", ex);
            }

            return null;
        }

        /// <summary>
        /// Pass-through to MediatR, provided so ICommander can be injected everywhere
        /// in favor of IMediator.
        /// </summary>
        public TResponse Send<TResponse>(IRequest<TResponse> command)
        {
            return _dispatcher.Send(command);
        }
    };
}