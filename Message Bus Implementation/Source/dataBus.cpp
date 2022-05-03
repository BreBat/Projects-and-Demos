#include "dataBus.h"

dataBus::dataBus()
{
}

dataBus::~dataBus()
{
}

//Returns -1 if subscriber not found, else returns index
int dataBus::findSubscriber(dataBusReceiver* sub)
{
	for (int i = 0; i < subscribers.size(); i++)
	{
		if (subscribers.at(i) == sub) return i;
	}
	return -1;
}

void dataBus::publish(string msg, string aut)
{
	dataBusMessage message(msg, aut);
	for (int i = 0; i < subscribers.size(); i++)
	{
		subscribers.at(i)->receieve(message);
	}

	cout << "DATABUS: Sent message \"" << msg << "\" from " << aut << " to " << subscribers.size() << " objects." << endl;
}

void dataBus::subscribe(dataBusReceiver* sub)
{
	if (findSubscriber(sub) == -1)
	{
		subscribers.push_back(sub);
		cout << "DATABUS: Gained subscriber (" << subscribers.size() << " subs total)" << endl;
	}
	else
	{
		//do nothing
	}
}

void dataBus::unsubscribe(dataBusReceiver* sub)
{
	int presence = findSubscriber(sub);
	if (presence == -1)
	{
		//do nothing
	}
	else
	{
		subscribers.erase(subscribers.cbegin() + presence);
		cout << "DATABUS: Lost subscriber (" << subscribers.size() << " subs total)" << endl;
	}
}