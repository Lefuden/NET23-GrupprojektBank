﻿using NET23_GrupprojektBank.Managers;
 
namespace NET23_GrupprojektBank.Managers
{
        public enum EventStatus
    {
        LoginSuccess,
        LoginFailed,
        LoginLocked,
        LoginUnlocked,
        CurrencyExchangeRateUpdateSuccess,
        CurrencyExchangeRateUpdateFailed,
        CheckingCreationSuccess,
        CheckingCreationFailed,
        SavingsCreationSuccess,
        SavingsCreationFailed,
        TransactionSuccess,
        TransactionFailed,
        TransactionCreated,
        DepositSuccess,
        DepositFailed,
        DepositFailedNegativeOrZeroSum,
        DepositCreated,
        WithdrawalSuccess,
        WithdrawalFailed,
        WithdrawalFailedInsufficientFunds,
        WithdrawalCreated,
        TransferSuccess,
        TransferReceivedSuccess,
        TransferFailed,
        TransferFailedInsufficientFunds,
        TransferFailedNegativeOrZeroSum,
        TransferCreated,
        LoanSuccess,
        LoanFailed,
        LoanFailedNegativeOrZeroSum,
        LoanCreated,
        ValidUsername,
        InvalidUsername,
        ValidPassword,
        InvalidPassword,
        AccountCreationSuccess,
        AccountCreationFailed,
        AdressSuccess,
        AdressFailed,
        EmailSuccess,
        EmailFailed,
        PhoneSuccess,
        PhoneFailed,
        ContactInformationSuccess,
        ContactInformationFailed,
        InvalidInput,
        TransactionManagerAddedToQueueSuccess,
        TransactionManagerAddedToQueueFailed,
        NonAdminUser,
        AdminUpdatedCurrencyFromFile,
        AdminUpdatedCurrencyFromWebApi,
        AdminInvalidInput
    }
}