#include <iostream>
#include <string>
#include <sstream>
#include "Polynomial.h"

using namespace std;

Polynomial * input();

int main(void)
{
	int opr;
	cout << "Choose an Operation 1.Evaluate 2.Multiply 3.Differentiate: ";
	cin >> opr;
	if (opr == 1)
	{
		Polynomial * pv;
		try
		{
			pv = input();
		}
		catch (char * e) 
		{
			cout << e << endl;
			return 2;
		}
		int x = 0;
		cout << "Enter the value of x: ";
		cin >> x;
		cout << pv->evalute(x) << endl;
		delete pv;
	}
	else if(opr == 2)
	{
		Polynomial * p1;
		Polynomial * p2;
		try
		{
			p1 = input();
			p2 = input();
		}
		catch (char * e)
		{
			cout << e << endl;
			return 2;
		}
		Polynomial * p3 = (*p1) * (*p2);
		cout << p3->print() << endl;
		delete p1;
		delete p2; 
		delete p3;
	}
	else if (opr == 3)
	{
		Polynomial * pd;
		try
		{
			pd = input();
		}
		catch (char * e)
		{
			cout << e << endl;
			return 2;
		}
		cout << pd->differentiate() << endl;
		delete pd;
	}
	else
	{
		cout << "Not Supported Operation" << endl;
		return 1;
	}
	return 0;
}

Polynomial * input()
{
	int type;
	int order;
	double * terms;
	int count = 0;
	cout << "Choose the Polynomial Type 1.Normal 2.Sparse: ";
	cin >> type;
	if (!(type == 1 || type == 2)) throw "Not Supported Type";
	cout << "Enter the Polynomial Order: ";
	cin >> order;
	if (order < 0) throw "Order must be positive integer";
	string equation;
	if (type == 1)
	{
		terms = new double[order + 1];
		cout << "e.g. 4 0 2 means 4*x^2 + 2" << endl;
	}
	else
	{
		terms = new double[2 * (order + 1)];
		cout << "e.g. 1 99 2 1 means x^99 + 2*x" << endl;
	}
	cout << "Enter the Polynomial Coefficients: ";
	cin.ignore(numeric_limits<streamsize>::max(), '\n');
	getline(cin, equation);
	istringstream is(equation);
	string term;
	while (getline(is, term, ' '))
	{
		terms[count++] = stod(term);
	}
	switch (type)
	{
	case 1:
		if (count != order + 1) throw "Invalid number of coefficients";
		break;
	case 2:
		if ((count % 2) != 0) throw "Invalid number of coefficients";
		break;
	}
	return new Polynomial(terms, count, order, type);
}