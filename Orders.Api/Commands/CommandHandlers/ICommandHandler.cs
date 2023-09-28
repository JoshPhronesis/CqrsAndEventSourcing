namespace Orders.Api.Commands.CommandHandlers;

public interface ICommandHandler<in TCommand> where TCommand: ICommand
{
    Task HandleAsync(TCommand command);
}