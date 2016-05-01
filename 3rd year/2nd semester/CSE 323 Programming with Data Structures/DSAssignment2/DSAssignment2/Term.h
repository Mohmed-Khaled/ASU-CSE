#pragma once
class Term 
{
private:
	double _coeff;
	int _pow;
	Term * _next;
public:
	Term() : _coeff(0),_pow(0),_next(nullptr) {}
	friend class Polynomial;
};