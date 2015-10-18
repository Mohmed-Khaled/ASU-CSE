#include "Move.h"
#include <iostream>

using namespace std;

Move::Move(double a, double b)
{
	Move::x = a;
	Move::y = b;
}

void Move::showmove() const
{
	cout << "x: " << Move::x << ", y: " << Move::y << endl;
}

Move Move::add(const Move & m) const
{
	double newX = Move::x + m.x;
	double newY = Move::y + m.y;
	return Move(newX, newY);
}

void Move::reset(double a, double b)
{
	Move::x = a;
	Move::y = b;
}