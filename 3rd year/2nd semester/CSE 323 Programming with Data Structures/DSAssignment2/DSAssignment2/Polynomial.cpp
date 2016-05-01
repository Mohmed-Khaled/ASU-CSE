#include "Polynomial.h"
#include <iostream>
#include <string>

Polynomial::Polynomial()
{
	_front = new Term();
}

Polynomial::Polynomial(double * terms, int count,int order, int type)
{
	_order = order;
	_type = type;
	_front = new Term();
	Term * i = _front;
	switch (_type)
	{
	case 1: //Normal
		for (int it = 0; it < count; it++)
		{
			i->_coeff = terms[it];
			i->_pow = order--;
			if (order >= 0) 
			{
				i->_next = new Term();
				i = i->_next;
			}
		}
		break;
	case 2: //Sparse
		for (int it = 0; it < count; it++)
		{
			i->_coeff = terms[it];
			++it;
			int power = int(terms[it]);
			i->_pow = power;
			if (it + 1 < count)
			{
				i->_next = new Term();
				i = i->_next;
			}
		}
		break;
	default:
		throw new exception("Not Supported type");
	}
	delete[] terms;
}

Polynomial::~Polynomial()
{
	while (_front != nullptr) 
	{
		Term * tmp = _front;
		_front = _front->_next;
		delete tmp;
	}
}


string Polynomial::evalute(double valueX)
{
	double result = 0;
	Term * i = _front;
	switch (_type)
	{
	case 1: //Normal
		while(i->_pow != 0)
		{
			result += i->_coeff;
			result *= valueX;
			i = i->_next;
		}
		result += i->_coeff;
		break;
	case 2: //Sparse
		while (i != nullptr)
		{
			result += i->_coeff * pow(valueX,i->_pow);
			i = i->_next;
		}
		break;
	default:
		throw new exception("Not Supported type");
	}
	return "P(" + to_string(valueX) + ") = " + to_string(result);
}

string Polynomial::differentiate()
{
	Term * i = _front;
	while (i != nullptr) 
	{
		i->_coeff *= i->_pow--;
		if (i->_next != nullptr && i->_next->_pow == 0) 
		{
			Term * tmp = i->_next;
			i->_next = nullptr;
			delete tmp;
			break;
		}
		else
		{
			i = i->_next;
		}
	}
	return print();
}

string Polynomial::print()
{
	string equation = "P(x) = ";
	Term * i = _front;
	while (i != nullptr)
	{
		if(i->_coeff == 0)
		{
			i = i->_next;
			continue;
		}
		if (i != _front) equation += " + ";
		if (i->_pow != 0 && i->_pow != 1)
			equation += to_string(i->_coeff) + " * x ^ " + to_string(i->_pow);
		else if (i->_pow == 1)
			equation += to_string(i->_coeff) + " * x";
		else if (i->_pow == 0)
			equation += to_string(i->_coeff);
		i = i->_next;
	}
	return equation;
}

Polynomial * Polynomial::operator+(const Polynomial & p)
{
	Polynomial * result =  new Polynomial();
	Term * i = _front;
	Term * j = p._front;
	Term * r = result->_front;
	if (_type == p._type) result->_type = _type;
	else result->_type = 2;
	if (_order >= p._order) result->_order = _order;
	else result->_order = p._order;
	while (i != nullptr && j != nullptr) 
	{
		if (i->_pow > j->_pow) 
		{
			r->_coeff = i->_coeff;
			r->_pow = i->_pow;
			if (i->_next != nullptr || j != nullptr) 
			{
				r->_next = new Term();
				r = r->_next;
			}
			i = i->_next;
		}
		else if (i->_pow < j->_pow)
		{
			r->_coeff = j->_coeff;
			r->_pow = j->_pow;
			if (i != nullptr || j->_next != nullptr)
			{
				r->_next = new Term();
				r = r->_next;
			}
			j = j->_next;
		}
		else
		{
			r->_coeff = i->_coeff + j->_coeff;
			r->_pow = i->_pow;
			if (i->_next != nullptr || j->_next != nullptr)
			{
				r->_next = new Term();
				r = r->_next;
			}
			i = i->_next;
			j = j->_next;
		}
	}
	while (i != nullptr) 
	{
		r->_coeff = i->_coeff;
		r->_pow = i->_pow;
		if (i->_next != nullptr)
		{
			r->_next = new Term();
			r = r->_next;
		}
		i = i->_next;
	}
	while (j != nullptr) 
	{
		r->_coeff = j->_coeff;
		r->_pow = j->_pow;
		if (j->_next != nullptr)
		{
			r->_next = new Term();
			r = r->_next;
		}
		j = j->_next;
	}
	return result;
}

Polynomial * Polynomial::operator*(const Polynomial & p)
{
	Polynomial * result = new Polynomial();
	if (_type == p._type) result->_type = _type;
	else result->_type = 2;
	result->_order = _order + p._order;
	delete result->_front;
	result->_front = nullptr;
	Term * j = p._front;
	while (j != nullptr)
	{
		if (j->_coeff != 0)
		{
			Polynomial * tmp = Multiply(j);
			result = *result + *tmp;
			delete tmp;
		}
		j = j->_next;
	}
	return result;
}

Polynomial * Polynomial::Multiply(Term * t)
{
	Polynomial * result = new Polynomial();
	result->_type = 2;
	result->_order = _order + t->_pow;
	Term * i = _front;
	Term * r = result->_front;
	while (i != nullptr) 
	{
		r->_coeff = i->_coeff * t->_coeff;
		r->_pow = i->_pow + t->_pow;
		if (i->_next != nullptr)
		{
			r->_next = new Term();
			r = r->_next;
		}
		i = i->_next;
	}
	return result;
}
