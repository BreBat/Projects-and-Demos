#include "dataBusMessage.h"

dataBusMessage::dataBusMessage(string msg, string aut)
{
	message = msg;
	author = aut;
}

dataBusMessage::~dataBusMessage()
{
}

string dataBusMessage::getMessage() { return message; }
string dataBusMessage::getAuthor() { return author; }