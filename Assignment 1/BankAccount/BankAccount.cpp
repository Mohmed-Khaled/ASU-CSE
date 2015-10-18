#include <iostream>
#include "BankAccount.h"
using namespace std;

BankAccount::BankAccount()
{
	BankAccount::depositor_name = "";
	BankAccount::account_number = "";
	BankAccount::balance = 0;
}

BankAccount::BankAccount(string depositor_name, string account_number, int balance)
{
	BankAccount::depositor_name = depositor_name;
	BankAccount::account_number = account_number;
	BankAccount::balance = balance;
}

void BankAccount::deposit(int value)
{
	BankAccount::balance += value;
}

void BankAccount::withdraw(int value)
{
	BankAccount::balance -= value;
}

void BankAccount::displayAccount()
{
	cout << "Name: " << BankAccount::depositor_name << endl;
	cout << "Account Number: " << BankAccount::account_number << endl;
	cout << "Balance: " << BankAccount::balance << endl;
}