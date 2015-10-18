#include <string>
using namespace std;

class BankAccount{
public:
	BankAccount();
	BankAccount(string depositor_name, string account_number, int balance);
	void deposit(int value);
	void withdraw(int value);
	void displayAccount();
private:
	string depositor_name;
	string account_number;
	int balance;
};