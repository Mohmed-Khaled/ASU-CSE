#include <iostream>
#include "Person.h"
using namespace std;

void main()
{
	Person one;
	Person two("Smythecraft");
	Person three("Dimwiddy", "Sam");
	one.show();
	one.formalShow();
	two.show();
	two.formalShow();
	three.show();
	three.formalShow();
}
