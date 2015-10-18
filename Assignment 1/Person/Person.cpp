#include <iostream>
#include "Person.h"
using namespace std;

Person::Person(const string & ln, const char * fn)
{
	Person::lname = ln;
	strcpy_s(Person::fname, fn);
}

void Person::show() const
{
	cout << Person::fname << " " << Person::lname << endl;
}

void Person::formalShow() const
{
	cout << Person::lname << ", " << Person::fname << endl;
}