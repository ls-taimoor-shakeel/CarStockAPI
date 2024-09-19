public interface IDealerService
{
    Task<Dealer> RegisterDealer(RegisterDealer dealer);
    Task<string> Login(LoginDealer loginDetails);
}
