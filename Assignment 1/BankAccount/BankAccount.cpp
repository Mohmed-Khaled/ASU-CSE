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
	if (value > 0)
		BankAccount::balance += value;
	else
		cout << "Wrong value for money" << endl;
}

void BankAccount::withdraw(int value)
{
	if (value <= 0)
		cout << "Wrong value for money" << endl;
	else if(value > BankAccount::balance)
		cout << "You don't have the required money in your account" << endl;
	else
		BankAccount::balance -= value;
}

void BankAccount::displayAccount()
{
	cout << "Name: " << BankAccount::depositor_name << endl;
	cout << "Account Number: " << BankAccount::account_number << endl;
	cout << "Balance: " << BankAccount::balance << endl;
}
