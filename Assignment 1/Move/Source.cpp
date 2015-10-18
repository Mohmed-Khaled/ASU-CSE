#include <iostream>
#include "Move.h"
using namespace std;

void main()
{
	Move one(2,3);
	Move two(6,4);
	one.showmove();
	two.showmove();
	Move three = one.add(two);
	three.showmove();
}