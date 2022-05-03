#include <ofMain.h>
#include "dataBusMessage.h"

#pragma once
class dataBusReceiver
{
private:
	list<dataBusMessage> messages;

public:
	dataBusReceiver();
	~dataBusReceiver();

	void receieve(dataBusMessage msg);
	pair<string, string> getMessage();
};

