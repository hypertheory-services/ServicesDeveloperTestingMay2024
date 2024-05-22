


namespace Banking;

public class Account(ICalculateBonuses bonusCalculator, IProvideCustomerRelations customerRelations)
{
    private decimal _balance = 7000M;

    public void Deposit(decimal amountToDeposit)
    {
        decimal amountOfBonus = bonusCalculator.HowMuchBonusFor(_balance, amountToDeposit);
        _balance += amountToDeposit + amountOfBonus;
    }

    public decimal GetBalance()
    {
        return _balance;
    }

    public void Withdraw(decimal amountToWithdraw)
    {
        if (amountToWithdraw > _balance)
        {
            throw new AccountOverdraftException();
        }
        _balance -= amountToWithdraw;

        if (_balance == 0)
        {
            customerRelations.SendEmailAboutBalanceBeingZero(this);
        }
    }
}

public class AccountOverdraftException : ArgumentOutOfRangeException;