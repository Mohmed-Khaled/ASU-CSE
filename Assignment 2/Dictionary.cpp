#include <iostream>
#include <string>
#include "dbc.h"
#include <regex>

using namespace std;

class Dictionary 
{
private:
	string names[100];
	string emails[100];
	int size;
	bool checkName(string name)
	{

		regex nameRegex("[A-Za-z][A-Za-z', ]*");
		if (regex_match(name, nameRegex))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	bool checkEmail(string email)
	{
		if (email == "")
		{
			return false;
		}
		int atPos = -1;
		int dotPos = -1;
		for (int i = 0, len = email.length(); i < len;i++)
		{
			if (email[i] == '@')
			{
				atPos = i;
			}
			else if (email[i] == '.')
			{
				dotPos = i;
			}
		}
		if (atPos == -1 || dotPos == -1)
		{
			return false;
		}
		if (atPos < 1 || dotPos < atPos + 2 || dotPos == email.length() - 1)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	bool nameExist(string name)
	{
		for (int i = 0; i<size; i++)
		{
			if (names[i] == name)
			{
				return true;
			}
		}
		return false;
	}
public:
	// default constructor
	Dictionary()
	{
		size =0;
	}
	// commands
	void Add(string name,string email)
	{
		int entrySize;
		INVARIANT0(size < 100);
		REQUIRE0((entrySize = getSize()) || 1);
		REQUIRE0(checkName(name));
		REQUIRE0(checkEmail(email));
		//body start
		names[size] = name;
		emails[size] = email;
		size++;
		//body end
		ENSURE0(nameExist(name));
		ENSURE0(getEmail(name) == email);
		ENSURE0(getSize() == entrySize + 1);
		INVARIANT0(size < 100);
	}

	void Remove(string name)
	{
		int entrySize;
		INVARIANT0(size < 100);
		REQUIRE0((entrySize = getSize()) || 1);
		REQUIRE0(checkName(name));
		//body start
		// a simple example on removing array of numbers
		// if the array is  {1,5,3,4,2,6,7,8,9,10}
		// and its size is 10 elements
		// and i want to remove entry "2"
		// i'll find its index first through the following code segment
		int indextoberemoved;

		for(int i =0;i<size;i++)
		{
			if(names[i] == name)
			{
				indextoberemoved = i;
				break;
			}
		}
		// then i'll move all elements after that index backword one step
		// and with decrementing the size, this is equivalent to removing that element
		size--;
		for(int i = indextoberemoved;i<size;i++)
		{
			names[i] = names[i+1];
			emails[i] = emails[i+1];
		}
		//body end
		ENSURE0(!nameExist(name));
		ENSURE0(getSize() == entrySize - 1);
		INVARIANT0(size < 100);
	}
	void printentries()
	{
		for(int i = 0;i < size;i++)
		{
			cout << "Entry #" << i+1 << ":" << endl << names[i] << ": " << emails[i] <<endl;
		}
	}
	int getSize() 
	{ 
		return size; 
	}
	string getEmail(string name) 
	{ 
		int index = -1;

		for (int i = 0; i<size; i++)
		{
			if (names[i] == name)
			{
				index = i;
				break;
			}
		}
		if (index != -1)
		{
			return emails[index];
		}
	}
};



void main()
{
	Dictionary x;
	x.Add("omar","omar@live.com");
	x.Add("hassan","hassan@live.com");
	cout<<"Before Deleting Hassan"<<endl;
	x.printentries();
	x.Remove("hassan");
	cout<<"After Deleting Hassan"<<endl;
	x.printentries();
}