#include <ofMain.h>

#pragma once
class dataBusMessage
{
private:
	string message;
	string author;

public:
	dataBusMessage(string msg, string aut);
	~dataBusMessage();

	string getMessage();
	string getAuthor();
};

