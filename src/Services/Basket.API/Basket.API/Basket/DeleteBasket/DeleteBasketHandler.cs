
namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;

    public record DeleteBasketResult(bool IsSuccess);

    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
        }
    }

    public class DeleteBasketHandler
        : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
