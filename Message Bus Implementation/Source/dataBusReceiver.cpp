#include "dataBusReceiver.h"

dataBusReceiver::dataBusReceiver()
{
}

dataBusReceiver::~dataBusReceiver()
{
}

void dataBusReceiver::receieve(dataBusMessage msg)
{
	messages.push_back(msg);
	//cout << "DBUS RECEIVER: Received message from dataBus." << endl;
}

pair<string, string> dataBusReceiver::getMessage()
{
	if (messages.size() == 0)
		return pair<string, string>("null","null");
	else
	{
		pair<string, string> ret;
		ret.first = messages.front().getMessage();
		ret.second = messages.front().getAuthor();
		messages.pop_front(); 
		return ret;		
	}
}
