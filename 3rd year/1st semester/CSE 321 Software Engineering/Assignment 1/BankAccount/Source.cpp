#include "BankAccount.h"
#include <iostream>

using namespace std;

void main()
{
	BankAccount b1("Mohamed", "20", 2000000);
	b1.displayAccount();
	b1.deposit(250000);
	b1.displayAccount();
	b1.withdraw(50000);
	b1.displayAccount();
}