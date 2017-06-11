using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using MediatR;
using Newtonsoft.Json;

namespace RiotHangfireDemo
{
    public interface ICommand
    {
        // Placeholder interface to find commands
    };

    public interface ICommander
    {
        object Execute(string commandName, string commandJson);
        object Execute(object cmd);
    };

    public class Commander : ICommander
    {
        private static Dictionary<string, Type> _commands;
        private readonly IMediator _mediator;

        public Commander(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Find all ICommand classes in provided Assemblies
        /// </summary>
        /// <param name="assemblies"></param>
        public static void Initialize(params Assembly[] assemblies)
        {
            _commands = assemblies
                .SelectMany(x => x.ExportedTypes)
                .Where(x => x.IsClass
                            && !x.IsAbstract
                            && !x.IsInterface
                            && !x.IsNested && x.HasInterface<ICommand>())
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
                commandJson = "{}";
            }

            var command = JsonConvert.DeserializeObject(commandJson, commandType);
            var result = Execute(command);

            return result;
        }

        /// <summary>
        /// Translate object-typed command to IRequest type so it can be executed by MediatR.
        /// </summary>
        public object Execute(object cmd)
        {
            if (cmd == null)
                return null;

            try
            {
                var requestInterface = cmd.GetType().GetInterface("IRequest`1");
                var sendMethod = _mediator.GetType().GetMethod("Send").MakeGenericMethod(requestInterface.GetGenericArguments());

                var result = sendMethod.Invoke(_mediator, new[] { cmd });

                return result;
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"ERROR on request type {cmd.GetType()}", ex);
            }

            return null;
        }

        public TResponse Execute<TResponse>(IRequest<TResponse> cmd)
        {
            return _mediator.Send(cmd);
        }
    };
}