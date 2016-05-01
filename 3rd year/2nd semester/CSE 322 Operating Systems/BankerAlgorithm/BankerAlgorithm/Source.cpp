#include <iostream>
#include <vector>

using namespace std;

int n;
int m;
vector<int> available;
vector<vector<int>> max;
vector<vector<int>> allocation;
vector<vector<int>> need;
vector<int> sequence;

void takeInput();
void printMatrcies();
bool checkSafety();
bool requestResource();
void testInputs() 
{
	n = 5;
	m = 4;
	available = { 1,5,2,0 };
	max = 
	{
		{0,0,1,2},
		{1,7,5,0},
		{2,3,5,6},
		{0,6,5,2},
		{0,6,5,6}
	};
	allocation =
	{
		{ 0,0,1,2 },
		{ 1,0,0,0 },
		{ 1,3,5,4 },
		{ 0,6,3,2 },
		{ 0,0,1,4 }
	};
	need =
	{
		{ 0,0,0,0 },
		{ 0,7,5,0 },
		{ 1,0,0,2 },
		{ 0,0,2,0 },
		{ 0,6,4,2 }
	};
}

int main(void)
{
	try 
	{
		takeInput();
		//testInputs();
	}
	catch(char * e)
	{
		cout << "ERROR: " << e << endl;
		return 1;
	}
	printMatrcies();
	int answer;
question: cout << "Do you want to make a resource request ? 1.yes 2.no : ";
	cin >> answer;
	if (answer == 1)
	{
		try 
		{
			if(requestResource())
			{
				cout << "Request granted...Allocation after request =>" << endl;
				printMatrcies();
			}
			else 
			{
				cout << "Request cannot be granted" << endl;
			}

		}
		catch (char * e) 
		{
			cout << "ERROR: " << e << endl;
			return 2;
		}
	}
	else if (answer != 2) 
	{
		cout << "ERROR: Invalid Answer" << endl;
		goto question;
	}
	cout << "######Banker Algorithm######" << endl;
	if (checkSafety()) 
	{
		cout << "Safe Sequence => ";
		for (int i = 0; i < n; i++)
		{
			cout << "P" << sequence[i] << " ";
		}
		cout << endl;
	}
	else
	{
		cout << "Unsafe System" << endl;
	}
	return 0;
}

void takeInput()
{
	cout << "Enter number of processes: ";
	cin >> n;
	if (n <= 0) throw "Number of processes must be positive integer.";
	cout << "Enter number of resources: ";
	cin >> m;
	if (m <= 0) throw "Number of resources must be positive integer.";
	available = vector<int>(m);
	max = vector<vector<int>>(n,vector<int>(m));
	allocation = vector<vector<int>>(n, vector<int>(m));
	need = vector<vector<int>>(n, vector<int>(m));
	cout << "######Enter Available Vector######" << endl;
	for (int i = 0; i < m; i++) 
	{
		cout << "Enter available for R" << i << ": ";
		cin >> available[i];
		if (available[i] < 0) throw "Available resources must be positive integer or zero.";
	}
	cout << "######Enter Max Matrix######" << endl;
	for (int i = 0; i < n; i++)
	{
		cout << "Enter max used in P" << i << "=>" << endl;
		for (int j = 0; j < m; j++)
		{
			cout << "Enter max R" << j << " used in P" << i << ": ";
			cin >> max[i][j];
			if (max[i][j] < 0) throw "Max resources must be positive integer or zero.";
		}
	}
	cout << "######Enter Allocation Matrix######" << endl;
	for (int i = 0; i < n; i++)
	{
		cout << "Enter allocated for P" << i << "=>" << endl;
		for (int j = 0; j < m; j++)
		{
			cout << "Enter allocated R" << j << " for P" << i << ": ";
			cin >> allocation[i][j];
			if (allocation[i][j] < 0) throw "Allocated resources must be positive integer or zero.";
			if (allocation[i][j] > max[i][j]) 
				throw "Allocated resources must be less than or equal max resources.";
			need[i][j] = max[i][j] - allocation[i][j];
		}
	}
}

void printMatrcies()
{
	cout << "######Available Vector######" << endl;
	for (int i = 0; i < m; i++)
	{
		cout << available[i] << " ";
	}
	cout << endl;
	cout << "######Max Matrix######" << endl;
	for (int i = 0; i < n; i++)
	{
		for (int j = 0; j < m; j++)
		{
			cout << max[i][j] << " ";
		}
		cout << endl;
	}
	cout << "######Allocation Matrix######" << endl;
	for (int i = 0; i < n; i++)
	{
		for (int j = 0; j < m; j++)
		{
			cout << allocation[i][j] << " ";
		}
		cout << endl;
	}
	cout << "######Need Matrix######" << endl;
	for (int i = 0; i < n; i++)
	{
		for (int j = 0; j < m; j++)
		{
			cout << need[i][j] << " ";
		}
		cout << endl;
	}
}

bool checkSafety()
{
	vector<int> work = available;
	vector<bool> finish(n, false);
	bool safe = true;
	bool checkFlag;
	do
	{
		checkFlag = false;
		for (int i = 0; i < n; i++)
		{
			if (!finish[i])
			{
				bool flag = true;
				for (int j = 0; j < m; j++)
				{
					if (need[i][j] > work[j])
					{
						flag = false;
						break;
					}
				}
				if (!flag) continue;
				for (int j = 0; j < m; j++)
				{
					work[j] = work[j] + allocation[i][j];
				}
				finish[i] = true;
				sequence.push_back(i);
				for (int j = 0; j < n; j++)
				{
					if (!finish[j])
					{
						checkFlag = true;
						break;
					}
				}
			}
		}
	} while (checkFlag);
	for (int i = 0; i < n; i++)
	{
		if (!finish[i])
		{
			safe = false;
			break;
		}
	}
	if (sequence.size() != n) safe = false;
	return safe;
}

bool requestResource()
{
	vector<int> request(m);
	int pid;
	cout << "######Enter Request Vector######" << endl;
	cout << "Enter requesting process index (from 0 to " << n - 1 <<"): ";
	cin >> pid;
	for (int i = 0;i < m;i++)
	{
		cout << "Enter request for R" << i << ": ";
		cin >> request[i];
		if (request[i] < 0) throw "Request resources must be positive integer or zero.";
	}
	for (int i = 0;i < m;i++)
	{
		if (request[i] > need[pid][i])
			throw "Process has exceeded its maximum claim.";
		if (request[i] > available[i])
			return false;
	}
	for (int i = 0; i < m; i++)
	{
		available[i] -= request[i];
		allocation[pid][i] += request[i];
		need[pid][i] -= request[i];
	}
	if (checkSafety) return true;
	else
	{
		for (int i = 0; i < m; i++)
		{
			available[i] += request[i];
			allocation[pid][i] -= request[i];
			need[pid][i] += request[i];
		}
	}
}