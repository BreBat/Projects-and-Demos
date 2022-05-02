#include "dataBusSender.h"

dataBusSender::dataBusSender(string name)
{
	authorName = name;
	carrier = NULL;
}

dataBusSender::~dataBusSender()
{
}

void dataBusSender::setCarrier(dataBus* bus)
{
	if (bus != NULL)
	{
		carrier = bus;
	}
	else
	{
		cout << "DATABUS SENDER rejected null carrier assignment!" << endl;
	}
}

void dataBusSender::send(string message)
{
	if (carrier != NULL)
	{
		carrier->publish(message, authorName);
	}
	else
	{
		cout << "DATABUS SENDER has no carrier!" << endl;
	}
}