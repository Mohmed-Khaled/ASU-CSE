#pragma once
#include "Term.h"

using namespace std;

class Polynomial 
{
public:
	Polynomial();
	Polynomial(double * terms,int count, int order, int type);
	~Polynomial();
	string evalute(double valueX);
	string differentiate();
	string print();
	Polynomial * operator+(const Polynomial& p);
	Polynomial * operator*(const Polynomial& p);
private:
	Term * _front;
	int _order;
	int _type;
	Polynomial * Multiply(Term * t);
};