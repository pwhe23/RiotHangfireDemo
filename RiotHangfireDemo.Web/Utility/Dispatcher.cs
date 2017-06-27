using System;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    //REF: https://github.com/danielwertheim/routemeister/blob/feature-dispatchers-net-standard/src/projects/Routemeister/Dispatchers/AsyncDispatcher.cs
    public class Dispatcher
    {
        private readonly Routemeister.MessageHandlerCreator _messageHandlerCreator;
        private readonly Routemeister.MessageRoutes _messageRoutes;

        public Dispatcher(Routemeister.MessageHandlerCreator messageHandlerCreator, Routemeister.MessageRoutes messageRoutes)
        {
            _messageHandlerCreator = messageHandlerCreator;
            _messageRoutes = messageRoutes;
        }

        public TResponse Send<TResponse>(IRequest<TResponse> request)
        {
            var route = _messageRoutes.GetRoute(request.GetType());
            if (route.Actions.Length != 1)
                throw new ArgumentException($"The request '{route.MessageType.FullName}' reqires a single route.", nameof(request));

            var action = route.Actions[0];
            var envelope = new Routemeister.MessageEnvelope(request, route.MessageType);
            var creator = _messageHandlerCreator(action.HandlerType, envelope);
            var result = (TResponse)action.Invoke(creator, envelope.Message);

            return result;
        }
    };
}